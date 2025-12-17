using System;

namespace GTASaveData.Interfaces
{
    /// <summary>
    /// An interface for <i>Grand Theft Auto</i> game save files.
    /// </summary>
    public interface ISaveFile
    {

        /// <summary>
        /// The <i>Grand Theft Auto</i> game this save belongs to.
        /// </summary>
        Game Game { get; }
    }
}
