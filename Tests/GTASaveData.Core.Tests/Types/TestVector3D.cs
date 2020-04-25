using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.Types
{
    public class TestVector3D : SaveDataObjectTestBase<Vector3D>
    {
        public override Vector3D GenerateTestObject(DataFormat format)
        {
            Faker<Vector3D> model = new Faker<Vector3D>()
                .RuleFor(x => x.X, f => f.Random.Float())
                .RuleFor(x => x.Y, f => f.Random.Float())
                .RuleFor(x => x.Z, f => f.Random.Float());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Vector3D x0 = GenerateTestObject();
            Vector3D x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.X, x1.X);
            Assert.Equal(x0.Y, x1.Y);
            Assert.Equal(x0.Z, x1.Z);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
