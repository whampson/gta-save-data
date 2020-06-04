using Bogus;
using System;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPlayerPed : Base<PlayerPed>
    {
        public override PlayerPed GenerateTestObject(FileFormat format)
        {
            Faker<PlayerPed> model = new Faker<PlayerPed>()
                .RuleFor(x => x.Position, f => Generator.Vector3D(f))
                .RuleFor(x => x.CreatedBy, f => f.PickRandom<CharCreatedBy>())
                .RuleFor(x => x.Health, f => f.Random.Float())
                .RuleFor(x => x.Armor, f => f.Random.Float())
                .RuleFor(x => x.Weapons, f => Generator.Array(PlayerPed.Limits.NumWeapons, g => Generator.Generate<Weapon, TestWeapon>(format)))
                .RuleFor(x => x.MaxWeaponTypeAllowed, f => f.Random.Byte())
                .RuleFor(x => x.MaxStamina, f => f.Random.Float())
                .RuleFor(x => x.TargetableObjects, f => Generator.Array(PlayerPed.Limits.NumTargetableObjects, g => f.Random.Int()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            PlayerPed x0 = GenerateTestObject(format);
            PlayerPed x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.CreatedBy, x1.CreatedBy);
            Assert.Equal(x0.Health, x1.Health);
            Assert.Equal(x0.Armor, x1.Armor);
            Assert.Equal(x0.Weapons, x1.Weapons);
            Assert.Equal(x0.MaxWeaponTypeAllowed, x1.MaxWeaponTypeAllowed);
            Assert.Equal(x0.MaxStamina, x1.MaxStamina);
            Assert.Equal(x0.TargetableObjects, x1.TargetableObjects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }
    }
}