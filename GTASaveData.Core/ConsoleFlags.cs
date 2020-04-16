using System;

namespace GTASaveData
{
    [Flags]
    public enum ConsoleFlags
    {
        None            = 0,
        NorthAmerica    = (1 << 0),
        Europe          = (1 << 1),
        Japan           = (1 << 2),
        Australia       = (1 << 3),
        Steam           = (1 << 4),
    }
}
