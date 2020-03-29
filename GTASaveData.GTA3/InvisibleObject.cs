using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    [Size(8)]
    public class InvisibleObject : SaveDataObject, IEquatable<InvisibleObject>
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

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            Type = (ObjectType) buf.ReadInt32();
            StaticIndex = buf.ReadInt32();
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write((int) Type);
            buf.Write(StaticIndex);
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

            return Type.Equals(other.Type)
                && StaticIndex.Equals(other.StaticIndex);
        }
    }
}
