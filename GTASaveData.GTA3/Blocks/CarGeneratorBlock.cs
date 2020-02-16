using GTASaveData.Common.Blocks;
using GTASaveData.Serialization;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3.Blocks
{
    public class CarGeneratorBlock : SerializableObject,
        ICarGeneratorBlock<CarGenerator>,
        IEquatable<CarGeneratorBlock>
    {
        public static class Limits
        {
            public const int CarGeneratorsCount = 160;
        }

        private int m_numberOfCarGenerators;
        private int m_numberOfActiveCarGenerators;
        private int m_processCounter;
        private int m_generateEvenIfPlayerIsCloseCounter;
        private Array<CarGenerator> m_parkedCars;

        public int TotalNumberOfCarGenerators
        {
            get { return m_numberOfCarGenerators; }
            set { m_numberOfCarGenerators = value; OnPropertyChanged(); }
        }

        public int NumberOfParkedCarsToGenerate
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

        public Array<CarGenerator> ParkedCars
        {
            get { return m_parkedCars; }
            set { m_parkedCars = value; OnPropertyChanged(); }
        }
        public CarGeneratorBlock()
        {
            m_parkedCars = new Array<CarGenerator>();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            int constant0Ch = r.ReadInt32();
            Debug.Assert(constant0Ch == 0x0C);
            m_numberOfCarGenerators = r.ReadInt32();
            m_numberOfActiveCarGenerators = r.ReadInt32();
            m_processCounter = r.ReadByte();
            m_generateEvenIfPlayerIsCloseCounter = r.ReadByte();
            r.Align();
            int constant2D00h = r.ReadInt32();
            Debug.Assert(constant2D00h == 0x2D00);
            m_parkedCars = r.ReadArray<CarGenerator>(Limits.CarGeneratorsCount);
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(0x0C);
            w.Write(m_numberOfCarGenerators);
            w.Write(m_numberOfActiveCarGenerators);
            w.Write((byte) m_processCounter);
            w.Write((byte) m_generateEvenIfPlayerIsCloseCounter);
            w.Align();
            w.Write(0x2D00);
            w.Write(m_parkedCars.ToArray(), Limits.CarGeneratorsCount);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CarGeneratorBlock);
        }

        public bool Equals(CarGeneratorBlock other)
        {
            if (other == null)
            {
                return false;
            }

            return m_numberOfCarGenerators.Equals(other.m_numberOfCarGenerators)
                && m_numberOfActiveCarGenerators.Equals(other.m_numberOfActiveCarGenerators)
                && m_processCounter.Equals(other.m_processCounter)
                && m_generateEvenIfPlayerIsCloseCounter.Equals(other.m_generateEvenIfPlayerIsCloseCounter)
                && m_parkedCars.SequenceEqual(other.m_parkedCars);
        }
    }
}
