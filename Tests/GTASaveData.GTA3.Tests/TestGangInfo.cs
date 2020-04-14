using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGangInfo : Base<GangInfo>
    {
        public override GangInfo GenerateTestObject(SaveFileFormat format)
        {
            Faker<GangInfo> model = new Faker<GangInfo>()
                .RuleFor(x => x.VehicleModel, f => f.Random.Int())
                .RuleFor(x => x.PedModelOverride, f => f.Random.SByte())
                .RuleFor(x => x.Weapon1, f => f.PickRandom<WeaponType>())
                .RuleFor(x => x.Weapon2, f => f.PickRandom<WeaponType>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            GangInfo x0 = GenerateTestObject();
            GangInfo x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.VehicleModel, x1.VehicleModel);
            Assert.Equal(x0.PedModelOverride, x1.PedModelOverride);
            Assert.Equal(x0.Weapon1, x1.Weapon1);
            Assert.Equal(x0.Weapon2, x1.Weapon2);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
