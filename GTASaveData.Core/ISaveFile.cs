using System;
using System.Collections.Generic;

namespace GTASaveData.Types.Interfaces
{
    public interface ISaveFile
    {
        string Name { get; set; }
        DateTime TimeLastSaved { get; set; }
        DataFormat FileFormat { get; set; }
        IReadOnlyList<SaveDataObject> Blocks { get; }
    }
}
