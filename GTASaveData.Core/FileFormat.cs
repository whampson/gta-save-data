using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData
{
    public struct FileFormat : IEquatable<FileFormat>
    {
        public static readonly FileFormat Default = new FileFormat("", "", "");

        private readonly string m_id;
        private readonly string m_name;
        private readonly string m_description;
        private readonly IEnumerable<GameConsole> m_supportedConsoles;

        public string Id => m_id ?? "";
        public string Name => m_name ?? "";
        public string Description => m_description ?? "";
        public IEnumerable<GameConsole> SupportedConsoles => m_supportedConsoles ?? new List<GameConsole>();
        public bool IsAndroid => IsSupportedOn(ConsoleType.Android);
        public bool IsiOS => IsSupportedOn(ConsoleType.iOS);
        public bool IsMacOS => IsSupportedOn(ConsoleType.MacOS);
        public bool IsMobile => IsAndroid || IsiOS;
        public bool IsPC => IsMacOS || IsWin32;
        public bool IsPS2 => IsSupportedOn(ConsoleType.PS2);
        public bool IsPS3 => IsSupportedOn(ConsoleType.PS3);
        public bool IsPSP => IsSupportedOn(ConsoleType.PSP);
        public bool IsWin32 => IsSupportedOn(ConsoleType.Win32);
        public bool IsXbox => IsSupportedOn(ConsoleType.Xbox);
        public bool IsXbox360 => IsSupportedOn(ConsoleType.Xbox360);

        public FileFormat(string id, string name, string description, params GameConsole[] supportedConsoles)
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
            if (!(obj is FileFormat))
            {
                return false;
            }

            return Equals((FileFormat) obj);
        }

        public bool Equals(FileFormat other)
        {
            return SupportedConsoles.SequenceEqual(other.SupportedConsoles);
        }

        public override string ToString()
        {
            return Name;
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
