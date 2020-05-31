﻿using System.Collections.Generic;

namespace GTASaveData.Types.Interfaces
{
    public interface ICarGeneratorData
    {
        ICarGenerator this[int index] { get; set; }
        IEnumerable<ICarGenerator> CarGenerators { get; }
        int NumberOfCarGenerators { get; set; }
        int NumberOfEnabledCarGenerators { get; set; }
    }
}
