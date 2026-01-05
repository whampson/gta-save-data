using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestCraneData : Base<CraneData>
    {
        public override CraneData GenerateTestObject(GTA3SaveParams p)
        {
            Faker<CraneData> model = new Faker<CraneData>()
                .RuleFor(x => x.NumCranes, f => f.Random.Int())
                .RuleFor(x => x.CarsCollectedMilitaryCrane, f => f.PickRandom<CollectCarsMilitaryCrane>())
                .RuleFor(x => x.Cranes, f => Generator.Array(CraneData.MaxNumCranes, g => Generator.Generate<Crane, TestCrane, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            CraneData x0 = GenerateTestObject(p);
            CraneData x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.NumCranes, x1.NumCranes);
            Assert.Equal(x0.CarsCollectedMilitaryCrane, x1.CarsCollectedMilitaryCrane);
            Assert.Equal(x0.Cranes, x1.Cranes);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            CraneData x0 = GenerateTestObject(p);
            CraneData x1 = new CraneData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
