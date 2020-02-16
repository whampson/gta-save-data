using Bogus;
using GTASaveData.GTA3;
using GTASaveData.GTA3.Blocks;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3.Blocks
{
    public class TestPickupBlock : SerializableObjectTestBase<PickupBlock>
    {
        public override PickupBlock GenerateTestVector(FileFormat format)
        {
            Faker<PickupBlock> model = new Faker<PickupBlock>()
                .RuleFor(x => x.Pickups, f => Generator.CreateArray(PickupBlock.Limits.PickupsCount, g => Generator.Generate<Pickup, TestPickup>()))
                .RuleFor(x => x.LastCollectedIndex, f => f.Random.UShort())
                .RuleFor(x => x.PickupsCollected, f => Generator.CreateArray(PickupBlock.Limits.PickupsCollectedCount, g => f.Random.Int()));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            PickupBlock x0 = GenerateTestVector();
            PickupBlock x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Pickups, x1.Pickups);
            Assert.Equal(x0.LastCollectedIndex, x1.LastCollectedIndex);
            Assert.Equal(x0.PickupsCollected, x1.PickupsCollected);
            Assert.Equal(x0, x1);
            Assert.Equal(9492, data.Length);
        }
    }
}
