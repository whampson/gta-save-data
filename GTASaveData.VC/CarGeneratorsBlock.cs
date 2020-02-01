using GTASaveData.Serialization;
using System;
using System.Linq;

namespace GTASaveData.VC
{
    public sealed class CarGeneratorsBlock : SaveDataObject,
        IEquatable<CarGeneratorsBlock>
    {
        public static class Limits
        {
            public const int CarGeneratorsCount = 185;
        }

        private int m_numberOfCarGenerators;
        private int m_numberOfActiveCarGenerators;
        private int m_processCounter;
        private int m_generateEvenIfPlayerIsCloseCounter;
        private StaticArray<CarGenerator> m_carGenerators;

        public int NumberOfCarGenerators
        {
            get { return m_numberOfCarGenerators; }
            set { m_numberOfCarGenerators = value; OnPropertyChanged(); }
        }

        public int NumberOfActiveCarGenerators
        {
            get { return m_numberOfActiveCarGenerators; }
            set { m_numberOfActiveCarGenerators = value; OnPropertyChanged(); }
        }

        public int ProcessCounter
        {
            get { return m_processCounter; }
            set { m_processCounter = value; OnPropertyChanged(); }
        }

        public int GenerateEvenIfPlayerIsCloseCounter
        {
            get { return m_generateEvenIfPlayerIsCloseCounter; }
            set { m_generateEvenIfPlayerIsCloseCounter = value; OnPropertyChanged(); }
        }

        public StaticArray<CarGenerator> CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; OnPropertyChanged(); }
        }

        public CarGeneratorsBlock()
        {
            m_carGenerators = new StaticArray<CarGenerator>(Limits.CarGeneratorsCount);
        }

        private CarGeneratorsBlock(SaveDataSerializer r, FileFormat fmt)
        {
            r.ReadInt32();
            m_numberOfCarGenerators = r.ReadInt32();
            m_numberOfActiveCarGenerators = r.ReadInt32();
            m_processCounter = r.ReadByte();
            m_generateEvenIfPlayerIsCloseCounter = r.ReadByte();
            r.Align();
            r.ReadInt32();
            m_carGenerators = r.ReadArray<CarGenerator>(Limits.CarGeneratorsCount);
        }

        protected override void WriteObjectData(SaveDataSerializer w, FileFormat fmt)
        {
            w.Write(0x0C);
            w.Write(m_numberOfCarGenerators);
            w.Write(m_numberOfActiveCarGenerators);
            w.Write((byte) m_processCounter);
            w.Write((byte) m_generateEvenIfPlayerIsCloseCounter);
            w.Align();
            w.Write(0x1FCC);
            w.WriteArray(m_carGenerators.ToArray(), Limits.CarGeneratorsCount);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CarGeneratorsBlock);
        }

        public bool Equals(CarGeneratorsBlock other)
        {
            if (other == null)
            {
                return false;
            }

            return m_numberOfCarGenerators.Equals(other.m_numberOfCarGenerators)
                && m_numberOfActiveCarGenerators.Equals(other.m_numberOfActiveCarGenerators)
                && m_processCounter.Equals(other.m_processCounter)
                && m_generateEvenIfPlayerIsCloseCounter.Equals(other.m_generateEvenIfPlayerIsCloseCounter)
                /*&& m_carGenerators.SequenceEqual(other.m_carGenerators)*/;
        }
    }
}
