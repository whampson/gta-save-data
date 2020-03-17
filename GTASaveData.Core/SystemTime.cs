using GTASaveData.Serialization;
using System;
using System.Diagnostics;

namespace GTASaveData
{
    /// <summary>
    /// Represents the Win32 <c>SYSTEMTIME</c> structure.
    /// </summary>
    [Size(16)]
    public class SystemTime : SerializableObject,
        IEquatable<SystemTime>
    {
        private ushort m_year;
        private ushort m_month;
        private ushort m_dayOfWeek;
        private ushort m_day;
        private ushort m_hour;
        private ushort m_minute;
        private ushort m_second;
        private ushort m_millisecond;

        public ushort Year
        {
            get { return m_year; }
            set { m_year = value; OnPropertyChanged(); }
        }

        public ushort Month
        {
            get { return m_month; }
            set { m_month = value; OnPropertyChanged(); }
        }

        public ushort DayOfWeek
        {
            get { return m_dayOfWeek; }
            set { m_dayOfWeek = value; OnPropertyChanged(); }
        }

        public ushort Day
        {
            get { return m_day; }
            set { m_day= value; OnPropertyChanged(); }
        }

        public ushort Hour
        {
            get { return m_hour; }
            set { m_hour = value; OnPropertyChanged(); }
        }

        public ushort Minute
        {
            get { return m_minute; }
            set { m_minute = value; OnPropertyChanged(); }
        }

        public ushort Second
        {
            get { return m_second; }
            set { m_second = value; OnPropertyChanged(); }
        }

        public ushort Millisecond
        {
            get { return m_millisecond; }
            set { m_millisecond = value; OnPropertyChanged(); }
        }

        public SystemTime()
        { }

        public SystemTime(DateTime dateTime)
        {
            m_year = (ushort) dateTime.Year;
            m_month = (ushort) dateTime.Month;
            m_dayOfWeek = (ushort) dateTime.DayOfWeek;
            m_day = (ushort) dateTime.Day;
            m_hour = (ushort) dateTime.Hour;
            m_minute = (ushort) dateTime.Minute;
            m_second = (ushort) dateTime.Second;
            m_millisecond = (ushort) dateTime.Millisecond;

            
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_year = r.ReadUInt16();
            m_month = r.ReadUInt16();
            m_dayOfWeek = r.ReadUInt16();
            m_day = r.ReadUInt16();
            m_hour = r.ReadUInt16();
            m_minute = r.ReadUInt16();
            m_second = r.ReadUInt16();
            m_millisecond = r.ReadUInt16();

            Debug.Assert(r.Position() - r.Marked() == SizeOf<SystemTime>());
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_year);
            w.Write(m_month);
            w.Write(m_dayOfWeek);
            w.Write(m_day);
            w.Write(m_hour);
            w.Write(m_minute);
            w.Write(m_second);
            w.Write(m_millisecond);

            Debug.Assert(w.Position() - w.Marked() == SizeOf<SystemTime>());
        }

        public DateTime ToDateTime()
        {
            return new DateTime(
                m_year,
                m_month,
                m_day,
                m_hour,
                m_minute,
                m_second,
                m_millisecond);
        }

        public override string ToString()
        {
            return ToDateTime().ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SystemTime);
        }

        public bool Equals(SystemTime other)
        {
            if (other == null)
            {
                return false;
            }

            return m_year.Equals(other.m_year)
                && m_month.Equals(other.m_month)
                && m_dayOfWeek.Equals(other.m_dayOfWeek)
                && m_day.Equals(other.m_day)
                && m_hour.Equals(other.m_hour)
                && m_minute.Equals(other.m_minute)
                && m_second.Equals(other.m_second)
                && m_millisecond.Equals(other.m_millisecond);
        }

        public static explicit operator DateTime(SystemTime t)
        {
            return t.ToDateTime();
        }

        public static explicit operator SystemTime(DateTime t)
        {
            return new SystemTime(t);
        }
    }
}
