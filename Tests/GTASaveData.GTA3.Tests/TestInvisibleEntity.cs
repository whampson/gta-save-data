using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestInvisibleEntity : Base<InvisibleObject>
    {
        public override InvisibleObject GenerateTestObject(FileFormat format)
        {
            Faker<InvisibleObject> model = new Faker<InvisibleObject>()
                .RuleFor(x => x.Type, f => f.PickRandom<PoolType>())
                .RuleFor(x => x.Handle, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            InvisibleObject x0 = GenerateTestObject();
            InvisibleObject x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.Handle, x1.Handle);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
