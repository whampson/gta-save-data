using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestSystemTime : TestBase<SystemTime>
    {
        [Fact]
        public void RandomDataSerialization()
        {
            Faker f = new Faker();

            SystemTime x0 = new SystemTime(Generator.Date(f));
            SystemTime x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.Year, x1.Year);
            Assert.Equal(x0.Month, x1.Month);
            Assert.Equal(x0.DayOfWeek, x1.DayOfWeek);
            Assert.Equal(x0.Day, x1.Day);
            Assert.Equal(x0.Hour, x1.Hour);
            Assert.Equal(x0.Minute, x1.Minute);
            Assert.Equal(x0.Second, x1.Second);
            Assert.Equal(x0.Milliseconds, x1.Milliseconds);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0), data.Length);
        }
    }
}
