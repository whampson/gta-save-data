using System;

namespace GTASaveData.Interfaces
{
    /// <summary>
    /// An interface for <i>Grand Theft Auto</i> game save files.
    /// </summary>
    public interface ISaveFile
    {
        /// <summary>
        /// The internal name of the save file.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The time the file was last saved.
        /// </summary>
        DateTime TimeStamp { get; set; }

        /// <summary>
        /// The <i>Grand Theft Auto</i> game this save belongs to.
        /// </summary>
        Game Game { get; }
    }
}
