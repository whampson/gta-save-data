using GTASaveData.Types;
using System;

namespace GTASaveData.GTA3
{
    public class Clock : GTAObject,
        IEquatable<Clock>
    {
        private int m_millisecondsPerGameMinute;
        private uint m_lastClockTick;
        private int m_gameClockHours;
        private int m_gameClockMinutes;

        public int MillisecondsPerGameMinute
        {
            get { return m_millisecondsPerGameMinute; }
            set { m_millisecondsPerGameMinute = value; OnPropertyChanged(); }
        }

        public uint LastClockTick
        {
            get { return m_lastClockTick; }
            set { m_lastClockTick = value; OnPropertyChanged(); }
        }

        public int GameClockHours
        {
            get { return m_gameClockHours; }
            set { m_gameClockHours = value; OnPropertyChanged(); }
        }

        public int GameClockMinutes
        {
            get { return m_gameClockMinutes; }
            set { m_gameClockMinutes = value; OnPropertyChanged(); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Clock);
        }

        public bool Equals(Clock other)
        {
            if (other == null)
            {
                return false;
            }

            return m_millisecondsPerGameMinute.Equals(other.m_millisecondsPerGameMinute)
                && m_lastClockTick.Equals(other.m_lastClockTick)
                && m_gameClockHours.Equals(other.m_gameClockHours)
                && m_gameClockMinutes.Equals(other.m_gameClockMinutes);
        }
    }
}
