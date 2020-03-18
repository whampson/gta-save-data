using GTASaveData.Serialization;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x2514)]
    public class Pickups : SerializableObject,
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

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_pickups = r.ReadArray<Pickup>(Limits.NumberOfPickups);
            m_lastCollectedIndex = r.ReadUInt16();
            r.ReadUInt16();
            m_pickupsCollected = r.ReadArray<int>(Limits.NumberOfPickupsCollected);

            Debug.Assert(r.Position() - r.Marked() == SizeOf<Pickups>());
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_pickups.ToArray(), Limits.NumberOfPickups);
            w.Write((ushort) m_lastCollectedIndex);
            w.Write((ushort) 0);
            w.Write(m_pickupsCollected.ToArray(), Limits.NumberOfPickupsCollected);

            Debug.Assert(w.Position() - w.Marked() == SizeOf<Pickups>());
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
