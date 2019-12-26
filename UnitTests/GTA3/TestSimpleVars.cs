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
        public override SimpleVars GenerateTestVector(SystemType system)
        {
            Faker<SimpleVars> model = new Faker<SimpleVars>()
                .RuleFor(x => x.LastMissionPassedName, f => !system.HasFlag(SystemType.PS2) ? TestHelper.RandomString(f, 23) : string.Empty)
                .RuleFor(x => x.SaveTime, (system.HasFlag(SystemType.PC) || system.HasFlag(SystemType.Xbox)) ? TestHelper.Generate<SystemTime, TestSystemTime>() : new SystemTime())
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
                .RuleFor(x => x.OldWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.NewWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.ForcedWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.InterpolationValue, f => f.Random.Float())
                .RuleFor(x => x.PrefsMusicVolume, f => system.HasFlag(SystemType.PS2) ? f.Random.Int() : 0)
                .RuleFor(x => x.PrefsSfxVolume, f => system.HasFlag(SystemType.PS2) ? f.Random.Int() : 0)
                .RuleFor(x => x.PrefsUseVibration, f => system.HasFlag(SystemType.PS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsStereoMono, f => system.HasFlag(SystemType.PS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsRadioStation, f => system.HasFlag(SystemType.PS2) ? f.PickRandom<RadioStation>() : 0)
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
        [InlineData(SystemType.Android, 0xB0)]
        [InlineData(SystemType.IOS, 0xB0)]
        [InlineData(SystemType.PC, 0xBC)]
        [InlineData(SystemType.PS2, 0xB0)]
        [InlineData(SystemType.PS2AU, 0xA8)]
        [InlineData(SystemType.PS2JP, 0xB0)]
        [InlineData(SystemType.Xbox, 0xBC)]
        public void Serialization(SystemType system, int expectedSize)
        {
            SimpleVars x0 = GenerateTestVector(system);
            SimpleVars x1 = TestHelper.CreateSerializedCopy(x0, out byte[] data, system);

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }
    }
}
