using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x1C)]
    public class Pickup : GTAObject,
        IEquatable<Pickup>
    {
        private PickupType m_type;
        private bool m_hasBeenPickedUp;
        private int m_quantity;
        private uint m_objectIndex;
        private uint m_timer;
        private int m_modelId;
        private int m_flags;
        private Vector m_position;

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

        public int Amount
        {
            get { return m_quantity; }
            set { m_quantity = value; OnPropertyChanged(); }
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

        public int ModelId
        {
            get { return m_modelId; }
            set { m_modelId = value; OnPropertyChanged(); }
        }

        public int Flags
        {
            get { return m_flags; }
            set { m_flags = value; OnPropertyChanged(); }
        }

        public Vector Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Pickup()
        {
            m_position = new Vector();
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            m_type = (PickupType) buf.ReadByte();
            m_hasBeenPickedUp = buf.ReadBool();
            m_quantity = buf.ReadUInt16();
            m_objectIndex = buf.ReadUInt32();
            m_timer = buf.ReadUInt32();
            m_modelId = buf.ReadInt16();
            m_flags = buf.ReadUInt16();
            m_position = buf.ReadObject<Vector>();

            Debug.Assert(buf.Offset == SizeOf<Pickup>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write((byte) m_type);
            buf.Write(m_hasBeenPickedUp);
            buf.Write((ushort) m_quantity);
            buf.Write(m_objectIndex);
            buf.Write(m_timer);
            buf.Write((short) m_modelId);
            buf.Write((ushort) m_flags);
            buf.Write(m_position);

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

            return m_type.Equals(other.m_type)
                && m_hasBeenPickedUp.Equals(other.m_hasBeenPickedUp)
                && m_quantity.Equals(other.m_quantity)
                && m_objectIndex.Equals(other.m_objectIndex)
                && m_timer.Equals(other.m_timer)
                && m_modelId.Equals(other.m_modelId)
                && m_flags.Equals(other.m_flags)
                && m_position.Equals(other.m_position);
        }
    }
}
