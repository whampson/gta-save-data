using System.Numerics;
using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestVector2 : TestBase<Vector2, SerializationParams>
    {
        [Fact]
        public void RandomDataSerialization()
        {
            Faker f = new Faker();

            Vector2 x0 = Generator.Vector2(f);
            Vector2 x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.X, x1.X);
            Assert.Equal(x0.Y, x1.Y);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }

        public override int GetSizeOfTestObject(Vector2 obj)
        {
            return 8;
        }
    }
}
