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
        public void Serialization()
        {
            SystemTime x0 = GenerateTestVector();
            SystemTime x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(16, data.Length);
        }
    }
}
