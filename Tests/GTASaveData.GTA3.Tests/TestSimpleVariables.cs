using Bogus;
using System;
using TestFramework;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3.Tests
{
    public class TestSimpleVariables : Base<SimpleVariables>
    {
        public override SimpleVariables GenerateTestObject(FileFormat format)
        {
            Faker<SimpleVariables> model = new Faker<SimpleVariables>()
                .RuleFor(x => x.LastMissionPassedName, f => (!format.IsPS2) ? Generator.Words(f, SimpleVariables.MaxMissionPassedNameLength - 1) : "")
                .RuleFor(x => x.TimeStamp, f => (format.IsPC || format.IsXbox) ? Generator.Date(f) : DateTime.MinValue)
                .RuleFor(x => x.CurrLevel, f => f.PickRandom<Level>())
                .RuleFor(x => x.CameraPosition, f => Generator.Vector3D(f))
                .RuleFor(x => x.MillisecondsPerGameMinute, f => f.Random.Int())
                .RuleFor(x => x.LastClockTick, f => f.Random.UInt())
                .RuleFor(x => x.GameClockHours, f => f.Random.Byte())
                .RuleFor(x => x.GameClockMinutes, f => f.Random.Byte())
                .RuleFor(x => x.CurrPadMode, f => f.Random.Short())
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
                .RuleFor(x => x.WeatherInterpolation, f => f.Random.Float())
                .RuleFor(x => x.PrefsMusicVolume, f => (format.IsPS2) ? f.Random.Int() : default)
                .RuleFor(x => x.PrefsSfxVolume, f => (format.IsPS2) ? f.Random.Int() : default)
                .RuleFor(x => x.PrefsUseVibration, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.PrefsStereoMono, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.PrefsRadioStation, f => (format.IsPS2) ? f.PickRandom<RadioStation>() : default)
                .RuleFor(x => x.PrefsBrightness, f => (format.IsPS2) ? f.Random.Int() : default)
                .RuleFor(x => x.PrefsShowSubtitles, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.PrefsLanguage, f => (format.IsPS2) ? f.PickRandom<Language>() : default)
                .RuleFor(x => x.PrefsUseWideScreen, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.BlurOn, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.CompileDateAndTime, f => Generator.Date(f))
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.CameraCarZoomIndicator, f => f.Random.Float())
                .RuleFor(x => x.CameraPedZoomIndicator, f => f.Random.Float())
                .RuleFor(x => x.IsQuickSave, f => (format.IsMobile) ? f.PickRandom<QuickSaveState>() : default);

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            SimpleVariables x0 = GenerateTestObject(format);
            SimpleVariables x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.LastMissionPassedName, x1.LastMissionPassedName);
            Assert.Equal(x0.TimeStamp, x1.TimeStamp);
            Assert.Equal(x0.CurrLevel, x1.CurrLevel);
            Assert.Equal(x0.CameraPosition, x1.CameraPosition);
            Assert.Equal(x0.MillisecondsPerGameMinute, x1.MillisecondsPerGameMinute);
            Assert.Equal(x0.LastClockTick, x1.LastClockTick);
            Assert.Equal(x0.GameClockHours, x1.GameClockHours);
            Assert.Equal(x0.GameClockMinutes, x1.GameClockMinutes);
            Assert.Equal(x0.CurrPadMode, x1.CurrPadMode);
            Assert.Equal(x0.TimeInMilliseconds, x1.TimeInMilliseconds);
            Assert.Equal(x0.TimeScale, x1.TimeScale);
            Assert.Equal(x0.TimeStep, x1.TimeStep);
            Assert.Equal(x0.TimeStepNonClipped, x1.TimeStepNonClipped);
            Assert.Equal(x0.FrameCounter, x1.FrameCounter);
            Assert.Equal(x0.TimeStep2, x1.TimeStep2);
            Assert.Equal(x0.FramesPerUpdate, x1.FramesPerUpdate);
            Assert.Equal(x0.TimeScale2, x1.TimeScale2);
            Assert.Equal(x0.OldWeatherType, x1.OldWeatherType);
            Assert.Equal(x0.NewWeatherType, x1.NewWeatherType);
            Assert.Equal(x0.ForcedWeatherType, x1.ForcedWeatherType);
            Assert.Equal(x0.WeatherInterpolation, x1.WeatherInterpolation);
            Assert.Equal(x0.PrefsMusicVolume, x1.PrefsMusicVolume);
            Assert.Equal(x0.PrefsSfxVolume, x1.PrefsSfxVolume);
            Assert.Equal(x0.PrefsUseVibration, x1.PrefsUseVibration);
            Assert.Equal(x0.PrefsStereoMono, x1.PrefsStereoMono);
            Assert.Equal(x0.PrefsRadioStation, x1.PrefsRadioStation);
            Assert.Equal(x0.PrefsBrightness, x1.PrefsBrightness);
            Assert.Equal(x0.PrefsShowSubtitles, x1.PrefsShowSubtitles);
            Assert.Equal(x0.PrefsLanguage, x1.PrefsLanguage);
            Assert.Equal(x0.PrefsUseWideScreen, x1.PrefsUseWideScreen);
            Assert.Equal(x0.BlurOn, x1.BlurOn);
            Assert.Equal(x0.CompileDateAndTime, x1.CompileDateAndTime);
            Assert.Equal(x0.WeatherTypeInList, x1.WeatherTypeInList);
            Assert.Equal(x0.CameraCarZoomIndicator, x1.CameraCarZoomIndicator);
            Assert.Equal(x0.CameraPedZoomIndicator, x1.CameraPedZoomIndicator);
            Assert.Equal(x0.IsQuickSave, x1.IsQuickSave);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            SimpleVariables x0 = GenerateTestObject();
            SimpleVariables x1 = new SimpleVariables(x0);

            Assert.Equal(x0, x1);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
