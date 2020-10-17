using GTASaveData.Types.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GTASaveData.Types
{
    /// <summary>
    /// A .NET version of the Win32 <c>SYSTEMTIME</c> structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct SystemTime : ISaveDataObject, IEquatable<SystemTime>
    {
        private const int Size = 16;

        public short Year { get; set; }
        public short Month { get; set; }
        public short DayOfWeek { get; set; }
        public short Day { get; set; }
        public short Hour { get; set; }
        public short Minute { get; set; }
        public short Second { get; set; }
        public short Millisecond { get; set; }

        public SystemTime(short year, short month, short dayOfWeek, short day,
            short hour, short minute, short second, short millisecond)
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

        int ISaveDataObject.ReadData(DataBuffer buf, FileFormat fmt)
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

        int ISaveDataObject.WriteData(DataBuffer buf, FileFormat fmt)
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

        int ISaveDataObject.GetSize(FileFormat fmt)
        {
            return Size;
        }

        public bool IsValidDateTime(out DateTime dt)
        {
            return DateTime.TryParseExact(
                $"{Year:04}-{Month:02}-{Day:02} {Hour:02}:{Minute:02}:{Second:02}.{Millisecond}",
                "yyyy-MM-dd HH:mm:ss.fff",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dt);
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
            if (IsValidDateTime(out DateTime dt))
            {
                return dt.ToString("dd MMM yyyy HH:mm:ss.fff");
            }

            return "(invalid date)";
        }

        public static bool operator ==(SystemTime d1, SystemTime d2)
        {
            return d1.Equals(d2);
        }

        public static bool operator !=(SystemTime d1, SystemTime d2)
        {
            return !d1.Equals(d2);
        }

        public static explicit operator DateTime(SystemTime t)
        {
            if (t.IsValidDateTime(out DateTime dt))
            {
                return dt;
            }

            return DateTime.MinValue;
        }

        public static implicit operator SystemTime(DateTime t)
        {
            return new SystemTime(t);
        }
    }
}
