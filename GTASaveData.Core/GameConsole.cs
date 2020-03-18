using System;

namespace GTASaveData
{
    public struct GameConsole : IEquatable<GameConsole>
    {
        public ConsoleType Type { get; }
        public ConsoleFlags Flags { get; }

        public GameConsole(ConsoleType type)
            : this(type, ConsoleFlags.None)
        { }

        public GameConsole(ConsoleType type, ConsoleFlags flags)
        {
            Type = type;
            Flags = flags;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * Type.GetHashCode();
            hash += 23 * Flags.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Equals((GameConsole) obj);
        }

        public bool Equals(GameConsole other)
        {
            return Type.Equals(other.Type)
                && Flags.Equals(other.Flags);
        }

        public override string ToString()
        {
            return string.Format("GameConsole: {{ Type = {0}, Flags = {1} }}", Type, Flags);
        }
    }

    public enum ConsoleType
    {
        None,
        Android,
        iOS,
        MacOS,
        PS2,
        PS3,
        PSP,
        Win32,
        Xbox,
        Xbox360
    }

    [Flags]
    public enum ConsoleFlags
    {
        None = 0,
        NorthAmerica = 0b_0000_0001,
        Europe = 0b_0000_0010,
        Japan = 0b_0000_0100,
        Australia = 0b_0000_1000,
        Steam = 0b_0001_0000,
    }
}
