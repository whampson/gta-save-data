using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGTA3Save : GTA3SaveDataObjectTestBase<GTA3Save>
    {
        public override GTA3Save GenerateTestObject(SaveFileFormat format)
        {
            Faker<GTA3Save> model = new Faker<GTA3Save>()
                .RuleFor(x => x.FileFormat, format)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables>(format))
                .RuleFor(x => x.Scripts, Generator.Generate<TheScripts, TesttheScripts>(format))
                //.RuleFor(x => x.PedPool, Generator.Generate<PedPool, TestPedPool>(format))
                //.RuleFor(x => x.Garages, Generator.Generate<Garages, TestGarages>(format))
                //.RuleFor(x => x.VehiclePool, Generator.Generate<VehiclePool, TestVehiclePool>(format))
                //.RuleFor(x => x.ObjectPool, Generator.Generate<ObjectPool, TestObjectPool>(format))
                //.RuleFor(x => x.PathFind, Generator.Generate<PathFind, TestPathFind>(format))
                //.RuleFor(x => x.Cranes, Generator.Generate<Cranes, TestCranes>(format))
                //.RuleFor(x => x.Pickups, Generator.Generate<Pickups, TestPickups>(format))
                //.RuleFor(x => x.PhoneInfo, Generator.Generate<PhoneInfo, TestPhoneInfo>(format))
                //.RuleFor(x => x.RestartPoints, Generator.Generate<RestartPoints, TestRestartPoints>(format))
                //.RuleFor(x => x.RadarBlips, Generator.Generate<RadarBlips, TestRadarBlips>(format))
                //.RuleFor(x => x.Zones, Generator.Generate<Zones, TestZones>(format))
                //.RuleFor(x => x.GangData, Generator.Generate<GangData, TestGangData>(format))
                //.RuleFor(x => x.CarGenerators, Generator.Generate<TheCarGenerators, TestTheCarGenerators>(format))
                //.RuleFor(x => x.ParticleObjects, Generator.Generate<Particles, TestParticles>(format))
                //.RuleFor(x => x.AudioScriptObjects, Generator.Generate<AudioScriptObjects, TestAudioScriptObjects>(format))
                //.RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo>(format))
                //.RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats>(format))
                //.RuleFor(x => x.Streaming, Generator.Generate<Streaming, TestStreaming>(format))
                //.RuleFor(x => x.PedTypeInfo, Generator.Generate<PedTypeInfo, TestPedTypeInfo>(format))
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
            GTA3Save x0 = GenerateTestObject(format);
            GTA3Save x1 = CreateSerializedCopy(x0, format, out byte[] data);

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

            GTA3Save x0 = SaveFile.Load<GTA3Save>(path, format);
            GTA3Save x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
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
            Assert.Equal(x0.GangData, x1.GangData);
            Assert.Equal(x0.CarGenerators, x1.CarGenerators);
            Assert.Equal(x0.ParticleObjects, x1.ParticleObjects);
            Assert.Equal(x0.AudioScriptObjects, x1.AudioScriptObjects);
            Assert.Equal(x0.PlayerInfo, x1.PlayerInfo);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0.Streaming, x1.Streaming);
            Assert.Equal(x0.PedTypeInfo, x1.PedTypeInfo);
            Assert.Equal(x0, x1);
        }

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { GTA3Save.FileFormats.PC, "1_JM4" },
            new object[] { GTA3Save.FileFormats.PC, "2_AS3" },
            //new object[] { GTA3Save.FileFormats.PS2_AU, "1_T4X4_2" },
            //new object[] { GTA3Save.FileFormats.PS2_AU, "2_AS3" },
            //new object[] { GTA3Save.FileFormats.PS2_AU, "3_CAT2" },
            //new object[] { GTA3Save.FileFormats.PS2_JP, "1_LM1" },
            //new object[] { GTA3Save.FileFormats.PS2_JP, "2_LM2" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "1_T4X4_1" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "2_LM1" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "3_CAT2" },
        };
    }
}
