using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.VC.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            new object[] { SaveFileVC.FileFormats.Android },
            new object[] { SaveFileVC.FileFormats.iOS },
            new object[] { SaveFileVC.FileFormats.PC },
            new object[] { SaveFileVC.FileFormats.PC_Steam },
            //new object[] { SaveFileVC.FileFormats.PS2 },
            //new object[] { SaveFileVC.FileFormats.Xbox },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { SaveFileVC.FileFormats.Android, "CAP_1" },
            new object[] { SaveFileVC.FileFormats.Android, "COK_4" },
            new object[] { SaveFileVC.FileFormats.Android, "CUB_1" },
            new object[] { SaveFileVC.FileFormats.Android, "FIN_1 0" },
            new object[] { SaveFileVC.FileFormats.Android, "FIN_1 1" },
            new object[] { SaveFileVC.FileFormats.Android, "LAW_3" },
            new object[] { SaveFileVC.FileFormats.iOS, "FIN_1 0" },
            new object[] { SaveFileVC.FileFormats.iOS, "FIN_1 1" },
            new object[] { SaveFileVC.FileFormats.iOS, "FIN_1 2" },
            new object[] { SaveFileVC.FileFormats.iOS, "FIN_1 3" },
            new object[] { SaveFileVC.FileFormats.PC, "COK_2" },
            new object[] { SaveFileVC.FileFormats.PC, "CREAM" },
            new object[] { SaveFileVC.FileFormats.PC, "FIN_1" },
            new object[] { SaveFileVC.FileFormats.PC, "ITBEG" },
            new object[] { SaveFileVC.FileFormats.PC, "ITBEG Japan" },
            new object[] { SaveFileVC.FileFormats.PC, "ITBEG StarterSave" },
            new object[] { SaveFileVC.FileFormats.PC, "JOB_5" },
            new object[] { SaveFileVC.FileFormats.PC, "TEX_3" },
            new object[] { SaveFileVC.FileFormats.PC_Steam, "BUD_3" },
            new object[] { SaveFileVC.FileFormats.PC_Steam, "COK_3" },
            new object[] { SaveFileVC.FileFormats.PC_Steam, "FIN_1" },
            //new object[] { SaveFileVC.FileFormats.PS2, "CREAM" },
            //new object[] { SaveFileVC.FileFormats.PS2, "FIN_1 1" },
            //new object[] { SaveFileVC.FileFormats.PS2, "FIN_1 2" },
            //new object[] { SaveFileVC.FileFormats.PS2, "FIN_1 3" },
            //new object[] { SaveFileVC.FileFormats.PS2, "TEX_1" },
            //new object[] { SaveFileVC.FileFormats.PS2, "TEX_2" },
            //new object[] { SaveFileVC.FileFormats.Xbox, "CAP_1" },
            //new object[] { SaveFileVC.FileFormats.Xbox, "FIN_1" },
            //new object[] { SaveFileVC.FileFormats.Xbox, "LAW_2" },
        };
    }
}
