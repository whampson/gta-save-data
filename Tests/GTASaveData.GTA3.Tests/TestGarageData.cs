using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGarageData : Base<GarageData>
    {
        public override GarageData GenerateTestObject(SaveFileFormat format)
        {
            Faker<GarageData> model = new Faker<GarageData>()
                .RuleFor(x => x.NumGarages, f => f.Random.Int())
                .RuleFor(x => x.BombsAreFree, f => f.Random.Bool())
                .RuleFor(x => x.RespraysAreFree, f => f.Random.Bool())
                .RuleFor(x => x.CarsCollected, f => f.Random.Int())
                .RuleFor(x => x.BankVansCollected, f => f.Random.Int())
                .RuleFor(x => x.PoliceCarsCollected, f => f.Random.Int())
                .RuleFor(x => x.CarTypesCollected1, f => f.PickRandom<CollectCars1>())
                .RuleFor(x => x.CarTypesCollected2, f => f.PickRandom<CollectCars2>())
                .RuleFor(x => x.CarTypesCollected3, f => f.PickRandom<CollectCars3>())
                .RuleFor(x => x.LastTimeHelpMessage, f => f.Random.Int())
                .RuleFor(x => x.CarsInSafeHouse, f => Generator.CreateArray(GarageData.Limits.NumberOfCarsPerSafeHouse * GarageData.Limits.NumberOfSafeHouses, g => Generator.Generate<StoredCar, TestStoredCar>()))
                .RuleFor(x => x.Garages, f => Generator.CreateArray(GarageData.Limits.NumberOfGarages, g => Generator.Generate<Garage, TestGarage>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            GarageData x0 = GenerateTestObject();
            GarageData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.NumGarages, x1.NumGarages);
            Assert.Equal(x0.BombsAreFree, x1.BombsAreFree);
            Assert.Equal(x0.RespraysAreFree, x1.RespraysAreFree);
            Assert.Equal(x0.CarsCollected, x1.CarsCollected);
            Assert.Equal(x0.BankVansCollected, x1.BankVansCollected);
            Assert.Equal(x0.PoliceCarsCollected, x1.PoliceCarsCollected);
            Assert.Equal(x0.CarTypesCollected1, x1.CarTypesCollected1);
            Assert.Equal(x0.CarTypesCollected2, x1.CarTypesCollected2);
            Assert.Equal(x0.CarTypesCollected3, x1.CarTypesCollected3);
            Assert.Equal(x0.LastTimeHelpMessage, x1.LastTimeHelpMessage);
            Assert.Equal(x0.CarsInSafeHouse, x1.CarsInSafeHouse);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
