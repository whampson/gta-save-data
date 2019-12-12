using System;
using System.ComponentModel;

namespace GTASaveData.Common
{
    [Flags]
    public enum SystemType
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
