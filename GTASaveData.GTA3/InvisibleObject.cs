using System;

namespace GTASaveData.GTA3
{
    public class InvisibleObject : SaveDataObject, IEquatable<InvisibleObject>
    {
        private PoolType m_type;
        private int m_handle;

        public PoolType Type
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
            Type = (PoolType) buf.ReadInt32();
            Handle = buf.ReadInt32();
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write((int) Type);
            buf.Write(Handle);
        }

        protected override int GetSize(DataFormat fmt)
        {
            return 8;
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
                && Handle.Equals(other.Handle);
        }
    }
}
