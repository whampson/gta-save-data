using GTASaveData.Common;
using GTASaveData.Serialization;
using GTASaveData.VC;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.VC
{
    public sealed class SimpleVars : SaveDataObject,
        IEquatable<SimpleVars>
    {
        public static class Limits
        {
            public const int LastMissionPassedNameLength = 24;
            public const int RadioListenTimeCount = 10;
        }

        // This is the number of bytes in a GTA:VC save excluding the 4-byte block size
        // values that appear before each outer data block. It shows up in SimpleVars
        // despite not being used at all by the game. Non-Japanese versions add 1 to
        // this number for some reason.
        private const uint TotalBlockDataSize = 0x31400;

        private string m_lastMissionPassedName;
        private SystemTime m_saveTime;
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
        private int m_prefsControllerConfig;    // TODO: enum?
        private int m_prefsMusicVolume;
        private int m_prefsSfxVolume;
        private bool m_prefsUseVibration;
        private bool m_prefsStereoMono;
        private RadioStation m_prefsRadioStation;
        private int m_prefsBrightness;
        private bool m_prefsUseWideScreen;
        private bool m_prefsShowTrails;
        private bool m_prefsShowSubtitles;
        private int m_prefsLanguage;            // TODO: enum
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
        private StaticArray<uint> m_radioListenTime;

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
            get { return m_prefsControllerConfig; }
            set { m_prefsControllerConfig = value; OnPropertyChanged(); }
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

        public int PrefsLanguage
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

        public StaticArray<uint> RadioListenTime
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
            m_radioListenTime = new StaticArray<uint>(Limits.RadioListenTimeCount);
        }

        private SimpleVars(SaveDataSerializer serializer, FileFormat format)
            : this()
        {
            m_lastMissionPassedName = serializer.ReadString(Limits.LastMissionPassedNameLength, unicode: true);
            if (!format.IsPS2)  // TODO: confirm
            {
                if (format.IsPC || format.IsXbox)   // TODO: confirm
                {
                    m_saveTime = serializer.ReadObject<SystemTime>();
                }
            }
            int constant = serializer.ReadInt32();
            Debug.Assert(constant == TotalBlockDataSize || constant == (TotalBlockDataSize + 1));
            m_currLevel = (Level) serializer.ReadUInt32();
            m_cameraPosition = serializer.ReadObject<Vector3d>();
            if (format.HasFlag(ConsoleFlags.Steam)) // TODO: distinguish between Windows Steam and macOS Steam
            {
                m_unknownSteamOnly = serializer.ReadInt32();
            }
            m_millisecondsPerGameMinute = serializer.ReadUInt32();
            m_lastClockTick = serializer.ReadUInt32();
            if (format.IsPS2)   // TODO: confirm
            {
                m_gameClockHours = serializer.ReadInt32();
                m_gameClockMinutes = serializer.ReadInt32();
                m_prefsControllerConfig = serializer.ReadInt32();
            }
            else
            {
                m_gameClockHours = serializer.ReadByte();
                serializer.Align();
                m_gameClockMinutes = serializer.ReadByte();
                serializer.Align();
                m_prefsControllerConfig = serializer.ReadInt16();
                serializer.Align();
            }
            m_timeInMilliseconds = serializer.ReadUInt32();
            m_timeScale = serializer.ReadSingle();
            m_timeStep = serializer.ReadSingle();
            m_timeStepNonClipped = serializer.ReadSingle();
            m_frameCounter = serializer.ReadUInt32();
            m_timeStep2 = serializer.ReadSingle();
            m_framesPerUpdate = serializer.ReadSingle();
            m_timeScale2 = serializer.ReadSingle();
            m_oldWeatherType = (WeatherType) serializer.ReadInt16();
            serializer.Align();
            m_newWeatherType = (WeatherType) serializer.ReadInt16();
            serializer.Align();
            m_forcedWeatherType = (WeatherType) serializer.ReadInt16();
            serializer.Align();
            m_interpolationValue = serializer.ReadSingle();
            if (format.IsPS2)   // TODO: confirm
            {
                m_prefsMusicVolume = serializer.ReadInt32();
                m_prefsSfxVolume = serializer.ReadInt32();
                if (!format.HasFlag(ConsoleFlags.Australia))
                {
                    m_prefsControllerConfig = serializer.ReadInt32();
                }
                m_prefsUseVibration = serializer.ReadBool(4);
                m_prefsStereoMono = serializer.ReadBool(4);
                m_prefsRadioStation = (RadioStation) serializer.ReadInt32();
                m_prefsBrightness = serializer.ReadInt32();
                if (!format.HasFlag(ConsoleFlags.Australia))
                {
                    m_prefsShowTrails = serializer.ReadBool(4);
                }
                m_prefsShowSubtitles = serializer.ReadBool(4);
                m_prefsLanguage = serializer.ReadInt32();
                m_prefsUseWideScreen = serializer.ReadBool(4);
                m_prefsControllerConfig = serializer.ReadInt32();
                m_prefsShowTrails = serializer.ReadBool(4);
            }
            //m_compileDateAndTime = serializer.ReadObject<Timestamp>();
            m_weatherTypeInList = serializer.ReadInt32();
            m_inCarCameraMode = serializer.ReadSingle();
            m_onFootCameraMode = serializer.ReadSingle();
            //if (format.IsMobile)
            //{
            //    m_isQuickSave = serializer.ReadInt32();
            //}
            m_currentInterior = serializer.ReadInt32();
            m_taxiBoost = serializer.ReadBool();
            serializer.Align();
            m_invertLook = serializer.ReadBool();
            serializer.Align();
            m_extraColor = serializer.ReadInt32();
            m_isExtraColorOn = serializer.ReadBool(4);
            m_extraColorInterpolation = serializer.ReadSingle();
            m_radioListenTime = new StaticArray<uint>(serializer.ReadArray<uint>(10));
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.Write(m_lastMissionPassedName, Limits.LastMissionPassedNameLength, unicode: true);
            if (!format.IsPS2)
            {
                if (format.IsPC || format.IsXbox)
                {
                    serializer.WriteObject(m_saveTime);
                }
            }
            serializer.Write(format.HasFlag(ConsoleFlags.Japan) ? TotalBlockDataSize : TotalBlockDataSize + 1);
            serializer.Write((int) m_currLevel);
            serializer.WriteObject(m_cameraPosition);
            if (format.HasFlag(ConsoleFlags.Steam)) // TODO: distinguish between Windows Steam and macOS Steam
            {
                serializer.Write(m_unknownSteamOnly);   // 0x3DF5C2FD
            }
            serializer.Write(m_millisecondsPerGameMinute);
            serializer.Write(m_lastClockTick);
            if (format.IsPS2)
            {
                serializer.Write(m_gameClockHours);
                serializer.Write(m_gameClockMinutes);
                serializer.Write((int) m_prefsControllerConfig);
            }
            else
            {
                serializer.Write((byte) m_gameClockHours);
                serializer.Align();
                serializer.Write((byte) m_gameClockMinutes);
                serializer.Align();
                serializer.Write((short) m_prefsControllerConfig);
                serializer.Align();
            }
            serializer.Write(m_timeInMilliseconds);
            serializer.Write(m_timeScale);
            serializer.Write(m_timeStep);
            serializer.Write(m_timeStepNonClipped);
            serializer.Write(m_frameCounter);
            serializer.Write(m_timeStep2);
            serializer.Write(m_framesPerUpdate);
            serializer.Write(m_timeScale2);
            serializer.Write((short) m_oldWeatherType);
            serializer.Align();
            serializer.Write((short) m_newWeatherType);
            serializer.Align();
            serializer.Write((short) m_forcedWeatherType);
            serializer.Align();
            serializer.Write(m_interpolationValue);
            if (format.IsPS2)
            {
                serializer.Write(m_prefsMusicVolume);
                serializer.Write(m_prefsSfxVolume);
                if (!format.HasFlag(ConsoleFlags.Australia))
                {
                    serializer.Write((int) m_prefsControllerConfig);
                }
                serializer.Write(m_prefsUseVibration, 4);
                serializer.Write(m_prefsStereoMono, 4);
                serializer.Write((int) m_prefsRadioStation);
                serializer.Write(m_prefsBrightness);
                if (!format.HasFlag(ConsoleFlags.Australia))
                {
                    serializer.Write(m_prefsShowTrails, 4);
                }
                serializer.Write(m_prefsShowSubtitles, 4);
                serializer.Write((int) m_prefsLanguage);
                serializer.Write(m_prefsUseWideScreen, 4);
                serializer.Write((int) m_prefsControllerConfig);
                serializer.Write(m_prefsShowTrails, 4);
            }
            //serializer.WriteObject(m_compileDateAndTime);
            serializer.Write(m_weatherTypeInList);
            serializer.Write(m_inCarCameraMode);
            serializer.Write(m_onFootCameraMode);
            serializer.Write(m_currentInterior);
            serializer.Write(m_taxiBoost);
            serializer.Align();
            serializer.Write(m_invertLook);
            serializer.Align();
            serializer.Write(m_extraColor);
            serializer.Write(m_isExtraColorOn, 4);
            serializer.Write(m_extraColorInterpolation);
            serializer.WriteArray(m_radioListenTime.ToArray());
            //if (format.IsMobile)
            //{
            //    serializer.Write(m_isQuickSave);
            //}
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(SimpleVars other)
        {
            if (other == null)
            {
                return false;
            }

            return m_lastMissionPassedName.Equals(other.m_lastMissionPassedName)
                && m_saveTime.Equals(other.m_saveTime)
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
                && m_prefsControllerConfig.Equals(other.m_prefsControllerConfig)
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
