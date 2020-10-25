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
            new object[] { SaveFileSA.FileFormats.PC },
            //new object[] { SASave.FileFormats.PS2 },
            //new object[] { SASave.FileFormats.Xbox },
        };

        public static IEnumerable<object[]> TestFiles => new[]
{
            new object[] { SaveFileSA.FileFormats.PC, "BCES4_2" },
            new object[] { SaveFileSA.FileFormats.PC, "CASINO3" },
            new object[] { SaveFileSA.FileFormats.PC, "CASINO6" },
            new object[] { SaveFileSA.FileFormats.PC, "GROVE_1" },
            new object[] { SaveFileSA.FileFormats.PC, "RIOT_4" },
            new object[] { SaveFileSA.FileFormats.PC, "STAD_01" },
            new object[] { SaveFileSA.FileFormats.PC, "STAD_03" },
            new object[] { SaveFileSA.FileFormats.PC, "STRAP_4" },
        };
    }
}
