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
        Xbox360,


        //MacOS,
        //Windows
    }

    /// <summary>
    /// Regions and other meta flags pertaining to the <see cref="ConsoleType"/>s.
    /// </summary>
    [Flags]
    public enum ConsoleFlags
    {
        None            = 0,
        NorthAmerica    = 0b_0000_0001,
        Europe          = 0b_0000_0010,
        Japan           = 0b_0000_0100,
        Australia       = 0b_0000_1000,
        Steam           = 0b_0001_0000,
    }
}
