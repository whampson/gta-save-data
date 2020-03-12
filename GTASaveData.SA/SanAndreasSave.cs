using GTASaveData.Common;
using GTASaveData.Extensions;
using GTASaveData.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData.SA
{
    public class SanAndreasSave : GrandTheftAutoSave,
        IGrandTheftAutoSave,
        IEquatable<SanAndreasSave>
    {
        private const int SizeOfGameInBytes = 0x31800;

        public SimpleVars SimpleVars
        {
            get { return m_blocks[0] as SimpleVars; }
            set { m_blocks[0] = value; OnPropertyChanged(); }
        }

        public override string Name => SimpleVars.SaveName;

        protected override int MaxBlockSize => 0xC800;

        protected override int BlockCount => 28;

        protected override int SectionCount => throw new NotImplementedException();

        protected override int SimpleVarsSize => 0x138;

        ISimpleVars IGrandTheftAutoSave.SimpleVars => SimpleVars;

        protected override FileFormat DetectFileFormat(byte[] data)
        {
            return FileFormat.None;
        }

        protected byte[] ReadBlock(Serializer r)
        {
            const string Tag = "BLOCK";

            long mark;
            string tag;
            byte[] data;
            int blockSize;
            int bufferSize;
            int index;

            tag = r.ReadString(5);
            Debug.Assert(tag == Tag);

            mark = r.BaseStream.Position;
            blockSize = 0;
            index = -1;

            do
            {
                bufferSize = (int) Math.Min(MaxBlockSize, r.BaseStream.Length - mark - 4);
                if (bufferSize < 4)
                {
                    break;
                }

                data = r.ReadBytes(bufferSize);
                index = data.FindFirst("BLOCK".GetAsciiBytes());
                if (index == -1)
                {
                    blockSize += data.Length;
                }
            } while (index == -1);

            blockSize += index;

            r.BaseStream.Position = mark;
            data = r.ReadBytes(blockSize);

            return data;
        }

        protected byte[] CreateBlock(byte[] data)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer w = CreateSerializer(m))
                {
                    w.Write("BLOCK", 5);
                    w.Write(data);
                }

                return m.ToArray();
            }
        }

        protected override byte[] ReadBlock(Serializer r, string tag) => ReadBlock(r);

        protected override byte[] CreateBlock(string tag, byte[] data) => CreateBlock(data);

        protected override void LoadSection(int index, byte[] data) => throw new NotImplementedException();

        protected override byte[] SaveSection(int index) => throw new NotImplementedException();

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            int index = 0;

            while (index < BlockCount)
            {
                byte[] data = ReadBlock(r);
                switch (index)
                {
                    case 0: SimpleVars = Deserialize<SimpleVars>(data); break;
                    default:
                        m_blocks[index] = new Block(data);
                        break;
                }

                index++;
            }

            Debug.WriteLine("Loaded San Andreas -- blocks: {0}", index);
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            int bytesWritten = 0;
            int checkSum = 0;

            foreach (var obj in m_blocks)
            {
                byte[] block = CreateBlock(Serialize(obj));
                checkSum += block.Sum(x => x);

                bytesWritten += w.Write(block);
            }

            // TODO: padding

            w.Write(checkSum);
            bytesWritten += 4;

            Debug.WriteLine("Saved San Andreas -- total bytes: {0}, blocks: {1}", bytesWritten, m_blocks.Length);
            Debug.Assert(m_blocks.Length == BlockCount);
            Debug.Assert(bytesWritten == SizeOfGameInBytes);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SanAndreasSave);
        }

        public bool Equals(SanAndreasSave other)
        {
            if (other == null)
            {
                return false;
            }

            return m_blocks.SequenceEqual(other.m_blocks);
        }
    }
}
