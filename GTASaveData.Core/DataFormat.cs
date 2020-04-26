using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable IDE1006 // Naming Styles
namespace GTASaveData
{
    public struct DataFormat : IEquatable<DataFormat>
    {
        public static readonly DataFormat Default = new DataFormat("Default", "", "");

        private readonly string m_id;
        private readonly string m_name;
        private readonly string m_description;
        private readonly IEnumerable<GameConsole> m_supportedConsoles;

        public string FormatId => m_id ?? "";
        public string FormatName => m_name ?? "";
        public string FormatDescription => m_description ?? "";
        public IEnumerable<GameConsole> SupportedConsoles => m_supportedConsoles ?? new List<GameConsole>();
        public bool Android => IsSupportedOn(ConsoleType.Android);
        public bool iOS => IsSupportedOn(ConsoleType.iOS);
        public bool MacOS => IsSupportedOn(ConsoleType.MacOS);
        public bool Mobile => Android || iOS;
        public bool PC => MacOS || Win32;
        public bool PS2 => IsSupportedOn(ConsoleType.PS2);
        public bool PS3 => IsSupportedOn(ConsoleType.PS3);
        public bool PSP => IsSupportedOn(ConsoleType.PSP);
        public bool Win32 => IsSupportedOn(ConsoleType.Win32);
        public bool Xbox => IsSupportedOn(ConsoleType.Xbox);
        public bool Xbox360 => IsSupportedOn(ConsoleType.Xbox360);

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
            return FormatName;
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
#pragma warning restore IDE1006 // Naming Styles
