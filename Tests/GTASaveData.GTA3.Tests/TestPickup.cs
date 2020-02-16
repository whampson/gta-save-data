using Bogus;
using GTASaveData.Common;
using GTASaveData.Core.Tests.Common;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestPickup : SerializableObjectTestBase<Pickup>
    {
        public override Pickup GenerateTestVector(FileFormat format)
        {
            Faker<Pickup> model = new Faker<Pickup>()
                .RuleFor(x => x.Type, f => f.PickRandom<PickupType>())
                .RuleFor(x => x.HasBeenPickedUp, f => f.Random.Bool())
                .RuleFor(x => x.Amount, f => f.Random.UShort())
                .RuleFor(x => x.ObjectIndex, f => f.Random.UInt())
                .RuleFor(x => x.RegenerationTime, f => f.Random.UInt())
                .RuleFor(x => x.ModelId, f => f.Random.UShort())
                .RuleFor(x => x.Flags, f => f.Random.UShort())
                .RuleFor(x => x.Position, f => Generator.Generate<Vector3d, TestVector3d>());

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            Pickup x0 = GenerateTestVector();
            Pickup x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.HasBeenPickedUp, x1.HasBeenPickedUp);
            Assert.Equal(x0.Amount, x1.Amount);
            Assert.Equal(x0.ObjectIndex, x1.ObjectIndex);
            Assert.Equal(x0.RegenerationTime, x1.RegenerationTime);
            Assert.Equal(x0.ModelId, x1.ModelId);
            Assert.Equal(x0.Flags, x1.Flags);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0, x1);
            Assert.Equal(28, data.Length);
        }
    }
}
