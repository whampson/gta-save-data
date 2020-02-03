using GTASaveData.Serialization;
using Newtonsoft.Json;
using System;

namespace GTASaveData.Common
{
    public class Vector3d : Chunk, IEquatable<Vector3d>
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

        [JsonIgnore]
        public double Magnitude
        {
            get { return Math.Sqrt((m_x * m_x) + (m_y * m_y) + (m_z * m_z)); }
        }

        public Vector3d()
        { }

        private Vector3d(SaveDataSerializer serializer, FileFormat format)
        {
            m_x = serializer.ReadSingle();
            m_y = serializer.ReadSingle();
            m_z = serializer.ReadSingle();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.Write(m_x);
            serializer.Write(m_y);
            serializer.Write(m_z);
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

            return m_x.Equals(other.m_x)
                && m_y.Equals(other.m_y)
                && m_z.Equals(other.m_z);
        }
    }
}
