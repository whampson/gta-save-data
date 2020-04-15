using Bogus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGTA3Save : Base<GTA3Save>
    {
        public override GTA3Save GenerateTestObject(SaveFileFormat format)
        {
            Faker<GTA3Save> model = new Faker<GTA3Save>()
                .RuleFor(x => x.FileFormat, format)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables>(format))
                .RuleFor(x => x.Scripts, Generator.Generate<ScriptData, TestScriptData>(format))
                //.RuleFor(x => x.PedPool, Generator.Generate<PedPool, TestPedPool>(format))
                .RuleFor(x => x.Garages, Generator.Generate<GarageData, TestGarageData>(format))
                //.RuleFor(x => x.VehiclePool, Generator.Generate<VehiclePool, TestVehiclePool>(format))
                //.RuleFor(x => x.ObjectPool, Generator.Generate<ObjectPool, TestObjectPool>(format))
                .RuleFor(x => x.Paths, Generator.Generate<PathData, TestPathData>(format))
                .RuleFor(x => x.Cranes, Generator.Generate<CraneData, TestCraneData>(format))
                .RuleFor(x => x.Pickups, Generator.Generate<PickupData, TestPickupData>(format))
                //.RuleFor(x => x.PhoneInfo, Generator.Generate<PhoneInfo, TestPhoneInfo>(format))
                //.RuleFor(x => x.RestartPoints, Generator.Generate<RestartPoints, TestRestartPoints>(format))
                //.RuleFor(x => x.RadarBlips, Generator.Generate<RadarBlips, TestRadarBlips>(format))
                //.RuleFor(x => x.Zones, Generator.Generate<Zones, TestZones>(format))
                .RuleFor(x => x.Gangs, Generator.Generate<GangData, TestGangData>(format))
                .RuleFor(x => x.CarGenerators, Generator.Generate<CarGeneratorData, TestCarGeneratorData>(format))
                //.RuleFor(x => x.ParticleObjects, Generator.Generate<Particles, TestParticles>(format))
                .RuleFor(x => x.AudioScriptObjects, Generator.Generate<AudioScriptData, TestAudioScriptData>(format))
                .RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo>(format))
                .RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats>(format))
                .RuleFor(x => x.Streaming, Generator.Generate<Streaming, TestStreaming>(format))
                .RuleFor(x => x.PedType, Generator.Generate<PedTypeData, TestPedTypeData>(format));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void FileFormatDetection(SaveFileFormat expectedFormat, string filename)
        {
            string path = TestData.GetTestDataPath(GameType.III, expectedFormat, filename);
            SaveFile.GetFileFormat<GTA3Save>(path, out SaveFileFormat detectedFormat);

            Assert.Equal(expectedFormat, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(SaveFileFormat format)
        {
            using GTA3Save x0 = GenerateTestObject(format);
            using GTA3Save x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataSerialization(SaveFileFormat format, string filename)
        {
            string path = TestData.GetTestDataPath(GameType.III, format, filename);

            using GTA3Save x0 = SaveFile.Load<GTA3Save>(path, format);
            using GTA3Save x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        [Fact]
        public void BlockSizeExceeded()
        {
            string path = TestData.GetTestDataPath(GameType.III, GTA3Save.FileFormats.PC, "CAT2");
            byte[] data = File.ReadAllBytes(path);

            using GTA3Save x = new GTA3Save()
            {
                FileFormat = GTA3Save.FileFormats.PC,
                BlockSizeChecks = true
            };
            // Fudge the block size
            data[0] = 0xBE;
            data[1] = 0xBA;
            data[2] = 0xFE;
            data[3] = 0xCA;
            Assert.Throws<SerializationException>(() => x.Load(data));

            // Make the script space huge
            x.Scripts.GlobalSpace = Helpers.CreateArray<byte>(100000);
            Assert.Throws<SerializationException>(() => x.Save(out byte[] _));
        }

        private void AssertSavesAreEqual(GTA3Save x0, GTA3Save x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.PedPool, x1.PedPool);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.VehiclePool, x1.VehiclePool);
            Assert.Equal(x0.ObjectPool, x1.ObjectPool);
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
            Assert.Equal(x0.PedType, x1.PedType);
            Assert.Equal(x0, x1);
        }

        public static IEnumerable<object[]> TestFiles => new[]
        {
            //new object[] { GTA3Save.FileFormats.Android, "AS2" },
            //new object[] { GTA3Save.FileFormats.Android, "AS3" },
            //new object[] { GTA3Save.FileFormats.Android, "CAT2" },
            //new object[] { GTA3Save.FileFormats.Android, "LM1" },
            //new object[] { GTA3Save.FileFormats.Android, "LM1 NonGXTName" },
            //new object[] { GTA3Save.FileFormats.Android, "LM1 VehiclePool" },
            //new object[] { GTA3Save.FileFormats.Android, "RC2 iOSConversion" },
            //new object[] { GTA3Save.FileFormats.Android, "T4X4_1" },
            //new object[] { GTA3Save.FileFormats.Android, "T4X4_2" },
            //new object[] { GTA3Save.FileFormats.iOS, "CAT2" },
            //new object[] { GTA3Save.FileFormats.iOS, "CAT2 GTASnP" },
            //new object[] { GTA3Save.FileFormats.iOS, "JM2" },
            //new object[] { GTA3Save.FileFormats.iOS, "LM1" },
            //new object[] { GTA3Save.FileFormats.iOS, "LM3" },
            //new object[] { GTA3Save.FileFormats.iOS, "LM3 VehiclePool" },
            new object[] { GTA3Save.FileFormats.PC, "AS3" },
            new object[] { GTA3Save.FileFormats.PC, "CAT2" },
            new object[] { GTA3Save.FileFormats.PC, "CAT2 SpecialVehicles1" },
            new object[] { GTA3Save.FileFormats.PC, "CAT2 SpecialVehicles2" },
            new object[] { GTA3Save.FileFormats.PC, "CAT2 SpecialVehicles3" },
            new object[] { GTA3Save.FileFormats.PC, "JM4" },
            new object[] { GTA3Save.FileFormats.PC, "RC1" },
            new object[] { GTA3Save.FileFormats.PC, "RC3" },
            new object[] { GTA3Save.FileFormats.PC, "T4X4_1" },
            new object[] { GTA3Save.FileFormats.PC, "T4X4_3" },
            //new object[] { GTA3Save.FileFormats.PS2_AU, "AS3" },
            //new object[] { GTA3Save.FileFormats.PS2_AU, "CAT2" },
            //new object[] { GTA3Save.FileFormats.PS2_AU, "T4X4_2" },
            //new object[] { GTA3Save.FileFormats.PS2_JP, "LM1" },
            //new object[] { GTA3Save.FileFormats.PS2_JP, "LM2" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "CAT2" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "LM1" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "LM1 EUv1" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "LM1 EUv2" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "LM1 US" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "T4X4_1" },
            //new object[] { GTA3Save.FileFormats.Xbox, "JM2" },
            //new object[] { GTA3Save.FileFormats.Xbox, "LM1 1" },
            //new object[] { GTA3Save.FileFormats.Xbox, "LM1 2" },
            //new object[] { GTA3Save.FileFormats.Xbox, "LM1 ChainGame100" },
        };
    }
}
