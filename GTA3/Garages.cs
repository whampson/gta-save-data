using System;
using System.Linq;
using WpfEssentials;

namespace GTASaveData.GTA3
{
    public sealed class Garages : SaveDataObject,
        IEquatable<Garages>
    {
        public static class Limits
        {
            public const int StoredCarSlotsCount = 6;
            public const int GarageObjectsCount = 32;
        }

        private int m_numberOfGarages;
        private bool m_freeBombs;
        private bool m_freeResprays;
        private int m_carsCollected;
        private int m_bankVansCollected;
        private int m_policeCarsCollected;
        private CollectCars1 m_carTypesCollected1;
        private CollectCars2 m_carTypesCollected2;
        private CollectCars3 m_carTypesCollected3;
        private int m_lastTimeHelpMessage;
        private FullyObservableCollection<StoredCarSlot> m_storedCarSlots;
        private FullyObservableCollection<Garage> m_garageObjects;

        public int NumberOfGarages
        { 
            get { return m_numberOfGarages; }
            set { m_numberOfGarages = value; OnPropertyChanged(); }
        }

        public bool FreeBombs
        { 
            get { return m_freeBombs; }
            set { m_freeBombs = value; OnPropertyChanged(); }
        }

        public bool FreeResprays
        { 
            get { return m_freeResprays; }
            set { m_freeResprays = value; OnPropertyChanged(); }
        }

        public int CarsCollected
        { 
            get { return m_carsCollected; }
            set { m_carsCollected = value; OnPropertyChanged(); }
        }

        public int BankVansCollected
        { 
            get { return m_bankVansCollected; }
            set { m_bankVansCollected = value; OnPropertyChanged(); }
        }

        public int PoliceCarsCollected
        { 
            get { return m_policeCarsCollected; }
            set { m_policeCarsCollected = value; OnPropertyChanged(); }
        }

        public CollectCars1 CarTypesCollected1
        { 
            get { return m_carTypesCollected1; }
            set { m_carTypesCollected1 = value; OnPropertyChanged(); }
        }

        public CollectCars2 CarTypesCollected2
        { 
            get { return m_carTypesCollected2; }
            set { m_carTypesCollected2 = value; OnPropertyChanged(); }
        }

        public CollectCars3 CarTypesCollected3
        { 
            get { return m_carTypesCollected3; }
            set { m_carTypesCollected3 = value; OnPropertyChanged(); }
        }

        public int LastTimeHelpMessage
        { 
            get { return m_lastTimeHelpMessage; }
            set { m_lastTimeHelpMessage = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<StoredCarSlot> StoredCarSlots
        { 
            get { return m_storedCarSlots; }
            set { m_storedCarSlots = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<Garage> GarageObjects
        { 
            get { return m_garageObjects; }
            set { m_garageObjects = value; OnPropertyChanged(); }
        }

        public Garages()
        {
            m_storedCarSlots = new FullyObservableCollection<StoredCarSlot>();
            m_garageObjects = new FullyObservableCollection<Garage>();
        }

        private Garages(SaveDataSerializer serializer, SystemType system)
        {
            m_numberOfGarages = serializer.ReadInt32();
            m_freeBombs = serializer.ReadBool(4);
            m_freeResprays = serializer.ReadBool(4);
            m_carsCollected = serializer.ReadInt32();
            m_bankVansCollected = serializer.ReadInt32();
            m_policeCarsCollected = serializer.ReadInt32();
            m_carTypesCollected1 = (CollectCars1) serializer.ReadInt32();
            m_carTypesCollected2 = (CollectCars2) serializer.ReadInt32();
            m_carTypesCollected3 = (CollectCars3) serializer.ReadInt32();
            m_lastTimeHelpMessage = serializer.ReadInt32();
            m_storedCarSlots = new FullyObservableCollection<StoredCarSlot>(serializer.ReadArray<StoredCarSlot>(Limits.StoredCarSlotsCount));
            m_garageObjects = new FullyObservableCollection<Garage>(serializer.ReadArray<Garage>(Limits.GarageObjectsCount));
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, SystemType system)
        {
            serializer.Write(m_numberOfGarages);
            serializer.Write(m_freeBombs, 4);
            serializer.Write(m_freeResprays, 4);
            serializer.Write(m_carsCollected);
            serializer.Write(m_bankVansCollected);
            serializer.Write(m_policeCarsCollected);
            serializer.Write((int) m_carTypesCollected1);
            serializer.Write((int) m_carTypesCollected2);
            serializer.Write((int) m_carTypesCollected3);
            serializer.Write(m_lastTimeHelpMessage);
            serializer.WriteArray(m_storedCarSlots, Limits.StoredCarSlotsCount);
            serializer.WriteArray(m_garageObjects, Limits.GarageObjectsCount);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Garages);
        }

        public bool Equals(Garages other)
        {
            if (other == null)
            {
                return false;
            }

            return m_numberOfGarages.Equals(other.m_numberOfGarages)
                && m_freeBombs.Equals(other.m_freeBombs)
                && m_freeResprays.Equals(other.m_freeResprays)
                && m_carsCollected.Equals(other.m_carsCollected)
                && m_bankVansCollected.Equals(other.m_bankVansCollected)
                && m_policeCarsCollected.Equals(other.m_policeCarsCollected)
                && m_carTypesCollected1.Equals(other.m_carTypesCollected1)
                && m_carTypesCollected2.Equals(other.m_carTypesCollected2)
                && m_carTypesCollected3.Equals(other.m_carTypesCollected3)
                && m_lastTimeHelpMessage.Equals(other.m_lastTimeHelpMessage)
                && m_storedCarSlots.SequenceEqual(other.m_storedCarSlots)
                && m_garageObjects.SequenceEqual(other.m_garageObjects);
        }
    }
}
