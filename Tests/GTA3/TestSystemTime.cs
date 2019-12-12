using Bogus;
using GTASaveData.GTA3;
using System;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestSystemTime
    {
        public static SystemTime Generate()
        {
            return new SystemTime(
                new Faker().Date.Between(new DateTime(1970, 1, 1), DateTime.Now));
        }

        [Fact]
        public void Sanity()
        {
            SystemTime t0 = Generate();
            SystemTime t1 = TestHelper.CreateSerializedCopy(t0);

            Assert.Equal(t0, t1);
        }
    }
}
