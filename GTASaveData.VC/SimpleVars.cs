using GTASaveData.Common;
using GTASaveData.Serialization;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.VC
{
    public sealed class SimpleVars : SerializableObject,
        IEquatable<SimpleVars>
    {
        public static class Limits
        {
            public const int LastMissionPassedNameLength = 24;
            public const int RadioListenTimeCount = 10;
        }

        //// This is the number of bytes in a GTA:VC save excluding the 4-byte block size
        //// values that appear before each outer data block. It shows up in SimpleVars
        //// despite not being used at all by the game. Non-Japanese versions add 1 to
        //// this number for some reason.
        //private const uint TotalBlockDataSize = 0x31400;

        private string m_lastMissionPassedName;
        private SystemTime m_saveTime;
        private int m_sizeOfGameInBytes;
        private Level m_currLevel;
        private Vector3d m_cameraPosition;
        private int m_unknownSteamOnly;
        private uint m_millisecondsPerGameMinute;
        private uint m_lastClockTick;
        private int m_gameClockHours;
        private int m_gameClockMinutes;
        private uint m_timeInMilliseconds;
        private float m_timeScale;
        private float m_timeScale2;
        private float m_timeStep;
        private float m_timeStep2;
        private float m_timeStepNonClipped;
        private float m_framesPerUpdate;
        private uint m_frameCounter;
        private WeatherType m_oldWeatherType;
        private WeatherType m_newWeatherType;
        private WeatherType m_forcedWeatherType;
        private float m_interpolationValue;
        private int m_prefsPadMode;
        private int m_prefsMusicVolume;
        private int m_prefsSfxVolume;
        private bool m_prefsUseVibration;
        private bool m_prefsStereoMono;
        private RadioStation m_prefsRadioStation;
        private int m_prefsBrightness;
        private bool m_prefsUseWideScreen;
        private bool m_prefsShowTrails;
        private bool m_prefsShowSubtitles;
        private Language m_prefsLanguage;
        //private Timestamp m_compileDateAndTime;
        private int m_weatherTypeInList;
        private float m_inCarCameraMode;
        private float m_onFootCameraMode;
        //private int m_isQuickSave;              // TODO: enum?
        private int m_currentInterior;          // TODO: enum
        private bool m_taxiBoost;
        private bool m_invertLook;
        private int m_extraColor;
        private bool m_isExtraColorOn;
        private float m_extraColorInterpolation;
        private Array<uint> m_radioListenTime;

        public string LastMissionPassedName
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
        }

        public SystemTime SaveTime
        {
            get { return m_saveTime; }
            set { m_saveTime = value; OnPropertyChanged(); }
        }

        public int SizeOfGameInBytes
        {
            get { return m_sizeOfGameInBytes; }
            set { m_sizeOfGameInBytes = value; OnPropertyChanged(); }
        }

        public Level CurrLevel
        {
            get { return m_currLevel; }
            set { m_currLevel = value; OnPropertyChanged(); }
        }

        public Vector3d CameraPosition
        {
            get { return m_cameraPosition; }
            set { m_cameraPosition = value; OnPropertyChanged(); }
        }

        public int UnknownSteamOnly
        {
            get { return m_unknownSteamOnly; }
            set { m_unknownSteamOnly = value; OnPropertyChanged(); }
        }

        public uint MillisecondsPerGameMinute
        {
            get { return m_millisecondsPerGameMinute; }
            set { m_millisecondsPerGameMinute = value; OnPropertyChanged(); }
        }

        public uint LastClockTick
        {
            get { return m_lastClockTick; }
            set { m_lastClockTick = value; OnPropertyChanged(); }
        }

        public int GameClockHours
        {
            get { return m_gameClockHours; }
            set { m_gameClockHours = value; OnPropertyChanged(); }
        }

        public int GameClockMinutes
        {
            get { return m_gameClockMinutes; }
            set { m_gameClockMinutes = value; OnPropertyChanged(); }
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

        public float TimeScale2
        {
            get { return m_timeScale2; }
            set { m_timeScale2 = value; OnPropertyChanged(); }
        }

        public float TimeStep
        {
            get { return m_timeStep; }
            set { m_timeStep = value; OnPropertyChanged(); }
        }

        public float TimeStep2
        {
            get { return m_timeStep2; }
            set { m_timeStep2 = value; OnPropertyChanged(); }
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

        public float InterpolationValue
        {
            get { return m_interpolationValue; }
            set { m_interpolationValue = value; OnPropertyChanged(); }
        }

        public int PrefsControllerConfig
        {
            get { return m_prefsPadMode; }
            set { m_prefsPadMode = value; OnPropertyChanged(); }
        }

        public int PrefsMusicVolume
        {
            get { return m_prefsMusicVolume; }
            set { m_prefsMusicVolume = value; OnPropertyChanged(); }
        }

        public int PrefsSfxVolume
        {
            get { return m_prefsSfxVolume; }
            set { m_prefsSfxVolume = value; OnPropertyChanged(); }
        }

        public bool PrefsUseVibration
        {
            get { return m_prefsUseVibration; }
            set { m_prefsUseVibration = value; OnPropertyChanged(); }
        }

        public bool PrefsStereoMono
        {
            get { return m_prefsStereoMono; }
            set { m_prefsStereoMono = value; OnPropertyChanged(); }
        }

        public RadioStation PrefsRadioStation
        {
            get { return m_prefsRadioStation; }
            set { m_prefsRadioStation = value; OnPropertyChanged(); }
        }

        public int PrefsBrightness
        {
            get { return m_prefsBrightness; }
            set { m_prefsBrightness = value; OnPropertyChanged(); }
        }

        public bool PrefsUseWideScreen
        {
            get { return m_prefsUseWideScreen; }
            set { m_prefsUseWideScreen = value; OnPropertyChanged(); }
        }

        public bool PrefsShowTrails
        {
            get { return m_prefsShowTrails; }
            set { m_prefsShowTrails = value; OnPropertyChanged(); }
        }

        public bool PrefsShowSubtitles
        {
            get { return m_prefsShowSubtitles; }
            set { m_prefsShowSubtitles = value; OnPropertyChanged(); }
        }

        public Language PrefsLanguage
        {
            get { return m_prefsLanguage; }
            set { m_prefsLanguage = value; OnPropertyChanged(); }
        }

        //public Timestamp CompileDateAndTime
        //{
        //    get { return m_compileDateAndTime; }
        //    set { m_compileDateAndTime = value; OnPropertyChanged(); }
        //}

        public int WeatherTypeInList
        {
            get { return m_weatherTypeInList; }
            set { m_weatherTypeInList = value; OnPropertyChanged(); }
        }

        public float InCarCameraMode
        {
            get { return m_inCarCameraMode; }
            set { m_inCarCameraMode = value; OnPropertyChanged(); }
        }

        public float OnFootCameraMode
        {
            get { return m_onFootCameraMode; }
            set { m_onFootCameraMode = value; OnPropertyChanged(); }
        }

        //public int IsQuickSave
        //{
        //    get { return m_isQuickSave; }
        //    set { m_isQuickSave = value; OnPropertyChanged(); }
        //}

        public int CurrentInterior
        {
            get { return m_currentInterior; }
            set { m_currentInterior = value; OnPropertyChanged(); }
        }

        public bool TaxiBoost
        {
            get { return m_taxiBoost; }
            set { m_taxiBoost = value; OnPropertyChanged(); }
        }

        public bool InvertLook
        {
            get { return m_invertLook; }
            set { m_invertLook = value; OnPropertyChanged(); }
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

        public Array<uint> RadioListenTime
        {
            get { return m_radioListenTime; }
            set { m_radioListenTime = value; OnPropertyChanged(); }
        }

        public SimpleVars()
        {
            m_lastMissionPassedName = string.Empty;
            m_saveTime = new SystemTime();
            m_cameraPosition = new Vector3d();
            //m_compileDateAndTime = new Timestamp();
            m_radioListenTime = new Array<uint>();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_lastMissionPassedName = r.ReadString(Limits.LastMissionPassedNameLength, unicode: true);
            if (!fmt.SupportsPS2)  // TODO: confirm
            {
                if (fmt.SupportsPC || fmt.SupportsXbox)   // TODO: confirm
                {
                    m_saveTime = r.ReadObject<SystemTime>();
                }
            }
            m_sizeOfGameInBytes = r.ReadInt32();
            Debug.Assert(m_sizeOfGameInBytes == (fmt.IsSupported(ConsoleType.PS2, ConsoleFlags.Japan) ? 0x31400 : 0x31401));    // maybe
            m_currLevel = (Level) r.ReadUInt32();
            m_cameraPosition = r.ReadObject<Vector3d>();
            if (fmt.IsSupported(ConsoleType.Win32, ConsoleFlags.Steam))
            {
                m_unknownSteamOnly = r.ReadInt32();
            }
            m_millisecondsPerGameMinute = r.ReadUInt32();
            m_lastClockTick = r.ReadUInt32();
            if (fmt.SupportsPS2)   // TODO: confirm
            {
                m_gameClockHours = r.ReadInt32();
                m_gameClockMinutes = r.ReadInt32();
                m_prefsPadMode = r.ReadUInt16();
                r.Align();
            }
            else
            {
                m_gameClockHours = r.ReadByte();
                r.Align();
                m_gameClockMinutes = r.ReadByte();
                r.Align();
                m_prefsPadMode = r.ReadInt16();
                r.Align();
            }
            m_timeInMilliseconds = r.ReadUInt32();
            m_timeScale = r.ReadSingle();
            m_timeStep = r.ReadSingle();
            m_timeStepNonClipped = r.ReadSingle();
            m_frameCounter = r.ReadUInt32();
            m_timeStep2 = r.ReadSingle();
            m_framesPerUpdate = r.ReadSingle();
            m_timeScale2 = r.ReadSingle();
            m_oldWeatherType = (WeatherType) r.ReadInt16();
            r.Align();
            m_newWeatherType = (WeatherType) r.ReadInt16();
            r.Align();
            m_forcedWeatherType = (WeatherType) r.ReadInt16();
            r.Align();
            m_interpolationValue = r.ReadSingle();
            if (fmt.SupportsPS2)   // TODO: confirm
            {
                m_prefsMusicVolume = r.ReadInt32();
                m_prefsSfxVolume = r.ReadInt32();
                if (!fmt.IsSupported(ConsoleType.PS2, ConsoleFlags.Australia))
                {
                    m_prefsPadMode = r.ReadInt32();
                }
                m_prefsUseVibration = r.ReadBool(4);
                m_prefsStereoMono = r.ReadBool(4);
                m_prefsRadioStation = (RadioStation) r.ReadInt32();
                m_prefsBrightness = r.ReadInt32();
                if (!fmt.IsSupported(ConsoleType.PS2, ConsoleFlags.Australia))
                {
                    m_prefsShowTrails = r.ReadBool(4);
                }
                m_prefsShowSubtitles = r.ReadBool(4);
                m_prefsLanguage = (Language) r.ReadInt32();
                m_prefsUseWideScreen = r.ReadBool(4);
                m_prefsPadMode = r.ReadInt32();
                m_prefsShowTrails = r.ReadBool(4);
            }
            //m_compileDateAndTime = serializer.ReadObject<Timestamp>();
            m_weatherTypeInList = r.ReadInt32();
            m_inCarCameraMode = r.ReadSingle();
            m_onFootCameraMode = r.ReadSingle();
            //if (format.SupportsMobile)
            //{
            //    m_isQuickSave = serializer.ReadInt32();
            //}
            m_currentInterior = r.ReadInt32();
            m_taxiBoost = r.ReadBool();
            r.Align();
            m_invertLook = r.ReadBool();
            r.Align();
            m_extraColor = r.ReadInt32();
            m_isExtraColorOn = r.ReadBool(4);
            m_extraColorInterpolation = r.ReadSingle();
            m_radioListenTime = r.ReadArray<uint>(Limits.RadioListenTimeCount);
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_lastMissionPassedName, Limits.LastMissionPassedNameLength, unicode: true);
            if (!fmt.SupportsPS2)
            {
                if (fmt.SupportsPC || fmt.SupportsXbox)
                {
                    w.Write(m_saveTime);
                }
            }
            w.Write(m_sizeOfGameInBytes);
            w.Write((int) m_currLevel);
            w.Write(m_cameraPosition);
            if (fmt.IsSupported(ConsoleType.Win32, ConsoleFlags.Steam)) // TODO: distinguish between Windows Steam and macOS Steam
            {
                w.Write(m_unknownSteamOnly);   // 0x3DF5C2FD
            }
            w.Write(m_millisecondsPerGameMinute);
            w.Write(m_lastClockTick);
            if (fmt.SupportsPS2)
            {
                w.Write(m_gameClockHours);
                w.Write(m_gameClockMinutes);
                w.Write((ushort) m_prefsPadMode);
                w.Align();
            }
            else
            {
                w.Write((byte) m_gameClockHours);
                w.Align();
                w.Write((byte) m_gameClockMinutes);
                w.Align();
                w.Write((ushort) m_prefsPadMode);
                w.Align();
            }
            w.Write(m_timeInMilliseconds);
            w.Write(m_timeScale);
            w.Write(m_timeStep);
            w.Write(m_timeStepNonClipped);
            w.Write(m_frameCounter);
            w.Write(m_timeStep2);
            w.Write(m_framesPerUpdate);
            w.Write(m_timeScale2);
            w.Write((short) m_oldWeatherType);
            w.Align();
            w.Write((short) m_newWeatherType);
            w.Align();
            w.Write((short) m_forcedWeatherType);
            w.Align();
            w.Write(m_interpolationValue);
            if (fmt.SupportsPS2)
            {
                w.Write(m_prefsMusicVolume);
                w.Write(m_prefsSfxVolume);
                if (!fmt.IsSupported(ConsoleType.PS2, ConsoleFlags.Australia))
                {
                    w.Write(m_prefsPadMode);
                }
                w.Write(m_prefsUseVibration, 4);
                w.Write(m_prefsStereoMono, 4);
                w.Write((int) m_prefsRadioStation);
                w.Write(m_prefsBrightness);
                if (!fmt.IsSupported(ConsoleType.PS2, ConsoleFlags.Australia))
                {
                    w.Write(m_prefsShowTrails, 4);
                }
                w.Write(m_prefsShowSubtitles, 4);
                w.Write((int) m_prefsLanguage);
                w.Write(m_prefsUseWideScreen, 4);
                w.Write(m_prefsPadMode);
                w.Write(m_prefsShowTrails, 4);
            }
            //serializer.WriteObject(m_compileDateAndTime);
            w.Write(m_weatherTypeInList);
            w.Write(m_inCarCameraMode);
            w.Write(m_onFootCameraMode);
            w.Write(m_currentInterior);
            w.Write(m_taxiBoost);
            w.Align();
            w.Write(m_invertLook);
            w.Align();
            w.Write(m_extraColor);
            w.Write(m_isExtraColorOn, 4);
            w.Write(m_extraColorInterpolation);
            w.Write(m_radioListenTime.ToArray(), Limits.RadioListenTimeCount);
            //if (format.SupportsMobile)
            //{
            //    serializer.Write(m_isQuickSave);
            //}
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SimpleVars);
        }

        public bool Equals(SimpleVars other)
        {
            if (other == null)
            {
                return false;
            }

            return m_lastMissionPassedName.Equals(other.m_lastMissionPassedName)
                && m_saveTime.Equals(other.m_saveTime)
                && m_sizeOfGameInBytes.Equals(other.m_sizeOfGameInBytes)
                && m_currLevel.Equals(other.m_currLevel)
                && m_cameraPosition.Equals(other.m_cameraPosition)
                && m_unknownSteamOnly.Equals(other.m_unknownSteamOnly)
                && m_millisecondsPerGameMinute.Equals(other.m_millisecondsPerGameMinute)
                && m_lastClockTick.Equals(other.m_lastClockTick)
                && m_gameClockHours.Equals(other.m_gameClockHours)
                && m_gameClockMinutes.Equals(other.m_gameClockMinutes)
                && m_timeInMilliseconds.Equals(other.m_timeInMilliseconds)
                && m_timeScale.Equals(other.m_timeScale)
                && m_timeScale2.Equals(other.m_timeScale2)
                && m_timeStep.Equals(other.m_timeStep)
                && m_timeStep2.Equals(other.m_timeStep2)
                && m_timeStepNonClipped.Equals(other.m_timeStepNonClipped)
                && m_framesPerUpdate.Equals(other.m_framesPerUpdate)
                && m_frameCounter.Equals(other.m_frameCounter)
                && m_oldWeatherType.Equals(other.m_oldWeatherType)
                && m_newWeatherType.Equals(other.m_newWeatherType)
                && m_forcedWeatherType.Equals(other.m_forcedWeatherType)
                && m_interpolationValue.Equals(other.m_interpolationValue)
                && m_prefsPadMode.Equals(other.m_prefsPadMode)
                && m_prefsMusicVolume.Equals(other.m_prefsMusicVolume)
                && m_prefsSfxVolume.Equals(other.m_prefsSfxVolume)
                && m_prefsUseVibration.Equals(other.m_prefsUseVibration)
                && m_prefsStereoMono.Equals(other.m_prefsStereoMono)
                && m_prefsRadioStation.Equals(other.m_prefsRadioStation)
                && m_prefsBrightness.Equals(other.m_prefsBrightness)
                && m_prefsUseWideScreen.Equals(other.m_prefsUseWideScreen)
                && m_prefsShowTrails.Equals(other.m_prefsShowTrails)
                && m_prefsShowSubtitles.Equals(other.m_prefsShowSubtitles)
                && m_prefsLanguage.Equals(other.m_prefsLanguage)
                //&& m_compileDateAndTime.Equals(other.m_compileDateAndTime)
                && m_weatherTypeInList.Equals(other.m_weatherTypeInList)
                && m_inCarCameraMode.Equals(other.m_inCarCameraMode)
                && m_onFootCameraMode.Equals(other.m_onFootCameraMode)
                //&& m_isQuickSave.Equals(other.m_isQuickSave);
                && m_currentInterior.Equals(other.m_currentInterior)
                && m_taxiBoost.Equals(other.m_taxiBoost)
                && m_invertLook.Equals(other.m_invertLook)
                && m_extraColor.Equals(other.m_extraColor)
                && m_isExtraColorOn.Equals(other.m_isExtraColorOn)
                && m_extraColorInterpolation.Equals(other.m_extraColorInterpolation)
                && m_radioListenTime.SequenceEqual(other.m_radioListenTime);

        }
    }
}
