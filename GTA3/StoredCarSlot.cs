using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public sealed class StoredCarSlot : SaveDataObject,
        IEquatable<StoredCarSlot>
    {
        private StoredCar m_portland;
        private StoredCar m_staunton;
        private StoredCar m_shoreside;

        public StoredCar Portland
        {
            get { return m_portland; }
            set { m_portland = value; OnPropertyChanged(); }
        }

        public StoredCar Staunton
        {
            get { return m_staunton; }
            set { m_staunton = value; OnPropertyChanged(); }
        }

        public StoredCar Shoreside
        {
            get { return m_shoreside; }
            set { m_shoreside = value; OnPropertyChanged(); }
        }

        public StoredCarSlot()
        {
            m_portland = new StoredCar();
            m_staunton = new StoredCar();
            m_shoreside = new StoredCar();
        }

        private StoredCarSlot(SaveDataSerializer serializer, FileFormat format)
        {
            m_portland = serializer.ReadObject<StoredCar>();
            m_staunton = serializer.ReadObject<StoredCar>();
            m_shoreside = serializer.ReadObject<StoredCar>();
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.WriteObject(m_portland);
            serializer.WriteObject(m_staunton);
            serializer.WriteObject(m_shoreside);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StoredCarSlot);
        }

        public bool Equals(StoredCarSlot other)
        {
            if (other == null)
            {
                return false;
            }

            return m_portland.Equals(other.m_portland)
                && m_staunton.Equals(other.m_staunton)
                && m_shoreside.Equals(other.m_shoreside);
        }
    }
}
