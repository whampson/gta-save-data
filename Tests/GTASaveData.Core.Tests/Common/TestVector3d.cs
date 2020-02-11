using Bogus;
using GTASaveData.Common;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.Common
{
    public class TestVector3d : SerializableObjectTestBase<Vector3d>
    {
        public override Vector3d GenerateTestVector(FileFormat format)
        {
            Faker<Vector3d> model = new Faker<Vector3d>()
                .RuleFor(x => x.X, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.Y, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.Z, f => f.Random.Float(-4000, 4000));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Vector3d x0 = GenerateTestVector();
            Vector3d x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(12, data.Length);
        }
    }
}
