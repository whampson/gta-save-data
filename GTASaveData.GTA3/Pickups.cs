using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x2514)]
    public class Pickups : GTAObject,
        IEquatable<Pickups>
    {
        public static class Limits
        {
            public const int NumberOfPickups = 336;
            public const int NumberOfPickupsCollected = 20;
        }

        private Array<Pickup> m_pickups;
        private int m_lastCollectedIndex;
        private Array<int> m_pickupsCollected;

        public Array<Pickup> PickupArray
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

        public Pickups()
        {
            m_pickups = new Array<Pickup>();
            m_pickupsCollected = new Array<int>();
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            m_pickups = buf.ReadArray<Pickup>(Limits.NumberOfPickups);
            m_lastCollectedIndex = buf.ReadUInt16();
            buf.ReadUInt16();
            m_pickupsCollected = buf.ReadArray<int>(Limits.NumberOfPickupsCollected);

            Debug.Assert(buf.Offset == SizeOf<Pickups>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(m_pickups.ToArray(), Limits.NumberOfPickups);
            buf.Write((ushort) m_lastCollectedIndex);
            buf.Write((ushort) 0);
            buf.Write(m_pickupsCollected.ToArray(), Limits.NumberOfPickupsCollected);

            Debug.Assert(buf.Offset == SizeOf<Pickups>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pickups);
        }

        public bool Equals(Pickups other)
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
