using Bogus;
using GTASaveData.Common;
using GTASaveData.GTA3;
using Tests.Helpers;
using Xunit;

namespace Tests.GTA3
{
    public class TestSimpleVars
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

        public static SimpleVars Generate(SystemType system)
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
                .RuleFor(x => x.SaveTime, (system.HasFlag(SystemType.PC) || system.HasFlag(SystemType.Xbox)) ? TestSystemTime.Generate() : new SystemTime())
                .RuleFor(x => x.FileId, system.HasFlag(SystemType.PS2JP) ? 0x31400U : 0x31401U)
                .RuleFor(x => x.CurrLevel, f => f.PickRandom<Level>())
                .RuleFor(x => x.CameraPosition, f => TestVector3d.Generate())
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
                .RuleFor(x => x.CompileDateAndTime, TestTimestamp.Generate())
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.InCarCameraMode, f => f.Random.Float())
                .RuleFor(x => x.OnFootCameraMode, f => f.Random.Float())
                .RuleFor(x => x.IsQuickSave, f => (system.HasFlag(SystemType.Android) || system.HasFlag(SystemType.IOS)) ? f.Random.Bool() : false);

            return model.Generate();
        }

        [Theory]
        [InlineData(SystemType.PC)]
        [InlineData(SystemType.PS2)]
        public void Serialization(SystemType system)
        {
            SimpleVars a = Generate(system);
            SimpleVars b = TestHelper.CreateSerializedCopy(a, out byte[] _, system);    // TODO: check data size

            Assert.Equal(a, b);
        }

        //[Fact]
        //public void SanityAndroid()
        //{
        //    SimpleVars a = Generate();
        //    SimpleVars b = TestHelper.CreateSerializedCopy(a, out byte[] data, SystemType.Android);

        //    CoreSanityCheck(a, b);

        //    Assert.Equal(FileIdNonJP, b.FileId);
        //    Assert.Equal(a.LastMissionPassedName, b.LastMissionPassedName);
        //    Assert.Equal(a.IsQuickSave, b.IsQuickSave);
        //    Assert.Equal(SizeAndroid, data.Length);
        //}

        //[Fact]
        //public void SanityIOS()
        //{
        //    SimpleVars a = Generate();
        //    SimpleVars b = TestHelper.CreateSerializedCopy(a, out byte[] data, SystemType.IOS);

        //    CoreSanityCheck(a, b);

        //    Assert.Equal(FileIdNonJP, b.FileId);
        //    Assert.Equal(a.LastMissionPassedName, b.LastMissionPassedName);
        //    Assert.Equal(a.IsQuickSave, b.IsQuickSave);
        //    Assert.Equal(SizeIOS, data.Length);
        //}

        //[Fact]
        //public void SanityPC()
        //{
        //    SimpleVars a = Generate();
        //    SimpleVars b = TestHelper.CreateSerializedCopy(a, out byte[] data, SystemType.PC);

        //    CoreSanityCheck(a, b);

        //    Assert.Equal(FileIdNonJP, b.FileId);
        //    Assert.Equal(a.LastMissionPassedName, b.LastMissionPassedName);
        //    Assert.Equal(a.SaveTime, b.SaveTime);
        //    Assert.Equal(SizePC, data.Length);
        //}

        //[Fact]
        //public void SanityPS2()
        //{
        //    SimpleVars a = Generate();
        //    SimpleVars b = TestHelper.CreateSerializedCopy(a, out byte[] data, SystemType.PS2);

        //    CoreSanityCheck(a, b);
        //    PS2SanityCheck(a, b);

        //    Assert.Equal(FileIdNonJP, b.FileId);
        //    Assert.Equal(SizePS2, data.Length);
        //}

        //[Fact]
        //public void SanityPS2AU()
        //{
        //    SimpleVars a = Generate();
        //    SimpleVars b = TestHelper.CreateSerializedCopy(a, out byte[] data, SystemType.PS2AU);

        //    CoreSanityCheck(a, b);
        //    PS2SanityCheck(a, b);

        //    Assert.Equal(FileIdNonJP, b.FileId);
        //    Assert.Equal(SizePS2AU, data.Length);
        //}

        //[Fact]
        //public void SanityPS2JP()
        //{
        //    SimpleVars a = Generate();
        //    SimpleVars b = TestHelper.CreateSerializedCopy(a, out byte[] data, SystemType.PS2JP);

        //    CoreSanityCheck(a, b);
        //    PS2SanityCheck(a, b);

        //    Assert.Equal(FileIdJP, b.FileId);
        //    Assert.Equal(SizePS2JP, data.Length);
        //}

        //[Fact]
        //public void SanityXbox()
        //{
        //    SimpleVars a = Generate();
        //    SimpleVars b = TestHelper.CreateSerializedCopy(a, out byte[] data, SystemType.Xbox);

        //    CoreSanityCheck(a, b);

        //    Assert.Equal(FileIdNonJP, b.FileId);
        //    Assert.Equal(a.LastMissionPassedName, b.LastMissionPassedName);
        //    Assert.Equal(a.SaveTime, b.SaveTime);
        //    Assert.Equal(SizeXbox, data.Length);
        //}

        //private void CoreSanityCheck(SimpleVars a, SimpleVars b)
        //{
        //    Assert.Equal(a.CurrLevel, b.CurrLevel);
        //    Assert.Equal(a.CameraPosition, b.CameraPosition);
        //    Assert.Equal(a.MillisecondsPerGameMinute, b.MillisecondsPerGameMinute);
        //    Assert.Equal(a.LastClockTick, b.LastClockTick);
        //    Assert.Equal(a.GameClockHours, b.GameClockHours);
        //    Assert.Equal(a.GameClockMinutes, b.GameClockMinutes);
        //    Assert.Equal(a.PrefsControllerConfig, b.PrefsControllerConfig);
        //    Assert.Equal(a.TimeInMilliseconds, b.TimeInMilliseconds);
        //    Assert.Equal(a.TimeScale, b.TimeScale);
        //    Assert.Equal(a.TimeStep, b.TimeStep);
        //    Assert.Equal(a.TimeStepNonClipped, b.TimeStepNonClipped);
        //    Assert.Equal(a.FrameCounter, b.FrameCounter);
        //    Assert.Equal(a.TimeStep2, b.TimeStep2);
        //    Assert.Equal(a.FramesPerUpdate, b.FramesPerUpdate);
        //    Assert.Equal(a.TimeScale2, b.TimeScale2);
        //    Assert.Equal(a.OldWeatherType, b.OldWeatherType);
        //    Assert.Equal(a.NewWeatherType, b.NewWeatherType);
        //    Assert.Equal(a.ForcedWeatherType, b.ForcedWeatherType);
        //    Assert.Equal(a.InterpolationValue, b.InterpolationValue);
        //    Assert.Equal(a.CompileDateAndTime, b.CompileDateAndTime);
        //    Assert.Equal(a.WeatherTypeInList, b.WeatherTypeInList);
        //    Assert.Equal(a.InCarCameraMode, b.InCarCameraMode);
        //    Assert.Equal(a.OnFootCameraMode, b.OnFootCameraMode);
        //}

        //private void PS2SanityCheck(SimpleVars a, SimpleVars b)
        //{
        //    Assert.Equal(a.PrefsMusicVolume, b.PrefsMusicVolume);
        //    Assert.Equal(a.PrefsSfxVolume, b.PrefsSfxVolume);
        //    Assert.Equal(a.PrefsUseVibration, b.PrefsUseVibration);
        //    Assert.Equal(a.PrefsStereoMono, b.PrefsStereoMono);
        //    Assert.Equal(a.PrefsRadioStation, b.PrefsRadioStation);
        //    Assert.Equal(a.PrefsBrightness, b.PrefsBrightness);
        //    Assert.Equal(a.PrefsShowSubtitles, b.PrefsShowSubtitles);
        //    Assert.Equal(a.PrefsLanguage, b.PrefsLanguage);
        //    Assert.Equal(a.PrefsUseWideScreen, b.PrefsUseWideScreen);
        //    Assert.Equal(a.PrefsControllerConfig, b.PrefsControllerConfig);
        //    Assert.Equal(a.PrefsShowTrails, b.PrefsShowTrails);
        //}
    }
}
