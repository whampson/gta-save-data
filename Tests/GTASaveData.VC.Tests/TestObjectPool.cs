using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.VC.Tests
{
    public class TestObjectPool : Base<ObjectPool>
    {
        public override ObjectPool GenerateTestObject(FileFormat format)
        {
            Faker<ObjectPool> model = new Faker<ObjectPool>()
                .RuleFor(x => x.Objects,
                    f => Generator.Array(f.Random.Int(1, 25), g => Generator.Generate<PhysicalObject, TestPhysicalObject>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            ObjectPool x0 = GenerateTestObject(format);
            ObjectPool x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Objects, x1.Objects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            ObjectPool x0 = GenerateTestObject();
            ObjectPool x1 = new ObjectPool(x0);

            Assert.Equal(x0, x1);
        }
    }
}
