using Bogus;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestVector3d
    {
        public static Vector3d Generate()
        {
            Faker<Vector3d> model = new Faker<Vector3d>()
                .RuleFor(x => x.X, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.Y, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.Z, f => f.Random.Float(-4000, 4000));

            return model.Generate();
        }

        [Fact]
        public void Sanity()
        {
            Vector3d v0 = Generate();
            Vector3d v1 = TestHelper.CreateSerializedCopy(v0);

            Assert.Equal(v0, v1);
        }
    }
}
