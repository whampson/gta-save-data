using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestScriptData : Base<ScriptData>
    {
        public override ScriptData GenerateTestObject(SaveFileFormat format)
        {
            Faker faker = new Faker();

            int varSpace = faker.Random.Int(4, 4000);
            int runningScripts = faker.Random.Int(1, 10);

            Faker<ScriptData> model = new Faker<ScriptData>()
                .RuleFor(x => x.GlobalSpace, f => Generator.CreateArray(varSpace, g => f.Random.Byte()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.Contacts, f => Generator.CreateArray(ScriptData.Limits.NumberOfContacts, g => Generator.Generate<Contact, TestContact>()))
                .RuleFor(x => x.Collectives, f => Generator.CreateArray(ScriptData.Limits.NumberOfCollectives, g => Generator.Generate<Collective, TestCollective>()))
                .RuleFor(x => x.NextFreeCollectiveIndex, f => f.Random.Int())
                .RuleFor(x => x.BuildingSwaps, f => Generator.CreateArray(ScriptData.Limits.NumberOfBuildingSwaps, g => Generator.Generate<BuildingSwap, TestBuildingSwap>()))
                .RuleFor(x => x.InvisibilitySettings, f => Generator.CreateArray(ScriptData.Limits.NumberOfInvisibilitySettings, g => Generator.Generate<InvisibleEntity, TestInvisibleEntity>()))
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
            ScriptData x0 = GenerateTestObject(format);
            ScriptData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.GlobalSpace, x1.GlobalSpace);
            Assert.Equal(x0.OnAMissionFlag, x1.OnAMissionFlag);
            Assert.Equal(x0.Contacts, x1.Contacts);
            Assert.Equal(x0.Collectives, x1.Collectives);
            Assert.Equal(x0.NextFreeCollectiveIndex, x1.NextFreeCollectiveIndex);
            Assert.Equal(x0.BuildingSwaps, x1.BuildingSwaps);
            Assert.Equal(x0.InvisibilitySettings, x1.InvisibilitySettings);
            Assert.Equal(x0.UsingAMultiScriptFile, x1.UsingAMultiScriptFile);
            Assert.Equal(x0.MainScriptSize, x1.MainScriptSize);
            Assert.Equal(x0.LargestMissionScriptSize, x1.LargestMissionScriptSize);
            Assert.Equal(x0.NumberOfMissionScripts, x1.NumberOfMissionScripts);
            Assert.Equal(x0.ActiveScripts, x1.ActiveScripts);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void GlobalVariables()
        {
            Faker f = new Faker();
            string path = TestData.GetTestDataPath(GameType.III, GTA3Save.FileFormats.PC, "CAT2");
            using GTA3Save x = SaveFile.Load<GTA3Save>(path, GTA3Save.FileFormats.PC);

            Assert.Equal(987.5, x.Scripts.GetGlobalAsFloat(804));

            int numGlobals = x.Scripts.NumberOfGlobalVariables;
            int i0 = f.Random.Int(0, numGlobals - 1);
            int i1 = f.Random.Int(0, numGlobals - 1);
            int v0 = f.Random.Int();
            float v1 = f.Random.Float();

            x.Scripts.SetGlobal(i0, v0);
            x.Scripts.SetGlobal(i1, v1);

            int r0 = x.Scripts.GetGlobal(i0);
            float r1 = x.Scripts.GetGlobalAsFloat(i1);

            Assert.Equal(v0, r0);
            Assert.Equal(v1, r1);
        }
    }
}
