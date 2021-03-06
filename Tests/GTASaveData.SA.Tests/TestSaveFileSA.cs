using Bogus;
using TestFramework;
using Xunit;
using System.Linq;
using System;

namespace GTASaveData.SA.Tests
{
    public class TestSaveFileSA : Base<SaveFileSA>
    {
        public override SaveFileSA GenerateTestObject(FileFormat format)
        {
            Faker<SaveFileSA> model = new Faker<SaveFileSA>()
                .RuleFor(x => x.FileFormat, format)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables>(format));
            // TODO: the rest of the blocks

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void FileFormatDetection(FileFormat expectedFormat, string filename)
        {
            string path = TestData.GetTestDataPath(Game.SA, expectedFormat, filename);
            SaveFile.GetFileFormat<SaveFileSA>(path, out FileFormat detectedFormat);

            Assert.Equal(expectedFormat, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            using SaveFileSA x0 = GenerateTestObject(format);
            using SaveFileSA x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataSerialization(FileFormat format, string filename)
        {
            string path = TestData.GetTestDataPath(Game.SA, format, filename);

            using SaveFileSA x0 = SaveFile.Load<SaveFileSA>(path, format);
            using SaveFileSA x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        private void AssertSavesAreEqual(SaveFileSA x0, SaveFileSA x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.Pools, x1.Pools);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.GameLogic, x1.GameLogic);
            Assert.Equal(x0.Paths, x1.Paths);
            Assert.Equal(x0.Pickups, x1.Pickups);
            Assert.Equal(x0.PhoneInfo, x1.PhoneInfo);
            Assert.Equal(x0.RestartPoints, x1.RestartPoints);
            Assert.Equal(x0.RadarBlips, x1.RadarBlips);
            Assert.Equal(x0.Zones, x1.Zones);
            Assert.Equal(x0.GangData, x1.GangData);
            Assert.Equal(x0.CarGenerators, x1.CarGenerators);
            Assert.Equal(x0.PedGenerators, x1.PedGenerators);
            Assert.Equal(x0.AudioScriptObjects, x1.AudioScriptObjects);
            Assert.Equal(x0.PlayerInfo, x1.PlayerInfo);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0.SetPieces, x1.SetPieces);
            Assert.Equal(x0.Streaming, x1.Streaming);
            Assert.Equal(x0.PedTypeInfo, x1.PedTypeInfo);
            Assert.Equal(x0.Tags, x1.Tags);
            Assert.Equal(x0.IplStore, x1.IplStore);
            Assert.Equal(x0.Shopping, x1.Shopping);
            Assert.Equal(x0.GangWars, x1.GangWars);
            Assert.Equal(x0.StuntJumps, x1.StuntJumps);
            Assert.Equal(x0.EntryExits, x1.EntryExits);
            Assert.Equal(x0.Radio, x1.Radio);
            Assert.Equal(x0.User3dMarkers, x1.User3dMarkers);
            Assert.Equal(x0.PostEffects, x1.PostEffects);
            Assert.Equal(x0, x1);
        }
    }
}
