using GTASaveData.Common;
using System;

namespace GTASaveData.GTA3
{
    public sealed class InvisibilitySetting : GTAObject,
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

        private InvisibilitySetting(Serializer serializer)
        {
            m_type = (ObjectType) serializer.ReadInt32();
            m_staticIndex = serializer.ReadInt32();
        }

        private void Serialize(Serializer serializer)
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

            return m_type == other.m_type
                && m_staticIndex == other.m_staticIndex;
        }

        public override string ToString()
        {
            return BuildToString(new (string, object)[]
            {
                (nameof(Type), Type),
                (nameof(StaticIndex), StaticIndex)
            });
        }
    }
}
