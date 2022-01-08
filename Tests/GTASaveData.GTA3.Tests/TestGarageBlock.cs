using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGarageBlock : Base<GarageBlock>
    {
        public override GarageBlock GenerateTestObject(GTA3SaveParams p)
        {
            Faker<GarageBlock> model = new Faker<GarageBlock>()
                .RuleFor(x => x.NumGarages, f => f.Random.Int())
                .RuleFor(x => x.FreeBombs, f => f.Random.Bool())
                .RuleFor(x => x.FreeResprays, f => f.Random.Bool())
                .RuleFor(x => x.CarsCollected, f => f.Random.Int())
                .RuleFor(x => x.BankVansCollected, f => f.Random.Int())
                .RuleFor(x => x.PoliceCarsCollected, f => f.Random.Int())
                .RuleFor(x => x.CarTypesCollected1, f => f.PickRandom<CollectCars1Types>())
                .RuleFor(x => x.CarTypesCollected2, f => f.PickRandom<CollectCars2Types>())
                .RuleFor(x => x.CarTypesCollected3, f => (CollectCar3Types) f.Random.Int())
                .RuleFor(x => x.LastTimeHelpMessage, f => f.Random.UInt())
                .RuleFor(x => x.CarsInSafeHouse, f => Generator.Array(GarageBlock.NumStoredCars, g => Generator.Generate<StoredCar, TestStoredCar, GTA3SaveParams>(p)))
                .RuleFor(x => x.Garages, f => Generator.Array(p.NumGarages, g => Generator.Generate<Garage,  TestGarage, GTA3SaveParams>(p)));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            GarageBlock x0 = GenerateTestObject(p);
            GarageBlock x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.NumGarages, x1.NumGarages);
            Assert.Equal(x0.FreeBombs, x1.FreeBombs);
            Assert.Equal(x0.FreeResprays, x1.FreeResprays);
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
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }


        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            GarageBlock x0 = GenerateTestObject(p);
            GarageBlock x1 = new GarageBlock(x0);

            Assert.Equal(x0, x1);
        }

        public override int GetSizeOfTestObject(GarageBlock obj, GTA3SaveParams p)
        {
            return 0x156C;
        }
    }
}
