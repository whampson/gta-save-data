using Bogus;
using GTASaveData.VC;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;
using GTASaveData.VC.Blocks;

namespace GTASaveData.Tests.VC.Blocks
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

            Assert.Equal(x0, x1);
            Assert.Equal(0x1FE0, data.Length);
        }
    }
}
