using Bogus;
using System.Collections.Generic;
using TestFramework;
using Xunit;
using System.Linq;
using System;
using System.IO;

namespace GTASaveData.VC.Tests
{
    public class TestViceCitySave : Base<VCSave>
    {
        public override VCSave GenerateTestObject(FileFormat format)
        {
            Faker<VCSave> model = new Faker<VCSave>()
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
            string path = TestData.GetTestDataPath(Game.VC, expectedFormat, filename);
            SaveData.GetFileFormat<VCSave>(path, out FileFormat detectedFormat);

            Assert.Equal(expectedFormat, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            using VCSave x0 = GenerateTestObject(format);
            using VCSave x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataSerialization(FileFormat format, string filename)
        {
            string path = TestData.GetTestDataPath(Game.VC, format, filename);

            using VCSave x0 = SaveData.Load<VCSave>(path, format);
            using VCSave x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        [Fact]
        public void BlockSizeExceeded()
        {
            string path = TestData.GetTestDataPath(Game.VC, VCSave.FileFormats.PC, "COK_2");
            byte[] data = File.ReadAllBytes(path);

            // Fudge the block size
            data[0] = 0xBE;
            data[1] = 0xBA;
            data[2] = 0xFE;
            data[3] = 0xCA;
            Assert.Throws<SerializationException>(() => SaveData.Load<VCSave>(data, VCSave.FileFormats.PC));

            // TODO: uncomment when Scripts done
            // Make the script space huge
            //VCSave x = SaveData.Load<VCSave>(path, VCSave.FileFormats.PC);
            //x.Scripts.ScriptSpace = GTAObject.CreateArray<byte>(100000);
            //Assert.Throws<SerializationException>(() => x.Save(out byte[] _));
        }

        private void AssertSavesAreEqual(VCSave x0, VCSave x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.PlayerPeds, x1.PlayerPeds);
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
            new object[] { VCSave.FileFormats.PC, "COK_2" },
            new object[] { VCSave.FileFormats.PC, "CREAM" },
            new object[] { VCSave.FileFormats.PC, "FIN_1" },
            new object[] { VCSave.FileFormats.PC, "ITBEG" },
            new object[] { VCSave.FileFormats.PC, "ITBEG Japan" },
            new object[] { VCSave.FileFormats.PC, "ITBEG StarterSave" },
            new object[] { VCSave.FileFormats.PC, "JOB_5" },
            new object[] { VCSave.FileFormats.PC, "TEX_3" },
            new object[] { VCSave.FileFormats.PC_Steam, "BUD_3" },
            new object[] { VCSave.FileFormats.PC_Steam, "COK_3" },
            new object[] { VCSave.FileFormats.PC_Steam, "FIN_1" },
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
