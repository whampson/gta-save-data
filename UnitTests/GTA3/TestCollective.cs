using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestCollective
        : SaveDataObjectTestBase<Collective>
    {
        public override Collective GenerateTestVector(SystemType system)
        {
            Faker<Collective> model = new Faker<Collective>()
                .RuleFor(x => x.Unknown0, f => f.Random.Int())
                .RuleFor(x => x.Unknown1, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Collective c0 = GenerateTestVector();
            Collective c1 = TestHelper.CreateSerializedCopy(c0, out byte[] data);

            Assert.Equal(c0, c1);
            Assert.Equal(8, data.Length);
        }
    }
}
