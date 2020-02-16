using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public class InvisibleObject : SerializableObject,
        IEquatable<InvisibleObject>
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

        public InvisibleObject()
        { }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_type = (ObjectType) r.ReadInt32();
            m_staticIndex = r.ReadInt32();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write((int) m_type);
            w.Write(m_staticIndex);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InvisibleObject);
        }

        public bool Equals(InvisibleObject other)
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
