using System;
using System.Diagnostics;

namespace GTASaveData.Types
{
    /// <summary>
    /// Represents a date and time.
    /// </summary>
    [Size(0x18)]
    public class Date : GTAObject,
        IEquatable<Date>
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
        { }

        public Date(DateTime dateTime)
        {
            m_second = dateTime.Second;
            m_minute = dateTime.Minute;
            m_hour = dateTime.Hour;
            m_day = dateTime.Day;
            m_month = dateTime.Month;
            m_year = dateTime.Year;
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            m_second = buf.ReadInt32();
            m_minute = buf.ReadInt32();
            m_hour = buf.ReadInt32();
            m_day = buf.ReadInt32();
            m_month = buf.ReadInt32();
            m_year = buf.ReadInt32();

            Debug.Assert(buf.Offset == SizeOf<Date>());
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(m_second);
            buf.Write(m_minute);
            buf.Write(m_hour);
            buf.Write(m_day);
            buf.Write(m_month);
            buf.Write(m_year);

            Debug.Assert(buf.Offset == SizeOf<Date>());
        }

        public DateTime ToDateTime()
        {
            return new DateTime(
                m_year,
                m_month,
                m_day,
                m_hour,
                m_minute,
                m_second);
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

            return m_year.Equals(other.m_year)
                && m_month.Equals(other.m_month)
                && m_day.Equals(other.m_day)
                && m_hour.Equals(other.m_hour)
                && m_minute.Equals(other.m_minute)
                && m_second.Equals(other.m_second);
        }

        public static explicit operator DateTime(Date t)
        {
            return t.ToDateTime();
        }

        public static explicit operator Date(DateTime t)
        {
            return new Date(t);
        }
    }
}
