using Bogus;
using System;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.VCS.Tests
{
    public class TestVCSSave : Base<VCSSave>
    {
        public override VCSSave GenerateTestObject(FileFormat format)
        {
            Faker<VCSSave> model = new Faker<VCSSave>()
                .RuleFor(x => x.FileFormat, format)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables>(format))
                .RuleFor(x => x.Scripts, Generator.Generate<ScriptData, TestScriptData>(format))
                // .RuleFor(x => x.Garages, Generator.Generate<GarageData, TestGarageData>(format))
                // .RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo>(format))
                // .RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats>(format))
                ;

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void FileFormatDetection(FileFormat expectedFormat, string filename)
        {
            string path = TestData.GetTestDataPath(Game.VCS, expectedFormat, filename);
            SaveData.GetFileFormat<VCSSave>(path, out FileFormat detectedFormat);

            Assert.Equal(expectedFormat, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            VCSSave x0 = GenerateTestObject(format);
            VCSSave x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, format);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataSerialization(FileFormat format, string filename)
        {
            string path = TestData.GetTestDataPath(Game.VCS, format, filename);

            VCSSave x0 = SaveData.Load<VCSSave>(path, format);
            VCSSave x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, format);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            VCSSave x0 = GenerateTestObject();
            VCSSave x1 = new VCSSave(x0);

            Assert.Equal(x0, x1);
        }

        private void AssertSavesAreEqual(VCSSave x0, VCSSave x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.PlayerInfo, x1.PlayerInfo);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0, x1);
        }

        private void AssertCheckSumValid(byte[] data, FileFormat format)
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
