using Bogus;
using TestFramework;
using Xunit;
using System.Linq;
using System;
using System.IO;

namespace GTASaveData.VC.Tests
{
    public class TestSaveFileVC : Base<SaveFileVC>
    {
        public override SaveFileVC GenerateTestObject(FileFormat format)
        {
            Faker<SaveFileVC> model = new Faker<SaveFileVC>()
                .RuleFor(x => x.FileFormat, format)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables>(format))
                //.RuleFor(x => x.Scripts, Generator.Generate<TheScripts, TestTheScripts>(format))
                .RuleFor(x => x.PedPool, Generator.Generate<PedPool, TestPedPool>(format))
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
                .RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats>(format))
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
            SaveFile.GetFileFormat<SaveFileVC>(path, out FileFormat detectedFormat);

            Assert.Equal(expectedFormat, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            using SaveFileVC x0 = GenerateTestObject(format);
            using SaveFileVC x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, format);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataSerialization(FileFormat format, string filename)
        {
            string path = TestData.GetTestDataPath(Game.VC, format, filename);

            using SaveFileVC x0 = SaveFile.Load<SaveFileVC>(path, format);
            using SaveFileVC x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, format);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            SaveFileVC x0 = GenerateTestObject();
            SaveFileVC x1 = new SaveFileVC(x0);

            Assert.Equal(x0, x1);
        }

        [Fact]
        public void BlockBounds()
        {
            string path = TestData.GetTestDataPath(Game.VC, SaveFileVC.FileFormats.PC, "COK_2");
            byte[] data = File.ReadAllBytes(path);

            // Fudge the block size
            data[0] = 0xBE;
            data[1] = 0xBA;
            data[2] = 0xFE;
            data[3] = 0xCA;
            Assert.Throws<SerializationException>(() => SaveFile.Load<SaveFileVC>(data, SaveFileVC.FileFormats.PC));

            // TODO: uncomment when Scripts done
            // Make the script space huge
            //VCSave x = SaveData.Load<VCSave>(path, VCSave.FileFormats.PC);
            //x.Scripts.ScriptSpace = GTAObject.CreateArray<byte>(100000);
            //Assert.Throws<SerializationException>(() => x.Save(out byte[] _));
        }

        private void AssertSavesAreEqual(SaveFileVC x0, SaveFileVC x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.PedPool, x1.PedPool);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.GameLogic, x1.GameLogic);
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
            Assert.Equal(x0.ScriptPaths, x1.ScriptPaths);
            Assert.Equal(x0.PlayerInfo, x1.PlayerInfo);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0.SetPieces, x1.SetPieces);
            Assert.Equal(x0.Streaming, x1.Streaming);
            Assert.Equal(x0.PedTypeInfo, x1.PedTypeInfo);
            Assert.Equal(x0, x1);
        }

        private void AssertCheckSumValid(byte[] data, FileFormat format)
        {
            int sumOffset = (format.IsXbox) ? data.Length - 24 : data.Length - 4;
            int calculatedSum = data.Take(sumOffset).Sum(x => x);
            int storedSum = BitConverter.ToInt32(data, sumOffset);
            Assert.Equal(calculatedSum, storedSum);
        }
    }
}
