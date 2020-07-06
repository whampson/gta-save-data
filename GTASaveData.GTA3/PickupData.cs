using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class PickupData : SaveDataObject,
        IEquatable<PickupData>, IDeepClonable<PickupData>,
        IEnumerable<Pickup>
    {
        public const int MaxNumPickups = 336;
        public const int MaxNumCollectedPickups = 20;

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

        public Pickup this[int i]
        {
            get { return Pickups[i]; }
            set { Pickups[i] = value; OnPropertyChanged(); }
        }

        public PickupData()
        {
            Pickups = ArrayHelper.CreateArray<Pickup>(MaxNumPickups);
            PickupsCollected = ArrayHelper.CreateArray<int>(MaxNumCollectedPickups);
        }

        public PickupData(PickupData other)
        {
            Pickups = ArrayHelper.DeepClone(other.Pickups);
            LastCollectedIndex = other.LastCollectedIndex;
            PickupsCollected = ArrayHelper.DeepClone(other.PickupsCollected);
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Pickups = buf.Read<Pickup>(MaxNumPickups);
            LastCollectedIndex = buf.ReadInt16();
            buf.ReadInt16();
            PickupsCollected = buf.Read<int>(MaxNumCollectedPickups);

            Debug.Assert(buf.Offset == SizeOfType<PickupData>());
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Pickups, MaxNumPickups);
            buf.Write(LastCollectedIndex);
            buf.Write((short) 0);
            buf.Write(PickupsCollected, MaxNumCollectedPickups);

            Debug.Assert(buf.Offset == SizeOfType<PickupData>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x2514;
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

        public PickupData DeepClone()
        {
            return new PickupData(this);
        }

        public IEnumerator<Pickup> GetEnumerator()
        {
            return Pickups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
