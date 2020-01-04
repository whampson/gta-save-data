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

        public static IEnumerable<object[]> SerializationData => new[]
        {
            new object[] { GTA3SaveData.FileFormats.Android },
            new object[] { GTA3SaveData.FileFormats.IOS },
            new object[] { GTA3SaveData.FileFormats.PC },
            new object[] { GTA3SaveData.FileFormats.PS2 },
            new object[] { GTA3SaveData.FileFormats.PS2AU },
            new object[] { GTA3SaveData.FileFormats.PS2JP },
            new object[] { GTA3SaveData.FileFormats.Xbox },
        };
    }
}
