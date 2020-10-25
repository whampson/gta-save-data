using System;
using System.Diagnostics;
using GTASaveData.Interfaces;

namespace GTASaveData.VC
{
    public class Weapon : SaveDataObject,
        IEquatable<Weapon>, IDeepClonable<Weapon>
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

        public Weapon(Weapon other)
        {
            Type = other.Type;
            State = other.State;
            AmmoInClip = other.AmmoInClip;
            AmmoInClip = other.AmmoInClip;
            Unknown = other.Unknown;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            Type = (WeaponType) buf.ReadInt32();
            State = (WeaponState) buf.ReadInt32();
            AmmoInClip = buf.ReadUInt32();
            AmmoTotal = buf.ReadUInt32();
            Timer = buf.ReadUInt32();
            if (!fmt.IsPS2)
            {
                // TODO: confirm this is !PS2
                Unknown = buf.ReadBool();
                buf.ReadBytes(3);
            }

            Debug.Assert(buf.Offset == SizeOfType<Weapon>(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write((uint) Type);
            buf.Write((uint) State);
            buf.Write(AmmoInClip);
            buf.Write(AmmoTotal);
            buf.Write(Timer);
            if (!fmt.IsPS2)
            {
                buf.Write(Unknown);
                buf.Write(new byte[3]);
            }

            Debug.Assert(buf.Offset == SizeOfType<Weapon>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPS2)
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

        public Weapon DeepClone()
        {
            return new Weapon(this);
        }
    }

    public enum WeaponType
    {
        None,
        BrassKnuckles,
        ScrewDriver,
        GolfClub,
        NightStick,
        Knife,
        BaseballBat,
        Hammer,
        Cleaver,
        Machete,
        Katana,
        Chainsaw,
        Grenade,
        DetonatorGrenade,
        TearGas,
        Molotov,
        Rocket,
        Colt45,
        Python,
        Shotgun,
        Spas12Shotgun,
        StubbyShotgun,
        Tec9,
        M4,
        Ruger,
        SniperRifle,
        LaserScopeSniperRifle,
        RocketLauncher,
        FlameThrower,
        M60,
        MiniGun,
        Detonator,
        HeliCannon,
        Camera
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