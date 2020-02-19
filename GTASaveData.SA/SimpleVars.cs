using GTASaveData.Common;
using GTASaveData.Serialization;
using System;
using System.Diagnostics;

namespace GTASaveData.SA
{
    public class SimpleVars : SerializableObject,
        ISimpleVars,
        IEquatable<SimpleVars>
    {
        public static class Limits
        {
            public const int SaveNameLength = 100;
            public const int UnknownDataLength = 45;
        }

        private uint m_versionId;
        private string m_saveName;
        private int m_currentMissionPack;
        private int m_currentLevel;
        private Vector3d m_cameraPosition;
        private int m_millisecondsPerGameMinute;
        private uint m_lastClockTick;
        private int m_gameMonth;
        private int m_gameMonthDay;
        private int m_gameHour;
        private int m_gameMinute;
        private int m_weekday;
        private bool m_timeCopyFlag;
        private int m_currentPadMode;
        private bool m_cheatedFlag;
        private uint m_timeInMilliseconds;
        private float m_timeScale;
        private float m_timeStep;
        private float m_tickTime;
        private int m_frameCounter;
        private int m_previousWeather;
        private int m_currentWeather;
        private int m_forcedWeather;
        private float m_weatherInterpolation;
        private int m_weatherTypeInList;
        private float m_amountOfRainFallen;
        private int m_inCarCameraMode;
        private int m_onFootCameraMode;
        private int m_currentInterior;
        private bool m_invertLook;
        private int m_extraColorId;
        private bool m_extraOn;
        private float m_extraInterpolation;
        private int m_extraWeather;
        private int m_currentWaterConfiguration;
        private bool m_riotMode;
        private bool m_unknownRiotRelated;
        private int m_maxWantedLevel;
        private int m_maxChaos;
        private bool m_isFrench;
        private bool m_isGerman;
        private bool m_isUncensored;
        private Array<byte> m_unknown;
        private int m_cinematicCameraHelpRemaining;
        private SystemTime m_saveTime;
        private uint m_targetMarkerHandle;
        private bool m_carTheftHelpShown;
        private bool m_allTaxisHaveNitro;
        private bool m_prostitutesPayYou;

        public uint VersionId
        { 
            get { return m_versionId; }
            set { m_versionId = value; OnPropertyChanged(); }
        }

        public string SaveName
        { 
            get { return m_saveName; }
            set { m_saveName = value; OnPropertyChanged(); }
        }

        public int CurrentMissionPack
        { 
            get { return m_currentMissionPack; }
            set { m_currentMissionPack = value; OnPropertyChanged(); }
        }

        public int CurrentLevel
        { 
            get { return m_currentLevel; }
            set { m_currentLevel = value; OnPropertyChanged(); }
        }

        public Vector3d CameraPosition
        { 
            get { return m_cameraPosition; }
            set { m_cameraPosition = value; OnPropertyChanged(); }
        }

        public int MillisecondsPerGameMinute
        { 
            get { return m_millisecondsPerGameMinute; }
            set { m_millisecondsPerGameMinute = value; OnPropertyChanged(); }
        }

        public uint WeatherTimer
        { 
            get { return m_lastClockTick; }
            set { m_lastClockTick = value; OnPropertyChanged(); }
        }

        public int GameMonth
        { 
            get { return m_gameMonth; }
            set { m_gameMonth = value; OnPropertyChanged(); }
        }

        public int GameMonthDay
        { 
            get { return m_gameMonthDay; }
            set { m_gameMonthDay = value; OnPropertyChanged(); }
        }

        public int GameHour
        { 
            get { return m_gameHour; }
            set { m_gameHour = value; OnPropertyChanged(); }
        }

        public int GameMinute
        { 
            get { return m_gameMinute; }
            set { m_gameMinute = value; OnPropertyChanged(); }
        }

        public int Weekday
        { 
            get { return m_weekday; }
            set { m_weekday = value; OnPropertyChanged(); }
        }

        public bool TimeCopyFlag
        { 
            get { return m_timeCopyFlag; }
            set { m_timeCopyFlag = value; OnPropertyChanged(); }
        }

        public int CurrentPadMode
        { 
            get { return m_currentPadMode; }
            set { m_currentPadMode = value; OnPropertyChanged(); }
        }

        public bool CheatedFlag
        {
            get { return m_cheatedFlag; }
            set { m_cheatedFlag = value; OnPropertyChanged(); }
        }

        public uint GlobalTimer
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

        public float TickTime
        { 
            get { return m_tickTime; }
            set { m_tickTime = value; OnPropertyChanged(); }
        }

        public int FrameCounter
        { 
            get { return m_frameCounter; }
            set { m_frameCounter = value; OnPropertyChanged(); }
        }

        public int PreviousWeather
        { 
            get { return m_previousWeather; }
            set { m_previousWeather = value; OnPropertyChanged(); }
        }

        public int CurrentWeather
        { 
            get { return m_currentWeather; }
            set { m_currentWeather = value; OnPropertyChanged(); }
        }

        public int ForcedWeather
        { 
            get { return m_forcedWeather; }
            set { m_forcedWeather = value; OnPropertyChanged(); }
        }

        public float WeatherInterpolation
        { 
            get { return m_weatherInterpolation; }
            set { m_weatherInterpolation = value; OnPropertyChanged(); }
        }

        public int WeatherTypeInList
        { 
            get { return m_weatherTypeInList; }
            set { m_weatherTypeInList = value; OnPropertyChanged(); }
        }

        public float AmountOfRainFallen
        { 
            get { return m_amountOfRainFallen; }
            set { m_amountOfRainFallen = value; OnPropertyChanged(); }
        }

        public int InCarCameraMode
        { 
            get { return m_inCarCameraMode; }
            set { m_inCarCameraMode = value; OnPropertyChanged(); }
        }

        public int OnFootCameraMode
        { 
            get { return m_onFootCameraMode; }
            set { m_onFootCameraMode = value; OnPropertyChanged(); }
        }

        public int CurrentInterior
        { 
            get { return m_currentInterior; }
            set { m_currentInterior = value; OnPropertyChanged(); }
        }

        public bool InvertLook
        { 
            get { return m_invertLook; }
            set { m_invertLook = value; OnPropertyChanged(); }
        }

        public int ExtraColorId
        { 
            get { return m_extraColorId; }
            set { m_extraColorId = value; OnPropertyChanged(); }
        }

        public bool ExtraOn
        { 
            get { return m_extraOn; }
            set { m_extraOn = value; OnPropertyChanged(); }
        }

        public float ExtraInterpolation
        { 
            get { return m_extraInterpolation; }
            set { m_extraInterpolation = value; OnPropertyChanged(); }
        }

        public int ExtraWeather
        { 
            get { return m_extraWeather; }
            set { m_extraWeather = value; OnPropertyChanged(); }
        }

        public int CurrentWaterConfiguration
        { 
            get { return m_currentWaterConfiguration; }
            set { m_currentWaterConfiguration = value; OnPropertyChanged(); }
        }

        public bool RiotMode
        { 
            get { return m_riotMode; }
            set { m_riotMode = value; OnPropertyChanged(); }
        }

        public bool UnknownRiotRelated
        { 
            get { return m_unknownRiotRelated; }
            set { m_unknownRiotRelated = value; OnPropertyChanged(); }
        }

        public Array<byte> Unknown
        {
            get { return m_unknown; }
            set { m_unknown = value; OnPropertyChanged(); }
        }

        public int MaxWantedLevel
        { 
            get { return m_maxWantedLevel; }
            set { m_maxWantedLevel = value; OnPropertyChanged(); }
        }

        public int MaxChaos
        { 
            get { return m_maxChaos; }
            set { m_maxChaos = value; OnPropertyChanged(); }
        }

        public bool IsFrench
        { 
            get { return m_isFrench; }
            set { m_isFrench = value; OnPropertyChanged(); }
        }

        public bool IsGerman
        { 
            get { return m_isGerman; }
            set { m_isGerman = value; OnPropertyChanged(); }
        }

        public bool IsUncensored
        { 
            get { return m_isUncensored; }
            set { m_isUncensored = value; OnPropertyChanged(); }
        }

        public int CinematicCameraHelpRemaining
        { 
            get { return m_cinematicCameraHelpRemaining; }
            set { m_cinematicCameraHelpRemaining = value; OnPropertyChanged(); }
        }

        public SystemTime SaveTime
        { 
            get { return m_saveTime; }
            set { m_saveTime = value; OnPropertyChanged(); }
        }

        public uint TargetMarkerHandle
        { 
            get { return m_targetMarkerHandle; }
            set { m_targetMarkerHandle = value; OnPropertyChanged(); }
        }

        public bool CarTheftHelpShown
        { 
            get { return m_carTheftHelpShown; }
            set { m_carTheftHelpShown = value; OnPropertyChanged(); }
        }

        public bool AllTaxisHaveNitro
        { 
            get { return m_allTaxisHaveNitro; }
            set { m_allTaxisHaveNitro = value; OnPropertyChanged(); }
        }

        public bool ProstitutesPayYou
        { 
            get { return m_prostitutesPayYou; }
            set { m_prostitutesPayYou = value; OnPropertyChanged(); }
        }

        public SimpleVars()
        {
            m_cameraPosition = new Vector3d();
            m_saveTime = new SystemTime();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            long start = r.BaseStream.Position;

            m_versionId = r.ReadUInt32();
            m_saveName = r.ReadString(Limits.SaveNameLength);
            m_currentMissionPack = r.ReadByte();
            r.Align();
            m_currentLevel = r.ReadInt32();
            m_cameraPosition = r.ReadObject<Vector3d>();
            m_millisecondsPerGameMinute = r.ReadInt32();
            m_lastClockTick = r.ReadUInt32();
            m_gameMonth = r.ReadByte();
            m_gameMonthDay = r.ReadByte();
            m_gameHour = r.ReadByte();
            m_gameMinute = r.ReadByte();
            m_weekday = r.ReadByte();
            m_gameMonth = r.ReadByte();     // deliberately copied
            m_gameMonthDay = r.ReadByte();  // deliberately copied
            m_gameHour = r.ReadByte();      // deliberately copied
            m_gameMinute = r.ReadByte();    // deliberately copied
            m_timeCopyFlag = r.ReadBool();
            m_currentPadMode = r.ReadUInt16();
            m_cheatedFlag = r.ReadBool();
            r.Align();
            m_timeInMilliseconds = r.ReadUInt32();
            m_timeScale = r.ReadSingle();
            m_timeStep = r.ReadSingle();
            m_tickTime = r.ReadSingle();
            m_frameCounter = r.ReadInt32();
            m_previousWeather = r.ReadInt16();
            m_currentWeather = r.ReadInt16();
            m_forcedWeather = r.ReadInt16();
            m_weatherInterpolation = r.ReadSingle();     // maybe dword?
            r.Align();
            m_weatherTypeInList = r.ReadInt32();
            m_amountOfRainFallen = r.ReadSingle();
            m_inCarCameraMode = r.ReadInt32();
            m_onFootCameraMode = r.ReadInt32();
            m_currentInterior = r.ReadInt32();
            m_invertLook = r.ReadBool();
            r.Align();
            m_extraColorId = r.ReadInt32();
            m_extraOn = r.ReadBool();
            r.Align();
            m_extraInterpolation = r.ReadSingle();
            m_extraWeather = r.ReadInt32();
            m_currentWaterConfiguration = r.ReadInt32();
            m_riotMode = r.ReadBool();
            m_unknownRiotRelated = r.ReadBool();
            r.Align();
            m_maxWantedLevel = r.ReadInt32();
            m_maxChaos = r.ReadInt32();
            m_isFrench = r.ReadBool();
            m_isGerman = r.ReadBool();
            m_isUncensored = r.ReadBool();
            r.Align();
            m_unknown = r.ReadBytes(Limits.UnknownDataLength);
            m_cinematicCameraHelpRemaining = r.ReadByte();
            m_saveTime = r.ReadObject<SystemTime>();
            m_targetMarkerHandle = r.ReadUInt32();
            m_carTheftHelpShown = r.ReadBool();
            m_allTaxisHaveNitro = r.ReadBool();
            m_prostitutesPayYou = r.ReadBool();
            r.Align();

            Debug.Assert(r.BaseStream.Position - start == 0x0138, "Invalid SimpleVars size!");
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            long start = w.BaseStream.Position;

            w.Write(m_versionId);
            w.Write(m_saveName, Limits.SaveNameLength);
            w.Write((byte) m_currentMissionPack);
            w.Align();
            w.Write(m_currentLevel);
            w.Write(m_cameraPosition);
            w.Write(m_millisecondsPerGameMinute);
            w.Write(m_lastClockTick);
            w.Write((byte) m_gameMonth);
            w.Write((byte) m_gameMonthDay);
            w.Write((byte) m_gameHour);
            w.Write((byte) m_gameMinute);
            w.Write((byte) m_weekday);  
            w.Write((byte) m_gameMonth);    // deliberately copied
            w.Write((byte) m_gameMonthDay); // deliberately copied
            w.Write((byte) m_gameHour);     // deliberately copied
            w.Write((byte) m_gameMinute);   // deliberately copied
            w.Write(m_timeCopyFlag);
            w.Write((ushort) m_currentPadMode);
            w.Write(m_cheatedFlag);
            w.Align();
            w.Write(m_timeInMilliseconds);
            w.Write(m_timeScale);
            w.Write(m_timeStep);
            w.Write(m_tickTime);
            w.Write(m_frameCounter);
            w.Write((short) m_previousWeather);
            w.Write((short) m_currentWeather);
            w.Write((short) m_forcedWeather);
            w.Write(m_weatherInterpolation);
            w.Align();
            w.Write(m_weatherTypeInList);
            w.Write(m_amountOfRainFallen);
            w.Write(m_inCarCameraMode);
            w.Write(m_onFootCameraMode);
            w.Write(m_currentInterior);
            w.Write(m_invertLook);
            w.Align();
            w.Write(m_extraColorId);
            w.Write(m_extraOn);
            w.Align();
            w.Write(m_extraInterpolation);
            w.Write(m_extraWeather);
            w.Write(m_currentWaterConfiguration);
            w.Write(m_riotMode);
            w.Write(m_unknownRiotRelated);
            w.Align();
            w.Write(m_maxWantedLevel);
            w.Write(m_maxChaos);
            w.Write(m_isFrench);
            w.Write(m_isGerman);
            w.Write(m_isUncensored);
            w.Align();
            w.Write(m_unknown.ToArray(), Limits.UnknownDataLength);
            w.Write((byte) m_cinematicCameraHelpRemaining);
            w.Write(m_saveTime);
            w.Write(m_targetMarkerHandle);
            w.Write(m_carTheftHelpShown);
            w.Write(m_allTaxisHaveNitro);
            w.Write(m_prostitutesPayYou);
            w.Align();

            Debug.Assert(w.BaseStream.Position - start == 0x0138, "Invalid SimpleVars size!");
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

            return m_versionId.Equals(other.m_versionId)
                && m_saveName.Equals(other.m_saveName)
                && m_currentMissionPack.Equals(other.m_currentMissionPack)
                && m_currentLevel.Equals(other.m_currentLevel)
                && m_cameraPosition.Equals(other.m_cameraPosition)
                && m_millisecondsPerGameMinute.Equals(other.m_millisecondsPerGameMinute)
                && m_lastClockTick.Equals(other.m_lastClockTick)
                && m_gameMonth.Equals(other.m_gameMonth)
                && m_gameMonthDay.Equals(other.m_gameMonthDay)
                && m_gameHour.Equals(other.m_gameHour)
                && m_gameMinute.Equals(other.m_gameMinute)
                && m_weekday.Equals(other.m_weekday)
                && m_timeCopyFlag.Equals(other.m_timeCopyFlag)
                && m_currentPadMode.Equals(other.m_currentPadMode)
                && m_cheatedFlag.Equals(other.m_cheatedFlag)
                && m_timeInMilliseconds.Equals(other.m_timeInMilliseconds)
                && m_timeScale.Equals(other.m_timeScale)
                && m_timeStep.Equals(other.m_timeStep)
                && m_tickTime.Equals(other.m_tickTime)
                && m_frameCounter.Equals(other.m_frameCounter)
                && m_previousWeather.Equals(other.m_previousWeather)
                && m_currentWeather.Equals(other.m_currentWeather)
                && m_forcedWeather.Equals(other.m_forcedWeather)
                && m_weatherInterpolation.Equals(other.m_weatherInterpolation)
                && m_weatherTypeInList.Equals(other.m_weatherTypeInList)
                && m_amountOfRainFallen.Equals(other.m_amountOfRainFallen)
                && m_inCarCameraMode.Equals(other.m_inCarCameraMode)
                && m_onFootCameraMode.Equals(other.m_onFootCameraMode)
                && m_currentInterior.Equals(other.m_currentInterior)
                && m_invertLook.Equals(other.m_invertLook)
                && m_extraColorId.Equals(other.m_extraColorId)
                && m_extraOn.Equals(other.m_extraOn)
                && m_extraInterpolation.Equals(other.m_extraInterpolation)
                && m_extraWeather.Equals(other.m_extraWeather)
                && m_currentWaterConfiguration.Equals(other.m_currentWaterConfiguration)
                && m_riotMode.Equals(other.m_riotMode)
                && m_unknownRiotRelated.Equals(other.m_unknownRiotRelated)
                && m_maxWantedLevel.Equals(other.m_maxWantedLevel)
                && m_maxChaos.Equals(other.m_maxChaos)
                && m_isFrench.Equals(other.m_isFrench)
                && m_isGerman.Equals(other.m_isGerman)
                && m_isUncensored.Equals(other.m_isUncensored)
                && m_cinematicCameraHelpRemaining.Equals(other.m_cinematicCameraHelpRemaining)
                && m_saveTime.Equals(other.m_saveTime)
                && m_targetMarkerHandle.Equals(other.m_targetMarkerHandle)
                && m_carTheftHelpShown.Equals(other.m_carTheftHelpShown)
                && m_allTaxisHaveNitro.Equals(other.m_allTaxisHaveNitro)
                && m_prostitutesPayYou.Equals(other.m_prostitutesPayYou);
        }
    }
}
