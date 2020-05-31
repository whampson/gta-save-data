using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests.Types
{
    public class TestVector2D : TestBase<Vector2D>
    {
        [Fact]
        public void RandomDataSerialization()
        {
            Faker f = new Faker();

            Vector2D x0 = Generator.Vector2D(f);
            Vector2D x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.X, x1.X);
            Assert.Equal(x0.Y, x1.Y);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
