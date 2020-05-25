using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPedPool : Base<PedPool>
    {
        public override PedPool GenerateTestObject(SaveDataFormat format)
        {
            Faker<PedPool> model = new Faker<PedPool>()
                .RuleFor(x => x.PlayerPeds, f => Generator.Array(f.Random.Int(1, 25), g => Generator.Generate<PlayerPed, TestPlayerPed>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(SaveDataFormat format)
        {
            PedPool x0 = GenerateTestObject(format);
            PedPool x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.PlayerPeds, x1.PlayerPeds);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }
    }
}
