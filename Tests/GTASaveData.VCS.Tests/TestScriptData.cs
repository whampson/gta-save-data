﻿using Bogus;
using GTASaveData.Core.Tests;
using TestFramework;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.VCS.Tests
{
    public class TestScriptData : Base<ScriptData>
    {
        public override ScriptData GenerateTestObject(FileFormat format)
        {
            Faker faker = new Faker();

            int varSpace = faker.Random.Int(4, 8000);
            int runningScripts = faker.Random.Int(1, 20);

            Faker<ScriptData> model = new Faker<ScriptData>()
                .RuleFor(x => x.Globals, f => Generator.Array(varSpace, g => f.Random.Int()))
                .RuleFor(x => x.OnAMissionFlag, f => f.Random.Int())
                .RuleFor(x => x.LastMissionPassedTime, f => f.Random.UInt())
                .RuleFor(x => x.BuildingSwaps, f => Generator.Array(ScriptData.NumBuildingSwaps, g => Generator.Generate<BuildingSwap, TestBuildingSwap>()))
                .RuleFor(x => x.InvisibilitySettings, f => Generator.Array(ScriptData.NumInvisibilitySettings, g => Generator.Generate<InvisibleObject, TestInvisibleObject>()))
                .RuleFor(x => x.UsingAMultiScriptFile, f => f.Random.Bool())
                .RuleFor(x => x.PlayerHasMetDebbieHarry, f => f.Random.Bool())
                .RuleFor(x => x.MainScriptSize, f => f.Random.Int())
                .RuleFor(x => x.LargestMissionScriptSize, f => f.Random.Int())
                .RuleFor(x => x.NumberOfMissionScripts, f => f.Random.Short())
                .RuleFor(x => x.Threads, f => Generator.Array(runningScripts, g => Generator.Generate<RunningScript, TestRunningScript>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            ScriptData x0 = GenerateTestObject(format);
            ScriptData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Globals, x1.Globals);
            Assert.Equal(x0.OnAMissionFlag, x1.OnAMissionFlag);
            Assert.Equal(x0.LastMissionPassedTime, x1.LastMissionPassedTime);
            Assert.Equal(x0.BuildingSwaps, x1.BuildingSwaps);
            Assert.Equal(x0.InvisibilitySettings, x1.InvisibilitySettings);
            Assert.Equal(x0.UsingAMultiScriptFile, x1.UsingAMultiScriptFile);
            Assert.Equal(x0.PlayerHasMetDebbieHarry, x1.PlayerHasMetDebbieHarry);
            Assert.Equal(x0.MainScriptSize, x1.MainScriptSize);
            Assert.Equal(x0.LargestMissionScriptSize, x1.LargestMissionScriptSize);
            Assert.Equal(x0.NumberOfMissionScripts, x1.NumberOfMissionScripts);
            Assert.Equal(x0.Threads, x1.Threads);

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
            x0.Threads[0].InstructionPointer = 6969;
            Assert.NotEqual(x0.Threads[0], x1.Threads[0]);
        }

        // [Fact]
        // public void GlobalVariables()
        // {
        //     Faker f = new Faker();
        //     string path = TestData.GetTestDataPath(Game.LCS, VCSSave.FileFormats.PS2, "NEDS4");
        //     VCSSave x = SaveData.Load<VCSSave>(path, VCSSave.FileFormats.PS2);

        //     Assert.Equal(272.1489f, x.Scripts.GetGlobalAsFloat(7));

        //     int numGlobals = x.Scripts.GlobalVariables.Count();
        //     int i0 = f.Random.Int(0, numGlobals - 1);
        //     int i1 = f.Random.Int(0, numGlobals - 1);
        //     int v0 = f.Random.Int();
        //     float v1 = f.Random.Float();

        //     x.Scripts.SetGlobal(i0, v0);
        //     x.Scripts.SetGlobal(i1, v1);

        //     int r0 = x.Scripts.GetGlobal(i0);
        //     float r1 = x.Scripts.GetGlobalAsFloat(i1);

        //     Assert.Equal(v0, r0);
        //     Assert.Equal(v1, r1);
        // }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
