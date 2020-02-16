using Bogus;
using GTASaveData.Common;
using GTASaveData.Serialization;
using System;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.Common
{
    public class TestTimestamp : SerializableObjectTestBase<Timestamp>
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

            Assert.Equal(x0.Year, x1.Year);
            Assert.Equal(x0.Month, x1.Month);
            Assert.Equal(x0.Day, x1.Day);
            Assert.Equal(x0.Hour, x1.Hour);
            Assert.Equal(x0.Minute, x1.Minute);
            Assert.Equal(x0.Second, x1.Second);
            Assert.Equal(x0, x1);
            Assert.Equal(24, data.Length);
        }
    }
}
