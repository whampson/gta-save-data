using System;

namespace GTASaveData.GTA3
{
    public sealed class InvisibilitySetting : SaveDataObject,
        IEquatable<InvisibilitySetting>
    {
        private ObjectType m_type;
        private int m_staticIndex;

        public ObjectType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public int StaticIndex
        {
            get { return m_staticIndex; }
            set { m_staticIndex = value; OnPropertyChanged(); }
        }

        public InvisibilitySetting()
        { }

        protected override void ReadObjectData(SaveDataSerializer serializer, SystemType system)
        {
            m_type = (ObjectType) serializer.ReadInt32();
            m_staticIndex = serializer.ReadInt32();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, SystemType system)
        {
            serializer.Write((int) m_type);
            serializer.Write(m_staticIndex);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InvisibilitySetting);
        }

        public bool Equals(InvisibilitySetting other)
        {
            if (other == null)
            {
                return false;
            }

            return m_type.Equals(other.m_type)
                && m_staticIndex.Equals(other.m_staticIndex);
        }
    }
}
