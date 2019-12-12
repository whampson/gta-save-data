using Bogus;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestVector2d
    {
        public static Vector2d Generate()
        {
            Faker<Vector2d> model = new Faker<Vector2d>()
                .RuleFor(x => x.X, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.Y, f => f.Random.Float(-4000, 4000));

            return model.Generate();
        }

        [Fact]
        public void Sanity()
        {
            Vector2d v0 = Generate();
            Vector2d v1 = TestHelper.CreateSerializedCopy(v0);

            Assert.Equal(v0, v1);
        }
    }
}
