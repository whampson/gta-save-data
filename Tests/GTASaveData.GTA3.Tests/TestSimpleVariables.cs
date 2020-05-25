using Bogus;
using TestFramework;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3.Tests
{
    public class TestSimpleVariables : Base<SimpleVariables>
    {
        public override SimpleVariables GenerateTestObject(SaveDataFormat format)
        {
            Faker<SimpleVariables> model = new Faker<SimpleVariables>()
                .RuleFor(x => x.SaveName, f => Generator.Words(f, SimpleVariables.Limits.MaxNameLength - 1))
                .RuleFor(x => x.TimeLastSaved, f => Generator.Date(f))
                .RuleFor(x => x.SaveSize, f => f.Random.Int())
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
                .RuleFor(x => x.CompileDateAndTime, f => Generator.Date(f))
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.CameraCarZoomIndicator, f => f.Random.Float())
                .RuleFor(x => x.CameraPedZoomIndicator, f => f.Random.Float());

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(SaveDataFormat format)
        {
            SimpleVariables x0 = GenerateTestObject(format);
            SimpleVariables x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.SaveName, x1.SaveName);
            Assert.Equal(x0.TimeLastSaved, x1.TimeLastSaved);
            Assert.Equal(x0.SaveSize, x1.SaveSize);
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
            Assert.Equal(x0.CompileDateAndTime, x1.CompileDateAndTime);
            Assert.Equal(x0.WeatherTypeInList, x1.WeatherTypeInList);
            Assert.Equal(x0.CameraCarZoomIndicator, x1.CameraCarZoomIndicator);
            Assert.Equal(x0.CameraPedZoomIndicator, x1.CameraPedZoomIndicator);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(format), data.Length);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
