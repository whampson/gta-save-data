using Bogus;
using GTASaveData.Core.Tests;
using GTASaveData.Types;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestScriptBlock : Base<ScriptsBlock>
    {
        public override ScriptsBlock GenerateTestObject(FileFormat format)
        {
            Faker faker = new Faker();

            int varSpace = faker.Random.Int(4, 8000);
            int runningScripts = faker.Random.Int(1, 20);

            Faker<ScriptsBlock> model = new Faker<ScriptsBlock>()
                .RuleFor(x => x.ScriptSpace, f => Generator.Array(varSpace, g => f.Random.Byte()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.Contacts, f => Generator.Array(ScriptsBlock.NumContacts, g => Generator.Generate<Contact, TestContact>()))
                .RuleFor(x => x.Collectives, f => Generator.Array(ScriptsBlock.NumCollectives, g => Generator.Generate<Collective, TestCollective>()))
                .RuleFor(x => x.NextFreeCollectiveIndex, f => f.Random.Int())
                .RuleFor(x => x.BuildingSwaps, f => Generator.Array(ScriptsBlock.NumBuildingSwaps, g => Generator.Generate<BuildingSwap, TestBuildingSwap>()))
                .RuleFor(x => x.InvisibilitySettings, f => Generator.Array(ScriptsBlock.NumInvisibilitySettings, g => Generator.Generate<InvisibleObject, TestInvisibleObject>()))
                .RuleFor(x => x.UsingAMultiScriptFile, f => f.Random.Bool())
                .RuleFor(x => x.MainScriptSize, f => f.Random.Int())
                .RuleFor(x => x.LargestMissionScriptSize, f => f.Random.Int())
                .RuleFor(x => x.NumberOfMissionScripts, f => f.Random.Short())
                .RuleFor(x => x.RunningScripts, f => Generator.Array(runningScripts, g => Generator.Generate<RunningScript, TestRunningScript>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            ScriptsBlock x0 = GenerateTestObject(format);
            ScriptsBlock x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.ScriptSpace, x1.ScriptSpace);
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
            Assert.Equal(x0.RunningScripts, x1.RunningScripts);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            ScriptsBlock x0 = GenerateTestObject();
            ScriptsBlock x1 = new ScriptsBlock(x0);

            Assert.Equal(x0, x1);

            // Prove that deep copy actually happened
            x0.RunningScripts[0].IP = 6969;
            Assert.NotEqual(x0.RunningScripts[0], x1.RunningScripts[0]);
        }

        [Fact]
        public void GlobalVariables()
        {
            Faker f = new Faker();
            string path = TestData.GetTestDataPath(Game.GTA3, SaveFileGTA3.FileFormats.PC, "CAT2");
            using SaveFileGTA3 x = SaveFile.Load<SaveFileGTA3>(path, SaveFileGTA3.FileFormats.PC);

            Assert.Equal(987.5, x.Scripts.GetGlobalAsFloat(804));

            int numGlobals = x.Scripts.Globals.Count();
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

        [Fact]
        public void ScriptSpaceReadWrite()
        {
            string path = TestData.GetTestDataPath(Game.GTA3, SaveFileGTA3.FileFormats.PC, "CAT2");
            using SaveFileGTA3 x = SaveFile.Load<SaveFileGTA3>(path, SaveFileGTA3.FileFormats.PC);

            byte b = 0xA5;
            short s = unchecked((short) 0xCCEE);
            int i = unchecked((int) 0xCAFEBABE);
            float f = 435.5625f;
            string l = "FUCKER";
            int offset = 420;

            offset = x.Scripts.Write1ByteToScript(offset, b);
            offset = x.Scripts.Write2BytesToScript(offset, s);
            offset = x.Scripts.Write4BytesToScript(offset, i);
            offset = x.Scripts.WriteFloatToScript(offset, f);
            offset = x.Scripts.WriteTextLabelToScript(offset, l);
            Assert.Equal(437, offset);

            offset = 420;
            offset = x.Scripts.Read1ByteFromScript(offset, out byte b2);
            offset = x.Scripts.Read2BytesFromScript(offset, out short s2);
            offset = x.Scripts.Read4BytesFromScript(offset, out int i2);
            offset = x.Scripts.ReadFloatFromScript(offset, out float f2);
            offset = x.Scripts.ReadTextLabelFromScript(offset, out string l2);
            Assert.Equal(437, offset);
            Assert.Equal(b, b2);
            Assert.Equal(s, s2);
            Assert.Equal(i, i2);
            Assert.Equal(f, f2);
            Assert.Equal(l, l2);
        }

        [Fact]
        public void GrowShrinkScriptSpace()
        {
            string path = TestData.GetTestDataPath(Game.GTA3, SaveFileGTA3.FileFormats.PC, "CAT2");
            using SaveFileGTA3 x = SaveFile.Load<SaveFileGTA3>(path, SaveFileGTA3.FileFormats.PC);

            var origScriptSpace = x.Scripts.ScriptSpace;
            int origSize = origScriptSpace.Count;
            int amount = 1000;

            int newSize = x.Scripts.ResizeScriptSpace(amount);    // grow
            Assert.Equal(origSize + amount, x.Scripts.ScriptSpace.Count);
            Assert.Equal(origSize + amount, newSize);

            newSize = x.Scripts.ResizeScriptSpace(-amount);   // shrink
            Assert.Equal(origSize, x.Scripts.ScriptSpace.Count);
            Assert.Equal(origSize, newSize);

            Assert.Equal(origScriptSpace, x.Scripts.ScriptSpace);
            Assert.True(origScriptSpace != x.Scripts.ScriptSpace);
        }
    }
}
