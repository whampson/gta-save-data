using GTASaveData.Types.Interfaces;
using System;

namespace GTASaveData.Types
{
    public class InvisibleObject : SaveDataObject, IInvisibleObject,
        IEquatable<InvisibleObject>, IDeepClonable<InvisibleObject>
    {
        private PoolType m_type;
        private int m_handle;

        int IInvisibleObject.Type
        {
            get { return (int) Type; }
            set { Type = (PoolType) Type; }
        }

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

        public InvisibleObject()
        { }

        public InvisibleObject(InvisibleObject other)
        {
            Type = other.Type;
            Handle = other.Handle;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Type = (PoolType) buf.ReadInt32();
            Handle = buf.ReadInt32();
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write((int) Type);
            buf.Write(Handle);
        }

        protected override int GetSize(FileFormat fmt)
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

        public InvisibleObject DeepClone()
        {
            return new InvisibleObject(this);
        }
    }
}
