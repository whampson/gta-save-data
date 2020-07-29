using System.ComponentModel;

namespace GTASaveData
{
    /// <summary>
    /// Supported GTA games.
    /// </summary>
    public enum Game
    {
        [Description("GTA III")]
        GTA3,

        [Description("Vice City")]
        VC,

        //[Description("San Andreas")]
        //SA,

        [Description("Liberty City Stories")]
        LCS,

        [Description("Vice City Stories")]
        VCS,

        //[Description("GTA IV")]
        //IV
    }
}
