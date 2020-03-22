using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.Types
{
    public class TestVector2D : SerializableObjectTestBase<Vector2D>
    {
        public override Vector2D GenerateTestObject(SaveFileFormat format)
        {
            Faker<Vector2D> model = new Faker<Vector2D>()
                .RuleFor(x => x.X, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.Y, f => f.Random.Float(-4000, 4000));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Vector2D x0 = GenerateTestObject();
            Vector2D x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.X, x1.X);
            Assert.Equal(x0.Y, x1.Y);
            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }
    }
}
