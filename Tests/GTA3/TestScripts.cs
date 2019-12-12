using Bogus;
using GTASaveData.Common;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestScripts
    {
        public static Scripts Generate(bool isPS2 = false)
        {
            Faker faker = new Faker();

            int numGlobals = faker.Random.Int(100, 5000);
            int numRunningScripts = faker.Random.Int(1, 100);

            Faker<Scripts> model = new Faker<Scripts>()
                .RuleFor(x => x.GlobalVariables, f => TestHelper.CreateCollection(numGlobals, e => f.Random.UInt()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.Contacts, f => TestHelper.CreateStructCollection(Scripts.ContactCount, e => TestContactInfo.Generate()))
                .RuleFor(x => x.Collectives, f => TestHelper.CreateStructCollection(Scripts.CollectiveCount, e => TestCollective.Generate()))
                .RuleFor(x => x.NextFreeCollectiveIndex, f => f.Random.Int())
                .RuleFor(x => x.BuildingSwaps, f => TestHelper.CreateStructCollection(Scripts.BuildingSwapCount, e => TestBuildingSwap.Generate()))
                .RuleFor(x => x.InvisibilitySettings, f => TestHelper.CreateStructCollection(Scripts.InvisibilitySettingCount, e => TestInvisibilitySetting.Generate()))
                .RuleFor(x => x.UsingAMultiScriptFile, f => f.Random.Bool())
                .RuleFor(x => x.MainScriptSize, f => f.Random.Int())
                .RuleFor(x => x.LargestMissionScriptSize, f => f.Random.Int())
                .RuleFor(x => x.NumberOfMissionScripts, f => f.Random.Short())
                .RuleFor(x => x.RunningScripts, f => TestHelper.CreateStructCollection(numRunningScripts, x => TestRunningScript.Generate(isPS2)));

            return model.Generate();
        }

        [Fact]
        public void Sanity()
        {
            Scripts scr0 = Generate();
            Scripts scr1 = TestHelper.CreateSerializedCopy(scr0);

            Assert.Equal(scr0, scr1);
            //Assert.Equal(scr0.GlobalVariables, scr1.GlobalVariables);
            //Assert.Equal(scr0.OnAMissionFlag, scr1.OnAMissionFlag);
            //Assert.Equal(scr0.Contacts, scr1.Contacts);
            //Assert.Equal(scr0.Collectives, scr1.Collectives);
            //Assert.Equal(scr0.NextFreeCollectiveIndex, scr1.NextFreeCollectiveIndex);
            //Assert.Equal(scr0.BuildingSwaps, scr1.BuildingSwaps);
            //Assert.Equal(scr0.InvisibilitySettings, scr1.InvisibilitySettings);
            //Assert.Equal(scr0.UsingAMultiScriptFile, scr1.UsingAMultiScriptFile);
            //Assert.Equal(scr0.MainScriptSize, scr1.MainScriptSize);
            //Assert.Equal(scr0.OnAMissionFlag, scr1.OnAMissionFlag);
            //Assert.Equal(scr0.LargestMissionScriptSize, scr1.LargestMissionScriptSize);
            //Assert.Equal(scr0.NumberOfMissionScripts, scr1.NumberOfMissionScripts);
            //Assert.Equal(scr0.RunningScripts, scr1.RunningScripts);
        }

        [Fact]
        public void SanityPS2()
        {
            Scripts scr0 = Generate(isPS2: true);
            Scripts scr1 = TestHelper.CreateSerializedCopy(scr0, SystemType.PS2);

            Assert.Equal(scr0, scr1);
            //Assert.Equal(scr0.GlobalVariables, scr1.GlobalVariables);
            //Assert.Equal(scr0.OnAMissionFlag, scr1.OnAMissionFlag);
            //Assert.Equal(scr0.Contacts, scr1.Contacts);
            //Assert.Equal(scr0.Collectives, scr1.Collectives);
            //Assert.Equal(scr0.NextFreeCollectiveIndex, scr1.NextFreeCollectiveIndex);
            //Assert.Equal(scr0.BuildingSwaps, scr1.BuildingSwaps);
            //Assert.Equal(scr0.InvisibilitySettings, scr1.InvisibilitySettings);
            //Assert.Equal(scr0.UsingAMultiScriptFile, scr1.UsingAMultiScriptFile);
            //Assert.Equal(scr0.MainScriptSize, scr1.MainScriptSize);
            //Assert.Equal(scr0.OnAMissionFlag, scr1.OnAMissionFlag);
            //Assert.Equal(scr0.LargestMissionScriptSize, scr1.LargestMissionScriptSize);
            //Assert.Equal(scr0.NumberOfMissionScripts, scr1.NumberOfMissionScripts);
            //Assert.Equal(scr0.RunningScripts, scr1.RunningScripts);
        }
    }
}
