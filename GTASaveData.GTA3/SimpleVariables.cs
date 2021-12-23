using System;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Types;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Miscelllaneous variables saved in a GTA3 save file.
    /// </summary>
    public class SimpleVariables : SaveDataObject,
        IEquatable<SimpleVariables>, IDeepClonable<SimpleVariables>
    {
        public const int MaxMissionPassedNameLength = 24;

        private string m_lastMissionPassedName;
        private SystemTime m_timeStamp;
        private int m_sizeOfGameInBytes;
        private Level m_currLevel;
        private Vector3 m_cameraPosition;
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
        private CameraMode m_cameraCarZoomIndicator;
        private CameraMode m_cameraPedZoomIndicator;
        private QuickSaveState m_isQuickSave;
        private bool m_cheatedFlag;

        /// <summary>
        /// The name of the last mission passed; the name of the save as shown in-game.
        /// Length: 24.
        /// </summary>
        /// <remarks>
        /// Not available on PS2. Name is a GXT key if prefixed by \uFFFF on Android and iOS only.
        /// </remarks>
        public string LastMissionPassedName
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The time this save was created or modified by the game.
        /// </summary>
        public SystemTime TimeStamp
        {
            get { return m_timeStamp; }
            set { m_timeStamp = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The total number of bytes in each block of the save file.
        /// </summary>
        /// <remarks>
        /// This value is pretty much meaningless; it is written to the save file and never read back.
        /// It's meant to indicate the total number of bytes in the save file excluding the 4-byte
        /// checksum and the 4-byte 'size' values affixed to the beginning of each data block, and
        /// is used for size-checking to ensure the save file is valid.
        /// </remarks>
        public int SizeOfGameInBytes
        {
            get { return m_sizeOfGameInBytes; }
            set { m_sizeOfGameInBytes = value; }
        }

        /// <summary>
        /// The current island (level) that the game is saved on.
        /// </summary>
        /// <remarks>
        /// Pretty much useless.
        /// </remarks>
        public Level CurrentLevel
        {
            get { return m_currLevel; }
            set { m_currLevel = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The position of the camera in the game world.
        /// </summary>
        /// <remarks>
        /// Useless in the vanilla game.
        /// </remarks>
        public Vector3 CameraPosition
        {
            get { return m_cameraPosition; }
            set { m_cameraPosition = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The number of milliseconds in one game minute. Use this to control
        /// how fast the clock ticks.
        /// </summary>
        public int MillisecondsPerGameMinute
        {
            get { return m_millisecondsPerGameMinute; }
            set { m_millisecondsPerGameMinute = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The last timer tick value. Should be equal or close to the value of
        /// <see cref="TimeInMilliseconds"/>.
        /// </summary>
        /// <remarks>
        /// The game will act weird if this value is larger than 2147483648 (MAX_INT).
        /// </remarks>
        public uint LastClockTick
        {
            get { return m_lastClockTick; }
            set { m_lastClockTick = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The hour value of the on-screen clock.
        /// </summary>
        public byte GameClockHours
        {
            get { return m_gameClockHours; }
            set { m_gameClockHours = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The minute value of the on-screen clock.
        /// </summary>
        public byte GameClockMinutes
        {
            get { return m_gameClockMinutes; }
            set { m_gameClockMinutes = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The current controller configuration.
        /// </summary>
        /// <remarks>
        /// I think this is only meaningful on PS2.
        /// </remarks>
        public short CurrPadMode
        {
            get { return m_currPadMode; }
            set { m_currPadMode = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The number of milliseconds passed since the start of the story mode.
        /// </summary>
        /// <remarks>
        /// The game will act weird if this value is larger than 2147483648 (MAX_INT).
        /// This value is sometimes referred to as the Global Timer.
        /// </remarks>
        public uint TimeInMilliseconds
        {
            get { return m_timeInMilliseconds; }
            set { m_timeInMilliseconds = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The game timer scale factor.
        /// </summary>
        /// <remarks>
        /// May not be used in the vanilla game.
        /// </remarks>
        public float TimeScale
        {
            get { return m_timeScale; }
            set { m_timeScale = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Mostly used for animation speed.
        /// </summary>
        /// <remarks>
        /// May not be used in the vanilla game.
        /// </remarks>
        public float TimeStep
        {
            get { return m_timeStep; }
            set { m_timeStep = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Related to <see cref="TimeStep"/>.
        /// </summary>
        /// <remarks>
        /// May not be used in the vanilla game.
        /// </remarks>
        public float TimeStepNonClipped
        {
            get { return m_timeStepNonClipped; }
            set { m_timeStepNonClipped = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The number of frames rendered since the story mode began.
        /// </summary>
        public uint FrameCounter
        {
            get { return m_frameCounter; }
            set { m_frameCounter = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Useless value that is not used by the game.
        /// </summary>
        public float TimeStep2
        {
            get { return m_timeStep2; }
            set { m_timeStep2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Useless value that is not used by the game.
        /// </summary>
        public float FramesPerUpdate
        {
            get { return m_framesPerUpdate; }
            set { m_framesPerUpdate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Useless value that is not used by the game.
        /// </summary>
        public float TimeScale2
        {
            get { return m_timeScale2; }
            set { m_timeScale2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The previous weather type in the weather cycle.
        /// </summary>
        public WeatherType OldWeatherType
        {
            get { return m_oldWeatherType; }
            set { m_oldWeatherType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The next weather type in the weather cycle.
        /// </summary>
        public WeatherType NewWeatherType
        {
            get { return m_newWeatherType; }
            set { m_newWeatherType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Overrides the weather cycle and forces a specific weather type.
        /// </summary>
        public WeatherType ForcedWeatherType
        {
            get { return m_forcedWeatherType; }
            set { m_forcedWeatherType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The current blend level between the previous and next weather type.
        /// </summary>
        public float WeatherInterpolation
        {
            get { return m_weatherInterpolationValue; }
            set { m_weatherInterpolationValue = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The radio volume.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public int MusicVolume
        {
            get { return m_prefsMusicVolume; }
            set { m_prefsMusicVolume = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The game environment volume.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public int SfxVolume
        {
            get { return m_prefsSfxVolume; }
            set { m_prefsSfxVolume = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Enable or disable controller vibration.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public bool UseVibration
        {
            get { return m_prefsUseVibration; }
            set { m_prefsUseVibration = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Select stereo (false) or mono (true) audio output.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public bool StereoMono
        {
            get { return m_prefsStereoMono; }
            set { m_prefsStereoMono = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Current radio station selection in the pause menu. Absolutely useless.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public RadioStation RadioStation
        {
            get { return m_prefsRadioStation; }
            set { m_prefsRadioStation = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The display brightness value.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public int Brightness
        {
            get { return m_prefsBrightness; }
            set { m_prefsBrightness = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Enable or disable mission dialogue subtitles.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public bool ShowSubtitles
        {
            get { return m_prefsShowSubtitles; }
            set { m_prefsShowSubtitles = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Select the game language. The corresponding GXT file must be present in the
        /// game directory or the game will crash.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public Language Language
        {
            get { return m_prefsLanguage; }
            set { m_prefsLanguage = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Enable or disable wide screen mode.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public bool UseWideScreen
        {
            get { return m_prefsUseWideScreen; }
            set { m_prefsUseWideScreen = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Enable or disable the 'trails' effect.
        /// </summary>
        /// <remarks>
        /// This setting is only present on PS2.
        /// </remarks>
        public bool BlurOn
        {
            get { return m_blurOn; }
            set { m_blurOn = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Game executable compilation timestamp. Useless value and only set on PS2 saves.
        /// </summary>
        public Date CompileDateAndTime
        {
            get { return m_compileDateAndTime; }
            set { m_compileDateAndTime = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The current index in the weather cycle.
        /// </summary>
        /// <remarks>
        /// The weather cycle is hardcoded as 64 weather types with the next
        /// weather type in the sequence beginning at the top of the hour.
        /// </remarks>
        public int WeatherTypeInList
        {
            get { return m_weatherTypeInList; }
            set { m_weatherTypeInList = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// In-car camera position.
        /// </summary>
        public CameraMode CameraModeInCar
        {
            get { return m_cameraCarZoomIndicator; }
            set { m_cameraCarZoomIndicator = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// On-foot camera position.
        /// </summary>
        /// <remarks>
        /// Only the following values are valid:
        /// <see cref="CameraMode.Near"/>,
        /// <see cref="CameraMode.Middle"/>,
        /// <see cref="CameraMode.Far"/>
        /// </remarks>
        public CameraMode CameraModeOnFoot
        {
            get { return m_cameraPedZoomIndicator; }
            set { m_cameraPedZoomIndicator = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indicates whether this is an autosaved game.
        /// </summary>
        /// <remarks>
        /// Only present on the mobile version.
        /// </remarks>
        public QuickSaveState IsQuickSave
        {
            get { return m_isQuickSave; }
            set { m_isQuickSave = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indicates whether cheats have been used on this game.
        /// </summary>
        /// <remarks>
        /// Only present on the Definitive Edition.
        /// </remarks>
        public bool CheatedFlag
        {
            get { return m_cheatedFlag; }
            set { m_cheatedFlag = value; OnPropertyChanged(); }
        }

        public SimpleVariables()
        {
            LastMissionPassedName = "";
            TimeStamp = SystemTime.MinValue;
            CompileDateAndTime = Date.MinValue;
            CameraPosition = new Vector3();
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
            StereoMono = other.StereoMono;
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
            CheatedFlag = other.CheatedFlag;
        }

        protected override void ReadData(DataBuffer buf, FileType fmt)
        {
            if (fmt.FlagDE)
            {
                uint unk00 = buf.ReadUInt32();       // probably padding
                Debug.Assert(unk00 == 0xFF00FF00U);
                int unk04 = buf.ReadInt32();       // maybe byte order mark, always 0xB or 0xB0000000
                Debug.Assert(unk04 == 0x0B);
                ulong unk08 = buf.ReadUInt64();      // serialized timestamp
                byte[] unk10 = buf.ReadBytes(20);    // maybe related to title
                int titleSize = buf.ReadInt32();
                Debug.Assert(titleSize < 8, "Title is longer than a GXT key!");
                LastMissionPassedName = buf.ReadString(titleSize);
            }
            else
            {
                if (!fmt.IsPS2) LastMissionPassedName = buf.ReadString(MaxMissionPassedNameLength, unicode: true);
                if (fmt.IsPC || fmt.IsXbox) TimeStamp = buf.ReadStruct<SystemTime>();
            }
            SizeOfGameInBytes = buf.ReadInt32();
            CurrentLevel = (Level) buf.ReadInt32();
            CameraPosition = buf.ReadStruct<Vector3>();
            MillisecondsPerGameMinute = buf.ReadInt32();
            LastClockTick = buf.ReadUInt32();
            if (fmt.FlagDE)
            {
                GameClockHours = buf.ReadByte();
                GameClockMinutes = buf.ReadByte();
                CurrPadMode = buf.ReadInt16();
            }
            else
            {
                GameClockHours = (byte) buf.ReadInt32();
                buf.Align4();
                GameClockMinutes = (byte) buf.ReadInt32();
                buf.Align4();
                CurrPadMode = buf.ReadInt16();
                buf.Align4();
            }
            TimeInMilliseconds = buf.ReadUInt32();
            TimeScale = buf.ReadFloat();
            TimeStep = buf.ReadFloat();
            TimeStepNonClipped = buf.ReadFloat();
            FrameCounter = buf.ReadUInt32();
            TimeStep2 = buf.ReadFloat();
            FramesPerUpdate = buf.ReadFloat();
            TimeScale2 = buf.ReadFloat();
            OldWeatherType = (WeatherType) buf.ReadInt16();
            if (!fmt.FlagDE) buf.Align4();
            NewWeatherType = (WeatherType) buf.ReadInt16();
            if (!fmt.FlagDE) buf.Align4();
            ForcedWeatherType = (WeatherType) buf.ReadInt16();
            if (!fmt.FlagDE) buf.Align4();
            WeatherInterpolation = buf.ReadFloat();
            if (fmt.IsPS2)
            {
                MusicVolume = buf.ReadInt32();
                SfxVolume = buf.ReadInt32();
                if (!fmt.FlagAustralia)
                {
                    CurrPadMode = buf.ReadInt16();  // duplicate of CurrPadMode
                    buf.Align4();
                }
                UseVibration = buf.ReadBool();
                buf.Align4();
                StereoMono = buf.ReadBool();
                buf.Align4();
                RadioStation = (RadioStation) buf.ReadByte();
                buf.Align4();
                Brightness = buf.ReadInt32();
                if (!fmt.FlagAustralia)
                {
                    BlurOn = buf.ReadBool();        // duplicate of BlurOn
                    buf.Align4();
                }
                ShowSubtitles = buf.ReadBool();
                buf.Align4();
                Language = (Language) buf.ReadInt32();
                UseWideScreen = buf.ReadBool();
                buf.Align4();
                CurrPadMode = buf.ReadInt16();      // duplicate of CurrPadMode
                buf.Align4();
                BlurOn = buf.ReadBool();
                buf.Align4();
            }
            CompileDateAndTime = buf.ReadStruct<Date>();
            WeatherTypeInList = buf.ReadInt32();
            if (fmt.FlagDE)
            {
                CameraModeInCar = (CameraMode) buf.ReadInt32();
                CameraModeOnFoot = (CameraMode) buf.ReadInt32();
            }
            else
            {
                CameraModeInCar = (CameraMode) (int) buf.ReadFloat();
                CameraModeOnFoot = (CameraMode) (int) buf.ReadFloat();
            }
            if (fmt.IsMobile || fmt.FlagDE)
            {
                IsQuickSave = (QuickSaveState) buf.ReadInt32();
            }
            if (fmt.FlagDE)
            {
                _ = buf.ReadBytes(3);   // unused
                CheatedFlag = buf.ReadBool();
            }

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileType fmt)
        {
            if (fmt.FlagDE)
            {
                throw new NotSupportedException("Definitive Edition not supported yet!!");
            }

            if (!fmt.IsPS2) buf.Write($"{LastMissionPassedName}\0", MaxMissionPassedNameLength, unicode: true, zeroTerminate: false);
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
                if (!fmt.FlagAustralia)
                {
                    buf.Write(CurrPadMode);
                    buf.Align4();
                }
                buf.Write(UseVibration);
                buf.Align4();
                buf.Write(StereoMono);
                buf.Align4();
                buf.Write((byte) RadioStation);
                buf.Align4();
                buf.Write(Brightness);
                if (!fmt.FlagAustralia)
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
            buf.Write(CompileDateAndTime.Second);
            buf.Write(CompileDateAndTime.Minute);
            buf.Write(CompileDateAndTime.Hour);
            buf.Write(CompileDateAndTime.Day);
            buf.Write(CompileDateAndTime.Month);
            buf.Write(CompileDateAndTime.Year);
            buf.Write(WeatherTypeInList);
            buf.Write((float) CameraModeInCar);     // stored as a float for some reason
            buf.Write((float) CameraModeOnFoot);    // stored as a float for some reason
            if (fmt.IsMobile) buf.Write((int) IsQuickSave);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileType fmt)
        {
            if (fmt.FlagDE)
            {
                return 0x9E + (LastMissionPassedName.Length + 1);
            }

            if (fmt.IsPS2 && fmt.FlagAustralia) return 0xA8;
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
                && StereoMono.Equals(other.StereoMono)
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
}
