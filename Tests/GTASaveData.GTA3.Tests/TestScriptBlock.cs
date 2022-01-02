using Bogus;
using GTASaveData.Core.Tests;
using GTASaveData.Types;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestScriptBlock : Base<ScriptBlock>
    {
        public override ScriptBlock GenerateTestObject(GTA3SaveParams p)
        {
            Faker faker = new Faker();

            int varSpace = faker.Random.Int(4, 8000);
            int runningScripts = faker.Random.Int(1, 20);

            Faker<ScriptBlock> model = new Faker<ScriptBlock>()
                .RuleFor(x => x.ScriptSpace, f => Generator.Array(varSpace, g => f.Random.Byte()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.Contacts, f => Generator.Array(p.NumContacts, g => Generator.Generate<Contact, TestContact, GTA3SaveParams>(p)))
                .RuleFor(x => x.Collectives, f => Generator.Array(p.NumCollectives, g => Generator.Generate<Collective, TestCollective, SerializationParams>(p)))
                .RuleFor(x => x.NextFreeCollectiveIndex, f => f.Random.Int())
                .RuleFor(x => x.BuildingSwaps, f => Generator.Array(p.NumBuildingSwaps, g => Generator.Generate<BuildingSwap, TestBuildingSwap, SerializationParams>(p)))
                .RuleFor(x => x.InvisibilitySettings, f => Generator.Array(p.NumInvisibilitySettings, g => Generator.Generate<InvisibleObject, TestInvisibleObject, SerializationParams>(p)))
                .RuleFor(x => x.UsingAMultiScriptFile, f => f.Random.Bool())
                .RuleFor(x => x.MainScriptSize, f => f.Random.Int())
                .RuleFor(x => x.LargestMissionScriptSize, f => f.Random.Int())
                .RuleFor(x => x.NumberOfMissionScripts, f => f.Random.Short())
                .RuleFor(x => x.RunningScripts, f => Generator.Array(runningScripts, g => Generator.Generate<RunningScript, TestRunningScript, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            ScriptBlock x0 = GenerateTestObject(p);
            ScriptBlock x1 = CreateSerializedCopy(x0, p, out byte[] data);

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
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            ScriptBlock x0 = GenerateTestObject(p);
            ScriptBlock x1 = new ScriptBlock(x0);

            Assert.Equal(x0, x1);

            // Prove that deep copy actually happened
            x0.RunningScripts[0].IP = 6969;
            Assert.NotEqual(x0.RunningScripts[0], x1.RunningScripts[0]);
        }

        [Fact]
        public void SerializationParams()
        {
            Faker faker = new Faker();
            GTA3SaveParams p = GTA3SaveParams.GetDefaults();

            p.NumContacts = faker.Random.Int(0, 100);
            p.NumCollectives = faker.Random.Int(0, 100);
            p.NumBuildingSwaps = faker.Random.Int(0, 100);
            p.NumInvisibilitySettings = faker.Random.Int(0, 100);

            ScriptBlock x0 = GenerateTestObject(p);
            ScriptBlock x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Fact]
        public void GlobalVariables()
        {
            Faker f = new Faker();
            string path = TestData.GetTestDataPath(Game.GTA3, GTA3Save.FileTypes.PC, "CAT2");
            using GTA3Save x = GTA3Save.Load(path, GTA3Save.FileTypes.PC);

            Assert.Equal(987.5, x.Script.GetGlobalAsFloat(804));

            int numGlobals = x.Script.Globals.Count();
            int i0 = f.Random.Int(0, numGlobals - 1);
            int i1 = f.Random.Int(0, numGlobals - 1);
            int v0 = f.Random.Int();
            float v1 = f.Random.Float();

            x.Script.SetGlobal(i0, v0);
            x.Script.SetGlobal(i1, v1);

            int r0 = x.Script.GetGlobal(i0);
            float r1 = x.Script.GetGlobalAsFloat(i1);

            Assert.Equal(v0, r0);
            Assert.Equal(v1, r1);
        }

        [Fact]
        public void ScriptSpaceReadWrite()
        {
            string path = TestData.GetTestDataPath(Game.GTA3, GTA3Save.FileTypes.PC, "CAT2");
            using GTA3Save x = GTA3Save.Load(path, GTA3Save.FileTypes.PC);

            byte b = 0xA5;
            short s = unchecked((short) 0xCCEE);
            int i = unchecked((int) 0xCAFEBABE);
            float f = 435.5625f;
            string l = "FUCKER";
            int addr = 420;

            addr += x.Script.Write1ByteToScript(addr, b);
            addr += x.Script.Write2BytesToScript(addr, s);
            addr += x.Script.Write4BytesToScript(addr, i);
            addr += x.Script.WriteFloatToScript(addr, f);
            addr += x.Script.WriteTextLabelToScript(addr, l);
            Assert.Equal(437, addr);

            addr = 420;
            addr += x.Script.Read1ByteFromScript(addr, out byte b2);
            addr += x.Script.Read2BytesFromScript(addr, out short s2);
            addr += x.Script.Read4BytesFromScript(addr, out int i2);
            addr += x.Script.ReadFloatFromScript(addr, out float f2);
            addr += x.Script.ReadTextLabelFromScript(addr, out string l2);
            Assert.Equal(437, addr);
            Assert.Equal(b, b2);
            Assert.Equal(s, s2);
            Assert.Equal(i, i2);
            Assert.Equal(f, f2);
            Assert.Equal(l, l2);
        }

        [Fact]
        public void GrowShrinkScriptSpace()
        {
            string path = TestData.GetTestDataPath(Game.GTA3, GTA3Save.FileTypes.PC, "CAT2");
            using GTA3Save x = GTA3Save.Load(path, GTA3Save.FileTypes.PC);

            var origScriptSpace = x.Script.ScriptSpace;
            int origSize = origScriptSpace.Count;
            int amount = 1000;

            int newSize = x.Script.GrowScriptSpace(amount);
            Assert.Equal(origSize + amount, x.Script.ScriptSpace.Count);
            Assert.Equal(origSize + amount, newSize);

            newSize = x.Script.ShrinkScriptSpace(amount);
            Assert.Equal(origSize, x.Script.ScriptSpace.Count);
            Assert.Equal(origSize, newSize);

            Assert.Equal(origScriptSpace, x.Script.ScriptSpace);
            Assert.True(origScriptSpace != x.Script.ScriptSpace);
        }
    }
}
