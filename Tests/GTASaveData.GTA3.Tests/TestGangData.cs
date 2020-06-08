using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGangData : Base<GangData>
    {
        public override GangData GenerateTestObject(FileFormat format)
        {
            Faker<GangData> model = new Faker<GangData>()
                .RuleFor(x => x.Gangs, f => Generator.Array(GangData.MaxNumGangs, g => Generator.Generate<Gang, TestGang>()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            GangData x0 = GenerateTestObject(format);
            GangData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Gangs, x1.Gangs);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            GangData x0 = GenerateTestObject();
            GangData x1 = new GangData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
