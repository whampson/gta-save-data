using GTASaveData.Serialization;
using System;

namespace GTASaveData.Common
{
    public sealed class Vector2d : Chunk,
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

        private Vector2d(SaveDataSerializer serializer, FileFormat format)
        {
            m_x = serializer.ReadSingle();
            m_y = serializer.ReadSingle();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
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
    }
}
