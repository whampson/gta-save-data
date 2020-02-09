using GTASaveData.Serialization;
using System;

namespace GTASaveData.Common
{
    public class Timestamp : Chunk, IEquatable<Timestamp>
    {
        private int m_year;
        private int m_month;
        private int m_day;
        private int m_hour;
        private int m_minute;
        private int m_second;

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

        public Timestamp()
        { }

        public Timestamp(DateTime dateTime)
        {
            m_second = dateTime.Second;
            m_minute = dateTime.Minute;
            m_hour = dateTime.Hour;
            m_day = dateTime.Day;
            m_month = dateTime.Month;
            m_year = dateTime.Year;
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            m_second = r.ReadInt32();
            m_minute = r.ReadInt32();
            m_hour = r.ReadInt32();
            m_day = r.ReadInt32();
            m_month = r.ReadInt32();
            m_year = r.ReadInt32();
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_second);
            w.Write(m_minute);
            w.Write(m_hour);
            w.Write(m_day);
            w.Write(m_month);
            w.Write(m_year);
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

        public override bool Equals(object obj)
        {
            return Equals(obj as Timestamp);
        }

        public bool Equals(Timestamp other)
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
    }
}
