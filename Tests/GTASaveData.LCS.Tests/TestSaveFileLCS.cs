using Bogus;
using System;
using System.IO;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.LCS.Tests
{
    public class TestSaveFileLCS : Base<LCSSave>
    {
        public override LCSSave GenerateTestObject(LCSSaveParams p)
        {
            Faker<LCSSave> model = new Faker<LCSSave>()
                .RuleFor(x => x.FileType, p.FileType)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables, LCSSaveParams>(p))
                .RuleFor(x => x.Scripts, Generator.Generate<ScriptData, TestScriptData, LCSSaveParams>(p))
                .RuleFor(x => x.Garages, Generator.Generate<GarageData, TestGarageData, LCSSaveParams>(p))
                .RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo, LCSSaveParams>(p))
                .RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats, LCSSaveParams>(p))
                ;

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void FileFormatDetection(FileType expectedFileType, string filename)
        {
            string path = TestData.GetTestDataPath(Game.LCS, expectedFileType, filename);
            LCSSave.TryGetFileType(path, out FileType detectedFormat);

            Assert.Equal(expectedFileType, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(LCSSaveParams p)
        {
            LCSSave x0 = GenerateTestObject(p);
            LCSSave x1 = CreateSerializedCopy(x0, p, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, p.FileType);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataJsonSerialization(LCSSaveParams p)
        {
            LCSSave x0 = GenerateTestObject(p);
            LCSSave x1 = CreateSerializedJsonCopy(x0, out string json);

            AssertSavesAreEqual(x0, x1);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataSerialization(FileType fileType, string filename)
        {
            string path = TestData.GetTestDataPath(Game.LCS, fileType, filename);

            LCSSaveParams p = LCSSaveParams.GetDefaults(fileType);
            LCSSave x0 = LCSSave.LoadFromFile(path, fileType);
            LCSSave x1 = CreateSerializedCopy(x0, p, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, p.FileType);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataJsonSerialization(FileType fileType, string filename)
        {
            string path = TestData.GetTestDataPath(Game.LCS, fileType, filename);

            LCSSaveParams p = LCSSaveParams.GetDefaults(fileType);
            LCSSave x0 = LCSSave.LoadFromFile(path, fileType);
            LCSSave x1 = CreateSerializedJsonCopy(x0, out string json, Newtonsoft.Json.Formatting.None);

            //// uncomment to update JSON files
            //File.WriteAllText(path, json);

            AssertSavesAreEqual(x0, x1);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataCorrectness(FileType fileType, string filename)
        {
            string path = TestData.GetTestDataPath(Game.LCS, fileType, filename);
            string json = File.ReadAllText($"{path}.json"); // ASSUMES JSON DATA IS CORRECT (it should be.........)

            LCSSave gold = SaveDataObject.FromJsonString<LCSSave>(json);

            LCSSaveParams p = LCSSaveParams.GetDefaults(fileType);
            LCSSave x0 = LCSSave.LoadFromFile(path, fileType);
            LCSSave x1 = CreateSerializedCopy(x0, p, out byte[] data);

            AssertSavesAreEqual(gold, x0);
            AssertSavesAreEqual(gold, x1);
        }

        [Fact]
        public void CopyConstructor()
        {
            LCSSave x0 = GenerateTestObject();
            LCSSave x1 = new LCSSave(x0);

            Assert.Equal(x0, x1);
        }

        private void AssertSavesAreEqual(LCSSave x0, LCSSave x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.PlayerInfo, x1.PlayerInfo);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0, x1);
        }

        private void AssertCheckSumValid(byte[] data, FileType format)
        {
            if (format.IsPS2)
            {
                int sumOffset = data.Length - 4;
                int calculatedSum = data.Take(sumOffset).Sum(x => x);
                int storedSum = BitConverter.ToInt32(data, sumOffset);
                Assert.Equal(calculatedSum, storedSum);
            }
        }
    }
}
