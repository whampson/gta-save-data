using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRestartPoint : Base<RestartPoint>
    {
        public override RestartPoint GenerateTestObject(DataFormat format)
        {
            Faker<RestartPoint> model = new Faker<RestartPoint>()
                .RuleFor(x => x.Position, f => Generator.Vector3D(f))
                .RuleFor(x => x.Angle, f => f.Random.Float());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            RestartPoint x0 = GenerateTestObject();
            RestartPoint x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Angle, x1.Angle);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
