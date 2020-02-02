using GTASaveData.Helpers;
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
        /// file's data to disk. Note that the <see cref="FilePaddingMode"/>
        /// must be set to <see cref="PaddingMode.Sequence"/> for this
        /// property to have an effect.
        /// </summary>
        public byte[] FilePaddingSequence
        {
            get { return m_paddingSequence; }
            set { m_paddingSequence = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the current <see cref="Serialization.FileFormat"/>.
        /// </summary>
        [JsonIgnore]
        public FileFormat FileFormat
        {
            get { return m_fileFormat; }
            protected set { m_fileFormat = value; OnPropertyChanged(); }
        }

        public abstract string Name { get; }

        public abstract IReadOnlyList<Chunk> Blocks { get; }

        protected abstract Dictionary<FileFormat, int> MaxBlockSize { get; }

        protected abstract Dictionary<FileFormat, int> SimpleVarsSize { get; }

        /// <summary>
        /// Creates a new <see cref="SaveDataSerializer"/> object.
        /// </summary>
        protected SaveData()
        {
            m_paddingMode = PaddingMode.Zeros;
            m_paddingSequence = null;
            m_fileFormat = FileFormat.Unknown;
        }

        public static int GetBlockCount(string path)
        {
            return GetBlockCount(File.ReadAllBytes(path));
        }

        public static int GetBlockCount(byte[] data)
        {
            if (data == null)
            {
                return -1;
            }

            using (MemoryStream m = new MemoryStream(data))
            {
                using (SaveDataSerializer s = new SaveDataSerializer(m))
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

        public static T LoadFromFile<T>(string path) where T : SaveData, new()
        {
            return LoadFromFile<T>(path, GetFileFormat<T>(path));
        }

        public static T LoadFromFile<T>(string path, FileFormat format) where T : SaveData, new()
        {
            if (path == null || format == null)
            {
                return null;
            }

            Debug.WriteLine("Loading {0}, format {1}", typeof(T).Name, format.DisplayName);
            Debug.WriteLine("File has {0} blocks.", GetBlockCount(path));

            byte[] data = File.ReadAllBytes(path);
            using (MemoryStream m = new MemoryStream(data))
            {
                using (SaveDataSerializer s = new SaveDataSerializer(m))
                {
                    return s.ReadObject<T>(format);
                }
            }
        }

        /// <summary>
        /// Writes all data to a file on disk using the specified <see cref="Serialization.FileFormat"/>.
        /// </summary>
        /// <param name="path">The path to the file to write.</param>
        /// <param name="format">The file format to write.</param>
        public void Write(string path, FileFormat format)
        {
            byte[] data = Serialize(this, format);
            File.WriteAllBytes(path, data);
        }

        protected T Deserialize<T>(byte[] buffer, FileFormat format = null)
        {
            return SaveDataSerializer.Deserialize<T>(buffer, format);
        }

        protected byte[] Serialize<T>(T obj, FileFormat format = null)
        {
            return SaveDataSerializer.Serialize(obj, format, m_paddingMode, m_paddingSequence);
        }

        protected SaveDataSerializer CreateSerializer(Stream baseStream)
        {
            return new SaveDataSerializer(baseStream, m_paddingMode, m_paddingSequence);
        }
 
        protected virtual byte[] CreateBlock(params ByteBuffer[] chunks)
        {
            return CreateBlock(null, chunks);
        }

        protected virtual byte[] CreateBlock(string tag, params ByteBuffer[] chunks)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (SaveDataSerializer s = CreateSerializer(m))
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
                using (SaveDataSerializer s = CreateSerializer(m))
                {
                    s.WritePadding(length);
                }

                return CreateBlock(m.ToArray());
            }
        }

        protected virtual ByteBuffer ReadBlock(SaveDataSerializer s, string tag = null)
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

        protected virtual int WriteBlock(SaveDataSerializer s, string tag, params ByteBuffer[] chunks)
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
