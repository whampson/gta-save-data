using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public class Collective : SerializableObject,
        IEquatable<Collective>
    {
        private int m_unknown0;
        private int m_unknown1;

        public int Unknown0
        {
            get { return m_unknown0; }
            set { m_unknown0 = value; OnPropertyChanged(); }
        }

        public int Unknown1
        {
            get { return m_unknown1; }
            set { m_unknown1 = value; OnPropertyChanged(); }
        }

        public Collective()
        {
            m_unknown0 = -1;
            m_unknown0 = 0;
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_unknown0 = r.ReadInt32();
            m_unknown1 = r.ReadInt32();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_unknown0);
            w.Write(m_unknown1);
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

            return m_unknown0.Equals(other.m_unknown0)
                && m_unknown1.Equals(other.m_unknown1);
        }
    }
}
