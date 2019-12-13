using GTASaveData.Common;
using System;

namespace GTASaveData.GTA3
{
    public sealed class Vector2d : GTAObject,
        IEquatable<Vector2d>
    {
        private float m_x;
        private float m_y;

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

        public Vector2d()
        { }

        private Vector2d(Serializer serializer)
        {
            m_x = serializer.ReadSingle();
            m_y = serializer.ReadSingle();
        }

        private void Serialize(Serializer serializer)
        {
            serializer.Write(m_x);
            serializer.Write(m_y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vector2d);
        }

        public bool Equals(Vector2d other)
        {
            if (other == null)
            {
                return false;
            }

            return m_x.Equals(other.m_x)
                && m_y.Equals(other.m_y);
        }

        public override string ToString()
        {
            return string.Format("<{0:0.###},{1:0.###}>", m_x, m_y);
        }
    }
}
