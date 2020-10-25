using Bogus;
using System;
using TestFramework;
using Xunit;

namespace GTASaveData.VCS.Tests
{
    public class TestSimpleVariables : Base<SimpleVariables>
    {
        public override SimpleVariables GenerateTestObject(FileFormat format)
        {
            Faker<SimpleVariables> model = new Faker<SimpleVariables>()
                .RuleFor(x => x.CurrentLevel, f => f.Random.Int())
                .RuleFor(x => x.CurrentArea, f => f.Random.Int())
                .RuleFor(x => x.Language, f => f.Random.Int())
                .RuleFor(x => x.MillisecondsPerGameMinute, f => f.Random.Int())
                .RuleFor(x => x.LastClockTick, f => f.Random.UInt())
                .RuleFor(x => x.GameClockHours, f => f.Random.Byte())
                .RuleFor(x => x.GameClockMinutes, f => f.Random.Byte())
                .RuleFor(x => x.GameClockSeconds, f => f.Random.Short())
                .RuleFor(x => x.TimeInMilliseconds, f => f.Random.UInt())
                .RuleFor(x => x.TimeScale, f => f.Random.Float())
                .RuleFor(x => x.TimeStep, f => f.Random.Float())
                .RuleFor(x => x.TimeStepNonClipped, f => f.Random.Float())
                .RuleFor(x => x.FramesPerUpdate, f => f.Random.Float())
                .RuleFor(x => x.FrameCounter, f => f.Random.UInt())
                .RuleFor(x => x.OldWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.NewWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.ForcedWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.WeatherInterpolationValue, f => f.Random.Float())
                .RuleFor(x => x.CameraPosition, f => Generator.Vector3(f))
                .RuleFor(x => x.CameraModeInCar, f => f.Random.Float())
                .RuleFor(x => x.CameraModeOnFoot, f => f.Random.Float())
                .RuleFor(x => x.ExtraColor, f => f.Random.Int())
                .RuleFor(x => x.IsExtraColorOn, f => f.Random.Bool())
                .RuleFor(x => x.ExtraColorInterpolation, f => f.Random.Float())
                .RuleFor(x => x.Brightness, f => f.Random.Int())
                .RuleFor(x => x.DisplayHud, f => f.Random.Bool())
                .RuleFor(x => x.ShowSubtitles, f => f.Random.Bool())
                .RuleFor(x => x.RadarMode, f => f.PickRandom<RadarMode>())
                .RuleFor(x => x.BlurOn, f => f.Random.Bool())
                .RuleFor(x => x.UseWideScreen, f => (format.IsPS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.MusicVolume, f => f.Random.Int())
                .RuleFor(x => x.SfxVolume, f => f.Random.Int())
                .RuleFor(x => x.RadioStation, f => f.PickRandom<RadioStation>())
                .RuleFor(x => x.StereoOutput, f => f.Random.Bool())
                .RuleFor(x => x.PadMode, f => f.Random.Short())
                .RuleFor(x => x.InvertLook, f => f.Random.Bool())
                .RuleFor(x => x.UseVibration, f => (format.IsPS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.SwapNippleAndDPad, f => (!format.IsPS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.HasPlayerCheated, f => f.Random.Bool())
                .RuleFor(x => x.AllTaxisHaveNitro, f => f.Random.Bool())
                .RuleFor(x => x.TargetIsOn, f => f.Random.Bool())
                .RuleFor(x => x.TargetPosition, f => Generator.Vector2(f))
                .RuleFor(x => x.PlayerPosition, f => Generator.Vector3(f))
                .RuleFor(x => x.TrailsOn, f => (format.IsPS2) ? f.Random.Bool() : false)
                .RuleFor(x => x.TimeStamp, f => (format.IsPS2) ? new Date(Generator.Date(f)) : Date.MinValue)
                .RuleFor(x => x.Unknown78hPS2, f => (format.IsPS2) ? f.Random.Int() : 0)
                .RuleFor(x => x.Unknown7ChPS2, f => (format.IsPS2) ? f.Random.Int() : 0)
                .RuleFor(x => x.Unknown90hPS2, f => (format.IsPS2) ? f.Random.Int() : 0)
                .RuleFor(x => x.UnknownB8hPSP, f => (format.IsPSP) ? f.Random.Int() : 0)
                .RuleFor(x => x.UnknownD8hPS2, f => (format.IsPS2) ? f.Random.Byte() : (byte) 0)
                .RuleFor(x => x.UnknownD9hPS2, f => (format.IsPS2) ? f.Random.Byte() : (byte) 0);

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            SimpleVariables x0 = GenerateTestObject(format);
            SimpleVariables x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.CurrentLevel, x1.CurrentLevel);
            Assert.Equal(x0.CurrentArea, x1.CurrentArea);
            Assert.Equal(x0.Language, x1.Language);
            Assert.Equal(x0.MillisecondsPerGameMinute, x1.MillisecondsPerGameMinute);
            Assert.Equal(x0.LastClockTick, x1.LastClockTick);
            Assert.Equal(x0.GameClockHours, x1.GameClockHours);
            Assert.Equal(x0.GameClockMinutes, x1.GameClockMinutes);
            Assert.Equal(x0.GameClockSeconds, x1.GameClockSeconds);
            Assert.Equal(x0.TimeInMilliseconds, x1.TimeInMilliseconds);
            Assert.Equal(x0.TimeScale, x1.TimeScale);
            Assert.Equal(x0.TimeStep, x1.TimeStep);
            Assert.Equal(x0.TimeStepNonClipped, x1.TimeStepNonClipped);
            Assert.Equal(x0.FramesPerUpdate, x1.FramesPerUpdate);
            Assert.Equal(x0.FrameCounter, x1.FrameCounter);
            Assert.Equal(x0.OldWeatherType, x1.OldWeatherType);
            Assert.Equal(x0.NewWeatherType, x1.NewWeatherType);
            Assert.Equal(x0.ForcedWeatherType, x1.ForcedWeatherType);
            Assert.Equal(x0.WeatherTypeInList, x1.WeatherTypeInList);
            Assert.Equal(x0.WeatherInterpolationValue, x1.WeatherInterpolationValue);
            Assert.Equal(x0.CameraPosition, x1.CameraPosition);
            Assert.Equal(x0.CameraModeInCar, x1.CameraModeInCar);
            Assert.Equal(x0.CameraModeOnFoot, x1.CameraModeOnFoot);
            Assert.Equal(x0.ExtraColor, x1.ExtraColor);
            Assert.Equal(x0.IsExtraColorOn, x1.IsExtraColorOn);
            Assert.Equal(x0.ExtraColorInterpolation, x1.ExtraColorInterpolation);
            Assert.Equal(x0.Brightness, x1.Brightness);
            Assert.Equal(x0.DisplayHud, x1.DisplayHud);
            Assert.Equal(x0.ShowSubtitles, x1.ShowSubtitles);
            Assert.Equal(x0.RadarMode, x1.RadarMode);
            Assert.Equal(x0.BlurOn, x1.BlurOn);
            Assert.Equal(x0.UseWideScreen, x1.UseWideScreen);
            Assert.Equal(x0.MusicVolume, x1.MusicVolume);
            Assert.Equal(x0.SfxVolume, x1.SfxVolume);
            Assert.Equal(x0.RadioStation, x1.RadioStation);
            Assert.Equal(x0.StereoOutput, x1.StereoOutput);
            Assert.Equal(x0.PadMode, x1.PadMode);
            Assert.Equal(x0.InvertLook, x1.InvertLook);
            Assert.Equal(x0.UseVibration, x1.UseVibration);
            Assert.Equal(x0.SwapNippleAndDPad, x1.SwapNippleAndDPad);
            Assert.Equal(x0.HasPlayerCheated, x1.HasPlayerCheated);
            Assert.Equal(x0.AllTaxisHaveNitro, x1.AllTaxisHaveNitro);
            Assert.Equal(x0.TargetIsOn, x1.TargetIsOn);
            Assert.Equal(x0.TargetPosition, x1.TargetPosition);
            Assert.Equal(x0.PlayerPosition, x1.PlayerPosition);
            Assert.Equal(x0.TrailsOn, x1.TrailsOn);
            Assert.Equal(x0.TimeStamp, x1.TimeStamp);
            Assert.Equal(x0.Unknown78hPS2, x1.Unknown78hPS2);
            Assert.Equal(x0.Unknown7ChPS2, x1.Unknown7ChPS2);
            Assert.Equal(x0.Unknown90hPS2, x1.Unknown90hPS2);
            Assert.Equal(x0.UnknownB8hPSP, x1.UnknownB8hPSP);
            Assert.Equal(x0.UnknownD8hPS2, x1.UnknownD8hPS2);
            Assert.Equal(x0.UnknownD9hPS2, x1.UnknownD9hPS2);

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
