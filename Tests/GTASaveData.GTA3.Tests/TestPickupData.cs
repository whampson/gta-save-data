using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPickupData : Base<PickupData>
    {
        public override PickupData GenerateTestObject(GTA3SaveParams p)
        {
            Faker<PickupData> model = new Faker<PickupData>()
                .RuleFor(x => x.Pickups, f => Generator.Array(PickupData.MaxNumPickups, g => Generator.Generate<Pickup, TestPickup, GTA3SaveParams>(p)))
                .RuleFor(x => x.LastCollectedIndex, f => f.Random.Short())
                .RuleFor(x => x.PickupsCollected, f => Generator.Array(PickupData.MaxNumCollectedPickups, g => f.Random.Int()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void RandomDataSerialization(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PickupData x0 = GenerateTestObject(p);
            PickupData x1 = CreateSerializedCopy(x0, p, out byte[] data);

            Assert.Equal(x0.Pickups, x1.Pickups);
            Assert.Equal(x0.LastCollectedIndex, x1.LastCollectedIndex);
            Assert.Equal(x0.PickupsCollected, x1.PickupsCollected);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        [Theory]
        [MemberData(nameof(FileTypes))]
        public void CopyConstructor(FileType t)
        {
            GTA3SaveParams p = GTA3SaveParams.GetDefaults(t);
            PickupData x0 = GenerateTestObject(p);
            PickupData x1 = new PickupData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
