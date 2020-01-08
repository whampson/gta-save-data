using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using System.Collections.Generic;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestGTA3SaveData : SaveDataObjectTestBase<GTA3SaveData>
    {
        public override GTA3SaveData GenerateTestVector(FileFormat format)
        {
            Faker faker = new Faker();

            Faker<GTA3SaveData> model = new Faker<GTA3SaveData>()
                .RuleFor(x => x.SimpleVars, f => Generator.Generate<SimpleVars, TestSimpleVars>(format))
                .RuleFor(x => x.Scripts, Generator.Generate<Scripts, TestScripts>(format))
                //.RuleFor(x => x.PedPool, TestHelper.Generate<PedPool, TestPedPool>(format))
                .RuleFor(x => x.Garages, Generator.Generate<Garages, TestGarages>(format));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(SerializationData))]
        public void Serialization(FileFormat format)
        {
            GTA3SaveData x0 = GenerateTestVector(format);
            GTA3SaveData x1 = CreateSerializedCopy(x0, format);

            Assert.Equal(x0, x1);
            // TODO: data size check?
        }

        [Theory]
        [MemberData(nameof(FileFormatData))]
        public void FileTypeDetection(FileFormat expectedFileFormat, string filePath)
        {
            FileFormat detected = GTA3SaveData.GetFileFormat(filePath);

            Assert.Equal(expectedFileFormat, detected);
        }

        public static IEnumerable<object[]> FileFormatData => new[]
        {
            new object[] { GTA3SaveData.FileFormats.PC, "./TestData/GTA3/PC/1_JM4" },
            new object[] { GTA3SaveData.FileFormats.PC, "./TestData/GTA3/PC/2_AS3" },
            new object[] { GTA3SaveData.FileFormats.PS2AU, "./TestData/GTA3/PS2AU/1_T4X4_2" },
            new object[] { GTA3SaveData.FileFormats.PS2AU, "./TestData/GTA3/PS2AU/2_AS3" },
            new object[] { GTA3SaveData.FileFormats.PS2AU, "./TestData/GTA3/PS2AU/3_CAT2" },
            new object[] { GTA3SaveData.FileFormats.PS2JP, "./TestData/GTA3/PS2JP/1_LM1" },
            new object[] { GTA3SaveData.FileFormats.PS2JP, "./TestData/GTA3/PS2JP/2_LM2" },
            new object[] { GTA3SaveData.FileFormats.PS2NAEU, "./TestData/GTA3/PS2NAEU/1_T4X4_1" },
            new object[] { GTA3SaveData.FileFormats.PS2NAEU, "./TestData/GTA3/PS2NAEU/2_LM1" },
            new object[] { GTA3SaveData.FileFormats.PS2NAEU, "./TestData/GTA3/PS2NAEU/3_CAT2" },
        };

        public static IEnumerable<object[]> SerializationData => new[]
        {
            new object[] { GTA3SaveData.FileFormats.Android },
            new object[] { GTA3SaveData.FileFormats.IOS },
            new object[] { GTA3SaveData.FileFormats.PC },
            new object[] { GTA3SaveData.FileFormats.PS2AU },
            new object[] { GTA3SaveData.FileFormats.PS2JP },
            new object[] { GTA3SaveData.FileFormats.PS2NAEU },
            new object[] { GTA3SaveData.FileFormats.Xbox },
        };
    }
}
