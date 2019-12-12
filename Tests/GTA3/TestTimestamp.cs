using Bogus;
using GTASaveData.GTA3;
using System;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestTimestamp
    {
        public static Timestamp Generate()
        {
            return new Timestamp(
                new Faker().Date.Between(new DateTime(1970, 1, 1), DateTime.Now));
        }

        [Fact]
        public void Sanity()
        {
            Timestamp t0 = Generate();
            Timestamp t1 = TestHelper.CreateSerializedCopy(t0);

            Assert.Equal(t0, t1);
        }
    }
}
