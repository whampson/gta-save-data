using System.Collections.Generic;

namespace GTASaveData.Types.Interfaces
{
    public interface ICarGeneratorData
    {
        int NumberOfCarGenerators { get; set; }
        int NumberOfEnabledCarGenerators { get; set; }
        byte ProcessCounter { get; set; }
        byte GenerateEvenIfPlayerIsCloseCounter { get; set; }
        IEnumerable<ICarGenerator> CarGenerators { get; }
        ICarGenerator this[int index] { get; set; }
    }
}
