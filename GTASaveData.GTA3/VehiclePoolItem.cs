using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public sealed class VehiclePoolItem : SaveDataObject,
        IEquatable<VehiclePoolItem>
    {
        private bool m_isBoat;
        private ushort m_modelId;       // TODO: enum
        private uint m_vehicleRef;
        private Vehicle m_vehicle;

        public bool IsBoat
        { 
            get { return m_isBoat; }
        }

        public ushort ModelId
        { 
            get { return m_modelId; }
            set { m_modelId = value; OnPropertyChanged(); }
        }

        public uint VehicleRef
        { 
            get { return m_vehicleRef; }
            set { m_vehicleRef = value; OnPropertyChanged(); }
        }

        public Vehicle Vehicle
        { 
            get { return m_vehicle; }
            set { m_vehicle = value; OnPropertyChanged(); }
        }

        public VehiclePoolItem()
            : this(false)
        { }

        public VehiclePoolItem(bool isBoat)
        {
            m_isBoat = isBoat;
            if (isBoat)
            {
                m_vehicle = new Boat();
            }
            else
            {
                m_vehicle = new Car();
            }
        }

        private VehiclePoolItem(SaveDataSerializer serializer, FileFormat format)
        {
            m_isBoat = serializer.ReadBool(4);
            m_modelId = serializer.ReadUInt16();
            m_vehicleRef = serializer.ReadUInt32();
            if (m_isBoat)
            {
                m_vehicle = serializer.ReadObject<Boat>(format);
            }
            else
            {
                m_vehicle = serializer.ReadObject<Car>(format);
            }
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.Write(m_isBoat, 4);
            serializer.Write(m_modelId);
            serializer.Write(m_vehicleRef);
            serializer.WriteObject(m_vehicle, format);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(VehiclePoolItem other)
        {
            if (other == null)
            {
                return false;
            }

            return m_isBoat.Equals(other.m_isBoat)
                && m_modelId.Equals(other.m_modelId)
                && m_vehicleRef.Equals(other.m_vehicleRef)
                && m_vehicle.Equals(other.m_vehicle);
        }
    }
}