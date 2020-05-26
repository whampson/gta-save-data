using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPickupData : Base<PickupData>
    {
        public override PickupData GenerateTestObject(FileFormat format)
        {
            Faker<PickupData> model = new Faker<PickupData>()
                .RuleFor(x => x.Pickups, f => Generator.Array(PickupData.Limits.MaxNumPickups, g => Generator.Generate<Pickup, TestPickup>()))
                .RuleFor(x => x.LastCollectedIndex, f => f.Random.Short())
                .RuleFor(x => x.PickupsCollected, f => Generator.Array(PickupData.Limits.MaxNumCollectedPickups, g => f.Random.Int()));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
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
