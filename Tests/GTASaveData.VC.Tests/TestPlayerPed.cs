using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.VC.Tests
{
    public class TestPlayerPed : Base<PlayerPed>
    {
        public override PlayerPed GenerateTestObject(FileFormat format)
        {
            int numTargetableObjects = (format.IsMobile) ? PlayerPed.NumTargetableObjectsMobile : PlayerPed.NumTargetableObjects;
            Faker<PlayerPed> model = new Faker<PlayerPed>()

                /* These fields are saved in the PedPool structure that wraps the PlayerPed struct,
                   not in the PlayerPed struct itself */
                //.RuleFor(x => x.Type, f => f.PickRandom<PedTypeId>())
                //.RuleFor(x => x.ModelIndex, f => f.Random.Short())
                //.RuleFor(x => x.Handle, f => f.Random.Int())

                .RuleFor(x => x.Position, f => Generator.Vector3(f))
                .RuleFor(x => x.CreatedBy, f => f.PickRandom<CharCreatedBy>())
                .RuleFor(x => x.Health, f => f.Random.Float())
                .RuleFor(x => x.Armor, f => f.Random.Float())
                .RuleFor(x => x.Weapons, f => Generator.Array(PlayerPed.NumWeapons, g => Generator.Generate<Weapon, TestWeapon>(format)))
                .RuleFor(x => x.MaxStamina, f => f.Random.Float())
                .RuleFor(x => x.TargetableObjects, f => Generator.Array(numTargetableObjects, g => f.Random.Int()));

                /* Ditto. */
                //.RuleFor(x => x.MaxWantedLevel, f => f.Random.Int())
                //.RuleFor(x => x.MaxChaosLevel, f => f.Random.Int())
                //.RuleFor(x => x.ModelName, f => Generator.AsciiString(f, PlayerPed.MaxModelNameLength - 1));

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
            Assert.Equal(x0.MaxStamina, x1.MaxStamina);
            Assert.Equal(x0.TargetableObjects, x1.TargetableObjects);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            PlayerPed x0 = GenerateTestObject();
            PlayerPed x1 = new PlayerPed(x0);

            Assert.Equal(x0, x1);
        }
    }
}