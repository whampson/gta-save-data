using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TestFramework;
using Xunit;

namespace GTASaveData.LCS.Tests
{
    public class TestVolatileMask : Base<LCSSave>
    {
        public override LCSSave GenerateTestObject(LCSSaveParams p) => new LCSSave();

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void VolatileMaskIsStable(FileType ft, ScriptVersion platform, string name)
        {
            string path = TestData.GetTestDataPath(Game.LCS, platform.ToString(), name);
            LCSSaveParams p = LCSSaveParams.GetDefaults(ft);
            LCSSave x0 = LCSSave.LoadFromFile(path, ft);

            Serializer.Write(x0, p, out byte[] data0, out bool[] volatileMask0);
            string signature0 = Signature(volatileMask0);

            // Re-serializing the same object must produce an identical mask
            // (volatility is a structural property, not data-dependent noise).
            Serializer.Write(x0, p, out byte[] _, out bool[] volatileMask1);
            Assert.Equal(signature0, Signature(volatileMask1));

            // The mask must also be stable across a save -> load -> save cycle.
            string temp = WriteTempFile(data0);
            try
            {
                LCSSave x1 = LCSSave.LoadFromFile(temp, ft);
                Serializer.Write(x1, p, out byte[] _, out bool[] volatileMask2);
                Assert.Equal(signature0, Signature(volatileMask2));
            }
            finally
            {
                File.Delete(temp);
            }
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void VolatileMaskMatchesSnapshot(FileType ft, ScriptVersion platform, string name)
        {
            string path = TestData.GetTestDataPath(Game.LCS, platform.ToString(), name);
            LCSSaveParams p = LCSSaveParams.GetDefaults(ft);
            LCSSave x0 = LCSSave.LoadFromFile(path, ft);

            Serializer.Write(x0, p, out byte[] _, out bool[] volatileMask);
            string hash = Sha256(Signature(volatileMask));

            Assert.True(ExpectedVolatileMasks.TryGetValue((ft, name), out string expected),
                $"Missing volatile-mask snapshot for '{name}' ({ft}). Add its hash to {nameof(ExpectedVolatileMasks)}.");
            Assert.Equal(expected, hash);
        }

        private static string WriteTempFile(byte[] data)
        {
            string path = Path.Combine(Path.GetTempPath(), $"lcs_save_{Guid.NewGuid():N}.bin");
            File.WriteAllBytes(path, data);
            return path;
        }

        private static string Signature(bool[] volatileMask)
        {
            List<string> ranges = new List<string>();
            int start = -1;
            for (int i = 0; i < volatileMask.Length; i++)
            {
                if (volatileMask[i] && start < 0) start = i;
                if (!volatileMask[i] && start >= 0) { ranges.Add($"{start:X}-{i - 1:X}"); start = -1; }
            }
            if (start >= 0) ranges.Add($"{start:X}-{volatileMask.Length - 1:X}");

            return string.Join(",", ranges);
        }

        private static string Sha256(string value)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
                return string.Concat(hash.Select(b => b.ToString("x2")));
            }
        }

