using Bogus;
using GTASaveData.GTA3;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestStaticReplacement : SerializableObjectTestBase<StaticReplacement>
    {
        public override StaticReplacement GenerateTestObject(SaveFileFormat format)
        {
            Faker<StaticReplacement> model = new Faker<StaticReplacement>()
                .RuleFor(x => x.Type, f => f.PickRandom<ObjectType>())
                .RuleFor(x => x.StaticIndex, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.NewModelIndex, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.OldModelIndex, f => f.Random.Int(0, 9999));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            StaticReplacement x0 = GenerateTestObject();
            StaticReplacement x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.StaticIndex, x1.StaticIndex);
            Assert.Equal(x0.NewModelIndex, x1.NewModelIndex);
            Assert.Equal(x0.OldModelIndex, x1.OldModelIndex);
            Assert.Equal(x0, x1);
            Assert.Equal(SizeOf<StaticReplacement>(), data.Length);
        }
    }
}
