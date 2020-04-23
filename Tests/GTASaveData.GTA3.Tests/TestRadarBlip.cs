using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRadarBlip : Base<RadarBlip>
    {
        public override RadarBlip GenerateTestObject(DataFormat format)
        {
            Faker<RadarBlip> model = new Faker<RadarBlip>()
                .RuleFor(x => x.ColorId, f => f.Random.Int())
                .RuleFor(x => x.Type, f => f.PickRandom<BlipType>())
                .RuleFor(x => x.Handle, f => f.Random.Int())
                .RuleFor(x => x.RadarPosition, Generator.Generate<Vector2D, TestVector2D>())
                .RuleFor(x => x.WorldPosition, Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.BlipIndex, f => f.Random.Short())
                .RuleFor(x => x.Dim, f => f.Random.Bool())
                .RuleFor(x => x.InUse, f => f.Random.Bool())
                .RuleFor(x => x.Radius, f => f.Random.Float())
                .RuleFor(x => x.Scale, f => f.Random.Short())
                .RuleFor(x => x.Display, f => f.PickRandom<BlipDisplay>())
                .RuleFor(x => x.Sprite, f => f.PickRandom<BlipSprite>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            RadarBlip x0 = GenerateTestObject();
            RadarBlip x1 = CreateSerializedCopy(x0, out byte[] data);

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
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}