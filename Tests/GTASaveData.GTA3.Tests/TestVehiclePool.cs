using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using System.Collections.Generic;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestVehiclePool : SaveDataObjectTestBase<VehiclePool>
    {
        public override VehiclePool GenerateTestVector(FileFormat format)
        {
            Faker<VehiclePool> model = new Faker<VehiclePool>()
                .RuleFor(x => x.Pool, f => Generator.CreateObjectCollection(1, g => Generator.Generate<VehiclePoolItem, TestVehiclePoolItem>(format)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(SerializationData))]
        public void Serialization(FileFormat format, int carObjSize, int boatObjSize)
        {
            VehiclePool x0 = GenerateTestVector(format);
            VehiclePool x1 = CreateSerializedCopy(x0, out byte[] data, format);

            int numCars = 0, numBoats = 0;
            foreach (var item in x0.Pool)
            {
                if (item.IsBoat)
                {
                    numBoats++;
                }
                else
                {
                    numCars++;
                }
            }

            int expectedSize = (numCars * carObjSize) + (numBoats * boatObjSize) + 8;

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }

        public static IEnumerable<object[]> SerializationData => new[]
        {
            new object[] { GTA3Save.FileFormats.Android, 1462, 1170 },
            new object[] { GTA3Save.FileFormats.IOS, 1462, 1170 },
            new object[] { GTA3Save.FileFormats.PC, 1458, 1166 },
            new object[] { GTA3Save.FileFormats.PS2NAEU, 1626, 1210 },
            new object[] { GTA3Save.FileFormats.PS2AU, 1626, 1210 },
            new object[] { GTA3Save.FileFormats.PS2JP, 1626, 1210 },
            new object[] { GTA3Save.FileFormats.Xbox, 1458, 1166 },
        };
    }
}
