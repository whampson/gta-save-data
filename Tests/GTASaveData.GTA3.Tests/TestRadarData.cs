using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRadarData : Base<RadarData>
    {
        public override RadarData GenerateTestObject(FileType format)
        {
            Faker<RadarData> model = new Faker<RadarData>()
                .RuleFor(x => x.RadarBlips,
                    f => Generator.Array(RadarData.MaxNumRadarBlips, g => Generator.Generate<RadarBlip, TestRadarBlip>()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            RadarData x0 = GenerateTestObject(format);
            RadarData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.RadarBlips, x1.RadarBlips);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            RadarData x0 = GenerateTestObject();
            RadarData x1 = new RadarData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
