using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using System.Collections.Generic;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestVehicle : SaveDataObjectTestBase<Vehicle>
    {
        public override Vehicle GenerateTestVector(FileFormat format)
        {
            int unknownArray0Size = (format.IsPS2)
                ? Vehicle.Limits.UnknownArray0SizePS2
                : Vehicle.Limits.UnknownArray0Size;
            int unknownArray1Size = (format.IsPS2)
                ? Vehicle.Limits.UnknownArray1SizePS2
                : Vehicle.Limits.UnknownArray1Size;

            Faker<Vehicle> model = new Faker<Vehicle>()
                .RuleFor(x => x.Unknown0, f => f.Random.Int())
                .RuleFor(x => x.ModelId, f => f.Random.Int())
                .RuleFor(x => x.Unknown1, f => f.Random.Int())
                .RuleFor(x => x.UnknownArray0, f => Generator.CreateValueCollection(unknownArray0Size, g => f.Random.Byte()))
                .RuleFor(x => x.Position, f => Generator.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.UnknownArray1, f => Generator.CreateValueCollection(unknownArray1Size, g => f.Random.Byte()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(SerializationData))]
        public void Serialization(FileFormat format)
        {
            int expectedSize = (format.IsPS2)
                ? 1628
                : 1460;

            Vehicle x0 = GenerateTestVector(format);
            Vehicle x1 = CreateSerializedCopy(x0, out byte[] data, format);

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }

        public static IEnumerable<object[]> SerializationData => new[]
        {
            new object[] { GTA3Save.FileFormats.Android },
            new object[] { GTA3Save.FileFormats.IOS},
            new object[] { GTA3Save.FileFormats.PC},
            new object[] { GTA3Save.FileFormats.PS2NAEU },
            new object[] { GTA3Save.FileFormats.PS2AU },
            new object[] { GTA3Save.FileFormats.PS2JP },
            new object[] { GTA3Save.FileFormats.Xbox },
        };
    }
}
