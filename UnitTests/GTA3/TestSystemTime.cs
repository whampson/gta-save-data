using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using System;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestSystemTime
        : SaveDataObjectTestBase<SystemTime>
    {
        public override SystemTime GenerateTestVector(SystemType system)
        {
            return new SystemTime(
                new Faker().Date.Between(new DateTime(1970, 1, 1), DateTime.Now));
        }

        [Fact]
        public void Sanity()
        {
            SystemTime t0 = GenerateTestVector();
            SystemTime t1 = TestHelper.CreateSerializedCopy(t0);

            Assert.Equal(t0, t1);
        }
    }
}
