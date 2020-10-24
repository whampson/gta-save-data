using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA4
{
    [Size(0xD551)]
    public class Pickups : SaveDataObject, IEquatable<Pickups>
    {
        public static class Limits
        {
            public const int MaxPickupsCount = 650;
        }

        private int m_pickupsCount;
        private Array<Pickup> m_pickupsArray;
        private byte m_weaponPickupMessagesRemaining;
        private int m_unknown;

        public int PickupsCount
        {
            get { return m_pickupsCount; }
            set { m_pickupsCount = value; OnPropertyChanged(); }
        }

        public Array<Pickup> PickupsArray
        {
            get { return m_pickupsArray; }
            set { m_pickupsArray = value; OnPropertyChanged(); }
        }

        public byte WeaponPickupMessagesRemaining
        {
            get { return m_weaponPickupMessagesRemaining; }
            set { m_weaponPickupMessagesRemaining = value; OnPropertyChanged(); }
        }

        public int Unknown
        {
            get { return m_unknown; }
            set { m_unknown = value; OnPropertyChanged(); }
        }

        public Pickups()
        {
            PickupsArray = new Array<Pickup>();
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            PickupsCount = buf.ReadInt32();
            PickupsArray = buf.ReadArray<Pickup>(Limits.MaxPickupsCount);
            WeaponPickupMessagesRemaining = buf.ReadByte();
            Unknown = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOfType<Pickups>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(PickupsCount);
            buf.Write(PickupsArray, Limits.MaxPickupsCount);
            buf.Write(WeaponPickupMessagesRemaining);
            buf.Write(Unknown);

            Debug.Assert(buf.Offset == SizeOfType<Pickups>());
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

            return PickupsCount.Equals(other.PickupsCount)
                && PickupsArray.SequenceEqual(other.PickupsArray)
                && WeaponPickupMessagesRemaining.Equals(other.WeaponPickupMessagesRemaining)
                && Unknown.Equals(other.Unknown);
        }
    }
}
