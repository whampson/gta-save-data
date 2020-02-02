using GTASaveData.Common;
using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public sealed class StoredCar : Chunk,
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

        private StoredCar(SaveDataSerializer serializer, FileFormat format)
        {
            m_modelId = serializer.ReadInt32();
            m_position = serializer.ReadObject<Vector3d>();
            m_rotation = serializer.ReadObject<Vector3d>();
            m_immunities = (StoredCarImmunities) serializer.ReadInt32();
            m_color1 = serializer.ReadByte();
            m_color2 = serializer.ReadByte();
            m_radio = (RadioStation) serializer.ReadByte();
            m_extra1 = serializer.ReadSByte();
            m_extra2 = serializer.ReadSByte();
            m_bomb = (BombType) serializer.ReadByte();
            serializer.Align();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.Write((int) m_modelId);
            serializer.WriteObject(m_position);
            serializer.WriteObject(m_rotation);
            serializer.Write((int) m_immunities);
            serializer.Write(m_color1);
            serializer.Write(m_color2);
            serializer.Write((byte) m_radio);
            serializer.Write(m_extra1);
            serializer.Write(m_extra2);
            serializer.Write((byte) m_bomb);
            serializer.Align();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
