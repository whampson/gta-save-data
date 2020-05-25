using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    public class Zone : SaveDataObject, IEquatable<Zone>
    {
        public static class Limits
        {
            public const int MaxNameLength = 8;
        }

        private string m_name;
        private Vector3D m_min;
        private Vector3D m_max;
        private ZoneType m_type;
        private Level m_level;
        private short m_zoneInfoDay;
        private short m_zoneInfoNight;
        private int m_childZoneIndex;
        private int m_parentZoneIndex;
        private int m_nextZoneIndex;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public Vector3D Min
        {
            get { return m_min; }
            set { m_min = value; OnPropertyChanged(); }
        }

        public Vector3D Max
        {
            get { return m_max; }
            set { m_max = value; OnPropertyChanged(); }
        }

        public ZoneType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public Level Level
        {
            get { return m_level; }
            set { m_level = value; OnPropertyChanged(); }
        }

        public short ZoneInfoDay
        {
            get { return m_zoneInfoDay; }
            set { m_zoneInfoDay = value; OnPropertyChanged(); }
        }

        public short ZoneInfoNight
        {
            get { return m_zoneInfoNight; }
            set { m_zoneInfoNight = value; OnPropertyChanged(); }
        }

        public int ChildZoneIndex
        {
            get { return m_childZoneIndex; }
            set { m_childZoneIndex = value; OnPropertyChanged(); }
        }

        public int ParentZoneIndex
        {
            get { return m_parentZoneIndex; }
            set { m_parentZoneIndex = value; OnPropertyChanged(); }
        }

        public int NextZoneIndex
        {
            get { return m_nextZoneIndex; }
            set { m_nextZoneIndex = value; OnPropertyChanged(); }
        }


        public Zone()
        {
            Name = "";
        }

        protected override void ReadData(StreamBuffer buf, SaveDataFormat fmt)
        {
            Name = buf.ReadString(Limits.MaxNameLength);
            Min = buf.Read<Vector3D>();
            Max = buf.Read<Vector3D>();
            Type = (ZoneType) buf.ReadInt32();
            Level = (Level) buf.ReadInt32();
            ZoneInfoDay = buf.ReadInt16();
            ZoneInfoNight = buf.ReadInt16();
            ChildZoneIndex = buf.ReadInt32();
            ParentZoneIndex = buf.ReadInt32();
            NextZoneIndex = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<Zone>());
        }

        protected override void WriteData(StreamBuffer buf, SaveDataFormat fmt)
        {
            buf.Write(Name.PadRight(Limits.MaxNameLength, '\0'), Limits.MaxNameLength);
            buf.Write(Min);
            buf.Write(Max);
            buf.Write((int) Type);
            buf.Write((int) Level);
            buf.Write(ZoneInfoDay);
            buf.Write(ZoneInfoNight);
            buf.Write(ChildZoneIndex);
            buf.Write(ParentZoneIndex);
            buf.Write(NextZoneIndex);

            Debug.Assert(buf.Offset == SizeOf<Zone>());
        }

        protected override int GetSize(SaveDataFormat fmt)
        {
            return 56;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Zone);
        }

        public bool Equals(Zone other)
        {
            if (other == null)
            {
                return false;
            }

            return Name.Equals(other.Name)
                && Min.Equals(other.Min)
                && Max.Equals(other.Max)
                && Type.Equals(other.Type)
                && Level.Equals(other.Level)
                && ZoneInfoDay.Equals(other.ZoneInfoDay)
                && ZoneInfoNight.Equals(other.ZoneInfoNight)
                && ChildZoneIndex.Equals(other.ChildZoneIndex)
                && ParentZoneIndex.Equals(other.ParentZoneIndex)
                && NextZoneIndex.Equals(other.NextZoneIndex);
        }
    }

    public enum ZoneType
    {
        Audio,
        Info,
        Navig,
        Map
    }
}