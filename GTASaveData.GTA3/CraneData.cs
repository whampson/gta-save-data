﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class CraneData : SaveDataObject,
        IEquatable<CraneData>, IDeepClonable<CraneData>,
        IEnumerable<Crane>
    {
        public const int MaxNumCranes = 8;

        private int m_numCranes;
        private CollectCarsMilitaryCrane m_carsCollectedMilitaryCrane;
        private ObservableArray<Crane> m_cranes;

        public int NumCranes
        {
            get { return m_numCranes; }
            set { m_numCranes = value; }
        }

        public CollectCarsMilitaryCrane CarsCollectedMilitaryCrane
        {
            get { return m_carsCollectedMilitaryCrane; }
            set { m_carsCollectedMilitaryCrane = value; OnPropertyChanged(); }
        }

        public ObservableArray<Crane> Cranes
        {
            get { return m_cranes; }
            set { m_cranes = value; OnPropertyChanged(); }
        }

        public CraneData()
        {
            Cranes = ArrayHelper.CreateArray<Crane>(MaxNumCranes);
        }

        public CraneData(CraneData other)
        {
            NumCranes = other.NumCranes;
            CarsCollectedMilitaryCrane = other.CarsCollectedMilitaryCrane;
            Cranes = ArrayHelper.DeepClone(other.Cranes);
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            NumCranes = buf.ReadInt32();
            CarsCollectedMilitaryCrane = (CollectCarsMilitaryCrane) buf.ReadInt32();
            Cranes = buf.ReadArray<Crane>(MaxNumCranes);

            Debug.Assert(buf.Offset == SizeOf<CraneData>());
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write(NumCranes);
            buf.Write((int) CarsCollectedMilitaryCrane);
            buf.Write(Cranes, MaxNumCranes);

            Debug.Assert(buf.Offset == SizeOf<CraneData>());
        }

        protected override int GetSize(SerializationParams prm)
        {
            return 0x408;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CraneData);
        }

        public bool Equals(CraneData other)
        {
            if (other == null)
            {
                return false;
            }

            return NumCranes.Equals(other.NumCranes)
                && CarsCollectedMilitaryCrane.Equals(other.CarsCollectedMilitaryCrane)
                && Cranes.SequenceEqual(other.Cranes);
        }

        public CraneData DeepClone()
        {
            return new CraneData(this);
        }

        public IEnumerator<Crane> GetEnumerator()
        {
            return Cranes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public enum CraneState
    {
        Idle,
        GoingTowardsTarget,
        LiftingTarget,
        GoingTowardsTargetOnlyHeight,
        RotatingTarget,
        DroppingTarget
    }

    public enum CraneStatus
    {
        None,
        Activated,
        Deactivated
    }

    [Flags]
    public enum CollectCarsMilitaryCrane
    {
        Firetruck = (1 << 0),
        Ambulance = (1 << 1),
        Enforcer = (1 << 2),
        FbiCar = (1 << 3),
        Rhino = (1 << 4),
        BarracksOL = (1 << 5),
        Police = (1 << 6),
    }
}
