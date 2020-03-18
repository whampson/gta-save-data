using GTASaveData.Serialization;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(0x1C)]
    public class Pickup : SerializableObject,
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

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_type = (PickupType) r.ReadByte();
            m_hasBeenPickedUp = r.ReadBool();
            m_quantity = r.ReadUInt16();
            m_objectIndex = r.ReadUInt32();
            m_timer = r.ReadUInt32();
            m_modelId = r.ReadInt16();
            m_flags = r.ReadUInt16();
            m_position = r.ReadObject<Vector>();

            Debug.Assert(r.Position() - r.Marked() == SizeOf<Pickup>());
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write((byte) m_type);
            w.Write(m_hasBeenPickedUp);
            w.Write((ushort) m_quantity);
            w.Write(m_objectIndex);
            w.Write(m_timer);
            w.Write((short) m_modelId);
            w.Write((ushort) m_flags);
            w.Write(m_position);

            Debug.Assert(w.Position() - w.Marked() == SizeOf<Pickup>());
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
