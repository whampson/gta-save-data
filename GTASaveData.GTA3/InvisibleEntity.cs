using System;

namespace GTASaveData.GTA3
{
    [Size(8)]
    public class InvisibleEntity : SaveDataObject, IEquatable<InvisibleEntity>
    {
        private ObjectType m_type;
        private int m_handle;

        public ObjectType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public int Handle
        {
            get { return m_handle; }
            set { m_handle = value; OnPropertyChanged(); }
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Type = (ObjectType) buf.ReadInt32();
            Handle = buf.ReadInt32();
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write((int) Type);
            buf.Write(Handle);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InvisibleEntity);
        }

        public bool Equals(InvisibleEntity other)
        {
            if (other == null)
            {
                return false;
            }

            return Type.Equals(other.Type)
                && Handle.Equals(other.Handle);
        }
    }
}
