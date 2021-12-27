using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRestartPoint : Base<RestartPoint>
    {
        public override RestartPoint GenerateTestObject(GTA3SaveParams p)
        {
            Faker<RestartPoint> model = new Faker<RestartPoint>()
                .RuleFor(x => x.Position, f => Generator.Vector3(f))
                .RuleFor(x => x.Angle, f => f.Random.Float());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            RestartPoint x0 = GenerateTestObject(p);
            RestartPoint x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Angle, x1.Angle);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            RestartPoint x0 = GenerateTestObject(p);
            RestartPoint x1 = new RestartPoint(x0);

            Assert.Equal(x0, x1);
        }
    }
}
