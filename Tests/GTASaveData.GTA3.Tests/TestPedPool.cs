using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPedPool : Base<PedPool>
    {
        public override PedPool GenerateTestObject(FileFormat format)
        {
            Faker<PedPool> model = new Faker<PedPool>()
                .RuleFor(x => x.PlayerPeds, f => Generator.Array(f.Random.Int(1, 3),
                    g =>
                    {
                        var ped = Generator.Generate<PlayerPed, TestPlayerPed>(format);
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
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            PedPool x0 = GenerateTestObject(format);
            PedPool x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.PlayerPeds, x1.PlayerPeds);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            PedPool x0 = GenerateTestObject();
            PedPool x1 = new PedPool(x0);

            Assert.Equal(x0, x1);
        }
    }
}
