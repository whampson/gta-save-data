using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;

namespace GTASaveData
{
    /// <summary>
    /// Contains the saved state of a <i>Grand Theft Auto</i> game.
    /// </summary>
    public abstract class SaveData : SaveDataObject
    {
        private FileFormat m_fileFormat;
        private PaddingType m_paddingType;
        private byte[] m_paddingBytes;

        /// <summary>
        /// Gets or sets the file format, which defines the order
        /// and manner in which data is read and written.
        /// </summary>
        public FileFormat FileFormat
        {
            get { return m_fileFormat; }
            set { m_fileFormat = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the padding scheme.
        /// </summary>
        public PaddingType PaddingType
        {
            get { return m_paddingType; }
            set { m_paddingType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the bytes to use for pattern-based padding.
        /// </summary>
        public byte[] PaddingBytes
        {
            get { return m_paddingBytes; }
            set { m_paddingBytes = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the embedded save name.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Gets or sets the embedded save time stamp.
        /// </summary>
        public abstract DateTime TimeStamp { get; set; }

        /// <summary>
        /// Creates a new <see cref="SaveData"/> instance.
        /// </summary>
        public SaveData()
        { }

        /// <summary>
        /// Attempts to determine the file format of the specified save data.
        /// </summary>
        /// <typeparam name="T">The <see cref="SaveData"/> type.</typeparam>
        /// <param name="data">The data to parse.</param>
        /// <param name="fmt">The detected file format.</param>
        /// <returns>A value indicating whether file format detection was successful.</returns>
        public static bool GetFileFormat<T>(byte[] data, out FileFormat fmt) where T : SaveData, new()
        {
            return new T().DetectFileFormat(data, out fmt);
        }

        /// <summary>
        /// Attempts to determine the file format of the specified save data.
        /// </summary>
        /// <typeparam name="T">The <see cref="SaveData"/> type.</typeparam>
        /// <param name="path">The path to the file to parse.</param>
        /// <param name="fmt">The detected file format.</param>
        /// <returns>A value indicating whether file format detection was successful.</returns>
        public static bool GetFileFormat<T>(string path, out FileFormat fmt) where T : SaveData, new()
        {
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                if (info.Length > 0x40000)
                {
                    goto Fail;
                }

                byte[] data = File.ReadAllBytes(path);
                return GetFileFormat<T>(data, out fmt);
            }

        Fail:
            fmt = FileFormat.Default;
            return false;
        }

        /// <summary>
        /// Creates a <see cref="SaveData"/> object from the specified byte array
        /// using the detected file format.
        /// </summary>
        /// <typeparam name="T">The <see cref="SaveData"/> type to create.</typeparam>
        /// <param name="data">The data to deserialize.</param>
        /// <returns>A <see cref="SaveData"/> object containing the deserialized data.</returns>
        public static T Load<T>(byte[] data) where T : SaveData, new()
        {
            return Load<T>(data, FileFormat.Default);
        }

        /// <summary>
        /// Creates a <see cref="SaveData"/> object from the specified file
        /// using the detected file format.
        /// </summary>
        /// <typeparam name="T">The <see cref="SaveData"/> type to create.</typeparam>
        /// <param name="path">The path to the file to deserialize.</param>
        /// <returns>A <see cref="SaveData"/> object containing the deserialized data.</returns>
        public static T Load<T>(string path) where T : SaveData, new()
        {
            return Load<T>(path, FileFormat.Default); ;
        }

        /// <summary>
        /// Creates a <see cref="SaveData"/> object from the specified byte array
        /// using the specified file format.
        /// </summary>
        /// <typeparam name="T">The <see cref="SaveData"/> type to create.</typeparam>
        /// <param name="data">The data to deserialize.</param>
        /// <param name="fmt">The file format to use for deserialization.</param>
        /// <returns>A <see cref="SaveData"/> object containing the deserialized data.</returns>
        public static T Load<T>(byte[] data, FileFormat fmt) where T : SaveData, new()
        {
            if (fmt == FileFormat.Default)
            {
                if (!GetFileFormat<T>(data, out fmt))
                {
                    return null;
                }
            }

            T obj = new T() { FileFormat = fmt };
            obj.Load(data);
            return obj;
        }

        /// <summary>
        /// Creates a <see cref="SaveData"/> object from the specified file
        /// using the specified file format.
        /// </summary>
        /// <typeparam name="T">The <see cref="SaveData"/> type to create.</typeparam>
        /// <param name="path">The data to deserialize.</param>
        /// <param name="fmt">The file format to use for deserialization.</param>
        /// <returns>A <see cref="SaveData"/> object containing the deserialized data.</returns>
        public static T Load<T>(string path, FileFormat fmt) where T : SaveData, new()
        {
            if (fmt == FileFormat.Default)
            {
                if (!GetFileFormat<T>(path, out fmt))
                {
                    return null;
                }
            }

            T obj = new T() { FileFormat = fmt };
            obj.Load(path);
            return obj;
        }

        /// <summary>
        /// Loads the object data from the buffer and
        /// calls the corresponding event handlers.
        /// </summary>
        /// <param name="data">The data to deserialize.</param>
        /// <returns>The number of bytes read.</returns>
        private int Load(byte[] data)
        {
            Name = "";
            TimeStamp = DateTime.Now;

            OnLoading();
            int bytesRead = DeserializeData(this, data);
            OnLoad();

            Debug.WriteLine($"SaveData: Read {bytesRead} bytes for load.");
            return bytesRead;
        }

        /// <summary>
        /// Loads the object data from the buffer and
        /// calls the corresponding event handlers.
        /// </summary>
        /// <param name="path">The data to deserialize.</param>
        /// <returns>The number of bytes read.</returns>
        private int Load(string path)
        {
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                if (info.Length > 0x40000)
                {
                    throw new InvalidDataException("File is too large to be a GTA save file.");
                }

                OnFileLoading(path);
                byte[] data = File.ReadAllBytes(path);
                Name = Path.GetFileNameWithoutExtension(path);
                TimeStamp = File.GetLastWriteTime(path);
                OnFileLoad(path);

                return Load(data);
            }

            throw new DirectoryNotFoundException($"The path does not exist: {path}");
        }

        /// <summary>
        /// Saves the object data to a byte buffer.
        /// </summary>
        /// <param name="data">The resulting buffer.</param>
        /// <returns>The number of bytes written.</returns>
        public int Save(out byte[] data)
        {
            OnSaving();
            int bytesWritten = SerializeData(this, out data);
            OnSave();

            Debug.WriteLine($"SaveData: Wrote {bytesWritten} bytes for save.");
            return bytesWritten;
        }

        /// <summary>
        /// Saves the object data to a file.
        /// </summary>
        /// <param name="path">The path to the file to write.</param>
        /// <returns>The number of bytes written.</returns>
        public int Save(string path)
        {
            int bytesWritten = Save(out byte[] data);

            OnFileSaving(path);
            File.WriteAllBytes(path, data);
            OnFileSave(path);

            return bytesWritten;
        }

        /// <summary>
        /// Reads in the object's data from a byte buffer using
        /// the <see cref="FileFormat"/>.
        /// </summary>
        protected int DeserializeData<T>(T obj, byte[] data,
            bool bigEndian = false) where T : SaveDataObject
        {
            return Serializer.Read(obj, data, FileFormat, bigEndian);
        }

        /// <summary>
        /// Writes out the object's data to a byte buffer using
        /// the <see cref="FileFormat"/>, <see cref="PaddingType"/>,
        /// and <see cref="PaddingBytes"/>.
        /// </summary>
        protected int SerializeData<T>(T obj, out byte[] data,
            bool bigEndian = false) where T : SaveDataObject
        {
            return Serializer.Write(obj, FileFormat, out data, bigEndian, PaddingType, PaddingBytes);
        }

        /// <summary>
        /// Event handler executed before <see cref="Load(byte[])"/> is called.
        /// </summary>
        protected virtual void OnLoading() { }

        /// <summary>
        /// Event handler executed after <see cref="Load(byte[])"/> is called.
        /// </summary>
        protected virtual void OnLoad() { }

        /// <summary>
        /// Event handler executed before <see cref="Save(out byte[])"/> is called.
        /// </summary>
        protected virtual void OnSaving() { }

        /// <summary>
        /// Event handler executed after <see cref="Save(out byte[])"/> is called.
        /// </summary>
        protected virtual void OnSave() { }

        /// <summary>
        /// Event handler executed before <see cref="Load(string)"/> is called.
        /// </summary>
        protected virtual void OnFileLoading(string path) { }

        /// <summary>
        /// Event handler executed after <see cref="Load(string)"/> is called.
        /// </summary>
        protected virtual void OnFileLoad(string path) { }

        /// <summary>
        /// Event handler executed before <see cref="Save(string)"/> is called.
        /// </summary>
        protected virtual void OnFileSaving(string path) { }

        /// <summary>
        /// Event handler executed before <see cref="Save(string)"/> is called.
        /// </summary>
        protected virtual void OnFileSave(string path) { }

        /// <summary>
        /// Loads all of this object's data from the buffer.
        /// </summary>
        /// <param name="file">The buffer to read.</param>
        protected abstract void LoadAllData(DataBuffer file);

        /// <summary>
        /// Saves all of this object's data to the buffer.
        /// </summary>
        /// <param name="file">The buffer to write.</param>
        protected abstract void SaveAllData(DataBuffer file);

        /// <summary>
        /// Parses the data and determines the file format.
        /// </summary>
        /// <param name="data">The data to parse.</param>
        /// <param name="fmt">The detected file format.</param>
        /// <returns>A value indicating whether the format detection was successful.</returns>
        protected abstract bool DetectFileFormat(byte[] data, out FileFormat fmt);

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            FileFormat = fmt;
            LoadAllData(buf);
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            FileFormat = fmt;
            SaveAllData(buf);
        }
    }
}
