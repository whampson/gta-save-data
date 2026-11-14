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
        public void FileFormatDetection(FileType expectedFileType, ScriptVersion platform, string filename)
        {
            string path = TestData.GetTestDataPath(Game.LCS, platform.ToString(), filename);
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
        public void RealDataSerialization(FileType fileType, ScriptVersion platform, string filename)
        {
            string path = TestData.GetTestDataPath(Game.LCS, platform.ToString(), filename);

            LCSSaveParams p = LCSSaveParams.GetDefaults(fileType);
            LCSSave x0 = LCSSave.LoadFromFile(path, fileType);
            LCSSave x1 = CreateSerializedCopy(x0, p, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, p.FileType);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataJsonSerialization(FileType fileType, ScriptVersion platform, string filename)
        {
            string path = TestData.GetTestDataPath(Game.LCS, platform.ToString(), filename);

            LCSSaveParams p = LCSSaveParams.GetDefaults(fileType);
            LCSSave x0 = LCSSave.LoadFromFile(path, fileType);
            LCSSave x1 = CreateSerializedJsonCopy(x0, out string json);

            AssertSavesAreEqual(x0, x1);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataCorrectness(FileType fileType, ScriptVersion platform, string filename)
        {
            string path = TestData.GetTestDataPath(Game.LCS, platform.ToString(), filename);

            LCSSaveParams p = LCSSaveParams.GetDefaults(fileType);
            LCSSave x0 = LCSSave.LoadFromFile(path, fileType);

            byte[] original = File.ReadAllBytes(path);
            Serializer.Write(x0, p, out byte[] data, out bool[] volatileMask);

            Assert.Equal(original.Length, data.Length);
            Assert.Equal(original.Length, volatileMask.Length);

            int firstMismatch = -1;
            for (int i = 0; i < data.Length; i++)
            {
                if (!volatileMask[i] && original[i] != data[i])
                {
                    firstMismatch = i;
                    break;
                }
            }

            string message = firstMismatch < 0
                ? string.Empty
                : $"Byte {firstMismatch} differs (original 0x{original[firstMismatch]:X2}, serialized 0x{data[firstMismatch]:X2})";
            Assert.True(firstMismatch < 0, message);
        }

        [Fact]
        public void CopyConstructor()
        {
            LCSSave x0 = GenerateTestObject();
            LCSSave x1 = new LCSSave(x0);

            Assert.Equal(x0, x1);
        }

        [Fact]
        public void BlockStructure()
        {
            string path = TestData.GetTestDataPath(Game.LCS, LCSSave.FileTypes.PS2, "NEDS4");
            byte[] data = File.ReadAllBytes(path);

            Assert.Equal(0x19000, data.Length);

            string[] expectedTags = { "SIMP", "SRPT", "GRGE", "PLYR", "STAT" };
            int offset = 0;
            foreach (string tag in expectedTags)
            {
                Assert.Equal(tag, System.Text.Encoding.ASCII.GetString(data, offset, 4));
                int size = BitConverter.ToInt32(data, offset + 4);
                offset += 8 + ((size + 3) & ~3);   // tag + size + aligned data
            }
        }

        [Fact]
        public void FileFormatDetectionFailure()
        {
            string path = WriteTempFile(0x100, new Random(42));

            try
            {
                Assert.False(LCSSave.TryGetFileType(path, out FileType detected));
                Assert.Equal(FileType.Default, detected);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void LoadFromFileUndetectableType()
        {
            string path = WriteTempFile(0x100, new Random(43));

            try
            {
                Assert.Throws<InvalidDataException>(() => LCSSave.LoadFromFile(path, FileType.Default));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void LoadFromFileInvalidType()
        {
            string path = TestData.GetTestDataPath(Game.LCS, LCSSave.FileTypes.PS2, "NEDS4");
            Assert.Throws<InvalidDataException>(() => LCSSave.LoadFromFile(path, new FileType("unknown")));
        }

        [Fact]
        public void LoadFromFileTruncated()
        {
            string src = TestData.GetTestDataPath(Game.LCS, LCSSave.FileTypes.PS2, "NEDS4");
            byte[] full = File.ReadAllBytes(src);
            string path = Path.Combine(Path.GetTempPath(), $"lcs_truncated_{Guid.NewGuid():N}.bin");
            File.WriteAllBytes(path, full.Take(0x200).ToArray());

            try
            {
                Assert.Throws<EndOfStreamException>(() => LCSSave.LoadFromFile(path, LCSSave.FileTypes.PS2));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void TryLoadFromFileFailure()
        {
            string path = Path.Combine(Path.GetTempPath(), $"does_not_exist_{Guid.NewGuid():N}.bin");
            Assert.False(LCSSave.TryLoadFromFile(path, LCSSave.FileTypes.PS2, out LCSSave save));
            Assert.Null(save);
        }

        [Fact]
        public void TryGetFileTypeFailure()
        {
            string path = Path.Combine(Path.GetTempPath(), $"does_not_exist_{Guid.NewGuid():N}.bin");
            Assert.False(LCSSave.TryGetFileType(path, out FileType t));
            Assert.Equal(FileType.Default, t);
        }

        private static string WriteTempFile(int length, Random rng)
        {
            string path = Path.Combine(Path.GetTempPath(), $"lcs_garbage_{Guid.NewGuid():N}.bin");
            byte[] garbage = new byte[length];
            rng.NextBytes(garbage);
            File.WriteAllBytes(path, garbage);
            return path;
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
