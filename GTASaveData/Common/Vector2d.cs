using GTASaveData.Serialization;
using Newtonsoft.Json;
using System;

namespace GTASaveData.Common
{
    public class Vector2d : Chunk, IEquatable<Vector2d>
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

        [JsonIgnore]
        public double Magnitude
        {
            get { return Math.Sqrt((m_x * m_x) + (m_y * m_y)); }
        }

        public Vector2d()
        { }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_x = r.ReadSingle();
            m_y = r.ReadSingle();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_x);
            w.Write(m_y);
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
