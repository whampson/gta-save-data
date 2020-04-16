using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x408)]
    public class CraneData : SaveDataObject, IEquatable<CraneData>
    {
        public static class Limits
        {
            public const int NumberOfCranes = 8;
        }

        private int m_numCranes;
        private CollectCarsMilitaryCrane m_carsCollectedMilitaryCrane;
        private Array<Crane> m_cranes;

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

        public Array<Crane> Cranes
        {
            get { return m_cranes; }
            set { m_cranes = value; OnPropertyChanged(); }
        }

        public CraneData()
        {
            Cranes = new Array<Crane>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            NumCranes = buf.ReadInt32();
            CarsCollectedMilitaryCrane = (CollectCarsMilitaryCrane) buf.ReadInt32();
            Cranes = buf.ReadArray<Crane>(Limits.NumberOfCranes);

            Debug.Assert(buf.Offset == SizeOf<CraneData>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(NumCranes);
            buf.Write((int) CarsCollectedMilitaryCrane);
            buf.Write(Cranes.ToArray(), Limits.NumberOfCranes);

            Debug.Assert(buf.Offset == SizeOf<CraneData>());
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
    }
}
