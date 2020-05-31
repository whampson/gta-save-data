using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestWeapon : Base<Weapon>
    {
        public override Weapon GenerateTestObject(FileFormat format)
        {
            Faker<Weapon> model = new Faker<Weapon>()
                .RuleFor(x => x.Type, f => f.PickRandom<WeaponType>())
                .RuleFor(x => x.State, f => f.PickRandom<WeaponState>())
                .RuleFor(x => x.AmmoInClip, f => f.Random.UInt())
                .RuleFor(x => x.AmmoInClip, f => f.Random.UInt())
                .RuleFor(x => x.Unknown, f => f.Random.Bool());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            Weapon x0 = GenerateTestObject(format);
            Weapon x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.AmmoInClip, x1.AmmoInClip);
            Assert.Equal(x0.AmmoInClip, x1.AmmoInClip);
            Assert.Equal(x0.Unknown, x1.Unknown);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestType(format), data.Length);
        }
    }
}
