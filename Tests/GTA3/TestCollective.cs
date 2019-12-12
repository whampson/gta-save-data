using Bogus;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestCollective
    {
        public static Collective Generate()
        {
            Faker<Collective> model = new Faker<Collective>()
                .RuleFor(x => x.Unknown0, f => f.Random.Int())
                .RuleFor(x => x.Unknown1, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void Sanity()
        {
            Collective c0 = Generate();
            Collective c1 = TestHelper.CreateSerializedCopy(c0);

            Assert.Equal(c0, c1);
        }
    }
}
