using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.Types
{
    public class TestVector3D : TestBase<Vector3D>
    {
        [Fact]
        public void RandomDataSerialization()
        {
            Faker f = new Faker();

            Vector3D x0 = Generator.Vector3D(f);
            Vector3D x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.X, x1.X);
            Assert.Equal(x0.Y, x1.Y);
            Assert.Equal(x0.Z, x1.Z);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
