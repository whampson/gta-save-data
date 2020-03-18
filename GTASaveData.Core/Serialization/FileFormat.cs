using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData.Serialization
{
    /// <summary>
    /// Represents a standard way that a <see cref="GTASave"/> file can be encoded.
    /// </summary>
    public struct FileFormat : IEquatable<FileFormat>
    {
        /// <summary>
        /// Represents an ambiguous or irrelevant file format.
        /// </summary>
        public static readonly FileFormat None = new FileFormat(null, null);

        public string Name { get; }
        public string Description { get; }
        public IEnumerable<GameConsole> SupportedConsoles { get; }

        public bool SupportsAndroid => IsSupportedOn(ConsoleType.Android);

        public bool SupportsIOS => IsSupportedOn(ConsoleType.iOS);

        public bool SupportsMacOS => IsSupportedOn(ConsoleType.MacOS);

        public bool SupportsMobile => SupportsAndroid || SupportsIOS;

        public bool SupportsPC => SupportsMacOS || SupportsWin32;

        public bool SupportsPS2 => IsSupportedOn(ConsoleType.PS2);

        public bool SupportsPS3 => IsSupportedOn(ConsoleType.PS3);

        public bool SupportsPSP => IsSupportedOn(ConsoleType.PSP);

        public bool SupportsWin32 => IsSupportedOn(ConsoleType.Win32);

        public bool SupportsXbox => IsSupportedOn(ConsoleType.Xbox);

        public bool SupportsXbox360 => IsSupportedOn(ConsoleType.Xbox360);

        public FileFormat(string name, string description, params GameConsole[] supportedConsoles)
        {
            Name = name;
            Description = description;
            SupportedConsoles = new List<GameConsole>(supportedConsoles);
        }

        public bool IsSupportedOn(GameConsole c)
        {
            return SupportedConsoles.Contains(c);
        }

        public bool IsSupportedOn(ConsoleType c, ConsoleFlags flags = ConsoleFlags.None)
        {
            return SupportedConsoles.Any(x => x.Type == c && x.Flags.HasFlag(flags));
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash += 23 * Name.GetHashCode();
            hash += 23 * Description.GetHashCode();
            foreach (GameConsole c in SupportedConsoles)
            {
                hash += 23 * c.GetHashCode();
            }

            return hash;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Equals((FileFormat) obj);
        }

        public bool Equals(FileFormat other)
        {
            return Name.Equals(other.Name)
                && Description.Equals(other.Description)
                && SupportedConsoles.SequenceEqual(other.SupportedConsoles);
        }

        public override string ToString()
        {
            return Description ?? Name ?? string.Empty;
        }

        public static bool operator ==(FileFormat left, FileFormat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FileFormat left, FileFormat right)
        {
            return !left.Equals(right);
        }
    }
}
