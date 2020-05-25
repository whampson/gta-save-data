using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Weapon : SaveDataObject, IEquatable<Weapon>
    {
        private WeaponType m_type;
        private WeaponState m_state;
        private uint m_ammoInClip;
        private uint m_ammoTotal;
        private uint m_timer;
        private bool m_unknown;

        public WeaponType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public WeaponState State
        {
            get { return m_state; }
            set { m_state = value; OnPropertyChanged(); }
        }

        public uint AmmoInClip
        {
            get { return m_ammoInClip; }
            set { m_ammoInClip = value; OnPropertyChanged(); }
        }

        public uint AmmoTotal
        {
            get { return m_ammoTotal; }
            set { m_ammoTotal = value; OnPropertyChanged(); }
        }

        public uint Timer
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        public bool Unknown
        {
            get { return m_unknown; }
            set { m_unknown = value; OnPropertyChanged(); }
        }


        public Weapon()
        { }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Type = (WeaponType) buf.ReadInt32();
            State = (WeaponState) buf.ReadInt32();
            AmmoInClip = buf.ReadUInt32();
            AmmoTotal = buf.ReadUInt32();
            Timer = buf.ReadUInt32();
            if (!fmt.PS2)
            {
                Unknown = buf.ReadBool();
                buf.ReadBytes(3);
            }

            Debug.Assert(buf.Offset == SizeOf<Weapon>(fmt));
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write((uint) Type);
            buf.Write((uint) State);
            buf.Write(AmmoInClip);
            buf.Write(AmmoTotal);
            buf.Write(Timer);
            if (!fmt.PS2)
            {
                buf.Write(Unknown);
                buf.Write(new byte[3]);
            }

            Debug.Assert(buf.Offset == SizeOf<Weapon>(fmt));
        }

        protected override int GetSize(DataFormat fmt)
        {
            if (fmt.PS2)
            {
                return 20;
            }

            return 24;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Weapon);
        }

        public bool Equals(Weapon other)
        {
            if (other == null)
            {
                return false;
            }

            return Type.Equals(other.Type)
                && State.Equals(other.State)
                && AmmoInClip.Equals(other.AmmoInClip)
                && AmmoInClip.Equals(other.AmmoInClip)
                && Unknown.Equals(other.Unknown);
        }
    }

    public enum WeaponType
    {
        None,
        BaseballBat,
        Colt45,
        Uzi,
        Shotgun,
        AK47,
        M16,
        SniperRifle,
        RocketLauncher,
        Flamethrower,
        Molotov,
        Grenade,
        Detonator
    }

    public enum WeaponState
    {
        Ready,
        Firing,
        Reloading,
        OutOfAmmo,
        MeleeMadeContact
    }
}