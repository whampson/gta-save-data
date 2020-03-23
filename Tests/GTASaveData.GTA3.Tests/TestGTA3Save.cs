using Bogus;
using GTASaveData.Core.Tests.Types;
using GTASaveData.GTA3;
using GTASaveData.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using TestFramework;
using Xunit;

namespace GTASaveData.Tests.GTA3
{
    public class TestGTA3Save : SerializableObjectTestBase<GTA3Save>
    {
        public override GTA3Save GenerateTestObject(SaveFileFormat format)
        {
            Faker faker = new Faker();

            Faker<Game> gameFaker = new Faker<Game>()
                .RuleFor(x => x.CurrLevel, f => f.PickRandom<LevelType>());

            Faker<Camera> cameraFaker = new Faker<Camera>()
                .RuleFor(x => x.Position, f => Generator.Generate<Vector, TestVector>())
                .RuleFor(x => x.CarZoomIndicator, f => f.Random.Float())
                .RuleFor(x => x.PedZoomIndicator, f => f.Random.Float());

            Faker<Clock> clockFaker = new Faker<Clock>()
                .RuleFor(x => x.MillisecondsPerGameMinute, f => f.Random.Int())
                .RuleFor(x => x.LastClockTick, f => f.Random.UInt())
                .RuleFor(x => x.GameClockHours, f => f.Random.Byte())
                .RuleFor(x => x.GameClockMinutes, f => f.Random.Byte());

            Faker<Timer> timerFaker = new Faker<Timer>()
                .RuleFor(x => x.TimeInMilliseconds, f => f.Random.UInt())
                .RuleFor(x => x.TimeScale, f => f.Random.Float())
                .RuleFor(x => x.TimeStep, f => f.Random.Float())
                .RuleFor(x => x.TimeStepNonClipped, f => f.Random.Float())
                .RuleFor(x => x.FrameCounter, f => f.Random.UInt());

            Faker<TimeStep> timeStepFaker = new Faker<TimeStep>()
                .RuleFor(x => x.TimeStepValue, f => f.Random.Float())
                .RuleFor(x => x.FramesPerUpdate, f => f.Random.Float())
                .RuleFor(x => x.TimeScale, f => f.Random.Float());

            Faker<Weather> weatherFaker = new Faker<Weather>()
                .RuleFor(x => x.OldWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.NewWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.ForcedWeatherType, f => f.PickRandom<WeatherType>())
                .RuleFor(x => x.InterpolationValue, f => f.Random.Float())
                .RuleFor(x => x.WeatherTypeInList, f => f.Random.Int());

            Faker<GTA3Save> fileModel = new Faker<GTA3Save>()
                .RuleFor(x => x.FileFormat, format)
                .RuleFor(x => x.Name, f => !format.SupportedOnPS2 ? Generator.RandomWords(f, GTA3Save.Limits.MaxNameLength - 1) : string.Empty)
                .RuleFor(x => x.TimeLastSaved, (format.SupportedOnPC || format.SupportedOnXbox) ? Generator.Generate<SystemTime, TestSystemTime>().ToDateTime() : new DateTime())
                .RuleFor(x => x.SaveSize, format.IsSupportedOn(ConsoleType.PS2, ConsoleFlags.Japan) ? 201728 : 201729)
                .RuleFor(x => x.Game, gameFaker.Generate())
                .RuleFor(x => x.TheCamera, cameraFaker.Generate())
                .RuleFor(x => x.Clock, clockFaker.Generate())
                .RuleFor(x => x.Timer, timerFaker.Generate())
                .RuleFor(x => x.TimeStep, timeStepFaker.Generate())
                .RuleFor(x => x.Weather, weatherFaker.Generate())
                //.RuleFor(x => x.PrefsMusicVolume, f => format.SupportedOnPS2 ? f.Random.Int() : 0)
                //.RuleFor(x => x.PrefsSfxVolume, f => format.SupportedOnPS2 ? f.Random.Int() : 0)
                //.RuleFor(x => x.PrefsUseVibration, f => format.SupportedOnPS2 ? f.Random.Bool() : false)
                //.RuleFor(x => x.PrefsStereoMono, f => format.SupportedOnPS2 ? f.Random.Bool() : false)
                //.RuleFor(x => x.PrefsRadioStation, f => format.SupportedOnPS2 ? f.PickRandom<RadioStation>() : 0)
                //.RuleFor(x => x.PrefsBrightness, f => format.SupportedOnPS2 ? f.Random.Int() : 0)
                //.RuleFor(x => x.PrefsShowSubtitles, f => format.SupportedOnPS2 ? f.Random.Bool() : false)
                //.RuleFor(x => x.PrefsLanguage, f => format.SupportedOnPS2 ? f.PickRandom<Language>() : 0)
                //.RuleFor(x => x.PrefsUseWideScreen, f => format.SupportedOnPS2 ? f.Random.Bool() : false)
                //.RuleFor(x => x.PrefsControllerConfig, f => format.SupportedOnPS2 ? f.Random.Int() : 0)
                //.RuleFor(x => x.PrefsShowTrails, f => format.SupportedOnPS2 ? f.Random.Bool() : false)
                .RuleFor(x => x.CompileDateAndTime, Generator.Generate<Date, TestDate>())
                //.RuleFor(x => x.IsQuickSave, f => format.SupportedOnMobile ? f.Random.Int() : 0);
                .RuleFor(x => x.Scripts, Generator.Generate<TheScripts, TestTheScripts>(format))
                //.RuleFor(x => x.PedPool, TestHelper.Generate<PedPool, TestPedPool>(format))
                //.RuleFor(x => x.Garages, Generator.Generate<Garages, TestGarages>(format))
                //.RuleFor(x => x.VehiclePool, Generator.Generate<VehiclePool, TestVehiclePool>(format))
                //.RuleFor(x => x.ObjectPool, Generator.Generate<ObjectPool, TestObjectPool>(format))
                //.RuleFor(x => x.PathFind, Generator.Generate<PathFind, TestPathFind>(format))
                //.RuleFor(x => x.Cranes, Generator.Generate<Cranes, TestCranes>(format))
                //.RuleFor(x => x.Pickups, Generator.Generate<Pickups, TestPickups>(format))
                //.RuleFor(x => x.PhoneInfo, Generator.Generate<PhoneInfo, TestPhoneInfo>(format))
                //.RuleFor(x => x.RestartPoints, Generator.Generate<RestartPoints, TestRestartPoints>(format))
                //.RuleFor(x => x.RadarBlips, Generator.Generate<RadarBlips, TestRadarBlips>(format))
                //.RuleFor(x => x.Zones, Generator.Generate<Zones, TestZones>(format))
                //.RuleFor(x => x.GangData, Generator.Generate<GangData, TestGangData>(format))
                //.RuleFor(x => x.CarGenerators, Generator.Generate<TheCarGenerators, TestTheCarGenerators>(format))
                //.RuleFor(x => x.ParticleObjects, Generator.Generate<Particles, TestParticles>(format))
                //.RuleFor(x => x.AudioScriptObjects, Generator.Generate<AudioScriptObjects, TestAudioScriptObjects>(format))
                //.RuleFor(x => x.PlayerInfo, Generator.Generate<PlayerInfo, TestPlayerInfo>(format))
                //.RuleFor(x => x.Stats, Generator.Generate<Stats, TestStats>(format))
                //.RuleFor(x => x.Streaming, Generator.Generate<Streaming, TestStreaming>(format))
                //.RuleFor(x => x.PedTypeInfo, Generator.Generate<PedTypeInfo, TestPedTypeInfo>(format))
                ;

            return fileModel.Generate();
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void FileFormatDetection(SaveFileFormat expected, string filename)
        {
            string path = TestData.GetTestDataPath(GameType.III, expected, filename);
            SaveFile.GetFileFormat<GTA3Save>(path, out SaveFileFormat detected);

            Assert.Equal(expected, detected);
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(SaveFileFormat format)
        {
            GTA3Save x0 = GenerateTestObject(format);
            GTA3Save x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(_ => _);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void RealDataSerialization(SaveFileFormat format, string filename)
        {
            string path = TestData.GetTestDataPath(GameType.III, format, filename);

            GTA3Save x0 = SaveFile.Load<GTA3Save>(path);
            GTA3Save x1 = CreateSerializedCopy(x0, format, out byte[] data);

            AssertEqual(x0, x1);

            int calculatedSum = data.Take(data.Length - 4).Sum(_ => _);
            int storedSum = BitConverter.ToInt32(data, data.Length - 4);
            Assert.Equal(calculatedSum, storedSum);
        }

        private void AssertEqual(GTA3Save x0, GTA3Save x1)
        {
            // TODO: add PS2 stuff
            Assert.Equal(x0.Name, x1.Name);
            Assert.Equal(x0.TimeLastSaved, x1.TimeLastSaved);
            Assert.Equal(x0.SaveSize, x1.SaveSize);
            Assert.Equal(x0.Game.CurrLevel, x1.Game.CurrLevel);
            Assert.Equal(x0.TheCamera.Position, x1.TheCamera.Position);
            Assert.Equal(x0.Clock.MillisecondsPerGameMinute, x1.Clock.MillisecondsPerGameMinute);
            Assert.Equal(x0.Clock.LastClockTick, x1.Clock.LastClockTick);
            Assert.Equal(x0.Clock.GameClockHours, x1.Clock.GameClockHours);
            Assert.Equal(x0.Clock.GameClockMinutes, x1.Clock.GameClockMinutes);
            Assert.Equal(x0.Timer.TimeInMilliseconds, x1.Timer.TimeInMilliseconds);
            Assert.Equal(x0.Timer.TimeScale, x1.Timer.TimeScale);
            Assert.Equal(x0.Timer.TimeStep, x1.Timer.TimeStep);
            Assert.Equal(x0.Timer.TimeStepNonClipped, x1.Timer.TimeStepNonClipped);
            Assert.Equal(x0.Timer.FrameCounter, x1.Timer.FrameCounter);
            Assert.Equal(x0.TimeStep.TimeStepValue, x1.TimeStep.TimeStepValue);
            Assert.Equal(x0.TimeStep.FramesPerUpdate, x1.TimeStep.FramesPerUpdate);
            Assert.Equal(x0.TimeStep.TimeScale, x1.TimeStep.TimeScale);
            Assert.Equal(x0.Weather.OldWeatherType, x1.Weather.OldWeatherType);
            Assert.Equal(x0.Weather.NewWeatherType, x1.Weather.NewWeatherType);
            Assert.Equal(x0.Weather.ForcedWeatherType, x1.Weather.ForcedWeatherType);
            Assert.Equal(x0.Weather.InterpolationValue, x1.Weather.InterpolationValue);
            Assert.Equal(x0.CompileDateAndTime, x1.CompileDateAndTime);
            Assert.Equal(x0.Weather.WeatherTypeInList, x1.Weather.WeatherTypeInList);
            Assert.Equal(x0.TheCamera.CarZoomIndicator, x1.TheCamera.CarZoomIndicator);
            Assert.Equal(x0.TheCamera.PedZoomIndicator, x1.TheCamera.PedZoomIndicator);
            Assert.Equal(x0.Scripts, x1.Scripts);
            Assert.Equal(x0.PedPool, x1.PedPool);
            Assert.Equal(x0.Garages, x1.Garages);
            Assert.Equal(x0.VehiclePool, x1.VehiclePool);
            Assert.Equal(x0.ObjectPool, x1.ObjectPool);
            Assert.Equal(x0.Paths, x1.Paths);
            Assert.Equal(x0.Cranes, x1.Cranes);
            Assert.Equal(x0.Pickups, x1.Pickups);
            Assert.Equal(x0.PhoneInfo, x1.PhoneInfo);
            Assert.Equal(x0.RestartPoints, x1.RestartPoints);
            Assert.Equal(x0.RadarBlips, x1.RadarBlips);
            Assert.Equal(x0.Zones, x1.Zones);
            Assert.Equal(x0.GangData, x1.GangData);
            Assert.Equal(x0.CarGenerators, x1.CarGenerators);
            Assert.Equal(x0.ParticleObjects, x1.ParticleObjects);
            Assert.Equal(x0.AudioScriptObjects, x1.AudioScriptObjects);
            Assert.Equal(x0.PlayerInfo, x1.PlayerInfo);
            Assert.Equal(x0.Stats, x1.Stats);
            Assert.Equal(x0.Streaming, x1.Streaming);
            Assert.Equal(x0.PedTypeInfo, x1.PedTypeInfo);
            Assert.Equal(x0, x1);
        }

        public static IEnumerable<object[]> TestFiles => new[]
        {
            new object[] { GTA3Save.FileFormats.PC, "1_JM4" },
            new object[] { GTA3Save.FileFormats.PC, "2_AS3" },
            //new object[] { GTA3Save.FileFormats.PS2_AU, "1_T4X4_2" },
            //new object[] { GTA3Save.FileFormats.PS2_AU, "2_AS3" },
            //new object[] { GTA3Save.FileFormats.PS2_AU, "3_CAT2" },
            //new object[] { GTA3Save.FileFormats.PS2_JP, "1_LM1" },
            //new object[] { GTA3Save.FileFormats.PS2_JP, "2_LM2" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "1_T4X4_1" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "2_LM1" },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU, "3_CAT2" },
        };

        public static IEnumerable<object[]> FileFormats => new[]
        {
            //new object[] { GTA3Save.FileFormats.Android },
            //new object[] { GTA3Save.FileFormats.iOS },
            new object[] { GTA3Save.FileFormats.PC },
            //new object[] { GTA3Save.FileFormats.PS2_AU },
            //new object[] { GTA3Save.FileFormats.PS2_JP },
            //new object[] { GTA3Save.FileFormats.PS2_NAEU },
            //new object[] { GTA3Save.FileFormats.Xbox },
        };
    }
}
