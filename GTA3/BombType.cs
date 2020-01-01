using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GTASaveData.GTA3
{
    public enum BombType
    {
        [Description("(none)")]
        None,

        [Description("Timer")]
        Timer,

        [Description("Ignition")]
        Ignition,

        [Description("Remote")]
        Remote,

        [Description("Timer (armed)")]
        TimerArmed,

        [Description("Ignition (armed)")]
        IgnitionArmed
    }
}
