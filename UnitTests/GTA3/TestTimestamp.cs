using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using System;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
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
        public void Sanity()
        {
            Timestamp t0 = GenerateTestVector();
            Timestamp t1 = TestHelper.CreateSerializedCopy(t0);

            Assert.Equal(t0, t1);
        }
    }
}
