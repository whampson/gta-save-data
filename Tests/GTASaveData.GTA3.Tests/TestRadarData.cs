using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRadarData : Base<RadarData>
    {
        public override RadarData GenerateTestObject(DataFormat format)
        {
            Faker<RadarData> model = new Faker<RadarData>()
                .RuleFor(x => x.RadarBlips,
                    f => Generator.CreateArray(RadarData.Limits.MaxNumRadarBlips, g => Generator.Generate<RadarBlip, TestRadarBlip>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            RadarData x0 = GenerateTestObject();
            RadarData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.RadarBlips, x1.RadarBlips);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
