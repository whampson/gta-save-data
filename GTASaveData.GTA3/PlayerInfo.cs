using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class PlayerInfo : SaveDataObject,
        IEquatable<PlayerInfo>, IDeepClonable<PlayerInfo>
    {
        public const int MaxPlayerNameLength = 70;

        private int m_money;
        private WastedBustedState m_wastedBustedState;
        private uint m_wastedBustedTime;
        private short m_trafficMultiplier;
        private float m_roadDensity;
        private int m_moneyOnScreen;
        private int m_packagesCollected;
        private int m_packagesTotal;
        private bool m_infinteSprint;
        private bool m_fastReload;
        private bool m_getOutOfJailFree;
        private bool m_getOutOfHospitalFree;
        private string m_playerName;

        public int Money
        {
            get { return m_money; }
            set { m_money = value; OnPropertyChanged(); }
        }

        public WastedBustedState WastedBustedState
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

        public int PackagesTotal
        {
            get { return m_packagesTotal; }
            set { m_packagesTotal = value; OnPropertyChanged(); }
        }

        public bool InfinteSprint
        {
            get { return m_infinteSprint; }
            set { m_infinteSprint = value; OnPropertyChanged(); }
        }

        public bool FastReload
        {
            get { return m_fastReload; }
            set { m_fastReload = value; OnPropertyChanged(); }
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
        }

        public PlayerInfo(PlayerInfo other)
        {
            Money = other.Money;
            WastedBustedState = other.WastedBustedState;
            WastedBustedTime = other.WastedBustedTime;
            TrafficMultiplier = other.TrafficMultiplier;
            RoadDensity = other.RoadDensity;
            MoneyOnScreen = other.MoneyOnScreen;
            PackagesCollected = other.PackagesCollected;
            PackagesTotal = other.PackagesTotal;
            InfinteSprint = other.InfinteSprint;
            FastReload = other.FastReload;
            GetOutOfJailFree = other.GetOutOfJailFree;
            GetOutOfHospitalFree = other.GetOutOfHospitalFree;
            PlayerName = other.PlayerName;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Money = buf.ReadInt32();
            WastedBustedState = (WastedBustedState) buf.ReadByte();
            WastedBustedTime = buf.ReadUInt32();
            TrafficMultiplier = buf.ReadInt16();
            RoadDensity = buf.ReadFloat();
            MoneyOnScreen = buf.ReadInt32();
            PackagesCollected = buf.ReadInt32();
            PackagesTotal = buf.ReadInt32();
            InfinteSprint = buf.ReadBool();
            FastReload = buf.ReadBool();
            GetOutOfJailFree = buf.ReadBool();
            GetOutOfHospitalFree = buf.ReadBool();
            PlayerName = buf.ReadString(MaxPlayerNameLength);

            buf.Skip(215);

            Debug.Assert(buf.Offset == SizeOfType<PlayerInfo>());
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Money);
            buf.Write((byte) WastedBustedState);
            buf.Write(WastedBustedTime);
            buf.Write(TrafficMultiplier);
            buf.Write(RoadDensity);
            buf.Write(MoneyOnScreen);
            buf.Write(PackagesCollected);
            buf.Write(PackagesTotal);
            buf.Write(InfinteSprint);
            buf.Write(FastReload);
            buf.Write(GetOutOfJailFree);
            buf.Write(GetOutOfHospitalFree);
            buf.Write(PlayerName, MaxPlayerNameLength);

            // Game writes some garbage here due to incorrect size calculation
            buf.Skip(215);

            Debug.Assert(buf.Offset == SizeOfType<PlayerInfo>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0x13C;
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
                && MoneyOnScreen.Equals(other.MoneyOnScreen)
                && PackagesCollected.Equals(other.PackagesCollected)
                && PackagesTotal.Equals(other.PackagesTotal)
                && InfinteSprint.Equals(other.InfinteSprint)
                && FastReload.Equals(other.FastReload)
                && GetOutOfJailFree.Equals(other.GetOutOfJailFree)
                && GetOutOfHospitalFree.Equals(other.GetOutOfHospitalFree)
                && PlayerName.Equals(other.PlayerName);
        }

        public PlayerInfo DeepClone()
        {
            return new PlayerInfo(this);
        }
    }

    public enum WastedBustedState
    {
        Playing,
        Wasted,
        Busted,
        FailedCriticalMission
    }
}
