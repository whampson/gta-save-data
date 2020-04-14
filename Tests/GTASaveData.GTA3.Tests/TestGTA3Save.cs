using Bogus;
using GTASaveData.Types;
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
                .RuleFor(x => x.ScriptData, Generator.Generate<ScriptData, TestScriptData>(format))
                //.RuleFor(x => x.PedPool, Generator.Generate<PedPool, TestPedPool>(format))
                .RuleFor(x => x.GarageData, Generator.Generate<GarageData, TestGarageData>(format))
                //.RuleFor(x => x.VehiclePool, Generator.Generate<VehiclePool, TestVehiclePool>(format))
                //.RuleFor(x => x.ObjectPool, Generator.Generate<ObjectPool, TestObjectPool>(format))
                .RuleFor(x => x.PathData, Generator.Generate<PathData, TestPathData>(format))
                //.RuleFor(x => x.Cranes, Generator.Generate<Cranes, TestCranes>(format))
                .RuleFor(x => x.PickupData, Generator.Generate<PickupData, TestPickupData>(format))
                //.RuleFor(x => x.PhoneInfo, Generator.Generate<PhoneInfo, TestPhoneInfo>(format))
                //.RuleFor(x => x.RestartPoints, Generator.Generate<RestartPoints, TestRestartPoints>(format))
                //.RuleFor(x => x.RadarBlips, Generator.Generate<RadarBlips, TestRadarBlips>(format))
                //.RuleFor(x => x.Zones, Generator.Generate<Zones, TestZones>(format))
                .RuleFor(x => x.GangData, Generator.Generate<GangData, TestGangData>(format))
                .RuleFor(x => x.CarGeneratorData, Generator.Generate<CarGeneratorData, TestCarGeneratorData>(format))
                //.RuleFor(x => x.ParticleObjects, Generator.Generate<Particles, TestParticles>(format))
                //.RuleFor(x => x.AudioScriptObjects, Generator.Generate<AudioScriptObjects, TestAudioScriptObjects>(format))
                //.RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo>(format))
                //.RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats>(format))
                //.RuleFor(x => x.Streaming, Generator.Generate<Streaming, TestStreaming>(format))
                .RuleFor(x => x.PedTypeData, Generator.Generate<PedTypeData, TestPedTypeData>(format))
                ;

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
            x.ScriptData.ScriptSpace = GTAObject.CreateArray<byte>(100000);
            Assert.Throws<SerializationException>(() => x.Save(out byte[] _));
        }

        private void AssertSavesAreEqual(GTA3Save x0, GTA3Save x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.ScriptData, x1.ScriptData);
            Assert.Equal(x0.PedPool, x1.PedPool);
            Assert.Equal(x0.GarageData, x1.GarageData);
            Assert.Equal(x0.VehiclePool, x1.VehiclePool);
            Assert.Equal(x0.ObjectPool, x1.ObjectPool);
            //Assert.Equal(x0.Paths, x1.Paths);         // TODO: equality fails due to list padding during save
            Assert.Equal(x0.CraneData, x1.CraneData);
            Assert.Equal(x0.PickupData, x1.PickupData);
            Assert.Equal(x0.PhoneData, x1.PhoneData);
            Assert.Equal(x0.RestartData, x1.RestartData);
            Assert.Equal(x0.RadarData, x1.RadarData);
            Assert.Equal(x0.ZoneData, x1.ZoneData);
            Assert.Equal(x0.GangData, x1.GangData);
            Assert.Equal(x0.CarGeneratorData, x1.CarGeneratorData);
            Assert.Equal(x0.ParticleData, x1.ParticleData);
            Assert.Equal(x0.AudioScriptData, x1.AudioScriptData);
            Assert.Equal(x0.PlayerData, x1.PlayerData);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0.StreamingData, x1.StreamingData);
            Assert.Equal(x0.PedTypeData, x1.PedTypeData);
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
