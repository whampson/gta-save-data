using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    [Size(0x124)]
    public class RestartData : SaveDataObject, IEquatable<RestartData>
    {
        public static class Limits
        {
            public const int MaxNumWastedRestarts = 8;
            public const int MaxNumBustedRestarts = 8;
        }

        private Array<RestartPoint> m_wastedRestartPoints;
        private Array<RestartPoint> m_bustedRestartPoints;
        private short m_numberOfWastedRestartPoints;
        private short m_numberOfBustedRestartPoints;
        private bool m_overrideNextRestart;
        private RestartPoint m_overrideRestartPoint;
        private bool m_fadeInAfteNextDeath;
        private bool m_fadeInAfteNextArrest;
        private LevelType m_overrideHospitalLevel;
        private LevelType m_overridePoliceStationLevel;

        public Array<RestartPoint> WastedRestartPoints
        {
            get { return m_wastedRestartPoints; }
            set { m_wastedRestartPoints = value; OnPropertyChanged(); }
        }

        public Array<RestartPoint> BustedRestartPoints
        {
            get { return m_bustedRestartPoints; }
            set { m_bustedRestartPoints = value; OnPropertyChanged(); }
        }

        public short NumberOfWastedRestartPoints
        {
            get { return m_numberOfWastedRestartPoints; }
            set { m_numberOfWastedRestartPoints = value; OnPropertyChanged(); }
        }

        public short NumberOfBustedRestartPoints
        {
            get { return m_numberOfBustedRestartPoints; }
            set { m_numberOfBustedRestartPoints = value; OnPropertyChanged(); }
        }

        public bool OverrideNextRestart
        {
            get { return m_overrideNextRestart; }
            set { m_overrideNextRestart = value; OnPropertyChanged(); }
        }

        public RestartPoint OverrideRestartPoint
        {
            get { return m_overrideRestartPoint; }
            set { m_overrideRestartPoint = value; OnPropertyChanged(); }
        }

        public bool FadeInAfteNextDeath
        {
            get { return m_fadeInAfteNextDeath; }
            set { m_fadeInAfteNextDeath = value; OnPropertyChanged(); }
        }

        public bool FadeInAfteNextArrest
        {
            get { return m_fadeInAfteNextArrest; }
            set { m_fadeInAfteNextArrest = value; OnPropertyChanged(); }
        }

        public LevelType OverrideHospitalLevel
        {
            get { return m_overrideHospitalLevel; }
            set { m_overrideHospitalLevel = value; OnPropertyChanged(); }
        }

        public LevelType OverridePoliceStationLevel
        {
            get { return m_overridePoliceStationLevel; }
            set { m_overridePoliceStationLevel = value; OnPropertyChanged(); }
        }


        public RestartData()
        {
            WastedRestartPoints = new Array<RestartPoint>();
            BustedRestartPoints = new Array<RestartPoint>();
            OverrideRestartPoint = new RestartPoint();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "RST");

            WastedRestartPoints = buf.Read<RestartPoint>(Limits.MaxNumWastedRestarts);
            BustedRestartPoints = buf.Read<RestartPoint>(Limits.MaxNumBustedRestarts);
            NumberOfWastedRestartPoints = buf.ReadInt16();
            NumberOfBustedRestartPoints = buf.ReadInt16();
            OverrideNextRestart = buf.ReadBool();
            buf.Align4Bytes();
            OverrideRestartPoint = buf.Read<RestartPoint>();
            FadeInAfteNextDeath = buf.ReadBool();
            FadeInAfteNextArrest = buf.ReadBool();
            OverrideHospitalLevel = (LevelType) buf.ReadByte();
            OverridePoliceStationLevel = (LevelType) buf.ReadByte();

            Debug.Assert(buf.Offset == size + GTA3Save.SaveHeaderSize);
            Debug.Assert(size == SizeOf<RestartData>() - GTA3Save.SaveHeaderSize);
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            GTA3Save.WriteSaveHeader(buf, "RST", SizeOf<RestartData>() - GTA3Save.SaveHeaderSize);

            buf.Write(WastedRestartPoints.ToArray(), Limits.MaxNumWastedRestarts);
            buf.Write(BustedRestartPoints.ToArray(), Limits.MaxNumBustedRestarts);
            buf.Write(NumberOfWastedRestartPoints);
            buf.Write(NumberOfBustedRestartPoints);
            buf.Write(OverrideNextRestart);
            buf.Align4Bytes();
            buf.Write(OverrideRestartPoint);
            buf.Write(FadeInAfteNextDeath);
            buf.Write(FadeInAfteNextArrest);
            buf.Write((byte) OverrideHospitalLevel);
            buf.Write((byte) OverridePoliceStationLevel);

            Debug.Assert(buf.Offset == SizeOf<RestartData>());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RestartData);
        }

        public bool Equals(RestartData other)
        {
            if (other == null)
            {
                return false;
            }

            return WastedRestartPoints.SequenceEqual(other.WastedRestartPoints)
                && BustedRestartPoints.SequenceEqual(other.BustedRestartPoints)
                && NumberOfWastedRestartPoints.Equals(other.NumberOfWastedRestartPoints)
                && NumberOfBustedRestartPoints.Equals(other.NumberOfBustedRestartPoints)
                && OverrideNextRestart.Equals(other.OverrideNextRestart)
                && OverrideRestartPoint.Equals(other.OverrideRestartPoint)
                && FadeInAfteNextDeath.Equals(other.FadeInAfteNextDeath)
                && FadeInAfteNextArrest.Equals(other.FadeInAfteNextArrest)
                && OverrideHospitalLevel.Equals(other.OverrideHospitalLevel)
                && OverridePoliceStationLevel.Equals(other.OverridePoliceStationLevel);
        }
    }
}
