using System;
using System.Diagnostics;
using System.Numerics;
using GTASaveData.Interfaces;

namespace GTASaveData.GTA3
{
    public class Zone : SaveDataObject,
        IEquatable<Zone>, IDeepClonable<Zone>
    {
        public const int MaxNameLength = 8;

        private string m_name;
        private Vector3 m_min;
        private Vector3 m_max;
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

        public Vector3 Min
        {
            get { return m_min; }
            set { m_min = value; OnPropertyChanged(); }
        }

        public Vector3 Max
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
            Min = new Vector3();
            Max = new Vector3();
        }

        public Zone(Zone other)
        {
            Name = other.Name;
            Min = other.Min;
            Max = other.Max;
            Type = other.Type;
            Level = other.Level;
            ZoneInfoDay = other.ZoneInfoDay;
            ZoneInfoNight = other.ZoneInfoNight;
            ChildZoneIndex = other.ChildZoneIndex;
            ParentZoneIndex = other.ParentZoneIndex;
            NextZoneIndex = other.NextZoneIndex;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            Name = buf.ReadString(MaxNameLength);
            Min = buf.ReadStruct<Vector3>();
            Max = buf.ReadStruct<Vector3>();
            Type = (ZoneType) buf.ReadInt32();
            Level = (Level) buf.ReadInt32();
            ZoneInfoDay = buf.ReadInt16();
            ZoneInfoNight = buf.ReadInt16();
            ChildZoneIndex = buf.ReadInt32();
            ParentZoneIndex = buf.ReadInt32();
            NextZoneIndex = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<Zone>());
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(Name.PadRight(MaxNameLength, '\0'), MaxNameLength);
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

        protected override int GetSize(FileFormat fmt)
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

        public Zone DeepClone()
        {
            return new Zone(this);
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