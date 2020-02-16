using GTASaveData.Common;
using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public class StoredCar : SerializableObject,
        IEquatable<StoredCar>
    {
        private int m_modelId;          // TODO: enum
        private Vector3d m_position;
        private Vector3d m_rotation;
        private StoredCarImmunities m_immunities;
        private byte m_color1;
        private byte m_color2;
        private RadioStation m_radio;
        private sbyte m_extra1;
        private sbyte m_extra2;
        private BombType m_bomb;

        public int ModelId
        {
            get { return m_modelId; }
            set { m_modelId = value; OnPropertyChanged(); }
        }

        public Vector3d Position
        {
            get { return m_position; }
            set { m_position = value; OnPropertyChanged(); }
        }

        public Vector3d Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; OnPropertyChanged(); }
        }

        public StoredCarImmunities Immunities
        {
            get { return m_immunities; }
            set { m_immunities = value; OnPropertyChanged(); }
        }

        public byte Color1
        {
            get { return m_color1; }
            set { m_color1 = value; OnPropertyChanged(); }
        }

        public byte Color2
        {
            get { return m_color2; }
            set { m_color2 = value; OnPropertyChanged(); }
        }

        public RadioStation Radio
        {
            get { return m_radio; }
            set { m_radio = value; OnPropertyChanged(); }
        }

        public sbyte Extra1
        {
            get { return m_extra1; }
            set { m_extra1 = value; OnPropertyChanged(); }
        }

        public sbyte Extra2
        {
            get { return m_extra2; }
            set { m_extra2 = value; OnPropertyChanged(); }
        }

        public BombType Bomb
        {
            get { return m_bomb; }
            set { m_bomb = value; OnPropertyChanged(); }
        }


        public StoredCar()
        {
            m_position = new Vector3d();
            m_rotation = new Vector3d();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_modelId = r.ReadInt32();
            m_position = r.ReadObject<Vector3d>();
            m_rotation = r.ReadObject<Vector3d>();
            m_immunities = (StoredCarImmunities) r.ReadInt32();
            m_color1 = r.ReadByte();
            m_color2 = r.ReadByte();
            m_radio = (RadioStation) r.ReadByte();
            m_extra1 = r.ReadSByte();
            m_extra2 = r.ReadSByte();
            m_bomb = (BombType) r.ReadByte();
            r.Align();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write((int) m_modelId);
            w.Write(m_position);
            w.Write(m_rotation);
            w.Write((int) m_immunities);
            w.Write(m_color1);
            w.Write(m_color2);
            w.Write((byte) m_radio);
            w.Write(m_extra1);
            w.Write(m_extra2);
            w.Write((byte) m_bomb);
            w.Align();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StoredCar);
        }

        public bool Equals(StoredCar other)
        {
            if (other == null)
            {
                return false;
            }

            return m_modelId.Equals(other.m_modelId)
                && m_position.Equals(other.m_position)
                && m_rotation.Equals(other.m_rotation)
                && m_immunities.Equals(other.m_immunities)
                && m_color1.Equals(other.m_color1)
                && m_color2.Equals(other.m_color2)
                && m_radio.Equals(other.m_radio)
                && m_extra1.Equals(other.m_extra1)
                && m_extra2.Equals(other.m_extra2)
                && m_bomb.Equals(other.m_bomb);
        }
    }
}
