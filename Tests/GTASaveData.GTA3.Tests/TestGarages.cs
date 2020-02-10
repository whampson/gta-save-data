using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestGarages : SaveDataObjectTestBase<Garages>
    {
        public override Garages GenerateTestVector(FileFormat format)
        {
            Faker<Garages> model = new Faker<Garages>()
                .RuleFor(x => x.NumberOfGarages, f => f.Random.Int())
                .RuleFor(x => x.FreeBombs, f => f.Random.Bool())
                .RuleFor(x => x.FreeResprays, f => f.Random.Bool())
                .RuleFor(x => x.CarsCollected, f => f.Random.Int())
                .RuleFor(x => x.BankVansCollected, f => f.Random.Int())
                .RuleFor(x => x.PoliceCarsCollected, f => f.Random.Int())
                .RuleFor(x => x.CarTypesCollected1, f => f.PickRandom<CollectCars1>())
                .RuleFor(x => x.CarTypesCollected2, f => f.PickRandom<CollectCars2>())
                .RuleFor(x => x.CarTypesCollected3, f => f.PickRandom<CollectCars3>())
                .RuleFor(x => x.LastTimeHelpMessage, f => f.Random.Int())
                .RuleFor(x => x.StoredCarSlots, f => Generator.CreateObjectCollection(Garages.Limits.StoredCarSlotsCount, g => Generator.Generate<StoredCarSlot, TestStoredCarSlot>()))
                .RuleFor(x => x.GarageObjects, f => Generator.CreateObjectCollection(Garages.Limits.GarageObjectsCount, g => Generator.Generate<Garage, TestGarage>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Garages x0 = GenerateTestVector();
            Garages x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(5240, data.Length);
        }
    }
}
