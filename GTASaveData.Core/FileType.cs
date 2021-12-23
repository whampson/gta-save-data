using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// A <see cref="FileType"/> is a specifier for the arrangement of bytes in a save file.
    /// It consists of two parts: a list of supported <see cref="GameSystem"/>s, and a set
    /// of <see cref="FileTypeFlags"/>.
    /// </summary>
    /// <remarks>
    /// Many games exhibit minor layout differences in the save files between platforms.
    /// For example, the data in GTA3's PS2 and PC save files are packaged differently in terms
    /// of how the blocks are written. A <see cref="FileType"/> is designed to disambiguate
    /// thesee differing data layouts. It does NOT reveal information about the game script version
    /// or other game differences.
    /// <para>
    /// Equality between file formats is determined by comparing the compatibility list and flags;
    /// ID, display name, and description are ignored as they may be shared between similar file
    /// types.
    /// </para>
    /// </remarks>
    public struct FileType : IEquatable<FileType>
    {
        /// <summary>
        /// Placeholder for areas where <see cref="FileType"/> is irrelevant;
        /// do not use.
        /// </summary>
        public static readonly FileType Default = new FileType("", "", "");

        private readonly string m_id;
        private readonly string m_displayName;
        private readonly string m_description;
        private readonly FileTypeFlags m_flags;
        private readonly IEnumerable<GameSystem> m_compatibleWith;

        /// <summary>
        /// A short identifier for this file type.
        /// </summary>
        /// <remarks>
        /// Example: PC_STEAM
        /// </remarks>
        public string Id => m_id ?? "";

        /// <summary>
        /// A friendly name to use for this file type.
        /// </summary>
        /// <remarks>
        /// Example: PC (Steam)
        /// </remarks>
        public string DisplayName => m_displayName ?? "";

        /// <summary>
        /// A long description of this file type.
        /// </summary>
        public string Description => m_description ?? "";

        /// <summary>
        ///  A set of <see cref="FileTypeFlags"/> used to further delineate file formats.
        /// </summary>
        public FileTypeFlags Flags => m_flags;

        /// <summary>
        /// Game systems which are compatible with this <see cref="FileType"/>.
        /// </summary>
        public IEnumerable<GameSystem> CompatibilityList => m_compatibleWith ?? new List<GameSystem>();

        /// <summary>
        /// Creates a new <see cref="FileType"/>.
        /// </summary>
        /// <param name="id">A short identifier.</param>
        /// <param name="compatibleWith">A list of compatible <see cref="GameSystem"/>s.</param>
        public FileType(string id, params GameSystem[] compatibleWith)
            : this(id, id, id, FileTypeFlags.None, compatibleWith)
        { }

        /// <summary>
        /// Creates a new <see cref="FileType"/>.
        /// </summary>
        /// <param name="id">A short identifier.</param>
        /// <param name="flags">A set of <see cref="FileTypeFlags"/>.</param>
        /// <param name="compatibleWith">A list of compatible <see cref="GameSystem"/>s.</param>
        public FileType(string id, FileTypeFlags flags, params GameSystem[] compatibleWith)
            : this(id, id, id, flags, compatibleWith)
        { }

        /// <summary>
        /// Creates a new <see cref="FileType"/>.
        /// </summary>
        /// <param name="id">A short identifier.</param>
        /// <param name="displayName">A short friendly name.</param>
        /// <param name="description">A long description.</param>
        /// <param name="compatibleWith">A list of compatible <see cref="GameSystem"/>s.</param>
        public FileType(string id, string displayName, string description,
            params GameSystem[] compatibleWith)
            : this(id, displayName, description, FileTypeFlags.None, compatibleWith)
        { }

        /// <summary>
        /// Creates a new <see cref="FileType"/>.
        /// </summary>
        /// <param name="id">A short identifier.</param>
        /// <param name="displayName">A short friendly name.</param>
        /// <param name="description">A long description.</param>
        /// <param name="flags">A set of <see cref="FileTypeFlags"/>.</param>
        /// <param name="compatibleWith">A list of compatible <see cref="GameSystem"/>s.</param>
        public FileType(string id, string displayName, string description, FileTypeFlags flags,
            params GameSystem[] compatibleWith)
        {
            m_id = id;
            m_displayName = displayName;
            m_description = description;
            m_flags = flags;
            m_compatibleWith = new List<GameSystem>(compatibleWith);
        }

        /// <summary>
        /// Checks whether this <see cref="FileType"/> is supported on the specified <see cref="GameSystem"/>.
        /// </summary>
        public bool IsSupportedOn(GameSystem s)
        {
            return CompatibilityList.Contains(s);
        }

        /// <summary>
        /// Checks wither this <see cref="FileType"/> has a specific <see cref="FileTypeFlags"/> value set.
        /// </summary>
        public bool HasFlag(FileTypeFlags f)
        {
            return Flags.HasFlag(f);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (GameSystem c in CompatibilityList)
            {
                hash += 23 * c.GetHashCode();
            }
            hash += 23 * Flags.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FileType))
            {
                return false;
            }

            return Equals((FileType) obj);
        }

        public bool Equals(FileType other)
        {
            // Deliberately ignoring Id/DisplayName/Description as they may not be unique
            return CompatibilityList.SequenceEqual(other.CompatibilityList)
                && Flags.Equals(other.Flags);
        }

        public override string ToString()
        {
            return DisplayName;
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
        [JsonIgnore] public bool IsAndroid => IsSupportedOn(GameSystem.Android);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on Apple iOS.
        /// </summary>
        [JsonIgnore] public bool IsiOS => IsSupportedOn(GameSystem.iOS);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the PlayStation 2.
        /// </summary>
        [JsonIgnore] public bool IsPS2 => IsSupportedOn(GameSystem.PS2);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the PlayStation 3.
        /// </summary>
        [JsonIgnore] public bool IsPS3 => IsSupportedOn(GameSystem.PS3);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the PlayStation 4.
        /// </summary>
        [JsonIgnore] public bool IsPS4 => IsSupportedOn(GameSystem.PS4);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the PlayStation 5.
        /// </summary>
        [JsonIgnore] public bool IsPS5 => IsSupportedOn(GameSystem.PS5);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the PlayStation Portable.
        /// </summary>
        [JsonIgnore] public bool IsPSP => IsSupportedOn(GameSystem.PSP);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the Nintendo Switch.
        /// </summary>
        [JsonIgnore] public bool IsSwitch => IsSupportedOn(GameSystem.Switch);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the original Xbox.
        /// </summary>
        [JsonIgnore] public bool IsXbox => IsSupportedOn(GameSystem.Xbox);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the Xbox 360.
        /// </summary>
        [JsonIgnore] public bool IsXbox360 => IsSupportedOn(GameSystem.Xbox360);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on the Xbox One
        /// or Xbox Series X|S.
        /// </summary>
        [JsonIgnore] public bool IsXboxOne => IsSupportedOn(GameSystem.XboxOne);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on Microsoft Windows.
        /// </summary>
        [JsonIgnore] public bool IsWindows => IsSupportedOn(GameSystem.Windows);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on
        /// Android or iOS.
        /// </summary>
        [JsonIgnore] public bool IsMobile => IsAndroid || IsiOS;

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> is supported on Microsoft Windows.
        /// </summary>
        [JsonIgnore] public bool IsPC => IsWindows;


        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> has the Steam flag set.
        /// </summary>
        [JsonIgnore] public bool FlagSteam => HasFlag(FileTypeFlags.Steam);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> has the Australia flag set.
        /// </summary>
        [JsonIgnore] public bool FlagAustralia => HasFlag(FileTypeFlags.Australia);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> has the Japan flag set.
        /// </summary>
        [JsonIgnore] public bool FlagJapan => HasFlag(FileTypeFlags.Japan);

        /// <summary>
        /// Gets a value indicating whether this <see cref="FileType"/> has the Definitive Edition flag set.
        /// </summary>
        [JsonIgnore] public bool FlagDE => HasFlag(FileTypeFlags.DE);
    }

    /// <summary>
    /// Game systems that support GTA games.
    /// </summary>
    /// <remarks>
    /// List may be incomplete.
    /// </remarks>
    public enum GameSystem
    {
        None,

        /// <summary>
        /// Android OS.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto III</i><br/>
        /// <i>Grand Theft Auto: Vice City</i><br/>
        /// <i>Grand Theft Auto: San Andreas</i><br/>
        /// <i>Grand Theft Auto: Liberty City Stories</i><br/>
        /// </remarks>
        Android,

        /// <summary>
        /// Apple iOS.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto III</i><br/>
        /// <i>Grand Theft Auto: Vice City</i><br/>
        /// <i>Grand Theft Auto: San Andreas</i><br/>
        /// <i>Grand Theft Auto: Liberty City Stories</i><br/>
        /// </remarks>
        iOS,

        /// <summary>
        /// Sony PlayStation 2.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto III</i><br/>
        /// <i>Grand Theft Auto: Vice City</i><br/>
        /// <i>Grand Theft Auto: San Andreas</i><br/>
        /// <i>Grand Theft Auto: Liberty City Stories</i><br/>
        /// <i>Grand Theft Auto: Vice City Stories</i><br/>
        /// </remarks>
        PS2,

        /// <summary>
        /// Sony PlayStation 3.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto: San Andreas</i><br/>
        /// <i>Grand Theft Auto IV</i><br/>
        /// <i>Grand Theft Auto IV: The Lost and Damned</i><br/>
        /// <i>Grand Theft Auto: The Ballad of Gay Tony</i><br/>
        /// </remarks>
        PS3,

        /// <summary>
        /// Sony PlayStation 4.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto III - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: Vice City - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: San Andreas - The Definitive Edition</i><br/>
        /// </remarks>
        PS4,

        /// <summary>
        /// Sony PlayStation 5.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto III - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: Vice City - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: San Andreas - The Definitive Edition</i><br/>
        /// </remarks>
        PS5,

        /// <summary>
        /// Sony PlayStation Portable.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto: Liberty City Stories</i><br/>
        /// <i>Grand Theft Auto: Vice City Stories</i><br/>
        /// </remarks>
        PSP,

        /// <summary>
        /// Nintendo Switch.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto III - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: Vice City - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: San Andreas - The Definitive Edition</i><br/>
        /// </remarks>
        Switch,

        /// <summary>
        /// Microsoft Xbox.
        /// </summary>
        /// /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto III</i><br/>
        /// <i>Grand Theft Auto: Vice City</i><br/>
        /// <i>Grand Theft Auto: San Andreas</i><br/>
        /// </remarks>
        Xbox,

        /// <summary>
        /// Microsoft Xbox 360.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto: San Andreas</i><br/>
        /// <i>Grand Theft Auto IV</i><br/>
        /// <i>Grand Theft Auto IV: The Lost and Damned</i><br/>
        /// <i>Grand Theft Auto: The Ballad of Gay Tony</i><br/>
        /// </remarks>
        Xbox360,

        /// <summary>
        /// Microsoft Xbox One and Xbox Series X|S.
        /// </summary>
        /// <remarks>
        /// The Xbox One and Xbox Series X|S are lumped together as one
        /// because all consoles run the same operating system.
        /// <para>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto III - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: Vice City - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: San Andreas - The Definitive Edition</i><br/>
        /// </para>
        /// </remarks>
        XboxOne,

        /// <summary>
        /// Microsoft Windows.
        /// </summary>
        /// <remarks>
        /// Supported games:<br/>
        /// <i>Grand Theft Auto III</i><br/>
        /// <i>Grand Theft Auto III - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: Vice City</i><br/>
        /// <i>Grand Theft Auto: Vice City - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto: San Andreas</i><br/>
        /// <i>Grand Theft Auto: San Andreas - The Definitive Edition</i><br/>
        /// <i>Grand Theft Auto IV</i><br/>
        /// <i>Grand Theft Auto IV: The Lost and Damned</i><br/>
        /// <i>Grand Theft Auto: The Ballad of Gay Tony</i><br/>
        /// </remarks>
        Windows,
    }


    /// <summary>
    /// Modifier flags that may be used to further delineate a file type.
    /// </summary>
    /// <remarks>
    /// These typically align with different versions of the same game.
    /// </remarks>
    [Flags]
    public enum FileTypeFlags
    {
        None = 0,

        /// <summary>
        /// The PC Steam version of <i>Grand Theft Auto: Vice City</i>.
        /// </summary>
        Steam = 1 << 0,

        /// <summary>
        /// The Japanese PS2 release of <i>Grand Theft Auto III</i>.
        /// </summary>
        Japan = 1 << 1,

        /// <summary>
        /// The Australian PS2 release of <i>Grand Theft Auto III</i>.
        /// </summary>
        Australia = 1 << 2,

        /// <summary>
        /// <i>Grand Theft Auto: The Trilogy - The Definitive Edition</i>
        /// </summary>
        DE = 1 << 3,
    }
}
