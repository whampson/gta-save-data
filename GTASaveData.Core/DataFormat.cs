using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData
{
    public struct DataFormat : IEquatable<DataFormat>
    {
        public static readonly DataFormat Default = new DataFormat("Default", "", "");

        private readonly string m_id;
        private readonly string m_name;
        private readonly string m_description;
        private readonly IEnumerable<GameConsole> m_supportedConsoles;

        public string Id => m_id ?? "";
        public string Name => m_name ?? "";
        public string Description => m_description ?? "";
        public IEnumerable<GameConsole> SupportedConsoles => m_supportedConsoles ?? new List<GameConsole>();

        public bool IsSupportedOnAndroid => IsSupportedOn(ConsoleType.Android);
        public bool IsSupportedOniOS => IsSupportedOn(ConsoleType.iOS);
        public bool IsSupportedOnMacOS => IsSupportedOn(ConsoleType.MacOS);
        public bool IsSupportedOnMobile => IsSupportedOnAndroid || IsSupportedOniOS;
        public bool IsSupportedOnPC => IsSupportedOnMacOS || IsSupportedOnWin32;
        public bool IsSupportedOnPS2 => IsSupportedOn(ConsoleType.PS2);
        public bool IsSupportedOnPS3 => IsSupportedOn(ConsoleType.PS3);
        public bool IsSupportedOnPSP => IsSupportedOn(ConsoleType.PSP);
        public bool IsSupportedOnWin32 => IsSupportedOn(ConsoleType.Win32);
        public bool IsSupportedOnXbox => IsSupportedOn(ConsoleType.Xbox);
        public bool IsSupportedOnXbox360 => IsSupportedOn(ConsoleType.Xbox360);

        public DataFormat(string id, string name, string description, params GameConsole[] supportedConsoles)
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
            if (!(obj is DataFormat))
            {
                return false;
            }

            return Equals((DataFormat) obj);
        }

        public bool Equals(DataFormat other)
        {
            return SupportedConsoles.SequenceEqual(other.SupportedConsoles);
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(DataFormat left, DataFormat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DataFormat left, DataFormat right)
        {
            return !left.Equals(right);
        }
    }
}
