using Bogus;
using GTASaveData.GTA3;
using GTASaveData.GTA3.Blocks;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3.Blocks
{
    public class TestGarageBlock : SerializableObjectTestBase<GarageBlock>
    {
        public override GarageBlock GenerateTestVector(FileFormat format)
        {
            Faker<GarageBlock> model = new Faker<GarageBlock>()
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
                .RuleFor(x => x.StoredCars, f => Generator.CreateArray(GarageBlock.Limits.StoredCarsCount, g => Generator.Generate<StoredCar, TestStoredCar>()))
                .RuleFor(x => x.Garages, f => Generator.CreateArray(GarageBlock.Limits.GaragesCount, g => Generator.Generate<Garage, TestGarage>()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            GarageBlock x0 = GenerateTestVector();
            GarageBlock x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.NumberOfGarages, x1.NumberOfGarages);
            Assert.Equal(x0.FreeBombs, x1.FreeBombs);
            Assert.Equal(x0.FreeResprays, x1.FreeResprays);
            Assert.Equal(x0.CarsCollected, x1.CarsCollected);
            Assert.Equal(x0.BankVansCollected, x1.BankVansCollected);
            Assert.Equal(x0.PoliceCarsCollected, x1.PoliceCarsCollected);
            Assert.Equal(x0.CarTypesCollected1, x1.CarTypesCollected1);
            Assert.Equal(x0.CarTypesCollected2, x1.CarTypesCollected2);
            Assert.Equal(x0.CarTypesCollected3, x1.CarTypesCollected3);
            Assert.Equal(x0.LastTimeHelpMessage, x1.LastTimeHelpMessage);
            Assert.Equal(x0.StoredCars, x1.StoredCars);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0, x1);
            Assert.Equal(5240, data.Length);
        }
    }
}
