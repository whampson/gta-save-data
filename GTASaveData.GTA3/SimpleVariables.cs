using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3
{
    public class SimpleVariables : SaveDataObject, ISimpleVariables,
        IEquatable<SimpleVariables>, IDeepClonable<SimpleVariables>
    {
        public const int MaxMissionPassedNameLength = 24;

        private string m_lastMissionPassedName;
        private SystemTime m_timeStamp;
        private int m_sizeOfGameInBytes;
        private Level m_currLevel;
        private Vector3D m_cameraPosition;
        private int m_millisecondsPerGameMinute;
        private uint m_lastClockTick;
        private byte m_gameClockHours;
        private byte m_gameClockMinutes;
        private short m_currPadMode;
        private uint m_timeInMilliseconds;
        private float m_timeScale;
        private float m_timeStep;
        private float m_timeStepNonClipped;
        private uint m_frameCounter;
        private float m_timeStep2;
        private float m_framesPerUpdate;
        private float m_timeScale2;
        private WeatherType m_oldWeatherType;
        private WeatherType m_newWeatherType;
        private WeatherType m_forcedWeatherType;
        private float m_weatherInterpolationValue;
        private int m_prefsMusicVolume;
        private int m_prefsSfxVolume;
        private bool m_prefsUseVibration;
        private bool m_prefsStereoMono;
        private RadioStation m_prefsRadioStation;
        private int m_prefsBrightness;
        private bool m_prefsShowSubtitles;
        private Language m_prefsLanguage;
        private bool m_prefsUseWideScreen;
        private bool m_blurOn;
        private Date m_compileDateAndTime;
        private int m_weatherTypeInList;
        private float m_cameraCarZoomIndicator;
        private float m_cameraPedZoomIndicator;
        private QuickSaveState m_isQuickSave;

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

        public Level CurrentLevel
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

        public uint FrameCounter
        {
            get { return m_frameCounter; }
            set { m_frameCounter = value; OnPropertyChanged(); }
        }

        [Obsolete("Not used by the game.")]
        public float TimeStep2
        {
            get { return m_timeStep2; }
            set { m_timeStep2 = value; OnPropertyChanged(); }
        }

        [Obsolete("Not used by the game.")]
        public float FramesPerUpdate
        {
            get { return m_framesPerUpdate; }
            set { m_framesPerUpdate = value; OnPropertyChanged(); }
        }

        [Obsolete("Not used by the game.")]
        public float TimeScale2
        {
            get { return m_timeScale2; }
            set { m_timeScale2 = value; OnPropertyChanged(); }
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

        public bool UseVibration
        {
            get { return m_prefsUseVibration; }
            set { m_prefsUseVibration = value; OnPropertyChanged(); }
        }

        public bool StereoOutput
        {
            get { return m_prefsStereoMono; }
            set { m_prefsStereoMono = value; OnPropertyChanged(); }
        }

        public RadioStation RadioStation
        {
            get { return m_prefsRadioStation; }
            set { m_prefsRadioStation = value; OnPropertyChanged(); }
        }

        public int Brightness
        {
            get { return m_prefsBrightness; }
            set { m_prefsBrightness = value; OnPropertyChanged(); }
        }

        public bool ShowSubtitles
        {
            get { return m_prefsShowSubtitles; }
            set { m_prefsShowSubtitles = value; OnPropertyChanged(); }
        }

        public Language Language
        {
            get { return m_prefsLanguage; }
            set { m_prefsLanguage = value; OnPropertyChanged(); }
        }

        public bool UseWideScreen
        {
            get { return m_prefsUseWideScreen; }
            set { m_prefsUseWideScreen = value; OnPropertyChanged(); }
        }

        public bool BlurOn
        {
            get { return m_blurOn; }
            set { m_blurOn = value; OnPropertyChanged(); }
        }


        [Obsolete("Not used by the game.")]
        public Date CompileDateAndTime
        {
            get { return m_compileDateAndTime; }
            set { m_compileDateAndTime = value; OnPropertyChanged(); }
        }

        public int WeatherTypeInList
        {
            get { return m_weatherTypeInList; }
            set { m_weatherTypeInList = value; OnPropertyChanged(); }
        }

        public float CameraModeInCar
        {
            get { return m_cameraCarZoomIndicator; }
            set { m_cameraCarZoomIndicator = value; OnPropertyChanged(); }
        }

        public float CameraModeOnFoot
        {
            get { return m_cameraPedZoomIndicator; }
            set { m_cameraPedZoomIndicator = value; OnPropertyChanged(); }
        }

        public QuickSaveState IsQuickSave
        {
            get { return m_isQuickSave; }
            set { m_isQuickSave = value; OnPropertyChanged(); }
        }

        public SimpleVariables()
        {
            LastMissionPassedName = "";
            TimeStamp = DateTime.MinValue;
            CompileDateAndTime = DateTime.MinValue;
            CameraPosition = new Vector3D();
        }

        public SimpleVariables(SimpleVariables other)
        {
            LastMissionPassedName = other.LastMissionPassedName;
            TimeStamp = other.TimeStamp;
            SizeOfGameInBytes = other.SizeOfGameInBytes;
            CurrentLevel = other.CurrentLevel;
            CameraPosition = other.CameraPosition;
            MillisecondsPerGameMinute = other.MillisecondsPerGameMinute;
            LastClockTick = other.LastClockTick;
            GameClockHours = other.GameClockHours;
            GameClockMinutes = other.GameClockMinutes;
            CurrPadMode = other.CurrPadMode;
            TimeInMilliseconds = other.TimeInMilliseconds;
            TimeScale = other.TimeScale;
            TimeStep = other.TimeStep;
            TimeStepNonClipped = other.TimeStepNonClipped;
            FrameCounter = other.FrameCounter;
            TimeStep2 = other.TimeStep2;
            FramesPerUpdate = other.FramesPerUpdate;
            TimeScale2 = other.TimeScale2;
            OldWeatherType = other.OldWeatherType;
            NewWeatherType = other.NewWeatherType;
            ForcedWeatherType = other.ForcedWeatherType;
            WeatherInterpolation = other.WeatherInterpolation;
            MusicVolume = other.MusicVolume;
            SfxVolume = other.SfxVolume;
            UseVibration = other.UseVibration;
            StereoOutput = other.StereoOutput;
            RadioStation = other.RadioStation;
            Brightness = other.Brightness;
            ShowSubtitles = other.ShowSubtitles;
            Language = other.Language;
            UseWideScreen = other.UseWideScreen;
            BlurOn = other.BlurOn;
            CompileDateAndTime = other.CompileDateAndTime;
            WeatherTypeInList = other.WeatherTypeInList;
            CameraModeInCar = other.CameraModeInCar;
            CameraModeOnFoot = other.CameraModeOnFoot;
            IsQuickSave = other.IsQuickSave;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            if (!fmt.IsPS2) LastMissionPassedName = buf.ReadString(MaxMissionPassedNameLength, unicode: true);
            if (fmt.IsPC || fmt.IsXbox) TimeStamp = buf.Read<SystemTime>();
            SizeOfGameInBytes = buf.ReadInt32();
            CurrentLevel = (Level) buf.ReadInt32();
            CameraPosition = buf.Read<Vector3D>();
            MillisecondsPerGameMinute = buf.ReadInt32();
            LastClockTick = buf.ReadUInt32();
            GameClockHours = (byte) buf.ReadInt32();
            buf.Align4();
            GameClockMinutes = (byte) buf.ReadInt32();
            buf.Align4();
            CurrPadMode = buf.ReadInt16();
            buf.Align4();
            TimeInMilliseconds = buf.ReadUInt32();
            TimeScale = buf.ReadFloat();
            TimeStep = buf.ReadFloat();
            TimeStepNonClipped = buf.ReadFloat();
            FrameCounter = buf.ReadUInt32();
            TimeStep2 = buf.ReadFloat();
            FramesPerUpdate = buf.ReadFloat();
            TimeScale2 = buf.ReadFloat();
            OldWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4();
            NewWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4();
            ForcedWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4();
            WeatherInterpolation = buf.ReadFloat();
            if (fmt.IsPS2)
            {
                MusicVolume = buf.ReadInt32();
                SfxVolume = buf.ReadInt32();
                if (!fmt.IsAustralian)
                {
                    buf.ReadInt16();    // duplicate of CurrPadMode
                    buf.Align4();
                }
                UseVibration = buf.ReadBool();
                buf.Align4();
                StereoOutput = buf.ReadBool();
                buf.Align4();
                RadioStation = (RadioStation) buf.ReadByte();
                buf.Align4();
                Brightness = buf.ReadInt32();
                if (!fmt.IsAustralian)
                {
                    buf.ReadBool();     // duplicate of BlurOn
                    buf.Align4();
                }
                ShowSubtitles = buf.ReadBool();
                buf.Align4();
                Language = (Language) buf.ReadInt32();
                UseWideScreen = buf.ReadBool();
                buf.Align4();
                buf.ReadInt16();        // duplicate of CurrPadMode
                buf.Align4();
                BlurOn = buf.ReadBool();
                buf.Align4();
            }
            CompileDateAndTime = buf.Read<Date>();
            WeatherTypeInList = buf.ReadInt32();
            CameraModeInCar = buf.ReadFloat();
            CameraModeOnFoot = buf.ReadFloat();
            if (fmt.IsMobile) IsQuickSave = (QuickSaveState) buf.ReadInt32();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            if (!fmt.IsPS2) buf.Write(LastMissionPassedName, MaxMissionPassedNameLength, unicode: true);
            if (fmt.IsPC || fmt.IsXbox) buf.Write(TimeStamp);
            buf.Write(SizeOfGameInBytes);
            buf.Write((int) CurrentLevel);
            buf.Write(CameraPosition);
            buf.Write(MillisecondsPerGameMinute);
            buf.Write(LastClockTick);
            buf.Write(GameClockHours);
            buf.Align4();
            buf.Write(GameClockMinutes);
            buf.Align4();
            buf.Write(CurrPadMode);
            buf.Align4();
            buf.Write(TimeInMilliseconds);
            buf.Write(TimeScale);
            buf.Write(TimeStep);
            buf.Write(TimeStepNonClipped);
            buf.Write(FrameCounter);
            buf.Write(TimeStep2);
            buf.Write(FramesPerUpdate);
            buf.Write(TimeScale2);
            buf.Write((short) OldWeatherType);
            buf.Align4();
            buf.Write((short) NewWeatherType);
            buf.Align4();
            buf.Write((short) ForcedWeatherType);
            buf.Align4();
            buf.Write(WeatherInterpolation);
            if (fmt.IsPS2)
            {
                buf.Write(MusicVolume);
                buf.Write(SfxVolume);
                if (!fmt.IsAustralian)
                {
                    buf.Write(CurrPadMode);
                    buf.Align4();
                }
                buf.Write(UseVibration);
                buf.Align4();
                buf.Write(StereoOutput);
                buf.Align4();
                buf.Write((byte) RadioStation);
                buf.Align4();
                buf.Write(Brightness);
                if (!fmt.IsAustralian)
                {
                    buf.Write(BlurOn);
                    buf.Align4();
                }
                buf.Write(ShowSubtitles);
                buf.Align4();
                buf.Write((int) Language);
                buf.Write(UseWideScreen);
                buf.Align4();
                buf.Write(CurrPadMode);
                buf.Align4();
                buf.Write(BlurOn);
                buf.Align4();
            }
            buf.Write(CompileDateAndTime);
            buf.Write(WeatherTypeInList);
            buf.Write(CameraModeInCar);
            buf.Write(CameraModeOnFoot);
            if (fmt.IsMobile) buf.Write((int) IsQuickSave);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsPS2 && fmt.IsAustralian) return 0xA8;
            if (fmt.IsPS2) return 0xB0;
            if (fmt.IsMobile) return 0xB0;
            if (fmt.IsPC || fmt.IsXbox) return 0xBC;
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
                && TimeStamp.Equals(other.TimeStamp)
                && SizeOfGameInBytes.Equals(other.SizeOfGameInBytes)
                && CurrentLevel.Equals(other.CurrentLevel)
                && CameraPosition.Equals(other.CameraPosition)
                && MillisecondsPerGameMinute.Equals(other.MillisecondsPerGameMinute)
                && LastClockTick.Equals(other.LastClockTick)
                && GameClockHours.Equals(other.GameClockHours)
                && GameClockMinutes.Equals(other.GameClockMinutes)
                && CurrPadMode.Equals(other.CurrPadMode)
                && TimeInMilliseconds.Equals(other.TimeInMilliseconds)
                && TimeScale.Equals(other.TimeScale)
                && TimeStep.Equals(other.TimeStep)
                && TimeStepNonClipped.Equals(other.TimeStepNonClipped)
                && FrameCounter.Equals(other.FrameCounter)
                && TimeStep2.Equals(other.TimeStep2)
                && FramesPerUpdate.Equals(other.FramesPerUpdate)
                && TimeScale2.Equals(other.TimeScale2)
                && OldWeatherType.Equals(other.OldWeatherType)
                && NewWeatherType.Equals(other.NewWeatherType)
                && ForcedWeatherType.Equals(other.ForcedWeatherType)
                && WeatherInterpolation.Equals(other.WeatherInterpolation)
                && MusicVolume.Equals(other.MusicVolume)
                && SfxVolume.Equals(other.SfxVolume)
                && UseVibration.Equals(other.UseVibration)
                && StereoOutput.Equals(other.StereoOutput)
                && RadioStation.Equals(other.RadioStation)
                && Brightness.Equals(other.Brightness)
                && ShowSubtitles.Equals(other.ShowSubtitles)
                && Language.Equals(other.Language)
                && UseWideScreen.Equals(other.UseWideScreen)
                && BlurOn.Equals(other.BlurOn)
                && CompileDateAndTime.Equals(other.CompileDateAndTime)
                && WeatherTypeInList.Equals(other.WeatherTypeInList)
                && CameraModeInCar.Equals(other.CameraModeInCar)
                && CameraModeOnFoot.Equals(other.CameraModeOnFoot)
                && IsQuickSave.Equals(other.IsQuickSave);
        }

        public SimpleVariables DeepClone()
        {
            return new SimpleVariables(this);
        }
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

    public enum Level
    {
        None,
        Industrial,
        Commercial,
        Suburban
    }

    public enum WeatherType
    {
        None = -1,
        Sunny,
        Cloudy,
        Rainy,
        Foggy
    }

    public enum QuickSaveState
    {
        None,
        Normal,
        OnMission
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
