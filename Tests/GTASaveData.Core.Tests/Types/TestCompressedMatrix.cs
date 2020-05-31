using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.Types
{
    public class TestCompressedMatrix : TestBase<CompressedMatrix>
    {
        [Fact]
        public void RandomDataSerialization()
        {
            Faker f = new Faker();

            CompressedMatrix x0 = new CompressedMatrix()
            {
                Position = Generator.Vector3D(f),
                RightX = f.Random.Byte(),
                RightY = f.Random.Byte(),
                RightZ = f.Random.Byte(),
                ForwardX = f.Random.Byte(),
                ForwardY = f.Random.Byte(),
                ForwardZ = f.Random.Byte()
            };
            CompressedMatrix x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0.RightX, x1.RightX);
            Assert.Equal(x0.RightY, x1.RightY);
            Assert.Equal(x0.RightZ, x1.RightZ);
            Assert.Equal(x0.ForwardX, x1.ForwardX);
            Assert.Equal(x0.ForwardY, x1.ForwardY);
            Assert.Equal(x0.ForwardZ, x1.ForwardZ);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
