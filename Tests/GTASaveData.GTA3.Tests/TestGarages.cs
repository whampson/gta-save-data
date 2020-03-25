using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGarages : GTA3SaveDataObjectTestBase<Garages>
    {
        public override Garages GenerateTestObject(SaveFileFormat format)
        {
            Faker<Garages> model = new Faker<Garages>()
                .RuleFor(x => x.NumberOfGarages, f => f.Random.Int())
                .RuleFor(x => x.BombsAreFree, f => f.Random.Bool())
                .RuleFor(x => x.RespraysAreFree, f => f.Random.Bool())
                .RuleFor(x => x.CarsCollected, f => f.Random.Int())
                .RuleFor(x => x.BankVansCollected, f => f.Random.Int())
                .RuleFor(x => x.PoliceCarsCollected, f => f.Random.Int())
                .RuleFor(x => x.CarTypesCollected1, f => f.PickRandom<CollectCars1>())
                .RuleFor(x => x.CarTypesCollected2, f => f.PickRandom<CollectCars2>())
                .RuleFor(x => x.CarTypesCollected3, f => f.PickRandom<CollectCars3>())
                .RuleFor(x => x.LastTimeHelpMessage, f => f.Random.Int())
                .RuleFor(x => x.StoredCars, f => Generator.CreateArray(Garages.Limits.NumberOfStoredCars, g => Generator.Generate<StoredCar, TestStoredCar>()))
                .RuleFor(x => x.GarageArray, f => Generator.CreateArray(Garages.Limits.NumberOfGarages, g => Generator.Generate<Garage, TestGarage>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Garages x0 = GenerateTestObject();
            Garages x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.NumberOfGarages, x1.NumberOfGarages);
            Assert.Equal(x0.BombsAreFree, x1.BombsAreFree);
            Assert.Equal(x0.RespraysAreFree, x1.RespraysAreFree);
            Assert.Equal(x0.CarsCollected, x1.CarsCollected);
            Assert.Equal(x0.BankVansCollected, x1.BankVansCollected);
            Assert.Equal(x0.PoliceCarsCollected, x1.PoliceCarsCollected);
            Assert.Equal(x0.CarTypesCollected1, x1.CarTypesCollected1);
            Assert.Equal(x0.CarTypesCollected2, x1.CarTypesCollected2);
            Assert.Equal(x0.CarTypesCollected3, x1.CarTypesCollected3);
            Assert.Equal(x0.LastTimeHelpMessage, x1.LastTimeHelpMessage);
            Assert.Equal(x0.StoredCars, x1.StoredCars);
            Assert.Equal(x0.GarageArray, x1.GarageArray);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
