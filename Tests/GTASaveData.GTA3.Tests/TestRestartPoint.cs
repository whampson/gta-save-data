using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRestartPoint : Base<RestartPoint>
    {
        public override RestartPoint GenerateTestObject(FileType format)
        {
            Faker<RestartPoint> model = new Faker<RestartPoint>()
                .RuleFor(x => x.Position, f => Generator.Vector3(f))
                .RuleFor(x => x.Angle, f => f.Random.Float());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            RestartPoint x0 = GenerateTestObject(format);
            RestartPoint x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Angle, x1.Angle);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            RestartPoint x0 = GenerateTestObject();
            RestartPoint x1 = new RestartPoint(x0);

            Assert.Equal(x0, x1);
        }
    }
}
