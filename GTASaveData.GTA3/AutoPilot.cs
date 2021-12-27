using System;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class AutoPilot : SaveDataObject,
        IEquatable<AutoPilot>, IDeepClonable<AutoPilot>
    {
        private int m_currRouteNode;
        private int m_nextRouteNode;
        private int m_prevRouteNode;
        private uint m_timeEnteredCurve;
        private uint m_timeToSpendOnCurrentCurve;
        private int m_currPathNodeInfo;
        private int m_nextPathNodeInfo;
        private int m_prevPathNodeInfo;
        private uint m_antiReverseTimer;
        private uint m_timeToStartMission;
        private byte m_prevDirection;
        private byte m_currDirection;
        private byte m_nextDirection;
        private byte m_currLane;
        private byte m_nextLane;
        private CarDrivingStyle m_drivingStyle;
        private CarMission m_mission;
        private CarTempAction m_tempAction;
        private uint m_timeTempAction;
        private float m_maxTrafficSpeed;
        private byte m_cruiseSpeed;
        private bool m_slowedDownByCars;
        private bool m_slowedDownByPeds;
        private bool m_stayInCurrentLevel;
        private bool m_stayInFastLane;
        private bool m_ignorePathFinding;
        private Vector3 m_destination;
        private short m_pathFindNodesCount;

        public int CurrRouteNode
        {
            get { return m_currRouteNode; }
            set { m_currRouteNode = value; OnPropertyChanged(); }
        }

        public int NextRouteNode
        {
            get { return m_nextRouteNode; }
            set { m_nextRouteNode = value; OnPropertyChanged(); }
        }

        public int PrevRouteNode
        {
            get { return m_prevRouteNode; }
            set { m_prevRouteNode = value; OnPropertyChanged(); }
        }

        public uint TimeEnteredCurve
        {
            get { return m_timeEnteredCurve; }
            set { m_timeEnteredCurve = value; OnPropertyChanged(); }
        }

        public uint TimeToSpendOnCurrentCurve
        {
            get { return m_timeToSpendOnCurrentCurve; }
            set { m_timeToSpendOnCurrentCurve = value; OnPropertyChanged(); }
        }

        public int CurrPathNodeInfo
        {
            get { return m_currPathNodeInfo; }
            set { m_currPathNodeInfo = value; OnPropertyChanged(); }
        }

        public int NextPathNodeInfo
        {
            get { return m_nextPathNodeInfo; }
            set { m_nextPathNodeInfo = value; OnPropertyChanged(); }
        }

        public int PrevPathNodeInfo
        {
            get { return m_prevPathNodeInfo; }
            set { m_prevPathNodeInfo = value; OnPropertyChanged(); }
        }

        public uint AntiReverseTimer
        {
            get { return m_antiReverseTimer; }
            set { m_antiReverseTimer = value; OnPropertyChanged(); }
        }

        public uint TimeToStartMission
        {
            get { return m_timeToStartMission; }
            set { m_timeToStartMission = value; OnPropertyChanged(); }
        }

        public byte PrevDirection
        {
            get { return m_prevDirection; }
            set { m_prevDirection = value; OnPropertyChanged(); }
        }

        public byte CurrDirection
        {
            get { return m_currDirection; }
            set { m_currDirection = value; OnPropertyChanged(); }
        }

        public byte NextDirection
        {
            get { return m_nextDirection; }
            set { m_nextDirection = value; OnPropertyChanged(); }
        }

        public byte CurrLane
        {
            get { return m_currLane; }
            set { m_currLane = value; OnPropertyChanged(); }
        }

        public byte NextLane
        {
            get { return m_nextLane; }
            set { m_nextLane = value; OnPropertyChanged(); }
        }

        public CarDrivingStyle DrivingStyle
        {
            get { return m_drivingStyle; }
            set { m_drivingStyle = value; OnPropertyChanged(); }
        }

        public CarMission Mission
        {
            get { return m_mission; }
            set { m_mission = value; OnPropertyChanged(); }
        }

        public CarTempAction TempAction
        {
            get { return m_tempAction; }
            set { m_tempAction = value; OnPropertyChanged(); }
        }

        public uint TimeTempAction
        {
            get { return m_timeTempAction; }
            set { m_timeTempAction = value; OnPropertyChanged(); }
        }

        public float MaxTrafficSpeed
        {
            get { return m_maxTrafficSpeed; }
            set { m_maxTrafficSpeed = value; OnPropertyChanged(); }
        }

        public byte CruiseSpeed
        {
            get { return m_cruiseSpeed; }
            set { m_cruiseSpeed = value; OnPropertyChanged(); }
        }

        public bool SlowedDownByCars
        {
            get { return m_slowedDownByCars; }
            set { m_slowedDownByCars = value; OnPropertyChanged(); }
        }

        public bool SlowedDownByPeds
        {
            get { return m_slowedDownByPeds; }
            set { m_slowedDownByPeds = value; OnPropertyChanged(); }
        }

        public bool StayInCurrentLevel
        {
            get { return m_stayInCurrentLevel; }
            set { m_stayInCurrentLevel = value; OnPropertyChanged(); }
        }

        public bool StayInFastLane
        {
            get { return m_stayInFastLane; }
            set { m_stayInFastLane = value; OnPropertyChanged(); }
        }

        public bool IgnorePathFinding
        {
            get { return m_ignorePathFinding; }
            set { m_ignorePathFinding = value; OnPropertyChanged(); }
        }

        public Vector3 Destination
        {
            get { return m_destination; }
            set { m_destination = value; OnPropertyChanged(); }
        }

        public short PathFindNodesCount
        {
            get { return m_pathFindNodesCount; }
            set { m_pathFindNodesCount = value; OnPropertyChanged(); }
        }


        public AutoPilot()
        {
            TimeToSpendOnCurrentCurve = 1000;
            NextDirection = 1;
            CruiseSpeed = 10;
            MaxTrafficSpeed = 10;
            Destination = new Vector3();
        }

        public AutoPilot(AutoPilot other)
        {
            CurrRouteNode = other.CurrRouteNode;
            NextRouteNode = other.NextRouteNode;
            PrevRouteNode = other.PrevRouteNode;
            TimeEnteredCurve = other.TimeEnteredCurve;
            TimeToSpendOnCurrentCurve = other.TimeToSpendOnCurrentCurve;
            CurrPathNodeInfo = other.CurrPathNodeInfo;
            NextPathNodeInfo = other.NextPathNodeInfo;
            PrevPathNodeInfo = other.PrevPathNodeInfo;
            AntiReverseTimer = other.AntiReverseTimer;
            TimeToStartMission = other.TimeToStartMission;
            PrevDirection = other.PrevDirection;
            CurrDirection = other.CurrDirection;
            NextDirection = other.NextDirection;
            CurrLane = other.CurrLane;
            NextLane = other.NextLane;
            DrivingStyle = other.DrivingStyle;
            Mission = other.Mission;
            TempAction = other.TempAction;
            TimeTempAction = other.TimeTempAction;
            MaxTrafficSpeed = other.MaxTrafficSpeed;
            CruiseSpeed = other.CruiseSpeed;
            SlowedDownByCars = other.SlowedDownByCars;
            SlowedDownByPeds = other.SlowedDownByPeds;
            StayInCurrentLevel = other.StayInCurrentLevel;
            StayInFastLane = other.StayInFastLane;
            IgnorePathFinding = other.IgnorePathFinding;
            Destination = other.Destination;
            PathFindNodesCount = other.PathFindNodesCount;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            CurrRouteNode = buf.ReadInt32();
            NextRouteNode = buf.ReadInt32();
            PrevRouteNode = buf.ReadInt32();
            TimeEnteredCurve = buf.ReadUInt32();
            TimeToSpendOnCurrentCurve = buf.ReadUInt32();
            CurrPathNodeInfo = buf.ReadInt32();
            NextPathNodeInfo = buf.ReadInt32();
            PrevPathNodeInfo = buf.ReadInt32();
            AntiReverseTimer = buf.ReadUInt32();
            TimeToStartMission = buf.ReadUInt32();
            PrevDirection = buf.ReadByte();
            CurrDirection = buf.ReadByte();
            NextDirection = buf.ReadByte();
            CurrLane = buf.ReadByte();
            NextLane = buf.ReadByte();
            DrivingStyle = (CarDrivingStyle) buf.ReadByte();
            Mission = (CarMission) buf.ReadByte();
            TempAction = (CarTempAction) buf.ReadByte();
            TimeTempAction = buf.ReadUInt32();
            MaxTrafficSpeed = buf.ReadFloat();
            CruiseSpeed = buf.ReadByte();
            byte flags = buf.ReadByte();
            SlowedDownByCars = (flags & 0x01) != 0;
            SlowedDownByPeds = (flags & 0x02) != 0;
            StayInCurrentLevel = (flags & 0x04) != 0;
            StayInFastLane = (flags & 0x08) != 0;
            IgnorePathFinding = (flags & 0x10) != 0;
            buf.Skip(2);
            Destination = buf.ReadStruct<Vector3>();
            buf.Skip(32);
            PathFindNodesCount = buf.ReadInt16();
            buf.Skip(6);

            Debug.Assert(buf.Offset == SizeOf<AutoPilot>());
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            buf.Write(CurrRouteNode);
            buf.Write(NextRouteNode);
            buf.Write(PrevRouteNode);
            buf.Write(TimeEnteredCurve);
            buf.Write(TimeToSpendOnCurrentCurve);
            buf.Write(CurrPathNodeInfo);
            buf.Write(NextPathNodeInfo);
            buf.Write(PrevPathNodeInfo);
            buf.Write(AntiReverseTimer);
            buf.Write(TimeToStartMission);
            buf.Write(PrevDirection);
            buf.Write(CurrDirection);
            buf.Write(NextDirection);
            buf.Write(CurrLane);
            buf.Write(NextLane);
            buf.Write((byte) DrivingStyle);
            buf.Write((byte) Mission);
            buf.Write((byte) TempAction);
            buf.Write(TimeTempAction);
            buf.Write(MaxTrafficSpeed);
            buf.Write(CruiseSpeed);
            byte flags = 0;
            if (SlowedDownByCars) flags |= 0x01;
            if (SlowedDownByPeds) flags |= 0x02;
            if (StayInCurrentLevel) flags |= 0x04;
            if (StayInFastLane) flags |= 0x08;
            if (IgnorePathFinding) flags |= 0x10;
            buf.Write(flags);
            buf.Skip(2);
            buf.Write(Destination);
            buf.Skip(32);
            buf.Write(PathFindNodesCount);
            buf.Skip(6);

            Debug.Assert(buf.Offset == SizeOf<AutoPilot>());
        }

        protected override int GetSize(SerializationParams prm)
        {
            return 0x70;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AutoPilot);
        }

        public bool Equals(AutoPilot other)
        {
            if (other == null)
            {
                return false;
            }

            return CurrRouteNode.Equals(other.CurrRouteNode)
                && NextRouteNode.Equals(other.NextRouteNode)
                && PrevRouteNode.Equals(other.PrevRouteNode)
                && TimeEnteredCurve.Equals(other.TimeEnteredCurve)
                && TimeToSpendOnCurrentCurve.Equals(other.TimeToSpendOnCurrentCurve)
                && CurrPathNodeInfo.Equals(other.CurrPathNodeInfo)
                && NextPathNodeInfo.Equals(other.NextPathNodeInfo)
                && PrevPathNodeInfo.Equals(other.PrevPathNodeInfo)
                && AntiReverseTimer.Equals(other.AntiReverseTimer)
                && TimeToStartMission.Equals(other.TimeToStartMission)
                && PrevDirection.Equals(other.PrevDirection)
                && CurrDirection.Equals(other.CurrDirection)
                && NextDirection.Equals(other.NextDirection)
                && CurrLane.Equals(other.CurrLane)
                && NextLane.Equals(other.NextLane)
                && DrivingStyle.Equals(other.DrivingStyle)
                && Mission.Equals(other.Mission)
                && TempAction.Equals(other.TempAction)
                && TimeTempAction.Equals(other.TimeTempAction)
                && MaxTrafficSpeed.Equals(other.MaxTrafficSpeed)
                && CruiseSpeed.Equals(other.CruiseSpeed)
                && SlowedDownByCars.Equals(other.SlowedDownByCars)
                && SlowedDownByPeds.Equals(other.SlowedDownByPeds)
                && StayInCurrentLevel.Equals(other.StayInCurrentLevel)
                && StayInFastLane.Equals(other.StayInFastLane)
                && IgnorePathFinding.Equals(other.IgnorePathFinding)
                && Destination.Equals(other.Destination)
                && PathFindNodesCount.Equals(other.PathFindNodesCount);
        }

        public AutoPilot DeepClone()
        {
            return new AutoPilot(this);
        }
    }

    public enum CarDrivingStyle : byte
    {
        StopForCars,
        SlowDownForCars,
        AvoidCars,
        PloughThrough,
        StopForCarsIgnoreLights
    }

    public enum CarMission : byte
    {
        None,
        Cruise,
        RamPlayerFar,
        RamPlayerNear,
        BlockPlayerFar,
        BlockPlayerNear,
        BlockPlayerHandBrakeStop,
        WaitForDeletion,
        GotoCoords,
        GotoCoordsStraight,
        EmergencyVehicleStop,
        StopForever,
        GotoCoordsAccurate,
        GotoCoordsStraightAccurate,
        GotoCoordsAsTheCrowFlies,
        RamCarFar,
        RamCarNear,
        BlockCarFar,
        BlockCarClose,
        BlockCarHandBrakeStop
    }

    public enum CarTempAction : byte
    {
        None,
        Wait,
        Reverse,
        HandBrakeLeft,
        HandBrakeRight,
        HandBrake,
        TurnLeft,
        TurnRight,
        GoStraight,
        SwerveLeft,
        SwerveRight
    }
}