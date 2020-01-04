﻿using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.GTA3
{
    public class TestGTA3SaveData
        : SaveDataObjectTestBase<GTA3SaveData>
    {
        public override GTA3SaveData GenerateTestVector(SystemType system)
        {
            Faker faker = new Faker();

            Faker<GTA3SaveData> model = new Faker<GTA3SaveData>()
                .RuleFor(x => x.SimpleVars, f => TestHelper.Generate<SimpleVars, TestSimpleVars>(system))
                .RuleFor(x => x.Scripts, TestHelper.Generate<Scripts, TestScripts>(system))
                //.RuleFor(x => x.PedPool, TestHelper.Generate<PedPool, TestPedPool>(system))
                .RuleFor(x => x.Garages, TestHelper.Generate<Garages, TestGarages>(system));

            return model.Generate();
        }

        [Theory]
        [InlineData(SystemType.Android)]
        [InlineData(SystemType.IOS)]
        [InlineData(SystemType.PC)]
        [InlineData(SystemType.PS2)]
        [InlineData(SystemType.PS2AU)]
        [InlineData(SystemType.PS2JP)]
        [InlineData(SystemType.Xbox)]
        public void Serialization(SystemType system)
        {
            GTA3SaveData x0 = GenerateTestVector(system);
            GTA3SaveData x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data, system);

            Assert.Equal(x0, x1);
            // TODO: data size check?
        }
    }
}