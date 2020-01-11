using Bogus;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestPickups : SaveDataObjectTestBase<Pickups>
    {
        public override Pickups GenerateTestVector(FileFormat format)
        {
            Faker<Pickups> model = new Faker<Pickups>()
                .RuleFor(x => x.PickupsArray, f => Generator.CreateObjectCollection(Pickups.Limits.PickupsArrayCount, g => Generator.Generate<Pickup, TestPickup>()))
                .RuleFor(x => x.LastCollectedIndex, f => f.Random.UShort())
                .RuleFor(x => x.PickupsCollected, f => Generator.CreateValueCollection(Pickups.Limits.PickupsCollectedCount, g => f.Random.Int()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Pickups x0 = GenerateTestVector();
            Pickups x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(9492, data.Length);
        }
    }
}
