using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class GangData : SaveDataObject,
        IEquatable<GangData>, IDeepClonable<GangData>,
        IEnumerable<Gang>
    {
        public const int MaxNumGangs = 9;

        private ObservableArray<Gang> m_gangs;

        public ObservableArray<Gang> Gangs
        {
            get { return m_gangs; }
            set { m_gangs = value;OnPropertyChanged(); }
        }

        public Gang this[int i]
        {
            get { return Gangs[i]; }
            //set { Gangs[i] = value; OnPropertyChanged(); }
        }

        public Gang this[GangType g]
        {
            get { return Gangs[(int) g]; }
            //set { Gangs[(int) g] = value; OnPropertyChanged(); }
        }

        public GangData()
        {
            Gangs = ArrayHelper.CreateArray<Gang>(MaxNumGangs);
        }

        public GangData(GangData other)
        {
            Gangs = ArrayHelper.DeepClone(other.Gangs);
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            int size = GTA3Save.ReadBlockHeader(buf, out string tag);
            Debug.Assert(tag == "GNG");

            Gangs = buf.ReadArray<Gang>(MaxNumGangs);

            Debug.Assert(buf.Offset == SizeOf<GangData>());
            Debug.Assert(size == SizeOf<GangData>() - GTA3Save.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            GTA3Save.WriteBlockHeader(buf, "GNG", SizeOf<GangData>() - GTA3Save.BlockHeaderSize);
            buf.Write(Gangs, MaxNumGangs);

            Debug.Assert(buf.Offset == SizeOf<GangData>());
        }

        protected override int GetSize(SerializationParams prm)
        {
            return 0x98;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GangData);
        }

        public bool Equals(GangData other)
        {
            if (other == null)
            {
                return false;
            }

            return Gangs.SequenceEqual(other.Gangs);
        }

        public GangData DeepClone()
        {
            return new GangData(this);
        }

        public IEnumerator<Gang> GetEnumerator()
        {
            return Gangs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public enum GangType
    {
        Mafia,
        Triads,
        Diablos,
        Yakuza,
        Yardies,
        Cartel,
        Hoods,
        Gang8,
        Gang9
    }
}
