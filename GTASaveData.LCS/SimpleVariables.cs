using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.LCS
{
    public class SimpleVariables : SaveDataObject,
        IEquatable<SimpleVariables>, IDeepClonable<SimpleVariables>
    {
        public const int MaxMissionPassedNameLength = 60;

        private string m_lastMissionPassedName;
        private int m_currentLevel;
        private int m_currentArea;
        private int m_prefsLanguage;
        private int m_millisecondsPerGameMinute;
        private uint m_lastClockTick;
        private byte m_gameClockHours;
        private byte m_gameClockMinutes;
        private short m_gameClockSeconds;
        private uint m_timeInMilliseconds;
        private float m_timeScale;
        private float m_timeStep;
        private float m_timeStepNonClipped;
        private float m_framesPerUpdate;
        private uint m_frameCounter;
        private WeatherType m_oldWeatherType;
        private WeatherType m_newWeatherType;
        private WeatherType m_forcedWeatherType;
        private int m_weatherTypeInList;
        private float m_weatherInterpolationValue;
        private Vector3D m_cameraPosition;
        private float m_cameraModeInCar;
        private float m_cameraModeOnFoot;
        private int m_extraColor;
        private bool m_isExtraColorOn;
        private float m_extraColorInterpolation;
        private int m_prefsBrightness;
        private bool m_prefsDisplayHud;
        private bool m_prefsShowSubtitles;
        private RadarMode m_prefsRadarMode;
        private bool m_blurOn;  // PSP color filter
        private bool m_prefsUseWideScreen;
        private int m_prefsMusicVolume;
        private int m_prefsSfxVolume;
        private RadioStation m_prefsRadioStation;
        private bool m_prefsStereoMono;
        private short m_padMode;
        private bool m_prefsInvertLook;
        private bool m_prefsUseVibration;
        private bool m_swapNippleAndDPad;
        private bool m_hasPlayerCheated;
        private bool m_allTaxisHaveNitro;
        private bool m_targetIsOn;
        private Vector2D m_targetPosition;
        private Date m_timeStamp;

        public string LastMissionPassedName
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
        }

        public int CurrentLevel
        {
            get { return m_currentLevel; }
            set { m_currentLevel = value; OnPropertyChanged(); }
        }

        public int CurrentArea
        {
            get { return m_currentArea; }
            set { m_currentArea = value; OnPropertyChanged(); }
        }

        public int Language
        {
            get { return m_prefsLanguage; }
            set { m_prefsLanguage = value; OnPropertyChanged(); }
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

        public short GameClockSeconds
        {
            get { return m_gameClockSeconds; }
            set { m_gameClockSeconds = value; OnPropertyChanged(); }
        }

        public uint TimeInMilliseconds
        {
            get { return m_timeInMilliseconds; }
            set { m_timeInMilliseconds = value; OnPropertyChanged(); }
        }

        public float TimeScale
        {
            get { return m_timeScale; }
            set { m_timeScale = value; OnPropertyChanged(); }
        }

        public float TimeStep
        {
            get { return m_timeStep; }
            set { m_timeStep = value; OnPropertyChanged(); }
        }

        public float TimeStepNonClipped
        {
            get { return m_timeStepNonClipped; }
            set { m_timeStepNonClipped = value; OnPropertyChanged(); }
        }

        public float FramesPerUpdate
        {
            get { return m_framesPerUpdate; }
            set { m_framesPerUpdate = value; OnPropertyChanged(); }
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

        public int WeatherTypeInList
        {
            get { return m_weatherTypeInList; }
            set { m_weatherTypeInList = value; OnPropertyChanged(); }
        }

        public float WeatherInterpolationValue
        {
            get { return m_weatherInterpolationValue; }
            set { m_weatherInterpolationValue = value; OnPropertyChanged(); }
        }

        public Vector3D CameraPosition
        {
            get { return m_cameraPosition; }
            set { m_cameraPosition = value; OnPropertyChanged(); }
        }

        public float CameraModeInCar
        {
            get { return m_cameraModeInCar; }
            set { m_cameraModeInCar = value; OnPropertyChanged(); }
        }

        public float CameraModeOnFoot
        {
            get { return m_cameraModeOnFoot; }
            set { m_cameraModeOnFoot = value; OnPropertyChanged(); }
        }

        public int ExtraColor
        {
            get { return m_extraColor; }
            set { m_extraColor = value; OnPropertyChanged(); }
        }

        public bool IsExtraColorOn
        {
            get { return m_isExtraColorOn; }
            set { m_isExtraColorOn = value; OnPropertyChanged(); }
        }

        public float ExtraColorInterpolation
        {
            get { return m_extraColorInterpolation; }
            set { m_extraColorInterpolation = value; OnPropertyChanged(); }
        }

        public int Brightness
        {
            get { return m_prefsBrightness; }
            set { m_prefsBrightness = value; OnPropertyChanged(); }
        }

        public bool DisplayHud
        {
            get { return m_prefsDisplayHud; }
            set { m_prefsDisplayHud = value; OnPropertyChanged(); }
        }

        public bool ShowSubtitles
        {
            get { return m_prefsShowSubtitles; }
            set { m_prefsShowSubtitles = value; OnPropertyChanged(); }
        }

        public RadarMode RadarMode
        {
            get { return m_prefsRadarMode; }
            set { m_prefsRadarMode = value; OnPropertyChanged(); }
        }

        public bool BlurOn
        {
            get { return m_blurOn; }
            set { m_blurOn = value; OnPropertyChanged(); }
        }

        public bool UseWideScreen
        {
            get { return m_prefsUseWideScreen; }
            set { m_prefsUseWideScreen = value; OnPropertyChanged(); }
        }

        public int MusicVolume
        {
            get { return m_prefsMusicVolume; }
            set { m_prefsMusicVolume = value; OnPropertyChanged(); }
        }

        public int SfxVolume
        {
            get { return m_prefsSfxVolume; }
            set { m_prefsSfxVolume = value; OnPropertyChanged(); }
        }

        public RadioStation RadioStation
        {
            get { return m_prefsRadioStation; }
            set { m_prefsRadioStation = value; OnPropertyChanged(); }
        }

        public bool StereoOutput
        {
            get { return m_prefsStereoMono; }
            set { m_prefsStereoMono = value; OnPropertyChanged(); }
        }

        public short PadMode
        {
            get { return m_padMode; }
            set { m_padMode = value; OnPropertyChanged(); }
        }

        public bool InvertLook
        {
            get { return m_prefsInvertLook; }
            set { m_prefsInvertLook = value; OnPropertyChanged(); }
        }

        public bool UseVibration
        {
            get { return m_prefsUseVibration; }
            set { m_prefsUseVibration = value; OnPropertyChanged(); }
        }

        public bool SwapNippleAndDPad
        {
            get { return m_swapNippleAndDPad; }
            set { m_swapNippleAndDPad = value; OnPropertyChanged(); }
        }

        public bool HasPlayerCheated
        {
            get { return m_hasPlayerCheated; }
            set { m_hasPlayerCheated = value; OnPropertyChanged(); }
        }

        public bool AllTaxisHaveNitro
        {
            get { return m_allTaxisHaveNitro; }
            set { m_allTaxisHaveNitro = value; OnPropertyChanged(); }
        }

        public bool TargetIsOn
        {
            get { return m_targetIsOn; }
            set { m_targetIsOn = value; OnPropertyChanged(); }
        }

        public Vector2D TargetPosition
        {
            get { return m_targetPosition; }
            set { m_targetPosition = value; OnPropertyChanged(); }
        }

        public Date TimeStamp
        {
            get { return m_timeStamp; }
            set { m_timeStamp = value; OnPropertyChanged(); }
        }

        public SimpleVariables()
        {
            LastMissionPassedName = "";
            TimeStamp = DateTime.MinValue;
        }

        public SimpleVariables(SimpleVariables other)
        {
            LastMissionPassedName = other.LastMissionPassedName;
            CurrentLevel = other.CurrentLevel;
            CurrentArea = other.CurrentArea;
            Language = other.Language;
            MillisecondsPerGameMinute = other.MillisecondsPerGameMinute;
            LastClockTick = other.LastClockTick;
            GameClockHours = other.GameClockHours;
            GameClockMinutes = other.GameClockMinutes;
            GameClockSeconds = other.GameClockSeconds;
            TimeInMilliseconds = other.TimeInMilliseconds;
            TimeScale = other.TimeScale;
            TimeStep = other.TimeStep;
            TimeStepNonClipped = other.TimeStepNonClipped;
            FramesPerUpdate = other.FramesPerUpdate;
            FrameCounter = other.FrameCounter;
            OldWeatherType = other.OldWeatherType;
            NewWeatherType = other.NewWeatherType;
            ForcedWeatherType = other.ForcedWeatherType;
            WeatherTypeInList = other.WeatherTypeInList;
            WeatherInterpolationValue = other.WeatherInterpolationValue;
            CameraPosition = other.CameraPosition;
            CameraModeInCar = other.CameraModeInCar;
            CameraModeOnFoot = other.CameraModeOnFoot;
            ExtraColor = other.ExtraColor;
            IsExtraColorOn = other.IsExtraColorOn;
            ExtraColorInterpolation = other.ExtraColorInterpolation;
            Brightness = other.Brightness;
            DisplayHud = other.DisplayHud;
            ShowSubtitles = other.ShowSubtitles;
            RadarMode = other.RadarMode;
            BlurOn = other.BlurOn;
            UseWideScreen = other.UseWideScreen;
            MusicVolume = other.MusicVolume;
            SfxVolume = other.SfxVolume;
            RadioStation = other.RadioStation;
            StereoOutput = other.StereoOutput;
            PadMode = other.PadMode;
            InvertLook = other.InvertLook;
            UseVibration = other.UseVibration;
            SwapNippleAndDPad = other.SwapNippleAndDPad;
            HasPlayerCheated = other.HasPlayerCheated;
            AllTaxisHaveNitro = other.AllTaxisHaveNitro;
            TargetIsOn = other.TargetIsOn;
            TargetPosition = other.TargetPosition;
            TimeStamp = other.TimeStamp;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Skip(8); // unused
            if (fmt.IsMobile)
            {
                buf.Skip(4); // unused
                LastMissionPassedName = buf.ReadString(MaxMissionPassedNameLength, unicode: true);
            }
            CurrentLevel = buf.ReadInt32();
            CurrentArea = buf.ReadInt32();
            Language = buf.ReadInt32();
            MillisecondsPerGameMinute = buf.ReadInt32();
            LastClockTick = buf.ReadUInt32();
            GameClockHours = buf.ReadByte();
            GameClockMinutes = buf.ReadByte();
            GameClockSeconds = buf.ReadInt16();
            TimeInMilliseconds = buf.ReadUInt32();
            TimeScale = buf.ReadFloat();
            TimeStep = buf.ReadFloat();
            TimeStepNonClipped = buf.ReadFloat();
            FramesPerUpdate = buf.ReadFloat();
            FrameCounter = buf.ReadUInt32();
            OldWeatherType = (WeatherType) buf.ReadInt16();
            NewWeatherType = (WeatherType) buf.ReadInt16();
            ForcedWeatherType = (WeatherType) buf.ReadInt16();
            buf.Skip(2);
            WeatherTypeInList = buf.ReadInt32();
            WeatherInterpolationValue = buf.ReadFloat();
            CameraPosition = buf.Read<Vector3D>();
            CameraModeInCar = buf.ReadFloat();
            CameraModeOnFoot = buf.ReadFloat();
            ExtraColor = buf.ReadInt32();       // for interiors, I think
            IsExtraColorOn = buf.ReadBool(4);
            ExtraColorInterpolation = buf.ReadFloat();
            Brightness = buf.ReadInt32();
            DisplayHud = buf.ReadBool();
            ShowSubtitles = buf.ReadBool();
            buf.Skip(2);
            RadarMode = (RadarMode) buf.ReadInt32();
            if (fmt.IsPS2)
            {
                BlurOn = buf.ReadBool(4);
                buf.Skip(8); // possibly 2 unused settings
                UseWideScreen = buf.ReadBool(4);
            }
            else
            {
                BlurOn = buf.ReadBool();
                buf.Skip(3);
            }
            MusicVolume = buf.ReadInt32();
            SfxVolume = buf.ReadInt32();
            RadioStation = (RadioStation) buf.ReadByte();
            StereoOutput = buf.ReadBool();
            buf.Skip(2);
            buf.Skip(10 * 4); // unused
            if (!fmt.IsPSP) buf.Skip(4); // unused
            PadMode = buf.ReadInt16();
            if (fmt.IsPS2)
            {
                buf.Skip(2);
                InvertLook = !buf.ReadBool(4);  // negated
                UseVibration = buf.ReadBool();
                buf.Skip(3);
                HasPlayerCheated = buf.ReadBool(4);
                AllTaxisHaveNitro = buf.ReadBool(4);
                TargetIsOn = buf.ReadBool();
                buf.Skip(3);
            }
            else
            {
                InvertLook = buf.ReadBool();
                if (fmt.IsPSP) InvertLook = !InvertLook;
                SwapNippleAndDPad = buf.ReadBool();
                HasPlayerCheated = buf.ReadBool();
                AllTaxisHaveNitro = buf.ReadBool();
                TargetIsOn = buf.ReadBool();
                buf.Skip(1);
            }
            TargetPosition = buf.Read<Vector2D>();
            if (fmt.IsPS2)
            {
                buf.Skip(4);    // unused
                TimeStamp = buf.Read<Date>();
            }

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Skip(4); // unused
            if (fmt.IsPS2 || fmt.IsPSP) buf.Write(3);
            else buf.Write(8);
            if (fmt.IsMobile)
            {
                buf.Skip(4); // unused
                buf.Write(LastMissionPassedName, MaxMissionPassedNameLength, unicode: true);
            }
            buf.Write(CurrentLevel);
            buf.Write(CurrentArea);
            buf.Write(Language);
            buf.Write(MillisecondsPerGameMinute);
            buf.Write(LastClockTick);
            buf.Write(GameClockHours);
            buf.Write(GameClockMinutes);
            buf.Write(GameClockSeconds);
            buf.Write(TimeInMilliseconds);
            buf.Write(TimeScale);
            buf.Write(TimeStep);
            buf.Write(TimeStepNonClipped);
            buf.Write(FramesPerUpdate);
            buf.Write(FrameCounter);
            buf.Write((short) OldWeatherType);
            buf.Write((short) NewWeatherType);
            buf.Write((short) ForcedWeatherType);
            buf.Skip(2);
            buf.Write(WeatherTypeInList);
            buf.Write(WeatherInterpolationValue);
            buf.Write(CameraPosition);
            buf.Write(CameraModeInCar);
            buf.Write(CameraModeOnFoot);
            buf.Write(ExtraColor);
            buf.Write(IsExtraColorOn, 4);
            buf.Write(ExtraColorInterpolation);
            buf.Write(Brightness);
            buf.Write(DisplayHud);
            buf.Write(ShowSubtitles);
            buf.Skip(2);
            buf.Write((int) RadarMode);
            if (fmt.IsPS2)
            {
                buf.Write(BlurOn, 4);
                buf.Skip(8); // possibly 2 unused settings
                buf.Write(UseWideScreen, 4);
            }
            else
            {
                buf.Write(BlurOn);
                buf.Skip(3);
            }
            buf.Write(MusicVolume);
            buf.Write(SfxVolume);
            buf.Write((byte) RadioStation);
            buf.Write(StereoOutput);
            buf.Skip(2);
            buf.Skip(10 * 4); // unused
            if (!fmt.IsPSP) buf.Skip(4); // unused
            buf.Write(PadMode);
            if (fmt.IsPS2)
            {
                buf.Skip(2);
                buf.Write(!InvertLook, 4);  // negated
                buf.Write(UseVibration);
                buf.Skip(3);
                buf.Write(HasPlayerCheated, 4);
                buf.Write(AllTaxisHaveNitro, 4);
                buf.Write(TargetIsOn);
                buf.Skip(3);
            }
            else
            {
                bool invertLook = InvertLook;
                if (fmt.IsPSP) invertLook = !invertLook;
                buf.Write(invertLook);
                buf.Write(SwapNippleAndDPad);
                buf.Write(HasPlayerCheated);
                buf.Write(AllTaxisHaveNitro);
                buf.Write(TargetIsOn);
                buf.Skip(1);
            }
            buf.Write(TargetPosition);
            if (fmt.IsPS2)
            {
                buf.Skip(4);    // unused
                buf.Write(TimeStamp);
            }

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPSP) return 0xBC;
            if (fmt.IsPS2) return 0xF8;
            if (fmt.IsMobile) return 0x13C;
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

            return LastMissionPassedName.Equals(other.LastMissionPassedName)
                && CurrentLevel.Equals(other.CurrentLevel)
                && CurrentArea.Equals(other.CurrentArea)
                && Language.Equals(other.Language)
                && MillisecondsPerGameMinute.Equals(other.MillisecondsPerGameMinute)
                && LastClockTick.Equals(other.LastClockTick)
                && GameClockHours.Equals(other.GameClockHours)
                && GameClockMinutes.Equals(other.GameClockMinutes)
                && GameClockSeconds.Equals(other.GameClockSeconds)
                && TimeInMilliseconds.Equals(other.TimeInMilliseconds)
                && TimeScale.Equals(other.TimeScale)
                && TimeStep.Equals(other.TimeStep)
                && TimeStepNonClipped.Equals(other.TimeStepNonClipped)
                && FramesPerUpdate.Equals(other.FramesPerUpdate)
                && FrameCounter.Equals(other.FrameCounter)
                && OldWeatherType.Equals(other.OldWeatherType)
                && NewWeatherType.Equals(other.NewWeatherType)
                && ForcedWeatherType.Equals(other.ForcedWeatherType)
                && WeatherTypeInList.Equals(other.WeatherTypeInList)
                && WeatherInterpolationValue.Equals(other.WeatherInterpolationValue)
                && CameraPosition.Equals(other.CameraPosition)
                && CameraModeInCar.Equals(other.CameraModeInCar)
                && CameraModeOnFoot.Equals(other.CameraModeOnFoot)
                && ExtraColor.Equals(other.ExtraColor)
                && IsExtraColorOn.Equals(other.IsExtraColorOn)
                && ExtraColorInterpolation.Equals(other.ExtraColorInterpolation)
                && Brightness.Equals(other.Brightness)
                && DisplayHud.Equals(other.DisplayHud)
                && ShowSubtitles.Equals(other.ShowSubtitles)
                && RadarMode.Equals(other.RadarMode)
                && BlurOn.Equals(other.BlurOn)
                && UseWideScreen.Equals(other.UseWideScreen)
                && MusicVolume.Equals(other.MusicVolume)
                && SfxVolume.Equals(other.SfxVolume)
                && RadioStation.Equals(other.RadioStation)
                && StereoOutput.Equals(other.StereoOutput)
                && PadMode.Equals(other.PadMode)
                && InvertLook.Equals(other.InvertLook)
                && UseVibration.Equals(other.UseVibration)
                && SwapNippleAndDPad.Equals(other.SwapNippleAndDPad)
                && HasPlayerCheated.Equals(other.HasPlayerCheated)
                && AllTaxisHaveNitro.Equals(other.AllTaxisHaveNitro)
                && TargetIsOn.Equals(other.TargetIsOn)
                && TargetPosition.Equals(other.TargetPosition)
                && TimeStamp.Equals(other.TimeStamp);
        }

        public SimpleVariables DeepClone()
        {
            return new SimpleVariables(this);
        }
    }

    public enum DataBlock
    {
        SimpleVars,
        Scripts,
        Garages,
        PlayerInfo,
        Stats
    }

    public enum WeatherType
    {
        Sunny,
        Cloudy,
        Rainy,
        Foggy,
        ExtraSunny,
        Hurricane,
        ExtraColours,
        Snowy       // PSP only :(
    }

    public enum RadarMode
    {
        // TOOD: confirm
        MapAndBlips,
        BlipsOnly,
        RadarOff,
    }
}
