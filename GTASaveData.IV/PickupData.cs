using GTASaveData.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.IV
{
    public class PickupData : Block,
        IEquatable<PickupData>, IDeepClonable<PickupData>,
        IEnumerable<Pickup>
    {
        public const int MaxNumPickups = 650;

        private int m_numPickups;
        private ObservableArray<Pickup> m_Pickups;
        private byte m_weaponPickupMessagesRemaining;
        private int m_unknown;

        public int NumPickups
        {
            get { return m_numPickups; }
            set { m_numPickups = value; OnPropertyChanged(); }
        }

        public ObservableArray<Pickup> Pickups
        {
            get { return m_Pickups; }
            set { m_Pickups = value; OnPropertyChanged(); }
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
            Pickups = ArrayHelper.CreateArray<Pickup>(MaxNumPickups);
        }

        public PickupData(PickupData other)
        {
            NumPickups = other.NumPickups;
            Pickups = ArrayHelper.DeepClone(other.Pickups);
            WeaponPickupMessagesRemaining = other.WeaponPickupMessagesRemaining;
            Unknown = other.Unknown;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            BlockHeader head = ReadBlockHeader(buf);
            buf.Mark();

            NumPickups = buf.ReadInt32();
            Pickups = buf.ReadArray<Pickup>(MaxNumPickups);
            WeaponPickupMessagesRemaining = buf.ReadByte();
            Unknown = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<PickupData>());
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write(NumPickups);
            buf.Write(Pickups, MaxNumPickups);
            buf.Write(WeaponPickupMessagesRemaining);
            buf.Write(Unknown);

            Debug.Assert(buf.Offset == SizeOf<PickupData>());
        }

        protected override int GetSize(SerializationParams p)
        {
            return 0xD551;  // TODO: calculate this
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

            return NumPickups.Equals(other.NumPickups)
                && Pickups.SequenceEqual(other.Pickups)
                && WeaponPickupMessagesRemaining.Equals(other.WeaponPickupMessagesRemaining)
                && Unknown.Equals(other.Unknown);
        }

        public new PickupData DeepClone()
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
