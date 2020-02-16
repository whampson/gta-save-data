using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestCollective : SerializableObjectTestBase<Collective>
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
            Collective x0 = GenerateTestVector();
            Collective x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Unknown0, x1.Unknown0);
            Assert.Equal(x0.Unknown1, x1.Unknown1);
            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }
    }
}
