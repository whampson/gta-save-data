using GTASaveData.Types.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.LCS
{
    public class GarageData : SaveDataObject, IGarageData,
        IEquatable<GarageData>, IDeepClonable<GarageData>,
        IEnumerable<Garage>
    {
        public const int NumStoredCars = 48;
        public const int MaxNumGarages = 32;

        private int m_numGarages;
        private bool m_bombsAreFree;
        private bool m_respraysAreFree;
        private int m_carsCollected;        // not used
        private int m_bankVansCollected;    // not used
        private int m_policeCarsCollected;  // not used
        private LoveMediaCars m_carTypesCollected;
        private int m_carTypesCollected2;   // not used
        private int m_carTypesCollected3;   // not used
        private int m_carTypesCollected4;   // not used
        private int m_lastTimeHelpMessage;
        private Array<StoredCar> m_carsInSafeHouse;
        private Array<Garage> m_garages;

        public int NumGarages
        {
            get { return m_numGarages; }
            set { m_numGarages = value; OnPropertyChanged(); }
        }

        public bool FreeBombs
        {
            get { return m_bombsAreFree; }
            set { m_bombsAreFree = value; OnPropertyChanged(); }
        }

        public bool FreeResprays
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

        [Obsolete("Not used by the game.")]
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

        public LoveMediaCars CarTypesCollected
        {
            get { return m_carTypesCollected; }
            set { m_carTypesCollected = value; OnPropertyChanged(); }
        }

        [Obsolete("Not used by the game.")]
        public int CarTypesCollected2
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

        [Obsolete("Not used by the game.")]
        public int CarTypesCollected4
        {
            get { return m_carTypesCollected4; }
            set { m_carTypesCollected4 = value; OnPropertyChanged(); }
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

        public Garage this[int i]
        {
            get { return Garages[i]; }
            set { Garages[i] = value; OnPropertyChanged(); }
        }

        public IEnumerable<StoredCar> StoredCarsPortland 
        {
            get
            {
                for (int i = 0; i < NumStoredCars; i++)
                {
                    if ((i % 3) == 0) yield return CarsInSafeHouse[i];
                }
            }
        }

        public IEnumerable<StoredCar> StoredCarsStaunton
        {
            get
            {
                for (int i = 0; i < NumStoredCars; i++)
                {
                    if ((i % 3) == 1) yield return CarsInSafeHouse[i];
                }
            }
        }

        public IEnumerable<StoredCar> StoredCarsShoreside
        {
            get
            {
                for (int i = 0; i < NumStoredCars; i++)
                {
                    if ((i % 3) == 2) yield return CarsInSafeHouse[i];
                }
            }
        }

        IEnumerable<IStoredCar> IGarageData.CarsInSafeHouse => m_carsInSafeHouse;

        IEnumerable<IGarage> IGarageData.Garages => m_garages;

        public GarageData()
        {
            CarsInSafeHouse = ArrayHelper.CreateArray<StoredCar>(NumStoredCars);
            Garages = ArrayHelper.CreateArray<Garage>(MaxNumGarages);
        }

        public GarageData(GarageData other)
        {
            NumGarages = other.NumGarages;
            FreeBombs = other.FreeBombs;
            FreeResprays = other.FreeResprays;
            CarsCollected = other.CarsCollected;
            BankVansCollected = other.BankVansCollected;
            PoliceCarsCollected = other.PoliceCarsCollected;
            CarTypesCollected = other.CarTypesCollected;
            CarTypesCollected2 = other.CarTypesCollected2;
            CarTypesCollected3 = other.CarTypesCollected3;
            CarTypesCollected4 = other.CarTypesCollected4;
            LastTimeHelpMessage = other.LastTimeHelpMessage;
            CarsInSafeHouse = ArrayHelper.DeepClone(other.CarsInSafeHouse);
            Garages = ArrayHelper.DeepClone(other.Garages);
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            NumGarages = buf.ReadInt32();
            FreeBombs = buf.ReadBool(4);
            FreeResprays = buf.ReadBool(4);
            CarsCollected = buf.ReadInt32();
            BankVansCollected = buf.ReadInt32();
            PoliceCarsCollected = buf.ReadInt32();
            CarTypesCollected = (LoveMediaCars) buf.ReadInt32();
            CarTypesCollected2 = buf.ReadInt32();
            CarTypesCollected3 = buf.ReadInt32();
            CarTypesCollected4 = buf.ReadInt32();
            LastTimeHelpMessage = buf.ReadInt32();
            CarsInSafeHouse = buf.Read<StoredCar>(NumStoredCars);
            Garages = buf.Read<Garage>(MaxNumGarages, fmt);
            buf.Skip(344);

            Debug.Assert(buf.Offset == SizeOfType<GarageData>(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(NumGarages);
            buf.Write(FreeBombs, 4);
            buf.Write(FreeResprays, 4);
            buf.Write(CarsCollected);
            buf.Write(BankVansCollected);
            buf.Write(PoliceCarsCollected);
            buf.Write((int) CarTypesCollected);
            buf.Write(CarTypesCollected2);
            buf.Write(CarTypesCollected3);
            buf.Write(CarTypesCollected4);
            buf.Write(LastTimeHelpMessage);
            buf.Write(CarsInSafeHouse, NumStoredCars);
            buf.Write(Garages, fmt, MaxNumGarages);

            // Game writes some garbage here due to incorrect size calculation
            buf.Skip(344);

            Debug.Assert(buf.Offset == SizeOfType<GarageData>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPS2) return 0x29C4;
            if (fmt.IsPSP || fmt.IsMobile) return 0x25C4;
            throw SizeNotDefined(fmt);
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
                && FreeBombs.Equals(other.FreeBombs)
                && FreeResprays.Equals(other.FreeResprays)
                && CarsCollected.Equals(other.CarsCollected)
                && BankVansCollected.Equals(other.BankVansCollected)
                && PoliceCarsCollected.Equals(other.PoliceCarsCollected)
                && CarTypesCollected.Equals(other.CarTypesCollected)
                && CarTypesCollected2.Equals(other.CarTypesCollected2)
                && CarTypesCollected3.Equals(other.CarTypesCollected3)
                && CarTypesCollected4.Equals(other.CarTypesCollected4)
                && LastTimeHelpMessage.Equals(other.LastTimeHelpMessage)
                && CarsInSafeHouse.SequenceEqual(other.CarsInSafeHouse)
                && Garages.SequenceEqual(other.Garages);
        }

        public GarageData DeepClone()
        {
            return new GarageData(this);
        }

        public IEnumerator<Garage> GetEnumerator()
        {
            return Garages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Flags]
    public enum LoveMediaCars : int
    {
        Hearse = 1 << 0,
        Faggio = 1 << 1,
        Freeway = 1 << 2,
        DeimosSP = 1 << 3,
        Manana = 1 << 4,
        HellenbachGT = 1 << 5,
        PhobosVT = 1 << 6,
        V8Ghost = 1 << 7,
        ThunderRodd = 1 << 8,
        PCJ600 = 1 << 9,
        Sentinel = 1 << 10,
        Infernus = 1 << 11,
        Banshee = 1 << 12,
        Patriot = 1 << 13,
        BFInjection = 1 << 14,
        Landstalker = 1 << 15,
    }
}
#pragma warning restore CS0618 // Type or member is obsolete