using GTASaveData.Common;
using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public class Pickup : SerializableObject,
        IEquatable<Pickup>
    {
        private PickupType m_type;
        private bool m_hasBeenPickedUp;
        private ushort m_amount;
        private uint m_objectIndex;
        private uint m_regenerationTime;
        private ushort m_modelId;
        private ushort m_flags;
        private Vector3d m_position;

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

        public ushort Amount
        {
            get { return m_amount; }
            set { m_amount = value; OnPropertyChanged(); }
        }

        public uint ObjectIndex
        {
            get { return m_objectIndex; }
            set { m_objectIndex = value; OnPropertyChanged(); }
        }

        public uint RegenerationTime
        {
            get { return m_regenerationTime; }
            set { m_regenerationTime = value; OnPropertyChanged(); }
        }

        public ushort ModelId
        {
            get { return m_modelId; }
            set { m_modelId = value; OnPropertyChanged(); }
        }

        public ushort Flags
        {
            get { return m_flags; }
            set { m_flags = value; OnPropertyChanged(); }
        }

        public Vector3d Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Pickup()
        {
            m_position = new Vector3d();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_type = (PickupType) r.ReadByte();
            m_hasBeenPickedUp = r.ReadBool();
            m_amount = r.ReadUInt16();
            m_objectIndex = r.ReadUInt32();
            m_regenerationTime = r.ReadUInt32();
            m_modelId = r.ReadUInt16();
            m_flags = r.ReadUInt16();
            m_position = r.ReadObject<Vector3d>();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write((byte) m_type);
            w.Write(m_hasBeenPickedUp);
            w.Write(m_amount);
            w.Write(m_objectIndex);
            w.Write(m_regenerationTime);
            w.Write(m_modelId);
            w.Write(m_flags);
            w.Write(m_position);
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
                && m_amount.Equals(other.m_amount)
                && m_objectIndex.Equals(other.m_objectIndex)
                && m_regenerationTime.Equals(other.m_regenerationTime)
                && m_modelId.Equals(other.m_modelId)
                && m_flags.Equals(other.m_flags)
                && m_position.Equals(other.m_position);
        }
    }
}
