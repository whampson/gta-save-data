using GTASaveData.Interfaces;
using System;

namespace GTASaveData.Types
{
    /// <summary>
    /// Represents an object that should be invisible in the game.
    /// </summary>
    public class InvisibleObject : SaveDataObject,
        IEquatable<InvisibleObject>, IDeepClonable<InvisibleObject>
    {
        private EntityClassType m_type;
        private int m_handle;

        /// <summary>
        /// Entity class type. Controls which entity pool the <see cref="InvisibleObject"/>
        /// goes into when the game is loaded.
        /// </summary>
        public EntityClassType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Entity handle; pool index + 1.
        /// </summary>
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

        protected override void ReadData(DataBuffer buf, SerializationParams p)
        {
            Type = (EntityClassType) buf.ReadInt32();
            Handle = buf.ReadInt32();
        }

        protected override void WriteData(DataBuffer buf, SerializationParams p)
        {
            buf.Write((int) Type);
            buf.Write(Handle);
        }

        protected override int GetSize(SerializationParams p)
        {
            return sizeof(int)
                + sizeof(int);
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
