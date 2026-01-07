using Bogus;
using System;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.LCS.Tests
{
    public class TestSaveFileLCS : Base<LCSSave>
    {
        public override LCSSave GenerateTestObject(LCSSaveParams p)
        {
            Faker<LCSSave> model = new Faker<LCSSave>()
                .RuleFor(x => x.FileType, p.FileType)
                .RuleFor(x => x.SimpleVars, Generator.Generate<SimpleVariables, TestSimpleVariables, LCSSaveParams>(p))
                .RuleFor(x => x.Scripts, Generator.Generate<ScriptData, TestScriptData, LCSSaveParams>(p))
                .RuleFor(x => x.Garages, Generator.Generate<GarageData, TestGarageData, LCSSaveParams>(p))
                .RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo, LCSSaveParams>(p))
                .RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats, LCSSaveParams>(p))
                ;

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void FileFormatDetection(FileType expectedFileType, string filename)
        {
            string path = TestData.GetTestDataPath(Game.LCS, expectedFileType, filename);
            LCSSave.TryGetFileType(path, out FileType detectedFormat);

            Assert.Equal(expectedFileType, detectedFormat);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(LCSSaveParams p)
        {
            LCSSave x0 = GenerateTestObject(p);
            LCSSave x1 = CreateSerializedCopy(x0, p, out byte[] data);

            AssertSavesAreEqual(x0, x1);
            AssertCheckSumValid(data, p.FileType);
            Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        }

        //[Theory]
        //[MemberData(nameof(TestFiles))]
        //public void RealDataSerialization(FileType fileType, string filename)
        //{
        //    string path = TestData.GetTestDataPath(Game.LCS, fileType, filename);

        //    LCSSaveParams p = LCSSaveParams.GetDefaults(fileType);
        //    LCSSave x0 = LCSSave.LoadFromFile(path, fileType);
        //    LCSSave x1 = CreateSerializedCopy(x0, p, out byte[] data);

        //    AssertSavesAreEqual(x0, x1);
        //    AssertCheckSumValid(data, p.FileType);
        //    Assert.Equal(GetSizeOfTestObject(x0, p), data.Length);
        //}

        //// TODO: generate JSON
        //static readonly LCSSave Android_SALH5 = new LCSSave()
        //{
        //    SimpleVars = new SimpleVariables
        //    {
        //        LastMissionPassedName = "SALH5",
        //        CurrentLevel = 3,
        //        CurrentArea = 1,
        //        Language = 5,
        //        MillisecondsPerGameMinute = 1000,
        //        // etc.
        //    },

        //    Garages = new GarageData()
        //    {
        //        NumGarages = 18,
        //        FreeBombs = false,
        //        FreeResprays = false,
        //        CarTypesCollected = (LoveMediaCars) 0xFFFF,
        //        TimeHelpMessageLastShown = 687836,
        //        CarsInSafeHouse = new ObservableArray<StoredCar>() {
        //            new StoredCar() {
        //                Model = 137, Position = new Vector3(1159.288f, -270.625f, 17.55272f),
        //                Forward = new Vector3(-0.02580167f, 0.9996644f, -0.002366043f),
        //                TractionMultiplier = 1, Flags = (StoredCarFlags) 0x21, Color1 = 6, Color2 = 0,
        //                Radio = (RadioStation) 11, Extra1 = 2, Extra2 = -1
        //            },
        //            new StoredCar(),
        //            new StoredCar() {
        //                Model = 184, Position = new Vector3(-791.0613f, 287.3123f, 48.87781f),
        //                Forward = new Vector3(0.3067619f, 0.9515517f, -0.02113294f),
        //                TractionMultiplier = 1, Flags = 0, Color1 = 1, Color2 = 1,
        //                Radio = (RadioStation) 5, Extra1 = -1, Extra2 = -1
        //            }
        //        },
        //        Garages = new ObservableArray<Garage>() {
        //            new Garage() {
        //                Door1Pointer = 3534146944,
        //                Door2Pointer = 0,
        //                Corner1 = new Vector2(1160.7f, -267.5f),
        //                InfX = 1156f, InfY = -273.7f, InfZ = 16.25f,        // lol why is this not a vector?
        //                SupX = 1160.7f, SupY = -267.5f, SupZ = 19.25f,
        //                Dir1 = new Vector2(-1f, 0), Dir2 = new Vector2(0, -1f),
        //                Type = (GarageType) 16,
        //                RotatingDoor = true,
        //            },
        //            new Garage(),
        //            new Garage(),
        //            new Garage() {
        //                Door1Pointer = 3534146832,
        //                Door2Pointer = 0,
        //                Corner1 = new Vector2(1435.5f, -183.5f),
        //                InfX = 1428f, InfY = -187.5f, InfZ = 49.5f,
        //                SupX = 1142.5f, SupY = -179f, SupZ = 56f,
        //                Dir1 = new Vector2(-0.857493f, 0.5144958f), Dir2 = new Vector2(0.8682432f, -0.4961389f),
        //                Type = (GarageType) 19,
        //                RotatingDoor = true,
        //            }
        //        }
        //    }
        //};

        //static readonly LCSSave[] TestSaves =
        //{
        //    Android_SALH5,
        //};

        //void VerifySimpleVars(SimpleVariables expected, SimpleVariables actual)
        //{
        //    Assert.Equal(expected.LastMissionPassedName, actual.LastMissionPassedName);
        //    Assert.Equal(expected.CurrentLevel, actual.CurrentLevel);
        //    Assert.Equal(expected.CurrentArea, actual.CurrentArea);
        //    Assert.Equal(expected.Language, actual.Language);
        //    Assert.Equal(expected.MillisecondsPerGameMinute, actual.MillisecondsPerGameMinute);
        //}

        //const float FloatTolerance = 0.001f;

        //void VerifyGarage(Garage expected, Garage actual)
        //{
        //    Assert.Equal(expected.Type, actual.Type);
        //    Assert.Equal(expected.RotatingDoor, actual.RotatingDoor);
        //    Assert.Equal(expected.Door1Pointer, actual.Door1Pointer);
        //    Assert.True(Vector2.Distance(expected.Dir1, actual.Dir1) < FloatTolerance);
        //    Assert.True(Vector2.Distance(expected.Dir2, actual.Dir2) < FloatTolerance);
        //    Assert.True(Vector2.Distance(expected.Corner1, actual.Corner1) < FloatTolerance);
        //}

        //[Theory]
        //[InlineData(GameVersionId.Android, "SALH5 100Percent", 0)]
        ////[InlineData(GameConsole.iOS, "SALH5 100Percent")]
        ////[InlineData(GameConsole.PS2, "SALH5 100Percent")]
        ////[InlineData(GameConsole.PSP, "SALH5")]
        //public void VerifyLoad(GameVersionId console, string filename, int index)
        //{
        //    FileType format = LCSSave.FileTypes.GetByGameConsole(console);
        //    string path = TestData.GetTestDataPath(Game.LCS, format, filename);

        //    LCSSave goodSave = TestSaves[index];
        //    LCSSave save = LCSSave.LoadFromFile(path, format);

        //    VerifySimpleVars(goodSave.SimpleVars, save.SimpleVars);

        //    Assert.Equal(goodSave.Garages.CarsInSafeHouse[0], save.Garages.CarsInSafeHouse[0]);
        //    Assert.Equal(goodSave.Garages.CarsInSafeHouse[2], save.Garages.CarsInSafeHouse[2]);

        //    VerifyGarage(goodSave.Garages.Garages[0], save.Garages.Garages[0]);
        //    VerifyGarage(goodSave.Garages.Garages[3], save.Garages.Garages[3]);

        //}

        [Fact]
        public void CopyConstructor()
        {
            LCSSave x0 = GenerateTestObject();
            LCSSave x1 = new LCSSave(x0);

            Assert.Equal(x0, x1);
        }

        private void AssertSavesAreEqual(LCSSave x0, LCSSave x1)
        {
            Assert.Equal(x0.SimpleVars, x1.SimpleVars);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.PlayerInfo, x1.PlayerInfo);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0, x1);
        }

        private void AssertCheckSumValid(byte[] data, FileType format)
        {
            if (format.IsPS2)
            {
                int sumOffset = data.Length - 4;
                int calculatedSum = data.Take(sumOffset).Sum(x => x);
                int storedSum = BitConverter.ToInt32(data, sumOffset);
                Assert.Equal(calculatedSum, storedSum);
            }
        }
    }
}
