using System;
using System.Diagnostics;

namespace GTASaveData.Types
{
    /// <summary>
    /// A .NET version of the Win32 <c>SYSTEMTIME</c> structure.
    /// </summary>
    [Size(16)]
    public class SystemTime : SaveDataObject, IEquatable<SystemTime>
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
            : this(DateTime.Now)
        { }

        public SystemTime(DateTime dateTime)
        {
            Year = (ushort) dateTime.Year;
            Month = (ushort) dateTime.Month;
            DayOfWeek = (ushort) dateTime.DayOfWeek;
            Day = (ushort) dateTime.Day;
            Hour = (ushort) dateTime.Hour;
            Minute = (ushort) dateTime.Minute;
            Second = (ushort) dateTime.Second;
            Millisecond = (ushort) dateTime.Millisecond;
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            Year = buf.ReadUInt16();
            Month = buf.ReadUInt16();
            DayOfWeek = buf.ReadUInt16();
            Day = buf.ReadUInt16();
            Hour = buf.ReadUInt16();
            Minute = buf.ReadUInt16();
            Second = buf.ReadUInt16();
            Millisecond = buf.ReadUInt16();

            Debug.Assert(buf.Offset == SizeOf<SystemTime>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(Year);
            buf.Write(Month);
            buf.Write(DayOfWeek);
            buf.Write(Day);
            buf.Write(Hour);
            buf.Write(Minute);
            buf.Write(Second);
            buf.Write(Millisecond);

            Debug.Assert(buf.Offset == SizeOf<SystemTime>());
        }

        public DateTime ToDateTime()
        {
            return new DateTime(
                Year,
                Month,
                Day,
                Hour,
                Minute,
                Second,
                Millisecond);
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

            return Year.Equals(other.Year)
                && Month.Equals(other.Month)
                && DayOfWeek.Equals(other.DayOfWeek)
                && Day.Equals(other.Day)
                && Hour.Equals(other.Hour)
                && Minute.Equals(other.Minute)
                && Second.Equals(other.Second)
                && Millisecond.Equals(other.Millisecond);
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
