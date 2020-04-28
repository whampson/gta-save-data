using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class CarGeneratorData : SaveDataObject, IEquatable<CarGeneratorData>
    {
        public static class Limits
        {
            public const int MaxNumCarGenerators = 160;
        }

        private const int CarGeneratorDataSize = 12;
        private const int CarGeneratorArraySize = 0x2D00;

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

        public Array<CarGenerator> CarGenerators
        {
            get { return m_carGeneratorArray; }
            set { m_carGeneratorArray = value; OnPropertyChanged(); }
        }

        public CarGeneratorData()
        {
            CarGenerators = new Array<CarGenerator>();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "CGN");

            int infoSize = buf.ReadInt32();
            Debug.Assert(infoSize == CarGeneratorDataSize);
            NumberOfCarGenerators = buf.ReadInt32();
            CurrentActiveCount = buf.ReadInt32();
            ProcessCounter = buf.ReadByte();
            GenerateEvenIfPlayerIsCloseCounter = buf.ReadByte();
            buf.ReadInt16();
            int carGensSize = buf.ReadInt32();
            Debug.Assert(carGensSize == CarGeneratorArraySize);
            CarGenerators = buf.Read<CarGenerator>(Limits.MaxNumCarGenerators);

            Debug.Assert(buf.Offset == SizeOf<CarGeneratorData>());
            Debug.Assert(size == SizeOf<CarGeneratorData>() - GTA3Save.SaveHeaderSize);
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            GTA3Save.WriteSaveHeader(buf, "CGN", SizeOf<CarGeneratorData>() - GTA3Save.SaveHeaderSize);

            buf.Write(CarGeneratorDataSize);
            buf.Write(NumberOfCarGenerators);
            buf.Write(CurrentActiveCount);
            buf.Write(ProcessCounter);
            buf.Write(GenerateEvenIfPlayerIsCloseCounter);
            buf.Write((short) 0);
            buf.Write(CarGeneratorArraySize);
            buf.Write(CarGenerators.ToArray(), Limits.MaxNumCarGenerators);

            Debug.Assert(buf.Offset == SizeOf<CarGeneratorData>());
        }

        protected override int GetSize(DataFormat fmt)
        {
            return 0x2D1C;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CarGeneratorData);
        }

        public bool Equals(CarGeneratorData other)
        {
            if (other == null)
            {
                return false;
            }

            return NumberOfCarGenerators.Equals(other.NumberOfCarGenerators)
                && CurrentActiveCount.Equals(other.CurrentActiveCount)
                && ProcessCounter.Equals(other.ProcessCounter)
                && GenerateEvenIfPlayerIsCloseCounter.Equals(other.GenerateEvenIfPlayerIsCloseCounter)
                && CarGenerators.SequenceEqual(other.CarGenerators);
        }
    }
}
