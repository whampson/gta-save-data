using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.LCS.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            new object[] { SaveFileLCS.FileFormats.Android },
            new object[] { SaveFileLCS.FileFormats.iOS },
            new object[] { SaveFileLCS.FileFormats.PS2 },
            new object[] { SaveFileLCS.FileFormats.PSP },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { SaveFileLCS.FileFormats.Android, "MAC3" },
            new object[] { SaveFileLCS.FileFormats.Android, "SALH5 100Percent" },
            new object[] { SaveFileLCS.FileFormats.Android, "VIC1" },
            new object[] { SaveFileLCS.FileFormats.Android, "VIC1 2" },
            new object[] { SaveFileLCS.FileFormats.iOS, "MAR4" },
            new object[] { SaveFileLCS.FileFormats.iOS, "SALH5 100Percent" },
            new object[] { SaveFileLCS.FileFormats.iOS, "VIC1" },
            new object[] { SaveFileLCS.FileFormats.iOS, "VIC1 StarterSave" },
            new object[] { SaveFileLCS.FileFormats.PS2, "NEDS4" },
            new object[] { SaveFileLCS.FileFormats.PS2, "SALH5 100Percent" },
            new object[] { SaveFileLCS.FileFormats.PS2, "SALH5 SpecialVehicles" },
            new object[] { SaveFileLCS.FileFormats.PS2, "VIC1" },
            new object[] { SaveFileLCS.FileFormats.PSP, "MAR3" },
            new object[] { SaveFileLCS.FileFormats.PSP, "VIC1" },
            new object[] { SaveFileLCS.FileFormats.PSP, "SALH5" },
        };
    }
}
