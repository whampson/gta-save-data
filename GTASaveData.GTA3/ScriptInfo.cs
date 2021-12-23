using GTASaveData.Interfaces;
using ICSharpCode.SharpZipLib.Zip.Compression;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// MAIN.SCM variable names, labels, and offsets.
    /// </summary>
    public class ScriptInfo : SaveDataObject,
        IEquatable<ScriptInfo>, IDeepClonable<ScriptInfo>
    {
        private byte[] m_data;
        private string m_infoText;

        public string InfoText  // TODO: break into sections
        {
            get { return m_infoText; }
            private set { m_infoText = value; OnPropertyChanged(); }
        }

        public ScriptInfo()
        {
            m_data = new byte[0];
            m_infoText = "";
        }

        public ScriptInfo(ScriptInfo other)
        {
            m_data = new byte[other.m_data.Length];
            Array.Copy(other.m_data, m_data, m_data.Length);
            m_infoText = other.InfoText;
        }

        private void Decompress()
        {
            using DataBuffer compressed = new DataBuffer(m_data);
            using DataBuffer expanded = new DataBuffer();

            do
            {
                Inflater inflater = new Inflater();

                long magic = compressed.ReadInt64();     // 9E2A83C1h
                long maxBufSize = compressed.ReadInt64();
                long compBufSize = compressed.ReadInt64();
                long outBufSize = compressed.ReadInt64();
                long compBufSize2 = compressed.ReadInt64();      // why?
                long outBufSize2 = compressed.ReadInt64();       // why?

                Debug.Assert(outBufSize <= maxBufSize);
                Debug.Assert(compBufSize == compBufSize2);
                Debug.Assert(outBufSize == outBufSize2);

                byte[] compBuf = compressed.ReadBytes((int) compBufSize);
                byte[] outBuf = new byte[outBufSize];

                inflater.SetInput(compBuf);
                int size = inflater.Inflate(outBuf);

                Debug.Assert(size == outBufSize);
                expanded.Write(outBuf, 0, size);

            } while (compressed.Position < compressed.Length);

            expanded.Seek(0);

            int dataSize = expanded.ReadInt32();
            Debug.Assert(expanded.Length == dataSize + 4);

            InfoText = Encoding.ASCII.GetString(expanded.ReadBytes(dataSize - 1));
        }

        protected override void ReadData(DataBuffer buf, FileType fmt)
        {
            int size = buf.ReadInt32();
            m_data = buf.ReadBytes(size);

            Decompress();
        }

        protected override void WriteData(DataBuffer buf, FileType fmt)
        {
            buf.Write(m_data.Length);
            buf.Write(m_data);
        }

        protected override int GetSize(FileType fmt)
        {
            return 4 + m_data.Length;
        }

        public bool Equals(ScriptInfo other)
        {
            return m_data.SequenceEqual(other.m_data);
        }

        public ScriptInfo DeepClone()
        {
            return new ScriptInfo(this);
        }
    }
}
