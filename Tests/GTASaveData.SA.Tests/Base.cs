using GTASaveData.Types;
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

            //new object[] { SanAndreasSave.FileFormats.Mobile },
            new object[] { SanAndreasSave.FileFormats.PC },
            //new object[] { SanAndreasSave.FileFormats.PS2 },
            //new object[] { SanAndreasSave.FileFormats.Xbox },
        };
    }
}
