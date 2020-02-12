using System;

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
        public static readonly FileFormat None = new FileFormat();

        /// <summary>
        /// Gets the <see cref="FileFormat"/> display name.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the <see cref="ConsoleType"/> that this <see cref="FileFormat"/> is meant for.
        /// </summary>
        public ConsoleType TargetSystem { get; }

        /// <summary>
        /// Gets a <see cref="ConsoleFlags"/> value that futher describes this <see cref="FileFormat"/>.
        /// </summary>
        public ConsoleFlags Flags { get; }

        /// <summary>
        /// Gets a value indicating whether this file format is for the Android platform.
        /// </summary>
        public bool IsAndroid
        {
            get { return TargetSystem == ConsoleType.Android; }
        }

        /// <summary>
        /// Gets a value indicating whether this file format is for the iOS platform.
        /// </summary>
        public bool IsIOS
        {
            get { return TargetSystem == ConsoleType.IOS; }
        }

        /// <summary>
        /// Gets a value indicating whether this file format is for the Android or iOS (mobile) platforms.
        /// </summary>
        public bool IsMobile
        {
            get { return IsAndroid || IsIOS; }
        }

        /// <summary>
        /// Gets a value indicating whether this file format is for the Windows or macOS (PC) platforms.
        /// </summary>
        public bool IsPC
        {
            get { return TargetSystem == ConsoleType.PC; }
        }

        /// <summary>
        /// Gets a value indicating whether this file format is for the PlayStation 2 platform.
        /// </summary>
        public bool IsPS2
        {
            get { return TargetSystem == ConsoleType.PS2; }
        }

        /// <summary>
        /// Gets a value indicating whether this file format is for the PlayStation 3 platform.
        /// </summary>
        public bool IsPS3
        {
            get { return TargetSystem == ConsoleType.PS3; }
        }

        /// <summary>
        /// Gets a value indicating whether this file format is for the PlayStation Portable platform.
        /// </summary>
        public bool IsPSP
        {
            get { return TargetSystem == ConsoleType.PSP; }
        }

        /// <summary>
        /// Gets a value indicating whether this file format is for the Xbox platform.
        /// </summary>
        public bool IsXbox
        {
            get { return TargetSystem == ConsoleType.Xbox; }
        }

        /// <summary>
        /// Gets a value indicating whether this file format is for the Xbox 360 platform.
        /// </summary>
        public bool IsXbox360
        {
            get { return TargetSystem == ConsoleType.Xbox360; }
        }

        private FileFormat()
            : this(null, ConsoleType.None)
        { }

        /// <summary>
        /// Creates a new <see cref="FileFormat"/> instance.
        /// </summary>
        /// <param name="displayName">
        /// A string that describes this <see cref="FileFormat"/>.
        /// </param>
        /// <param name="targetSystem">
        /// The <see cref="ConsoleType"/> that this <see cref="FileFormat"/> is meant for.
        /// </param>
        /// <param name="flags">
        /// A <see cref="ConsoleFlags"/> value that futher describes this <see cref="FileFormat"/>.
        /// </param>
        public FileFormat(string displayName, ConsoleType targetSystem,
            ConsoleFlags flags = ConsoleFlags.None)
        {
            DisplayName = displayName;
            TargetSystem = targetSystem;
            Flags = flags;
        }

        /// <summary>
        /// Checks whether a console flag is set.
        /// </summary>
        /// <param name="flag">The flag to check for.</param>
        /// <returns>A value indicating whether the specified console flag is set.</returns>
        public bool HasFlag(ConsoleFlags flag)
        {
            return Flags.HasFlag(flag);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash *= DisplayName.GetHashCode();
            hash *= TargetSystem.GetHashCode();
            hash *= Flags.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileFormat);
        }

        public bool Equals(FileFormat other)
        {
            if (other == null)
            {
                return false;
            }

            return DisplayName.Equals(other.DisplayName)
                && TargetSystem.Equals(other.TargetSystem)
                && Flags.Equals(other.Flags);
        }

        public override string ToString()
        {
            return DisplayName ?? string.Empty;
        }
    }
}
