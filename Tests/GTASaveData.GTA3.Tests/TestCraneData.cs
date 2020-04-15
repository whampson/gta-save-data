using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestCraneData : Base<CraneData>
    {
        public override CraneData GenerateTestObject(SaveFileFormat format)
        {
            Faker<CraneData> model = new Faker<CraneData>()
                .RuleFor(x => x.NumCranes, f => f.Random.Int())
                .RuleFor(x => x.CarsCollectedMilitaryCrane, f => f.PickRandom<CollectCarsMilitaryCrane>())
                .RuleFor(x => x.Cranes, f => Generator.CreateArray<Crane>(CraneData.Limits.NumberOfCranes));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            CraneData x0 = GenerateTestObject();
            CraneData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.NumCranes, x1.NumCranes);
            Assert.Equal(x0.CarsCollectedMilitaryCrane, x1.CarsCollectedMilitaryCrane);
            Assert.Equal(x0.Cranes, x1.Cranes);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
