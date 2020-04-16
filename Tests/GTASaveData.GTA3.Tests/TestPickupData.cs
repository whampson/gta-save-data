using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPickupData : Base<PickupData>
    {
        public override PickupData GenerateTestObject(DataFormat format)
        {
            Faker<PickupData> model = new Faker<PickupData>()
                .RuleFor(x => x.Pickups, f => Generator.CreateArray(PickupData.Limits.NumberOfPickups, g => Generator.Generate<Pickup, TestPickup>()))
                .RuleFor(x => x.LastCollectedIndex, f => f.Random.Short())
                .RuleFor(x => x.PickupsCollected, f => Generator.CreateArray(PickupData.Limits.NumberOfCollectedPickups, g => f.Random.Int()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            PickupData x0 = GenerateTestObject();
            PickupData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Pickups, x1.Pickups);
            Assert.Equal(x0.LastCollectedIndex, x1.LastCollectedIndex);
            Assert.Equal(x0.PickupsCollected, x1.PickupsCollected);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
