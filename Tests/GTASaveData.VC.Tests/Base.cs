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
    }
}
