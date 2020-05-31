using System;

namespace GTASaveData
{
    /// <summary>
    /// Represents a videogame console.
    /// </summary>
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
            return $"{GetType().Name}: {{ Type = {Type}, Flags = {Flags} }}";
        }

        public static bool operator ==(GameConsole left, GameConsole right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GameConsole left, GameConsole right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// Game consoles that support GTA games.
    /// </summary>
    public enum ConsoleType
    {
        None,
        Android,
        iOS,
        PS2,
        PS3,
        PSP,
        Xbox,
        Xbox360,
        Win32,      // I know, I know, PCs aren't consoles...
        MacOS,
    }

    /// <summary>
    /// Various flags that can distinguish different versions of
    /// the same game system.
    /// </summary>
    [Flags]
    public enum ConsoleFlags
    {
        None = 0,
        NorthAmerica = (1 << 0),
        Europe = (1 << 1),
        Japan = (1 << 2),
        Australia = (1 << 3),
        Steam = (1 << 4),
    }
}
