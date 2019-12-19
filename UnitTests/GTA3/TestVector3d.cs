using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using System;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestVector3d
        : SaveDataObjectTestBase<Vector3d>
    {
        public override Vector3d GenerateTestVector(SystemType system)
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
            Vector3d x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(12, data.Length);
        }
    }
}
