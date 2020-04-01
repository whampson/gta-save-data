using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GTASaveData
{
    /// <summary>
    /// Represents a saved <i>Grand Theft Auto</i> game.
    /// </summary>
    public abstract class SaveFile : SaveDataObject, IGTASaveFile
    {
        private const int MaxBufferCapacity = 0x20000;
        private static readonly byte[] DefaultPadding = new byte[1] { 0 };

        private SaveFileFormat m_fileFormat;
        private PaddingType m_padding;
        private byte[] m_paddingBytes;
        private bool m_blockSizeChecksEnabled;
        private bool m_disposed;

        protected DataBuffer WorkBuff { get; }
        protected uint CheckSum { get; set; }
        protected abstract int BufferSize { get; }

        [JsonIgnore]
        public SaveFileFormat FileFormat
        {
            get { return m_fileFormat; }
            set { m_fileFormat = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public PaddingType Padding
        {
            get { return m_padding; }
            set { m_padding = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public byte[] PaddingBytes
        {
            get { return m_paddingBytes; }
            set { m_paddingBytes = value ?? DefaultPadding; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public bool BlockSizeChecksEnabled
        {
            get { return m_blockSizeChecksEnabled; }
            set { m_blockSizeChecksEnabled = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public abstract IReadOnlyList<SaveDataObject> Blocks { get; }

        [JsonIgnore]
        public abstract string Name { get; set;  }

        [JsonIgnore]
        public abstract DateTime TimeLastSaved { get; set; }

        public SaveFile()
        {
            m_disposed = false;
            WorkBuff = new DataBuffer(new byte[MaxBufferCapacity]);
            CheckSum = 0;
            FileFormat = SaveFileFormat.Default;
            Padding = PaddingType.Default;
            PaddingBytes = DefaultPadding;
            
// Hmmm...
#if !DEBUG
            BlockSizeChecks = true;
#endif
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                WorkBuff.Dispose();
                m_disposed = true;
            }
        }

        public static T Load<T>(string path)
            where T : SaveFile, new()
        {
            bool valid = GetFileFormat<T>(path, out SaveFileFormat fmt);
            if (!valid)
            {
                return null;
            }

            return Load<T>(path, fmt);
        }

        public static T Load<T>(string path, SaveFileFormat fmt)
            where T : SaveFile, new()
        {
            T obj = new T() { FileFormat = fmt };
            obj.Load(path);

            return obj;
        }

        public void Load(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            int bytesRead = Serializer.Read(this, data, FileFormat);

            Debug.WriteLine("Read {0} bytes from disk.", bytesRead);
        }

        public void Save(string path)
        {
            int bytesWritten = Serializer.Write(this, FileFormat, out byte[] data);
            File.WriteAllBytes(path, data);

            Debug.WriteLine("Wrote {0} bytes to disk.", bytesWritten);
        }

        public static bool GetFileFormat<T>(string path, out SaveFileFormat fmt)
            where T : SaveFile, new()
        {
            byte[] data = File.ReadAllBytes(path);
            return new T().DetectFileFormat(data, out fmt);
        }

        protected byte[] GenerateSpecialPadding(int length)
        {
            switch (Padding)
            {
                case PaddingType.Pattern:
                {
                    byte[] pad = new byte[length];
                    byte[] seq = PaddingBytes;

                    for (int i = 0; i < length; i++)
                    {
                        pad[i] = seq[i % seq.Length];
                    }

                    return pad;
                }

                case PaddingType.Random:
                {
                    byte[] pad = new byte[length];
                    Random rand = new Random();

                    rand.NextBytes(pad);
                    return pad;
                }
            }

            throw new InvalidOperationException("Invalid padding type.");
        }

        protected SerializationException BlockSizeException(int maxSize, int actualSize)
        {
            return new SerializationException("Maximum block size exeeded. (max = {0}, actual = {1})", maxSize, actualSize);
        }

        protected abstract void LoadAllData(DataBuffer file);
        protected abstract void SaveAllData(DataBuffer file);
        protected abstract bool DetectFileFormat(byte[] data, out SaveFileFormat fmt);

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            FileFormat = fmt;
            LoadAllData(buf);
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            FileFormat = fmt;
            SaveAllData(buf);
        }

        public override string ToString()
        {
            return string.Format("{0}: {{ Name = {1}, FileFormat = {2}, TimeLastSaved = {3} }}",
                GetType().Name, Name, FileFormat, TimeLastSaved.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
