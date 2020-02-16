using GTASaveData.Serialization;
using System;
using System.Linq;

namespace GTASaveData.GTA3.Blocks
{
    public class GarageBlock : SerializableObject,
        IEquatable<GarageBlock>
    {
        public static class Limits
        {
            public const int StoredCarsCount = 18;
            public const int GaragesCount = 32;
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
        private Array<StoredCar> m_storedCars;
        private Array<Garage> m_garages;

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

        public Array<StoredCar> StoredCars
        { 
            get { return m_storedCars; }
            set { m_storedCars = value; OnPropertyChanged(); }
        }

        public Array<Garage> Garages
        { 
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public GarageBlock()
        {
            m_storedCars = new Array<StoredCar>();
            m_garages = new Array<Garage>();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_numberOfGarages = r.ReadInt32();
            m_freeBombs = r.ReadBool(4);
            m_freeResprays = r.ReadBool(4);
            m_carsCollected = r.ReadInt32();
            m_bankVansCollected = r.ReadInt32();
            m_policeCarsCollected = r.ReadInt32();
            m_carTypesCollected1 = (CollectCars1) r.ReadInt32();
            m_carTypesCollected2 = (CollectCars2) r.ReadInt32();
            m_carTypesCollected3 = (CollectCars3) r.ReadInt32();
            m_lastTimeHelpMessage = r.ReadInt32();
            m_storedCars = r.ReadArray<StoredCar>(Limits.StoredCarsCount);
            m_garages = r.ReadArray<Garage>(Limits.GaragesCount);
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_numberOfGarages);
            w.Write(m_freeBombs, 4);
            w.Write(m_freeResprays, 4);
            w.Write(m_carsCollected);
            w.Write(m_bankVansCollected);
            w.Write(m_policeCarsCollected);
            w.Write((int) m_carTypesCollected1);
            w.Write((int) m_carTypesCollected2);
            w.Write((int) m_carTypesCollected3);
            w.Write(m_lastTimeHelpMessage);
            w.Write(m_storedCars.ToArray(), Limits.StoredCarsCount);
            w.Write(m_garages.ToArray(), Limits.GaragesCount);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GarageBlock);
        }

        public bool Equals(GarageBlock other)
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
                && m_storedCars.SequenceEqual(other.m_storedCars)
                && m_garages.SequenceEqual(other.m_garages);
        }
    }
}
