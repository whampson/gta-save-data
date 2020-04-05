using Bogus;
using Xunit;
using TestFramework;
using GTASaveData.Types;
using GTASaveData.Core.Tests.Types;

namespace GTASaveData.VC.Tests
{
    public class TestCarGenerator : Base<CarGenerator>
    {
        public override CarGenerator GenerateTestObject(SaveFileFormat format)
        {
            Faker<CarGenerator> model = new Faker<CarGenerator>()
                .RuleFor(x => x.ModelIndex, f => f.Random.Int())
                .RuleFor(x => x.Position, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.Angle, f => f.Random.Float())
                .RuleFor(x => x.Color1, f => f.Random.Short())
                .RuleFor(x => x.Color2, f => f.Random.Short())
                .RuleFor(x => x.ForceSpawn, f => f.Random.Bool())
                .RuleFor(x => x.AlarmChance, f => f.Random.Byte())
                .RuleFor(x => x.LockedChance, f => f.Random.Byte())
                .RuleFor(x => x.MinDelay, f => f.Random.UShort())
                .RuleFor(x => x.MaxDelay, f => f.Random.UShort())
                .RuleFor(x => x.Timer, f => f.Random.UInt())
                .RuleFor(x => x.VehicleHandle, f => f.Random.Int())
                .RuleFor(x => x.UsesRemaining, f => f.Random.Short())
                .RuleFor(x => x.IsBlocking, f => f.Random.Bool());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            CarGenerator x0 = GenerateTestObject();
            CarGenerator x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.ModelIndex, x1.ModelIndex);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Angle, x1.Angle);
            Assert.Equal(x0.Color1, x1.Color1);
            Assert.Equal(x0.Color2, x1.Color2);
            Assert.Equal(x0.ForceSpawn, x1.ForceSpawn);
            Assert.Equal(x0.AlarmChance, x1.AlarmChance);
            Assert.Equal(x0.LockedChance, x1.LockedChance);
            Assert.Equal(x0.MinDelay, x1.MinDelay);
            Assert.Equal(x0.MaxDelay, x1.MaxDelay);
            Assert.Equal(x0.Timer, x1.Timer);
            Assert.Equal(x0.VehicleHandle, x1.VehicleHandle);
            Assert.Equal(x0.UsesRemaining, x1.UsesRemaining);
            Assert.Equal(x0.IsBlocking, x1.IsBlocking);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
