using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestStoredCar : SaveDataObjectTestBase<StoredCar>
    {
        public override StoredCar GenerateTestVector(FileFormat format)
        {
            Faker<StoredCar> model = new Faker<StoredCar>()
                .RuleFor(x => x.ModelId, f => f.Random.Int())
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

            Assert.Equal(x0, x1);
            Assert.Equal(40, data.Length);
        }
    }
}
