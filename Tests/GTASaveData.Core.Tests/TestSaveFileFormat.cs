using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestSaveFileFormat
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
            SaveFileFormat fmt = new SaveFileFormat(
                "Test", "Test", "Test Format",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.NorthAmerica | ConsoleFlags.Europe),
                new GameConsole(ConsoleType.Win32, ConsoleFlags.Steam),
                new GameConsole(ConsoleType.Win32)
            );

            bool result = fmt.IsSupportedOn(type, flags);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(ConsoleType.PS2, ConsoleFlags.None, ConsoleType.PS2, ConsoleFlags.None, true)]
        [InlineData(ConsoleType.PS2, ConsoleFlags.None, ConsoleType.Xbox, ConsoleFlags.None, false)]
        [InlineData(ConsoleType.PS2, ConsoleFlags.Europe | ConsoleFlags.NorthAmerica, ConsoleType.PS2, ConsoleFlags.Europe, false)]
        [InlineData(ConsoleType.PS2, ConsoleFlags.Europe | ConsoleFlags.NorthAmerica, ConsoleType.PS2, ConsoleFlags.NorthAmerica | ConsoleFlags.Europe, true)]
        public void TestEquals(ConsoleType typeA, ConsoleFlags flagsA, ConsoleType typeB, ConsoleFlags flagsB, bool expectedResult)
        {
            SaveFileFormat f1 = new SaveFileFormat("F1", "Format 1", null, new GameConsole(typeA, flagsA));
            SaveFileFormat f2 = new SaveFileFormat("F2", "Format 2", null, new GameConsole(typeB, flagsB));

            if (expectedResult)
            {
                Assert.Equal(f1, f2);
                Assert.True(f1 == f2);
            }
            else
            {
                Assert.NotEqual(f1, f2);
                Assert.True(f1 != f2);
            }
        }
    }
}
