using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x2D14)]
    public class TheCarGenerators : GTAObject,
        IEquatable<TheCarGenerators>
    {
        public static class Limits
        {
            public const int NumberOfCarGenerators = 160;
        }

        private int m_numberOfCarGenerators;
        private int m_currentActiveCount;
        private int m_processCounter;
        private int m_generateEvenIfPlayerIsCloseCounter;
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

        public Array<CarGenerator> CarGeneratorArray
        {
            get { return m_carGeneratorArray; }
            set { m_carGeneratorArray = value; OnPropertyChanged(); }
        }

        public TheCarGenerators()
        {
            m_carGeneratorArray = new Array<CarGenerator>();
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            int infoSize = buf.ReadInt32();
            Debug.Assert(infoSize == 0x0C);
            m_numberOfCarGenerators = buf.ReadInt32();
            m_currentActiveCount = buf.ReadInt32();
            m_processCounter = buf.ReadByte();
            m_generateEvenIfPlayerIsCloseCounter = buf.ReadByte();
            buf.ReadInt16();
            int carGensSize = buf.ReadInt32();
            Debug.Assert(carGensSize == 0x2D00);
            m_carGeneratorArray = buf.ReadArray<CarGenerator>(Limits.NumberOfCarGenerators);

            Debug.Assert(buf.Offset == SizeOf<TheCarGenerators>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(0x0C);
            buf.Write(m_numberOfCarGenerators);
            buf.Write(m_currentActiveCount);
            buf.Write((byte) m_processCounter);
            buf.Write((byte) m_generateEvenIfPlayerIsCloseCounter);
            buf.Write((short) 0);
            buf.Write(0x2D00);
            buf.Write(m_carGeneratorArray.ToArray(), Limits.NumberOfCarGenerators);
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

            return m_numberOfCarGenerators.Equals(other.m_numberOfCarGenerators)
                && m_currentActiveCount.Equals(other.m_currentActiveCount)
                && m_processCounter.Equals(other.m_processCounter)
                && m_generateEvenIfPlayerIsCloseCounter.Equals(other.m_generateEvenIfPlayerIsCloseCounter)
                && m_carGeneratorArray.SequenceEqual(other.m_carGeneratorArray);
        }
    }
}
