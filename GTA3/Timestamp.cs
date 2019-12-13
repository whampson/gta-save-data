using GTASaveData.Common;
using System;

namespace GTASaveData.GTA3
{
    public sealed class Timestamp : GTAObject,
        IEquatable<Timestamp>
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

        private Timestamp(Serializer serializer)
        {
            m_second = serializer.ReadInt32();
            m_minute = serializer.ReadInt32();
            m_hour = serializer.ReadInt32();
            m_day = serializer.ReadInt32();
            m_month = serializer.ReadInt32();
            m_year = serializer.ReadInt32();
        }

        private void Serialize(Serializer serializer)
        {
            serializer.Write(m_second);
            serializer.Write(m_minute);
            serializer.Write(m_hour);
            serializer.Write(m_day);
            serializer.Write(m_month);
            serializer.Write(m_year);
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

        public override string ToString()
        {
            return ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
