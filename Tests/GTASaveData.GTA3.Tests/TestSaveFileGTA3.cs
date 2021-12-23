using Bogus;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestSaveFileGTA3 : Base<GTA3SaveFile>
    {
        public override GTA3SaveFile GenerateTestObject(FileType format)
        {
            Faker<GTA3SaveFile> model = new Faker<GTA3SaveFile>()
                .RuleFor(x => x.FileType, format)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables>(format))
                .RuleFor(x => x.Scripts, Generator.Generate<ScriptsBlock, TestScriptBlock>(format))
                .RuleFor(x => x.PlayerPeds, Generator.Generate<PedPool, TestPedPool>(format))
                .RuleFor(x => x.Garages, Generator.Generate<GarageData, TestGarageData>(format))
                .RuleFor(x => x.Vehicles, Generator.Generate<VehiclePool, TestVehiclePool>(format))
                .RuleFor(x => x.Objects, Generator.Generate<ObjectPool, TestObjectPool>(format))
                .RuleFor(x => x.Paths, Generator.Generate<PathData, TestPathData>(format))
                .RuleFor(x => x.Cranes, Generator.Generate<CraneData, TestCraneData>(format))
                .RuleFor(x => x.Pickups, Generator.Generate<PickupData, TestPickupData>(format))
                .RuleFor(x => x.PhoneInfo, Generator.Generate<PhoneData, TestPhoneData>(format))
                .RuleFor(x => x.RestartPoints, Generator.Generate<RestartData, TestRestartData>(format))
                .RuleFor(x => x.RadarBlips, Generator.Generate<RadarData, TestRadarData>(format))
                .RuleFor(x => x.Zones, Generator.Generate<ZoneData, TestZoneData>(format))
                .RuleFor(x => x.Gangs, Generator.Generate<GangData, TestGangData>(format))
                .RuleFor(x => x.CarGenerators, Generator.Generate<CarGeneratorData, TestCarGeneratorData>(format))
                .RuleFor(x => x.ParticleObjects, Generator.Generate<ParticleData, TestParticleData>(format))
                .RuleFor(x => x.AudioScriptObjects, Generator.Generate<AudioScriptData, TestAudioScriptData>(format))
                .RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo>(format))
                .RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats>(format))
                .RuleFor(x => x.Streaming, Generator.Generate<Streaming, TestStreaming>(format))
                .RuleFor(x => x.PedTypeInfo, Generator.Generate<PedTypeData, TestPedTypeData>(format))
                ;

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void DefinitiveEditionPlayground(FileType fmt, string path)
        {
            if (!fmt.FlagDE) return;

            path = TestData.GetTestDataPath(Game.GTA3, fmt, path);
            var save = SaveFile.Load<GTA3SaveFile>(path, fmt);

            Debug.WriteLine($"Title: {save.Title}");
            Debug.WriteLine($"TimeStamp: {save.TimeStamp}");
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void FileFormatDetection(FileType expectedFormat, string filename)
        {
            string path = TestData.GetTestDataPath(Game.GTA3, expectedFormat, filename);
            SaveFile.TryGetFileType<GTA3SaveFile>(path, out FileType detectedFormat);

            Assert.Equal(expectedFormat, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            using GTA3SaveFile x0 = GenerateTestObject(format);
            using GTA3SaveFile x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, format);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealData(FileType format, string filename)
        {
            string path = TestData.GetTestDataPath(Game.GTA3, format, filename);

            using GTA3SaveFile x0 = SaveFile.Load<GTA3SaveFile>(path, format);
            using GTA3SaveFile x1 = CreateSerializedCopy(x0, format, out byte[] data);

            // Copy properties that don't actually get saved to the new buffer.
            if (format.IsPS2)
            {
                x1.Title = x0.Title;
            }
            if (!format.IsPC && !format.IsXbox)
            {
                x1.TimeStamp = x0.TimeStamp;
            }

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, format);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            GTA3SaveFile x0 = GenerateTestObject();
            GTA3SaveFile x1 = new GTA3SaveFile(x0);

            Assert.Equal(x0, x1);
        }

        [Fact]
        public void BlockBounds()
        {
            string path = TestData.GetTestDataPath(Game.GTA3, GTA3SaveFile.FileFormats.PC, "CAT2");
            byte[] data = File.ReadAllBytes(path);

            // Fudge the block size
            data[0] = 0xBE;
            data[1] = 0xBA;
            data[2] = 0xFE;
            data[3] = 0xCA;
            Assert.Throws<SerializationException>(() => SaveFile.Load<GTA3SaveFile>(data, GTA3SaveFile.FileFormats.PC));

            // Make the script space huge
            GTA3SaveFile x = SaveFile.Load<GTA3SaveFile>(path, GTA3SaveFile.FileFormats.PC);
            x.Scripts.ScriptSpace = ArrayHelper.CreateArray<byte>(100000);
            Assert.Throws<EndOfStreamException>(() => x.Save(out byte[] _));
        }

        private void AssertSavesAreEqual(GTA3SaveFile x0, GTA3SaveFile x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.PlayerPeds, x1.PlayerPeds);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.Vehicles, x1.Vehicles);
            Assert.Equal(x0.Objects, x1.Objects);
            Assert.Equal(x0.Paths, x1.Paths);
            Assert.Equal(x0.Cranes, x1.Cranes);
            Assert.Equal(x0.Pickups, x1.Pickups);
            Assert.Equal(x0.PhoneInfo, x1.PhoneInfo);
            Assert.Equal(x0.RestartPoints, x1.RestartPoints);
            Assert.Equal(x0.RadarBlips, x1.RadarBlips);
            Assert.Equal(x0.Zones, x1.Zones);
            Assert.Equal(x0.Gangs, x1.Gangs);
            Assert.Equal(x0.CarGenerators, x1.CarGenerators);
            Assert.Equal(x0.ParticleObjects, x1.ParticleObjects);
            Assert.Equal(x0.AudioScriptObjects, x1.AudioScriptObjects);
            Assert.Equal(x0.PlayerInfo, x1.PlayerInfo);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0.Streaming, x1.Streaming);
            Assert.Equal(x0.PedTypeInfo, x1.PedTypeInfo);
            Assert.Equal(x0, x1);
        }

        private void AssertCheckSumValid(byte[] data, FileType format)
        {
            int sumOffset = (format.IsXbox) ? data.Length - 24 : data.Length - 4;
            int calculatedSum = data.Take(sumOffset).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, sumOffset);
            Assert.Equal(calculatedSum, storedSum);
        }
    }
}
