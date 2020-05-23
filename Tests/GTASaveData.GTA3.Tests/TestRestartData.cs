using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestRestartData : Base<RestartData>
    {
        public override RestartData GenerateTestObject(DataFormat format)
        {
            Faker<RestartData> model = new Faker<RestartData>()
                .RuleFor(x => x.WastedRestartPoints, Generator.Array(RestartData.Limits.MaxNumWastedRestarts, g => Generator.Generate<RestartPoint, TestRestartPoint>()))
                .RuleFor(x => x.BustedRestartPoints, Generator.Array(RestartData.Limits.MaxNumBustedRestarts, g => Generator.Generate<RestartPoint, TestRestartPoint>()))
                .RuleFor(x => x.NumberOfWastedRestartPoints, f => f.Random.Short())
                .RuleFor(x => x.NumberOfBustedRestartPoints, f => f.Random.Short())
                .RuleFor(x => x.OverrideNextRestart, f => f.Random.Bool())
                .RuleFor(x => x.OverrideRestartPoint, f => Generator.Generate<RestartPoint, TestRestartPoint>())
                .RuleFor(x => x.FadeInAfteNextDeath, f => f.Random.Bool())
                .RuleFor(x => x.FadeInAfteNextArrest, f => f.Random.Bool())
                .RuleFor(x => x.OverrideHospitalLevel, f => f.PickRandom<Level>())
                .RuleFor(x => x.OverridePoliceStationLevel, f => f.PickRandom<Level>());

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            RestartData x0 = GenerateTestObject();
            RestartData x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.WastedRestartPoints, x1.WastedRestartPoints);
            Assert.Equal(x0.BustedRestartPoints, x1.BustedRestartPoints);
            Assert.Equal(x0.NumberOfWastedRestartPoints, x1.NumberOfWastedRestartPoints);
            Assert.Equal(x0.NumberOfBustedRestartPoints, x1.NumberOfBustedRestartPoints);
            Assert.Equal(x0.OverrideNextRestart, x1.OverrideNextRestart);
            Assert.Equal(x0.OverrideRestartPoint, x1.OverrideRestartPoint);
            Assert.Equal(x0.FadeInAfteNextDeath, x1.FadeInAfteNextDeath);
            Assert.Equal(x0.FadeInAfteNextArrest, x1.FadeInAfteNextArrest);
            Assert.Equal(x0.OverrideHospitalLevel, x1.OverrideHospitalLevel);
            Assert.Equal(x0.OverridePoliceStationLevel, x1.OverridePoliceStationLevel);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
