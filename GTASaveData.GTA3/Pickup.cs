using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Pickup : SaveDataObject,
        IEquatable<Pickup>, IDeepClonable<Pickup>
    {
        private PickupType m_type;
        private bool m_hasBeenPickedUp;
        private ushort m_value;
        private uint m_objectIndex;
        private uint m_timer;
        private short m_modelIndex;
        private short m_poolIndex;
        private Vector3D m_position;

        public PickupType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public bool HasBeenPickedUp
        {
            get { return m_hasBeenPickedUp; }
            set { m_hasBeenPickedUp = value; OnPropertyChanged(); }
        }

        public ushort Value
        {
            get { return m_value; }
            set { m_value = value; OnPropertyChanged(); }
        }

        public uint ObjectIndex
        {
            get { return m_objectIndex; }
            set { m_objectIndex = value; OnPropertyChanged(); }
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

        public short PoolIndex
        {
            get { return m_poolIndex; }
            set { m_poolIndex = value; OnPropertyChanged(); }
        }

        public Vector3D Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Pickup()
        {
            PoolIndex = 1;
            Position = new Vector3D();
        }

        public Pickup(Pickup other)
        {
            Type = other.Type;
            HasBeenPickedUp = other.HasBeenPickedUp;
            Value = other.Value;
            ObjectIndex = other.ObjectIndex;
            RegenerationTime = other.RegenerationTime;
            ModelIndex = other.ModelIndex;
            PoolIndex = other.PoolIndex;
            Position = new Vector3D(other.Position);
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            Type = (PickupType) buf.ReadByte();
            HasBeenPickedUp = buf.ReadBool();
            Value = buf.ReadUInt16();
            ObjectIndex = buf.ReadUInt32();
            RegenerationTime = buf.ReadUInt32();
            ModelIndex = buf.ReadInt16();
            PoolIndex = buf.ReadInt16();
            Position = buf.Read<Vector3D>();

            Debug.Assert(buf.Offset == SizeOfType<Pickup>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write((byte) Type);
            buf.Write(HasBeenPickedUp);
            buf.Write(Value);
            buf.Write(ObjectIndex);
            buf.Write(RegenerationTime);
            buf.Write(ModelIndex);
            buf.Write(PoolIndex);
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
                && Value.Equals(other.Value)
                && ObjectIndex.Equals(other.ObjectIndex)
                && RegenerationTime.Equals(other.RegenerationTime)
                && ModelIndex.Equals(other.ModelIndex)
                && PoolIndex.Equals(other.PoolIndex)
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
