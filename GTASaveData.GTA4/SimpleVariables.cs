using System;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA4
{
    public class SimpleVariables : SaveDataObject, ISimpleVariables,
        IEquatable<SimpleVariables>, IDeepClonable<SimpleVariables>
    {
        private int m_closestSafehouseIndex;
        private bool m_fadeInAfterLoad;
        private int m_unknown04h;
        private int m_unknown08h;
        private Vector3 m_cameraPosition;
        private int m_unknown1Ch;
        private int m_millisecondsPerGameMinute;
        private uint m_lastClockTick;
        private int m_gameClockMonths;
        private int m_gameClockDays;
        private int m_gameClockHours;
        private int m_gameClockMinutes;
        private int m_gameClockDayOfWeek;
        private bool m_hasPlayerCheated;
        private uint m_timeInMilliseconds;
        private uint m_frameCounter;
        private WeatherType m_oldWeatherType;
        private WeatherType m_newWeatherType;
        private WeatherType m_forcedWeatherType;
        private float m_weatherInterpolationValue;
        private int m_weatherTypeInList;
        private float m_rain;
        private int m_cameraCarZoomIndicator;
        private int m_cameraPedZoomIndicator;
        private int m_cameraGunZoomIndicator;
        private int m_unknown6Ch;
        private int m_unknown70h;
        private int m_unknown74h;
        private int m_unknown78h;
        private int m_unknown7Ch;
        private int m_maximumWantedLevel;
        private int m_maximumChaos;
        private int m_unknown88h;
        private int m_unknown8Ch;
        private byte m_unknown90h;
        private int m_targetMarkerHandle;
        private byte m_unknown98h;
        private int m_unknown9Ch;
        private int m_unknownA0h;
        private int m_unknownA4h;
        private int m_unknownA8h;
        private int m_unknownACh;

        public int ClosestSafehouseIndex
        {
            get { return m_closestSafehouseIndex; }
            set { m_closestSafehouseIndex = value; OnPropertyChanged(); }
        }

        public bool FadeInAfterLoad
        {
            get { return m_fadeInAfterLoad; }
            set { m_fadeInAfterLoad = value; OnPropertyChanged(); }
        }

        public int Unknown04h
        {
            get { return m_unknown04h; }
            set { m_unknown04h = value; OnPropertyChanged(); }
        }

        public int Unknown08h
        {
            get { return m_unknown08h; }
            set { m_unknown08h = value; OnPropertyChanged(); }
        }

        public Vector3 CameraPosition
        {
            get { return m_cameraPosition; }
            set { m_cameraPosition = value; OnPropertyChanged(); }
        }

        public int Unknown1Ch
        {
            get { return m_unknown1Ch; }
            set { m_unknown1Ch = value; OnPropertyChanged(); }
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

        public int GameClockMonths
        {
            get { return m_gameClockMonths; }
            set { m_gameClockMonths = value; OnPropertyChanged(); }
        }

        public int GameClockDays
        {
            get { return m_gameClockDays; }
            set { m_gameClockDays = value; OnPropertyChanged(); }
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

        public int GameClockDayOfWeek
        {
            get { return m_gameClockDayOfWeek; }
            set { m_gameClockDayOfWeek = value; OnPropertyChanged(); }
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

        public int CameraGunZoomIndicator
        {
            get { return m_cameraGunZoomIndicator; }
            set { m_cameraGunZoomIndicator = value; OnPropertyChanged(); }
        }

        public int Unknown6Ch
        {
            get { return m_unknown6Ch; }
            set { m_unknown6Ch = value; OnPropertyChanged(); }
        }

        public int Unknown70h
        {
            get { return m_unknown70h; }
            set { m_unknown70h = value; OnPropertyChanged(); }
        }

        public int Unknown74h
        {
            get { return m_unknown74h; }
            set { m_unknown74h = value; OnPropertyChanged(); }
        }

        public int Unknown78h
        {
            get { return m_unknown78h; }
            set { m_unknown78h = value; OnPropertyChanged(); }
        }

        public int Unknown7Ch
        {
            get { return m_unknown7Ch; }
            set { m_unknown7Ch = value; OnPropertyChanged(); }
        }

        public int MaximumWantedLevel
        {
            get { return m_maximumWantedLevel; }
            set { m_maximumWantedLevel = value; OnPropertyChanged(); }
        }

        public int MaximumChaos
        {
            get { return m_maximumChaos; }
            set { m_maximumChaos = value; OnPropertyChanged(); }
        }

        public int Unknown88h
        {
            get { return m_unknown88h; }
            set { m_unknown88h = value; OnPropertyChanged(); }
        }

        public int Unknown8Ch
        {
            get { return m_unknown8Ch; }
            set { m_unknown8Ch = value; OnPropertyChanged(); }
        }

        public byte Unknown90h
        {
            get { return m_unknown90h; }
            set { m_unknown90h = value; OnPropertyChanged(); }
        }

        public int TargetMarkerHandle
        {
            get { return m_targetMarkerHandle; }
            set { m_targetMarkerHandle = value; OnPropertyChanged(); }
        }

        public byte Unknown98h
        {
            get { return m_unknown98h; }
            set { m_unknown98h = value; OnPropertyChanged(); }
        }

        public int Unknown9Ch
        {
            get { return m_unknown9Ch; }
            set { m_unknown9Ch = value; OnPropertyChanged(); }
        }

        public int UnknownA0h
        {
            get { return m_unknownA0h; }
            set { m_unknownA0h = value; OnPropertyChanged(); }
        }

        public int UnknownA4h
        {
            get { return m_unknownA4h; }
            set { m_unknownA4h = value; OnPropertyChanged(); }
        }

        public int UnknownA8h
        {
            get { return m_unknownA8h; }
            set { m_unknownA8h = value; OnPropertyChanged(); }
        }

        public int UnknownACh
        {
            get { return m_unknownACh; }
            set { m_unknownACh = value; OnPropertyChanged(); }
        }

        public SimpleVariables()
        {
            CameraPosition = new Vector3();
        }

        public SimpleVariables(SimpleVariables other)
        {
            ClosestSafehouseIndex = other.ClosestSafehouseIndex;
            FadeInAfterLoad = other.FadeInAfterLoad;
            Unknown04h = other.Unknown04h;
            Unknown08h = other.Unknown08h;
            CameraPosition = other.CameraPosition;
            MillisecondsPerGameMinute = other.MillisecondsPerGameMinute;
            LastClockTick = other.LastClockTick;
            GameClockMonths = other.GameClockMonths;
            GameClockDays = other.GameClockDays;
            GameClockHours = other.GameClockHours;
            GameClockMinutes = other.GameClockMinutes;
            GameClockDayOfWeek = other.GameClockDayOfWeek;
            HasPlayerCheated = other.HasPlayerCheated;
            TimeInMilliseconds = other.TimeInMilliseconds;
            FrameCounter = other.FrameCounter;
            OldWeatherType = other.OldWeatherType;
            NewWeatherType = other.NewWeatherType;
            ForcedWeatherType = other.ForcedWeatherType;
            WeatherInterpolation = other.WeatherInterpolation;
            WeatherTypeInList = other.WeatherTypeInList;
            Rain = other.Rain;
            CameraCarZoomIndicator = other.CameraCarZoomIndicator;
            CameraPedZoomIndicator = other.CameraPedZoomIndicator;
            CameraGunZoomIndicator = other.CameraGunZoomIndicator;
            Unknown6Ch = other.Unknown6Ch;
            Unknown70h = other.Unknown70h;
            Unknown74h = other.Unknown74h;
            Unknown78h = other.Unknown78h;
            Unknown7Ch = other.Unknown7Ch;
            MaximumWantedLevel = other.MaximumWantedLevel;
            MaximumChaos = other.MaximumChaos;
            Unknown88h = other.Unknown88h;
            Unknown8Ch = other.Unknown8Ch;
            Unknown90h = other.Unknown90h;
            TargetMarkerHandle = other.TargetMarkerHandle;
            Unknown98h = other.Unknown98h;
            Unknown9Ch = other.Unknown9Ch;
            UnknownA0h = other.UnknownA0h;
            UnknownA4h = other.UnknownA4h;
            UnknownA8h = other.UnknownA8h;
            UnknownACh = other.UnknownACh;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            ClosestSafehouseIndex = buf.ReadInt32();
            FadeInAfterLoad = buf.ReadBool();
            buf.Skip(3);
            Unknown04h = buf.ReadInt32();
            Unknown08h = buf.ReadInt32();
            CameraPosition = new Vector3()
            {
                X = buf.ReadFloat(),
                Y = buf.ReadFloat(),
                Z = buf.ReadFloat(),
            };
            Unknown1Ch = buf.ReadInt32();
            MillisecondsPerGameMinute = buf.ReadInt32();
            LastClockTick = buf.ReadUInt32();
            GameClockMonths = buf.ReadInt32();
            GameClockDays = buf.ReadInt32();
            GameClockHours = buf.ReadInt32();
            GameClockMinutes = buf.ReadInt32();
            GameClockDayOfWeek = buf.ReadInt32();
            HasPlayerCheated = buf.ReadBool();
            buf.Skip(3);
            TimeInMilliseconds = buf.ReadUInt32();
            FrameCounter = buf.ReadUInt32();
            OldWeatherType = (WeatherType) buf.ReadInt32();
            NewWeatherType = (WeatherType) buf.ReadInt32();
            ForcedWeatherType = (WeatherType) buf.ReadInt32();
            WeatherInterpolation = buf.ReadFloat();
            WeatherTypeInList = buf.ReadInt32();
            Rain = buf.ReadFloat();
            CameraCarZoomIndicator = buf.ReadInt32();
            CameraPedZoomIndicator = buf.ReadInt32();
            CameraGunZoomIndicator = buf.ReadInt32();
            Unknown6Ch = buf.ReadInt32();
            Unknown70h = buf.ReadInt32();
            Unknown74h = buf.ReadInt32();
            Unknown78h = buf.ReadInt32();
            Unknown7Ch = buf.ReadInt32();
            MaximumWantedLevel = buf.ReadInt32();
            MaximumChaos = buf.ReadInt32();
            Unknown88h = buf.ReadInt32();
            Unknown8Ch = buf.ReadInt32();
            Unknown90h = buf.ReadByte();
            buf.Skip(3);
            TargetMarkerHandle = buf.ReadInt32();
            Unknown98h = buf.ReadByte();
            buf.Skip(3);
            Unknown9Ch = buf.ReadInt32();
            UnknownA0h = buf.ReadInt32();
            UnknownA4h = buf.ReadInt32();
            UnknownA8h = buf.ReadInt32();
            UnknownACh = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOfType<SimpleVariables>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(ClosestSafehouseIndex);
            buf.Write(FadeInAfterLoad);
            buf.Write(Unknown04h);
            buf.Write(Unknown08h);
            buf.Write(CameraPosition.X);
            buf.Write(CameraPosition.Y);
            buf.Write(CameraPosition.Z);
            buf.Write(Unknown1Ch);
            buf.Write(MillisecondsPerGameMinute);
            buf.Write(LastClockTick);
            buf.Write(GameClockMonths);
            buf.Write(GameClockDays);
            buf.Write(GameClockHours);
            buf.Write(GameClockMinutes);
            buf.Write(GameClockDayOfWeek);
            buf.Write(HasPlayerCheated);
            buf.Write(TimeInMilliseconds);
            buf.Write(FrameCounter);
            buf.Write((int) OldWeatherType);
            buf.Write((int) NewWeatherType);
            buf.Write((int) ForcedWeatherType);
            buf.Write(WeatherInterpolation);
            buf.Write(WeatherTypeInList);
            buf.Write(Rain);
            buf.Write(CameraCarZoomIndicator);
            buf.Write(CameraPedZoomIndicator);
            buf.Write(CameraGunZoomIndicator);
            buf.Write(Unknown6Ch);
            buf.Write(Unknown70h);
            buf.Write(Unknown74h);
            buf.Write(Unknown78h);
            buf.Write(Unknown7Ch);
            buf.Write(MaximumWantedLevel);
            buf.Write(MaximumChaos);
            buf.Write(Unknown88h);
            buf.Write(Unknown8Ch);
            buf.Write(Unknown90h);
            buf.Write(TargetMarkerHandle);
            buf.Write(Unknown98h);
            buf.Write(Unknown9Ch);
            buf.Write(UnknownA0h);
            buf.Write(UnknownA4h);
            buf.Write(UnknownA8h);
            buf.Write(UnknownACh);

            Debug.Assert(buf.Offset == SizeOfType<SimpleVariables>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 0xB0;
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

            return ClosestSafehouseIndex.Equals(other.ClosestSafehouseIndex)
                && FadeInAfterLoad.Equals(other.FadeInAfterLoad)
                && Unknown04h.Equals(other.Unknown04h)
                && Unknown08h.Equals(other.Unknown08h)
                && CameraPosition.Equals(other.CameraPosition)
                && MillisecondsPerGameMinute.Equals(other.MillisecondsPerGameMinute)
                && LastClockTick.Equals(other.LastClockTick)
                && GameClockMonths.Equals(other.GameClockMonths)
                && GameClockDays.Equals(other.GameClockDays)
                && GameClockHours.Equals(other.GameClockHours)
                && GameClockMinutes.Equals(other.GameClockMinutes)
                && GameClockDayOfWeek.Equals(other.GameClockDayOfWeek)
                && HasPlayerCheated.Equals(other.HasPlayerCheated)
                && TimeInMilliseconds.Equals(other.TimeInMilliseconds)
                && FrameCounter.Equals(other.FrameCounter)
                && OldWeatherType.Equals(other.OldWeatherType)
                && NewWeatherType.Equals(other.NewWeatherType)
                && ForcedWeatherType.Equals(other.ForcedWeatherType)
                && WeatherInterpolation.Equals(other.WeatherInterpolation)
                && WeatherTypeInList.Equals(other.WeatherTypeInList)
                && Rain.Equals(other.Rain)
                && CameraCarZoomIndicator.Equals(other.CameraCarZoomIndicator)
                && CameraPedZoomIndicator.Equals(other.CameraPedZoomIndicator)
                && CameraGunZoomIndicator.Equals(other.CameraGunZoomIndicator)
                && Unknown6Ch.Equals(other.Unknown6Ch)
                && Unknown70h.Equals(other.Unknown70h)
                && Unknown74h.Equals(other.Unknown74h)
                && Unknown78h.Equals(other.Unknown78h)
                && Unknown7Ch.Equals(other.Unknown7Ch)
                && MaximumWantedLevel.Equals(other.MaximumWantedLevel)
                && MaximumChaos.Equals(other.MaximumChaos)
                && Unknown88h.Equals(other.Unknown88h)
                && Unknown8Ch.Equals(other.Unknown8Ch)
                && Unknown90h.Equals(other.Unknown90h)
                && TargetMarkerHandle.Equals(other.TargetMarkerHandle)
                && Unknown98h.Equals(other.Unknown98h)
                && Unknown9Ch.Equals(other.Unknown9Ch)
                && UnknownA0h.Equals(other.UnknownA0h)
                && UnknownA4h.Equals(other.UnknownA4h)
                && UnknownA8h.Equals(other.UnknownA8h)
                && UnknownACh.Equals(other.UnknownACh);
        }

        public SimpleVariables DeepClone()
        {
            return new SimpleVariables(this);
        }
    }

    // TODO: Copied from VC. Fix for GTA IV
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
