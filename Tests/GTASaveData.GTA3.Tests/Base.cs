﻿using System.Collections.Generic;
using TestFramework;

namespace GTASaveData.GTA3.Tests
{
    public abstract class Base<T> : SaveDataObjectTestBase<T, GTA3SaveParams>
        where T : SaveDataObject, new()
    {
        public static IEnumerable<object[]> FileTypes => new[]
        {
            new object[] { GTA3Save.FileTypes.Android },
            new object[] { GTA3Save.FileTypes.iOS },
            new object[] { GTA3Save.FileTypes.PC },
            new object[] { GTA3Save.FileTypes.PS2 },
            new object[] { GTA3Save.FileTypes.PS2AU },
            new object[] { GTA3Save.FileTypes.PS2JP },
            new object[] { GTA3Save.FileTypes.Xbox },
            //new object[] { SaveFileGTA3.FileFormats.DefinitiveEdition },
        };

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { GTA3Save.FileTypes.Android, "AS2" },
            new object[] { GTA3Save.FileTypes.Android, "AS3" },
            new object[] { GTA3Save.FileTypes.Android, "CAT2" },
            new object[] { GTA3Save.FileTypes.Android, "LM1" },
            new object[] { GTA3Save.FileTypes.Android, "LM1 NonGXTName" },
            new object[] { GTA3Save.FileTypes.Android, "LM1 VehiclePool" },
            new object[] { GTA3Save.FileTypes.Android, "RC2 iOSConversion" },
            new object[] { GTA3Save.FileTypes.Android, "T4X4_1" },
            new object[] { GTA3Save.FileTypes.Android, "T4X4_3" },
            new object[] { GTA3Save.FileTypes.iOS, "CAT2" },
            new object[] { GTA3Save.FileTypes.iOS, "CAT2 GTASnP" },
            new object[] { GTA3Save.FileTypes.iOS, "JM2" },
            new object[] { GTA3Save.FileTypes.iOS, "LM1" },
            new object[] { GTA3Save.FileTypes.iOS, "LM3" },
            new object[] { GTA3Save.FileTypes.iOS, "LM3 VehiclePool" },
            new object[] { GTA3Save.FileTypes.iOS, "MEA4 VehiclePool" },
            new object[] { GTA3Save.FileTypes.PC, "AS3" },
            new object[] { GTA3Save.FileTypes.PC, "CAT2" },
            new object[] { GTA3Save.FileTypes.PC, "CAT2 SpecialVehicles1" },
            new object[] { GTA3Save.FileTypes.PC, "CAT2 SpecialVehicles2" },
            new object[] { GTA3Save.FileTypes.PC, "CAT2 SpecialVehicles3" },
            new object[] { GTA3Save.FileTypes.PC, "CAT2 VehiclePool" },
            new object[] { GTA3Save.FileTypes.PC, "JM4" },
            new object[] { GTA3Save.FileTypes.PC, "LM1" },
            new object[] { GTA3Save.FileTypes.PC, "RC1" },
            new object[] { GTA3Save.FileTypes.PC, "RC3" },
            new object[] { GTA3Save.FileTypes.PC, "T4X4_1" },
            new object[] { GTA3Save.FileTypes.PC, "T4X4_3" },
            new object[] { GTA3Save.FileTypes.PS2, "CAT2" },
            new object[] { GTA3Save.FileTypes.PS2, "LM1" },
            new object[] { GTA3Save.FileTypes.PS2, "LM1 EUv1" },
            new object[] { GTA3Save.FileTypes.PS2, "LM1 EUv2" },
            new object[] { GTA3Save.FileTypes.PS2, "LM1 US" },
            new object[] { GTA3Save.FileTypes.PS2, "LM1 VehiclePool" },
            new object[] { GTA3Save.FileTypes.PS2, "T4X4_1" },
            new object[] { GTA3Save.FileTypes.PS2AU, "AS3" },
            new object[] { GTA3Save.FileTypes.PS2AU, "CAT2" },
            new object[] { GTA3Save.FileTypes.PS2AU, "T4X4_2" },
            new object[] { GTA3Save.FileTypes.PS2JP, "LM1" },
            new object[] { GTA3Save.FileTypes.PS2JP, "LM2" },
            new object[] { GTA3Save.FileTypes.PS2JP, "LM2 Cheats" },
            new object[] { GTA3Save.FileTypes.Xbox, "CAT2" },
            new object[] { GTA3Save.FileTypes.Xbox, "DIAB2" },
            new object[] { GTA3Save.FileTypes.Xbox, "JM5" },
            new object[] { GTA3Save.FileTypes.Xbox, "LM1 1" },
            new object[] { GTA3Save.FileTypes.Xbox, "LM1 2" },
            new object[] { GTA3Save.FileTypes.Xbox, "LM1 99Packages" },
            new object[] { GTA3Save.FileTypes.Xbox, "LM1 ChainGame100" },
            new object[] { GTA3Save.FileTypes.Xbox, "LM1 VehiclePool" },
            new object[] { GTA3Save.FileTypes.Xbox, "LM5" },
            new object[] { GTA3Save.FileTypes.Xbox, "LOVE2" },
            new object[] { GTA3Save.FileTypes.PCDE, "CAT2" },
            new object[] { GTA3Save.FileTypes.PCDE, "HM_4" },
            new object[] { GTA3Save.FileTypes.PCDE, "LM1" },
            new object[] { GTA3Save.FileTypes.PCDE, "LOVE1" },
            new object[] { GTA3Save.FileTypes.PCDE, "LOVE7" },
            new object[] { GTA3Save.FileTypes.PCDE, "RC2" },
            new object[] { GTA3Save.FileTypes.PCDE, "YD2" },
            new object[] { GTA3Save.FileTypes.PCDE, "HM_5" },
            new object[] { GTA3Save.FileTypes.PCDE, "JM1" },
            new object[] { GTA3Save.FileTypes.PCDE, "RM4" },
            new object[] { GTA3Save.FileTypes.PCDE, "KM4" },
            new object[] { GTA3Save.FileTypes.PCDE, "AS1" },
        };
    }
}
