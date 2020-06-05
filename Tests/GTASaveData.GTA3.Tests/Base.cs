using GTASaveData.Types;
using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.GTA3.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            //new object[] { GTA3Save.FileFormats.Android },
            //new object[] { GTA3Save.FileFormats.iOS },
            new object[] { GTA3Save.FileFormats.PC },
            //new object[] { GTA3Save.FileFormats.PS2_AU },
            //new object[] { GTA3Save.FileFormats.PS2_JP },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU },
            new object[] { GTA3Save.FileFormats.Xbox },
        };
    }
}
