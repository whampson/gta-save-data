using GTASaveData.Serialization;
using Xunit;

namespace GTASaveData.Core.Tests.Serialization
{
    public class TestFileFormat
    {
        [Theory]
        [InlineData(ConsoleType.PS2, ConsoleFlags.NorthAmerica | ConsoleFlags.Europe, true)]
        [InlineData(ConsoleType.PS2, ConsoleFlags.NorthAmerica, true)]
        [InlineData(ConsoleType.PS2, ConsoleFlags.Australia, false)]
        [InlineData(ConsoleType.PS2, ConsoleFlags.None, true)]
        [InlineData(ConsoleType.Win32, ConsoleFlags.Steam, true)]
        [InlineData(ConsoleType.Win32, ConsoleFlags.None, true)]
        public void TestSupportedWithFlags(ConsoleType type, ConsoleFlags flags, bool expectedResult)
        {
            FileFormat fmt = new FileFormat(
                "Test", "Test Format",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.NorthAmerica | ConsoleFlags.Europe),
                new GameConsole(ConsoleType.Win32, ConsoleFlags.Steam),
                new GameConsole(ConsoleType.Win32)
            );

            bool result = fmt.IsSupported(type, flags);
            Assert.Equal(expectedResult, result);
        }

        // TODO :test equal
    }
}
