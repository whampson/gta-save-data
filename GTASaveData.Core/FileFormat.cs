using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// Represents a GTA savedata file format using a <see cref="GameConsole"/>
    /// and <see cref="FileFormatFlags"/> combination.
    /// </summary>
    public struct FileFormat : IEquatable<FileFormat>
    {
        public static readonly FileFormat Default = new FileFormat("", "", "");

        private readonly string m_id;
        private readonly string m_name;
        private readonly string m_description;
        private readonly FileFormatFlags m_flags;
        private readonly IEnumerable<GameConsole> m_supportedConsoles;

        public string Id => m_id ?? "";
        public string Name => m_name ?? "";
        public string Description => m_description ?? "";
        public FileFormatFlags Flags => m_flags;
        public IEnumerable<GameConsole> SupportedConsoles => m_supportedConsoles ?? new List<GameConsole>();

        [JsonIgnore]
        public bool IsAndroid => IsSupportedOn(GameConsole.Android);

        [JsonIgnore]
        public bool IsiOS => IsSupportedOn(GameConsole.iOS);

        [JsonIgnore]
        public bool IsMobile => IsAndroid || IsiOS;

        [JsonIgnore]
        public bool IsPS2 => IsSupportedOn(GameConsole.PS2);

        [JsonIgnore]
        public bool IsPS3 => IsSupportedOn(GameConsole.PS3);

        [JsonIgnore]
        public bool IsPSP => IsSupportedOn(GameConsole.PSP);

        [JsonIgnore]
        public bool IsXbox => IsSupportedOn(GameConsole.Xbox);

        [JsonIgnore]
        public bool IsXbox360 => IsSupportedOn(GameConsole.Xbox360);

        [JsonIgnore]
        public bool IsWin32 => IsSupportedOn(GameConsole.Win32);

        [JsonIgnore]
        public bool IsMacOS => IsSupportedOn(GameConsole.MacOS);

        [JsonIgnore]
        public bool IsPC => IsMacOS || IsWin32;

        [JsonIgnore]
        public bool IsSteam => HasFlag(FileFormatFlags.Steam);

        [JsonIgnore]
        public bool IsAustralian => HasFlag(FileFormatFlags.Australian);

        [JsonIgnore]
        public bool IsJapanese => HasFlag(FileFormatFlags.Japanese);

        public FileFormat(string name, GameConsole console)
            : this(name, name, name, FileFormatFlags.None, console)
        { }

        public FileFormat(string name, FileFormatFlags flags, GameConsole console)
            : this(name, name, name, flags, console)
        { }

        public FileFormat(string id, string name, string description,
            params GameConsole[] supportedConsoles)
            : this(id, name, description, FileFormatFlags.None, supportedConsoles)
        { }

        public FileFormat(string id, string name, string description, FileFormatFlags flags,
            params GameConsole[] supportedConsoles)
        {
            m_id = id;
            m_name = name;
            m_description = description;
            m_flags = flags;
            m_supportedConsoles = new List<GameConsole>(supportedConsoles);
        }

        public bool IsSupportedOn(GameConsole c)
        {
            return SupportedConsoles.Contains(c);
        }

        public bool HasFlag(FileFormatFlags f)
        {
            return Flags.HasFlag(f);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (GameConsole c in SupportedConsoles)
            {
                hash += 23 * c.GetHashCode();
            }
            hash += 23 * Flags.GetHashCode();

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
            // Deliberately ignoring Id/Name/Description
            return SupportedConsoles.SequenceEqual(other.SupportedConsoles)
                && Flags.Equals(other.Flags);
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

    /// <summary>
    /// Flags representing variations on file formats for a given console.
    /// </summary>
    [Flags]
    public enum FileFormatFlags
    {
        None = 0,
        Steam = 1 << 0,
        Japanese = 1 << 1,
        Australian = 1 << 2
    }

    /// <summary>
    /// Game consoles that support GTA games.
    /// </summary>
    public enum GameConsole
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
}
