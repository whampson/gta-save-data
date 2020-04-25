using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGangData : Base<GangData>
    {
        public override GangData GenerateTestObject(DataFormat format)
        {
            Faker<GangData> model = new Faker<GangData>()
                .RuleFor(x => x.Gangs, f => Generator.CreateArray(GangData.Limits.MaxNumGangs, g => Generator.Generate<Gang, TestGang>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            GangData x0 = GenerateTestObject();
            GangData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Gangs, x1.Gangs);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
