using System.Collections;
using System.Collections.Generic;

namespace GTASaveData.Common.Blocks
{
    public interface ICarGeneratorBlock
    {
        int NumberOfCarGenerators { get; set; }

        int NumberOfActiveCarGenerators { get; set; }

        IEnumerable<ICarGenerator> ParkedCars { get; }

        void SetParkedCar(int index, ICarGenerator cg);
    }
}
