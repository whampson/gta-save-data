using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TesttheScripts : GTA3SaveDataObjectTestBase<TheScripts>
    {
        public override TheScripts GenerateTestObject(SaveFileFormat format)
        {
            Faker faker = new Faker();

            int varSpace = faker.Random.Int(4, 4000);
            int runningScripts = faker.Random.Int(1, 10);

            Faker<TheScripts> model = new Faker<TheScripts>()
                .RuleFor(x => x.ScriptSpace, f => Generator.CreateArray(varSpace, g => f.Random.Byte()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.ContactArray, f => Generator.CreateArray(TheScripts.Limits.NumberOfContacts, g => Generator.Generate<ContactInfo, TestContactInfo>()))
                .RuleFor(x => x.CollectiveArray, f => Generator.CreateArray(TheScripts.Limits.NumberOfCollectives, g => Generator.Generate<Collective, TestCollective>()))
                .RuleFor(x => x.NextFreeCollectiveIndex, f => f.Random.Int())
                .RuleFor(x => x.BuildingSwapArray, f => Generator.CreateArray(TheScripts.Limits.NumberOfBuildingSwaps, g => Generator.Generate<StaticReplacement, TestStaticReplacement>()))
                .RuleFor(x => x.InvisibilitySettingArray, f => Generator.CreateArray(TheScripts.Limits.NumberOfInvisibilitySettings, g => Generator.Generate<InvisibleObject, TestInvisibleObject>()))
                .RuleFor(x => x.UsingAMultiScriptFile, f => f.Random.Bool())
                .RuleFor(x => x.MainScriptSize, f => f.Random.Int())
                .RuleFor(x => x.LargestMissionScriptSize, f => f.Random.Int())
                .RuleFor(x => x.NumberOfMissionScripts, f => f.Random.Short())
                .RuleFor(x => x.ActiveScripts, f => Generator.CreateArray(runningScripts, g => Generator.Generate<RunningScript, TestRunningScript>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void Serialization(SaveFileFormat format)
        {
            TheScripts x0 = GenerateTestObject(format);
            TheScripts x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.ScriptSpace, x1.ScriptSpace);
            Assert.Equal(x0.OnAMissionFlag, x1.OnAMissionFlag);
            Assert.Equal(x0.ContactArray, x1.ContactArray);
            Assert.Equal(x0.CollectiveArray, x1.CollectiveArray);
            Assert.Equal(x0.NextFreeCollectiveIndex, x1.NextFreeCollectiveIndex);
            Assert.Equal(x0.BuildingSwapArray, x1.BuildingSwapArray);
            Assert.Equal(x0.InvisibilitySettingArray, x1.InvisibilitySettingArray);
            Assert.Equal(x0.UsingAMultiScriptFile, x1.UsingAMultiScriptFile);
            Assert.Equal(x0.MainScriptSize, x1.MainScriptSize);
            Assert.Equal(x0.LargestMissionScriptSize, x1.LargestMissionScriptSize);
            Assert.Equal(x0.NumberOfMissionScripts, x1.NumberOfMissionScripts);
            Assert.Equal(x0.ActiveScripts, x1.ActiveScripts);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }
    }
}
