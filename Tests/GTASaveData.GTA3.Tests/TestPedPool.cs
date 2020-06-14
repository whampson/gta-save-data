using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPedPool : Base<PlayerPedPool>
    {
        public override PlayerPedPool GenerateTestObject(FileFormat format)
        {
            Faker<PlayerPedPool> model = new Faker<PlayerPedPool>()
                .RuleFor(x => x.PlayerPeds, f => Generator.Array(f.Random.Int(1, 3), g => Generator.Generate<PlayerPed, TestPlayerPed>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            PlayerPedPool x0 = GenerateTestObject(format);
            PlayerPedPool x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.PlayerPeds, x1.PlayerPeds);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            PlayerPedPool x0 = GenerateTestObject();
            PlayerPedPool x1 = new PlayerPedPool(x0);

            Assert.Equal(x0, x1);
        }
    }
}
