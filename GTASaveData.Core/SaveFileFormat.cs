using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData
{
    public struct SaveFileFormat : IEquatable<SaveFileFormat>
    {
        public static readonly SaveFileFormat Default = new SaveFileFormat("Default", "", "");

        private readonly string m_id;
        private readonly string m_name;
        private readonly string m_description;
        private readonly IEnumerable<GameConsole> m_supportedConsoles;

        public string Id => m_id ?? string.Empty;
        public string Name => m_name ?? string.Empty;
        public string Description => m_description ?? string.Empty;
        public IEnumerable<GameConsole> SupportedConsoles => m_supportedConsoles ?? new List<GameConsole>();

        public bool SupportedOnAndroid => IsSupportedOn(ConsoleType.Android);
        public bool SupportedOniOS => IsSupportedOn(ConsoleType.iOS);
        public bool SupportedOnMacOS => IsSupportedOn(ConsoleType.MacOS);
        public bool SupportedOnMobile => SupportedOnAndroid || SupportedOniOS;
        public bool SupportedOnPC => SupportedOnMacOS || SupportedOnWin32;
        public bool SupportedOnPS2 => IsSupportedOn(ConsoleType.PS2);
        public bool SupportedOnPS3 => IsSupportedOn(ConsoleType.PS3);
        public bool SupportedOnPSP => IsSupportedOn(ConsoleType.PSP);
        public bool SupportedOnWin32 => IsSupportedOn(ConsoleType.Win32);
        public bool SupportedOnXbox => IsSupportedOn(ConsoleType.Xbox);
        public bool SupportedOnXbox360 => IsSupportedOn(ConsoleType.Xbox360);

        public SaveFileFormat(string id, string name, string description, params GameConsole[] supportedConsoles)
        {
            m_id = id;
            m_name = name;
            m_description = description;
            m_supportedConsoles = new List<GameConsole>(supportedConsoles);
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
            if (!(obj is SaveFileFormat))
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
            return Name;
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
