using System.Collections.Generic;

namespace GTASaveData.Types.Interfaces
{
    public interface ICarGeneratorBlock
    {
        int NumberOfCarGenerators { get; set; }
        int CurrentActiveCount { get; set; }
        byte ProcessCounter { get; set; }
        byte GenerateEvenIfPlayerIsCloseCounter { get; set; }
        IEnumerable<ICarGenerator> CarGenerators { get; }
    }
}
