using Bogus;
using GTASaveData.Common;
using GTASaveData.Core.Tests.Common;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestStoredCar : SerializableObjectTestBase<StoredCar>
    {
        public override StoredCar GenerateTestVector(FileFormat format)
        {
            Faker<StoredCar> model = new Faker<StoredCar>()
                .RuleFor(x => x.Model, f => f.PickRandom<VehicleModel>())
                .RuleFor(x => x.Position, f => Generator.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.Rotation, f => Generator.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.Immunities, f => f.PickRandom<StoredCarImmunities>())
                .RuleFor(x => x.Color1, f => f.Random.Byte())
                .RuleFor(x => x.Color2, f => f.Random.Byte())
                .RuleFor(x => x.Radio, f => f.PickRandom<RadioStation>())
                .RuleFor(x => x.Extra1, f => f.Random.SByte())
                .RuleFor(x => x.Extra2, f => f.Random.SByte())
                .RuleFor(x => x.Bomb, f => f.PickRandom<BombType>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            StoredCar x0 = GenerateTestVector();
            StoredCar x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Model, x1.Model);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.Rotation, x1.Rotation);
            Assert.Equal(x0.Immunities, x1.Immunities);
            Assert.Equal(x0.Color1, x1.Color1);
            Assert.Equal(x0.Color2, x1.Color2);
            Assert.Equal(x0.Radio, x1.Radio);
            Assert.Equal(x0.Extra1, x1.Extra1);
            Assert.Equal(x0.Extra2, x1.Extra2);
            Assert.Equal(x0.Bomb, x1.Bomb);
            Assert.Equal(x0, x1);
            Assert.Equal(40, data.Length);
        }
    }
}
