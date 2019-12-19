using System;
using System.ComponentModel;

namespace GTASaveData
{
    /// <summary>
    /// Game systems that can run GTA games.
    /// </summary>
    /// <remarks>
    /// Bits 15:0 represent console types, bits 31:16 represent regions and other meta flags.
    /// </remarks>
    [Flags]
    public enum SystemType : int
    {
        [Description("(unspecified)")]
        Unspecified = 0,

        // ===== Consoles =====

        [Description("Android")]
        Android = 0x0001,

        [Description("iOS")]
        IOS = 0x0002,

        [Description("PC")]
        PC = 0x0004,

        [Description("PlayStation 2")]
        PS2 = 0x0008,

        [Description("PlayStation 2")]
        PS2AU = PS2 | Australian,

        [Description("PlayStation 2")]
        PS2JP = PS2 | Japanese,

        [Description("Xbox")]
        Xbox = 0x0010,

        // ===== Regions =====

        [Description("Australian")]
        Australian = 0x0100,

        [Description("Japanese")]
        Japanese = 0x0200
    }
}
