using Bogus;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestVector : SerializableObjectTestBase<Vector>
    {
        public override Vector GenerateTestVector(FileFormat format)
        {
            Faker<Vector> model = new Faker<Vector>()
                .RuleFor(x => x.X, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.Y, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.Z, f => f.Random.Float(-4000, 4000));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Vector x0 = GenerateTestVector();
            Vector x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.X, x1.X);
            Assert.Equal(x0.Y, x1.Y);
            Assert.Equal(x0.Z, x1.Z);
            Assert.Equal(x0, x1);
            Assert.Equal(12, data.Length);
        }
    }
}
