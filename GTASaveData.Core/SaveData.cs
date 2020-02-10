using GTASaveData.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// Represents a saved Grand Theft Auto game.
    /// </summary>
    public abstract class SaveData : Chunk
    {
        private PaddingMode m_paddingMode;
        private byte[] m_paddingSequence;
        private FileFormat m_fileFormat;

        /// <summary>
        /// Gets or sets the <see cref="PaddingMode"/> used when storing
        /// this file's data to disk.
        /// </summary>
        public PaddingMode FilePaddingMode
        {
            get { return m_paddingMode; }
            set { m_paddingMode = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the padding sequence used when storing this
        /// file's data to disk. Note that <see cref="FilePaddingMode"/>
        /// must be set to <see cref="PaddingMode.Sequence"/> for this
        /// property to have an effect.
        /// </summary>
        public byte[] FilePaddingSequence
        {
            get { return m_paddingSequence; }
            set { m_paddingSequence = value; OnPropertyChanged(); }
        }

        // TODO: elaborate on conversion limitations
        /// <summary>
        /// Gets or sets the current <see cref="Serialization.FileFormat"/>. This controls
        /// which game system a save file is formatted for. NOTE: this will not necessarily
        /// allow you to convert saves from one platform to another.
        /// </summary>
        [JsonIgnore]
        public FileFormat FileFormat
        {
            get { return m_fileFormat; }
            set { m_fileFormat = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the name of this save.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets a collection containing each section of the save.
        /// </summary>
        public abstract IReadOnlyList<Chunk> Blocks { get; }

        /// <summary>
        /// Gets a dictionary associating file formats with their maximum-allowed block sizes.
        /// </summary>
        protected abstract Dictionary<FileFormat, int> MaxBlockSize { get; }

        /// <summary>
        /// Gets a dictionary associating file formats with their SimpleVars sizes.
        /// </summary>
        protected abstract Dictionary<FileFormat, int> SimpleVarsSize { get; }

        /// <summary>
        /// Creates a new <see cref="Serializer"/> object.
        /// </summary>
        protected SaveData()
        {
            m_paddingMode = PaddingMode.Zeros;
            m_paddingSequence = null;
            m_fileFormat = null;
        }

        /// <summary>
        /// Gets the number of blocks in a save file.
        /// </summary>
        /// <param name="path">The path to the save file.</param>
        /// <returns>The number of data blocks, should be nonzero if the file is valid.</returns>
        public static int GetBlockCount(string path)
        {
            return GetBlockCount(File.ReadAllBytes(path));
        }

        /// <summary>
        /// Gets the number of blocks in a save file.
        /// </summary>
        /// <param name="data">The save file data.</param>
        /// <returns>The number of data blocks, should be nonzero if the file is valid.</returns>
        public static int GetBlockCount(byte[] data)
        {
            if (data == null)
            {
                return -1;
            }

            using (MemoryStream m = new MemoryStream(data))
            {
                using (Serializer s = new Serializer(m))
                {
                    int count = 0;
                    do
                    {
                        int blockSize = s.ReadInt32();
                        if (blockSize < m.Length - m.Position)
                        {
                            Debug.WriteLine("Block size: {0}", blockSize);
                            s.Skip(blockSize);
                            count++;
                        }
                    } while (m.Position < m.Length);

                    return count;
                }
            }
        }

        public static FileFormat GetFileFormat<T>(string path) where T : SaveData, new()
        {
            return new T().DetectFileFormat(path);
        }

        public static T Load<T>(string path) where T : SaveData, new()
        {
            return Load<T>(path, GetFileFormat<T>(path));
        }

        public static T Load<T>(string path, FileFormat format) where T : SaveData, new()
        {
            if (path == null)
            {
                return null;
            }

            return Load<T>(File.ReadAllBytes(path), format);
        }

        public static T Load<T>(byte[] data, FileFormat format) where T : SaveData, new()
        {
            if (data == null || format == null)
            {
                return null;
            }

            Debug.WriteLine("Loading {0}, format: {1}", typeof(T).Name, format.DisplayName);
            Debug.WriteLine("Save data has {0} blocks.", GetBlockCount(data));

            using (MemoryStream m = new MemoryStream(data))
            {
                using (Serializer s = new Serializer(m))
                {
                    return s.ReadChunk<T>(format);
                }
            }
        }

        /// <summary>
        /// Writes all data to a file on disk according to the current <see cref="FileFormat"/>.
        /// </summary>
        /// <param name="path">The path to the file to write. The file will be overwritten if it exists.</param>
        public void Store(string path)
        {
            byte[] data = Serialize(this, m_fileFormat);
            File.WriteAllBytes(path, data);
        }

        protected T Deserialize<T>(byte[] buffer, FileFormat format = null)
        {
            return Serializer.Deserialize<T>(buffer, format);
        }

        protected byte[] Serialize<T>(T obj, FileFormat format = null)
        {
            return Serializer.Serialize(obj, format, m_paddingMode, m_paddingSequence);
        }

        protected Serializer CreateSerializer(Stream baseStream)
        {
            return new Serializer(baseStream, m_paddingMode, m_paddingSequence);
        }
 
        protected virtual byte[] CreateBlock(params ByteBuffer[] chunks)
        {
            return CreateBlock(null, chunks);
        }

        protected virtual byte[] CreateBlock(string tag, params ByteBuffer[] chunks)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer s = CreateSerializer(m))
                {
                    WriteBlock(s, tag, chunks);
                }

                return m.ToArray();
            }
        }

        protected virtual byte[] CreatePaddingBlock(int length)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer s = CreateSerializer(m))
                {
                    s.WritePadding(length);
                }

                return CreateBlock(m.ToArray());
            }
        }

        protected virtual ByteBuffer ReadBlock(Serializer s, string tag = null)
        {
            int length = s.ReadInt32();
            Debug.WriteLineIf(length > MaxBlockSize[FileFormat], "Maximum allowed block size exceeded!");

            if (tag != null)
            {
                string str = s.ReadString(4);
                int dataLength = s.ReadInt32();
                Debug.Assert(str == tag, "Invalid block tag!", "Expected: {0}, Actual: {1}", tag, str);
                Debug.Assert(dataLength == length - 8);
                length = dataLength;
            }

            byte[] data = s.ReadBytes(length);
            s.Align();

            return data;
        }

        protected virtual int WriteBlock(Serializer s, string tag, params ByteBuffer[] chunks)
        {
            long mark = s.BaseStream.Position;

            int payloadSize = chunks.Sum(x => x.Length);
            int totalSize = (tag != null) ? (payloadSize + 8) : payloadSize;
            Debug.WriteLineIf(totalSize > MaxBlockSize[FileFormat], "Maximum allowed block size exceeded!");

            if (tag != null)
            {
                s.Write(payloadSize + 8);
                s.Write(tag, 4);
            }
            s.Write(payloadSize);
            foreach (ByteBuffer chunk in chunks)
            {
                s.Write((byte[]) chunk);
            }
            s.Align();

            return (int) (s.BaseStream.Position - mark);
        }

        protected abstract FileFormat DetectFileFormat(string path);

        public override string ToString()
        {
            return string.Format("{0}: [ Name = {1}, FileFormat = {2} ]", GetType().Name, Name, FileFormat);
        }
    }
}
