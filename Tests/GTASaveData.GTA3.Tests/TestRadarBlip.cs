using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRadarBlip : Base<RadarBlip>
    {
        public override RadarBlip GenerateTestObject(FileFormat format)
        {
            Faker<RadarBlip> model = new Faker<RadarBlip>()
                .RuleFor(x => x.Color, f => f.Random.Int())
                .RuleFor(x => x.Type, f => f.PickRandom<RadarBlipType>())
                .RuleFor(x => x.EntityHandle, f => f.Random.Int())
                .RuleFor(x => x.RadarPosition, f => Generator.Vector2D(f))
                .RuleFor(x => x.MarkerPosition, f => Generator.Vector3D(f))
                .RuleFor(x => x.Index, f => f.Random.Short())
                .RuleFor(x => x.IsBright, f => f.Random.Bool())
                .RuleFor(x => x.Enabled, f => f.Random.Bool())
                .RuleFor(x => x.DebugSphereRadius, f => f.Random.Float())
                .RuleFor(x => x.Scale, f => f.Random.Short())
                .RuleFor(x => x.Display, f => f.PickRandom<RadarBlipDisplay>())
                .RuleFor(x => x.Sprite, f => f.PickRandom<RadarBlipSprite>());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            RadarBlip x0 = GenerateTestObject(format);
            RadarBlip x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Color, x1.Color);
            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.EntityHandle, x1.EntityHandle);
            Assert.Equal(x0.RadarPosition, x1.RadarPosition);
            Assert.Equal(x0.MarkerPosition, x1.MarkerPosition);
            Assert.Equal(x0.Index, x1.Index);
            Assert.Equal(x0.IsBright, x1.IsBright);
            Assert.Equal(x0.Enabled, x1.Enabled);
            Assert.Equal(x0.DebugSphereRadius, x1.DebugSphereRadius);
            Assert.Equal(x0.Scale, x1.Scale);
            Assert.Equal(x0.Display, x1.Display);
            Assert.Equal(x0.Sprite, x1.Sprite);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            RadarBlip x0 = GenerateTestObject();
            RadarBlip x1 = new RadarBlip(x0);

            Assert.Equal(x0, x1);
        }
    }
}