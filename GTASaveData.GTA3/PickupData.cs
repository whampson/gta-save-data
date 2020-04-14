using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x2514)]
    public class PickupData : SaveDataObject, IEquatable<PickupData>
    {
        public static class Limits
        {
            public const int NumberOfPickups = 336;
            public const int NumberOfCollectedPickups = 20;
        }

        private Array<Pickup> m_pickUps;
        private short m_collectedPickupIndex;
        private Array<int> m_pickUpsCollected;

        public Array<Pickup> Pickups
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

        public PickupData()
        {
            Pickups = new Array<Pickup>();
            PickupsCollected = new Array<int>();
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            Pickups = buf.ReadArray<Pickup>(Limits.NumberOfPickups);
            LastCollectedIndex = buf.ReadInt16();
            buf.ReadInt16();
            PickupsCollected = buf.ReadArray<int>(Limits.NumberOfCollectedPickups);

            Debug.Assert(buf.Offset == SizeOf<PickupData>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(Pickups.ToArray(), Limits.NumberOfPickups);
            buf.Write(LastCollectedIndex);
            buf.Write((short) 0);
            buf.Write(PickupsCollected.ToArray(), Limits.NumberOfCollectedPickups);

            Debug.Assert(buf.Offset == SizeOf<PickupData>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PickupData);
        }

        public bool Equals(PickupData other)
        {
            if (other == null)
            {
                return false;
            }

            return Pickups.SequenceEqual(other.Pickups)
                && LastCollectedIndex.Equals(other.LastCollectedIndex)
                && PickupsCollected.SequenceEqual(other.PickupsCollected);
        }
    }
}
