using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestScripts
        : SaveDataObjectTestBase<Scripts>
    {
        public override Scripts GenerateTestVector(SystemType system)
        {
            Faker faker = new Faker();

            int numGlobals = faker.Random.Int(100, 5000);
            int numRunningScripts = faker.Random.Int(1, 100);

            Faker<Scripts> model = new Faker<Scripts>()
                .RuleFor(x => x.GlobalVariables, f => TestHelper.CreateValueCollection(numGlobals, e => f.Random.UInt()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.Contacts, f => TestHelper.CreateObjectCollection(Scripts.ContactCount, e => TestHelper.Generate<ContactInfo, TestContactInfo>()))
                .RuleFor(x => x.Collectives, f => TestHelper.CreateObjectCollection(Scripts.CollectiveCount, e => TestHelper.Generate<Collective, TestCollective>()))
                .RuleFor(x => x.NextFreeCollectiveIndex, f => f.Random.Int())
                .RuleFor(x => x.BuildingSwaps, f => TestHelper.CreateObjectCollection(Scripts.BuildingSwapCount, e => TestHelper.Generate<BuildingSwap, TestBuildingSwap>()))
                .RuleFor(x => x.InvisibilitySettings, f => TestHelper.CreateObjectCollection(Scripts.InvisibilitySettingCount, e => TestHelper.Generate<InvisibilitySetting, TestInvisibilitySetting>()))
                .RuleFor(x => x.UsingAMultiScriptFile, f => f.Random.Bool())
                .RuleFor(x => x.MainScriptSize, f => f.Random.Int())
                .RuleFor(x => x.LargestMissionScriptSize, f => f.Random.Int())
                .RuleFor(x => x.NumberOfMissionScripts, f => f.Random.Short())
                .RuleFor(x => x.RunningScripts, f => TestHelper.CreateObjectCollection(numRunningScripts, x => TestHelper.Generate<RunningScript, TestRunningScript>(system)));

            return model.Generate();
        }

        [Theory]
        [InlineData(SystemType.Android)]
        [InlineData(SystemType.IOS)]
        [InlineData(SystemType.PC)]
        [InlineData(SystemType.PS2)]
        [InlineData(SystemType.Xbox)]
        public void Serialization(SystemType system)
        {
            Scripts x0 = GenerateTestVector(system);
            Scripts x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data, system);

            Assert.Equal(x0, x1);
            // TODO: size check?
        }
    }
}
