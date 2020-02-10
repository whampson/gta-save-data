using GTASaveData.Extensions;
using Xunit;

namespace GTASaveData.Core.Tests.Extensions
{
    public class TestByteArrayExtensions
    {
        [Theory]
        [InlineData(new byte[] { 1 }, new byte[] { 0 }, -1)]
        [InlineData(new byte[] { 0 }, new byte[] { 0 }, 0)]
        [InlineData(new byte[] { 1, 0, 1 }, new byte[] { 1 }, 0)]
        [InlineData(new byte[] { 1, 0, 1 }, new byte[] { 0 }, 1)]
        [InlineData(new byte[] { 1, 3, 3, 1 }, new byte[] { 1, 3, }, 0)]
        [InlineData(new byte[] { 1, 3, 3, 1 }, new byte[] { 3, 3, }, 1)]
        [InlineData(new byte[] { 1, 3, 3, 1 }, new byte[] { 3, 1, }, 2)]
        public void FindFirst(byte[] buf, byte[] seq, int expectedIndex)
        {
            int index = buf.FindFirst(seq);

            Assert.Equal(expectedIndex, index);
        }
    }
}
