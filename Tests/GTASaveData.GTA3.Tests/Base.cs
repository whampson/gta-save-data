using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.GTA3.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            new object[] { SaveFileGTA3.FileFormats.Android },
            new object[] { SaveFileGTA3.FileFormats.iOS },
            new object[] { SaveFileGTA3.FileFormats.PC },
            new object[] { SaveFileGTA3.FileFormats.PS2 },
            new object[] { SaveFileGTA3.FileFormats.PS2AU },
            new object[] { SaveFileGTA3.FileFormats.PS2JP },
            new object[] { SaveFileGTA3.FileFormats.Xbox },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { SaveFileGTA3.FileFormats.Android, "AS2" },
            new object[] { SaveFileGTA3.FileFormats.Android, "AS3" },
            new object[] { SaveFileGTA3.FileFormats.Android, "CAT2" },
            new object[] { SaveFileGTA3.FileFormats.Android, "LM1" },
            new object[] { SaveFileGTA3.FileFormats.Android, "LM1 NonGXTName" },
            new object[] { SaveFileGTA3.FileFormats.Android, "LM1 VehiclePool" },
            new object[] { SaveFileGTA3.FileFormats.Android, "RC2 iOSConversion" },
            new object[] { SaveFileGTA3.FileFormats.Android, "T4X4_1" },
            new object[] { SaveFileGTA3.FileFormats.Android, "T4X4_3" },
            new object[] { SaveFileGTA3.FileFormats.iOS, "CAT2" },
            new object[] { SaveFileGTA3.FileFormats.iOS, "CAT2 GTASnP" },
            new object[] { SaveFileGTA3.FileFormats.iOS, "JM2" },
            new object[] { SaveFileGTA3.FileFormats.iOS, "LM1" },
            new object[] { SaveFileGTA3.FileFormats.iOS, "LM3" },
            new object[] { SaveFileGTA3.FileFormats.iOS, "LM3 VehiclePool" },
            new object[] { SaveFileGTA3.FileFormats.iOS, "MEA4 VehiclePool" },
            new object[] { SaveFileGTA3.FileFormats.PC, "AS3" },
            new object[] { SaveFileGTA3.FileFormats.PC, "CAT2" },
            new object[] { SaveFileGTA3.FileFormats.PC, "CAT2 SpecialVehicles1" },
            new object[] { SaveFileGTA3.FileFormats.PC, "CAT2 SpecialVehicles2" },
            new object[] { SaveFileGTA3.FileFormats.PC, "CAT2 SpecialVehicles3" },
            new object[] { SaveFileGTA3.FileFormats.PC, "CAT2 VehiclePool" },
            new object[] { SaveFileGTA3.FileFormats.PC, "JM4" },
            new object[] { SaveFileGTA3.FileFormats.PC, "RC1" },
            new object[] { SaveFileGTA3.FileFormats.PC, "RC3" },
            new object[] { SaveFileGTA3.FileFormats.PC, "T4X4_1" },
            new object[] { SaveFileGTA3.FileFormats.PC, "T4X4_3" },
            new object[] { SaveFileGTA3.FileFormats.PS2, "CAT2" },
            new object[] { SaveFileGTA3.FileFormats.PS2, "LM1" },
            new object[] { SaveFileGTA3.FileFormats.PS2, "LM1 EUv1" },
            new object[] { SaveFileGTA3.FileFormats.PS2, "LM1 EUv2" },
            new object[] { SaveFileGTA3.FileFormats.PS2, "LM1 US" },
            new object[] { SaveFileGTA3.FileFormats.PS2, "LM1 VehiclePool" },
            new object[] { SaveFileGTA3.FileFormats.PS2, "T4X4_1" },
            new object[] { SaveFileGTA3.FileFormats.PS2AU, "AS3" },
            new object[] { SaveFileGTA3.FileFormats.PS2AU, "CAT2" },
            new object[] { SaveFileGTA3.FileFormats.PS2AU, "T4X4_2" },
            new object[] { SaveFileGTA3.FileFormats.PS2JP, "LM1" },
            new object[] { SaveFileGTA3.FileFormats.PS2JP, "LM2" },
            new object[] { SaveFileGTA3.FileFormats.PS2JP, "LM2 Cheats" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "CAT2" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "DIAB2" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "JM5" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "LM1 1" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "LM1 2" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "LM1 99Packages" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "LM1 ChainGame100" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "LM1 VehiclePool" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "LM5" },
            new object[] { SaveFileGTA3.FileFormats.Xbox, "LOVE2" },
        };
    }
}
