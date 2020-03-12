using Bogus;
using GTASaveData.VC;
using GTASaveData.Serialization;
using System.Collections.Generic;
using System.IO;
using TestFramework;
using Xunit;
using GTASaveData.VC.Blocks;
using GTASaveData.Tests.VC.Blocks;
using System.Linq;
using System;

namespace GTASaveData.Tests.VC
{
    public class TestVCSave : SerializableObjectTestBase<ViceCitySave>
    {
        public override ViceCitySave GenerateTestVector(FileFormat format)
        {
            Faker faker = new Faker();

            Faker<ViceCitySave> model = new Faker<ViceCitySave>()
                .RuleFor(x => x.SimpleVars, f => Generator.Generate<SimpleVars, TestSimpleVars>(format))
                //.RuleFor(x => x.Scripts, Generator.Generate<Scripts, TestScripts>(format))
                //.RuleFor(x => x.PedPool, TestHelper.Generate<PedPool, TestPedPool>(format))
                //.RuleFor(x => x.Garages, Generator.Generate<Garages, TestGarages>(format))
                //.RuleFor(x => x.GameLogic, Generator.Generate<GameLogic, TestGameLogic>(format))
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
                .RuleFor(x => x.CarGenerators, Generator.Generate<CarGeneratorBlock, TestCarGeneratorBlock>(format))
                //.RuleFor(x => x.Particles, Generator.Generate<Particles, TestParticles>(format))
                //.RuleFor(x => x.AudioScriptObjects, Generator.Generate<AudioScriptObjects, TestAudioScriptObjects>(format))
                //.RuleFor(x => x.ScriptPaths, Generator.Generate<ScriptPaths, TestScriptPaths>(format))
                //.RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo>(format))
                //.RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats>(format))
                //.RuleFor(x => x.SetPieces, Generator.Generate<SetPieces, TestSetPieces>(format))
                //.RuleFor(x => x.Streaming, Generator.Generate<Streaming, TestStreaming>(format))
                //.RuleFor(x => x.PedTypeInfo, Generator.Generate<PedTypeInfo, TestPedTypeInfo>(format))
                ;

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void FileFormatDetection(FileFormat expected, string filename)
        {
            string path = TestData.GetTestDataPath(Game.ViceCity, expected, filename);

            FileFormat detected = GrandTheftAutoSave.GetFileFormat<ViceCitySave>(path);

            Assert.Equal(expected, detected);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            ViceCitySave x0 = GenerateTestVector(format);
            ViceCitySave x1 = CreateSerializedCopy(x0, out byte[] data, format);

            AssertEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(_ => _);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataSerialization(FileFormat format, string filename)
        {
            string path = TestData.GetTestDataPath(Game.ViceCity, format, filename);

            ViceCitySave x0 = GrandTheftAutoSave.Load<ViceCitySave>(path);
            ViceCitySave x1 = CreateSerializedCopy(x0, out byte[] data, format);

            AssertEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(_ => _);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        private void AssertEqual(ViceCitySave x0, ViceCitySave x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.PedPool, x1.PedPool);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.GameLogic, x1.GameLogic);
            Assert.Equal(x0.VehiclePool, x1.VehiclePool);
            Assert.Equal(x0.ObjectPool, x1.ObjectPool);
            Assert.Equal(x0.PathFind, x1.PathFind);
            Assert.Equal(x0.Cranes, x1.Cranes);
            Assert.Equal(x0.Pickups, x1.Pickups);
            Assert.Equal(x0.PhoneInfo, x1.PhoneInfo);
            Assert.Equal(x0.RestartPoints, x1.RestartPoints);
            Assert.Equal(x0.RadarBlips, x1.RadarBlips);
            Assert.Equal(x0.Zones, x1.Zones);
            Assert.Equal(x0.GangData, x1.GangData);
            Assert.Equal(x0.CarGenerators, x1.CarGenerators);
            Assert.Equal(x0.Particles, x1.Particles);
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
            new object[] { ViceCitySave.FileFormats.PCRetail, "ITBEG" },
            new object[] { ViceCitySave.FileFormats.PCRetail, "ITBEG_JP" },
            new object[] { ViceCitySave.FileFormats.PCRetail, "PROTEC3" },
            new object[] { ViceCitySave.FileFormats.PCSteam, "BARON3" },
            new object[] { ViceCitySave.FileFormats.PCRetail, "PROTEC3" },
            // TODO: PS2
        };

        public static IEnumerable<object[]> FileFormats => new[]
        {
            new object[] { ViceCitySave.FileFormats.PCRetail },
            new object[] { ViceCitySave.FileFormats.PCSteam },
            //new object[] { ViceCitySave.FileFormats.PS2 },
        };
    }
}
