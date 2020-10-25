using Bogus;
using Xunit;
using TestFramework;

namespace GTASaveData.VC.Tests
{
    public class TestCarGenerator : Base<CarGenerator>
    {
        public override CarGenerator GenerateTestObject(FileFormat format)
        {
            Faker<CarGenerator> model = new Faker<CarGenerator>()
                .RuleFor(x => x.Model, f => f.Random.Int())
                .RuleFor(x => x.Position, f => Generator.Vector3(f))
                .RuleFor(x => x.Heading, f => f.Random.Float())
                .RuleFor(x => x.Color1, f => f.Random.Short())
                .RuleFor(x => x.Color2, f => f.Random.Short())
                .RuleFor(x => x.ForceSpawn, f => f.Random.Bool())
                .RuleFor(x => x.AlarmChance, f => f.Random.Byte())
                .RuleFor(x => x.LockedChance, f => f.Random.Byte())
                .RuleFor(x => x.MinDelay, f => f.Random.UShort())
                .RuleFor(x => x.MaxDelay, f => f.Random.UShort())
                .RuleFor(x => x.Timer, f => f.Random.UInt())
                .RuleFor(x => x.Handle, f => f.Random.Int())
                .RuleFor(x => x.Enabled, f => f.Random.Bool())
                .RuleFor(x => x.IsBlocking, f => f.Random.Bool());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            CarGenerator x0 = GenerateTestObject();
            CarGenerator x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Model, x1.Model);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Heading, x1.Heading);
            Assert.Equal(x0.Color1, x1.Color1);
            Assert.Equal(x0.Color2, x1.Color2);
            Assert.Equal(x0.ForceSpawn, x1.ForceSpawn);
            Assert.Equal(x0.AlarmChance, x1.AlarmChance);
            Assert.Equal(x0.LockedChance, x1.LockedChance);
            Assert.Equal(x0.MinDelay, x1.MinDelay);
            Assert.Equal(x0.MaxDelay, x1.MaxDelay);
            Assert.Equal(x0.Timer, x1.Timer);
            Assert.Equal(x0.Handle, x1.Handle);
            Assert.Equal(x0.Enabled, x1.Enabled);
            Assert.Equal(x0.IsBlocking, x1.IsBlocking);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
