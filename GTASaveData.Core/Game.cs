using System.ComponentModel;

namespace GTASaveData
{
    /// <summary>
    /// <i>Grand Theft Auto</i> games supported by the <see cref="GTASaveData"/> library.
    /// </summary>
    /// <remarks>
    /// Only the "major versions" are listed; add-ons or remasters are included
    /// under the parent game.
    /// </remarks>
    public enum Game
    {
        [Description("Grand Theft Auto III")]
        GTA3,

        [Description("Grand Theft Auto: Vice City")]
        VC,

        [Description("Grand Theft Auto: San Andreas")]
        SA,

        [Description("Grand Theft Auto: Liberty City Stories")]
        LCS,

        [Description("Grand Theft Auto: Vice City Stories")]
        VCS,

        [Description("Grand Theft Auto IV")]
        IV
    }
}
