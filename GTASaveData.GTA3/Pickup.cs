using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Pickup : SaveDataObject,
        IEquatable<Pickup>, IDeepClonable<Pickup>
    {
        private PickupType m_type;
        private bool m_removed;
        private ushort m_quantity;
        private uint m_pObject;
        private uint m_timer;
        private short m_modelIndex;
        private short m_index;
        private Vector3D m_position;

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

        public uint Handle
        {
            get { return m_pObject; }
            set { m_pObject = value; OnPropertyChanged(); }
        }

        public uint RegenerationTime
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        public short ModelIndex
        {
            get { return m_modelIndex; }
            set { m_modelIndex = value; OnPropertyChanged(); }
        }

        public short PickupIndex
        {
            get { return m_index; }
            set { m_index = value; OnPropertyChanged(); }
        }

        public Vector3D Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Pickup()
        {
            PickupIndex = 1;
        }

        public Pickup(Pickup other)
        {
            Type = other.Type;
            HasBeenPickedUp = other.HasBeenPickedUp;
            Quantity = other.Quantity;
            Handle = other.Handle;
            RegenerationTime = other.RegenerationTime;
            ModelIndex = other.ModelIndex;
            PickupIndex = other.PickupIndex;
            Position = other.Position;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Type = (PickupType) buf.ReadByte();
            HasBeenPickedUp = buf.ReadBool();
            Quantity = buf.ReadUInt16();
            Handle = buf.ReadUInt32();
            RegenerationTime = buf.ReadUInt32();
            ModelIndex = buf.ReadInt16();
            PickupIndex = buf.ReadInt16();
            Position = buf.Read<Vector3D>();

            Debug.Assert(buf.Offset == SizeOfType<Pickup>());
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write((byte) Type);
            buf.Write(HasBeenPickedUp);
            buf.Write(Quantity);
            buf.Write(Handle);
            buf.Write(RegenerationTime);
            buf.Write(ModelIndex);
            buf.Write(PickupIndex);
            buf.Write(Position);

            Debug.Assert(buf.Offset == SizeOfType<Pickup>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x1C;
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
                && Handle.Equals(other.Handle)
                && RegenerationTime.Equals(other.RegenerationTime)
                && ModelIndex.Equals(other.ModelIndex)
                && PickupIndex.Equals(other.PickupIndex)
                && Position.Equals(other.Position);
        }

        public Pickup DeepClone()
        {
            return new Pickup(this);
        }
    }

    public enum PickupType
    {
        None,
        InShop,
        OnStreet,
        Once,
        OnceTimeout,
        Collectible1,
        InShopOutOfStock,
        Money,
        MineInactive,
        MineArmed,
        NauticalMineInactive,
        NauticalMineArmed,
        FloatingPackage,
        FloatingPackageFloating,
        OnStreetSlow
    }
}
