using Bogus;
using GTASaveData.Common;
using GTASaveData.Core.Tests.Common;
using GTASaveData.VC;
using GTASaveData.Serialization;
using System.Collections.Generic;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.VC
{
    public class TestSimpleVars : SerializableObjectTestBase<SimpleVars>
    {
        public override SimpleVars GenerateTestVector(FileFormat format)
        {
            Faker<SimpleVars> model = new Faker<SimpleVars>()
                .RuleFor(x => x.LastMissionPassedName, f => !format.IsPS2 ? Generator.RandomWords(f, 23) : string.Empty)
                .RuleFor(x => x.SaveTime, (format.IsPC || format.IsXbox) ? Generator.Generate<SystemTime, TestSystemTime>() : new SystemTime())
                .RuleFor(x => x.CurrLevel, f => f.PickRandom<Level>())
                .RuleFor(x => x.UnknownSteamOnly, f => format.HasFlag(ConsoleFlags.Steam) ? f.Random.Int() : 0)
                .RuleFor(x => x.CameraPosition, f => Generator.Generate<Vector3d, TestVector3d>())
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
                //.RuleFor(x => x.PrefsMusicVolume, f => format.IsPS2 ? f.Random.Int() : 0)
                //.RuleFor(x => x.PrefsSfxVolume, f => format.IsPS2 ? f.Random.Int() : 0)
                //.RuleFor(x => x.PrefsUseVibration, f => format.IsPS2 ? f.Random.Bool() : false)
                //.RuleFor(x => x.PrefsStereoMono, f => format.IsPS2 ? f.Random.Bool() : false)
                //.RuleFor(x => x.PrefsRadioStation, f => format.IsPS2 ? f.PickRandom<RadioStation>() : 0)
                //.RuleFor(x => x.PrefsBrightness, f => format.IsPS2 ? f.Random.Int() : 0)
                //.RuleFor(x => x.PrefsShowSubtitles, f => format.IsPS2 ? f.Random.Bool() : false)
                //.RuleFor(x => x.PrefsLanguage, f => format.IsPS2 ? f.PickRandom<Language>() : 0)
                //.RuleFor(x => x.PrefsUseWideScreen, f => format.IsPS2 ? f.Random.Bool() : false)
                //.RuleFor(x => x.PrefsControllerConfig, f => format.IsPS2 ? f.PickRandom<PadMode>() : 0)
                //.RuleFor(x => x.PrefsShowTrails, f => format.IsPS2 ? f.Random.Bool() : false)
                //.RuleFor(x => x.CompileDateAndTime, Generator.Generate<Timestamp, TestTimestamp>())
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int())
                .RuleFor(x => x.InCarCameraMode, f => f.Random.Float())
                .RuleFor(x => x.OnFootCameraMode, f => f.Random.Float())
                //.RuleFor(x => x.IsQuickSave, f => format.IsMobile ? f.Random.Int() : 0);
                .RuleFor(x => x.CurrentInterior, f => f.Random.Int())
                .RuleFor(x => x.TaxiBoost, f => f.Random.Bool())
                .RuleFor(x => x.InvertLook, f => f.Random.Bool())
                .RuleFor(x => x.ExtraColor, f => f.Random.Int())
                .RuleFor(x => x.IsExtraColorOn, f => f.Random.Bool())
                .RuleFor(x => x.ExtraColorInterpolation, f => f.Random.Float())
                .RuleFor(x => x.RadioListenTime, f => Generator.CreateArray(SimpleVars.Limits.RadioListenTimeCount, g => f.Random.UInt()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(SerializationData))]
        public void Serialization(FileFormat format, int expectedSize)
        {
            SimpleVars x0 = GenerateTestVector(format);
            SimpleVars x1 = CreateSerializedCopy(x0, out byte[] data, format);

            Assert.Equal(x0, x1);
            Assert.Equal(expectedSize, data.Length);
        }

        public static IEnumerable<object[]> SerializationData => new[]
        {
            new object[] { ViceCitySave.FileFormats.PCRetail, 0xE4 },
            new object[] { ViceCitySave.FileFormats.PCSteam, 0xE8 }
        };
    }
}
