using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class PathData : SaveDataObject,
        IEquatable<PathData>, IDeepClonable<PathData>,
        IEnumerable<PathNode>
    {
        private Array<PathNode> m_pathNodes;

        public Array<PathNode> PathNodes
        {
            get { return m_pathNodes; }
            set { m_pathNodes = value; OnPropertyChanged(); }
        }

        public PathNode this[int i]
        {
            get { return PathNodes[i]; }
            set { PathNodes[i] = value; OnPropertyChanged(); }
        }

        public PathData()
            : this(0)
        { }

        public PathData(int saveSize)
        {
            PathNodes = ArrayHelper.CreateArray<PathNode>((saveSize / 2) * 8);
        }

        public PathData(PathData other)
        {
            PathNodes = ArrayHelper.DeepClone(other.PathNodes);
        }

        public static PathData Load(byte[] data)
        {
            PathData p = new PathData(data.Length);
            Serializer.Read(p, data, FileFormat.Default);

            return p;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            int size = SizeOfObject(this);
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

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            int size = SizeOfObject(this);
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

        public PathData DeepClone()
        {
            return new PathData(this);
        }

        public IEnumerator<PathNode> GetEnumerator()
        {
            return PathNodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
