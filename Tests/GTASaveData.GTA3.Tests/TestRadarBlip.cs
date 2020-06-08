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
                .RuleFor(x => x.ColorId, f => f.Random.Int())
                .RuleFor(x => x.Type, f => f.PickRandom<RadarBlipType>())
                .RuleFor(x => x.Handle, f => f.Random.Int())
                .RuleFor(x => x.RadarPosition, f => Generator.Vector2D(f))
                .RuleFor(x => x.WorldPosition, f => Generator.Vector3D(f))
                .RuleFor(x => x.BlipIndex, f => f.Random.Short())
                .RuleFor(x => x.Dim, f => f.Random.Bool())
                .RuleFor(x => x.InUse, f => f.Random.Bool())
                .RuleFor(x => x.Radius, f => f.Random.Float())
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

            Assert.Equal(x0.ColorId, x1.ColorId);
            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.Handle, x1.Handle);
            Assert.Equal(x0.RadarPosition, x1.RadarPosition);
            Assert.Equal(x0.WorldPosition, x1.WorldPosition);
            Assert.Equal(x0.BlipIndex, x1.BlipIndex);
            Assert.Equal(x0.Dim, x1.Dim);
            Assert.Equal(x0.InUse, x1.InUse);
            Assert.Equal(x0.Radius, x1.Radius);
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