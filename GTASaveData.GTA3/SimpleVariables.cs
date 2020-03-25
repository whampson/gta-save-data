using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class SimpleVariables : SaveDataObject, IEquatable<SimpleVariables>
    {
        public static class Limits
        {
            public const int MaxSaveNameLength = 24;
        }

        private const int SizeOfSimpleVariablesPC = 0xBC;

        private string m_saveName;
        private SystemTime m_timeLastSaved;
        private int m_saveSize;
        private LevelType m_currLevel;
        private Vector m_cameraPosition;
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
        private Date m_compileDateAndTime;
        private int m_weatherTypeInList;
        private float m_cameraCarZoomIndicator;
        private float m_cameraPedZoomIndicator;

        public string SaveName
        {
            get { return m_saveName; }
            set { m_saveName = value; OnPropertyChanged(); }
        }

        public SystemTime TimeLastSaved
        {
            get { return m_timeLastSaved; }
            set { m_timeLastSaved = value; OnPropertyChanged(); }
        }

        public int SaveSize
        {
            get { return m_saveSize; }
            set { m_saveSize = value; OnPropertyChanged(); }
        }

        public LevelType CurrLevel
        {
            get { return m_currLevel; }
            set { m_currLevel = value; OnPropertyChanged(); }
        }

        public Vector CameraPosition
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

        public float WeatherInterpolationValue
        {
            get { return m_weatherInterpolationValue; }
            set { m_weatherInterpolationValue = value; OnPropertyChanged(); }
        }

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

        public SimpleVariables()
        {
            SaveName = string.Empty;
            TimeLastSaved = new SystemTime();
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            SaveName = buf.ReadString(Limits.MaxSaveNameLength, true);
            TimeLastSaved = buf.ReadObject<SystemTime>();
            SaveSize = buf.ReadInt32();
            CurrLevel = (LevelType) buf.ReadInt32();
            CameraPosition = buf.ReadObject<Vector>();
            MillisecondsPerGameMinute = buf.ReadInt32();
            LastClockTick = buf.ReadUInt32();
            GameClockHours = (byte) buf.ReadInt32();
            buf.Align4Bytes();
            GameClockMinutes = (byte) buf.ReadInt32();
            buf.Align4Bytes();
            CurrPadMode = buf.ReadInt16();
            buf.Align4Bytes();
            TimeInMilliseconds = buf.ReadUInt32();
            TimerTimeScale = buf.ReadSingle();
            TimerTimeStep = buf.ReadSingle();
            TimerTimeStepNonClipped = buf.ReadSingle();
            FrameCounter = buf.ReadUInt32();
            TimeStep = buf.ReadSingle();
            FramesPerUpdate = buf.ReadSingle();
            TimeScale = buf.ReadSingle();
            OldWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4Bytes();
            NewWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4Bytes();
            ForcedWeatherType = (WeatherType) buf.ReadInt16();
            buf.Align4Bytes();
            WeatherInterpolationValue = buf.ReadSingle();
            CompileDateAndTime = buf.ReadObject<Date>();
            WeatherTypeInList = buf.ReadInt32();
            CameraCarZoomIndicator = buf.ReadSingle();
            CameraPedZoomIndicator = buf.ReadSingle();

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(SaveName, Limits.MaxSaveNameLength, true);
            buf.Write(TimeLastSaved);
            buf.Write(SaveSize);
            buf.Write((int) CurrLevel);
            buf.Write(CameraPosition);
            buf.Write(MillisecondsPerGameMinute);
            buf.Write(LastClockTick);
            buf.Write(GameClockHours);
            buf.Align4Bytes();
            buf.Write(GameClockMinutes);
            buf.Align4Bytes();
            buf.Write(CurrPadMode);
            buf.Align4Bytes();
            buf.Write(TimeInMilliseconds);
            buf.Write(TimerTimeScale);
            buf.Write(TimerTimeStep);
            buf.Write(TimerTimeStepNonClipped);
            buf.Write(FrameCounter);
            buf.Write(TimeStep);
            buf.Write(FramesPerUpdate);
            buf.Write(TimeScale);
            buf.Write((short) OldWeatherType);
            buf.Align4Bytes();
            buf.Write((short) NewWeatherType);
            buf.Align4Bytes();
            buf.Write((short) ForcedWeatherType);
            buf.Align4Bytes();
            buf.Write(WeatherInterpolationValue);
            buf.Write(CompileDateAndTime);
            buf.Write(WeatherTypeInList);
            buf.Write(CameraCarZoomIndicator);
            buf.Write(CameraPedZoomIndicator);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(SaveFileFormat fmt)
        {
            if (fmt.SupportedOnPC)
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

            return SaveName.Equals(other.SaveName)
                && TimeLastSaved.Equals(other.TimeLastSaved)
                && SaveSize.Equals(other.SaveSize)
                && CurrLevel.Equals(other.CurrLevel)
                && CameraPosition.Equals(other.CameraPosition)
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
                && WeatherInterpolationValue.Equals(other.WeatherInterpolationValue)
                && CompileDateAndTime.Equals(other.CompileDateAndTime)
                && WeatherTypeInList.Equals(other.WeatherTypeInList)
                && CameraCarZoomIndicator.Equals(other.CameraCarZoomIndicator)
                && CameraPedZoomIndicator.Equals(other.CameraPedZoomIndicator);
        }
    }
}
