using System;

namespace GTASaveData.LCS
{
    /// <summary>
    /// Identifies the platform that produced a Liberty City Stories save.
    /// </summary>
    /// <remarks>
    /// This is a separate axis from the save-file <see cref="GTASaveData.FileType"/> layout: the
    /// mobile save layouts (<c>Mobile32</c>/<c>Mobile64</c>) are produced by both Android and iOS,
    /// so a single layout maps to more than one platform. The script content actually shipped on
    /// Android vs. iOS differs, and is captured by <see cref="ScriptVersion"/> rather than this enum.
    /// </remarks>
    public enum Platform
    {
        Android,
        iOS,
        PS2,
        PSP,
    }
}
