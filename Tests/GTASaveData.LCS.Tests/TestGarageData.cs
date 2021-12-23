using Bogus;
using TestFramework;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.LCS.Tests
{
    public class TestGarageData : Base<GarageData>
    {
        public override GarageData GenerateTestObject(FileType format)
        {
            Faker<GarageData> model = new Faker<GarageData>()
                .RuleFor(x => x.NumGarages, f => f.Random.Int())
                .RuleFor(x => x.FreeBombs, f => f.Random.Bool())
                .RuleFor(x => x.FreeResprays, f => f.Random.Bool())
                .RuleFor(x => x.CarsCollected, f => f.Random.Int())
                .RuleFor(x => x.BankVansCollected, f => f.Random.Int())
                .RuleFor(x => x.PoliceCarsCollected, f => f.Random.Int())
                .RuleFor(x => x.CarTypesCollected, f => f.PickRandom<LoveMediaCars>())
                .RuleFor(x => x.CarTypesCollected2, f => f.Random.Int())
                .RuleFor(x => x.CarTypesCollected3, f => f.Random.Int())
                .RuleFor(x => x.TimeHelpMessageLastShown, f => f.Random.UInt())
                .RuleFor(x => x.CarsInSafeHouse, f => Generator.Array(GarageData.NumStoredCars, g => Generator.Generate<StoredCar, TestStoredCar>()))
                .RuleFor(x => x.Garages, f => Generator.Array(GarageData.MaxNumGarages, g => Generator.Generate<Garage, TestGarage>()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileType format)
        {
            GarageData x0 = GenerateTestObject(format);
            GarageData x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.NumGarages, x1.NumGarages);
            Assert.Equal(x0.FreeBombs, x1.FreeBombs);
            Assert.Equal(x0.FreeResprays, x1.FreeResprays);
            Assert.Equal(x0.CarsCollected, x1.CarsCollected);
            Assert.Equal(x0.BankVansCollected, x1.BankVansCollected);
            Assert.Equal(x0.PoliceCarsCollected, x1.PoliceCarsCollected);
            Assert.Equal(x0.CarTypesCollected, x1.CarTypesCollected);
            Assert.Equal(x0.CarTypesCollected2, x1.CarTypesCollected2);
            Assert.Equal(x0.CarTypesCollected3, x1.CarTypesCollected3);
            Assert.Equal(x0.TimeHelpMessageLastShown, x1.TimeHelpMessageLastShown);
            Assert.Equal(x0.CarsInSafeHouse, x1.CarsInSafeHouse);
            Assert.Equal(x0.Garages, x1.Garages);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            GarageData x0 = GenerateTestObject();
            GarageData x1 = new GarageData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
