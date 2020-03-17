using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData.Serialization
{
    /// <summary>
    /// Represents a standard way that a <see cref="GrandTheftAutoSave"/> file can be encoded.
    /// </summary>
    public sealed class FileFormat : IEquatable<FileFormat>
    {
        /// <summary>
        /// Represents an ambiguous or irrelevant file format.
        /// </summary>
        public static readonly FileFormat None = new FileFormat(null, null);

        public FileFormat(string name, string description, params GameConsole[] supportedConsoles)
        {
            Name = name;
            Description = description;
            SupportedConsoles = new List<GameConsole>(supportedConsoles);
        }

        public string Name { get; }
        public string Description { get; }
        public IEnumerable<GameConsole> SupportedConsoles { get; }

        public bool SupportsAndroid => IsSupported(ConsoleType.Android);

        public bool SupportsIOS => IsSupported(ConsoleType.iOS);

        public bool SupportsMacOS => IsSupported(ConsoleType.MacOS);

        public bool SupportsMobile => SupportsAndroid || SupportsIOS;

        public bool SupportsPC => SupportsMacOS || SupportsWin32;

        public bool SupportsPS2 => IsSupported(ConsoleType.PS2);

        public bool SupportsPS3 => IsSupported(ConsoleType.PS3);

        public bool SupportsPSP => IsSupported(ConsoleType.PSP);

        public bool SupportsWin32 => IsSupported(ConsoleType.Win32);

        public bool SupportsXbox => IsSupported(ConsoleType.Xbox);

        public bool SupportsXbox360 => IsSupported(ConsoleType.Xbox360);

        public bool IsSupported(GameConsole c)
        {
            return SupportedConsoles.Contains(c);
        }

        public bool IsSupported(ConsoleType c, ConsoleFlags flags = ConsoleFlags.None)
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
            return Equals(obj as FileFormat);
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
    }
}
