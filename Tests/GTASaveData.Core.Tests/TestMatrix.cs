using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestMatrix : TestBase<Matrix, SerializationParams>
    {
        [Fact]
        public void RandomDataSerialization()
        {
            Faker f = new Faker();

            Matrix x0 = Generator.Matrix(f);
            Matrix x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Right, x1.Right);
            Assert.Equal(x0.Forward, x1.Forward);
            Assert.Equal(x0.Up, x1.Up);
            Assert.Equal(x0.Position, x1.Position);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }

        public override int GetSizeOfTestObject(Matrix obj)
        {
            return 64;
        }
    }
}
