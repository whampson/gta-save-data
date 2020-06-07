using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestCraneData : Base<CraneData>
    {
        public override CraneData GenerateTestObject(FileFormat format)
        {
            Faker<CraneData> model = new Faker<CraneData>()
                .RuleFor(x => x.NumCranes, f => f.Random.Int())
                .RuleFor(x => x.CarsCollectedMilitaryCrane, f => f.PickRandom<CollectCarsMilitaryCrane>())
                .RuleFor(x => x.Cranes, f => Generator.Array(CraneData.MaxNumCranes, g => Generator.Generate<Crane, TestCrane>()));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            CraneData x0 = GenerateTestObject();
            CraneData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.NumCranes, x1.NumCranes);
            Assert.Equal(x0.CarsCollectedMilitaryCrane, x1.CarsCollectedMilitaryCrane);
            Assert.Equal(x0.Cranes, x1.Cranes);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
