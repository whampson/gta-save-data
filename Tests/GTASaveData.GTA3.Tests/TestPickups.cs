using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPickups : GTA3SaveDataObjectTestBase<Pickups>
    {
        public override Pickups GenerateTestObject(SaveFileFormat format)
        {
            Faker<Pickups> model = new Faker<Pickups>()
                .RuleFor(x => x.PickupArray, f => Generator.CreateArray(Pickups.Limits.NumberOfPickups, g => Generator.Generate<Pickup, TestPickup>()))
                .RuleFor(x => x.LastCollectedIndex, f => f.Random.Short())
                .RuleFor(x => x.PickupsCollected, f => Generator.CreateArray(Pickups.Limits.NumberOfPickupsCollected, g => f.Random.Int()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Pickups x0 = GenerateTestObject();
            Pickups x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.PickupArray, x1.PickupArray);
            Assert.Equal(x0.LastCollectedIndex, x1.LastCollectedIndex);
            Assert.Equal(x0.PickupsCollected, x1.PickupsCollected);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
