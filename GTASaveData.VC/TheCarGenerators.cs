using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.VC
{
    [Size(0x1FE0)]
    public class TheCarGenerators : SaveDataObject, IEquatable<TheCarGenerators>
    {
        public static class Limits
        {
            public const int NumberOfCarGenerators = 185;
        }

        private const int SizeOfCarGeneratorData = 12;
        private const int SizeOfCarGeneratorArray = 0x1FCC;

        private int m_numberOfCarGenerators;
        private int m_currentActiveCount;
        private byte m_processCounter;
        private byte m_generateEvenIfPlayerIsCloseCounter;
        private Array<CarGenerator> m_carGeneratorArray;

        public int NumberOfCarGenerators
        {
            get { return m_numberOfCarGenerators; }
            set { m_numberOfCarGenerators = value; OnPropertyChanged(); }
        }

        public int CurrentActiveCount
        {
            get { return m_currentActiveCount; }
            set { m_currentActiveCount = value; OnPropertyChanged(); }
        }

        public byte ProcessCounter
        {
            get { return m_processCounter; }
            set { m_processCounter = value; OnPropertyChanged(); }
        }

        public byte GenerateEvenIfPlayerIsCloseCounter
        {
            get { return m_generateEvenIfPlayerIsCloseCounter; }
            set { m_generateEvenIfPlayerIsCloseCounter = value; OnPropertyChanged(); }
        }

        public Array<CarGenerator> CarGeneratorArray
        {
            get { return m_carGeneratorArray; }
            set { m_carGeneratorArray = value; OnPropertyChanged(); }
        }

        public TheCarGenerators()
        {
            CarGeneratorArray = new Array<CarGenerator>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int infoSize = buf.ReadInt32();
            Debug.Assert(infoSize == SizeOfCarGeneratorData);
            NumberOfCarGenerators = buf.ReadInt32();
            CurrentActiveCount = buf.ReadInt32();
            ProcessCounter = buf.ReadByte();
            GenerateEvenIfPlayerIsCloseCounter = buf.ReadByte();
            buf.ReadInt16();
            int carGensSize = buf.ReadInt32();
            Debug.Assert(carGensSize == SizeOfCarGeneratorArray);
            CarGeneratorArray = buf.ReadArray<CarGenerator>(Limits.NumberOfCarGenerators);

            Debug.Assert(buf.Offset == SizeOf<TheCarGenerators>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(SizeOfCarGeneratorData);
            buf.Write(NumberOfCarGenerators);
            buf.Write(CurrentActiveCount);
            buf.Write(ProcessCounter);
            buf.Write(GenerateEvenIfPlayerIsCloseCounter);
            buf.Write((short) 0);
            buf.Write(SizeOfCarGeneratorArray);
            buf.Write(CarGeneratorArray.ToArray(), Limits.NumberOfCarGenerators);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TheCarGenerators);
        }

        public bool Equals(TheCarGenerators other)
        {
            if (other == null)
            {
                return false;
            }

            return NumberOfCarGenerators.Equals(other.NumberOfCarGenerators)
                && CurrentActiveCount.Equals(other.CurrentActiveCount)
                && ProcessCounter.Equals(other.ProcessCounter)
                && GenerateEvenIfPlayerIsCloseCounter.Equals(other.GenerateEvenIfPlayerIsCloseCounter)
                && CarGeneratorArray.SequenceEqual(other.CarGeneratorArray);
        }
    }
}
