﻿using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestCollective : SaveDataObjectTestBase<Collective>
    {
        public override Collective GenerateTestObject(FileFormat format)
        {
            Faker<Collective> model = new Faker<Collective>()
                .RuleFor(x => x.Index, f => f.Random.Int())
                .RuleFor(x => x.Field04h, f => f.Random.Int());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            Collective x0 = GenerateTestObject();
            Collective x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Index, x1.Index);
            Assert.Equal(x0.Field04h, x1.Field04h);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }


        [Fact]
        public void CopyConstructor()
        {
            Collective x0 = GenerateTestObject();
            Collective x1 = new Collective(x0);

            Assert.Equal(x0, x1);
        }
    }
}
