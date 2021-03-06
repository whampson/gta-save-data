﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.VC
{
    public class SimpleVariables : SaveDataObject, ISimpleVariables,
        IEquatable<SimpleVariables>, IDeepClonable<SimpleVariables>
    {
        public const int MaxMissionPassedNameLength = 24;
        public const int MaxMissionPassedNameLengthMobile = 28;
        public const int RadioStationListCount = 10;
        public const int SteamMagic = 0x3DF5C2FD;

        private int m_saveVersionNumber;
        private string m_lastMissionPassedName;
        private SystemTime m_timeStamp;
        private int m_sizeOfGameInBytes;
        private Level m_currLevel;
        private Vector3 m_cameraPosition;
        private int m_steamMagicNumber;
        private int m_millisecondsPerGameMinute;
        private uint m_lastClockTick;
        private byte m_gameClockHours;
        private byte m_gameClockMinutes;
        private short m_currPadMode;
        private uint m_timeInMilliseconds;
        private float m_timerTimeScale;
        private float m_timerTimeStep;
        private float m_timerTimeStepNonClipped;
        private uint m_frameCounter;
        private float m_timeStep;
        private float m_framesPerUpdate;
        private float m_timeScale;
        private WeatherType m_oldWeatherType;
        private WeatherType m_newWeatherType;
        private WeatherType m_forcedWeatherType;
        private float m_weatherInterpolationValue;
        private int m_weatherTypeInList;
        private float m_cameraCarZoomIndicator;
        private float m_cameraPedZoomIndicator;
        private Interior m_currArea;
        private bool m_allTaxisHaveNitro;
        private bool m_invertLook4Pad;
        private int m_extraColour;
        private bool m_extraColourOn;
        private float m_extraColourInter;
        private Array<int> m_radioStationPositionList;

        public int SaveVersionNumber
        {
            get { return m_saveVersionNumber; }
            set { m_saveVersionNumber = value; }
        }

        public string LastMissionPassedName
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
        }

        public SystemTime TimeStamp
        {
            get { return m_timeStamp; }
            set { m_timeStamp = value; OnPropertyChanged(); }
        }

        public int SizeOfGameInBytes
        {
            get { return m_sizeOfGameInBytes; }
            set { m_sizeOfGameInBytes = value; }
        }

        public Level CurrLevel
        {
            get { return m_currLevel; }
            set { m_currLevel = value; OnPropertyChanged(); }
        }

        public Vector3 CameraPosition
        {
            get { return m_cameraPosition; }
            set { m_cameraPosition = value; OnPropertyChanged(); }
        }

        public int SteamMagicNumber
        {
            get { return m_steamMagicNumber; }
            set { m_steamMagicNumber = value; OnPropertyChanged(); }
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

        public short CurrPadMode
        {
            get { return m_currPadMode; }
            set { m_currPadMode = value; OnPropertyChanged(); }
        }

        public uint TimeInMilliseconds
        {
            get { return m_timeInMilliseconds; }
            set { m_timeInMilliseconds = value; OnPropertyChanged(); }
        }

        public float TimerTimeScale
        {
            get { return m_timerTimeScale; }
            set { m_timerTimeScale = value; OnPropertyChanged(); }
        }

        public float TimerTimeStep
        {
            get { return m_timerTimeStep; }
            set { m_timerTimeStep = value; OnPropertyChanged(); }
        }

        public float TimerTimeStepNonClipped
        {
            get { return m_timerTimeStepNonClipped; }
            set { m_timerTimeStepNonClipped = value; OnPropertyChanged(); }
        }

        public uint FrameCounter
        {
            get { return m_frameCounter; }
            set { m_frameCounter = value; OnPropertyChanged(); }
        }

        public float TimeStep
        {
            get { return m_timeStep; }
            set { m_timeStep = value; OnPropertyChanged(); }
        }

        public float FramesPerUpdate
        {
            get { return m_framesPerUpdate; }
            set { m_framesPerUpdate = value; OnPropertyChanged(); }
        }

        public float TimeScale
        {
            get { return m_timeScale; }
            set { m_timeScale = value; OnPropertyChanged(); }
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

        public float CameraCarZoomIndicator
        {
            get { return m_cameraCarZoomIndicator; }
            set { m_cameraCarZoomIndicator = value; OnPropertyChanged(); }
        }

        public float CameraPedZoomIndicator
        {
            get { return m_cameraPedZoomIndicator; }
            set { m_cameraPedZoomIndicator = value; OnPropertyChanged(); }
        }

        public Interior CurrArea
        {
            get { return m_currArea; }
            set { m_currArea = value; OnPropertyChanged(); }
        }

        public bool AllTaxisHaveNitro
        {
            get { return m_allTaxisHaveNitro; }
            set { m_allTaxisHaveNitro = value; OnPropertyChanged(); }
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

        public Array<int> RadioStationPositionList
        {
            get { return m_radioStationPositionList; }
            set { m_radioStationPositionList = value; OnPropertyChanged(); }
        }

        int ISimpleVariables.GameClockHours
        {
            get { return GameClockHours; }
            set { GameClockHours = (byte) value; OnPropertyChanged(); }
        }

        int ISimpleVariables.GameClockMinutes
        {
            get { return GameClockMinutes; }
            set { GameClockMinutes = (byte) value; OnPropertyChanged(); }
        }

        int ISimpleVariables.OldWeatherType
        {
            get { return (int) OldWeatherType; }
            set { OldWeatherType = (WeatherType) value; OnPropertyChanged(); }
        }

        int ISimpleVariables.NewWeatherType
        {
            get { return (int) NewWeatherType; }
            set { NewWeatherType = (WeatherType) value; OnPropertyChanged(); }
        }

        int ISimpleVariables.ForcedWeatherType
        {
            get { return (int) ForcedWeatherType; }
            set { ForcedWeatherType = (WeatherType) value; OnPropertyChanged(); }
        }

        public SimpleVariables()
        {
            LastMissionPassedName = "";
            TimeStamp = SystemTime.MinValue;
            RadioStationPositionList = new Array<int>();
        }

        public SimpleVariables(SimpleVariables other)
        {
            SaveVersionNumber = other.SaveVersionNumber;
            LastMissionPassedName = other.LastMissionPassedName;
            TimeStamp = other.TimeStamp;
            SizeOfGameInBytes = other.SizeOfGameInBytes;
            CurrLevel = other.CurrLevel;
            CameraPosition = other.CameraPosition;
            SteamMagicNumber = other.SteamMagicNumber;
            MillisecondsPerGameMinute = other.MillisecondsPerGameMinute;
            LastClockTick = other.LastClockTick;
            GameClockHours = other.GameClockHours;
            GameClockMinutes = other.GameClockMinutes;
            CurrPadMode = other.CurrPadMode;
            TimeInMilliseconds = other.TimeInMilliseconds;
            TimerTimeScale = other.TimerTimeScale;
            TimerTimeStep = other.TimerTimeStep;
            TimerTimeStepNonClipped = other.TimerTimeStepNonClipped;
            FrameCounter = other.FrameCounter;
            TimeStep = other.TimeStep;
            FramesPerUpdate = other.FramesPerUpdate;
            TimeScale = other.TimeScale;
            OldWeatherType = other.OldWeatherType;
            NewWeatherType = other.NewWeatherType;
            ForcedWeatherType = other.ForcedWeatherType;
            WeatherInterpolation = other.WeatherInterpolation;
            WeatherTypeInList = other.WeatherTypeInList;
            CameraCarZoomIndicator = other.CameraCarZoomIndicator;
            CameraPedZoomIndicator = other.CameraPedZoomIndicator;
            CurrArea = other.CurrArea;
            AllTaxisHaveNitro = other.AllTaxisHaveNitro;
            InvertLook4Pad = other.InvertLook4Pad;
            ExtraColour = other.ExtraColour;
            ExtraColourOn = other.ExtraColourOn;
            ExtraColourInterpolation = other.ExtraColourInterpolation;
            RadioStationPositionList = other.RadioStationPositionList;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            if (fmt.IsMobile)
            {
                SaveVersionNumber = buf.ReadInt32();
                LastMissionPassedName = buf.ReadString(MaxMissionPassedNameLengthMobile, unicode: true);
            }
            else
            {
                LastMissionPassedName = buf.ReadString(MaxMissionPassedNameLength, unicode: true);
                TimeStamp = buf.ReadStruct<SystemTime>();
            }
            SizeOfGameInBytes = buf.ReadInt32();
            CurrLevel = (Level) buf.ReadInt32();
            CameraPosition = buf.ReadStruct<Vector3>();
            if (fmt.IsPC && fmt.IsSteam) SteamMagicNumber = buf.ReadInt32();
            MillisecondsPerGameMinute = buf.ReadInt32();
            LastClockTick = buf.ReadUInt32();
            GameClockHours = (byte) buf.ReadInt32();
            buf.Align4();
            GameClockMinutes = (byte) buf.ReadInt32();
            buf.Align4();
            CurrPadMode = buf.ReadInt16();
            buf.Align4();
            TimeInMilliseconds = buf.ReadUInt32();
            TimerTimeScale = buf.ReadFloat();
            TimerTimeStep = buf.ReadFloat();
            TimerTimeStepNonClipped = buf.ReadFloat();
            FrameCounter = buf.ReadUInt32();
            TimeStep = buf.ReadFloat();
            FramesPerUpdate = buf.ReadFloat();
            TimeScale = buf.ReadFloat();
            OldWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4();
            NewWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4();
            ForcedWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4();
            WeatherInterpolation = buf.ReadFloat();
            WeatherTypeInList = buf.ReadInt32();
            CameraCarZoomIndicator = buf.ReadFloat();
            CameraPedZoomIndicator = buf.ReadFloat();
            CurrArea = (Interior) buf.ReadInt32();
            AllTaxisHaveNitro = buf.ReadBool();
            buf.Align4();
            InvertLook4Pad = buf.ReadBool();
            buf.Align4();
            ExtraColour = buf.ReadInt32();
            ExtraColourOn = buf.ReadBool(4);
            ExtraColourInterpolation = buf.ReadFloat();
            RadioStationPositionList = buf.ReadArray<int>(RadioStationListCount);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            if (fmt.IsMobile)
            {
                buf.Write(SaveVersionNumber);
                buf.Write(LastMissionPassedName, MaxMissionPassedNameLengthMobile, unicode: true);
            }
            else
            {
                buf.Write(LastMissionPassedName, MaxMissionPassedNameLength, unicode: true);
                buf.Write(TimeStamp);
            }
            buf.Write(SizeOfGameInBytes);
            buf.Write((int) CurrLevel);
            buf.Write(CameraPosition);
            if (fmt.IsPC && fmt.IsSteam) buf.Write(SteamMagicNumber);
            buf.Write(MillisecondsPerGameMinute);
            buf.Write(LastClockTick);
            buf.Write(GameClockHours);
            buf.Align4();
            buf.Write(GameClockMinutes);
            buf.Align4();
            buf.Write(CurrPadMode);
            buf.Align4();
            buf.Write(TimeInMilliseconds);
            buf.Write(TimerTimeScale);
            buf.Write(TimerTimeStep);
            buf.Write(TimerTimeStepNonClipped);
            buf.Write(FrameCounter);
            buf.Write(TimeStep);
            buf.Write(FramesPerUpdate);
            buf.Write(TimeScale);
            buf.Write((short) OldWeatherType);
            buf.Align4();
            buf.Write((short) NewWeatherType);
            buf.Align4();
            buf.Write((short) ForcedWeatherType);
            buf.Align4();
            buf.Write(WeatherInterpolation);
            buf.Write(WeatherTypeInList);
            buf.Write(CameraCarZoomIndicator);
            buf.Write(CameraPedZoomIndicator);
            buf.Write((int) CurrArea);
            buf.Write(AllTaxisHaveNitro);
            buf.Align4();
            buf.Write(InvertLook4Pad);
            buf.Align4();
            buf.Write(ExtraColour);
            buf.Write(ExtraColourOn, 4);
            buf.Write(ExtraColourInterpolation);
            buf.Write(RadioStationPositionList, RadioStationListCount);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsMobile)
            {
                return 0xE0;
            }
            else if (fmt.IsPC)
            {
                if (fmt.IsSteam)
                {
                    return 0xE8;
                }
                return 0xE4;
            }

            throw SizeNotDefined(fmt);
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

            return SaveVersionNumber.Equals(other.SaveVersionNumber)
                && LastMissionPassedName.Equals(other.LastMissionPassedName)
                && TimeStamp.Equals(other.TimeStamp)
                && SizeOfGameInBytes.Equals(other.SizeOfGameInBytes)
                && CurrLevel.Equals(other.CurrLevel)
                && CameraPosition.Equals(other.CameraPosition)
                && SteamMagicNumber.Equals(other.SteamMagicNumber)
                && MillisecondsPerGameMinute.Equals(other.MillisecondsPerGameMinute)
                && LastClockTick.Equals(other.LastClockTick)
                && GameClockHours.Equals(other.GameClockHours)
                && GameClockMinutes.Equals(other.GameClockMinutes)
                && CurrPadMode.Equals(other.CurrPadMode)
                && TimeInMilliseconds.Equals(other.TimeInMilliseconds)
                && TimerTimeScale.Equals(other.TimerTimeScale)
                && TimerTimeStep.Equals(other.TimerTimeStep)
                && TimerTimeStepNonClipped.Equals(other.TimerTimeStepNonClipped)
                && FrameCounter.Equals(other.FrameCounter)
                && TimeStep.Equals(other.TimeStep)
                && FramesPerUpdate.Equals(other.FramesPerUpdate)
                && TimeScale.Equals(other.TimeScale)
                && OldWeatherType.Equals(other.OldWeatherType)
                && NewWeatherType.Equals(other.NewWeatherType)
                && ForcedWeatherType.Equals(other.ForcedWeatherType)
                && WeatherInterpolation.Equals(other.WeatherInterpolation)
                && WeatherTypeInList.Equals(other.WeatherTypeInList)
                && CameraCarZoomIndicator.Equals(other.CameraCarZoomIndicator)
                && CameraPedZoomIndicator.Equals(other.CameraPedZoomIndicator)
                && CurrArea.Equals(other.CurrArea)
                && AllTaxisHaveNitro.Equals(other.AllTaxisHaveNitro)
                && InvertLook4Pad.Equals(other.InvertLook4Pad)
                && ExtraColour.Equals(other.ExtraColour)
                && ExtraColourOn.Equals(other.ExtraColourOn)
                && ExtraColourInterpolation.Equals(other.ExtraColourInterpolation)
                && RadioStationPositionList.SequenceEqual(other.RadioStationPositionList);
        }

        public SimpleVariables DeepClone()
        {
            return new SimpleVariables(this);
        }
    }

    public enum Interior
    {
        None,
        Hotel,
        Mansion,
        Bank,
        Mall,
        StripClub,
        Lawyers,
        CoffeeShop,
        ConcertHall,
        Studio,
        RifleRange,
        BikerBar,
        PoliceStation,
        Everywhere,
        Dirt,
        Blood,
        OvalRing,
        MalibuClub,
        PrintWorks
    }

    public enum Level
    {
        None,
        Beach,
        Mainland,
    }

    public enum Language
    {
        English,
        French,
        German,
        Italian,
        Spanish,
        Japanese,
    }

    public enum WeatherType
    {
        None = -1,
        Sunny,
        Cloudy,
        Rainy,
        Foggy,
        ExtraSunny,
        Hurricane,
        ExtraColours
    }
}
