using System;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class PedTypeData : SaveDataObject,
        IEquatable<PedTypeData>, IDeepClonable<PedTypeData>
    {
        public const int NumPedTypes = 23;
        
        private ObservableArray<PedType> m_pedTypes;
        public ObservableArray<PedType> PedTypes
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

        public PedType GetPedInfo(PedTypeId type)
        {
            return IsPedTypeValid(type) ? m_pedTypes[(int) type] : null;
        }

        public PedTypeFlags GetFlag(PedTypeId type)
        {
            return IsPedTypeValid(type) ? m_pedTypes[(int) type].Flag : 0;
        }

        public PedTypeFlags GetThreats(PedTypeId type)
        {
            return IsPedTypeValid(type) ? m_pedTypes[(int) type].Threats : 0;
        }

        public void SetThreat(PedTypeId type, PedTypeFlags threat)
        {
            if (IsPedTypeValid(type))
            {
                m_pedTypes[(int) type].Threats = threat;
            }
        }

        public void AddThreat(PedTypeId type, PedTypeFlags threat)
        {
            if (IsPedTypeValid(type))
            {
                m_pedTypes[(int) type].Threats |= threat;
            }
        }

        public void RemoveThreat(PedTypeId type, PedTypeFlags threat)
        {
            if (IsPedTypeValid(type))
            {
                m_pedTypes[(int) type].Threats &= ~threat;
            }
        }

        public bool IsThreat(PedTypeId type, PedTypeFlags threat)
        {
            return IsPedTypeValid(type) ? m_pedTypes[(int) type].Threats.HasFlag(threat) : false;
        }

        public PedTypeFlags GetAvoids(PedTypeId type)
        {
            return IsPedTypeValid(type) ? m_pedTypes[(int) type].Avoids : 0;
        }

        public void SetAvoid(PedTypeId type, PedTypeFlags avoid)
        {
            if (IsPedTypeValid(type))
            {
                m_pedTypes[(int) type].Avoids = avoid;
            }
        }

        public void AddAvoid(PedTypeId type, PedTypeFlags avoid)
        {
            if (IsPedTypeValid(type))
            {
                m_pedTypes[(int) type].Avoids |= avoid;
            }
        }

        public void RemoveAvoid(PedTypeId type, PedTypeFlags avoid)
        {
            if (IsPedTypeValid(type))
            {
                m_pedTypes[(int) type].Avoids &= ~avoid;
            }
        }

        public bool IsAvoid(PedTypeId type, PedTypeFlags threat)
        {
            return IsPedTypeValid(type) ? m_pedTypes[(int) type].Avoids.HasFlag(threat) : false;
        }

        private bool IsPedTypeValid(PedTypeId type)
        {
            return type >= PedTypeId.Player1 && type <= PedTypeId.Unused2;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            int size = SaveFileGTA3VC.ReadBlockHeader(buf, "PTP");

            PedTypes = buf.ReadArray<PedType>(NumPedTypes);

            Debug.Assert(buf.Offset == SizeOfType<PedTypeData>());
            Debug.Assert(size == SizeOfType<PedTypeData>() - SaveFileGTA3VC.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            SaveFileGTA3VC.WriteBlockHeader(buf, "PTP", SizeOfType<PedTypeData>() - SaveFileGTA3VC.BlockHeaderSize);

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
