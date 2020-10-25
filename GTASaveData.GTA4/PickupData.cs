using System;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA4
{
    public class PickupData : SaveDataObject,
        IEquatable<PickupData>, IDeepClonable<PickupData>
    {
        public const int MaxPickupsCount = 650;

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

        public PickupData()
        {
            PickupsArray = new Array<Pickup>();
        }

        public PickupData(PickupData other)
        {
            PickupsCount = other.PickupsCount;
            PickupsArray = ArrayHelper.DeepClone(other.PickupsArray);
            WeaponPickupMessagesRemaining = other.WeaponPickupMessagesRemaining;
            Unknown = other.Unknown;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            PickupsCount = buf.ReadInt32();
            PickupsArray = buf.ReadArray<Pickup>(MaxPickupsCount);
            WeaponPickupMessagesRemaining = buf.ReadByte();
            Unknown = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOfType<PickupData>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(PickupsCount);
            buf.Write(PickupsArray, MaxPickupsCount);
            buf.Write(WeaponPickupMessagesRemaining);
            buf.Write(Unknown);

            Debug.Assert(buf.Offset == SizeOfType<PickupData>());
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

            return PickupsCount.Equals(other.PickupsCount)
                && PickupsArray.SequenceEqual(other.PickupsArray)
                && WeaponPickupMessagesRemaining.Equals(other.WeaponPickupMessagesRemaining)
                && Unknown.Equals(other.Unknown);
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0xD551;  // TODO calculate
        }

        public PickupData DeepClone()
        {
            return new PickupData(this);
        }
    }
}
