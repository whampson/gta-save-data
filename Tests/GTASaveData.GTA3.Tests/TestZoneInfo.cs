using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestZoneInfo : Base<ZoneInfo>
    {
        public override ZoneInfo GenerateTestObject(SaveDataFormat format)
        {
            Faker<ZoneInfo> model = new Faker<ZoneInfo>()
                .RuleFor(x => x.CarDensity, f => f.Random.Short())
                .RuleFor(x => x.CarThreshold, f => Generator.Array(ZoneInfo.Limits.CarThresholdCapacity, g => f.Random.Short()))
                .RuleFor(x => x.CopCarDensity, f => f.Random.Short())
                .RuleFor(x => x.GangCarDensity, f => Generator.Array(ZoneInfo.Limits.GangDensityCapacity, g => f.Random.Short()))
                .RuleFor(x => x.PedDensity, f => f.Random.Short())
                .RuleFor(x => x.CopPedDensity, f => f.Random.Short())
                .RuleFor(x => x.GangPedDensity, f => Generator.Array(ZoneInfo.Limits.GangDensityCapacity, g => f.Random.Short()))
                .RuleFor(x => x.PedGroup, f => f.Random.Short());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            ZoneInfo x0 = GenerateTestObject();
            ZoneInfo x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.CarDensity, x1.CarDensity);
            Assert.Equal(x0.CarThreshold, x1.CarThreshold);
            Assert.Equal(x0.CopCarDensity, x1.CopCarDensity);
            Assert.Equal(x0.GangCarDensity, x1.GangCarDensity);
            Assert.Equal(x0.PedDensity, x1.PedDensity);
            Assert.Equal(x0.CopPedDensity, x1.CopPedDensity);
            Assert.Equal(x0.GangPedDensity, x1.GangPedDensity);
            Assert.Equal(x0.PedGroup, x1.PedGroup);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
