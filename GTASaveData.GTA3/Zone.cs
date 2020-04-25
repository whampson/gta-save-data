using GTASaveData.Types;
using System;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    [Size(56)]
    public class Zone : SaveDataObject, IEquatable<Zone>
    {
        public static class Limits
        {
            public const int MaxNameLength = 8;
        }

        private string m_name;
        private Vector m_min;
        private Vector m_max;
        private ZoneType m_type;
        private LevelType m_level;
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

        public Vector Min
        {
            get { return m_min; }
            set { m_min = value; OnPropertyChanged(); }
        }

        public Vector Max
        {
            get { return m_max; }
            set { m_max = value; OnPropertyChanged(); }
        }

        public ZoneType Type
        {
            get { return m_type; }
            set { m_type = value; OnPropertyChanged(); }
        }

        public LevelType Level
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
            Min = new Vector();
            Max = new Vector();
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Name = buf.ReadString(Limits.MaxNameLength);
            Min = buf.Read<Vector>();
            Max = buf.Read<Vector>();
            Type = (ZoneType) buf.ReadInt32();
            Level = (LevelType) buf.ReadInt32();
            ZoneInfoDay = buf.ReadInt16();
            ZoneInfoNight = buf.ReadInt16();
            ChildZoneIndex = buf.ReadInt32();
            ParentZoneIndex = buf.ReadInt32();
            NextZoneIndex = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<Zone>());
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Name, Limits.MaxNameLength);
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
}