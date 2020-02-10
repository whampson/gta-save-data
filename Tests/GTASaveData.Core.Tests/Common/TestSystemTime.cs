using Bogus;
using GTASaveData.Common;
using GTASaveData.Serialization;
using System;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.Common
{
    public class TestSystemTime : ChunkTestBase<SystemTime>
    {
        public override SystemTime GenerateTestVector(FileFormat format)
        {
            return new SystemTime(
                new Faker().Date.Between(new DateTime(1970, 1, 1), DateTime.Now));
        }

        [Fact]
        public void Serialization()
        {
            SystemTime x0 = GenerateTestVector();
            SystemTime x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0, x1);
            Assert.Equal(16, data.Length);
        }
    }
}
