using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class PathData : SaveDataObject, IEquatable<PathData>
    {
        private Array<PathNode> m_pathNodes;
        public Array<PathNode> PathNodes
        {
            get { return m_pathNodes; }
            set { m_pathNodes = value; OnPropertyChanged(); }
        }

        public PathData()
            : this(0)
        { }

        public PathData(int saveSize)
        {
            PathNodes = ArrayHelper.CreateArray<PathNode>((saveSize / 2) * 8);
        }

        public static PathData Load(byte[] data)
        {
            PathData p = new PathData(data.Length);
            Serializer.Read(p, data, FileFormat.Default);

            return p;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int size = SizeOf(this);
            byte[] data = buf.ReadBytes(size);
            int n = size / 2;

            for (int i = 0; i < PathNodes.Count; i++)
            {
                PathNodes[i].Disabled = ((data[i / 8] & (1 << i % 8)) != 0);
            }

            for (int i = 0; i < PathNodes.Count; i++)
            {
                PathNodes[i].BetweenLevels = ((data[i / 8 + n] & (1 << i % 8)) != 0);
            }

            Debug.Assert(buf.Offset == size);
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            int size = SizeOf(this);
            byte[] data = new byte[size];
            int n = size / 2;

            for (int i = 0; i < PathNodes.Count; i++)
            {
                if (PathNodes[i].Disabled)
                    data[i / 8] |= (byte) (1 << i % 8);
                else
                    data[i / 8] &= (byte) ~(1 << i % 8);
            }

            for (int i = 0; i < PathNodes.Count; i++)
            {
                if (PathNodes[i].BetweenLevels)
                    data[i / 8 + n] |= (byte) (1 << i % 8);
                else
                    data[i / 8 + n] &= (byte) ~(1 << i % 8);
            }

            buf.Write(data);
            Debug.Assert(buf.Offset == size);
        }

        protected override int GetSize(FileFormat fmt)
        {
            return ((PathNodes.Count + 7) / 8) * 2;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PathData);
        }

        public bool Equals(PathData other)
        {
            if (other == null)
            {
                return false;
            }

            return PathNodes.SequenceEqual(other.PathNodes);
        }
    }
}
