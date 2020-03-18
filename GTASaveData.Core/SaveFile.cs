using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData
{
    // TODO: add ability to add/remove blocks after main game data (make use of padding space)?

    /// <summary>
    /// Represents a saved <i>Grand Theft Auto</i> game.
    /// </summary>
    public abstract class SaveFile : GTAObject, IGTASave
    {
        protected WorkBuffer m_workBuf;
        private SaveFileFormat m_fileFormat;

        public SaveFileFormat SaveFormat
        {
            get { return m_fileFormat; }
            set { m_fileFormat = value; OnPropertyChanged(); }
        }

        public PaddingType PaddingMode
        {
            get { return m_workBuf.Padding; }
            set { m_workBuf.Padding = value; OnPropertyChanged(); }
        }

        public byte[] PaddingBytes
        {
            get { return m_workBuf.PaddingBytes; }
            set { m_workBuf.PaddingBytes = value; OnPropertyChanged(); }
        }

        public abstract string Name { get; set;  }
        public abstract DateTime TimeStamp { get; set; }

        public SaveFile()
        {
            m_fileFormat = SaveFileFormat.Default;
            m_workBuf = new WorkBuffer();
        }

        public void Write(string path)
        {
            m_workBuf.Reset();
            int bytesWritten = Serializer.Write(m_workBuf, this);
            File.WriteAllBytes(path, m_workBuf.ToByteArray());

            Debug.WriteLine("Wrote {0} bytes to disk.", bytesWritten);
        }

        protected abstract bool ReadBlockHeader(string tag, out int size);
        protected abstract void WriteBlockHeader(string tag, int size);
        protected abstract SaveFileFormat DetectFileFormat();

        public override string ToString()
        {
            return string.Format("{0}: {{ Name = {1}, FileFormat = {2} }}", GetType().Name, Name, m_fileFormat);
        }

        public static int GetAlignedAddress(int address)
        {
            const int WordSize = 4;

            return (address + WordSize) & ~WordSize;
        }
    }
}
