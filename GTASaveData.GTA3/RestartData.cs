﻿using System;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class RestartData : SaveDataObject,
        IEquatable<RestartData>, IDeepClonable<RestartData>
    {
        public const int MaxNumWastedRestarts = 8;
        public const int MaxNumBustedRestarts = 8;

        private ObservableArray<RestartPoint> m_wastedRestartPoints;
        private ObservableArray<RestartPoint> m_bustedRestartPoints;
        private short m_numberOfWastedRestartPoints;
        private short m_numberOfBustedRestartPoints;
        private bool m_overrideNextRestart;
        private RestartPoint m_overrideRestartPoint;
        private bool m_fadeInAfteNextDeath;
        private bool m_fadeInAfteNextArrest;
        private Level m_overrideHospitalLevel;
        private Level m_overridePoliceStationLevel;

        public ObservableArray<RestartPoint> WastedRestartPoints
        {
            get { return m_wastedRestartPoints; }
            set { m_wastedRestartPoints = value; OnPropertyChanged(); }
        }

        public ObservableArray<RestartPoint> BustedRestartPoints
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

        public Level OverrideHospitalLevel
        {
            get { return m_overrideHospitalLevel; }
            set { m_overrideHospitalLevel = value; OnPropertyChanged(); }
        }

        public Level OverridePoliceStationLevel
        {
            get { return m_overridePoliceStationLevel; }
            set { m_overridePoliceStationLevel = value; OnPropertyChanged(); }
        }

        public RestartData()
        {
            WastedRestartPoints = ArrayHelper.CreateArray<RestartPoint>(MaxNumWastedRestarts);
            BustedRestartPoints = ArrayHelper.CreateArray<RestartPoint>(MaxNumBustedRestarts);
            OverrideRestartPoint = new RestartPoint();
        }

        public RestartData(RestartData other)
        {
            WastedRestartPoints = ArrayHelper.DeepClone(other.WastedRestartPoints);
            BustedRestartPoints = ArrayHelper.DeepClone(other.BustedRestartPoints);
            NumberOfWastedRestartPoints = other.NumberOfWastedRestartPoints;
            NumberOfBustedRestartPoints = other.NumberOfBustedRestartPoints;
            OverrideNextRestart = other.OverrideNextRestart;
            OverrideRestartPoint = new RestartPoint(other.OverrideRestartPoint);
            FadeInAfteNextDeath = other.FadeInAfteNextDeath;
            FadeInAfteNextArrest = other.FadeInAfteNextArrest;
            OverrideHospitalLevel = other.OverrideHospitalLevel;
            OverridePoliceStationLevel = other.OverridePoliceStationLevel;
        }

        protected override void ReadData(DataBuffer buf, FileType fmt)
        {
            int size = SaveFileGTA3VC.ReadBlockHeader(buf, out string tag);
            Debug.Assert(tag == "RST");

            WastedRestartPoints = buf.ReadArray<RestartPoint>(MaxNumWastedRestarts);
            BustedRestartPoints = buf.ReadArray<RestartPoint>(MaxNumBustedRestarts);
            NumberOfWastedRestartPoints = buf.ReadInt16();
            NumberOfBustedRestartPoints = buf.ReadInt16();
            OverrideNextRestart = buf.ReadBool();
            buf.ReadBytes(3);
            OverrideRestartPoint = buf.ReadObject<RestartPoint>();
            FadeInAfteNextDeath = buf.ReadBool();
            FadeInAfteNextArrest = buf.ReadBool();
            OverrideHospitalLevel = (Level) buf.ReadByte();
            OverridePoliceStationLevel = (Level) buf.ReadByte();

            Debug.Assert(buf.Offset == size + SaveFileGTA3VC.BlockHeaderSize);
            Debug.Assert(size == SizeOf<RestartData>() - SaveFileGTA3VC.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, FileType fmt)
        {
            SaveFileGTA3VC.WriteBlockHeader(buf, "RST", SizeOf<RestartData>() - SaveFileGTA3VC.BlockHeaderSize);

            buf.Write(WastedRestartPoints, MaxNumWastedRestarts);
            buf.Write(BustedRestartPoints, MaxNumBustedRestarts);
            buf.Write(NumberOfWastedRestartPoints);
            buf.Write(NumberOfBustedRestartPoints);
            buf.Write(OverrideNextRestart);
            buf.Write(new byte[3]);
            buf.Write(OverrideRestartPoint);
            buf.Write(FadeInAfteNextDeath);
            buf.Write(FadeInAfteNextArrest);
            buf.Write((byte) OverrideHospitalLevel);
            buf.Write((byte) OverridePoliceStationLevel);

            Debug.Assert(buf.Offset == SizeOf<RestartData>());
        }

        protected override int GetSize(FileType fmt)
        {
            return 0x124;
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

        public RestartData DeepClone()
        {
            return new RestartData(this);
        }
    }
}
