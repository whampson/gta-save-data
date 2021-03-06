﻿using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.VC.Tests
{
    public class TestCarGeneratorData : Base<CarGeneratorData>
    {
        public override CarGeneratorData GenerateTestObject(FileFormat format)
        {
            Faker<CarGeneratorData> model = new Faker<CarGeneratorData>()
                .RuleFor(x => x.NumberOfCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.NumberOfEnabledCarGenerators, f => f.Random.Int())
                .RuleFor(x => x.ProcessCounter, f => f.Random.Byte())
                .RuleFor(x => x.GenerateEvenIfPlayerIsCloseCounter, f => f.Random.Byte())
                .RuleFor(x => x.CarGenerators, f => Generator.Array(CarGeneratorData.MaxNumCarGenerators, g => Generator.Generate<CarGenerator, TestCarGenerator>()));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            CarGeneratorData x0 = GenerateTestObject();
            CarGeneratorData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.NumberOfCarGenerators, x1.NumberOfCarGenerators);
            Assert.Equal(x0.NumberOfEnabledCarGenerators, x1.NumberOfEnabledCarGenerators);
            Assert.Equal(x0.ProcessCounter, x1.ProcessCounter);
            Assert.Equal(x0.GenerateEvenIfPlayerIsCloseCounter, x1.GenerateEvenIfPlayerIsCloseCounter);
            Assert.Equal(x0.CarGenerators, x1.CarGenerators);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            CarGeneratorData x0 = GenerateTestObject();
            CarGeneratorData x1 = new CarGeneratorData(x0);

            Assert.Equal(x0, x1);
        }
    }
}
