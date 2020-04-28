using Bogus;
using TestFramework;
using Xunit;

namespace GTASaveData.GTA3.Tests
{
    public class TestStats : Base<Stats>
    {
        public override Stats GenerateTestObject(DataFormat format)
        {
            Faker<Stats> model = new Faker<Stats>()
                .RuleFor(x => x.PeopleKilledByPlayer, f => f.Random.Int())
                .RuleFor(x => x.PeopleKilledByOthers, f => f.Random.Int())
                .RuleFor(x => x.CarsExploded, f => f.Random.Int())
                .RuleFor(x => x.RoundsFiredByPlayer, f => f.Random.Int())
                .RuleFor(x => x.PedsKilledOfThisType, f => Generator.Array(PedTypeData.Limits.NumberOfPedTypes, g => f.Random.Int()))
                .RuleFor(x => x.HelisDestroyed, f => f.Random.Int())
                .RuleFor(x => x.ProgressMade, f => f.Random.Int())
                .RuleFor(x => x.TotalProgressInGame, f => f.Random.Int())
                .RuleFor(x => x.KgsOfExplosivesUsed, f => f.Random.Int())
                .RuleFor(x => x.InstantHitsFiredByPlayer, f => f.Random.Int())
                .RuleFor(x => x.InstantHitsHitByPlayer, f => f.Random.Int())
                .RuleFor(x => x.CarsCrushed, f => f.Random.Int())
                .RuleFor(x => x.HeadsPopped, f => f.Random.Int())
                .RuleFor(x => x.TimesArrested, f => f.Random.Int())
                .RuleFor(x => x.TimesDied, f => f.Random.Int())
                .RuleFor(x => x.DaysPassed, f => f.Random.Int())
                .RuleFor(x => x.MmRain, f => f.Random.Int())
                .RuleFor(x => x.MaximumJumpDistance, f => f.Random.Float())
                .RuleFor(x => x.MaximumJumpHeight, f => f.Random.Float())
                .RuleFor(x => x.MaximumJumpFlips, f => f.Random.Int())
                .RuleFor(x => x.MaximumJumpSpins, f => f.Random.Int())
                .RuleFor(x => x.BestStuntJump, f => f.Random.Int())
                .RuleFor(x => x.NumberOfUniqueJumpsFound, f => f.Random.Int())
                .RuleFor(x => x.TotalNumberOfUniqueJumps, f => f.Random.Int())
                .RuleFor(x => x.MissionsGiven, f => f.Random.Int())
                .RuleFor(x => x.MissionsPassed, f => f.Random.Int())
                .RuleFor(x => x.PassengersDroppedOffWithTaxi, f => f.Random.Int())
                .RuleFor(x => x.MoneyMadeWithTaxi, f => f.Random.Int())
                .RuleFor(x => x.IndustrialPassed, f => f.Random.Bool())
                .RuleFor(x => x.CommercialPassed, f => f.Random.Bool())
                .RuleFor(x => x.SuburbanPassed, f => f.Random.Bool())
                .RuleFor(x => x.ElBurroTime, f => f.Random.Int())
                .RuleFor(x => x.DistanceTravelledOnFoot, f => f.Random.Float())
                .RuleFor(x => x.DistanceTravelledInVehicle, f => f.Random.Float())
                .RuleFor(x => x.Record4x4One, f => f.Random.Int())
                .RuleFor(x => x.Record4x4Two, f => f.Random.Int())
                .RuleFor(x => x.Record4x4Three, f => f.Random.Int())
                .RuleFor(x => x.Record4x4Mayhem, f => f.Random.Int())
                .RuleFor(x => x.LivesSavedWithAmbulance, f => f.Random.Int())
                .RuleFor(x => x.CriminalsCaught, f => f.Random.Int())
                .RuleFor(x => x.HighestLevelAmbulanceMission, f => f.Random.Int())
                .RuleFor(x => x.FiresExtinguished, f => f.Random.Int())
                .RuleFor(x => x.LongestFlightInDodo, f => f.Random.Int())
                .RuleFor(x => x.TimeTakenDefuseMission, f => f.Random.Int())
                .RuleFor(x => x.NumberKillFrenziesPassed, f => f.Random.Int())
                .RuleFor(x => x.TotalNumberKillFrenzies, f => f.Random.Int())
                .RuleFor(x => x.TotalNumberMissions, f => f.Random.Int())
                .RuleFor(x => x.FastestTimes, f => Generator.Array(Stats.Limits.MaxNumFastestTimes, g => f.Random.Int()))
                .RuleFor(x => x.HighestScores, f => Generator.Array(Stats.Limits.MaxNumHighestScores, g => f.Random.Int()))
                .RuleFor(x => x.KillsSinceLastCheckpoint, f => f.Random.Int())
                .RuleFor(x => x.TotalLegitimateKills, f => f.Random.Int())
                .RuleFor(x => x.LastMissionPassedName, f => Generator.Words(f, Stats.Limits.MaxLastMissionPassedNameLength - 1));

            return model.Generate();
        }

