using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestWeapon : Base<Weapon>
    {
        public override Weapon GenerateTestObject(GTA3SaveParams p)
        {
            Faker<Weapon> model = new Faker<Weapon>()
                .RuleFor(x => x.Type, f => f.PickRandom<WeaponType>())
                .RuleFor(x => x.State, f => f.PickRandom<WeaponState>())
                .RuleFor(x => x.AmmoInClip, f => f.Random.Int())
                .RuleFor(x => x.AmmoInClip, f => f.Random.Int())
                .RuleFor(x => x.AddRotOffset, f => (!p.FileType.IsPS2) ? f.Random.Bool() : default);

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Weapon x0 = GenerateTestObject(p);
            Weapon x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.State, x1.State);
            Assert.Equal(x0.AmmoInClip, x1.AmmoInClip);
            Assert.Equal(x0.AmmoInClip, x1.AmmoInClip);
            Assert.Equal(x0.AddRotOffset, x1.AddRotOffset);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            Weapon x0 = GenerateTestObject(p);
            Weapon x1 = new Weapon(x0);

            Assert.Equal(x0, x1);
        }
    }
}
