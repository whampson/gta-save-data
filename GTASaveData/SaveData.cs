using GTASaveData.Helpers;
using GTASaveData.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GTASaveData
{
    /// <summary>
    /// Represents a saved Grand Theft Auto game.
    /// </summary>
    public abstract class SaveData : SaveDataObject
    {
        private PaddingMode m_paddingMode;
        private byte[] m_paddingSequence;
        private FileFormat m_currentFormat;

        /// <summary>
        /// Gets or sets the <see cref="PaddingMode"/> used when storing
        /// this file's data to disk.
        /// </summary>
        [JsonIgnore]
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
        [JsonIgnore]
        public byte[] FilePaddingSequence
        {
            get { return m_paddingSequence; }
            set { m_paddingSequence = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the current <see cref="FileFormat"/> that this
        /// object's data is being written for.
        /// </summary>
        [JsonIgnore]
        protected FileFormat CurrentFormat
        {
            get { return m_currentFormat; }
            set { m_currentFormat = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Creates a new <see cref="SaveDataSerializer"/> object.
        /// </summary>
        protected SaveData()
        {
            m_paddingMode = PaddingMode.Zeros;
            m_paddingSequence = null;
            m_currentFormat = FileFormat.Unknown;
        }

        /// <summary>
        /// Writes all data to a file on disk using the specified <see cref="FileFormat"/>.
        /// </summary>
        /// <param name="path">The path to the file to write.</param>
        /// <param name="format">The file format to write.</param>
        public void Store(string path, FileFormat format)
        {
            byte[] data = Serialize(this, format);
            File.WriteAllBytes(path, data);
        }

        public abstract IList<SaveDataObject> GetAllBlocks();

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

        public static int CountBlocks(string path)
        {
            return CountBlocks(File.ReadAllBytes(path));
        }

        public static int CountBlocks(byte[] data)
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
    }
}
