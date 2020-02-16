using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestStaticReplacement : SerializableObjectTestBase<StaticReplacement>
    {
        public override StaticReplacement GenerateTestVector(FileFormat format)
        {
            Faker<StaticReplacement> model = new Faker<StaticReplacement>()
                .RuleFor(x => x.Type, f => f.PickRandom<ObjectType>())
                .RuleFor(x => x.StaticIndex, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.NewModelId, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.OldModelId, f => f.Random.Int(0, 9999));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            StaticReplacement x0 = GenerateTestVector();
            StaticReplacement x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.StaticIndex, x1.StaticIndex);
            Assert.Equal(x0.NewModelId, x1.NewModelId);
            Assert.Equal(x0.OldModelId, x1.OldModelId);
            Assert.Equal(x0, x1);
            Assert.Equal(16, data.Length);
        }
    }
}
