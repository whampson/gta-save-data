using System;
using System.Diagnostics;

namespace GTASaveData.Types
{
    /// <summary>
    /// Represents a date and time.
    /// </summary>
    [Size(24)]
    public class Date : SaveDataObject, IEquatable<Date>
    {
        private int m_second;
        private int m_minute;
        private int m_hour;
        private int m_day;
        private int m_month;
        private int m_year;

        public int Year
        {
            get { return m_year; }
            set { m_year = value; OnPropertyChanged(); }
        }

        public int Month
        {
            get { return m_month; }
            set { m_month = value; OnPropertyChanged(); }
        }

        public int Day
        {
            get { return m_day; }
            set { m_day = value; OnPropertyChanged(); }
        }

        public int Hour
        {
            get { return m_hour; }
            set { m_hour = value; OnPropertyChanged(); }
        }

        public int Minute
        {
            get { return m_minute; }
            set { m_minute = value; OnPropertyChanged(); }
        }

        public int Second
        {
            get { return m_second; }
            set { m_second = value; OnPropertyChanged(); }
        }

        public Date()
            : this(DateTime.Now)
        { }

        public Date(DateTime dateTime)
        {
            Second = dateTime.Second;
            Minute = dateTime.Minute;
            Hour = dateTime.Hour;
            Day = dateTime.Day;
            Month = dateTime.Month;
            Year = dateTime.Year;
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            Second = buf.ReadInt32();
            Minute = buf.ReadInt32();
            Hour = buf.ReadInt32();
            Day = buf.ReadInt32();
            Month = buf.ReadInt32();
            Year = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<Date>());
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(Second);
            buf.Write(Minute);
            buf.Write(Hour);
            buf.Write(Day);
            buf.Write(Month);
            buf.Write(Year);

            Debug.Assert(buf.Offset == SizeOf<Date>());
        }

        public DateTime ToDateTime()
        {
            return new DateTime(
                Year,
                Month,
                Day,
                Hour,
                Minute,
                Second);
        }

        public override string ToString()
        {
            return ToDateTime().ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Date);
        }

        public bool Equals(Date other)
        {
            if (other == null)
            {
                return false;
            }

            return Year.Equals(other.Year)
                && Month.Equals(other.Month)
                && Day.Equals(other.Day)
                && Hour.Equals(other.Hour)
                && Minute.Equals(other.Minute)
                && Second.Equals(other.Second);
        }

        public static implicit operator DateTime(Date t)
        {
            return t.ToDateTime();
        }

        public static explicit operator Date(DateTime t)
        {
            return new Date(t);
        }
    }
}
