using System;

namespace GTASaveData.Serialization
{
    /// <summary>
    /// Game systems that GTA games can run on.
    /// </summary>
    public enum ConsoleType
    {
        None,

        Android,
        IOS,
        PC,
        PS2,
        PS3,
        PSP,
        Xbox,
        Xbox360
    }

    /// <summary>
    /// Regions and other meta flags pertaining to the <see cref="ConsoleType"/>s.
    /// </summary>
    [Flags]
    public enum ConsoleFlags
    {
        None        = 0,
        Steam       = 0b_0000_0001,
        Japan       = 0b_0000_0010,
        Australia   = 0b_0000_0100,
    }
}
