using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPickup : Base<Pickup>
    {
        public override Pickup GenerateTestObject(DataFormat format)
        {
            Faker<Pickup> model = new Faker<Pickup>()
                .RuleFor(x => x.Type, f => f.PickRandom<PickupType>())
                .RuleFor(x => x.HasBeenPickedUp, f => f.Random.Bool())
                .RuleFor(x => x.Quantity, f => f.Random.UShort())
                .RuleFor(x => x.Handle, f => f.Random.UInt())
                .RuleFor(x => x.RegenerationTime, f => f.Random.UInt())
                .RuleFor(x => x.ModelIndex, f => f.Random.Short())
                .RuleFor(x => x.PickupIndex, f => f.Random.Short())
                .RuleFor(x => x.Position, f => Generator.Generate<Vector, TestVector>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Pickup x0 = GenerateTestObject();
            Pickup x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.HasBeenPickedUp, x1.HasBeenPickedUp);
            Assert.Equal(x0.Quantity, x1.Quantity);
            Assert.Equal(x0.Handle, x1.Handle);
            Assert.Equal(x0.RegenerationTime, x1.RegenerationTime);
            Assert.Equal(x0.ModelIndex, x1.ModelIndex);
            Assert.Equal(x0.PickupIndex, x1.PickupIndex);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
