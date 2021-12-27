using Bogus;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestSaveFileGTA3 : Base<GTA3Save>
    {
        public override GTA3Save GenerateTestObject(GTA3SaveParams p)
        {
            Faker<GTA3Save> model = new Faker<GTA3Save>()
                .RuleFor(x => x.Params, p)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables, GTA3SaveParams>(p))
                .RuleFor(x => x.Scripts, Generator.Generate<ScriptsBlock, TestScriptBlock, GTA3SaveParams>(p))
                .RuleFor(x => x.PlayerPeds, Generator.Generate<PedPool, TestPedPool, GTA3SaveParams>(p))
                .RuleFor(x => x.Garages, Generator.Generate<GarageData, TestGarageData, GTA3SaveParams>(p))
                .RuleFor(x => x.Vehicles, Generator.Generate<VehiclePool, TestVehiclePool, GTA3SaveParams>(p))
                .RuleFor(x => x.Objects, Generator.Generate<ObjectPool, TestObjectPool, GTA3SaveParams>(p))
                .RuleFor(x => x.Paths, Generator.Generate<PathData, TestPathData, GTA3SaveParams>(p))
                .RuleFor(x => x.Cranes, Generator.Generate<CraneData, TestCraneData, GTA3SaveParams>(p))
                .RuleFor(x => x.Pickups, Generator.Generate<PickupData, TestPickupData, GTA3SaveParams>(p))
                .RuleFor(x => x.PhoneInfo, Generator.Generate<PhoneData, TestPhoneData, GTA3SaveParams>(p))
                .RuleFor(x => x.RestartPoints, Generator.Generate<RestartData, TestRestartData, GTA3SaveParams>(p))
                .RuleFor(x => x.RadarBlips, Generator.Generate<RadarData, TestRadarData, GTA3SaveParams>(p))
                .RuleFor(x => x.Zones, Generator.Generate<ZoneData, TestZoneData, GTA3SaveParams>(p))
                .RuleFor(x => x.Gangs, Generator.Generate<GangData, TestGangData, GTA3SaveParams>(p))
                .RuleFor(x => x.CarGenerators, Generator.Generate<CarGeneratorData, TestCarGeneratorData, GTA3SaveParams>(p))
                .RuleFor(x => x.ParticleObjects, Generator.Generate<ParticleData, TestParticleData, GTA3SaveParams>(p))
                .RuleFor(x => x.AudioScriptObjects, Generator.Generate<AudioScriptData, TestAudioScriptData, GTA3SaveParams>(p))
                .RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo, GTA3SaveParams>(p))
                .RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats, GTA3SaveParams>(p))
                .RuleFor(x => x.Streaming, Generator.Generate<Streaming, TestStreaming, GTA3SaveParams>(p))
                .RuleFor(x => x.PedTypeInfo, Generator.Generate<PedTypeData, TestPedTypeData, GTA3SaveParams>(p))
                ;

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void DefinitiveEditionPlayground(FileType fmt, string path)
        {
            if (!fmt.FlagDE) return;

            path = TestData.GetTestDataPath(Game.GTA3, fmt, path);
            var save = GTA3Save.Load(path, fmt);

            Debug.WriteLine($"Title: {save.Title}");
            Debug.WriteLine($"TimeStamp: {save.TimeStamp}");
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void FileFormatDetection(FileType expectedFormat, string filename)
        {
            string path = TestData.GetTestDataPath(Game.GTA3, expectedFormat, filename);
            GTA3Save.TryGetFileType<GTA3Save>(path, out FileType detectedFormat);

            Assert.Equal(expectedFormat, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            using GTA3Save x0 = GenerateTestObject(p);
            using GTA3Save x1 = CreateSerializedCopy(x0, p, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, p.FileType);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealData(FileType t, string filename)
        {
            string path = TestData.GetTestDataPath(Game.GTA3, t, filename);

            using GTA3Save x0 = GTA3Save.Load(path, t);
            using GTA3Save x1 = CreateSerializedCopy(x0, x0.Params, out byte[] data);

            // Copy properties that don't actually get saved to the new buffer.
            if (t.IsPS2)
            {
                x1.Title = x0.Title;
            }
            if (!t.IsPC && !t.IsXbox)
            {
                x1.TimeStamp = x0.TimeStamp;
            }

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, t);
            Assert.Equal(GetSizeOfTestObject(x0, x0.Params), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            GTA3Save x0 = GenerateTestObject(p);
            GTA3Save x1 = new GTA3Save(x0);

            Assert.Equal(x0, x1);
        }

        [Fact]
        public void BlockBounds()
        {
            FileType t = GTA3Save.FileTypes.PC;
            string path = TestData.GetTestDataPath(Game.GTA3, t, "CAT2");
            byte[] data = File.ReadAllBytes(path);

            // Fudge the block size
            data[0] = 0xBE;
            data[1] = 0xBA;
            data[2] = 0xFE;
            data[3] = 0xCA;
            Assert.Throws<SerializationException>(() => GTA3Save.Load(data, GTA3SaveParams.GetDefaults(t)));

            // Make the script space huge
            GTA3Save x = GTA3Save.Load(path, t);
            x.Scripts.ScriptSpace = ArrayHelper.CreateArray<byte>(100000);
            Assert.Throws<EndOfStreamException>(() => x.Save(out byte[] _));
        }

        private void AssertSavesAreEqual(GTA3Save x0, GTA3Save x1)
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

        private void AssertCheckSumValid(byte[] data, FileType t)
        {
            int sumOffset = (t.IsXbox) ? data.Length - 24 : data.Length - 4;
            int calculatedSum = data.Take(sumOffset).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, sumOffset);
            Assert.Equal(calculatedSum, storedSum);
        }
    }
}
