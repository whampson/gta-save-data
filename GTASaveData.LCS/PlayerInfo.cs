using System;
using System.Diagnostics;

namespace GTASaveData.LCS
{
    public class PlayerInfo : SaveDataObject,
        IEquatable<PlayerInfo>, IDeepClonable<PlayerInfo>
    {
        private int m_money;
        private int m_moneyOnScreen;
        private int m_packagesCollected;
        private byte m_maxHealth;
        private byte m_maxArmor;
        private bool m_infiniteSprint;
        private bool m_fastReload;
        private bool m_fireProof;
        private bool m_getOutOfJailFree;
        private bool m_getOutOfHospitalFree;

        public int Money
        {
            get { return m_money; }
            set { m_money = value; OnPropertyChanged(); }
        }

        public int MoneyOnScreen
        {
            get { return m_moneyOnScreen; }
            set { m_moneyOnScreen = value; OnPropertyChanged(); }
        }

        public int PackagesCollected
        {
            get { return m_packagesCollected; }
            set { m_packagesCollected = value; OnPropertyChanged(); }
        }

        public byte MaxHealth
        {
            get { return m_maxHealth; }
            set { m_maxHealth = value; OnPropertyChanged(); }
        }

        public byte MaxArmor
        {
            get { return m_maxArmor; }
            set { m_maxArmor = value; OnPropertyChanged(); }
        }

        public bool InfiniteSprint
        {
            get { return m_infiniteSprint; }
            set { m_infiniteSprint = value; OnPropertyChanged(); }
        }

        public bool FastReload
        {
            get { return m_fastReload; }
            set { m_fastReload = value; OnPropertyChanged(); }
        }

        public bool FireProof
        {
            get { return m_fireProof; }
            set { m_fireProof = value; OnPropertyChanged(); }
        }

        public bool GetOutOfJailFree
        {
            get { return m_getOutOfJailFree; }
            set { m_getOutOfJailFree = value; OnPropertyChanged(); }
        }

        public bool GetOutOfHospitalFree
        {
            get { return m_getOutOfHospitalFree; }
            set { m_getOutOfHospitalFree = value; OnPropertyChanged(); }
        }

        public PlayerInfo()
        { }

        public PlayerInfo(PlayerInfo other)
        {
            Money = other.Money;
            MoneyOnScreen = other.MoneyOnScreen;
            PackagesCollected = other.PackagesCollected;
            MaxHealth = other.MaxHealth;
            MaxArmor = other.MaxArmor;
            InfiniteSprint = other.InfiniteSprint;
            FastReload = other.FastReload;
            FireProof = other.FireProof;
            GetOutOfJailFree = other.GetOutOfJailFree;
            GetOutOfHospitalFree = other.GetOutOfHospitalFree;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            if (fmt.IsMobile) buf.Skip(128);
            Money = buf.ReadInt32();
            buf.ReadInt32();
            if (fmt.IsPS2 || fmt.IsPSP) buf.Skip(3);        // weird
            if (fmt.IsPS2 || fmt.IsPSP) buf.ReadFloat();    // 1
            MoneyOnScreen = buf.ReadInt32();
            PackagesCollected = buf.ReadInt32();
            buf.ReadInt32();                                // total hidden packages? 3?
            if (fmt.IsMobile)
            {
                buf.ReadFloat();    // 1
                buf.ReadInt32();
                MaxHealth = buf.ReadByte();
                MaxArmor = buf.ReadByte();
                buf.Skip(2);
                InfiniteSprint = buf.ReadBool();
                FastReload = buf.ReadBool();
                FireProof = buf.ReadBool();
                GetOutOfJailFree = buf.ReadBool();
                GetOutOfHospitalFree = buf.ReadBool();
                buf.Skip(3);
            }
            else if (fmt.IsPSP)
            {
                InfiniteSprint = buf.ReadBool();
                FastReload = buf.ReadBool();
                FireProof = buf.ReadBool();
                MaxHealth = buf.ReadByte();
                MaxArmor = buf.ReadByte();
                GetOutOfJailFree = buf.ReadBool();
                GetOutOfHospitalFree = buf.ReadBool();
                buf.Skip(2);
                buf.Skip(83 * sizeof(int));
            }
            else if (fmt.IsPS2)
            {
                InfiniteSprint = buf.ReadBool(4);
                FastReload = buf.ReadBool(4);
                FireProof = buf.ReadBool(4);
                MaxHealth = buf.ReadByte();
                MaxArmor = buf.ReadByte();
                GetOutOfJailFree = buf.ReadBool(4);
                GetOutOfHospitalFree = buf.ReadBool(4);
                buf.Skip(3);
                buf.Skip(87 * sizeof(int));
            }

            Debug.Assert(buf.Offset == SizeOfType<PlayerInfo>(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            if (fmt.IsMobile) buf.Write(new byte[128]);
            buf.Write(Money);
            buf.Write(0);
            if (fmt.IsPS2 || fmt.IsPSP) buf.Skip(3);        // weird
            if (fmt.IsPS2 || fmt.IsPSP) buf.Write(1.0f);
            buf.Write(MoneyOnScreen);
            buf.Write(PackagesCollected);
            buf.Write(3);
            if (fmt.IsMobile)
            {
                buf.Write(1.0f);
                buf.Write(0);       // value read is nonzero but it seems useless
                buf.Write(MaxHealth);
                buf.Write(MaxArmor);
                buf.Skip(2);
                buf.Write(InfiniteSprint);
                buf.Write(FastReload);
                buf.Write(FireProof);
                buf.Write(GetOutOfJailFree);
                buf.Write(GetOutOfHospitalFree);
                buf.Skip(3);
            }
            else if (fmt.IsPSP)
            {
                buf.Write(InfiniteSprint);
                buf.Write(FastReload);
                buf.Write(FireProof);
                buf.Write(MaxHealth);
                buf.Write(MaxArmor);
                buf.Write(GetOutOfJailFree);
                buf.Write(GetOutOfHospitalFree);
                buf.Skip(2);
                buf.Write(new byte[83 * sizeof(int)]);
            }
            else if (fmt.IsPS2)
            {
                buf.Write(InfiniteSprint, 4);
                buf.Write(FastReload, 4);
                buf.Write(FireProof, 4);
                buf.Write(MaxHealth);
                buf.Write(MaxArmor);
                buf.Write(GetOutOfJailFree, 4);
                buf.Write(GetOutOfHospitalFree, 4);
                buf.Skip(3);
                buf.Write(new byte[87 * sizeof(int)]);
            }

            Debug.Assert(buf.Offset == SizeOfType<PlayerInfo>(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPSP) return 0x170;
            if (fmt.IsPS2) return 0x190;
            if (fmt.IsMobile) return 0xA8;
            throw SizeNotDefined(fmt);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PlayerInfo);
        }

        public bool Equals(PlayerInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return Money.Equals(other.Money)
                && MoneyOnScreen.Equals(other.MoneyOnScreen)
                && PackagesCollected.Equals(other.PackagesCollected)
                && MaxHealth.Equals(other.MaxHealth)
                && MaxArmor.Equals(other.MaxArmor)
                && InfiniteSprint.Equals(other.InfiniteSprint)
                && FastReload.Equals(other.FastReload)
                && FireProof.Equals(other.FireProof)
                && GetOutOfJailFree.Equals(other.GetOutOfJailFree)
                && GetOutOfHospitalFree.Equals(other.GetOutOfHospitalFree);
        }

        public PlayerInfo DeepClone()
        {
            return new PlayerInfo(this);
        }
    }
}
