using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestSaveData
        : SaveDataObjectTestBase<GTA3SaveData>
    {
        public override GTA3SaveData GenerateTestVector(SystemType system)
        {
            Faker faker = new Faker();

            Faker<GTA3SaveData> model = new Faker<GTA3SaveData>()
                .RuleFor(x => x.SimpleVars, f => TestHelper.Generate<SimpleVars, TestSimpleVars>(system))
                .RuleFor(x => x.Scripts, TestHelper.Generate<Scripts, TestScripts>(system));

            return model.Generate();
        }

        [Theory]
        [InlineData(SystemType.Android)]
        [InlineData(SystemType.IOS)]
        [InlineData(SystemType.PC)]
        [InlineData(SystemType.PS2)]
        [InlineData(SystemType.PS2AU)]
        [InlineData(SystemType.PS2JP)]
        [InlineData(SystemType.Xbox)]
        public void Serialization(SystemType system)
        {
            GTA3SaveData x0 = GenerateTestVector(system);
            GTA3SaveData x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data, system);

            Assert.Equal(x0, x1);
            // TODO: data size check?
        }
    }
}
