using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestVehiclePool : Base<VehiclePool>
    {
        public override VehiclePool GenerateTestObject(GTA3SaveParams p)
        {
            Faker<VehiclePool> model = new Faker<VehiclePool>()
                .RuleFor(x => x.Cars, f => Generator.Array(f.Random.Int(1, 5), g => Generator.Generate<Automobile, TestAutomobile, GTA3SaveParams>(p)))
                .RuleFor(x => x.Boats, f => Generator.Array(f.Random.Int(1, 5), g => Generator.Generate<Boat, TestBoat, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            VehiclePool x0 = GenerateTestObject(p);
            VehiclePool x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Cars, x1.Cars);
            Assert.Equal(x0.Boats, x1.Boats);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            VehiclePool x0 = GenerateTestObject(p);
            VehiclePool x1 = new VehiclePool(x0);

            Assert.Equal(x0, x1);
        }
    }
}
