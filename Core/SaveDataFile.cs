using GTASaveData.Serialization;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GTASaveData
{
    /// <summary>
    /// Represents a saved Grand Theft Auto game.
    /// </summary>
    public abstract class SaveDataFile : SaveDataObject
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
        protected SaveDataFile()
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

        /// <summary>
        /// Creates a new <see cref="SaveDataSerializer"/> using the current
        /// <see cref="FilePaddingMode"/> and <see cref="FilePaddingSequence"/>
        /// values.
        /// </summary>
        /// <param name="baseStream">The source <see cref="Stream"/>.</param>
        /// <returns>A <see cref="SaveDataSerializer"/> instance.</returns>
        protected SaveDataSerializer CreateSerializer(Stream baseStream)
        {
            return new SaveDataSerializer(baseStream)
            {
                PaddingMode = m_paddingMode,
                PaddingSequence = m_paddingSequence
            };
        }

        /// <summary>
        /// Creates an object from the data stored in a byte array.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="buffer">The byte array containing serialized object data.</param>
        /// <param name="format">The format of the data.</param>
        /// <param name="length">The length of the data.</param>
        /// <param name="unicode">A value indicating whether to read Unicode characters.</param>
        /// <returns>An object of type <typeparamref name="T"/>.</returns>
        protected T Deserialize<T>(byte[] buffer,
            FileFormat format = null,
            int length = 0,
            bool unicode = false)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            using (SaveDataSerializer s = CreateSerializer(new MemoryStream(buffer)))
            {
                return s.GenericRead<T>(format, length, unicode);
            }
        }

        /// <summary>
        /// Creates a byte array from data in the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="format">The serialization format.</param>
        /// <param name="length">The length of the data to serialize.</param>
        /// <param name="unicode">A value indicating whether to write Unicode characters.</param>
        /// <returns>A byte array containing the serialized object data.</returns>
        protected byte[] Serialize<T>(T obj,
            FileFormat format = null,
            int length = 0,
            bool unicode = false)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            using (MemoryStream m = new MemoryStream())
            {
                using (SaveDataSerializer s = CreateSerializer(m))
                {
                    s.GenericWrite(obj, format, length, unicode);
                }

                return m.ToArray();
            }
        }
    }
}
