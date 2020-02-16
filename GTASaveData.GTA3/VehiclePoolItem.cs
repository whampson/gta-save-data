using GTASaveData.Serialization;
using System;

namespace GTASaveData.GTA3
{
    public class VehiclePoolItem : SerializableObject,
        IEquatable<VehiclePoolItem>
    {
        private bool m_isBoat;
        private VehicleModel m_modelId;
        private uint m_vehicleRef;
        private Vehicle m_vehicle;

        public bool IsBoat
        { 
            get { return m_isBoat; }
        }

        public VehicleModel ModelId
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
            m_vehicle = (isBoat)
                ? new Boat() as Vehicle
                : new Automobile() as Vehicle;
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_isBoat = r.ReadBool(4);
            m_modelId = (VehicleModel) r.ReadUInt16();
            m_vehicleRef = r.ReadUInt32();
            m_vehicle = (m_isBoat)
                ? r.ReadObject<Boat>(fmt) as Vehicle
                : r.ReadObject<Automobile>(fmt) as Vehicle;
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_isBoat, 4);
            w.Write((ushort) m_modelId);
            w.Write(m_vehicleRef);
            w.Write(m_vehicle, fmt);
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