using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace GTASaveData
{
    /// <summary>
    /// Contains the saved game state of a <i>Grand Theft Auto</i> game.
    /// </summary>
    public abstract class SaveData : SaveDataObject
    {
        private FileFormat m_fileFormat;
        private PaddingType m_paddingType;
        private byte[] m_paddingBytes;

        public FileFormat FileFormat
        {
            get { return m_fileFormat; }
            set { m_fileFormat = value; OnPropertyChanged(); }
        }

        public PaddingType PaddingType
        {
            get { return m_paddingType; }
            set { m_paddingType = value; OnPropertyChanged(); }
        }

        public byte[] PaddingBytes
        {
            get { return m_paddingBytes; }
            set { m_paddingBytes = value; OnPropertyChanged(); }
        }

        public abstract string Name { get; set; }
        public abstract DateTime TimeStamp { get; set; }

        public SaveData()
        { }

        protected int Load(byte[] data)
        {
            OnLoading();
            int bytesRead = Serializer.Read(this, data, FileFormat);
            OnLoad();

            Debug.WriteLine($"Read {bytesRead} bytes for load.");
            return bytesRead;
        }

        protected int Load(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            int bytesRead = Load(data);
            OnFileLoad(path);

            return bytesRead;
        }

        public int Save(string path)
        {
            int bytesWritten = Save(out byte[] data);
            File.WriteAllBytes(path, data);
            OnFileSave(path);

            return bytesWritten;
        }

        public int Save(out byte[] data)
        {
            OnSaving();
            int bytesWritten = Serializer.Write(this, FileFormat, out data);
            OnSave();

            Debug.WriteLine($"Wrote {bytesWritten} bytes for save.");
            return bytesWritten;
        }

        protected virtual void OnLoading()
        {
            TimeStamp = DateTime.Now;
        }

        protected virtual void OnLoad()
        { }

        protected virtual void OnFileLoad(string path)
        {
            Name = Path.GetFileNameWithoutExtension(path);
            TimeStamp = File.GetLastWriteTime(path);
        }

        protected virtual void OnSaving()
        { }

        protected virtual void OnSave()
        { }

        protected virtual void OnFileSave(string path)
        { }

        protected abstract void LoadAllData(StreamBuffer file);
        protected abstract void SaveAllData(StreamBuffer file);
        protected abstract bool DetectFileFormat(byte[] data, out FileFormat fmt);

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            FileFormat = fmt;
            LoadAllData(buf);
        }
        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            FileFormat = fmt;
            SaveAllData(buf);
        }

        public static bool GetFileFormat<T>(byte[] data, out FileFormat fmt) where T : SaveData, new()
        {
            return new T().DetectFileFormat(data, out fmt);
        }

        public static bool GetFileFormat<T>(string path, out FileFormat fmt) where T : SaveData, new()
        {
            byte[] data = File.ReadAllBytes(path);
            return GetFileFormat<T>(data, out fmt);
        }

        public static T Load<T>(byte[] data) where T : SaveData, new()
        {
            if (!GetFileFormat<T>(data, out FileFormat fmt)) return null;
            return Load<T>(data, fmt);
        }

        public static T Load<T>(byte[] data, FileFormat fmt) where T : SaveData, new()
        {
            T obj = new T() { FileFormat = fmt };
            obj.Load(data);

            return obj;
        }

        public static T Load<T>(string path) where T : SaveData, new()
        {
            if (!GetFileFormat<T>(path, out FileFormat fmt)) return null;
            return Load<T>(path, fmt);
        }

        public static T Load<T>(string path, FileFormat fmt) where T : SaveData, new()
        {
            T obj = new T() { FileFormat = fmt };
            obj.Load(path);

            return obj;
        }

        public override string ToString()
        {
            var now = TimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
            int size = GetSize(FileFormat);

            return $"{GetType().Name}: {{ " +
                $"Name = {Name}; " +
                $"TimeStamp = {now}; " +
                $"FileFormat = {FileFormat}; " +
                $"Size = {size}; " +
                $"Size = {size}; " +
                $"}}";
        }
    }
}
