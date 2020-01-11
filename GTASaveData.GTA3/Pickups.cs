using GTASaveData.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WpfEssentials;

namespace GTASaveData.GTA3
{
    public sealed class Pickups : SaveDataObject,
        IEquatable<Pickups>
    {
        public static class Limits
        {
            public const int PickupsArrayCount = 336;
            public const int PickupsCollectedCount = 20;
        }

        private FullyObservableCollection<Pickup> m_pickupsArray;
        private int m_lastCollectedIndex;
        private ObservableCollection<int> m_pickupsCollected;

        public FullyObservableCollection<Pickup> PickupsArray
        {
            get { return m_pickupsArray; }
            set { m_pickupsArray = value; OnPropertyChanged(); }
        }

        public int LastCollectedIndex
        {
            get { return m_lastCollectedIndex; }
            set { m_lastCollectedIndex = value; OnPropertyChanged(); }
        }

        public ObservableCollection<int> PickupsCollected
        {
            get { return m_pickupsCollected; }
            set { m_pickupsCollected = value; OnPropertyChanged(); }
        }

        public Pickups()
        {
            m_pickupsArray = new FullyObservableCollection<Pickup>();
            m_pickupsCollected = new ObservableCollection<int>();
        }

        private Pickups(SaveDataSerializer serializer, FileFormat format)
        {
            m_pickupsArray = new FullyObservableCollection<Pickup>(serializer.ReadArray<Pickup>(Limits.PickupsArrayCount));
            m_lastCollectedIndex = serializer.ReadUInt16();
            ushort constant0 = serializer.ReadUInt16();
            Debug.Assert(constant0 == 0);
            m_pickupsCollected = new ObservableCollection<int>(serializer.ReadArray<int>(Limits.PickupsCollectedCount));
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.WriteArray(m_pickupsArray.ToArray(), Limits.PickupsArrayCount);
            serializer.Write((ushort) m_lastCollectedIndex);
            serializer.Write((ushort) 0);
            serializer.WriteArray(m_pickupsCollected.ToArray(), Limits.PickupsCollectedCount);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
