using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.Types;
using TestFramework;
using Xunit;

namespace GTASaveData.VC.Tests
{
    public class TestSimpleVariables : Base<SimpleVariables>
    {
        public override SimpleVariables GenerateTestObject(SaveFileFormat format)
        {
            Faker<SimpleVariables> model = new Faker<SimpleVariables>()
                .RuleFor(x => x.LastMissionPassedName, f => Generator.RandomUnicodeString(f, SimpleVariables.Limits.MaxNameLength - 1))
                .RuleFor(x => x.TimeLastSaved, f => Generator.Generate<SystemTime, TestSystemTime>())
                .RuleFor(x => x.SaveSize, f => f.Random.Int())
                .RuleFor(x => x.CurrLevel, f => f.PickRandom<LevelType>())
                .RuleFor(x => x.CameraPosition, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.SteamWin32Only, (SimpleVariables.IsSteamWin32(format)) ? SimpleVariables.SteamWin32OnlyValue : 0)
                .RuleFor(x => x.MillisecondsPerGameMinute, f => f.Random.Int())
                .RuleFor(x => x.LastClockTick, f => f.Random.UInt())
                .RuleFor(x => x.GameClockHours, f => f.Random.Byte())
                .RuleFor(x => x.GameClockMinutes, f => f.Random.Byte())
                .RuleFor(x => x.CurrPadMode, f => f.Random.Short())
                .RuleFor(x => x.TimeInMilliseconds, f => f.Random.UInt())
                .RuleFor(x => x.TimerTimeScale, f => f.Random.Float())
                .RuleFor(x => x.TimerTimeStep, f => f.Random.Float())
                .RuleFor(x => x.TimerTimeStepNonClipped, f => f.Random.Float())
                .RuleFor(x => x.FrameCounter, f => f.Random.UInt())
                .RuleFor(x => x.TimeStep, f => f.Random.Float())
                .RuleFor(x => x.FramesPerUpdate, f => f.Random.Float())
                .RuleFor(x => x.TimeScale, f => f.Random.Float())
                .RuleFor(x => x.OldWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.NewWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.ForcedWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.WeatherInterpolation, f => f.Random.Float())
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.CameraCarZoomIndicator, f => f.Random.Float())
                .RuleFor(x => x.CameraPedZoomIndicator, f => f.Random.Float())
                .RuleFor(x => x.CurrArea, f => f.PickRandom<Interior>())
                .RuleFor(x => x.AllTaxisHaveNitro, f => f.Random.Bool())
                .RuleFor(x => x.InvertLook4Pad, f => f.Random.Bool())
                .RuleFor(x => x.ExtraColour, f => f.Random.Int())
                .RuleFor(x => x.ExtraColourOn, f => f.Random.Bool())
                .RuleFor(x => x.ExtraColourInterpolation, f => f.Random.Float())
                .RuleFor(x => x.RadioStationPositionList, f => Generator.CreateArray(SimpleVariables.Limits.RadioStationListCount, g => f.Random.Int()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void Serialization(SaveFileFormat format)
        {
            SimpleVariables x0 = GenerateTestObject(format);
            SimpleVariables x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.LastMissionPassedName, x1.LastMissionPassedName);
            Assert.Equal(x0.TimeLastSaved, x1.TimeLastSaved);
            Assert.Equal(x0.SaveSize, x1.SaveSize);
            Assert.Equal(x0.CurrLevel, x1.CurrLevel);
            Assert.Equal(x0.CameraPosition, x1.CameraPosition);
            Assert.Equal(x0.SteamWin32Only, x1.SteamWin32Only);
            Assert.Equal(x0.MillisecondsPerGameMinute, x1.MillisecondsPerGameMinute);
            Assert.Equal(x0.LastClockTick, x1.LastClockTick);
            Assert.Equal(x0.GameClockHours, x1.GameClockHours);
            Assert.Equal(x0.GameClockMinutes, x1.GameClockMinutes);
            Assert.Equal(x0.CurrPadMode, x1.CurrPadMode);
            Assert.Equal(x0.TimeInMilliseconds, x1.TimeInMilliseconds);
            Assert.Equal(x0.TimerTimeScale, x1.TimerTimeScale);
            Assert.Equal(x0.TimerTimeStep, x1.TimerTimeStep);
            Assert.Equal(x0.TimerTimeStepNonClipped, x1.TimerTimeStepNonClipped);
            Assert.Equal(x0.FrameCounter, x1.FrameCounter);
            Assert.Equal(x0.TimeStep, x1.TimeStep);
            Assert.Equal(x0.FramesPerUpdate, x1.FramesPerUpdate);
            Assert.Equal(x0.TimeScale, x1.TimeScale);
            Assert.Equal(x0.OldWeatherType, x1.OldWeatherType);
            Assert.Equal(x0.NewWeatherType, x1.NewWeatherType);
            Assert.Equal(x0.ForcedWeatherType, x1.ForcedWeatherType);
            Assert.Equal(x0.WeatherInterpolation, x1.WeatherInterpolation);
            Assert.Equal(x0.WeatherTypeInList, x1.WeatherTypeInList);
            Assert.Equal(x0.CameraCarZoomIndicator, x1.CameraCarZoomIndicator);
            Assert.Equal(x0.CameraPedZoomIndicator, x1.CameraPedZoomIndicator);
            Assert.Equal(x0.CurrArea, x1.CurrArea);
            Assert.Equal(x0.AllTaxisHaveNitro, x1.AllTaxisHaveNitro);
            Assert.Equal(x0.InvertLook4Pad, x1.InvertLook4Pad);
            Assert.Equal(x0.ExtraColour, x1.ExtraColour);
            Assert.Equal(x0.ExtraColourOn, x1.ExtraColourOn);
            Assert.Equal(x0.ExtraColourInterpolation, x1.ExtraColourInterpolation);
            Assert.Equal(x0.RadioStationPositionList, x1.RadioStationPositionList);
            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(format), data.Length);
        }
    }
}
