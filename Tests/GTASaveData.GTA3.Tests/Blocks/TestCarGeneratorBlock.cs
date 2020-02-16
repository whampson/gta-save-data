using Bogus;
using GTASaveData.GTA3;
using GTASaveData.GTA3.Blocks;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3.Blocks
{
    public class TestCarGeneratorBlock : SerializableObjectTestBase<CarGeneratorBlock>
    {
        public override CarGeneratorBlock GenerateTestVector(FileFormat format)
        {
            Faker<CarGeneratorBlock> model = new Faker<CarGeneratorBlock>()
                .RuleFor(x => x.TotalNumberOfCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.NumberOfParkedCarsToGenerate, f => f.Random.Int())
                .RuleFor(x => x.ProcessCounter, f => f.Random.Byte())
                .RuleFor(x => x.GenerateEvenIfPlayerIsCloseCounter, f => f.Random.Byte())
                .RuleFor(x => x.ParkedCars, f => Generator.CreateArray(CarGeneratorBlock.Limits.CarGeneratorsCount, g => Generator.Generate<CarGenerator, TestCarGenerator>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            CarGeneratorBlock x0 = GenerateTestVector();
            CarGeneratorBlock x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.TotalNumberOfCarGenerators, x1.TotalNumberOfCarGenerators);
            Assert.Equal(x0.NumberOfParkedCarsToGenerate, x1.NumberOfParkedCarsToGenerate);
            Assert.Equal(x0.ProcessCounter, x1.ProcessCounter);
            Assert.Equal(x0.GenerateEvenIfPlayerIsCloseCounter, x1.GenerateEvenIfPlayerIsCloseCounter);
            Assert.Equal(x0.ParkedCars, x1.ParkedCars);
            Assert.Equal(x0, x1);
            Assert.Equal(0x2D14, data.Length);
        }
    }
}
