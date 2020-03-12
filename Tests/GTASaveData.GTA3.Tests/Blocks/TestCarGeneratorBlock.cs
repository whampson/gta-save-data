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
                .RuleFor(x => x.NumberOfCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.NumberOfActiveCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.ProcessCounter, f => f.Random.Byte())
                .RuleFor(x => x.GenerateEvenIfPlayerIsCloseCounter, f => f.Random.Byte())
                .RuleFor(x => x.ParkedCars, f => Generator.CreateArray(CarGeneratorBlock.Limits.CarGeneratorsCapacity, g => Generator.Generate<CarGenerator, TestCarGenerator>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            CarGeneratorBlock x0 = GenerateTestVector();
            CarGeneratorBlock x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.NumberOfCarGenerators, x1.NumberOfCarGenerators);
            Assert.Equal(x0.NumberOfActiveCarGenerators, x1.NumberOfActiveCarGenerators);
            Assert.Equal(x0.ProcessCounter, x1.ProcessCounter);
            Assert.Equal(x0.GenerateEvenIfPlayerIsCloseCounter, x1.GenerateEvenIfPlayerIsCloseCounter);
            Assert.Equal(x0.ParkedCars, x1.ParkedCars);
            Assert.Equal(x0, x1);
            Assert.Equal(0x2D14, data.Length);
        }
    }
}
