using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x1C)]
    public class Pickup : SaveDataObject, IEquatable<Pickup>
    {
        private PickupType m_type;
        private bool m_removed;
        private ushort m_quantity;
        private uint m_pObject;
        private uint m_timer;
        private short m_modelIndex;
        private short m_index;
        private Vector m_position;

        public PickupType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public bool HasBeenPickedUp
        {
            get { return m_removed; }
            set { m_removed = value; OnPropertyChanged(); }
        }

        public ushort Quantity
        {
            get { return m_quantity; }
            set { m_quantity = value; OnPropertyChanged(); }
        }

        public uint ObjectIndex
        {
            get { return m_pObject; }
            set { m_pObject = value; OnPropertyChanged(); }
        }

        public uint Timer
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        public short ModelIndex
        {
            get { return m_modelIndex; }
            set { m_modelIndex = value; OnPropertyChanged(); }
        }

        public short Index
        {
            get { return m_index; }
            set { m_index = value; OnPropertyChanged(); }
        }

        public Vector Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Pickup()
        {
            Position = new Vector();
            Index = 1;
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Type = (PickupType) buf.ReadByte();
            HasBeenPickedUp = buf.ReadBool();
            Quantity = buf.ReadUInt16();
            ObjectIndex = buf.ReadUInt32();
            Timer = buf.ReadUInt32();
            ModelIndex = buf.ReadInt16();
            Index = buf.ReadInt16();
            Position = buf.Read<Vector>();

            Debug.Assert(buf.Offset == SizeOf<Pickup>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write((byte) Type);
            buf.Write(HasBeenPickedUp);
            buf.Write(Quantity);
            buf.Write(ObjectIndex);
            buf.Write(Timer);
            buf.Write(ModelIndex);
            buf.Write(Index);
            buf.Write(Position);

            Debug.Assert(buf.Offset == SizeOf<Pickup>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pickup);
        }

        public bool Equals(Pickup other)
        {
            if (other == null)
            {
                return false;
            }

            return Type.Equals(other.Type)
                && HasBeenPickedUp.Equals(other.HasBeenPickedUp)
                && Quantity.Equals(other.Quantity)
                && ObjectIndex.Equals(other.ObjectIndex)
                && Timer.Equals(other.Timer)
                && ModelIndex.Equals(other.ModelIndex)
                && Index.Equals(other.Index)
                && Position.Equals(other.Position);
        }
    }
}
