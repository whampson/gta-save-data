using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestVehiclePool : Base<VehiclePool>
    {
        public override VehiclePool GenerateTestObject(FileFormat format)
        {
            Faker<VehiclePool> model = new Faker<VehiclePool>()
                .RuleFor(x => x.Cars, f => Generator.Array(f.Random.Int(1, 15), g => Generator.Generate<Automobile, TestAutomobile>()))
                .RuleFor(x => x.Boats, f => Generator.Array(f.Random.Int(1, 15), g => Generator.Generate<Boat, TestBoat>()));

            return model.Generate();
        }

        [Fact]
        public void CopyConstructor()
        {
            VehiclePool x0 = GenerateTestObject();
            VehiclePool x1 = new VehiclePool(x0);

            Assert.Equal(x0, x1);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            VehiclePool x0 = GenerateTestObject(format);
            VehiclePool x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Cars, x1.Cars);
            Assert.Equal(x0.Boats, x1.Boats);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }
    }
}
