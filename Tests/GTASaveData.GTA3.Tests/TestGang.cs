using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGang : Base<Gang>
    {
        public override Gang GenerateTestObject(GTA3SaveParams p)
        {
            Faker<Gang> model = new Faker<Gang>()
                .RuleFor(x => x.VehicleModel, f => f.Random.Int())
                .RuleFor(x => x.PedModelOverride, f => f.Random.SByte())
                .RuleFor(x => x.Weapon1, f => f.PickRandom<WeaponType>())
                .RuleFor(x => x.Weapon2, f => f.PickRandom<WeaponType>());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Gang x0 = GenerateTestObject(p);
            Gang x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.VehicleModel, x1.VehicleModel);
            Assert.Equal(x0.PedModelOverride, x1.PedModelOverride);
            Assert.Equal(x0.Weapon1, x1.Weapon1);
            Assert.Equal(x0.Weapon2, x1.Weapon2);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Gang x0 = GenerateTestObject(p);
            Gang x1 = new Gang(x0);

            Assert.Equal(x0, x1);
        }
    }
}
