using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestVector2d : SaveDataObjectTestBase<Vector2d>
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

            Assert.Equal(x0, x1);
            Assert.Equal(8, data.Length);
        }
    }
}
