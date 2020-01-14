using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using System.Collections.Generic;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestCar : SaveDataObjectTestBase<Car>
    {
        public override Car GenerateTestVector(FileFormat format)
        {
            Faker<Car> model = new Faker<Car>()
                .RuleFor(x => x.UnknownArray0, f => Generator.CreateValueCollection(Car.Limits.GetUnknownArray0Size(format), g => f.Random.Byte()))
                .RuleFor(x => x.Position, f => Generator.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.UnknownArray1, f => Generator.CreateValueCollection(Car.Limits.GetUnknownArray1Size(format), g => f.Random.Byte()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(SerializationData))]
        public void Serialization(FileFormat format, int expectedSize)
        {
            Car x0 = GenerateTestVector(format);
            Car x1 = CreateSerializedCopy(x0, out byte[] data, format);

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }

        public static IEnumerable<object[]> SerializationData => new[]
        {
            new object[] { GTA3Save.FileFormats.Android, 1452 },
            new object[] { GTA3Save.FileFormats.IOS, 1452 },
            new object[] { GTA3Save.FileFormats.PC, 1448 },
            new object[] { GTA3Save.FileFormats.PS2NAEU, 1616 },
            new object[] { GTA3Save.FileFormats.PS2AU, 1616 },
            new object[] { GTA3Save.FileFormats.PS2JP, 1616 },
            new object[] { GTA3Save.FileFormats.Xbox, 1448 },
        };
    }
}
