using Bogus;
using GTASaveData.VC;
using GTASaveData.Serialization;
using System.Collections.Generic;
using System.IO;
using TestFramework;
using Xunit;
using GTASaveData.VC.Blocks;
using GTASaveData.Tests.VC.Blocks;

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
                //.RuleFor(x => x.Vehicles, Generator.Generate<VehiclePool, TestVehiclePool>(format))
                //.RuleFor(x => x.Objects, Generator.Generate<Objects, TestObjects>(format))
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
                //.RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo>(format))
                //.RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats>(format))
                //.RuleFor(x => x.Streaming, Generator.Generate<Streaming, TestStreaming>(format))
                //.RuleFor(x => x.PedTypeInfo, Generator.Generate<PedTypeInfo, TestPedTypeInfo>(format))
                ;

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void Serialization(FileFormat format, string path)
        {
            byte[] expected = File.ReadAllBytes(path);

            ViceCitySave x0 = GrandTheftAutoSave.Load<ViceCitySave>(path, format);
            ViceCitySave x1 = CreateSerializedCopy(x0, out byte[] data, format);

            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.CarGenerators, x1.CarGenerators);
            Assert.Equal(x0, x1);
            Assert.Equal(expected.Length, data.Length);
        }

        //[Theory]
        //[MemberData(nameof(SerializationData))]
        //public void Serialization(FileFormat format)
        //{
        //    VCSave x0 = GenerateTestVector(format);
        //    VCSave x1 = CreateSerializedCopy(x0, format);

        //    Assert.Equal(x0, x1);
        //    // TODO: data size check?
        //}

        //[Theory]
        //[MemberData(nameof(FileFormatData))]
        //public void FileTypeDetection(FileFormat expectedFileFormat, string filePath)
        //{
        //    FileFormat detected = VCSave.GetFileFormat(filePath);

        //    Assert.Equal(expectedFileFormat, detected);
        //}

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { ViceCitySave.FileFormats.PCRetail, "../../../../TestData/VC/PCRetail/StarterSave.b" },
        };

        //public static IEnumerable<object[]> SerializationData => new[]
        //{
        //    new object[] { VCSave.FileFormats.Android },
        //    new object[] { VCSave.FileFormats.IOS },
        //    new object[] { VCSave.FileFormats.PC },
        //    new object[] { VCSave.FileFormats.PS2AU },
        //    new object[] { VCSave.FileFormats.PS2JP },
        //    new object[] { VCSave.FileFormats.PS2NAEU },
        //    new object[] { VCSave.FileFormats.Xbox },
        //};
    }
}
