using GTASaveData.Common;
using System;

namespace GTASaveData.GTA3
{
    public sealed class Collective : GTAObject,
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
        }

        private Collective(Serializer serializer)
        {
            m_unknown0 = serializer.ReadInt32();
            m_unknown1 = serializer.ReadInt32();
        }

        private void Serialize(Serializer serializer)
        {
            serializer.Write(m_unknown0);
            serializer.Write(m_unknown1);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

            return m_unknown0 == other.m_unknown0
                && m_unknown1 == other.m_unknown1;
        }

        public override string ToString()
        {
            return BuildToString(new (string, object)[]
            {
                (nameof(Unknown0), Unknown0),
                (nameof(Unknown1), Unknown1)
            });
        }
    }
}
