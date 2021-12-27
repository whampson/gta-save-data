using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestZoneInfo : Base<ZoneInfo>
    {
        public override ZoneInfo GenerateTestObject(GTA3SaveParams p)
        {
            Faker<ZoneInfo> model = new Faker<ZoneInfo>()
                .RuleFor(x => x.CarDensity, f => f.Random.Short())
                .RuleFor(x => x.CarThreshold, f => Generator.Array(ZoneInfo.CarThresholdCapacity, g => f.Random.Short()))
                .RuleFor(x => x.CopCarDensity, f => f.Random.Short())
                .RuleFor(x => x.GangCarDensity, f => Generator.Array(ZoneInfo.GangDensityCapacity, g => f.Random.Short()))
                .RuleFor(x => x.PedDensity, f => f.Random.Short())
                .RuleFor(x => x.CopPedDensity, f => f.Random.Short())
                .RuleFor(x => x.GangPedDensity, f => Generator.Array(ZoneInfo.GangDensityCapacity, g => f.Random.Short()))
                .RuleFor(x => x.PedGroup, f => f.Random.Short());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            ZoneInfo x0 = GenerateTestObject(p);
            ZoneInfo x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.CarDensity, x1.CarDensity);
            Assert.Equal(x0.CarThreshold, x1.CarThreshold);
            Assert.Equal(x0.CopCarDensity, x1.CopCarDensity);
            Assert.Equal(x0.GangCarDensity, x1.GangCarDensity);
            Assert.Equal(x0.PedDensity, x1.PedDensity);
            Assert.Equal(x0.CopPedDensity, x1.CopPedDensity);
            Assert.Equal(x0.GangPedDensity, x1.GangPedDensity);
            Assert.Equal(x0.PedGroup, x1.PedGroup);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            ZoneInfo x0 = GenerateTestObject(p);
            ZoneInfo x1 = new ZoneInfo(x0);

            Assert.Equal(x0, x1);
        }
    }
}
