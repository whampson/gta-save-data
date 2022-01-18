using System;
using System.Diagnostics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class Weapon : SaveDataObject,
        IEquatable<Weapon>, IDeepClonable<Weapon>
    {
        private WeaponType m_type;
        private WeaponState m_state;
        private int m_ammoInClip;
        private int m_ammoTotal;
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

        public int AmmoInClip
        {
            get { return m_ammoInClip; }
            set { m_ammoInClip = value; OnPropertyChanged(); }
        }

        public int AmmoTotal
        {
            get { return m_ammoTotal; }
            set { m_ammoTotal = value; OnPropertyChanged(); }
        }

        public uint Timer
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        public bool AddRotOffset
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
            AddRotOffset = other.AddRotOffset;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            GTA3SaveParams p = (GTA3SaveParams) prm;

            Type = (WeaponType) buf.ReadInt32();
            State = (WeaponState) buf.ReadInt32();
            AmmoInClip = buf.ReadInt32();
            AmmoTotal = buf.ReadInt32();
            Timer = buf.ReadUInt32();
            if (!p.IsPS2)
            {
                AddRotOffset = buf.ReadBool();
                buf.ReadBytes(3);
            }

            Debug.Assert(buf.Offset == SizeOf<Weapon>(prm));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            GTA3SaveParams p = (GTA3SaveParams) prm;

            buf.Write((uint) Type);
            buf.Write((uint) State);
            buf.Write(AmmoInClip);
            buf.Write(AmmoTotal);
            buf.Write(Timer);
            if (!p.IsPS2)
            {
                buf.Write(AddRotOffset);
                buf.Write(new byte[3]);
            }

            Debug.Assert(buf.Offset == SizeOf<Weapon>(prm));
        }

        protected override int GetSize(SerializationParams prm)
        {
            GTA3SaveParams p = (GTA3SaveParams) prm;

            if (p.IsPS2)
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
                && AddRotOffset.Equals(other.AddRotOffset);
        }

        public Weapon DeepClone()
        {
            return new Weapon(this);
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