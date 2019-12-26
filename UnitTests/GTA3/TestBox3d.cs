using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestBox3d
        : SaveDataObjectTestBase<Box3d>
    {
        public override Box3d GenerateTestVector(SystemType system)
        {
            Faker<Box3d> model = new Faker<Box3d>()
                .RuleFor(x => x.XMin, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.XMax, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.YMin, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.YMax, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.ZMin, f => f.Random.Float(-4000, 4000))
                .RuleFor(x => x.ZMax, f => f.Random.Float(-4000, 4000));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Box3d x0 = GenerateTestVector();
            Box3d x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(24, data.Length);
        }
    }
}