        // How to generate an entry for a new save file:
        //   1. Add the save to TestData and register it in Base.cs.
        //   2. The test computes `hash = Sha256(Signature(volatileMask))`, where
        //      `Signature` is the comma-separated list of `start-end` hex ranges
        //      for every byte marked volatile. Run the test once and read the hash
        //      from the failure output (temporarily log `hash` if needed).
        //   3. Paste it here keyed by (FileType, filename).
        private static readonly Dictionary<(FileType, string), string> ExpectedVolatileMasks = new Dictionary<(FileType, string), string>
        {
            [(LCSSave.FileTypes.Mobile32, "MAC3")] = "2ac6f61d5a91671ad6fe5bf29d9d6a334557d7790544ed19d606692837297eed",
            [(LCSSave.FileTypes.Mobile32, "SALH5 100Percent")] = "e944b9b4c1ec6d755ce37a6953df42bcf1265391e22ea5ed0112f8b203b0aec6",
            [(LCSSave.FileTypes.Mobile32, "VIC1")] = "5d8bc9d2e07a3448635bd39f94ed3d57f554746e367fca3ed975703a67b8273f",
            [(LCSSave.FileTypes.Mobile32, "VIC1 2")] = "83519aa24085907714587b49417469f7fd6878d29c3d6d47e5e503b26a488c12",
            [(LCSSave.FileTypes.Mobile32, "SALH5 PackingHeat 1")] = "573065e0e6169225f8c19cc81871df3a6eb67bafe87aafb8d3242f93d093a5b0",
            [(LCSSave.FileTypes.Mobile32, "SALH5 PackingHeat 2")] = "573065e0e6169225f8c19cc81871df3a6eb67bafe87aafb8d3242f93d093a5b0",
            [(LCSSave.FileTypes.Mobile32, "SALH5 FromLaptop")] = "e944b9b4c1ec6d755ce37a6953df42bcf1265391e22ea5ed0112f8b203b0aec6",
            [(LCSSave.FileTypes.Mobile32, "SALH5 AamBaub 1")] = "32a00ad4ddb60d65c5dab7e497d09af95fb4b2b68fc060b174580836b0fab123",
            [(LCSSave.FileTypes.Mobile32, "SALH5 100Percent CP")] = "32a00ad4ddb60d65c5dab7e497d09af95fb4b2b68fc060b174580836b0fab123",
            [(LCSSave.FileTypes.Mobile32, "SALH5 AamBaub 9")] = "bff4573cd738724438f0ebc5e5d6fe74ea56a1a2cf8b93a566bf33c6abc29028",
            [(LCSSave.FileTypes.Mobile32, "MAC3 CP SetSave")] = "2ac6f61d5a91671ad6fe5bf29d9d6a334557d7790544ed19d606692837297eed",
            [(LCSSave.FileTypes.Mobile32, "VIC1 CartmanKusanagi")] = "83519aa24085907714587b49417469f7fd6878d29c3d6d47e5e503b26a488c12",
            [(LCSSave.FileTypes.Mobile64, "MAR4")] = "723da56ef0e0de3321aefad49b622659876ee49b26b743a2a90ad4574210dcc7",
            [(LCSSave.FileTypes.Mobile64, "SALH5 100Percent")] = "38bbd49809e7a965069961408690ef1902ba07fa7be4cc6f5eb69ccfe2ef1cfb",
            [(LCSSave.FileTypes.Mobile64, "VIC1")] = "f63faea40fee7e5fbce8bda0755a1c74a1825a0f85548ea904b4c97084c734ff",
            [(LCSSave.FileTypes.Mobile64, "VIC1 StarterSave")] = "29dcefb38db6fd908c3b0f48d9c8968b9b1492c81be34e891a4cb8fc1ee63e67",
            [(LCSSave.FileTypes.Mobile64, "VIC1 HomeSweetHome Vehicles")] = "29dcefb38db6fd908c3b0f48d9c8968b9b1492c81be34e891a4cb8fc1ee63e67",
            [(LCSSave.FileTypes.Mobile64, "SALH5 100Percent 2")] = "ed8d38a1a9b665c994c20668788069e59a044629f4062d9c74b26404aa533685",
            [(LCSSave.FileTypes.PS2, "NEDS4")] = "a4002bdcbfb2cb01aa9c399826650c2826cdc1a73b08e4ba0cbd43332cc68dc4",
            [(LCSSave.FileTypes.PS2, "SALH5 100Percent")] = "3b56d990c478c0706866da4a343926797da497eca1b1b828fa67d2c596a3b17b",
            [(LCSSave.FileTypes.PS2, "SALH5 SpecialVehicles")] = "7f9f2ce9c34ca0e46ba62fa3eb0c415f1d5a300b095dd9b85490bfcef20bc426",
            [(LCSSave.FileTypes.PS2, "VIC1")] = "ff9b1a2895b1a6fbc765ca91cd447f5eeeca442f4589d0140de22a4ee8375b29",
            [(LCSSave.FileTypes.PS2, "VIC1 HomeSweetHome")] = "229a2ff1952e9310c94c8695b037216b31bb13e80eaced7be1877cc21f806d82",
            [(LCSSave.FileTypes.PS2, "VIC1 Militia 1")] = "db528e83a915df7290423f382c8dab1cb589a176a41cc2e9088b517a872ee814",
            [(LCSSave.FileTypes.PS2, "SALH5 Militia 2")] = "7f9f2ce9c34ca0e46ba62fa3eb0c415f1d5a300b095dd9b85490bfcef20bc426",
            [(LCSSave.FileTypes.PS2, "RAYS5 MunitionsDump")] = "9b2bade1ae7e49b1333a50257f9fb96b97b8f6234772685aa40392d19b2052f8",
            [(LCSSave.FileTypes.PS2, "SAL7 TroubleWithTriads")] = "80d0ab7d754199ad2c4d208c3ea740bd71df8e240d31f43e7d5ac65a368b1d24",
            [(LCSSave.FileTypes.PS2, "SAL8 DrivingMrLeone")] = "487241ca51a0c2f5bdd8cb2d16c3bbe6efdaca775d682fa1972387a9067561d6",
            [(LCSSave.FileTypes.PS2, "SAL8 DrivingMrLeone 2")] = "487241ca51a0c2f5bdd8cb2d16c3bbe6efdaca775d682fa1972387a9067561d6",
            [(LCSSave.FileTypes.PS2, "SALH4 ShoresideRedemption")] = "61359e49dc825520e8c03cc22f9797e4fb26123988fb2de768ab8fca338e6e35",
            [(LCSSave.FileTypes.PS2, "SALH5 ARMax 1")] = "0409cfe9fe6e6520760847d389ff9d647ac2194236ecb8d128b8db88284d2665",
            [(LCSSave.FileTypes.PS2, "SALH5 ARMax 2")] = "b65cd0c985ab4e200234e5d9a7f399ca676d6edd30382bfd9ff1e7ee4b2342f7",
            [(LCSSave.FileTypes.PS2, "SALH5 FBICar")] = "e71c9b936e5f3b1acafe9af2b3cf52c6299ebf8d7c8ac9a2a785221b2ec9e5cb",
            [(LCSSave.FileTypes.PS2, "SALH5 MaxAmmo")] = "e26a1e968a653ed76fde7479a8d7949a4351b240f5834b7419f00713ba5c43ad",
            [(LCSSave.FileTypes.PS2, "SALH5 PoliceHeli EU")] = "d4d127e197bf32ababa86a71d8771aca5d2d56c06c20a6c6a2823df6671cebc8",
            [(LCSSave.FileTypes.PS2, "SALH5 SicilianGambit")] = "7f9f2ce9c34ca0e46ba62fa3eb0c415f1d5a300b095dd9b85490bfcef20bc426",
            [(LCSSave.FileTypes.PS2, "SALH5 SicilianGambit EU")] = "bfe0ea344abc904e628c7ae9e7d387a36769670fc12b2ee89f150fc338d81e48",
            [(LCSSave.FileTypes.PS2, "TOSH4 CashInKazukisChip")] = "e78a80d4f6966af3cd63d512cb40b90b0b77a8b25820c471b0aa327a8dc2bd65",
            [(LCSSave.FileTypes.PS2, "VIC1 AllSideMissions")] = "7573e5a917504673c699ab45da9d9218b0efeb45f7ed92f3d5095f2f507b5437",
            [(LCSSave.FileTypes.PS2, "VIC1 AVolatileSituation")] = "55b47ab969d783a893edc0e01f1d01024f1a707b578f3c5cab6922df7e741d0f",
            [(LCSSave.FileTypes.PS2, "VIC1 HomeSweetHome 2")] = "ed05177748c9078c5cf84163610db54304c9fff148da5e369683389967068008",
            [(LCSSave.FileTypes.PS2, "VIC1 HomeSweetHome 3")] = "ed05177748c9078c5cf84163610db54304c9fff148da5e369683389967068008",
            [(LCSSave.FileTypes.PS2, "VIC1 HomeSweetHome 4")] = "ed05177748c9078c5cf84163610db54304c9fff148da5e369683389967068008",
            [(LCSSave.FileTypes.PS2, "VIC1 MasterSave")] = "d962f186f04874e2e4e7ced2fa156d6e43418de07e540d31a1f1fddbdd79987d",
            [(LCSSave.FileTypes.PSP, "MAR3")] = "b7b58cb1e2c3cb54ab21e3a57ad3a07c34bb6d326c79a6229d4723d11ec6d4cc",
            [(LCSSave.FileTypes.PSP, "VIC1")] = "ad41f2d79f45b78bb468e9d4dd14df44f5e5ef379f7880987d2451acc95dab48",
            [(LCSSave.FileTypes.PSP, "SALH5")] = "414cb4f3ee78478a5f9f2e7d18c65f4fe44838872bad9518d0b0fd07757b4517",
            [(LCSSave.FileTypes.PSP, "MAR3 BoobyPrize")] = "00bd6d194c5197474d3282583f69acf672a94ebd62e45aa380be8fd2b9a457d0",
            [(LCSSave.FileTypes.PSP, "NEDS1 LCConfidential")] = "df0130b46d067aea0d1a3be5031fe1adc074598c6240e3b874e39ba22e214b3b",
            [(LCSSave.FileTypes.PSP, "SAL8 DrivingMrLeone")] = "5d090143bb0d88499dd9d3983911429b8ff341eeb2ddcf1b7e3bba9827af55f7",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent")] = "f1cedebf16a987e1681d55311997471b463b21f2677a92cda8bcdf0dc332bf9b",
            [(LCSSave.FileTypes.PSP, "SALH5 GlitchedVehicles")] = "06c3bcef76d220ce8e52f04995392038869226f584fb9fb8c132796de155c43c",
            [(LCSSave.FileTypes.PSP, "SALH5 GlitchedVehicles EU")] = "9055a515d8a0c1a0b427db6c73a893d15de6023bcbeb4bbf78fdf15db680e928",
            [(LCSSave.FileTypes.PSP, "SALH5 RCBandit")] = "8c0a18ee5abee111209f944fbb9202a0013ba37cc8bb32057c7b9fcdd7b1dc05",
            [(LCSSave.FileTypes.PSP, "SALH5 Panlantic")] = "19f9c06b1d7f9fd32d32d1568a9a4040269e1807f9796266b2bbc8fb33154074",
            [(LCSSave.FileTypes.PSP, "SALH5 Rhino")] = "990cb994e7cb11624a5c8cb882035e9592974c4b0c82bc6f31043a2cc642fccf",
            [(LCSSave.FileTypes.PSP, "SALH5 TinyInvisibleHeli")] = "19f9c06b1d7f9fd32d32d1568a9a4040269e1807f9796266b2bbc8fb33154074",
            [(LCSSave.FileTypes.PSP, "SALH5 TinyInvisibleHeli EU")] = "ad66b583ca9997cd74d6eb505f6ed0a46fcd3156f48da35abe8e883f0ec637d5",
            [(LCSSave.FileTypes.PSP, "SALH5 Hunter")] = "0b51dfb6d827d70f7829a9225ea8c1770423013fa85a68a444dd383a88a195e0",
            [(LCSSave.FileTypes.PSP, "SALH5 Dodo")] = "825440f3d63e04b6edde71fe37413fd6a75e1903a81f42df105b406d09d9b57f",
            [(LCSSave.FileTypes.PSP, "SALH5 Dodo EU")] = "ad66b583ca9997cd74d6eb505f6ed0a46fcd3156f48da35abe8e883f0ec637d5",
            [(LCSSave.FileTypes.PSP, "SALH5 GangVehicles")] = "d70b5558ad587c99bb1afe09859b05d0c94de68e4157b1b8c6d69484293fc24d",
            [(LCSSave.FileTypes.PSP, "SALH5 BPHelicopters")] = "514cd0137145bb65dc0a3b81c21bd97934a1af77a002627baf28333aea6763e7",
            [(LCSSave.FileTypes.PSP, "SALH5 BPHelicopters EU")] = "9055a515d8a0c1a0b427db6c73a893d15de6023bcbeb4bbf78fdf15db680e928",
            [(LCSSave.FileTypes.PSP, "SALH5 Maverick")] = "4180b2c7c35aee8d8206967f9a9b2b95a8a85b8f2406fafa37a89c1b00128c68",
            [(LCSSave.FileTypes.PSP, "DONS2 SteeringTheVote")] = "5a347c6c585dee10bf64efb6436ef2488ac57732a328b0477d32741211162ad5",
            [(LCSSave.FileTypes.PSP, "RAYS4 NightOfTheLividDreads")] = "a2897a4c6cba68d8e6e65c93375f47f94fd043dd2ae7bf073f6fbaf71ea6a4d2",
            [(LCSSave.FileTypes.PSP, "SALH5 43Percent")] = "64d11a2f0739388e1f482fb7bc96c6ed11ff1a50a75451afc8cd7a624aef8786",
            [(LCSSave.FileTypes.PSP, "SALH5 44Percent")] = "fc0192aa18017188d2e46c9e1e65cdfed047dd325ff0fb55b60189dafa16abec",
            [(LCSSave.FileTypes.PSP, "SALH5 50Percent")] = "2194b5a690abd059d76d4431401acec565a1ed9da58365eb1879b6b53ece3b24",
            [(LCSSave.FileTypes.PSP, "SALH5 51Percent")] = "27b20890347446bf0e640354df26f03adc823a7637d51cbb0ec9f5fc3d1062d1",
            [(LCSSave.FileTypes.PSP, "SALH5 80Percent")] = "b7e0187ffc4cf32130dfc1acebcf3bf7d408685bcbf8c6b316f696202a0629f2",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU")] = "cdde24e8866c06538cef599936fa5d22ab3beada41d7b64dddac1fecfa05012b",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 2")] = "00a2c8f5b50701a000f34db718a40ec1caa7de2deadad4194875bd2ed6917f73",
            [(LCSSave.FileTypes.PSP, "VIC1 2Percent")] = "3663286b27ec8a126d78e0c728495de0ee79064a1e6f3134ecdafc208657657b",
            [(LCSSave.FileTypes.PSP, "VIC1 5Percent")] = "eae22d0dc4b86ec1ea7cf0a928d75d6bd72419f3930b3af783425016fc2efc40",
            [(LCSSave.FileTypes.PSP, "VIC1 48Percent")] = "ad41f2d79f45b78bb468e9d4dd14df44f5e5ef379f7880987d2451acc95dab48",
            [(LCSSave.FileTypes.PSP, "VIC1 49Percent")] = "c837d75b2bd986c824a06cf494e24ad023c9ee1c8c6b8828704eed0a569e36c7",
            [(LCSSave.FileTypes.PSP, "VIC1 50Percent")] = "0a0ced28ef71e3cfdb9e2969f6a47fea8d298b33ba9ddcf38552ad403f24af47",
            [(LCSSave.FileTypes.PSP, "ULUS10041 S0")] = "8bee4116fb91006feb717045fd6562f8e89216422e1e9136af869f45bb21c4c9",
            [(LCSSave.FileTypes.PSP, "ULUS10041 S1")] = "b386f3d02f31d15dc61a44f9fc19f8f01f1467036e64c3864733789d01019391",
            [(LCSSave.FileTypes.PSP, "ULUS10041 S2")] = "ad41f2d79f45b78bb468e9d4dd14df44f5e5ef379f7880987d2451acc95dab48",
            [(LCSSave.FileTypes.PSP, "ULUS10041 S3")] = "774c8095acc1e0d934bdf8d62770b5211b2051298538b30985a11c3175839f8f",
            [(LCSSave.FileTypes.PSP, "ULUS10041 S7")] = "e1c337ba9eb4fd8eceed7a20e459e5e030a66cb2fb24821513c5374e707bf330",
            [(LCSSave.FileTypes.PSP, "ULES00151 S0")] = "e1c337ba9eb4fd8eceed7a20e459e5e030a66cb2fb24821513c5374e707bf330",
            [(LCSSave.FileTypes.PSP, "ULES00151 S1")] = "e29bbacddeba375a5b1690ba4a192d2d2ce0a0c961ac75f4c6770774ee7599a3",
            [(LCSSave.FileTypes.PSP, "ULES00151 S2")] = "df0130b46d067aea0d1a3be5031fe1adc074598c6240e3b874e39ba22e214b3b",
            [(LCSSave.FileTypes.PSP, "ULJM05255 S0 Japan")] = "864d6248fc67e31de2edda1d5912458f9749d996d8b3141dde7dc8dc7e40f5cd",
            [(LCSSave.FileTypes.PSP, "DONS1 34Percent 2")] = "a675fc30df708e57fab2b4340e855f3e54c22194a4335c2d3fd8003c6f09f0d2",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 17")] = "f1cedebf16a987e1681d55311997471b463b21f2677a92cda8bcdf0dc332bf9b",
            [(LCSSave.FileTypes.PSP, "SAL8 PortlandComplete EU 2")] = "794a8a0953c1edef40d3090f9d8741ff8ffe8eda7d4285c94f43c904ac114c31",
            [(LCSSave.FileTypes.PSP, "SALH2 35Percent EU 2")] = "5e3c85c5e5b1217e5132ec8baef2dfc4f3c86ecf3c647c28f2604371fa8ee413",
            [(LCSSave.FileTypes.PSP, "SALH5 42Percent EU 2")] = "3b0e9337d52879bda5824ae20e8b24e502e40b86f00271773a190c82c9e87fd1",
            [(LCSSave.FileTypes.PSP, "SAL6 CalmBeforeStorm 3")] = "54a9f26ce987398e3ef4cea3b6d1847fe37247c28492cec9b54c2d7a5fb7aa50",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 18")] = "8c0a18ee5abee111209f944fbb9202a0013ba37cc8bb32057c7b9fcdd7b1dc05",
            [(LCSSave.FileTypes.PSP, "SALH5 40Percent EU 2")] = "1201e37b23e6a604797055a09325e07ea8ee945df68db180625b5f431f124448",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 20")] = "c2086a553f07a9f298843ce7a384d011fc07168dc886083ad53634118d6c21b1",
            [(LCSSave.FileTypes.PSP, "SAL8 19Percent EU 2")] = "f077849189f4566b01e346853b4360204d0ab51cb65a4f35ccbfa77c3e62e3d7",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 19")] = "cf0f3d22d402b56a90ccf72eff92e5c2569c7d1b54a2cfe7d74f3ecf4bcd562e",
            [(LCSSave.FileTypes.PSP, "SAL6 CalmBeforeStorm 2 2")] = "da8b572372faec7799c528863c29f351aa5ab6b4cd13e73975af072d458dfed1",
            [(LCSSave.FileTypes.PSP, "SAL8 23Percent 2")] = "5d090143bb0d88499dd9d3983911429b8ff341eeb2ddcf1b7e3bba9827af55f7",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 21")] = "a60c1ddc9f9459c7672c96ad6d61346c77a35169e67231c9b2f7f57e50399d32",
            [(LCSSave.FileTypes.PSP, "SALH1 33Percent EU 2")] = "659890db20eafbd3fc793894607bcf71b42374447c7e6ec1793c9c8be3672344",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 20")] = "4a7145fd9302ed3bde426d8c08cf33d3db237d56ddf57a1b21103acc81cbfc3e",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 22")] = "5164bd59d1626752cf75a5fc5a14a565f86132c7e22c6615016c95bf76a80c58",
            [(LCSSave.FileTypes.PSP, "VIC1 25Percent 2")] = "eae22d0dc4b86ec1ea7cf0a928d75d6bd72419f3930b3af783425016fc2efc40",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 23")] = "00a2c8f5b50701a000f34db718a40ec1caa7de2deadad4194875bd2ed6917f73",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 21")] = "19f9c06b1d7f9fd32d32d1568a9a4040269e1807f9796266b2bbc8fb33154074",
            [(LCSSave.FileTypes.PSP, "SALH5 47Percent 2")] = "64d11a2f0739388e1f482fb7bc96c6ed11ff1a50a75451afc8cd7a624aef8786",
            [(LCSSave.FileTypes.PSP, "SALH3 HeliInGarage EU 2")] = "980a802210d36f4d09798da10e1ab99d828a27203c2f0e2b11fba8a14d54edb0",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 24")] = "bb2b2fc23321df41a60ffc77087820e552384acfb97fb8aea77a397536b4cc32",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 22")] = "8c0a18ee5abee111209f944fbb9202a0013ba37cc8bb32057c7b9fcdd7b1dc05",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 25")] = "d70b5558ad587c99bb1afe09859b05d0c94de68e4157b1b8c6d69484293fc24d",
            [(LCSSave.FileTypes.PSP, "VIC1 49Percent 3")] = "c837d75b2bd986c824a06cf494e24ad023c9ee1c8c6b8828704eed0a569e36c7",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 23")] = "00a2c8f5b50701a000f34db718a40ec1caa7de2deadad4194875bd2ed6917f73",
            [(LCSSave.FileTypes.PSP, "VIC1 StarterSave 2")] = "e1c337ba9eb4fd8eceed7a20e459e5e030a66cb2fb24821513c5374e707bf330",
            [(LCSSave.FileTypes.PSP, "SALH5 KingOutfit 2")] = "8c0a18ee5abee111209f944fbb9202a0013ba37cc8bb32057c7b9fcdd7b1dc05",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 26")] = "6c425da732278e456246de9cba73f9f67f48d083432068121cab69778a4de71a",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 27")] = "6c425da732278e456246de9cba73f9f67f48d083432068121cab69778a4de71a",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 28")] = "a5c73b22fa20718285b18b07517be2af0669ba84fa66d903430f7a188668af2e",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent Japan 5")] = "9055a515d8a0c1a0b427db6c73a893d15de6023bcbeb4bbf78fdf15db680e928",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 29")] = "69b9ee69893b6174a355a638008ef54ef7b2be71e012897aebaa1500a6fcb16b",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 24")] = "69b9ee69893b6174a355a638008ef54ef7b2be71e012897aebaa1500a6fcb16b",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 30")] = "5164bd59d1626752cf75a5fc5a14a565f86132c7e22c6615016c95bf76a80c58",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 25")] = "5164bd59d1626752cf75a5fc5a14a565f86132c7e22c6615016c95bf76a80c58",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 31")] = "51ee3f001112f557509a4c5210ee4662ee3d7c6cf78b55c88087a9dd6d93ecb9",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent Japan 6")] = "9055a515d8a0c1a0b427db6c73a893d15de6023bcbeb4bbf78fdf15db680e928",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 26")] = "ddef6aab89ade48a17804b2f37733d4ab51a6a334b38db2b9981db95e0883dbf",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 32")] = "51ee3f001112f557509a4c5210ee4662ee3d7c6cf78b55c88087a9dd6d93ecb9",
            [(LCSSave.FileTypes.PSP, "VIC1 49Percent EU 2")] = "c837d75b2bd986c824a06cf494e24ad023c9ee1c8c6b8828704eed0a569e36c7",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 27")] = "97c914784cb7456342abab6b7d2adff168ab9520fcda90039cf3851e55b942af",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 33")] = "ddef6aab89ade48a17804b2f37733d4ab51a6a334b38db2b9981db95e0883dbf",
            [(LCSSave.FileTypes.PSP, "SALH5 HideoutVehicles Japan 2")] = "9055a515d8a0c1a0b427db6c73a893d15de6023bcbeb4bbf78fdf15db680e928",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 28")] = "69b9ee69893b6174a355a638008ef54ef7b2be71e012897aebaa1500a6fcb16b",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 34")] = "51ee3f001112f557509a4c5210ee4662ee3d7c6cf78b55c88087a9dd6d93ecb9",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 35")] = "51ee3f001112f557509a4c5210ee4662ee3d7c6cf78b55c88087a9dd6d93ecb9",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 29")] = "ddef6aab89ade48a17804b2f37733d4ab51a6a334b38db2b9981db95e0883dbf",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent Japan 7")] = "9055a515d8a0c1a0b427db6c73a893d15de6023bcbeb4bbf78fdf15db680e928",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 30")] = "4ac2de89437951a679057fa72a0ca3f16229abed1b3b38a5afe553183a61ace0",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent 31")] = "5164bd59d1626752cf75a5fc5a14a565f86132c7e22c6615016c95bf76a80c58",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent EU 36")] = "4ac2de89437951a679057fa72a0ca3f16229abed1b3b38a5afe553183a61ace0",
            [(LCSSave.FileTypes.PSP, "SALH5 100Percent Japan 8")] = "9055a515d8a0c1a0b427db6c73a893d15de6023bcbeb4bbf78fdf15db680e928",
        };
    }
}
