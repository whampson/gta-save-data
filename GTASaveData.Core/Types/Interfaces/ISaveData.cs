using System;
using System.Collections.Generic;

namespace GTASaveData.Types.Interfaces
{
    public interface ISaveData
    {
        string Name { get; set; }
        DateTime TimeStamp { get; set; }
        FileFormat FileFormat { get; set; }

        IReadOnlyList<ISaveDataObject> Blocks { get; }

        bool HasCarGenerators { get; }
        ICarGeneratorData CarGenerators { get; set; }
    }
}
