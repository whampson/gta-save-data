using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestCollective : SaveDataObjectTestBase<Collective>
    {
        public override Collective GenerateTestVector(FileFormat format)
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
            Collective c1 = CreateSerializedCopy(c0, out byte[] data);

            Assert.Equal(c0, c1);
            Assert.Equal(8, data.Length);
        }
    }
}
