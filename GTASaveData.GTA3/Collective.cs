using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    [Size(8)]
    public class Collective : SerializableObject,
        IEquatable<Collective>
    {
        private int m_index;
        private int m_field04h;

        public int Index
        {
            get { return m_index; }
            set { m_index = value; OnPropertyChanged(); }
        }

        public int Field04h
        {
            get { return m_field04h; }
            set { m_field04h = value; OnPropertyChanged(); }
        }

        public Collective()
        {
            m_index = -1;
            m_field04h = 0;
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_index = r.ReadInt32();
            m_field04h = r.ReadInt32();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_index);
            w.Write(m_field04h);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Collective);
        }

        public bool Equals(Collective other)
        {
            if (other == null)
            {
                return false;
            }

            return m_index.Equals(other.m_index)
                && m_field04h.Equals(other.m_field04h);
        }
    }
}
