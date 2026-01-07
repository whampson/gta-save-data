using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.LCS.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T, LCSSaveParams>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            new object[] { LCSSaveParams.GetDefaults(LCSSave.FileTypes.Android) },
            new object[] { LCSSaveParams.GetDefaults(LCSSave.FileTypes.iOS) },
            new object[] { LCSSaveParams.GetDefaults(LCSSave.FileTypes.PS2) },
            new object[] { LCSSaveParams.GetDefaults(LCSSave.FileTypes.PSP) },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { LCSSave.FileTypes.Android, "MAC3" },
            new object[] { LCSSave.FileTypes.Android, "SALH5 100Percent" },
            new object[] { LCSSave.FileTypes.Android, "VIC1" },
            new object[] { LCSSave.FileTypes.Android, "VIC1 2" },
            new object[] { LCSSave.FileTypes.iOS, "MAR4" },
            new object[] { LCSSave.FileTypes.iOS, "SALH5 100Percent" },
            new object[] { LCSSave.FileTypes.iOS, "VIC1" },
            new object[] { LCSSave.FileTypes.iOS, "VIC1 StarterSave" },
            new object[] { LCSSave.FileTypes.PS2, "NEDS4" },
            new object[] { LCSSave.FileTypes.PS2, "SALH5 100Percent" },
            new object[] { LCSSave.FileTypes.PS2, "SALH5 SpecialVehicles" },
            new object[] { LCSSave.FileTypes.PS2, "VIC1" },
            new object[] { LCSSave.FileTypes.PSP, "MAR3" },
            new object[] { LCSSave.FileTypes.PSP, "VIC1" },
            new object[] { LCSSave.FileTypes.PSP, "SALH5" },
        };
    }
}
