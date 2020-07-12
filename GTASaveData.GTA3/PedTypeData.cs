using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class PedTypeData : SaveDataObject,
        IEquatable<PedTypeData>, IDeepClonable<PedTypeData>
    {
        public const int NumPedTypes = 23;
        
        private Array<PedType> m_pedTypes;
        public Array<PedType> PedTypes
        {
            get { return m_pedTypes; }
            set { m_pedTypes = value; OnPropertyChanged(); }
        }

        public PedTypeData()
        {
            PedTypes = ArrayHelper.CreateArray<PedType>(NumPedTypes);
        }

        public PedTypeData(PedTypeData other)
        {
            PedTypes = ArrayHelper.DeepClone(other.PedTypes);
        }

        public PedTypeFlags GetFlag(PedTypeId type)
        {
            return m_pedTypes[(int) type].Flag;
        }

        public PedTypeFlags GetAvoid(PedTypeId type)
        {
            return m_pedTypes[(int) type].Avoid;
        }

        public PedTypeFlags GetThreats(PedTypeId type)
        {
            return m_pedTypes[(int) type].Threats;
        }

        public void SetThreats(PedTypeId type, PedTypeFlags threat)
        {
            m_pedTypes[(int) type].Threats = threat;
        }

        public void AddThreat(PedTypeId type, PedTypeFlags threat)
        {
            m_pedTypes[(int) type].Threats |= threat;
        }

        public void RemoveThreat(PedTypeId type, PedTypeFlags threat)
        {
            m_pedTypes[(int) type].Threats &= ~threat;
        }

        public bool IsThreat(PedTypeId type, PedTypeFlags threat)
        {
            return m_pedTypes[(int) type].Threats.HasFlag(threat);
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int size = GTA3VCSave.ReadBlockHeader(buf, "PTP");

            PedTypes = buf.Read<PedType>(NumPedTypes);

            Debug.Assert(buf.Offset == SizeOfType<PedTypeData>());
            Debug.Assert(size == SizeOfType<PedTypeData>() - GTA3VCSave.BlockHeaderSize);
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            GTA3VCSave.WriteBlockHeader(buf, "PTP", SizeOfType<PedTypeData>() - GTA3VCSave.BlockHeaderSize);

            buf.Write(PedTypes, NumPedTypes);

            Debug.Assert(buf.Offset == SizeOfType<PedTypeData>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x2E8;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PedTypeData);
        }

        public bool Equals(PedTypeData other)
        {
            if (other == null)
            {
                return false;
            }

            return PedTypes.SequenceEqual(other.PedTypes);
        }

        public PedTypeData DeepClone()
        {
            return new PedTypeData(this);
        }
    }
}
