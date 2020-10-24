using System.Numerics;
using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestVector3 : TestBase<Vector3>
    {
        [Fact]
        public void RandomDataSerialization()
        {
            Faker f = new Faker();

            Vector3 x0 = Generator.Vector3(f);
            Vector3 x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.X, x1.X);
            Assert.Equal(x0.Y, x1.Y);
            Assert.Equal(x0.Z, x1.Z);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
