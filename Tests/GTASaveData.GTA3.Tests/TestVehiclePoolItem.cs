using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using System.Collections.Generic;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestVehiclePoolItem : SerializableObjectTestBase<VehiclePoolItem>
    {
        public VehiclePoolItem GenerateVehicleTestVector(FileFormat format)
        {
            Faker<VehiclePoolItem> model = new Faker<VehiclePoolItem>()
                .CustomInstantiator(f => new VehiclePoolItem(false))
                .RuleFor(x => x.ModelId, f => f.PickRandom<VehicleModel>())
                .RuleFor(x => x.VehicleRef, f => f.Random.UInt())
                .RuleFor(x => x.Vehicle, f => Generator.Generate<Automobile, TestAutomobile>(format));

            return model.Generate();
        }

        public VehiclePoolItem GenerateBoatTestVector(FileFormat format)
        {
            Faker<VehiclePoolItem> model = new Faker<VehiclePoolItem>()
                .CustomInstantiator(f => new VehiclePoolItem(true))
                .RuleFor(x => x.ModelId, f => f.PickRandom<VehicleModel>())
                .RuleFor(x => x.VehicleRef, f => f.Random.UInt())
                .RuleFor(x => x.Vehicle, f => Generator.Generate<Boat, TestBoat>(format));

            return model.Generate();
        }

        public override VehiclePoolItem GenerateTestVector(FileFormat format)
        {
            bool isBoat = new Faker().Random.Bool();
            return (isBoat)
                ? GenerateBoatTestVector(format)
                : GenerateVehicleTestVector(format);
        }

        [Theory]
        [MemberData(nameof(SerializationVehicleData))]
        public void SerializationVehicle(FileFormat format, int expectedSize)
        {
            VehiclePoolItem x0 = GenerateVehicleTestVector(format);
            VehiclePoolItem x1 = CreateSerializedCopy(x0, out byte[] data, format);

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }


        [Theory]
        [MemberData(nameof(SerializationBoatData))]
        public void SerializationBoat(FileFormat format, int expectedSize)
        {
            VehiclePoolItem x0 = GenerateBoatTestVector(format);
            VehiclePoolItem x1 = CreateSerializedCopy(x0, out byte[] data, format);

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }

        public static IEnumerable<object[]> SerializationVehicleData => new[]
        {
            new object[] { GTA3Save.FileFormats.Android, 1462 },
            new object[] { GTA3Save.FileFormats.IOS, 1462, },
            new object[] { GTA3Save.FileFormats.PC, 1458, },
            new object[] { GTA3Save.FileFormats.PS2NAEU, 1626, },
            new object[] { GTA3Save.FileFormats.PS2AU, 1626, },
            new object[] { GTA3Save.FileFormats.PS2JP, 1626, },
            new object[] { GTA3Save.FileFormats.Xbox, 1458, },
        };

        public static IEnumerable<object[]> SerializationBoatData => new[]
        {
            new object[] { GTA3Save.FileFormats.Android, 1170 },
            new object[] { GTA3Save.FileFormats.IOS, 1170 },
            new object[] { GTA3Save.FileFormats.PC, 1166 },
            new object[] { GTA3Save.FileFormats.PS2NAEU, 1210 },
            new object[] { GTA3Save.FileFormats.PS2AU, 1210 },
            new object[] { GTA3Save.FileFormats.PS2JP, 1210 },
            new object[] { GTA3Save.FileFormats.Xbox, 1166 },
        };
    }
}
