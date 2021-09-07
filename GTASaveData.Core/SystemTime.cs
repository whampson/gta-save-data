using System;
using System.Runtime.InteropServices;

namespace GTASaveData
{
    /// <summary>
    /// A .NET implementation of the Win32 <c>SYSTEMTIME</c> structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 16)]
    public struct SystemTime : IEquatable<SystemTime>
    {
        public static readonly SystemTime MinValue = new SystemTime(1601, 1, 1);
        public static readonly SystemTime MaxValue = new SystemTime(30827, 12, 31, 23, 59, 59, 999);

        [MarshalAs(UnmanagedType.U2)] public ushort Year;
        [MarshalAs(UnmanagedType.U2)] public ushort Month;
        [MarshalAs(UnmanagedType.U2)] public ushort DayOfWeek;
        [MarshalAs(UnmanagedType.U2)] public ushort Day;
        [MarshalAs(UnmanagedType.U2)] public ushort Hour;
        [MarshalAs(UnmanagedType.U2)] public ushort Minute;
        [MarshalAs(UnmanagedType.U2)] public ushort Second;
        [MarshalAs(UnmanagedType.U2)] public ushort Milliseconds;

        public SystemTime(ushort year, ushort month, ushort day, ushort hour = 0, ushort minute = 0, ushort second = 0, ushort millisecond = 0)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            Milliseconds = millisecond;
            DayOfWeek = 0;
        }

        public SystemTime(DateTime dt)
        {
            Year = Convert.ToUInt16(dt.Year);
            Month = Convert.ToUInt16(dt.Month);
            DayOfWeek = Convert.ToUInt16(dt.DayOfWeek);
            Day = Convert.ToUInt16(dt.Day);
            Hour = Convert.ToUInt16(dt.Hour);
            Minute = Convert.ToUInt16(dt.Minute);
            Second = Convert.ToUInt16(dt.Second);
            Milliseconds = Convert.ToUInt16(dt.Millisecond);
        }

        public override string ToString()
        {
            return $"{Month:D2}/{Day:D2}/{Year:D4} {Hour:D2}:{Minute:D2}:{Second:D2}.{Milliseconds:D3}";
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
            hash += 23 * Milliseconds.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SystemTime))
            {
                return false;
            }

            return Equals((SystemTime) obj);
        }

        public bool Equals(SystemTime other)
        {
            return Year.Equals(other.Year)
                && Month.Equals(other.Month)
                && Day.Equals(other.Day)
                && Hour.Equals(other.Hour)
                && Minute.Equals(other.Minute)
                && Second.Equals(other.Second)
                && Milliseconds.Equals(other.Milliseconds);
        }

        public static bool operator ==(SystemTime s1, SystemTime s2)
        {
            return s1.Equals(s2);
        }

        public static bool operator !=(SystemTime s1, SystemTime s2)
        {
            return !s1.Equals(s2);
        }

        public static explicit operator DateTime(SystemTime st)
        {
            if (st.Year == 0 || st == MinValue) return DateTime.MinValue;
            if (st == MaxValue) return DateTime.MaxValue;

            return new DateTime(st.Year, st.Month, st.Day, st.Hour, st.Minute, st.Second, st.Milliseconds, DateTimeKind.Local);
        }
    }
}
