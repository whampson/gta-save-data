using GTASaveData.Types.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GTASaveData.Types
{
    /// <summary>
    /// Represents a date and time.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 24)]
    public struct Date : ISaveDataObject, IEquatable<Date>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const int Size = 24;

        public int Second { get; set; }
        public int Minute { get; set; }
        public int Hour { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public Date(int year, int month, int day, int hour, int minute, int second)
        {
            Second = second;
            Minute = minute;
            Hour = hour;
            Day = day;
            Month = month;
            Year = year;
            PropertyChanged = null;
        }

        public Date(DateTime dateTime)
        {
            Second = dateTime.Second;
            Minute = dateTime.Minute;
            Hour = dateTime.Hour;
            Day = dateTime.Day;
            Month = dateTime.Month;
            Year = dateTime.Year;
            PropertyChanged = null;
        }

        int ISaveDataObject.ReadData(StreamBuffer buf, FileFormat fmt)
        {
            Second = buf.ReadInt32();
            Minute = buf.ReadInt32();
            Hour = buf.ReadInt32();
            Day = buf.ReadInt32();
            Month = buf.ReadInt32();
            Year = buf.ReadInt32();

            return Size;
        }

        int ISaveDataObject.WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(Second);
            buf.Write(Minute);
            buf.Write(Hour);
            buf.Write(Day);
            buf.Write(Month);
            buf.Write(Year);

            return Size;
        }

        int ISaveDataObject.GetSize(FileFormat fmt)
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
                Second);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * Year;
            hash += 23 * Month;
            hash += 23 * Day;
            hash += 23 * Hour;
            hash += 23 * Minute;
            hash += 23 * Second;

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

        public override string ToString()
        {
            return ToDateTime().ToString("dd MMM yyyy HH:mm:ss");
        }

        public static bool operator ==(Date d1, Date d2)
        {
            return d1.Equals(d2);
        }

        public static bool operator !=(Date d1, Date d2)
        {
            return !d1.Equals(d2);
        }

        public static explicit operator DateTime(Date t)
        {
            return t.ToDateTime();
        }

        public static implicit operator Date(DateTime t)
        {
            return new Date(t);
        }
    }
}
