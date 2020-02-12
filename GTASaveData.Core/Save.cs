using GTASaveData.Serialization;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace GTASaveData
{
    // TODO: add ability to add/remove blocks after main game data (make use of padding space)?

    /// <summary>
    /// Represents a saved <i>Grand Theft Auto</i> game.
    /// </summary>
    public abstract class GrandTheftAutoSave : SerializableObject
    {
        public static T Load<T>(string path) where T : GrandTheftAutoSave, new()
        {
            return Load<T>(path, GetFileFormat<T>(path));
        }

        public static T Load<T>(string path, FileFormat format) where T : GrandTheftAutoSave, new()
        {
            if (path == null)
            {
                return null;
            }

            return Load<T>(File.ReadAllBytes(path), format);
        }

        public static T Load<T>(byte[] data, FileFormat format) where T : GrandTheftAutoSave, new()
        {
            if (data == null || format == null)
            {
                return null;
            }

            Debug.WriteLine("Loading {0} {1}...", format.DisplayName, typeof(T).Name);

            using (MemoryStream m = new MemoryStream(data))
            {
                using (Serializer s = new Serializer(m))
                {
                    return s.ReadObject<T>(format);
                }
            }
        }
        public static FileFormat GetFileFormat<T>(string path) where T : GrandTheftAutoSave, new()
        {
            return new T().DetectFileFormat(File.ReadAllBytes(path));
        }

        protected SerializableObject[] m_blocks;

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
            set { m_fileFormat = value ?? FileFormat.None; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the name of this save.
        /// </summary>
        public abstract string Name { get; }

        protected abstract int MaxBlockSize { get; }

        protected abstract int BlockCount { get; }

        protected abstract int SectionCount { get; }

        protected abstract int SimpleVarsSize { get; }

        protected GrandTheftAutoSave()
        {
            m_fileFormat = FileFormat.None;
            m_paddingMode = PaddingMode.Zeros;
            m_paddingSequence = null;
            m_blocks = new SerializableObject[BlockCount];
        }

        /// <summary>
        /// Writes all data to a file on disk according to the current <see cref="FileFormat"/>.
        /// </summary>
        /// <param name="path">The path to the file to write. The file will be overwritten if it exists.</param>
        public void Save(string path)
        {
            byte[] data = Serialize(this);
            File.WriteAllBytes(path, data);
        }

        protected T Deserialize<T>(byte[] buffer)
        {
            return Serializer.Deserialize<T>(buffer, m_fileFormat);
        }

        protected byte[] Serialize<T>(T obj)
        {
            return Serializer.Serialize(obj, m_fileFormat, m_paddingMode, m_paddingSequence);
        }

        protected Serializer CreateSerializer(Stream baseStream)
        {
            return new Serializer(baseStream, m_paddingMode, m_paddingSequence);
        }

        protected byte[] CreateBlock<T>(string tag, T obj)
        {
            return CreateBlock(tag, Serialize(obj));
        }

        protected abstract FileFormat DetectFileFormat(byte[] data);

        protected abstract byte[] ReadBlock(Serializer r, string tag);

        protected abstract byte[] CreateBlock(string tag, byte[] data);
        
        protected abstract byte[] CreatePadding(int length);

        protected abstract void LoadSection(int index, byte[] data);

        protected abstract byte[] SaveSection(int index);

        public override string ToString()
        {
            return string.Format("{0}: [ Name = {1}, FileFormat = {2} ]", GetType().Name, Name, m_fileFormat);
        }
    }
}
