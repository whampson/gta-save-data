using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestInvisibleEntity : Base<InvisibleEntity>
    {
        public override InvisibleEntity GenerateTestObject(DataFormat format)
        {
            Faker<InvisibleEntity> model = new Faker<InvisibleEntity>()
                .RuleFor(x => x.Type, f => f.PickRandom<ObjectType>())
                .RuleFor(x => x.Handle, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            InvisibleEntity x0 = GenerateTestObject();
            InvisibleEntity x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.Handle, x1.Handle);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
