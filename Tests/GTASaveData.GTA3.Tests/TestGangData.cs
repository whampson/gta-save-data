using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGangData : Base<GangData>
    {
        public override GangData GenerateTestObject(GTA3SaveParams p)
        {
            Faker<GangData> model = new Faker<GangData>()
                .RuleFor(x => x.Gangs, f => Generator.Array(GangData.MaxNumGangs, g => Generator.Generate<Gang, TestGang, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            GangData x0 = GenerateTestObject(p);
            GangData x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Gangs, x1.Gangs);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            GangData x0 = GenerateTestObject(p);
            GangData x1 = new GangData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
