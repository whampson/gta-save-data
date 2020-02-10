using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestCarGeneratorsBlock : SaveDataObjectTestBase<CarGeneratorsBlock>
    {
        public override CarGeneratorsBlock GenerateTestVector(FileFormat format)
        {
            Faker<CarGeneratorsBlock> model = new Faker<CarGeneratorsBlock>()
                .RuleFor(x => x.NumberOfCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.NumberOfActiveCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.ProcessCounter, f => f.Random.Byte())
                .RuleFor(x => x.GenerateEvenIfPlayerIsCloseCounter, f => f.Random.Byte())
                .RuleFor(x => x.CarGenerators, f => Generator.CreateArray(CarGeneratorsBlock.Limits.CarGeneratorsCount, g => Generator.Generate<CarGenerator, TestCarGenerator>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            CarGeneratorsBlock x0 = GenerateTestVector();
            CarGeneratorsBlock x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(0x2D14, data.Length);
        }
    }
}
