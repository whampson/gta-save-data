using System;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3
{
    public class GarageData : SaveDataObject, IEquatable<GarageData>
    {
        public static class Limits
        {
            public const int CarsPerSafeHouse = 6;
            public const int NumberOfSafeHouses = 3;
            public const int MaxNumGarages = 32;
        }

        private int m_numGarages;
        private bool m_bombsAreFree;
        private bool m_respraysAreFree;
        private int m_carsCollected;        // not used
        private int m_bankVansCollected;
        private int m_policeCarsCollected;  // not used
        private CollectCars1 m_carTypesCollected1;
        private CollectCars2 m_carTypesCollected2;
        private int m_carTypesCollected3;   // not used
        private int m_lastTimeHelpMessage;
        private Array<StoredCar> m_carsInSafeHouse;
        private Array<Garage> m_garages;

        public int NumGarages
        { 
            get { return m_numGarages; }
            set { m_numGarages = value; OnPropertyChanged(); }
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

        [Obsolete("Not used by the game.")]
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

        [Obsolete("Not used by the game.")]
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

        [Obsolete("Not used by the game.")]
        public int CarTypesCollected3
        { 
            get { return m_carTypesCollected3; }
            set { m_carTypesCollected3 = value; OnPropertyChanged(); }
        }

        public int LastTimeHelpMessage
        { 
            get { return m_lastTimeHelpMessage; }
            set { m_lastTimeHelpMessage = value; OnPropertyChanged(); }
        }

        public Array<StoredCar> CarsInSafeHouse
        { 
            get { return m_carsInSafeHouse; }
            set { m_carsInSafeHouse = value; OnPropertyChanged(); }
        }

        public Array<Garage> Garages
        { 
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public GarageData()
        {
            CarsInSafeHouse = new Array<StoredCar>();
            Garages = new Array<Garage>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            NumGarages = buf.ReadInt32();
            BombsAreFree = buf.ReadBool(4);
            RespraysAreFree = buf.ReadBool(4);
            CarsCollected = buf.ReadInt32();
            BankVansCollected = buf.ReadInt32();
            PoliceCarsCollected = buf.ReadInt32();
            CarTypesCollected1 = (CollectCars1) buf.ReadInt32();
            CarTypesCollected2 = (CollectCars2) buf.ReadInt32();
            CarTypesCollected3 = buf.ReadInt32();
            LastTimeHelpMessage = buf.ReadInt32();
            CarsInSafeHouse = buf.Read<StoredCar>(Limits.CarsPerSafeHouse * Limits.NumberOfSafeHouses);
            Garages = buf.Read<Garage>(Limits.MaxNumGarages);

            buf.Skip(244);

            Debug.Assert(buf.Offset == SizeOf<GarageData>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(NumGarages);
            buf.Write(BombsAreFree, 4);
            buf.Write(RespraysAreFree, 4);
            buf.Write(CarsCollected);
            buf.Write(BankVansCollected);
            buf.Write(PoliceCarsCollected);
            buf.Write((int) CarTypesCollected1);
            buf.Write((int) CarTypesCollected2);
            buf.Write(CarTypesCollected3);
            buf.Write(LastTimeHelpMessage);
            buf.Write(CarsInSafeHouse.ToArray(), Limits.CarsPerSafeHouse * Limits.NumberOfSafeHouses);
            buf.Write(Garages.ToArray(), Limits.MaxNumGarages);

            // Game writes some garbage here due to incorrect size calculation
            buf.Skip(244);

            Debug.Assert(buf.Offset == SizeOf<GarageData>());
        }

        protected override int GetSize(DataFormat fmt)
        {
            return 0x156C;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GarageData);
        }

        public bool Equals(GarageData other)
        {
            if (other == null)
            {
                return false;
            }

            return NumGarages.Equals(other.NumGarages)
                && BombsAreFree.Equals(other.BombsAreFree)
                && RespraysAreFree.Equals(other.RespraysAreFree)
                && CarsCollected.Equals(other.CarsCollected)
                && BankVansCollected.Equals(other.BankVansCollected)
                && PoliceCarsCollected.Equals(other.PoliceCarsCollected)
                && CarTypesCollected1.Equals(other.CarTypesCollected1)
                && CarTypesCollected2.Equals(other.CarTypesCollected2)
                && CarTypesCollected3.Equals(other.CarTypesCollected3)
                && LastTimeHelpMessage.Equals(other.LastTimeHelpMessage)
                && CarsInSafeHouse.SequenceEqual(other.CarsInSafeHouse)
                && Garages.SequenceEqual(other.Garages);
        }
    }

    [Flags]
    public enum CollectCars1
    {
        Securicar = (1 << 0),
        Moonbeam = (1 << 1),
        Coach = (1 << 2),
        Flatbed = (1 << 3),
        Linerunner = (1 << 4),
        Trashmaster = (1 << 5),
        Patriot = (1 << 6),
        MrWhoopee = (1 << 7),
        Blista = (1 << 8),
        Mule = (1 << 9),
        Yankee = (1 << 10),
        Bobcat = (1 << 11),
        Dodo = (1 << 12),
        Bus = (1 << 13),
        Rumpo = (1 << 14),
        Pony = (1 << 15),
    }

    [Flags]
    public enum CollectCars2
    {
        Sentinel = (1 << 0),
        Cheetah = (1 << 1),
        Banshee = (1 << 2),
        Idaho = (1 << 3),
        Infernus = (1 << 4),
        Taxi = (1 << 5),
        Kuruma = (1 << 6),
        Stretch = (1 << 7),
        Perennial = (1 << 8),
        Stinger = (1 << 9),
        Manana = (1 << 10),
        Landstalker = (1 << 11),
        Stallion = (1 << 12),
        BFInjection = (1 << 13),
        Cabbie = (1 << 14),
        Esperanto = (1 << 15),
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
