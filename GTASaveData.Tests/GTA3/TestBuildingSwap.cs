using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestBuildingSwap : SaveDataObjectTestBase<BuildingSwap>
    {
        public override BuildingSwap GenerateTestVector(FileFormat format)
        {
            Faker<BuildingSwap> model = new Faker<BuildingSwap>()
                .RuleFor(x => x.Type, f => f.PickRandom<ObjectType>())
                .RuleFor(x => x.StaticIndex, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.NewModelId, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.OldModelId, f => f.Random.Int(0, 9999));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            BuildingSwap x0 = GenerateTestVector();
            BuildingSwap x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(16, data.Length);
        }
    }
}
