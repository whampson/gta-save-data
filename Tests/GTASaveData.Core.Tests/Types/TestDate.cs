using Bogus;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestDate : TestBase<Date>
    {
        [Fact]
        public void RandomDataSerialization()
        {
            Faker f = new Faker();

            Date x0 = Generator.Date(f);
            Date x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Year, x1.Year);
            Assert.Equal(x0.Month, x1.Month);
            Assert.Equal(x0.Day, x1.Day);
            Assert.Equal(x0.Hour, x1.Hour);
            Assert.Equal(x0.Minute, x1.Minute);
            Assert.Equal(x0.Second, x1.Second);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
