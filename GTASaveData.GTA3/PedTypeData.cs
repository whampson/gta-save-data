using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class PedTypeData : SaveDataObject, IEquatable<PedTypeData>
    {
        public static class Limits
        {
            public const int NumberOfPedTypes = 23;
        }

        private Array<PedType> m_pedTypes;
        public Array<PedType> PedTypes
        {
            get { return m_pedTypes; }
            set { m_pedTypes = value; OnPropertyChanged(); }
        }

        public PedTypeData()
        {
            PedTypes = new Array<PedType>();
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

        protected override void ReadData(StreamBuffer buf, SaveDataFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "PTP");

            PedTypes = buf.Read<PedType>(Limits.NumberOfPedTypes);

            Debug.Assert(buf.Offset == SizeOf<PedTypeData>());
            Debug.Assert(size == SizeOf<PedTypeData>() - GTA3Save.SaveHeaderSize);
        }

        protected override void WriteData(StreamBuffer buf, SaveDataFormat fmt)
        {
            GTA3Save.WriteSaveHeader(buf, "PTP", SizeOf<PedTypeData>() - GTA3Save.SaveHeaderSize);

            buf.Write(PedTypes.ToArray(), Limits.NumberOfPedTypes);

            Debug.Assert(buf.Offset == SizeOf<PedTypeData>());
        }

        protected override int GetSize(SaveDataFormat fmt)
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
    }
}
