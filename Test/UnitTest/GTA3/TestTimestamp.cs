using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using System;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.GTA3
{
    public class TestTimestamp
        : SaveDataObjectTestBase<Timestamp>
    {
        public override Timestamp GenerateTestVector(SystemType system)
        {
            return new Timestamp(
                new Faker().Date.Between(new DateTime(1970, 1, 1), DateTime.Now));
        }

        [Fact]
        public void Serialization()
        {
            Timestamp x0 = GenerateTestVector();
            Timestamp x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(24, data.Length);
        }
    }
}
