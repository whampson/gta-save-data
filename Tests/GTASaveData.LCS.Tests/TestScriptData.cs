﻿using Bogus;
using TestFramework;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.LCS.Tests
{
    public class TestScriptData : Base<ScriptData>
    {
        public override ScriptData GenerateTestObject(FileFormat format)
        {
            Faker faker = new Faker();

            int varSpace = faker.Random.Int(4, 8000);
            int runningScripts = faker.Random.Int(1, 20);

            Faker<ScriptData> model = new Faker<ScriptData>()
                .RuleFor(x => x.ScriptSpace, f => Generator.Array(varSpace, g => f.Random.Byte()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.LastMissionPassedTime, f => f.Random.UInt())
                .RuleFor(x => x.Collectives, f => Generator.Array(ScriptData.NumCollectives, g => Generator.Generate<Collective, TestCollective>()))
                .RuleFor(x => x.NextFreeCollectiveIndex, f => f.Random.Int())
                .RuleFor(x => x.BuildingSwaps, f => Generator.Array(ScriptData.NumBuildingSwaps, g => Generator.Generate<BuildingSwap, TestBuildingSwap>()))
                .RuleFor(x => x.InvisibilitySettings, f => Generator.Array(ScriptData.NumInvisibilitySettings, g => Generator.Generate<InvisibleObject, TestInvisibleObject>()))
                .RuleFor(x => x.UsingAMultiScriptFile, f => f.Random.Bool())
                .RuleFor(x => x.PlayerHasMetDebbieHarry, f => f.Random.Bool())
                .RuleFor(x => x.MainScriptSize, f => f.Random.Int())
                .RuleFor(x => x.LargestMissionScriptSize, f => f.Random.Int())
                .RuleFor(x => x.NumberOfMissionScripts, f => f.Random.Short())
                .RuleFor(x => x.ActiveScripts, f => Generator.Array(runningScripts, g => Generator.Generate<RunningScript, TestRunningScript>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            ScriptData x0 = GenerateTestObject(format);
            ScriptData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.ScriptSpace, x1.ScriptSpace);
            Assert.Equal(x0.OnAMissionFlag, x1.OnAMissionFlag);
            Assert.Equal(x0.LastMissionPassedTime, x1.LastMissionPassedTime);
            Assert.Equal(x0.Collectives, x1.Collectives);
            Assert.Equal(x0.NextFreeCollectiveIndex, x1.NextFreeCollectiveIndex);
            Assert.Equal(x0.BuildingSwaps, x1.BuildingSwaps);
            Assert.Equal(x0.InvisibilitySettings, x1.InvisibilitySettings);
            Assert.Equal(x0.UsingAMultiScriptFile, x1.UsingAMultiScriptFile);
            Assert.Equal(x0.PlayerHasMetDebbieHarry, x1.PlayerHasMetDebbieHarry);
            Assert.Equal(x0.MainScriptSize, x1.MainScriptSize);
            Assert.Equal(x0.LargestMissionScriptSize, x1.LargestMissionScriptSize);
            Assert.Equal(x0.NumberOfMissionScripts, x1.NumberOfMissionScripts);
            Assert.Equal(x0.ActiveScripts, x1.ActiveScripts);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            ScriptData x0 = GenerateTestObject();
            ScriptData x1 = new ScriptData(x0);

            Assert.Equal(x0, x1);

            // Prove that deep copy actually happened
            x0.ActiveScripts[0].IP = 6969;
            Assert.NotEqual(x0.ActiveScripts[0], x1.ActiveScripts[0]);
        }

        [Fact]
        public void GlobalVariables()
        {
            Faker f = new Faker();
            string path = TestData.GetTestDataPath(Game.LCS, LCSSave.FileFormats.PS2, "NEDS4");
            LCSSave x = SaveData.Load<LCSSave>(path, LCSSave.FileFormats.PS2);

            // TODO: fix for LCS
            Assert.Equal(272.1489f, x.Scripts.GetGlobalAsFloat(7));

            int numGlobals = x.Scripts.NumGlobalVariables;
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
        public void ScriptSpace()
        {
            string path = TestData.GetTestDataPath(Game.LCS, LCSSave.FileFormats.PS2, "NEDS4");
            LCSSave x = SaveData.Load<LCSSave>(path, LCSSave.FileFormats.PS2);

            byte b = 0xA5;
            ushort s = 0xCCEE;
            uint i = 0xCAFEBABE;
            float f = 133.7f;
            int offset = 420;

            offset += x.Scripts.Write1ByteToScript(offset, b);
            offset += x.Scripts.Write2BytesToScript(offset, s);
            offset += x.Scripts.Write4BytesToScript(offset, i);
            offset += x.Scripts.WriteFloatToScript(offset, f);
            Assert.Equal(431, offset);

            offset = 420;
            offset += x.Scripts.Read1ByteFromScript(offset, out byte b2);
            offset += x.Scripts.Read2BytesFromScript(offset, out ushort s2);
            offset += x.Scripts.Read4BytesFromScript(offset, out uint i2);
            offset += x.Scripts.ReadFloatFromScript(offset, out float f2);
            Assert.Equal(431, offset);
            Assert.Equal(b, b2);
            Assert.Equal(s, s2);
            Assert.Equal(i, i2);
            Assert.Equal(f, f2);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
