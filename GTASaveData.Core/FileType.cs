﻿using Newtonsoft.Json;
using System;

namespace GTASaveData
{
    /// <summary>
    /// A <see cref="FileType"/> identifies the physical layout of a save file. It consists of a
    /// unique string identifier and a set of <see cref="FileTypeFlags"/> that delineate  layout
    /// families.
    /// </summary>
    /// <remarks>
    /// Many games exhibit minor layout differences between platforms. For example, the data in
    /// GTA3's PS2 and PC save files are packaged differently in terms of how the blocks are
    /// written, and the mobile titles use either 32- or 64-bit pointers. A <see cref="FileType"/>
    /// disambiguates these layouts. It does NOT reveal the game script version or other content
    /// differences: a single layout can be produced by multiple platforms, and distinct scripts
    /// can share a layout.
    /// <para/>
    /// A <see cref="FileType"/> also does NOT enumerate which platforms produce it. For example,
    /// the mobile save layouts are shared by both Android and iOS, so a single <see cref="FileType"/>
    /// (e.g. <c>Mobile32</c>) is compatible with more than one platform. Where a game needs to
    /// know the platform (rather than the layout), it should map file types to platforms using a
    /// game-local enum (e.g. <c>GTASaveData.LCS.Platform</c>) rather than a field on this type.
    /// For Liberty City Stories, the script content shipped on Android vs. iOS actually differs,
    /// and is captured by <c>GTASaveData.LCS.ScriptVersion</c>.
    /// </remarks>
    public readonly struct FileType : IEquatable<FileType>
    {
        /// <summary>
        /// Placeholder generic <see cref="FileType"/>.
        /// </summary>
        public static readonly FileType Default = new FileType("");

        private readonly string m_id;
        private readonly FileTypeFlags m_flags;

        /// <summary>
        /// A short identifier for this file type.
        /// </summary>
        /// <remarks>
        /// Example: <c>PC_STEAM</c>
        /// </remarks>
        public string Id => m_id ?? "";

        /// <summary>
        /// Flags that delineate layout families (see <see cref="FileTypeFlags"/>).
        /// </summary>
        public FileTypeFlags Flags => m_flags;

        /// <summary>
        /// Creates a new <see cref="FileType"/>.
        /// </summary>
        /// <param name="id">A short identifier.</param>
        public FileType(string id)
            : this(id, 0)
        { }

        /// <summary>
        /// Creates a new <see cref="FileType"/>.
        /// </summary>
        /// <param name="id">A short identifier.</param>
        /// <param name="flags">A set of <see cref="FileTypeFlags"/> that delineate this layout.</param>
        public FileType(string id, FileTypeFlags flags)
        {
            m_id = id;
            m_flags = flags;
        }

        /// <summary>
        /// Checks whether this file type has a flag set.
        /// </summary>
        public bool HasFlag(FileTypeFlags flag)
        {
            return (Flags & flag) == flag;
        }

        public override int GetHashCode()
        {
            int hash = 17;
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
            return Id.Equals(other.Id) && Flags.Equals(other.Flags);
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
        /// Gets a value indicating whether this <see cref="FileType"/> uses the Android or iOS
        /// mobile layout.
        /// </summary>
        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> uses either mobile layout.
        /// </summary>
        [JsonIgnore] public bool IsMobile => IsMobile32 || IsMobile64;

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> uses the 32-bit pointer
        /// mobile layout.
        /// </summary>
        [JsonIgnore] public bool IsMobile32 => HasFlag(FileTypeFlags.Mobile32);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> uses the 64-bit pointer
        /// mobile layout.
        /// </summary>
        [JsonIgnore] public bool IsMobile64 => HasFlag(FileTypeFlags.Mobile64);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> uses the PlayStation 2 layout.
        /// </summary>
        [JsonIgnore] public bool IsPS2 => HasFlag(FileTypeFlags.PS2);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> uses the PlayStation 3 layout.
        /// </summary>
        [JsonIgnore] public bool IsPS3 => HasFlag(FileTypeFlags.PS3);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> uses the PlayStation Portable layout.
        /// </summary>
        [JsonIgnore] public bool IsPSP => HasFlag(FileTypeFlags.PSP);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> uses the original Xbox layout.
        /// </summary>
        [JsonIgnore] public bool IsXbox => HasFlag(FileTypeFlags.Xbox);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> uses the Microsoft Windows
        /// (PC) layout. This is the same layout produced by macOS.
        /// </summary>
        [JsonIgnore] public bool IsWindows => HasFlag(FileTypeFlags.Windows);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> uses the Definitive Edition layout.
        /// </summary>
        [JsonIgnore] public bool IsDefinitiveEdition => HasFlag(FileTypeFlags.DefinitiveEdition);
    }

    /// <summary>
    /// Bitflags that delineate save-file layout families. A <see cref="FileType"/> carries one or
    /// more of these flags to describe its physical layout; game-specific refinements (e.g. regional
    /// PS2 variants or the Steam build) can be represented with game-local flags in the upper bits,
    /// OR-ed together with the shared layout flags.
    /// </summary>
    /// <remarks>
    /// These flags describe <i>layouts</i>, not platforms. For example <see cref="Mobile32"/> and
    /// <see cref="Mobile64"/> describe the 32- and 64-bit pointer widths used by the mobile titles,
    /// and do not distinguish Android from iOS — both mobile platforms produce both layouts. Where a
    /// game must distinguish platforms, it should use a game-local enum.
    /// </remarks>
    [Flags]
    public enum FileTypeFlags : uint
    {
        Mobile32 = 1 << 0,
        Mobile64 = 1 << 1,
        PS2 = 1 << 2,
        PS3 = 1 << 3,
        PSP = 1 << 4,
        Xbox = 1 << 5,
        Windows = 1 << 6,
        DefinitiveEdition = 1 << 7,
    }
}