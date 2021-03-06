﻿using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestPickup : Base<Pickup>
    {
        public override Pickup GenerateTestObject(FileFormat format)
        {
            Faker<Pickup> model = new Faker<Pickup>()
                .RuleFor(x => x.Type, f => f.PickRandom<PickupType>())
                .RuleFor(x => x.HasBeenPickedUp, f => f.Random.Bool())
                .RuleFor(x => x.Value, f => f.Random.UShort())
                .RuleFor(x => x.ObjectIndex, f => f.Random.UInt())
                .RuleFor(x => x.RegenerationTime, f => f.Random.UInt())
                .RuleFor(x => x.ModelIndex, f => f.Random.Short())
                .RuleFor(x => x.PoolIndex, f => f.Random.Short())
                .RuleFor(x => x.Position, f => Generator.Vector3(f));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            Pickup x0 = GenerateTestObject(format);
            Pickup x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.HasBeenPickedUp, x1.HasBeenPickedUp);
            Assert.Equal(x0.Value, x1.Value);
            Assert.Equal(x0.ObjectIndex, x1.ObjectIndex);
            Assert.Equal(x0.RegenerationTime, x1.RegenerationTime);
            Assert.Equal(x0.ModelIndex, x1.ModelIndex);
            Assert.Equal(x0.PoolIndex, x1.PoolIndex);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            Pickup x0 = GenerateTestObject();
            Pickup x1 = new Pickup(x0);

            Assert.Equal(x0, x1);
        }
    }
}
