using Bogus;
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
                .RuleFor(x => x.TimeStamp, f => (format.IsPC || format.IsXbox) ? new SystemTime(Generator.Date(f)) : SystemTime.MinValue)
                .RuleFor(x => x.SizeOfGameInBytes, f => (format.IsPS2 && format.IsJapanese) ? 0x31400 : 0x31401)
                .RuleFor(x => x.CurrentLevel, f => f.PickRandom<Level>())
                .RuleFor(x => x.CameraPosition, f => Generator.Vector3(f))
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
                .RuleFor(x => x.MusicVolume, f => (format.IsPS2) ? f.Random.Int() : default)
                .RuleFor(x => x.SfxVolume, f => (format.IsPS2) ? f.Random.Int() : default)
                .RuleFor(x => x.UseVibration, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.StereoOutput, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.RadioStation, f => (format.IsPS2) ? f.PickRandom<RadioStation>() : default)
                .RuleFor(x => x.Brightness, f => (format.IsPS2) ? f.Random.Int() : default)
                .RuleFor(x => x.ShowSubtitles, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.Language, f => (format.IsPS2) ? f.PickRandom<Language>() : default)
                .RuleFor(x => x.UseWideScreen, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.BlurOn, f => (format.IsPS2) ? f.Random.Bool() : default)
                .RuleFor(x => x.CompileDateAndTime, f => new Date(Generator.Date(f)))
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.CameraModeInCar, f => f.Random.Float())
                .RuleFor(x => x.CameraModeOnFoot, f => f.Random.Float())
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
            Assert.Equal(x0.SizeOfGameInBytes, x1.SizeOfGameInBytes);
            Assert.Equal(x0.CurrentLevel, x1.CurrentLevel);
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
            Assert.Equal(x0.MusicVolume, x1.MusicVolume);
            Assert.Equal(x0.SfxVolume, x1.SfxVolume);
            Assert.Equal(x0.UseVibration, x1.UseVibration);
            Assert.Equal(x0.StereoOutput, x1.StereoOutput);
            Assert.Equal(x0.RadioStation, x1.RadioStation);
            Assert.Equal(x0.Brightness, x1.Brightness);
            Assert.Equal(x0.ShowSubtitles, x1.ShowSubtitles);
            Assert.Equal(x0.Language, x1.Language);
            Assert.Equal(x0.UseWideScreen, x1.UseWideScreen);
            Assert.Equal(x0.BlurOn, x1.BlurOn);
            Assert.Equal(x0.CompileDateAndTime, x1.CompileDateAndTime);
            Assert.Equal(x0.WeatherTypeInList, x1.WeatherTypeInList);
            Assert.Equal(x0.CameraModeInCar, x1.CameraModeInCar);
            Assert.Equal(x0.CameraModeOnFoot, x1.CameraModeOnFoot);
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
