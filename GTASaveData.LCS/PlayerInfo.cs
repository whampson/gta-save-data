using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.LCS
{
    public class PlayerInfo : SaveDataObject,
        IEquatable<PlayerInfo>, IDeepClonable<PlayerInfo>
    {
        public const int MaxPlayerNameLength = 35;

        private int m_money;
        private byte m_wastedBustedState;
        private uint m_wastedBustedTime;
        private short m_trafficMultiplier;
        private float m_roadDensity;
        private short m_carDensity;
        private int m_packagesTotal;
        private byte[] m_unknownData;
        private byte[] m_pspUnknownData;
        private byte m_unknownState;
        private byte m_unknownFlag;
        private int m_moneyOnScreen;
        private int m_packagesCollected;
        private byte m_maxHealth;
        private byte m_maxArmor;
        private bool m_infiniteSprint;
        private bool m_fastReload;
        private bool m_fireProof;
        private bool m_getOutOfJailFree;
        private bool m_getOutOfHospitalFree;
        private string m_playerName;

        public int Money
        {
            get { return m_money; }
            set { m_money = value; OnPropertyChanged(); }
        }

        public byte WastedBustedState
        {
            get { return m_wastedBustedState; }
            set { m_wastedBustedState = value; OnPropertyChanged(); }
        }

        public uint WastedBustedTime
        {
            get { return m_wastedBustedTime; }
            set { m_wastedBustedTime = value; OnPropertyChanged(); }
        }

        public short TrafficMultiplier
        {
            get { return m_trafficMultiplier; }
            set { m_trafficMultiplier = value; OnPropertyChanged(); }
        }

        public float RoadDensity
        {
            get { return m_roadDensity; }
            set { m_roadDensity = value; OnPropertyChanged(); }
        }

        public short CarDensity
        {
            get { return m_carDensity; }
            set { m_carDensity = value; OnPropertyChanged(); }
        }

        public int PackagesTotal
        {
            get { return m_packagesTotal; }
            set { m_packagesTotal = value; OnPropertyChanged(); }
        }

        public byte[] UnknownData
        {
            get { return m_unknownData; }
            set { m_unknownData = value; OnPropertyChanged(); }
        }

        public byte[] PspUnknownData
        {
            get { return m_pspUnknownData; }
            set { m_pspUnknownData = value; OnPropertyChanged(); }
        }

        public byte UnknownState
        {
            get { return m_unknownState; }
            set { m_unknownState = value; OnPropertyChanged(); }
        }

        public byte UnknownFlag
        {
            get { return m_unknownFlag; }
            set { m_unknownFlag = value; OnPropertyChanged(); }
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

        public string PlayerName
        {
            get { return m_playerName; }
            set { m_playerName = value; OnPropertyChanged(); }
        }

        public PlayerInfo()
        {
            PlayerName = "";
            UnknownData = new byte[77];
            PspUnknownData = new byte[74];
        }

        public PlayerInfo(PlayerInfo other)
        {
            Money = other.Money;
            WastedBustedState = other.WastedBustedState;
            WastedBustedTime = other.WastedBustedTime;
            TrafficMultiplier = other.TrafficMultiplier;
            RoadDensity = other.RoadDensity;
            CarDensity = other.CarDensity;
            PackagesTotal = other.PackagesTotal;
            UnknownData = other.UnknownData.ToArray();
            PspUnknownData = other.PspUnknownData.ToArray();
            UnknownState = other.UnknownState;
            UnknownFlag = other.UnknownFlag;
            MoneyOnScreen = other.MoneyOnScreen;
            PackagesCollected = other.PackagesCollected;
            MaxHealth = other.MaxHealth;
            MaxArmor = other.MaxArmor;
            InfiniteSprint = other.InfiniteSprint;
            FastReload = other.FastReload;
            FireProof = other.FireProof;
            GetOutOfJailFree = other.GetOutOfJailFree;
            GetOutOfHospitalFree = other.GetOutOfHospitalFree;
            PlayerName = other.PlayerName;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams p)
        {
            LCSSaveParams param = p as LCSSaveParams;

            if (param.FileType.IsMobile)
            {
                PlayerName = buf.ReadString(MaxPlayerNameLength, unicode: true);
                buf.Skip(58);
            }
            Money = buf.ReadInt32();
            if (param.FileType.IsPS2 || param.FileType.IsPSP)
            {
                WastedBustedState = buf.ReadByte();
                WastedBustedTime = buf.ReadUInt32();
                TrafficMultiplier = buf.ReadInt16();
                RoadDensity = buf.ReadFloat();
            }
            else
            {
                WastedBustedTime = buf.ReadUInt32();
            }
            MoneyOnScreen = buf.ReadInt32();
            PackagesCollected = buf.ReadInt32();
            PackagesTotal = buf.ReadInt32();
            if (param.FileType.IsMobile)
            {
                RoadDensity = buf.ReadFloat();
                TrafficMultiplier = buf.ReadInt16();
                CarDensity = buf.ReadInt16();
                MaxHealth = buf.ReadByte();
                MaxArmor = buf.ReadByte();
                WastedBustedState = buf.ReadByte();
                UnknownState = buf.ReadByte();
                InfiniteSprint = buf.ReadBool();
                FastReload = buf.ReadBool();
                FireProof = buf.ReadBool();
                GetOutOfJailFree = buf.ReadBool();
                GetOutOfHospitalFree = buf.ReadBool();
                UnknownFlag = buf.ReadByte();
                buf.Skip(2);
            }
            else if (param.FileType.IsPSP)
            {
                InfiniteSprint = buf.ReadBool();
                FastReload = buf.ReadBool();
                FireProof = buf.ReadBool();
                MaxHealth = buf.ReadByte();
                MaxArmor = buf.ReadByte();
                GetOutOfJailFree = buf.ReadBool();
                GetOutOfHospitalFree = buf.ReadBool();
                PspUnknownData = buf.ReadBytes(74);
                buf.Skip(260);
            }
            else if (param.FileType.IsPS2)
            {
                InfiniteSprint = buf.ReadBool(4);
                FastReload = buf.ReadBool(4);
                FireProof = buf.ReadBool(4);
                MaxHealth = buf.ReadByte();
                MaxArmor = buf.ReadByte();
                GetOutOfJailFree = buf.ReadBool(4);
                GetOutOfHospitalFree = buf.ReadBool(4);
                UnknownData = buf.ReadBytes(77);
                buf.Skip(274);
            }

            Debug.Assert(buf.Offset == SizeOf<PlayerInfo>(param));
        }

        protected override void WriteData(DataBuffer buf, SerializationParams p)
        {
            LCSSaveParams param = p as LCSSaveParams;

            if (param.FileType.IsMobile)
            {
                buf.Write(PlayerName, MaxPlayerNameLength, unicode: true);
                buf.Skip(58);
            }
            buf.Write(Money);
            if (param.FileType.IsPS2 || param.FileType.IsPSP)
            {
                buf.Write(WastedBustedState);
                buf.Write(WastedBustedTime);
                buf.Write(TrafficMultiplier);
                buf.Write(RoadDensity);
            }
            else
            {
                buf.Write(WastedBustedTime);
            }
            buf.Write(MoneyOnScreen);
            buf.Write(PackagesCollected);
            buf.Write(PackagesTotal);
            if (param.FileType.IsMobile)
            {
                buf.Write(RoadDensity);
                buf.Write(TrafficMultiplier);
                buf.Write(CarDensity);
                buf.Write(MaxHealth);
                buf.Write(MaxArmor);
                buf.Write(WastedBustedState);
                buf.Write(UnknownState);
                buf.Write(InfiniteSprint);
                buf.Write(FastReload);
                buf.Write(FireProof);
                buf.Write(GetOutOfJailFree);
                buf.Write(GetOutOfHospitalFree);
                buf.Write(UnknownFlag);
                buf.Skip(2);
            }
            else if (param.FileType.IsPSP)
            {
                buf.Write(InfiniteSprint);
                buf.Write(FastReload);
                buf.Write(FireProof);
                buf.Write(MaxHealth);
                buf.Write(MaxArmor);
                buf.Write(GetOutOfJailFree);
                buf.Write(GetOutOfHospitalFree);
                buf.Write(PspUnknownData);
                buf.Skip(260);
            }
            else if (param.FileType.IsPS2)
            {
                buf.Write(InfiniteSprint, 4);
                buf.Write(FastReload, 4);
                buf.Write(FireProof, 4);
                buf.Write(MaxHealth);
                buf.Write(MaxArmor);
                buf.Write(GetOutOfJailFree, 4);
                buf.Write(GetOutOfHospitalFree, 4);
                buf.Write(UnknownData);
                buf.Skip(274);
            }

            Debug.Assert(buf.Offset == SizeOf<PlayerInfo>(param));
        }

        protected override int GetSize(SerializationParams p)
        {
            LCSSaveParams param = p as LCSSaveParams;

            if (param.FileType.IsPSP) return 0x170;
            if (param.FileType.IsPS2) return 0x190;
            if (param.FileType.IsMobile) return 0xA8;
            throw SizeNotDefined(param);
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
                && WastedBustedState.Equals(other.WastedBustedState)
                && WastedBustedTime.Equals(other.WastedBustedTime)
                && TrafficMultiplier.Equals(other.TrafficMultiplier)
                && RoadDensity.Equals(other.RoadDensity)
                && CarDensity.Equals(other.CarDensity)
                && PackagesTotal.Equals(other.PackagesTotal)
                && UnknownData.SequenceEqual(other.UnknownData)
                && PspUnknownData.SequenceEqual(other.PspUnknownData)
                && UnknownState.Equals(other.UnknownState)
                && UnknownFlag.Equals(other.UnknownFlag)
                && MoneyOnScreen.Equals(other.MoneyOnScreen)
                && PackagesCollected.Equals(other.PackagesCollected)
                && MaxHealth.Equals(other.MaxHealth)
                && MaxArmor.Equals(other.MaxArmor)
                && InfiniteSprint.Equals(other.InfiniteSprint)
                && FastReload.Equals(other.FastReload)
                && FireProof.Equals(other.FireProof)
                && GetOutOfJailFree.Equals(other.GetOutOfJailFree)
                && GetOutOfHospitalFree.Equals(other.GetOutOfHospitalFree)
                && PlayerName.Equals(other.PlayerName);
        }

        public PlayerInfo DeepClone()
        {
            return new PlayerInfo(this);
        }
    }

    public enum PlayerOutfit
    {
        [Description("Casual")]
        Casual,

        [Description("Leone's Suit")]
        Leone,

        [Description("Overalls")]
        Overalls,

        [Description("Avenging Angels")]
        AvengingAngels,

        [Description("Chauffeur")]
        Chauffer,

        [Description("Lawyer")]
        Lawyer,

        [Description("Tuxedo")]
        Tuxedo,

        [Description("'The King' Jumpsuit")]
        TheKing,

        [Description("Cox Mascot Suit")]
        Cox,

        [Description("Underwear")]
        Underwear,

        [Description("Hero Garb")]
        Hero,

        [Description("'Dragon' Jumpsuit")]
        Dragon,

        [Description("Antonio")]
        Antonio,

        [Description("Sweats")]
        Sweats,

        [Description("Goodfella")]
        Goodfella,

        [Description("Wiseguy")]
        Wiseguy,
    }

    [Flags]
    public enum PlayerOutfitFlags : ushort
    {
        None,
        Casual = (1 << 0),
        Leone = (1 << 1),
        Overalls = (1 << 2),
        AvengingAngels = (1 << 3),
        Chauffer = (1 << 4),
        Lawyer = (1 << 5),
        Tuxedo = (1 << 6),
        TheKing = (1 << 7),
        Cox = (1 << 8),
        Underwear = (1 << 9),
        Hero = (1 << 10),
        Dragon = (1 << 11),
        Antonio = (1 << 12),
        Sweats = (1 << 13),
        Goodfella = (1 << 14),
        Wiseguy = (1 << 15),
    }
}
