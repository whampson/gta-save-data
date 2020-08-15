using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.LCS.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            new object[] { LCSSave.FileFormats.Android },
            new object[] { LCSSave.FileFormats.iOS },
            new object[] { LCSSave.FileFormats.PS2 },
            new object[] { LCSSave.FileFormats.PSP },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { LCSSave.FileFormats.Android, "MAC3" },
            new object[] { LCSSave.FileFormats.Android, "SALH5 100Percent" },
            new object[] { LCSSave.FileFormats.Android, "VIC1" },
            new object[] { LCSSave.FileFormats.Android, "VIC1 2" },
            new object[] { LCSSave.FileFormats.iOS, "MAR4" },
            new object[] { LCSSave.FileFormats.iOS, "SALH5 100Percent" },
            new object[] { LCSSave.FileFormats.iOS, "VIC1" },
            new object[] { LCSSave.FileFormats.iOS, "VIC1 StarterSave" },
            new object[] { LCSSave.FileFormats.PS2, "NEDS4" },
            new object[] { LCSSave.FileFormats.PS2, "SALH5 100Percent" },
            new object[] { LCSSave.FileFormats.PS2, "SALH5 SpecialVehicles" },
            new object[] { LCSSave.FileFormats.PS2, "VIC1" },
            new object[] { LCSSave.FileFormats.PSP, "MAR3" },
            new object[] { LCSSave.FileFormats.PSP, "VIC1" },
            new object[] { LCSSave.FileFormats.PSP, "SALH5" },
        };
    }
}
