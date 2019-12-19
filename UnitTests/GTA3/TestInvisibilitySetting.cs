using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestInvisibilitySetting
        : SaveDataObjectTestBase<InvisibilitySetting>
    {
        public override InvisibilitySetting GenerateTestVector(SystemType system)
        {
            Faker<InvisibilitySetting> model = new Faker<InvisibilitySetting>()
                .RuleFor(x => x.Type, f => f.PickRandom<ObjectType>())
                .RuleFor(x => x.StaticIndex, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void Sanity()
        {
            InvisibilitySetting x0 = GenerateTestVector();
            InvisibilitySetting x1 = TestHelper.CreateSerializedCopy(x0);

            Assert.Equal(x0, x1);
        }
    }
}
