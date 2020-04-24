using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x2E8)]
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

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "PTP");

            PedTypes = buf.Read<PedType>(Limits.NumberOfPedTypes);

            Debug.Assert(buf.Offset == SizeOf<PedTypeData>());
            Debug.Assert(size == SizeOf<PedTypeData>() - GTA3Save.SaveHeaderSize);
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            GTA3Save.WriteSaveHeader(buf, "PTP", SizeOf<PedTypeData>() - GTA3Save.SaveHeaderSize);

            buf.Write(PedTypes.ToArray(), Limits.NumberOfPedTypes);

            Debug.Assert(buf.Offset == SizeOf<PedTypeData>());
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
