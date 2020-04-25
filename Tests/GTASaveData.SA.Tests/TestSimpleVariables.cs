using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.SA.Tests
{
    public class TestSimpleVariables : Base<SimpleVariables>
    {
        public override SimpleVariables GenerateTestObject(DataFormat format)
        {
            Faker<SimpleVariables> model = new Faker<SimpleVariables>()
                .RuleFor(x => x.VersionId, f => f.Random.UInt())
                .RuleFor(x => x.LastMissionPassedName, f => Generator.RandomAsciiString(f, SimpleVariables.Limits.MaxNameLength - 1))
                .RuleFor(x => x.MissionPackGame, f => f.Random.Byte())
                .RuleFor(x => x.CurrLevel, f => f.PickRandom<LevelType>())
                .RuleFor(x => x.CameraPosition, f => Generator.Generate<Vector3D, TestVector3D>())
                .RuleFor(x => x.MillisecondsPerGameMinute, f => f.Random.Int())
                .RuleFor(x => x.LastClockTick, f => f.Random.UInt())
                .RuleFor(x => x.GameClockMonths, f => f.Random.Byte())
                .RuleFor(x => x.GameClockDays, f => f.Random.Byte())
                .RuleFor(x => x.GameClockHours, f => f.Random.Byte())
                .RuleFor(x => x.GameClockMinutes, f => f.Random.Byte())
                .RuleFor(x => x.GameClockDayOfWeek, f => f.Random.Byte())
                .RuleFor(x => x.StoredGameClockMonths, f => f.Random.Byte())
                .RuleFor(x => x.StoredGameClockDays, f => f.Random.Byte())
                .RuleFor(x => x.StoredGameClockHours, f => f.Random.Byte())
                .RuleFor(x => x.StoredGameClockMinutes, f => f.Random.Byte())
                .RuleFor(x => x.ClockHasBeenStored, f => f.Random.Bool())
                .RuleFor(x => x.CurrPadMode, f => f.Random.Short())
                .RuleFor(x => x.HasPlayerCheated, f => f.Random.Bool())
                .RuleFor(x => x.TimeInMilliseconds, f => f.Random.UInt())
                .RuleFor(x => x.TimeScale, f => f.Random.Float())
                .RuleFor(x => x.TimeStep, f => f.Random.Float())
                .RuleFor(x => x.TimeStepNonClipped, f => f.Random.Float())
                .RuleFor(x => x.FrameCounter, f => f.Random.UInt())
                .RuleFor(x => x.OldWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.NewWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.ForcedWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.WeatherInterpolation, f => f.Random.Float())
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.Rain, f => f.Random.Float())
                .RuleFor(x => x.CameraCarZoomIndicator, f => f.Random.Int())
                .RuleFor(x => x.CameraPedZoomIndicator, f => f.Random.Int())
                .RuleFor(x => x.CurrArea, f => f.Random.Int())
                .RuleFor(x => x.InvertLook4Pad, f => f.Random.Bool())
                .RuleFor(x => x.ExtraColour, f => f.Random.Int())
                .RuleFor(x => x.ExtraColourOn, f => f.Random.Bool())
                .RuleFor(x => x.ExtraColourInterpolation, f => f.Random.Float())
                .RuleFor(x => x.ExtraColourWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.WaterConfiguration, f => f.Random.Int())
                .RuleFor(x => x.LARiots, f => f.Random.Bool())
                .RuleFor(x => x.LARiotsNoPoliceCars, f => f.Random.Bool())
                .RuleFor(x => x.MaximumWantedLevel, f => f.Random.Int())
                .RuleFor(x => x.MaximumChaosLevel, f => f.Random.Int())
                .RuleFor(x => x.GermanGame, f => f.Random.Bool())
                .RuleFor(x => x.FrenchGame, f => f.Random.Bool())
                .RuleFor(x => x.NastyGame, f => f.Random.Bool())
                .RuleFor(x => x.CinematicCamMessagesLeftToDisplay, f => f.Random.Byte())
                .RuleFor(x => x.TimeLastSaved, f => Generator.Generate<SystemTime, TestSystemTime>())
                .RuleFor(x => x.TargetMarkerHandle, f => f.Random.Int())
                .RuleFor(x => x.HasDisplayedPlayerQuitEnterCarHelpText, f => f.Random.Bool())
                .RuleFor(x => x.AllTaxisHaveNitro, f => f.Random.Bool())
                .RuleFor(x => x.ProstiutesPayYou, f => f.Random.Bool());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void Serialization(DataFormat format)
        {
            SimpleVariables x0 = GenerateTestObject(format);
            SimpleVariables x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.VersionId, x1.VersionId);
            Assert.Equal(x0.LastMissionPassedName, x1.LastMissionPassedName);
            Assert.Equal(x0.MissionPackGame, x1.MissionPackGame);
            Assert.Equal(x0.CurrLevel, x1.CurrLevel);
            Assert.Equal(x0.CameraPosition, x1.CameraPosition);
            Assert.Equal(x0.MillisecondsPerGameMinute, x1.MillisecondsPerGameMinute);
            Assert.Equal(x0.LastClockTick, x1.LastClockTick);
            Assert.Equal(x0.GameClockMonths, x1.GameClockMonths);
            Assert.Equal(x0.GameClockDays, x1.GameClockDays);
            Assert.Equal(x0.GameClockHours, x1.GameClockHours);
            Assert.Equal(x0.GameClockMinutes, x1.GameClockMinutes);
            Assert.Equal(x0.GameClockDayOfWeek, x1.GameClockDayOfWeek);
            Assert.Equal(x0.StoredGameClockMonths, x1.StoredGameClockMonths);
            Assert.Equal(x0.StoredGameClockDays, x1.StoredGameClockDays);
            Assert.Equal(x0.StoredGameClockHours, x1.StoredGameClockHours);
            Assert.Equal(x0.StoredGameClockMinutes, x1.StoredGameClockMinutes);
            Assert.Equal(x0.ClockHasBeenStored, x1.ClockHasBeenStored);
            Assert.Equal(x0.CurrPadMode, x1.CurrPadMode);
            Assert.Equal(x0.HasPlayerCheated, x1.HasPlayerCheated);
            Assert.Equal(x0.TimeInMilliseconds, x1.TimeInMilliseconds);
            Assert.Equal(x0.TimeScale, x1.TimeScale);
            Assert.Equal(x0.TimeStep, x1.TimeStep);
            Assert.Equal(x0.TimeStepNonClipped, x1.TimeStepNonClipped);
            Assert.Equal(x0.FrameCounter, x1.FrameCounter);
            Assert.Equal(x0.OldWeatherType, x1.OldWeatherType);
            Assert.Equal(x0.NewWeatherType, x1.NewWeatherType);
            Assert.Equal(x0.ForcedWeatherType, x1.ForcedWeatherType);
            Assert.Equal(x0.WeatherInterpolation, x1.WeatherInterpolation);
            Assert.Equal(x0.WeatherTypeInList, x1.WeatherTypeInList);
            Assert.Equal(x0.Rain, x1.Rain);
            Assert.Equal(x0.CameraCarZoomIndicator, x1.CameraCarZoomIndicator);
            Assert.Equal(x0.CameraPedZoomIndicator, x1.CameraPedZoomIndicator);
            Assert.Equal(x0.CurrArea, x1.CurrArea);
            Assert.Equal(x0.InvertLook4Pad, x1.InvertLook4Pad);
            Assert.Equal(x0.ExtraColour, x1.ExtraColour);
            Assert.Equal(x0.ExtraColourOn, x1.ExtraColourOn);
            Assert.Equal(x0.ExtraColourInterpolation, x1.ExtraColourInterpolation);
            Assert.Equal(x0.ExtraColourWeatherType, x1.ExtraColourWeatherType);
            Assert.Equal(x0.WaterConfiguration, x1.WaterConfiguration);
            Assert.Equal(x0.LARiots, x1.LARiots);
            Assert.Equal(x0.LARiotsNoPoliceCars, x1.LARiotsNoPoliceCars);
            Assert.Equal(x0.MaximumWantedLevel, x1.MaximumWantedLevel);
            Assert.Equal(x0.MaximumChaosLevel, x1.MaximumChaosLevel);
            Assert.Equal(x0.GermanGame, x1.GermanGame);
            Assert.Equal(x0.FrenchGame, x1.FrenchGame);
            Assert.Equal(x0.NastyGame, x1.NastyGame);
            Assert.Equal(x0.CinematicCamMessagesLeftToDisplay, x1.CinematicCamMessagesLeftToDisplay);
            Assert.Equal(x0.TimeLastSaved, x1.TimeLastSaved);
            Assert.Equal(x0.TargetMarkerHandle, x1.TargetMarkerHandle);
            Assert.Equal(x0.HasDisplayedPlayerQuitEnterCarHelpText, x1.HasDisplayedPlayerQuitEnterCarHelpText);
            Assert.Equal(x0.AllTaxisHaveNitro, x1.AllTaxisHaveNitro);
            Assert.Equal(x0.ProstiutesPayYou, x1.ProstiutesPayYou);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(format), data.Length);
        }
    }
}
