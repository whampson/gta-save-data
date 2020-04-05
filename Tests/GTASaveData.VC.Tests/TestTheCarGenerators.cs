using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.VC.Tests
{
    public class TestTheCarGenerators : Base<TheCarGenerators>
    {
        public override TheCarGenerators GenerateTestObject(SaveFileFormat format)
        {
            Faker<TheCarGenerators> model = new Faker<TheCarGenerators>()
                .RuleFor(x => x.NumberOfCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.CurrentActiveCount, f => f.Random.Int())
                .RuleFor(x => x.ProcessCounter, f => f.Random.Byte())
                .RuleFor(x => x.GenerateEvenIfPlayerIsCloseCounter, f => f.Random.Byte())
                .RuleFor(x => x.CarGeneratorArray, f => Generator.CreateArray(TheCarGenerators.Limits.NumberOfCarGenerators, g => Generator.Generate<CarGenerator, TestCarGenerator>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            TheCarGenerators x0 = GenerateTestObject();
            TheCarGenerators x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.NumberOfCarGenerators, x1.NumberOfCarGenerators);
            Assert.Equal(x0.CurrentActiveCount, x1.CurrentActiveCount);
            Assert.Equal(x0.ProcessCounter, x1.ProcessCounter);
            Assert.Equal(x0.GenerateEvenIfPlayerIsCloseCounter, x1.GenerateEvenIfPlayerIsCloseCounter);
            Assert.Equal(x0.CarGeneratorArray, x1.CarGeneratorArray);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
