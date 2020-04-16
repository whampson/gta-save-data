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
    public abstract class SaveFile : SaveDataObject, ISaveFile
    {
        private static readonly byte[] DefaultPadding = new byte[1] { 0 };

        private PaddingType m_padding;
        private byte[] m_paddingBytes;
        private DataFormat m_fileFormat;

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
        public DataFormat FileFormat
        {
            get { return m_fileFormat; }
            set { m_fileFormat = value; OnPropertyChanged(); }
        }

        public abstract string Name { get; set; }
        public abstract DateTime TimeLastSaved { get; set; }
        public abstract IReadOnlyList<SaveDataObject> Blocks { get; }

        public SaveFile()
        {
            Padding = PaddingType.Default;
            PaddingBytes = DefaultPadding;
            FileFormat = DataFormat.Default;
        }

        public static bool GetFileFormat<T>(string path, out DataFormat fmt) where T : SaveFile, new()
        {
            byte[] data = File.ReadAllBytes(path);
            return new T().DetectFileFormat(data, out fmt);
        }

        public static T Load<T>(string path) where T : SaveFile, new()
        {
            bool valid = GetFileFormat<T>(path, out DataFormat fmt);
            if (!valid)
            {
                return null;
            }

            return Load<T>(path, fmt);
        }

        public static T Load<T>(string path, DataFormat fmt) where T : SaveFile, new()
        {
            T obj = new T() { FileFormat = fmt };
            obj.Load(path);

            return obj;
        }

        public int Load(string path)
        {
            byte[] data = File.ReadAllBytes(path);

            // Defaults, may be overridden by derived class's own load code.
            Name = Path.GetFileNameWithoutExtension(path);
            TimeLastSaved = File.GetLastWriteTime(path);

            return Load(data);
        }

        public int Load(byte[] data)
        {
            int bytesRead = Serializer.Read(this, data, FileFormat);

            Debug.WriteLine("Read {0} bytes from disk.", bytesRead);
            return bytesRead;
        }

        public int Save(string path)
        {
            int bytesWritten = Save(out byte[] data);
            File.WriteAllBytes(path, data);

            Debug.WriteLine("Wrote {0} bytes to disk.", bytesWritten);
            return bytesWritten;
        }

        public int Save(out byte[] data)
        {
            return Serializer.Write(this, FileFormat, out data);
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

            throw new InvalidOperationException(Strings.Error_InvalidPaddingType);
        }

        protected abstract void LoadAllData(StreamBuffer file);
        protected abstract void SaveAllData(StreamBuffer file);
        protected abstract bool DetectFileFormat(byte[] data, out DataFormat fmt);

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            FileFormat = fmt;
            LoadAllData(buf);
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
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
