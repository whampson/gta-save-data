using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.VC
{
    public class CarGeneratorData : SaveDataObject, ICarGeneratorData,
        IEquatable<CarGeneratorData>, IDeepClonable<CarGeneratorData>,
        IEnumerable<CarGenerator>
    {
        public const int MaxNumCarGenerators = 185;

        private const int CarGeneratorDataSize = 12;
        private const int CarGeneratorArraySize = 0x1FCC;

        private int m_numberOfCarGenerators;
        private int m_currentActiveCount;
        private byte m_processCounter;
        private byte m_generateEvenIfPlayerIsCloseCounter;
        private ObservableArray<CarGenerator> m_carGeneratorArray;

        public int NumberOfCarGenerators
        {
            get { return m_numberOfCarGenerators; }
            set { m_numberOfCarGenerators = value; OnPropertyChanged(); }
        }

        public int NumberOfEnabledCarGenerators
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

        public ObservableArray<CarGenerator> CarGenerators
        {
            get { return m_carGeneratorArray; }
            set { m_carGeneratorArray = value; OnPropertyChanged(); }
        }

        public CarGenerator this[int i]
        {
            get { return CarGenerators[i]; }
            set { CarGenerators[i] = value; OnPropertyChanged(); }
        }

        ICarGenerator ICarGeneratorData.this[int index]
        {
            get { return CarGenerators[index]; }
            set { CarGenerators[index] = (CarGenerator) value; OnPropertyChanged(); }
        }

        IEnumerable<ICarGenerator> ICarGeneratorData.CarGenerators => m_carGeneratorArray;

        public CarGeneratorData()
        {
            CarGenerators = ArrayHelper.CreateArray<CarGenerator>(MaxNumCarGenerators);
        }

        public CarGeneratorData(CarGeneratorData other)
        {
            NumberOfCarGenerators = other.NumberOfCarGenerators;
            NumberOfEnabledCarGenerators = other.NumberOfEnabledCarGenerators;
            ProcessCounter = other.ProcessCounter;
            GenerateEvenIfPlayerIsCloseCounter = other.GenerateEvenIfPlayerIsCloseCounter;
            CarGenerators = ArrayHelper.DeepClone(other.CarGenerators);
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            int size = SaveFileGTA3VC.ReadBlockHeader(buf, "CGN");

            int infoSize = buf.ReadInt32();
            Debug.Assert(infoSize == CarGeneratorDataSize);
            NumberOfCarGenerators = buf.ReadInt32();
            NumberOfEnabledCarGenerators = buf.ReadInt32();
            ProcessCounter = buf.ReadByte();
            GenerateEvenIfPlayerIsCloseCounter = buf.ReadByte();
            buf.ReadInt16();
            int carGensSize = buf.ReadInt32();
            Debug.Assert(carGensSize == CarGeneratorArraySize);
            CarGenerators = buf.ReadArray<CarGenerator>(MaxNumCarGenerators);

            Debug.Assert(buf.Offset == GetSize(fmt));
            Debug.Assert(size == GetSize(fmt) - SaveFileGTA3VC.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            SaveFileGTA3VC.WriteBlockHeader(buf, "CGN", SizeOf<CarGeneratorData>() - SaveFileGTA3VC.BlockHeaderSize);

            buf.Write(CarGeneratorDataSize);
            buf.Write(NumberOfCarGenerators);
            buf.Write(NumberOfEnabledCarGenerators);
            buf.Write(ProcessCounter);
            buf.Write(GenerateEvenIfPlayerIsCloseCounter);
            buf.Write((short) 0);
            buf.Write(CarGeneratorArraySize);
            buf.Write(CarGenerators, MaxNumCarGenerators);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x1FE8;
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
                && NumberOfEnabledCarGenerators.Equals(other.NumberOfEnabledCarGenerators)
                && ProcessCounter.Equals(other.ProcessCounter)
                && GenerateEvenIfPlayerIsCloseCounter.Equals(other.GenerateEvenIfPlayerIsCloseCounter)
                && CarGenerators.SequenceEqual(other.CarGenerators);
        }

        public CarGeneratorData DeepClone()
        {
            return new CarGeneratorData(this);
        }

        public IEnumerator<CarGenerator> GetEnumerator()
        {
            return CarGenerators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
