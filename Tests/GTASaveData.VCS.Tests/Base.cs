using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.VCS.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            new object[] { SaveFileVCS.FileFormats.PS2 },
            new object[] { SaveFileVCS.FileFormats.PSP },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
             new object[] { SaveFileVCS.FileFormats.PS2, "PHI_A02" },
             new object[] { SaveFileVCS.FileFormats.PS2, "DIA_C05" },
             new object[] { SaveFileVCS.FileFormats.PS2, "LAN_C04" },
             new object[] { SaveFileVCS.FileFormats.PSP, "DIA_C04" },
             new object[] { SaveFileVCS.FileFormats.PSP, "DIA_C05" },
             new object[] { SaveFileVCS.FileFormats.PSP, "DIA_C05 2" },
             new object[] { SaveFileVCS.FileFormats.PSP, "DIA_C05 3" },
             new object[] { SaveFileVCS.FileFormats.PSP, "JER_A01" },
             new object[] { SaveFileVCS.FileFormats.PSP, "JER_A03" },
             new object[] { SaveFileVCS.FileFormats.PSP, "LAN_B06" },
        };
    }
}
