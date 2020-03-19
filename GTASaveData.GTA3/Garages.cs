using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x1478)]
    public class Garages : GTAObject,
        IEquatable<Garages>
    {
        public static class Limits
        {
            public const int NumberOfStoredCars = 18;
            public const int NumberOfGarages = 32;
        }

        private int m_numberOfGarages;
        private bool m_bombsAreFree;
        private bool m_respraysAreFree;
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

        public bool BombsAreFree
        { 
            get { return m_bombsAreFree; }
            set { m_bombsAreFree = value; OnPropertyChanged(); }
        }

        public bool RespraysAreFree
        { 
            get { return m_respraysAreFree; }
            set { m_respraysAreFree = value; OnPropertyChanged(); }
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

        public Array<Garage> GarageArray
        { 
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public Garages()
        {
            m_storedCars = new Array<StoredCar>();
            m_garages = new Array<Garage>();
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            m_numberOfGarages = buf.ReadInt32();
            m_bombsAreFree = buf.ReadBool(4);
            m_respraysAreFree = buf.ReadBool(4);
            m_carsCollected = buf.ReadInt32();
            m_bankVansCollected = buf.ReadInt32();
            m_policeCarsCollected = buf.ReadInt32();
            m_carTypesCollected1 = (CollectCars1) buf.ReadInt32();
            m_carTypesCollected2 = (CollectCars2) buf.ReadInt32();
            m_carTypesCollected3 = (CollectCars3) buf.ReadInt32();
            m_lastTimeHelpMessage = buf.ReadInt32();
            m_storedCars = buf.ReadArray<StoredCar>(Limits.NumberOfStoredCars);
            m_garages = buf.ReadArray<Garage>(Limits.NumberOfGarages);

            Debug.Assert(buf.Offset == SizeOf<Garages>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(m_numberOfGarages);
            buf.Write(m_bombsAreFree, 4);
            buf.Write(m_respraysAreFree, 4);
            buf.Write(m_carsCollected);
            buf.Write(m_bankVansCollected);
            buf.Write(m_policeCarsCollected);
            buf.Write((int) m_carTypesCollected1);
            buf.Write((int) m_carTypesCollected2);
            buf.Write((int) m_carTypesCollected3);
            buf.Write(m_lastTimeHelpMessage);
            buf.Write(m_storedCars.ToArray(), Limits.NumberOfStoredCars);
            buf.Write(m_garages.ToArray(), Limits.NumberOfGarages);

            Debug.Assert(buf.Offset == SizeOf<Garages>());
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
                && m_bombsAreFree.Equals(other.m_bombsAreFree)
                && m_respraysAreFree.Equals(other.m_respraysAreFree)
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
