using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// A <see cref="FileType"/> identifies the convention used to store data in a save file.
    /// It consists of three parts: a unique string identifier, a set of file type-specific flags,
    /// and a list of supported game versions.
    /// </summary>
    /// <remarks>
    /// Many games exhibit minor layout differences in the save files between platforms.
    /// For example, the data in GTA3's PS2 and PC save files are packaged differently in terms
    /// of how the blocks are written. A <see cref="FileType"/> is designed to disambiguate
    /// thesee differing data layouts. It does NOT reveal information about the game script version
    /// or other game differences.
    /// </remarks>
    public struct FileType : IEquatable<FileType>
    {
        /// <summary>
        /// Placeholder generic <see cref="FileType"/>.
        /// </summary>
        public static readonly FileType Default = new FileType("");

        private readonly string m_id;
        private readonly uint m_flags;
        private readonly List<GameVersionId> m_compatibleWith;

        /// <summary>
        /// A short identifier for this file type.
        /// </summary>
        /// <remarks>
        /// Example: <c>PC_STEAM</c>
        /// </remarks>
        public string Id => m_id ?? "";

        /// <summary>
        /// Flags that can be used to further delineate similar file types.
        /// </summary>
        public uint Flags => m_flags;

        /// <summary>
        /// Game versions compatible with this file type.
        /// </summary>
        public IEnumerable<GameVersionId> CompatibilityList => m_compatibleWith ?? new List<GameVersionId>();

        /// <summary>
        /// Creates a new <see cref="FileType"/>.
        /// </summary>
        /// <param name="id">A short identifier.</param>
        /// <param name="compatibleWith">A list of compatible <see cref="GameVersionId"/>s.</param>
        public FileType(string id, params GameVersionId[] compatibleWith)
            : this(id, 0, compatibleWith)
        { }

        /// <summary>
        /// Creates a new <see cref="FileType"/>.
        /// </summary>
        /// <param name="id">A short identifier.</param>
        /// <param name="flags">A set of bitflags that can be used to delineate similar file types.</param>
        /// <param name="compatibleWith">A list of compatible <see cref="GameVersionId"/>s.</param>
        public FileType(string id, uint flags, params GameVersionId[] compatibleWith)
        {
            m_id = id;
            m_flags = flags;
            m_compatibleWith = new List<GameVersionId>(compatibleWith);
        }

        /// <summary>
        /// Checks whether this file type is supported by a specific game version.
        /// </summary>
        public bool IsSupportedBy(GameVersionId g)
        {
            return CompatibilityList.Contains(g);
        }

        /// <summary>
        /// Checks wither this file type has a flag set.
        /// </summary>
        public bool HasFlag(uint flag)
        {
            return (Flags & flag) == flag;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (GameVersionId c in CompatibilityList)
            {
                hash += 23 * c.GetHashCode();
            }
            hash += 23 * Flags.GetHashCode();
            hash += 23 * Id.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            return obj is FileType type && Equals(type);
        }

        public bool Equals(FileType other)
        {
            return Id.Equals(Id)
                && Flags.Equals(other.Flags)
                && CompatibilityList.SequenceEqual(other.CompatibilityList);
        }

        public override string ToString()
        {
            return Id;
        }

        public static bool operator ==(FileType left, FileType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FileType left, FileType right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the Android OS.
        /// </summary>
        [JsonIgnore] public bool IsAndroid => IsSupportedBy(GameVersionId.Android);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on Apple iOS.
        /// </summary>
        [JsonIgnore] public bool IsiOS => IsSupportedBy(GameVersionId.iOS);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the PlayStation 2.
        /// </summary>
        [JsonIgnore] public bool IsPS2 => IsSupportedBy(GameVersionId.PS2);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the PlayStation 3.
        /// </summary>
        [JsonIgnore] public bool IsPS3 => IsSupportedBy(GameVersionId.PS3);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the PlayStation Portable.
        /// </summary>
        [JsonIgnore] public bool IsPSP => IsSupportedBy(GameVersionId.PSP);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the original Xbox.
        /// </summary>
        [JsonIgnore] public bool IsXbox => IsSupportedBy(GameVersionId.Xbox);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on Microsoft Windows.
        /// </summary>
        [JsonIgnore] public bool IsWindows => IsSupportedBy(GameVersionId.Windows);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on Microsoft Windows.
        /// </summary>
        [JsonIgnore] public bool IsDefinitiveEdition => IsSupportedBy(GameVersionId.DefinitiveEdition);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on Android or iOS.
        /// </summary>
        [JsonIgnore] public bool IsMobile => IsAndroid || IsiOS;

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on Microsoft Windows.
        /// </summary>
        [JsonIgnore] public bool IsPC => IsWindows;
    }

    public enum GameVersionId
    {
        Android,
        iOS,
        PS2,
        PS3,
        PSP,
        Xbox,
        Windows,
        DefinitiveEdition,
    }
}
