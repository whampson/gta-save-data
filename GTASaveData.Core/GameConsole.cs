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

        public static bool operator ==(GameConsole left, GameConsole right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GameConsole left, GameConsole right)
        {
            return !left.Equals(right);
        }
    }
}
