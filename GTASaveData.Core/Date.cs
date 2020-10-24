using System;
using System.Runtime.InteropServices;

namespace GTASaveData
{
    /// <summary>
    /// Date structure used by the GTA games.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 24)]
    public struct Date : IEquatable<Date>
    {
        public static readonly Date MinValue = new Date(1601, 1, 1);
        public static readonly Date MaxValue = new Date(30827, 12, 31, 23, 59, 59);

        public int Second;
        public int Minute;
        public int Hour;
        public int Day;
        public int Month;
        public int Year;

        public Date(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public Date(DateTime dt)
        {
            dt = dt.ToUniversalTime();
            Year = dt.Year;
            Month = dt.Month;
            Day = dt.Day;
            Hour = dt.Hour;
            Minute = dt.Minute;
            Second = dt.Second;
        }

        public override string ToString()
        {
            return $"{Month:D2}/{Day:D2}/{Year:D4} {Hour:D2}:{Minute:D2}:{Second:D2}";
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * Year.GetHashCode();
            hash += 23 * Month.GetHashCode();
            hash += 23 * Day.GetHashCode();
            hash += 23 * Hour.GetHashCode();
            hash += 23 * Minute.GetHashCode();
            hash += 23 * Second.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Date))
            {
                return false;
            }

            return Equals((Date) obj);
        }

        public bool Equals(Date other)
        {
            return Year.Equals(other.Year)
                && Month.Equals(other.Month)
                && Day.Equals(other.Day)
                && Hour.Equals(other.Hour)
                && Minute.Equals(other.Minute)
                && Second.Equals(other.Second);
        }

        public static bool operator ==(Date s1, Date s2)
        {
            return s1.Equals(s2);
        }

        public static bool operator !=(Date s1, Date s2)
        {
            return !s1.Equals(s2);
        }

        public static implicit operator DateTime(Date st)
        {
            if (st.Year == 0 || st == MinValue) return DateTime.MinValue;
            if (st == MaxValue) return DateTime.MaxValue;

            return new DateTime(st.Year, st.Month, st.Day, st.Hour, st.Minute, st.Second, DateTimeKind.Local);
        }
    }
}
