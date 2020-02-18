namespace GTASaveData.Common.Blocks
{
    public interface ICarGeneratorBlock
    {
        int NumberOfParkedCarsToGenerate { get; set; }

        ICarGenerator[] ParkedCars { get; }
    }
}
