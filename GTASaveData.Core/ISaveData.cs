using System;
using System.Collections.Generic;

namespace GTASaveData.Types.Interfaces
{
    public interface ISaveData
    {
        string Name { get; set; }
        DateTime TimeLastSaved { get; set; }
        FileFormat FileFormat { get; set; }

        ICarGeneratorData CarGenerators { get; set; }

        IReadOnlyList<SaveDataObject> Blocks { get; }
    }
}
