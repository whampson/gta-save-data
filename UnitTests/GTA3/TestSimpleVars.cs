using Bogus;
using GTASaveData;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestSimpleVars
        : SaveDataObjectTestBase<SimpleVars>
    {
        private const int SizeAndroid = 0xB0;
        private const int SizeIOS = 0xB0;
        private const int SizePC = 0xBC;
        private const int SizePS2 = 0xB0;
        private const int SizePS2AU = 0xA8;
        private const int SizePS2JP = 0xB0;
        private const int SizeXbox = 0xBC;

        private const uint FileIdJP = 0x31400;
        private const uint FileIdNonJP = 0x31401;

        public override SimpleVars GenerateTestVector(SystemType system)
        {
            //Faker f = new Faker();
            //SimpleVars x = new SimpleVars();

            //if (!system.HasFlag(SystemType.PS2))
            //{
            //    x.LastMissionPassedName = TestHelper.RandomString(f, 23);
            //}
            //if (system.HasFlag(SystemType.PC) || system.HasFlag(SystemType.Xbox))
            //{
            //    // x.SaveTime = TestHelper.GenerateTestVector<SystemTime>(system); // prepend 'Test' onto typename
            //    x.SaveTime = TestSystemTime.Generate();
            //}

            //// TODO: finish this
            ////  make TestBase object and make inheritors override GenerateTestVector(SystemType)

            //return x;

            Faker<SimpleVars> model = new Faker<SimpleVars>()
                .RuleFor(x => x.LastMissionPassedName, f => !system.HasFlag(SystemType.PS2) ? TestHelper.RandomString(f, 23) : string.Empty)
                .RuleFor(x => x.SaveTime, (system.HasFlag(SystemType.PC) || system.HasFlag(SystemType.Xbox)) ? TestHelper.Generate<SystemTime, TestSystemTime>() : new SystemTime())
                .RuleFor(x => x.FileId, system.HasFlag(SystemType.PS2JP) ? 0x31400U : 0x31401U)
                .RuleFor(x => x.CurrLevel, f => f.PickRandom<Level>())
                .RuleFor(x => x.CameraPosition, f => TestHelper.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.MillisecondsPerGameMinute, f => f.Random.UInt())
                .RuleFor(x => x.LastClockTick, f => f.Random.UInt())
                .RuleFor(x => x.GameClockHours, f => f.Random.Byte())
                .RuleFor(x => x.GameClockMinutes, f => f.Random.Byte())
                .RuleFor(x => x.TimeInMilliseconds, f => f.Random.UInt())
                .RuleFor(x => x.TimeScale, f => f.Random.Float())
                .RuleFor(x => x.TimeStep, f => f.Random.Float())
                .RuleFor(x => x.TimeStepNonClipped, f => f.Random.Float())
                .RuleFor(x => x.FrameCounter, f => f.Random.UInt())
                .RuleFor(x => x.TimeStep2, f => f.Random.Float())
                .RuleFor(x => x.FramesPerUpdate, f => f.Random.Float())
                .RuleFor(x => x.TimeScale2, f => f.Random.Float())
                .RuleFor(x => x.OldWeatherType, f => f.PickRandom<Weather>())
                .RuleFor(x => x.NewWeatherType, f => f.PickRandom<Weather>())
                .RuleFor(x => x.ForcedWeatherType, f => f.PickRandom<Weather>())
                .RuleFor(x => x.InterpolationValue, f => f.Random.Float())
                .RuleFor(x => x.PrefsMusicVolume, f => system.HasFlag(SystemType.PS2) ? f.Random.Int() : 0)
                .RuleFor(x => x.PrefsSfxVolume, f => system.HasFlag(SystemType.PS2) ? f.Random.Int() : 0)
                .RuleFor(x => x.PrefsUseVibration, f => system.HasFlag(SystemType.PS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsStereoMono, f => system.HasFlag(SystemType.PS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsRadioStation, f => system.HasFlag(SystemType.PS2) ? f.PickRandom<Radio>() : 0)
                .RuleFor(x => x.PrefsBrightness, f => system.HasFlag(SystemType.PS2) ? f.Random.Int() : 0)
                .RuleFor(x => x.PrefsShowSubtitles, f => system.HasFlag(SystemType.PS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsLanguage, f => system.HasFlag(SystemType.PS2) ? f.PickRandom<Language>() : 0)
                .RuleFor(x => x.PrefsUseWideScreen, f => system.HasFlag(SystemType.PS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsControllerConfig, f => system.HasFlag(SystemType.PS2) ? f.PickRandom<PadMode>() : 0)
                .RuleFor(x => x.PrefsShowTrails, f => system.HasFlag(SystemType.PS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.CompileDateAndTime, TestHelper.Generate<Timestamp, TestTimestamp>())
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.InCarCameraMode, f => f.Random.Float())
                .RuleFor(x => x.OnFootCameraMode, f => f.Random.Float())
                .RuleFor(x => x.IsQuickSave, f => (system.HasFlag(SystemType.Android) || system.HasFlag(SystemType.IOS)) ? f.Random.Bool() : false);

            return model.Generate();
        }

        [Theory]
        [InlineData(SystemType.Android)]
        [InlineData(SystemType.IOS)]
        [InlineData(SystemType.PC)]
        [InlineData(SystemType.PS2)]
        [InlineData(SystemType.PS2AU)]
        [InlineData(SystemType.PS2JP)]
        [InlineData(SystemType.Xbox)]
        public void Serialization(SystemType system)
        {
            SimpleVars a = GenerateTestVector(system);
            SimpleVars b = TestHelper.CreateSerializedCopy(a, out byte[] _, system);    // TODO: check data size

            Assert.Equal(a, b);
        }
    }
}
