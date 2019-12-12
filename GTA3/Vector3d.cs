using GTASaveData.Common;
using System;

namespace GTASaveData.GTA3
{
    public sealed class Vector3d : GTAObject,
        IEquatable<Vector3d>
    {
        private float m_x;
        private float m_y;
        private float m_z;

        public float X
        {
            get { return m_x; }
            set { m_x = value; OnPropertyChanged(); }
        }

        public float Y
        {
            get { return m_y; }
            set { m_y = value; OnPropertyChanged(); }
        }

        public float Z
        {
            get { return m_z; }
            set { m_z = value; OnPropertyChanged(); }
        }

        public Vector3d()
        { }

        private Vector3d(Serializer serializer)
        {
            m_x = serializer.ReadSingle();
            m_y = serializer.ReadSingle();
            m_z = serializer.ReadSingle();
        }

        private void Serialize(Serializer serializer)
        {
            serializer.Write(m_x);
            serializer.Write(m_y);
            serializer.Write(m_z);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vector3d);
        }

        public bool Equals(Vector3d other)
        {
            if (other == null)
            {
                return false;
            }

            return m_x == other.m_x
                && m_y == other.m_y
                && m_z == other.m_z;
        }

        public override string ToString()
        {
            return string.Format("<{0:0.###},{1:0.###},{2:0.###}>", m_x, m_y, m_z);
        }
    }
}
