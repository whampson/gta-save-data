using Bogus;
using GTASaveData.GTA3;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestCollective : SerializableObjectTestBase<Collective>
    {
        public override Collective GenerateTestObject(SaveFileFormat format)
        {
            Faker<Collective> model = new Faker<Collective>()
                .RuleFor(x => x.Index, f => f.Random.Int())
                .RuleFor(x => x.Field04h, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Collective x0 = GenerateTestObject();
            Collective x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Index, x1.Index);
            Assert.Equal(x0.Field04h, x1.Field04h);
            Assert.Equal(x0, x1);
            Assert.Equal(SizeOf<Collective>(), data.Length);
        }
    }
}
