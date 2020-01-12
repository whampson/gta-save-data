using GTASaveData.Serialization;
using System;
using System.Linq;
using WpfEssentials;

namespace GTASaveData.GTA3
{
    // TODO: test
    public sealed class Vehicles : SaveDataObject,
        IEquatable<Vehicles>
    {
        private FullyObservableCollection<Vehicle> m_vehiclesArray;
        private FullyObservableCollection<Boat> m_boatsArray;

        public FullyObservableCollection<Vehicle> VehiclesArray
        {
            get { return m_vehiclesArray; }
            set { m_vehiclesArray = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<Boat> BoatsArray
        {
            get { return m_boatsArray; }
            set { m_boatsArray = value; OnPropertyChanged(); }
        }

        public Vehicles()
        {
            m_vehiclesArray = new FullyObservableCollection<Vehicle>();
            m_boatsArray = new FullyObservableCollection<Boat>();
        }

        private Vehicles(SaveDataSerializer serializer, FileFormat format)
        {
            int numberOfVehicles = serializer.ReadInt32();
            int numberOfBoats = serializer.ReadInt32();
            m_vehiclesArray = new FullyObservableCollection<Vehicle>(serializer.ReadArray<Vehicle>(numberOfVehicles, format));
            m_boatsArray = new FullyObservableCollection<Boat>(serializer.ReadArray<Boat>(numberOfBoats, format));
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.Write(m_vehiclesArray.Count);
            serializer.Write(m_boatsArray.Count);
            serializer.WriteArray(m_vehiclesArray.ToArray());
            serializer.WriteArray(m_boatsArray.ToArray());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vehicles);
        }

        public bool Equals(Vehicles other)
        {
            if (other == null)
            {
                return false;
            }

            return m_vehiclesArray.SequenceEqual(other.m_vehiclesArray)
                && m_boatsArray.SequenceEqual(other.m_boatsArray);
        }
    }
}