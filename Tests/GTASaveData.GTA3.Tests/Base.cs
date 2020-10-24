using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.GTA3.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileFormats => new[]
        {
            new object[] { GTA3Save.FileFormats.Android },
            new object[] { GTA3Save.FileFormats.iOS },
            new object[] { GTA3Save.FileFormats.PC },
            new object[] { GTA3Save.FileFormats.PS2 },
            new object[] { GTA3Save.FileFormats.PS2_AU },
            new object[] { GTA3Save.FileFormats.PS2_JP },
            new object[] { GTA3Save.FileFormats.Xbox },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { GTA3Save.FileFormats.Android, "AS2" },
            new object[] { GTA3Save.FileFormats.Android, "AS3" },
            new object[] { GTA3Save.FileFormats.Android, "CAT2" },
            new object[] { GTA3Save.FileFormats.Android, "LM1" },
            new object[] { GTA3Save.FileFormats.Android, "LM1 NonGXTName" },
            new object[] { GTA3Save.FileFormats.Android, "LM1 VehiclePool" },
            new object[] { GTA3Save.FileFormats.Android, "RC2 iOSConversion" },
            new object[] { GTA3Save.FileFormats.Android, "T4X4_1" },
            new object[] { GTA3Save.FileFormats.Android, "T4X4_3" },
            new object[] { GTA3Save.FileFormats.iOS, "CAT2" },
            new object[] { GTA3Save.FileFormats.iOS, "CAT2 GTASnP" },
            new object[] { GTA3Save.FileFormats.iOS, "JM2" },
            new object[] { GTA3Save.FileFormats.iOS, "LM1" },
            new object[] { GTA3Save.FileFormats.iOS, "LM3" },
            new object[] { GTA3Save.FileFormats.iOS, "LM3 VehiclePool" },
            new object[] { GTA3Save.FileFormats.iOS, "MEA4 VehiclePool" },
            //new object[] { GTA3Save.FileFormats.PC, "AS3" },
            new object[] { GTA3Save.FileFormats.PC, "CAT2" },
            new object[] { GTA3Save.FileFormats.PC, "CAT2 SpecialVehicles1" },
            new object[] { GTA3Save.FileFormats.PC, "CAT2 SpecialVehicles2" },
            new object[] { GTA3Save.FileFormats.PC, "CAT2 SpecialVehicles3" },
            new object[] { GTA3Save.FileFormats.PC, "CAT2 VehiclePool" },
            new object[] { GTA3Save.FileFormats.PC, "JM4" },
            new object[] { GTA3Save.FileFormats.PC, "RC1" },
            new object[] { GTA3Save.FileFormats.PC, "RC3" },
            new object[] { GTA3Save.FileFormats.PC, "T4X4_1" },
            new object[] { GTA3Save.FileFormats.PC, "T4X4_3" },
            new object[] { GTA3Save.FileFormats.PS2, "CAT2" },
            new object[] { GTA3Save.FileFormats.PS2, "LM1" },
            new object[] { GTA3Save.FileFormats.PS2, "LM1 EUv1" },
            new object[] { GTA3Save.FileFormats.PS2, "LM1 EUv2" },
            new object[] { GTA3Save.FileFormats.PS2, "LM1 US" },
            new object[] { GTA3Save.FileFormats.PS2, "LM1 VehiclePool" },
            new object[] { GTA3Save.FileFormats.PS2, "T4X4_1" },
            new object[] { GTA3Save.FileFormats.PS2_AU, "AS3" },
            new object[] { GTA3Save.FileFormats.PS2_AU, "CAT2" },
            new object[] { GTA3Save.FileFormats.PS2_AU, "T4X4_2" },
            new object[] { GTA3Save.FileFormats.PS2_JP, "LM1" },
            new object[] { GTA3Save.FileFormats.PS2_JP, "LM2" },
            new object[] { GTA3Save.FileFormats.PS2_JP, "LM2 Cheats" },
            new object[] { GTA3Save.FileFormats.Xbox, "CAT2" },
            new object[] { GTA3Save.FileFormats.Xbox, "DIAB2" },
            new object[] { GTA3Save.FileFormats.Xbox, "JM5" },
            new object[] { GTA3Save.FileFormats.Xbox, "LM1 1" },
            new object[] { GTA3Save.FileFormats.Xbox, "LM1 2" },
            new object[] { GTA3Save.FileFormats.Xbox, "LM1 99Packages" },
            new object[] { GTA3Save.FileFormats.Xbox, "LM1 ChainGame100" },
            new object[] { GTA3Save.FileFormats.Xbox, "LM1 VehiclePool" },
            new object[] { GTA3Save.FileFormats.Xbox, "LM5" },
            new object[] { GTA3Save.FileFormats.Xbox, "LOVE2" },
        };
    }
}