        [Fact]
        public void RandomDataSerialization()
        {
            Stats x0 = GenerateTestObject();
            Stats x1 = CreateSerializedCopy(x0, out byte[] data);

            Assert.Equal(x0.PeopleKilledByPlayer, x1.PeopleKilledByPlayer);
            Assert.Equal(x0.PeopleKilledByOthers, x1.PeopleKilledByOthers);
            Assert.Equal(x0.CarsExploded, x1.CarsExploded);
            Assert.Equal(x0.RoundsFiredByPlayer, x1.RoundsFiredByPlayer);
            Assert.Equal(x0.PedsKilledOfThisType, x1.PedsKilledOfThisType);
            Assert.Equal(x0.HelisDestroyed, x1.HelisDestroyed);
            Assert.Equal(x0.ProgressMade, x1.ProgressMade);
            Assert.Equal(x0.TotalProgressInGame, x1.TotalProgressInGame);
            Assert.Equal(x0.KgsOfExplosivesUsed, x1.KgsOfExplosivesUsed);
            Assert.Equal(x0.InstantHitsFiredByPlayer, x1.InstantHitsFiredByPlayer);
            Assert.Equal(x0.InstantHitsHitByPlayer, x1.InstantHitsHitByPlayer);
            Assert.Equal(x0.CarsCrushed, x1.CarsCrushed);
            Assert.Equal(x0.HeadsPopped, x1.HeadsPopped);
            Assert.Equal(x0.TimesArrested, x1.TimesArrested);
            Assert.Equal(x0.TimesDied, x1.TimesDied);
            Assert.Equal(x0.DaysPassed, x1.DaysPassed);
            Assert.Equal(x0.MmRain, x1.MmRain);
            Assert.Equal(x0.MaximumJumpDistance, x1.MaximumJumpDistance);
            Assert.Equal(x0.MaximumJumpHeight, x1.MaximumJumpHeight);
            Assert.Equal(x0.MaximumJumpFlips, x1.MaximumJumpFlips);
            Assert.Equal(x0.MaximumJumpSpins, x1.MaximumJumpSpins);
            Assert.Equal(x0.BestStuntJump, x1.BestStuntJump);
            Assert.Equal(x0.NumberOfUniqueJumpsFound, x1.NumberOfUniqueJumpsFound);
            Assert.Equal(x0.TotalNumberOfUniqueJumps, x1.TotalNumberOfUniqueJumps);
            Assert.Equal(x0.MissionsGiven, x1.MissionsGiven);
            Assert.Equal(x0.MissionsPassed, x1.MissionsPassed);
            Assert.Equal(x0.PassengersDroppedOffWithTaxi, x1.PassengersDroppedOffWithTaxi);
            Assert.Equal(x0.MoneyMadeWithTaxi, x1.MoneyMadeWithTaxi);
            Assert.Equal(x0.IndustrialPassed, x1.IndustrialPassed);
            Assert.Equal(x0.CommercialPassed, x1.CommercialPassed);
            Assert.Equal(x0.SuburbanPassed, x1.SuburbanPassed);
            Assert.Equal(x0.ElBurroTime, x1.ElBurroTime);
            Assert.Equal(x0.DistanceTravelledOnFoot, x1.DistanceTravelledOnFoot);
            Assert.Equal(x0.DistanceTravelledInVehicle, x1.DistanceTravelledInVehicle);
            Assert.Equal(x0.Record4x4One, x1.Record4x4One);
            Assert.Equal(x0.Record4x4Two, x1.Record4x4Two);
            Assert.Equal(x0.Record4x4Three, x1.Record4x4Three);
            Assert.Equal(x0.Record4x4Mayhem, x1.Record4x4Mayhem);
            Assert.Equal(x0.LivesSavedWithAmbulance, x1.LivesSavedWithAmbulance);
            Assert.Equal(x0.CriminalsCaught, x1.CriminalsCaught);
            Assert.Equal(x0.HighestLevelAmbulanceMission, x1.HighestLevelAmbulanceMission);
            Assert.Equal(x0.FiresExtinguished, x1.FiresExtinguished);
            Assert.Equal(x0.LongestFlightInDodo, x1.LongestFlightInDodo);
            Assert.Equal(x0.TimeTakenDefuseMission, x1.TimeTakenDefuseMission);
            Assert.Equal(x0.NumberKillFrenziesPassed, x1.NumberKillFrenziesPassed);
            Assert.Equal(x0.TotalNumberKillFrenzies, x1.TotalNumberKillFrenzies);
            Assert.Equal(x0.TotalNumberMissions, x1.TotalNumberMissions);
            Assert.Equal(x0.FastestTimes, x1.FastestTimes);
            Assert.Equal(x0.HighestScores, x1.HighestScores);
            Assert.Equal(x0.KillsSinceLastCheckpoint, x1.KillsSinceLastCheckpoint);
            Assert.Equal(x0.TotalLegitimateKills, x1.TotalLegitimateKills);
            Assert.Equal(x0.LastMissionPassedName, x1.LastMissionPassedName);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(), data.Length);
        }
    }
}
