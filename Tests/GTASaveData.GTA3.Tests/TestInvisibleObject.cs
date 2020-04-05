using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestInvisibleObject : Base<InvisibleObject>
    {
        public override InvisibleObject GenerateTestObject(SaveFileFormat format)
        {
            Faker<InvisibleObject> model = new Faker<InvisibleObject>()
                .RuleFor(x => x.Type, f => f.PickRandom<ObjectType>())
                .RuleFor(x => x.StaticIndex, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            InvisibleObject x0 = GenerateTestObject();
            InvisibleObject x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.StaticIndex, x1.StaticIndex);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
