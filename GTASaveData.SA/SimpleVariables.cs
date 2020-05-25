using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.SA
{
    public class SimpleVariables : SaveDataObject, IEquatable<SimpleVariables>
    {
        public static class Limits
        {
            public const int MaxNameLength = 100;   // PC, not sure if different on other platforms
        }

        private const int SizeOfSimpleVariablesPC = 0x138;
        private const int SizeOfSimpleVariablesAndroid = 0x1A8;
        
        private uint m_versionId;
        private string m_lastMissionPassedName;
        private byte m_missionPackGame;
        private LevelType m_currLevel;
        private Vector3D m_cameraPosition;
        private int m_millisecondsPerGameMinute;
        private uint m_lastClockTick;
        private byte m_gameClockMonths;
        private byte m_gameClockDays;
        private byte m_gameClockHours;
        private byte m_gameClockMinutes;
        private byte m_gameClockDayOfWeek;
        private byte m_storedGameClockMonths;
        private byte m_storedGameClockDays;
        private byte m_storedGameClockHours;
        private byte m_storedGameClockMinutes;
        private bool m_clockHasBeenStored;
        private short m_currPadMode;
        private bool m_hasPlayerCheated;
        private uint m_timeInMilliseconds;
        private float m_TimeScale;
        private float m_TimeStep;
        private float m_TimeStepNonClipped;
        private uint m_frameCounter;
        private WeatherType m_oldWeatherType;
        private WeatherType m_newWeatherType;
        private WeatherType m_forcedWeatherType;
        private float m_weatherInterpolationValue;
        private int m_weatherTypeInList;
        private float m_rain;
        private int m_cameraCarZoomIndicator;
        private int m_cameraPedZoomIndicator;
        private int m_currArea;
        private bool m_invertLook4Pad;
        private int m_extraColour;
        private bool m_extraColourOn;
        private float m_extraColourInter;
        private WeatherType m_extraColourWeatherType;
        private int m_waterConfiguration;
        private bool m_laRiots;
        private bool m_laRiotsNoPoliceCars;
        private int m_maximumWantedLevel;
        private int m_maximumChaosLevel;
        private bool m_germanGame;
        private bool m_frenchGame;
        private bool m_nastyGame;
        private byte m_cineyCamMessageDisplayed;
        private SystemTime m_timeLastSaved;
        private int m_targetMarkerHandle;
        private bool m_HasDisplayedPlayerQuitEnterCarHelpText;
        private bool m_allTaxisHaveNitro;
        private bool m_prostiutesPayYou;

        public string LastMissionPassedName
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
        }

        public uint VersionId
        {
            get { return m_versionId; }
            set { m_versionId = value; OnPropertyChanged(); }
        }

        public byte MissionPackGame
        {
            get { return m_missionPackGame; }
            set { m_missionPackGame = value; OnPropertyChanged(); }
        }

        public LevelType CurrLevel
        {
            get { return m_currLevel; }
            set { m_currLevel = value; OnPropertyChanged(); }
        }

        public Vector3D CameraPosition
        {
            get { return m_cameraPosition; }
            set { m_cameraPosition = value; OnPropertyChanged(); }
        }

        public int MillisecondsPerGameMinute
        {
            get { return m_millisecondsPerGameMinute; }
            set { m_millisecondsPerGameMinute = value; OnPropertyChanged(); }
        }

        public uint LastClockTick
        {
            get { return m_lastClockTick; }
            set { m_lastClockTick = value; OnPropertyChanged(); }
        }

        public byte GameClockMonths
        {
            get { return m_gameClockMonths; }
            set { m_gameClockMonths = value; OnPropertyChanged(); }
        }

        public byte GameClockDays
        {
            get { return m_gameClockDays; }
            set { m_gameClockDays = value; OnPropertyChanged(); }
        }

        public byte GameClockHours
        {
            get { return m_gameClockHours; }
            set { m_gameClockHours = value; OnPropertyChanged(); }
        }

        public byte GameClockMinutes
        {
            get { return m_gameClockMinutes; }
            set { m_gameClockMinutes = value; OnPropertyChanged(); }
        }

        public byte GameClockDayOfWeek
        {
            get { return m_gameClockDayOfWeek; }
            set { m_gameClockDayOfWeek = value; OnPropertyChanged(); }
        }

        public byte StoredGameClockMonths
        {
            get { return m_storedGameClockMonths; }
            set { m_storedGameClockMonths = value; OnPropertyChanged(); }
        }

        public byte StoredGameClockDays
        {
            get { return m_storedGameClockDays; }
            set { m_storedGameClockDays = value; OnPropertyChanged(); }
        }

        public byte StoredGameClockHours
        {
            get { return m_storedGameClockHours; }
            set { m_storedGameClockHours = value; OnPropertyChanged(); }
        }

        public byte StoredGameClockMinutes
        {
            get { return m_storedGameClockMinutes; }
            set { m_storedGameClockMinutes = value; OnPropertyChanged(); }
        }

        public bool ClockHasBeenStored
        {
            get { return m_clockHasBeenStored; }
            set { m_clockHasBeenStored = value; OnPropertyChanged(); }
        }

        public short CurrPadMode
        {
            get { return m_currPadMode; }
            set { m_currPadMode = value; OnPropertyChanged(); }
        }

        public bool HasPlayerCheated
        {
            get { return m_hasPlayerCheated; }
            set { m_hasPlayerCheated = value; OnPropertyChanged(); }
        }

        public uint TimeInMilliseconds
        {
            get { return m_timeInMilliseconds; }
            set { m_timeInMilliseconds = value; OnPropertyChanged(); }
        }

        public float TimeScale
        {
            get { return m_TimeScale; }
            set { m_TimeScale = value; OnPropertyChanged(); }
        }

        public float TimeStep
        {
            get { return m_TimeStep; }
            set { m_TimeStep = value; OnPropertyChanged(); }
        }

        public float TimeStepNonClipped
        {
            get { return m_TimeStepNonClipped; }
            set { m_TimeStepNonClipped = value; OnPropertyChanged(); }
        }

        public uint FrameCounter
        {
            get { return m_frameCounter; }
            set { m_frameCounter = value; OnPropertyChanged(); }
        }

        public WeatherType OldWeatherType
        {
            get { return m_oldWeatherType; }
            set { m_oldWeatherType = value; OnPropertyChanged(); }
        }

        public WeatherType NewWeatherType
        {
            get { return m_newWeatherType; }
            set { m_newWeatherType = value; OnPropertyChanged(); }
        }

        public WeatherType ForcedWeatherType
        {
            get { return m_forcedWeatherType; }
            set { m_forcedWeatherType = value; OnPropertyChanged(); }
        }

        public float WeatherInterpolation
        {
            get { return m_weatherInterpolationValue; }
            set { m_weatherInterpolationValue = value; OnPropertyChanged(); }
        }

        public int WeatherTypeInList
        {
            get { return m_weatherTypeInList; }
            set { m_weatherTypeInList = value; OnPropertyChanged(); }
        }

        public float Rain
        {
            get { return m_rain; }
            set { m_rain = value; OnPropertyChanged(); }
        }

        public int CameraCarZoomIndicator
        {
            get { return m_cameraCarZoomIndicator; }
            set { m_cameraCarZoomIndicator = value; OnPropertyChanged(); }
        }

        public int CameraPedZoomIndicator
        {
            get { return m_cameraPedZoomIndicator; }
            set { m_cameraPedZoomIndicator = value; OnPropertyChanged(); }
        }

        public int CurrArea
        {
            get { return m_currArea; }
            set { m_currArea = value; OnPropertyChanged(); }
        }

        public bool InvertLook4Pad
        {
            get { return m_invertLook4Pad; }
            set { m_invertLook4Pad = value; OnPropertyChanged(); }
        }

        public int ExtraColour
        {
            get { return m_extraColour; }
            set { m_extraColour = value; OnPropertyChanged(); }
        }

        public bool ExtraColourOn
        {
            get { return m_extraColourOn; }
            set { m_extraColourOn = value; OnPropertyChanged(); }
        }

        public float ExtraColourInterpolation
        {
            get { return m_extraColourInter; }
            set { m_extraColourInter = value; OnPropertyChanged(); }
        }

        public WeatherType ExtraColourWeatherType
        {
            get { return m_extraColourWeatherType; }
            set { m_extraColourWeatherType = value; OnPropertyChanged(); }
        }

        public int WaterConfiguration
        {
            get { return m_waterConfiguration; }
            set { m_waterConfiguration = value; OnPropertyChanged(); }
        }

        public bool LARiots
        {
            get { return m_laRiots; }
            set { m_laRiots = value; OnPropertyChanged(); }
        }

        public bool LARiotsNoPoliceCars
        {
            get { return m_laRiotsNoPoliceCars; }
            set { m_laRiotsNoPoliceCars = value; OnPropertyChanged(); }
        }

        public int MaximumWantedLevel
        {
            get { return m_maximumWantedLevel; }
            set { m_maximumWantedLevel = value; OnPropertyChanged(); }
        }

        public int MaximumChaosLevel
        {
            get { return m_maximumChaosLevel; }
            set { m_maximumChaosLevel = value; OnPropertyChanged(); }
        }

        public bool GermanGame
        {
            get { return m_germanGame; }
            set { m_germanGame = value; OnPropertyChanged(); }
        }

        public bool FrenchGame
        {
            get { return m_frenchGame; }
            set { m_frenchGame = value; OnPropertyChanged(); }
        }

        public bool NastyGame
        {
            get { return m_nastyGame; }
            set { m_nastyGame = value; OnPropertyChanged(); }
        }

        public byte CinematicCamMessagesLeftToDisplay
        {
            get { return m_cineyCamMessageDisplayed; }
            set { m_cineyCamMessageDisplayed = value; OnPropertyChanged(); }
        }

        public SystemTime TimeLastSaved
        {
            get { return m_timeLastSaved; }
            set { m_timeLastSaved = value; OnPropertyChanged(); }
        }

        public int TargetMarkerHandle
        {
            get { return m_targetMarkerHandle; }
            set { m_targetMarkerHandle = value; OnPropertyChanged(); }
        }

        public bool HasDisplayedPlayerQuitEnterCarHelpText
        {
            get { return m_HasDisplayedPlayerQuitEnterCarHelpText; }
            set { m_HasDisplayedPlayerQuitEnterCarHelpText = value; OnPropertyChanged(); }
        }

        public bool AllTaxisHaveNitro
        {
            get { return m_allTaxisHaveNitro; }
            set { m_allTaxisHaveNitro = value; OnPropertyChanged(); }
        }

        public bool ProstiutesPayYou
        {
            get { return m_prostiutesPayYou; }
            set { m_prostiutesPayYou = value; OnPropertyChanged(); }
        }

        public SimpleVariables()
        {
            LastMissionPassedName = "";
            TimeLastSaved = new SystemTime();
            CameraPosition = new Vector3D();
        }

        protected override void ReadData(StreamBuffer buf, SaveDataFormat fmt)
        {
            VersionId = buf.ReadUInt32();
            LastMissionPassedName = buf.ReadString(Limits.MaxNameLength);
            MissionPackGame = buf.ReadByte();
            buf.Align4Bytes();
            CurrLevel = (LevelType) buf.ReadInt32();
            CameraPosition = buf.Read<Vector3D>();
            MillisecondsPerGameMinute = buf.ReadInt32();
            LastClockTick = buf.ReadUInt32();
            GameClockMonths = buf.ReadByte();
            GameClockDays = buf.ReadByte();
            GameClockHours = buf.ReadByte();
            GameClockMinutes = buf.ReadByte();
            GameClockDayOfWeek = buf.ReadByte();
            StoredGameClockMonths = buf.ReadByte();
            StoredGameClockDays = buf.ReadByte();
            StoredGameClockHours = buf.ReadByte();
            StoredGameClockMinutes = buf.ReadByte();
            ClockHasBeenStored = buf.ReadBool();
            CurrPadMode = buf.ReadInt16();
            HasPlayerCheated = buf.ReadBool();
            buf.Align4Bytes();
            TimeInMilliseconds = buf.ReadUInt32();
            TimeScale = buf.ReadFloat();
            TimeStep = buf.ReadFloat();
            TimeStepNonClipped = buf.ReadFloat();
            FrameCounter = buf.ReadUInt32();
            OldWeatherType = (WeatherType) buf.ReadInt16();
            NewWeatherType = (WeatherType) buf.ReadInt16();
            ForcedWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4Bytes();
            WeatherInterpolation = buf.ReadFloat();
            WeatherTypeInList = buf.ReadInt32();
            Rain = buf.ReadFloat();
            CameraCarZoomIndicator = buf.ReadInt32();
            CameraPedZoomIndicator = buf.ReadInt32();
            CurrArea = buf.ReadInt32();
            InvertLook4Pad = buf.ReadBool();
            buf.Align4Bytes();
            ExtraColour = buf.ReadInt32();
            ExtraColourOn = buf.ReadBool();
            buf.Align4Bytes();
            ExtraColourInterpolation = buf.ReadFloat();
            ExtraColourWeatherType = (WeatherType) buf.ReadInt32();
            WaterConfiguration = buf.ReadInt32();
            LARiots = buf.ReadBool();
            LARiotsNoPoliceCars = buf.ReadBool();
            buf.Align4Bytes();
            MaximumWantedLevel = buf.ReadInt32();
            MaximumChaosLevel = buf.ReadInt32();
            GermanGame = buf.ReadBool();
            FrenchGame = buf.ReadBool();
            NastyGame = buf.ReadBool();
            buf.Align4Bytes();
            buf.Skip(0x2C);
            CinematicCamMessagesLeftToDisplay = buf.ReadByte();
            buf.Skip(1);    // Android: BlurOn
            TimeLastSaved = buf.Read<SystemTime>();
            buf.Align4Bytes();
            TargetMarkerHandle = buf.ReadInt32();
            HasDisplayedPlayerQuitEnterCarHelpText = buf.ReadBool();
            AllTaxisHaveNitro = buf.ReadBool();
            ProstiutesPayYou = buf.ReadBool();
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(StreamBuffer buf, SaveDataFormat fmt)
        {
            buf.Write(VersionId);
            buf.Write(LastMissionPassedName, Limits.MaxNameLength);
            buf.Write(MissionPackGame);
            buf.Align4Bytes();
            buf.Write((int) CurrLevel);
            buf.Write(CameraPosition);
            buf.Write(MillisecondsPerGameMinute);
            buf.Write(LastClockTick);
            buf.Write(GameClockMonths);
            buf.Write(GameClockDays);
            buf.Write(GameClockHours);
            buf.Write(GameClockMinutes);
            buf.Write(GameClockDayOfWeek);
            buf.Write(StoredGameClockMonths);
            buf.Write(StoredGameClockDays);
            buf.Write(StoredGameClockHours);
            buf.Write(StoredGameClockMinutes);
            buf.Write(ClockHasBeenStored);
            buf.Write(CurrPadMode);
            buf.Write(HasPlayerCheated);
            buf.Align4Bytes();
            buf.Write(TimeInMilliseconds);
            buf.Write(TimeScale);
            buf.Write(TimeStep);
            buf.Write(TimeStepNonClipped);
            buf.Write(FrameCounter);
            buf.Write((short) OldWeatherType);
            buf.Write((short) NewWeatherType);
            buf.Write((short) ForcedWeatherType);
            buf.Align4Bytes();
            buf.Write(WeatherInterpolation);
            buf.Write(WeatherTypeInList);
            buf.Write(Rain);
            buf.Write(CameraCarZoomIndicator);
            buf.Write(CameraPedZoomIndicator);
            buf.Write(CurrArea);
            buf.Write(InvertLook4Pad);
            buf.Align4Bytes();
            buf.Write(ExtraColour);
            buf.Write(ExtraColourOn);
            buf.Align4Bytes();
            buf.Write(ExtraColourInterpolation);
            buf.Write((int) ExtraColourWeatherType);
            buf.Write(WaterConfiguration);
            buf.Write(LARiots);
            buf.Write(LARiotsNoPoliceCars);
            buf.Align4Bytes();
            buf.Write(MaximumWantedLevel);
            buf.Write(MaximumChaosLevel);
            buf.Write(GermanGame);
            buf.Write(FrenchGame);
            buf.Write(NastyGame);
            buf.Align4Bytes();
            buf.Skip(0x2C);
            buf.Write(CinematicCamMessagesLeftToDisplay);
            buf.Skip(1);  // Android: BlurOn
            buf.Write(TimeLastSaved);
            buf.Align4Bytes();
            buf.Write(TargetMarkerHandle);
            buf.Write(HasDisplayedPlayerQuitEnterCarHelpText);
            buf.Write(AllTaxisHaveNitro);
            buf.Write(ProstiutesPayYou);
            buf.Align4Bytes();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(SaveDataFormat fmt)
        {
            if (fmt.PC)
            {
                return SizeOfSimpleVariablesPC;
            }

            throw new NotSupportedException();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SimpleVariables);
        }

        public bool Equals(SimpleVariables other)
        {
            if (other == null)
            {
                return false;
            }

            return VersionId.Equals(other.VersionId)
                && LastMissionPassedName.Equals(other.LastMissionPassedName)
                && MissionPackGame.Equals(other.MissionPackGame)
                && CurrLevel.Equals(other.CurrLevel)
                && CameraPosition.Equals(other.CameraPosition)
                && MillisecondsPerGameMinute.Equals(other.MillisecondsPerGameMinute)
                && LastClockTick.Equals(other.LastClockTick)
                && GameClockMonths.Equals(other.GameClockMonths)
                && GameClockDays.Equals(other.GameClockDays)
                && GameClockHours.Equals(other.GameClockHours)
                && GameClockMinutes.Equals(other.GameClockMinutes)
                && GameClockDayOfWeek.Equals(other.GameClockDayOfWeek)
                && StoredGameClockMonths.Equals(other.StoredGameClockMonths)
                && StoredGameClockDays.Equals(other.StoredGameClockDays)
                && StoredGameClockHours.Equals(other.StoredGameClockHours)
                && StoredGameClockMinutes.Equals(other.StoredGameClockMinutes)
                && ClockHasBeenStored.Equals(other.ClockHasBeenStored)
                && CurrPadMode.Equals(other.CurrPadMode)
                && HasPlayerCheated.Equals(other.HasPlayerCheated)
                && TimeInMilliseconds.Equals(other.TimeInMilliseconds)
                && TimeScale.Equals(other.TimeScale)
                && TimeStep.Equals(other.TimeStep)
                && TimeStepNonClipped.Equals(other.TimeStepNonClipped)
                && FrameCounter.Equals(other.FrameCounter)
                && OldWeatherType.Equals(other.OldWeatherType)
                && NewWeatherType.Equals(other.NewWeatherType)
                && ForcedWeatherType.Equals(other.ForcedWeatherType)
                && WeatherInterpolation.Equals(other.WeatherInterpolation)
                && WeatherTypeInList.Equals(other.WeatherTypeInList)
                && Rain.Equals(other.Rain)
                && CameraCarZoomIndicator.Equals(other.CameraCarZoomIndicator)
                && CameraPedZoomIndicator.Equals(other.CameraPedZoomIndicator)
                && CurrArea.Equals(other.CurrArea)
                && InvertLook4Pad.Equals(other.InvertLook4Pad)
                && ExtraColour.Equals(other.ExtraColour)
                && ExtraColourOn.Equals(other.ExtraColourOn)
                && ExtraColourInterpolation.Equals(other.ExtraColourInterpolation)
                && ExtraColourWeatherType.Equals(other.ExtraColourWeatherType)
                && WaterConfiguration.Equals(other.WaterConfiguration)
                && LARiots.Equals(other.LARiots)
                && LARiotsNoPoliceCars.Equals(other.LARiotsNoPoliceCars)
                && MaximumWantedLevel.Equals(other.MaximumWantedLevel)
                && MaximumChaosLevel.Equals(other.MaximumChaosLevel)
                && GermanGame.Equals(other.GermanGame)
                && FrenchGame.Equals(other.FrenchGame)
                && NastyGame.Equals(other.NastyGame)
                && CinematicCamMessagesLeftToDisplay.Equals(other.CinematicCamMessagesLeftToDisplay)
                && TimeLastSaved.Equals(other.TimeLastSaved)
                && TargetMarkerHandle.Equals(other.TargetMarkerHandle)
                && HasDisplayedPlayerQuitEnterCarHelpText.Equals(other.HasDisplayedPlayerQuitEnterCarHelpText)
                && AllTaxisHaveNitro.Equals(other.AllTaxisHaveNitro)
                && ProstiutesPayYou.Equals(other.ProstiutesPayYou);
        }
    }
}
