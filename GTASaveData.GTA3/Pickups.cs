using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x2514)]
    public class Pickups : SaveDataObject, IEquatable<Pickups>
    {
        public static class Limits
        {
            public const int NumberOfPickups = 336;
            public const int NumberOfPickupsCollected = 20;
        }

        private Array<Pickup> m_pickups;
        private short m_lastCollectedIndex;
        private Array<int> m_pickupsCollected;

        public Array<Pickup> PickupArray
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
        }

        public short LastCollectedIndex
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
            PickupArray = new Array<Pickup>();
            PickupsCollected = new Array<int>();
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            PickupArray = buf.ReadArray<Pickup>(Limits.NumberOfPickups);
            LastCollectedIndex = buf.ReadInt16();
            buf.ReadInt16();
            PickupsCollected = buf.ReadArray<int>(Limits.NumberOfPickupsCollected);

            Debug.Assert(buf.Offset == SizeOf<Pickups>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(PickupArray.ToArray(), Limits.NumberOfPickups);
            buf.Write(LastCollectedIndex);
            buf.Write((short) 0);
            buf.Write(PickupsCollected.ToArray(), Limits.NumberOfPickupsCollected);

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

            return PickupArray.SequenceEqual(other.PickupArray)
                && LastCollectedIndex.Equals(other.LastCollectedIndex)
                && PickupsCollected.SequenceEqual(other.PickupsCollected);
        }
    }
}
