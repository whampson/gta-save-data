using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.SA.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            // TODO: 360 and PS3?

            //new object[] { SASave.FileFormats.Mobile },
            new object[] { SASave.FileFormats.PC },
            //new object[] { SASave.FileFormats.PS2 },
            //new object[] { SASave.FileFormats.Xbox },
        };

        public static IEnumerable<object[]> TestFiles => new[]
{
            new object[] { SASave.FileFormats.PC, "BCES4_2" },
            new object[] { SASave.FileFormats.PC, "CASINO3" },
            new object[] { SASave.FileFormats.PC, "CASINO6" },
            new object[] { SASave.FileFormats.PC, "GROVE_1" },
            new object[] { SASave.FileFormats.PC, "RIOT_4" },
            new object[] { SASave.FileFormats.PC, "STAD_01" },
            new object[] { SASave.FileFormats.PC, "STAD_03" },
            new object[] { SASave.FileFormats.PC, "STRAP_4" },
        };
    }
}
