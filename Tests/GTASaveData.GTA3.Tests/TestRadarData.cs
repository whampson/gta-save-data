using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRadarData : Base<RadarData>
    {
        public override RadarData GenerateTestObject(GTA3SaveParams p)
        {
            Faker<RadarData> model = new Faker<RadarData>()
                .RuleFor(x => x.RadarBlips,
                    f => Generator.Array(RadarData.MaxNumRadarBlips, g => Generator.Generate<RadarBlip, TestRadarBlip, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            RadarData x0 = GenerateTestObject(p);
            RadarData x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.RadarBlips, x1.RadarBlips);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            RadarData x0 = GenerateTestObject(p);
            RadarData x1 = new RadarData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
