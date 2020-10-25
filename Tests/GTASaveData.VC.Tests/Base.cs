using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.VC.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            //new object[] { ViceCitySave.FileFormats.Android },
            //new object[] { ViceCitySave.FileFormats.iOS },
            new object[] { VCSave.FileFormats.PC },
            new object[] { VCSave.FileFormats.PC_Steam },
            //new object[] { ViceCitySave.FileFormats.PS2 },
            //new object[] { ViceCitySave.FileFormats.Xbox },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
            //new object[] { ViceCitySave.FileFormats.Android, "CAP_1" },
            //new object[] { ViceCitySave.FileFormats.Android, "COK_4" },
            //new object[] { ViceCitySave.FileFormats.Android, "CUB_1" },
            //new object[] { ViceCitySave.FileFormats.Android, "FIN_1" },
            //new object[] { ViceCitySave.FileFormats.Android, "LAW_3" },
            //new object[] { ViceCitySave.FileFormats.iOS, "FIN_1" },
            //new object[] { ViceCitySave.FileFormats.iOS, "FIN_1 SpecialVehicles1" },
            //new object[] { ViceCitySave.FileFormats.iOS, "FIN_1 SpecialVehicles1" },
            new object[] { VCSave.FileFormats.PC, "COK_2" },
            new object[] { VCSave.FileFormats.PC, "CREAM" },
            new object[] { VCSave.FileFormats.PC, "FIN_1" },
            new object[] { VCSave.FileFormats.PC, "ITBEG" },
            new object[] { VCSave.FileFormats.PC, "ITBEG Japan" },
            new object[] { VCSave.FileFormats.PC, "ITBEG StarterSave" },
            new object[] { VCSave.FileFormats.PC, "JOB_5" },
            new object[] { VCSave.FileFormats.PC, "TEX_3" },
            new object[] { VCSave.FileFormats.PC_Steam, "BUD_3" },
            new object[] { VCSave.FileFormats.PC_Steam, "COK_3" },
            new object[] { VCSave.FileFormats.PC_Steam, "FIN_1" },
            //new object[] { ViceCitySave.FileFormats.PS2, "CREAM" },
            //new object[] { ViceCitySave.FileFormats.PS2, "FIN_1 1" },
            //new object[] { ViceCitySave.FileFormats.PS2, "FIN_1 2" },
            //new object[] { ViceCitySave.FileFormats.PS2, "FIN_1 3" },
            //new object[] { ViceCitySave.FileFormats.PS2, "TEX_1" },
            //new object[] { ViceCitySave.FileFormats.PS2, "TEX_2" },
            //new object[] { ViceCitySave.FileFormats.Xbox, "CAP_1" },
            //new object[] { ViceCitySave.FileFormats.Xbox, "FIN_1" },
            //new object[] { ViceCitySave.FileFormats.Xbox, "LAW_2" },
        };
    }
}
