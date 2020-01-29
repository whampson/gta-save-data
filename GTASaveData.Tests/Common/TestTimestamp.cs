using Bogus;
using GTASaveData.Common;
using GTASaveData.Serialization;
using GTASaveData.Tests.TestFramework;
using System;
using Xunit;

namespace GTASaveData.Tests.Common
{
    public class TestTimestamp : SaveDataObjectTestBase<Timestamp>
    {
        public override Timestamp GenerateTestVector(FileFormat format)
        {
            return new Timestamp(
                new Faker().Date.Between(new DateTime(1970, 1, 1), DateTime.Now));
        }

        [Fact]
        public void Serialization()
        {
            Timestamp x0 = GenerateTestVector();
            Timestamp x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(24, data.Length);
        }
    }
}
