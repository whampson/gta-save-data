using Bogus;
using GTASaveData.Common;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.Common
{
    public class TestVector2d : SerializableObjectTestBase<Vector2d>
    {
        public override Vector2d GenerateTestVector(FileFormat format)
        {
            Faker<Vector2d> model = new Faker<Vector2d>()
                .RuleFor(x => x.X, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.Y, f => f.Random.Float(-4000, 4000));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Vector2d x0 = GenerateTestVector();
            Vector2d x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.X, x1.X);
            Assert.Equal(x0.Y, x1.Y);
            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }
    }
}
