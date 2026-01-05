using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestCarGeneratorData : Base<CarGeneratorData>
    {
        public override CarGeneratorData GenerateTestObject(GTA3SaveParams p)
        {
            Faker<CarGeneratorData> model = new Faker<CarGeneratorData>()
                .RuleFor(x => x.NumberOfCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.NumberOfEnabledCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.ProcessCounter, f => f.Random.Byte())
                .RuleFor(x => x.GenerateEvenIfPlayerIsCloseCounter, f => f.Random.Byte())
                .RuleFor(x => x.CarGenerators, f => Generator.Array(CarGeneratorData.MaxNumCarGenerators, g => Generator.Generate<CarGenerator, TestCarGenerator, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            CarGeneratorData x0 = GenerateTestObject(p);
            CarGeneratorData x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.NumberOfCarGenerators, x1.NumberOfCarGenerators);
            Assert.Equal(x0.NumberOfEnabledCarGenerators, x1.NumberOfEnabledCarGenerators);
            Assert.Equal(x0.ProcessCounter, x1.ProcessCounter);
            Assert.Equal(x0.GenerateEvenIfPlayerIsCloseCounter, x1.GenerateEvenIfPlayerIsCloseCounter);
            Assert.Equal(x0.CarGenerators, x1.CarGenerators);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            CarGeneratorData x0 = GenerateTestObject(p);
            CarGeneratorData x1 = new CarGeneratorData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
