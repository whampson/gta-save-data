namespace GTASaveData.Common.Blocks
{
    public interface ICarGeneratorBlock<T> where T : ICarGenerator, new()
    {
        Array<T> ParkedCars { get; set; }

        int NumberOfParkedCarsToGenerate { get; set; }
    }
}
