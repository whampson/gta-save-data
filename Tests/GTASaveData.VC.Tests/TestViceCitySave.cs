using Bogus;
using System.Collections.Generic;
using TestFramework;
using Xunit;
using System.Linq;
using System;
using System.IO;

namespace GTASaveData.VC.Tests
{
    public class TestViceCitySave : Base<ViceCitySave>
    {
        public override ViceCitySave GenerateTestObject(FileFormat format)
        {
            Faker<ViceCitySave> model = new Faker<ViceCitySave>()
                .RuleFor(x => x.FileFormat, format)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables>(format))
                //.RuleFor(x => x.Scripts, Generator.Generate<TheScripts, TestTheScripts>(format))
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
                .RuleFor(x => x.CarGenerators, Generator.Generate<CarGeneratorData, TestCarGeneratorData>(format))
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
        public void FileFormatDetection(FileFormat expectedFormat, string filename)
        {
            string path = TestData.GetTestDataPath(GameType.VC, expectedFormat, filename);
            GTASaveFile.GetFileFormat<ViceCitySave>(path, out FileFormat detectedFormat);

            Assert.Equal(expectedFormat, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            using ViceCitySave x0 = GenerateTestObject(format);
            using ViceCitySave x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataSerialization(FileFormat format, string filename)
        {
            string path = TestData.GetTestDataPath(GameType.VC, format, filename);

            using ViceCitySave x0 = GTASaveFile.Load<ViceCitySave>(path, format);
            using ViceCitySave x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        [Fact]
        public void BlockSizeExceeded()
        {
            string path = TestData.GetTestDataPath(GameType.VC, ViceCitySave.FileFormats.PC_Retail, "COK_2");
            byte[] data = File.ReadAllBytes(path);

            using ViceCitySave x = new ViceCitySave()
            {
                FileFormat = ViceCitySave.FileFormats.PC_Retail,
                BlockSizeChecks = true
            };

            // Fudge the block size
            data[0] = 0xBE;
            data[1] = 0xBA;
            data[2] = 0xFE;
            data[3] = 0xCA;
            Assert.Throws<SerializationException>(() => x.Load(data));

            // TODO: uncomment when Scripts done
            // Make the script space huge
            //x.Scripts.ScriptSpace = GTAObject.CreateArray<byte>(100000);
            //Assert.Throws<SerializationException>(() => x.Save(out byte[] _));
        }

        private void AssertSavesAreEqual(ViceCitySave x0, ViceCitySave x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.PedPool, x1.PedPool);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.GameLogic, x1.GameLogic);
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
            Assert.Equal(x0.ScriptPaths, x1.ScriptPaths);
            Assert.Equal(x0.PlayerInfo, x1.PlayerInfo);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0.SetPieces, x1.SetPieces);
            Assert.Equal(x0.Streaming, x1.Streaming);
            Assert.Equal(x0.PedTypeInfo, x1.PedTypeInfo);
            Assert.Equal(x0, x1);
        }

        public static IEnumerable<object[]> TestFiles => new[]
        {
            //new object[] { ViceCitySave.FileFormats.Android, "CAP_1" },
            //new object[] { ViceCitySave.FileFormats.Android, "COK_4" },
            //new object[] { ViceCitySave.FileFormats.Android, "CUB_1" },
            //new object[] { ViceCitySave.FileFormats.Android, "FIN_1" },
            //new object[] { ViceCitySave.FileFormats.Android, "LAW_3" },
            //new object[] { ViceCitySave.FileFormats.iOS, "FIN_1" },
            //new object[] { ViceCitySave.FileFormats.iOS, "FIN_1 SpecialVehicles1" },
            //new object[] { ViceCitySave.FileFormats.iOS, "FIN_1 SpecialVehicles1" },
            new object[] { ViceCitySave.FileFormats.PC_Retail, "COK_2" },
            new object[] { ViceCitySave.FileFormats.PC_Retail, "CREAM" },
            new object[] { ViceCitySave.FileFormats.PC_Retail, "FIN_1" },
            new object[] { ViceCitySave.FileFormats.PC_Retail, "ITBEG" },
            new object[] { ViceCitySave.FileFormats.PC_Retail, "ITBEG Japan" },
            new object[] { ViceCitySave.FileFormats.PC_Retail, "ITBEG StarterSave" },
            new object[] { ViceCitySave.FileFormats.PC_Retail, "JOB_5" },
            new object[] { ViceCitySave.FileFormats.PC_Retail, "TEX_3" },
            new object[] { ViceCitySave.FileFormats.PC_Steam, "BUD_3" },
            new object[] { ViceCitySave.FileFormats.PC_Steam, "COK_3" },
            new object[] { ViceCitySave.FileFormats.PC_Steam, "FIN_1" },
            //new object[] { ViceCitySave.FileFormats.PS2, "CREAM" },
            //new object[] { ViceCitySave.FileFormats.PS2, "FIN_1 1" },
            //new object[] { ViceCitySave.FileFormats.PS2, "FIN_1 2" },
            //new object[] { ViceCitySave.FileFormats.PS2, "FIN_1 3" },
            //new object[] { ViceCitySave.FileFormats.PS2, "TEX_1" },
            //new object[] { ViceCitySave.FileFormats.PS2, "TEX_2" },
            //new object[] { ViceCitySave.FileFormats.Xbox, "CAP_1" },
            //new object[] { ViceCitySave.FileFormats.Xbox, "FIN_1" },
            //new object[] { ViceCitySave.FileFormats.Xbox, "LAW_2" },
        };
    }
}
