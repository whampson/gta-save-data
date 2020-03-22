using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;
using System.IO;

namespace GTASaveData
{
    // TODO: add ability to add/remove blocks after main game data (make use of padding space)?

    /// <summary>
    /// Represents a saved <i>Grand Theft Auto</i> game.
    /// </summary>
    public abstract class SaveFile : SaveDataObject, IGTASaveFile
    {
        private static readonly byte[] DefaultPadding = new byte[1] { 0 };

        protected WorkBuffer m_workBuf;
        protected int m_checksum;

        private SaveFileFormat m_fileFormat;
        private PaddingType m_padding;
        private byte[] m_paddingBytes;
        private bool m_disposed;

        public SaveFileFormat FileFormat
        {
            get { return m_fileFormat; }
            set { m_fileFormat = value; OnPropertyChanged(); }
        }

        public PaddingType Padding
        {
            get { return m_padding; }
            set { m_padding = value; OnPropertyChanged(); }
        }

        public byte[] PaddingBytes
        {
            get { return m_paddingBytes; }
            set { m_paddingBytes = value ?? DefaultPadding; }
        }

        public abstract string Name { get; set;  }
        public abstract DateTime TimeLastSaved { get; set; }

        public SaveFile()
        {
            m_fileFormat = SaveFileFormat.Default;
            m_workBuf = new WorkBuffer();
            m_padding = PaddingType.Default;
            m_paddingBytes = DefaultPadding;
            m_disposed = false;
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_workBuf.Dispose();
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
            byte[] data = File.ReadAllBytes(path);
            int bytesRead = Serializer.Read(data, fmt, out T obj);

            Debug.WriteLine("Read {0} bytes from disk.", bytesRead);

            return obj;
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

        protected byte[] GetPaddingBytes(int length)
        {
            switch (m_padding)
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

            return m_workBuf.ToArray(length);
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            FileFormat = fmt;
            LoadAllData(buf);
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            FileFormat = fmt;
            SaveAllData(buf);
        }

        protected abstract int ReadBlock(WorkBuffer file);
        protected abstract int WriteBlock(WorkBuffer file);
        protected abstract void LoadAllData(WorkBuffer file);
        protected abstract void SaveAllData(WorkBuffer file);
        protected abstract bool DetectFileFormat(byte[] data, out SaveFileFormat fmt);

        public override string ToString()
        {
            return string.Format("{0}: {{ Name = {1}, FileFormat = {2} }}", GetType().Name, Name, FileFormat);
        }
    }
}
