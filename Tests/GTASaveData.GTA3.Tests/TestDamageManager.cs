using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestDamageManager : Base<DamageManager>
    {
        public override DamageManager GenerateTestObject(FileType format)
        {
            Faker<DamageManager> model = new Faker<DamageManager>()
                .RuleFor(x => x.WheelDamageEffect, f => f.Random.Float())
                .RuleFor(x => x.Engine, f => f.Random.Byte())
                .RuleFor(x => x.Wheels, f => Generator.Array(DamageManager.NumWheels, g => f.PickRandom<WheelStatus>()))
                .RuleFor(x => x.Doors, f => Generator.Array(DamageManager.NumDoors, g => f.PickRandom<DoorStatus>()))
                .RuleFor(x => x.Lights, f => Generator.Array(DamageManager.NumLights, g => f.PickRandom<LightStatus>()))
                .RuleFor(x => x.Panels, f => Generator.Array(DamageManager.NumPanels, g => f.PickRandom<PanelStatus>()))
                .RuleFor(x => x.Field24h, f => f.Random.Int());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            DamageManager x0 = GenerateTestObject(format);
            DamageManager x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.WheelDamageEffect, x1.WheelDamageEffect);
            Assert.Equal(x0.Engine, x1.Engine);
            Assert.Equal(x0.Wheels, x1.Wheels);
            Assert.Equal(x0.Doors, x1.Doors);
            Assert.Equal(x0.Lights, x1.Lights);
            Assert.Equal(x0.Panels, x1.Panels);
            Assert.Equal(x0.Field24h, x1.Field24h);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            DamageManager x0 = GenerateTestObject();
            DamageManager x1 = new DamageManager(x0);

            Assert.Equal(x0, x1);
        }
    }
}