using GTASaveData.Serialization;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WpfEssentials;

namespace GTASaveData.GTA3
{
    public class Pickups : SerializableObject,
        IEquatable<Pickups>
    {
        public static class Limits
        {
            public const int PickupsArrayCount = 336;
            public const int PickupsCollectedCount = 20;
        }

        private Array<Pickup> m_pickupsArray;
        private int m_lastCollectedIndex;
        private Array<int> m_pickupsCollected;

        public Array<Pickup> PickupsArray
        {
            get { return m_pickupsArray; }
            set { m_pickupsArray = value; OnPropertyChanged(); }
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
            m_pickupsArray = new Array<Pickup>();
            m_pickupsCollected = new Array<int>();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_pickupsArray = r.ReadArray<Pickup>(Limits.PickupsArrayCount);
            m_lastCollectedIndex = r.ReadUInt16();
            ushort constant0 = r.ReadUInt16();
            Debug.Assert(constant0 == 0);
            m_pickupsCollected = r.ReadArray<int>(Limits.PickupsCollectedCount);
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_pickupsArray.ToArray(), Limits.PickupsArrayCount);
            w.Write((ushort) m_lastCollectedIndex);
            w.Write((ushort) 0);
            w.Write(m_pickupsCollected.ToArray(), Limits.PickupsCollectedCount);
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

            return m_pickupsArray.SequenceEqual(other.m_pickupsArray)
                && m_lastCollectedIndex.Equals(other.m_lastCollectedIndex)
                && m_pickupsCollected.SequenceEqual(other.m_pickupsCollected);
        }
    }
}
