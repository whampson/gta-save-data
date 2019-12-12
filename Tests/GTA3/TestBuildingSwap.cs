using Bogus;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestBuildingSwap
    {
        public static BuildingSwap Generate()
        {
            Faker<BuildingSwap> model = new Faker<BuildingSwap>()
                .RuleFor(x => x.Type, f => f.PickRandom<ObjectType>())
                .RuleFor(x => x.StaticIndex, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.NewModelId, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.OldModelId, f => f.Random.Int(0, 9999));

            return model.Generate();
        }

        [Fact]
        public void Sanity()
        {
            BuildingSwap x0 = Generate();
            BuildingSwap x1 = TestHelper.CreateSerializedCopy(x0);

            Assert.Equal(x0, x1);
        }
    }
}
