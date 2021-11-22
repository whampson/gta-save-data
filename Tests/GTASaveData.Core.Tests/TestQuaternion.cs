using System.Numerics;
using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestQuaternion : TestBase<Quaternion>
    {
        [Fact]
        public void RandomDataSerialization()
        {
            Faker f = new Faker();

            Quaternion x0 = Generator.Quaternion(f);
            Quaternion x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.X, x1.X);
            Assert.Equal(x0.Y, x1.Y);
            Assert.Equal(x0.Z, x1.Z);
            Assert.Equal(x0.W, x1.W);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }

        public override int GetSizeOfTestObject(Quaternion obj)
        {
            return 16;
        }
    }
}
