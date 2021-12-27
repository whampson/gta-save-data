using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPedPool : Base<PedPool>
    {
        public override PedPool GenerateTestObject(GTA3SaveParams p)
        {
            Faker<PedPool> model = new Faker<PedPool>()
                .RuleFor(x => x.PlayerPeds, f => Generator.Array(f.Random.Int(1, 3),
                    g =>
                    {
                        var ped = Generator.Generate<PlayerPed, TestPlayerPed, GTA3SaveParams>(p);
                        ped.Type = f.PickRandom<PedTypeId>();
                        ped.ModelIndex = f.Random.Short();
                        ped.Handle = f.Random.Int();
                        ped.MaxWantedLevel = f.Random.Int();
                        ped.MaxChaosLevel = f.Random.Int();
                        ped.ModelName = Generator.AsciiString(f, PlayerPed.MaxModelNameLength - 1);
                        return ped;
                    }));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PedPool x0 = GenerateTestObject(p);
            PedPool x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.PlayerPeds, x1.PlayerPeds);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PedPool x0 = GenerateTestObject(p);
            PedPool x1 = new PedPool(x0);

            Assert.Equal(x0, x1);
        }
    }
}
