using Bogus;
using GTASaveData.Common;
using GTASaveData.Core.Tests.Common;
using GTASaveData.GTA3;
using GTASaveData.Serialization;
using System.Collections.Generic;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestSimpleVars : SerializableObjectTestBase<SimpleVars>
    {
        public override SimpleVars GenerateTestVector(FileFormat format)
        {
            Faker<SimpleVars> model = new Faker<SimpleVars>()
                .RuleFor(x => x.LastMissionPassedName, f => !format.SupportsPS2 ? Generator.RandomWords(f, 23) : string.Empty)
                .RuleFor(x => x.SaveTime, (format.SupportsPC || format.SupportsXbox) ? Generator.Generate<SystemTime, TestSystemTime>() : new SystemTime())
                .RuleFor(x => x.SizeOfGameInBytes, format.IsSupported(ConsoleType.PS2, ConsoleFlags.Japan) ? 0x31400 : 0x31401)
                .RuleFor(x => x.CurrLevel, f => f.PickRandom<Level>())
                .RuleFor(x => x.CameraPosition, f => Generator.Generate<Vector3d, TestVector3d>())
                .RuleFor(x => x.MillisecondsPerGameMinute, f => f.Random.Int())
                .RuleFor(x => x.LastClockTick, f => f.Random.UInt())
                .RuleFor(x => x.GameClockHours, f => f.Random.Byte())
                .RuleFor(x => x.GameClockMinutes, f => f.Random.Byte())
                .RuleFor(x => x.TimeInMilliseconds, f => f.Random.UInt())
                .RuleFor(x => x.TimeScale, f => f.Random.Float())
                .RuleFor(x => x.TimeStep, f => f.Random.Float())
                .RuleFor(x => x.TimeStepNonClipped, f => f.Random.Float())
                .RuleFor(x => x.FrameCounter, f => f.Random.Int())
                .RuleFor(x => x.TimeStep2, f => f.Random.Float())
                .RuleFor(x => x.FramesPerUpdate, f => f.Random.Float())
                .RuleFor(x => x.TimeScale2, f => f.Random.Float())
                .RuleFor(x => x.OldWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.NewWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.ForcedWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.InterpolationValue, f => f.Random.Float())
                .RuleFor(x => x.PrefsMusicVolume, f => format.SupportsPS2 ? f.Random.Int() : 0)
                .RuleFor(x => x.PrefsSfxVolume, f => format.SupportsPS2 ? f.Random.Int() : 0)
                .RuleFor(x => x.PrefsUseVibration, f => format.SupportsPS2 ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsStereoMono, f => format.SupportsPS2 ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsRadioStation, f => format.SupportsPS2 ? f.PickRandom<RadioStation>() : 0)
                .RuleFor(x => x.PrefsBrightness, f => format.SupportsPS2 ? f.Random.Int() : 0)
                .RuleFor(x => x.PrefsShowSubtitles, f => format.SupportsPS2 ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsLanguage, f => format.SupportsPS2 ? f.PickRandom<Language>() : 0)
                .RuleFor(x => x.PrefsUseWideScreen, f => format.SupportsPS2 ? f.Random.Bool() : false)
                .RuleFor(x => x.PrefsControllerConfig, f => format.SupportsPS2 ? f.Random.Int() : 0)
                .RuleFor(x => x.PrefsShowTrails, f => format.SupportsPS2 ? f.Random.Bool() : false)
                .RuleFor(x => x.CompileDateAndTime, Generator.Generate<Timestamp, TestTimestamp>())
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.InCarCameraMode, f => f.Random.Float())
                .RuleFor(x => x.OnFootCameraMode, f => f.Random.Float())
                .RuleFor(x => x.IsQuickSave, f => format.SupportsMobile ? f.Random.Int() : 0);

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(SerializationData))]
        public void Serialization(FileFormat format, int expectedSize)
        {
            SimpleVars x0 = GenerateTestVector(format);
            SimpleVars x1 = CreateSerializedCopy(x0, out byte[] data, format);

            Assert.Equal(x0.LastMissionPassedName, x1.LastMissionPassedName);
            Assert.Equal(x0.SaveTime, x1.SaveTime);
            Assert.Equal(x0.CurrLevel, x1.CurrLevel);
            Assert.Equal(x0.CameraPosition, x1.CameraPosition);
            Assert.Equal(x0.MillisecondsPerGameMinute, x1.MillisecondsPerGameMinute);
            Assert.Equal(x0.LastClockTick, x1.LastClockTick);
            Assert.Equal(x0.GameClockHours, x1.GameClockHours);
            Assert.Equal(x0.GameClockMinutes, x1.GameClockMinutes);
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
            Assert.Equal(x0.InterpolationValue, x1.InterpolationValue);
            Assert.Equal(x0.PrefsMusicVolume, x1.PrefsMusicVolume);
            Assert.Equal(x0.PrefsSfxVolume, x1.PrefsSfxVolume);
            Assert.Equal(x0.PrefsUseVibration, x1.PrefsUseVibration);
            Assert.Equal(x0.PrefsStereoMono, x1.PrefsStereoMono);
            Assert.Equal(x0.PrefsRadioStation, x1.PrefsRadioStation);
            Assert.Equal(x0.PrefsBrightness, x1.PrefsBrightness);
            Assert.Equal(x0.PrefsShowSubtitles, x1.PrefsShowSubtitles);
            Assert.Equal(x0.PrefsLanguage, x1.PrefsLanguage);
            Assert.Equal(x0.PrefsUseWideScreen, x1.PrefsUseWideScreen);
            Assert.Equal(x0.PrefsControllerConfig, x1.PrefsControllerConfig);
            Assert.Equal(x0.PrefsShowTrails, x1.PrefsShowTrails);
            Assert.Equal(x0.CompileDateAndTime, x1.CompileDateAndTime);
            Assert.Equal(x0.WeatherTypeInList, x1.WeatherTypeInList);
            Assert.Equal(x0.InCarCameraMode, x1.InCarCameraMode);
            Assert.Equal(x0.OnFootCameraMode, x1.OnFootCameraMode);
            Assert.Equal(x0.IsQuickSave, x1.IsQuickSave);
            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }

        public static IEnumerable<object[]> SerializationData => new[]
        {
            new object[] { GTA3Save.FileFormats.Android, 0xB0 },
            new object[] { GTA3Save.FileFormats.IOS, 0xB0 },
            new object[] { GTA3Save.FileFormats.PC, 0xBC },
            new object[] { GTA3Save.FileFormats.PS2NAEU, 0xB0 },
            new object[] { GTA3Save.FileFormats.PS2AU, 0xA8 },
            new object[] { GTA3Save.FileFormats.PS2JP, 0xB0 },
            new object[] { GTA3Save.FileFormats.Xbox, 0xBC },
        };
    }
}
