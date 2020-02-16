using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using Xunit;
using GTASaveData.Common;
using TestFramework;
using GTASaveData.Core.Tests.Common;

namespace GTASaveData.Tests.GTA3
{
    public class TestCarGenerator : SerializableObjectTestBase<CarGenerator>
    {
        public override CarGenerator GenerateTestVector(FileFormat format)
        {
            Faker<CarGenerator> model = new Faker<CarGenerator>()
                .RuleFor(x => x.Model, f => f.PickRandom<VehicleModel>())
                .RuleFor(x => x.Position, f => Generator.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.Heading, f => f.Random.Float())
                .RuleFor(x => x.Color1, f => f.Random.UShort())
                .RuleFor(x => x.Color2, f => f.Random.UShort())
                .RuleFor(x => x.ForceSpawn, f => f.Random.Bool())
                .RuleFor(x => x.AlarmChance, f => f.Random.Byte())
                .RuleFor(x => x.LockChance, f => f.Random.Byte())
                .RuleFor(x => x.MinDelay, f => f.Random.UShort())
                .RuleFor(x => x.MaxDelay, f => f.Random.UShort())
                .RuleFor(x => x.Timer, f => f.Random.UInt())
                .RuleFor(x => x.VehiclePoolIndex, f => f.Random.Int())
                .RuleFor(x => x.Enabled, f => f.Random.Bool())
                .RuleFor(x => x.HasRecentlyBeenStolen, f => f.Random.Bool())
                .RuleFor(x => x.VecInf, f => Generator.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.VecSup, f => Generator.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.Unknown, f => f.Random.Float());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            CarGenerator x0 = GenerateTestVector();
            CarGenerator x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Model, x1.Model);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Heading, x1.Heading);
            Assert.Equal(x0.Color1, x1.Color1);
            Assert.Equal(x0.Color2, x1.Color2);
            Assert.Equal(x0.ForceSpawn, x1.ForceSpawn);
            Assert.Equal(x0.AlarmChance, x1.AlarmChance);
            Assert.Equal(x0.LockChance, x1.LockChance);
            Assert.Equal(x0.MinDelay, x1.MinDelay);
            Assert.Equal(x0.MaxDelay, x1.MaxDelay);
            Assert.Equal(x0.Timer, x1.Timer);
            Assert.Equal(x0.VehiclePoolIndex, x1.VehiclePoolIndex);
            Assert.Equal(x0.Enabled, x1.Enabled);
            Assert.Equal(x0.HasRecentlyBeenStolen, x1.HasRecentlyBeenStolen);
            Assert.Equal(x0.VecInf, x1.VecInf);
            Assert.Equal(x0.VecSup, x1.VecSup);
            Assert.Equal(x0.Unknown, x1.Unknown);
            Assert.Equal(x0, x1);
            Assert.Equal(0x48, data.Length);
        }
    }
}
