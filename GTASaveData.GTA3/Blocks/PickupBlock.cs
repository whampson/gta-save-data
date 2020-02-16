using GTASaveData.Serialization;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3.Blocks
{
    public class PickupBlock : SerializableObject,
        IEquatable<PickupBlock>
    {
        public static class Limits
        {
            public const int PickupsCount = 336;
            public const int PickupsCollectedCount = 20;
        }

        private Array<Pickup> m_pickups;
        private int m_lastCollectedIndex;
        private Array<int> m_pickupsCollected;

        public Array<Pickup> Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
        }

        public int LastCollectedIndex
        {
            get { return m_lastCollectedIndex; }
            set { m_lastCollectedIndex = value; OnPropertyChanged(); }
        }

        public Array<int> PickupsCollected
        {
            get { return m_pickupsCollected; }
            set { m_pickupsCollected = value; OnPropertyChanged(); }
        }

        public PickupBlock()
        {
            m_pickups = new Array<Pickup>();
            m_pickupsCollected = new Array<int>();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_pickups = r.ReadArray<Pickup>(Limits.PickupsCount);
            m_lastCollectedIndex = r.ReadUInt16();
            ushort constant0 = r.ReadUInt16();
            Debug.Assert(constant0 == 0);
            m_pickupsCollected = r.ReadArray<int>(Limits.PickupsCollectedCount);
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_pickups.ToArray(), Limits.PickupsCount);
            w.Write((ushort) m_lastCollectedIndex);
            w.Write((ushort) 0);
            w.Write(m_pickupsCollected.ToArray(), Limits.PickupsCollectedCount);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PickupBlock);
        }

        public bool Equals(PickupBlock other)
        {
            if (other == null)
            {
                return false;
            }

            return m_pickups.SequenceEqual(other.m_pickups)
                && m_lastCollectedIndex.Equals(other.m_lastCollectedIndex)
                && m_pickupsCollected.SequenceEqual(other.m_pickupsCollected);
        }
    }
}
