using System;
using System.Collections.Generic;

namespace GTASaveData.Interfaces
{
    public interface ISaveFile
    {
        string Name { get; set; }
        DateTime TimeStamp { get; set; }
        FileFormat FileFormat { get; set; }

        IReadOnlyList<ISaveDataObject> Blocks { get; }

        bool HasSimpleVariables { get; }
        bool HasScriptData { get; }
        bool HasGarageData { get; }
        bool HasCarGenerators { get; }
        bool HasPlayerInfo { get; }
        bool HasStats { get; }

        ISimpleVariables SimpleVars { get; }
        IScriptData ScriptData { get; }
        IGarageData GarageData { get; }
        ICarGeneratorData CarGenerators { get; }
        IPlayerInfo PlayerInfo { get; }
        IStats Stats { get; }
    }
}
