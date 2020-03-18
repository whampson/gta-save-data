using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData
{
    public struct SaveFileFormat : IEquatable<SaveFileFormat>
    {
        public static readonly SaveFileFormat Default = new SaveFileFormat(null, null);

        public string Name { get; }
        public string Description { get; }
        public IEnumerable<GameConsole> SupportedConsoles { get; }

        public bool SupportedOnAndroid => IsSupportedOn(ConsoleType.Android);
        public bool SupportedOniOS => IsSupportedOn(ConsoleType.iOS);
        public bool SupportedOnMacOS => IsSupportedOn(ConsoleType.MacOS);
        public bool SupportedOnMobile => SupportedOnAndroid || SupportedOniOS;
        public bool SupportsPC => SupportedOnMacOS || SupportsWin32;
        public bool SupportsPS2 => IsSupportedOn(ConsoleType.PS2);
        public bool SupportsPS3 => IsSupportedOn(ConsoleType.PS3);
        public bool SupportsPSP => IsSupportedOn(ConsoleType.PSP);
        public bool SupportsWin32 => IsSupportedOn(ConsoleType.Win32);
        public bool SupportsXbox => IsSupportedOn(ConsoleType.Xbox);
        public bool SupportsXbox360 => IsSupportedOn(ConsoleType.Xbox360);

        public SaveFileFormat(string name, string description, params GameConsole[] supportedConsoles)
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
            return Equals((SaveFileFormat) obj);
        }

        public bool Equals(SaveFileFormat other)
        {
            return SupportedConsoles.SequenceEqual(other.SupportedConsoles);
        }

        public override string ToString()
        {
            return Description ?? Name ?? string.Empty;
        }

        public static bool operator ==(SaveFileFormat left, SaveFileFormat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SaveFileFormat left, SaveFileFormat right)
        {
            return !left.Equals(right);
        }
    }
}
