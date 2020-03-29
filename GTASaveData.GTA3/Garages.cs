﻿using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x1478)]
    public class Garages : SaveDataObject, IEquatable<Garages>
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
            StoredCars = new Array<StoredCar>();
            GarageArray = new Array<Garage>();
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            NumberOfGarages = buf.ReadInt32();
            BombsAreFree = buf.ReadBool(4);
            RespraysAreFree = buf.ReadBool(4);
            CarsCollected = buf.ReadInt32();
            BankVansCollected = buf.ReadInt32();
            PoliceCarsCollected = buf.ReadInt32();
            CarTypesCollected1 = (CollectCars1) buf.ReadInt32();
            CarTypesCollected2 = (CollectCars2) buf.ReadInt32();
            CarTypesCollected3 = (CollectCars3) buf.ReadInt32();
            LastTimeHelpMessage = buf.ReadInt32();
            StoredCars = buf.ReadArray<StoredCar>(Limits.NumberOfStoredCars);
            GarageArray = buf.ReadArray<Garage>(Limits.NumberOfGarages);

            Debug.Assert(buf.Offset == SizeOf<Garages>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(NumberOfGarages);
            buf.Write(BombsAreFree, 4);
            buf.Write(RespraysAreFree, 4);
            buf.Write(CarsCollected);
            buf.Write(BankVansCollected);
            buf.Write(PoliceCarsCollected);
            buf.Write((int) CarTypesCollected1);
            buf.Write((int) CarTypesCollected2);
            buf.Write((int) CarTypesCollected3);
            buf.Write(LastTimeHelpMessage);
            buf.Write(StoredCars.ToArray(), Limits.NumberOfStoredCars);
            buf.Write(GarageArray.ToArray(), Limits.NumberOfGarages);

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

            return NumberOfGarages.Equals(other.NumberOfGarages)
                && BombsAreFree.Equals(other.BombsAreFree)
                && RespraysAreFree.Equals(other.RespraysAreFree)
                && CarsCollected.Equals(other.CarsCollected)
                && BankVansCollected.Equals(other.BankVansCollected)
                && PoliceCarsCollected.Equals(other.PoliceCarsCollected)
                && CarTypesCollected1.Equals(other.CarTypesCollected1)
                && CarTypesCollected2.Equals(other.CarTypesCollected2)
                && CarTypesCollected3.Equals(other.CarTypesCollected3)
                && LastTimeHelpMessage.Equals(other.LastTimeHelpMessage)
                && StoredCars.SequenceEqual(other.StoredCars)
                && GarageArray.SequenceEqual(other.GarageArray);
        }
    }
}
