using System;

namespace GTASaveData.Types
{
    /// <summary>
    /// A .NET version of the Win32 <c>SYSTEMTIME</c> structure, which is stored in some GTA save files.
    /// </summary>
    public struct SystemTime : ISerializable, IEquatable<SystemTime>
    {
        private const int Size = 16;

        public short Year;
        public short Month;
        public short DayOfWeek;
        public short Day;
        public short Hour;
        public short Minute;
        public short Second;
        public short Millisecond;

        public SystemTime(short year, short month, short dayOfWeek, short day, short hour, short minute, short second, short millisecond)
        {
            Millisecond = millisecond;
            Second = second;
            Minute = minute;
            Hour = hour;
            Day = day;
            DayOfWeek = dayOfWeek;
            Month = month;
            Year = year;
        }

        public SystemTime(DateTime dateTime)
        {
            Year = (short) dateTime.Year;
            Month = (short) dateTime.Month;
            DayOfWeek = (short) dateTime.DayOfWeek;
            Day = (short) dateTime.Day;
            Hour = (short) dateTime.Hour;
            Minute = (short) dateTime.Minute;
            Second = (short) dateTime.Second;
            Millisecond = (short) dateTime.Millisecond;
        }

        int ISerializable.ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            Year = buf.ReadInt16();
            Month = buf.ReadInt16();
            DayOfWeek = buf.ReadInt16();
            Day = buf.ReadInt16();
            Hour = buf.ReadInt16();
            Minute = buf.ReadInt16();
            Second = buf.ReadInt16();
            Millisecond = buf.ReadInt16();

            return Size;
        }

        int ISerializable.WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Year);
            buf.Write(Month);
            buf.Write(DayOfWeek);
            buf.Write(Day);
            buf.Write(Hour);
            buf.Write(Minute);
            buf.Write(Second);
            buf.Write(Millisecond);

            return Size;
        }

        int ISerializable.GetSize(DataFormat fmt)
        {
            return Size;
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

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * Year;
            hash += 23 * Month;
            hash += 23 * DayOfWeek;
            hash += 23 * Day;
            hash += 23 * Hour;
            hash += 23 * Minute;
            hash += 23 * Second;
            hash += 23 * Millisecond;

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
                && DayOfWeek.Equals(other.DayOfWeek)
                && Day.Equals(other.Day)
                && Hour.Equals(other.Hour)
                && Minute.Equals(other.Minute)
                && Second.Equals(other.Second)
                && Millisecond.Equals(other.Millisecond);
        }

        public override string ToString()
        {
            return ToDateTime().ToString("dd MMM yyyy HH:mm:ss");
        }

        public static bool operator ==(SystemTime d1, SystemTime d2)
        {
            return d1.Equals(d2);
        }

        public static bool operator !=(SystemTime d1, SystemTime d2)
        {
            return !d1.Equals(d2);
        }

        public static implicit operator DateTime(SystemTime t)
        {
            return t.ToDateTime();
        }

        public static explicit operator SystemTime(DateTime t)
        {
            return new SystemTime(t);
        }
    }
}
