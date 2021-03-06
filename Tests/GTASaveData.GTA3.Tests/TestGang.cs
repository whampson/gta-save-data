﻿using Bogus;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestGang : Base<Gang>
    {
        public override Gang GenerateTestObject(FileFormat format)
        {
            Faker<Gang> model = new Faker<Gang>()
                .RuleFor(x => x.VehicleModel, f => f.Random.Int())
                .RuleFor(x => x.PedModelOverride, f => f.Random.SByte())
                .RuleFor(x => x.Weapon1, f => f.PickRandom<WeaponType>())
                .RuleFor(x => x.Weapon2, f => f.PickRandom<WeaponType>());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            Gang x0 = GenerateTestObject(format);
            Gang x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.VehicleModel, x1.VehicleModel);
            Assert.Equal(x0.PedModelOverride, x1.PedModelOverride);
            Assert.Equal(x0.Weapon1, x1.Weapon1);
            Assert.Equal(x0.Weapon2, x1.Weapon2);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            Gang x0 = GenerateTestObject();
            Gang x1 = new Gang(x0);

            Assert.Equal(x0, x1);
        }
    }
}
