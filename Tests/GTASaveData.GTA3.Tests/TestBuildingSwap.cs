﻿using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestBuildingSwap : Base<BuildingSwap>
    {
        public override BuildingSwap GenerateTestObject(SaveFileFormat format)
        {
            Faker<BuildingSwap> model = new Faker<BuildingSwap>()
                .RuleFor(x => x.Type, f => f.PickRandom<ObjectType>())
                .RuleFor(x => x.Handle, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.NewModel, f => f.Random.Int(0, 9999))
                .RuleFor(x => x.OldModel, f => f.Random.Int(0, 9999));

            return model.Generate();
        }

        [Fact]
        public void Serialization()
        {
            BuildingSwap x0 = GenerateTestObject();
            BuildingSwap x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Type, x1.Type);
            Assert.Equal(x0.Handle, x1.Handle);
            Assert.Equal(x0.NewModel, x1.NewModel);
            Assert.Equal(x0.OldModel, x1.OldModel);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}