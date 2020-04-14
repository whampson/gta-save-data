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
            public const int NumberOfCollectedPickups = 20;
        }

        private Array<Pickup> m_pickUps;
        private short m_collectedPickupIndex;
        private Array<int> m_pickUpsCollected;

        public Array<Pickup> PickupsArray
        {
            get { return m_pickUps; }
            set { m_pickUps = value; OnPropertyChanged(); }
        }

        public short LastCollectedIndex
        {
            get { return m_collectedPickupIndex; }
            set { m_collectedPickupIndex = value; OnPropertyChanged(); }
        }

        public Array<int> PickupsCollected
        {
            get { return m_pickUpsCollected; }
            set { m_pickUpsCollected = value; OnPropertyChanged(); }
        }

        public Pickups()
        {
            PickupsArray = new Array<Pickup>();
            PickupsCollected = new Array<int>();
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            PickupsArray = buf.ReadArray<Pickup>(Limits.NumberOfPickups);
            LastCollectedIndex = buf.ReadInt16();
            buf.ReadInt16();
            PickupsCollected = buf.ReadArray<int>(Limits.NumberOfCollectedPickups);

            Debug.Assert(buf.Offset == SizeOf<Pickups>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(PickupsArray.ToArray(), Limits.NumberOfPickups);
            buf.Write(LastCollectedIndex);
            buf.Write((short) 0);
            buf.Write(PickupsCollected.ToArray(), Limits.NumberOfCollectedPickups);

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

            return PickupsArray.SequenceEqual(other.PickupsArray)
                && LastCollectedIndex.Equals(other.LastCollectedIndex)
                && PickupsCollected.SequenceEqual(other.PickupsCollected);
        }
    }
}
