using Bogus;
using GTASaveData.VC;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;
using GTASaveData.Common;
using GTASaveData.Tests.Common;

namespace GTASaveData.Tests.VC
{
    public class TestCarGenerator : SaveDataObjectTestBase<CarGenerator>
    {
        public override CarGenerator GenerateTestVector(FileFormat format)
        {
            Faker<CarGenerator> model = new Faker<CarGenerator>()
                .RuleFor(x => x.Model, f => f.Random.Int())
                .RuleFor(x => x.Position, f => Generator.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.Heading, f => f.Random.Float())
                .RuleFor(x => x.Color1, f => f.Random.UShort())
                .RuleFor(x => x.Color2, f => f.Random.UShort())
                .RuleFor(x => x.ForceSpawn, f => f.Random.Bool())
                .RuleFor(x => x.AlarmChance, f => f.Random.Byte())
                .RuleFor(x => x.LockChance, f => f.Random.Byte())
                .RuleFor(x => x.MinDelay, f => f.Random.UShort())
                .RuleFor(x => x.MaxDelay, f => f.Random.UShort())
                .RuleFor(x => x.Timer, f => f.Random.Int())
                .RuleFor(x => x.VehiclePoolIndex, f => f.Random.Int())
                .RuleFor(x => x.Generate, f => f.Random.Bool())
                .RuleFor(x => x.HasRecentlyBeenStolen, f => f.Random.Bool());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            CarGenerator x0 = GenerateTestVector();
            CarGenerator x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(0x2C, data.Length);
        }
    }
}
