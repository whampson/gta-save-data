using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestInvisibleObject : SaveDataObjectTestBase<InvisibleObject, SerializationParams>
    {
        public override InvisibleObject GenerateTestObject(SerializationParams p)
        {
            Faker<InvisibleObject> model = new Faker<InvisibleObject>()
                .RuleFor(x => x.Type, f => f.PickRandom<EntityClassType>())
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
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            InvisibleObject x0 = GenerateTestObject();
            InvisibleObject x1 = new InvisibleObject(x0);

            Assert.Equal(x0, x1);
        }

        public override int GetSizeOfTestObject(InvisibleObject obj)
        {
            return 8;
        }
    }
}
