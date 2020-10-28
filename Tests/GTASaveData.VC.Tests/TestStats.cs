using Bogus;
using System;
using TestFramework;
using Xunit;

namespace GTASaveData.VC.Tests
{
    public class TestStats : Base<Stats>
    {
        public override Stats GenerateTestObject(FileFormat format)
        {
            Faker<Stats> model = new Faker<Stats>()
                .RuleFor(x => x.PeopleKilledByPlayer, f => f.Random.Int())
                .RuleFor(x => x.PeopleKilledByOthers, f => f.Random.Int())
                .RuleFor(x => x.CarsExploded, f => f.Random.Int())
                .RuleFor(x => x.BoatsExploded, f => f.Random.Int())
                .RuleFor(x => x.TyresPopped, f => f.Random.Int())
                .RuleFor(x => x.RoundsFiredByPlayer, f => f.Random.Int())
                .RuleFor(x => x.PedsKilledOfThisType, f => Generator.Array(Stats.NumPedTypes, g => f.Random.Int()))
                .RuleFor(x => x.HelisDestroyed, f => f.Random.Int())
                .RuleFor(x => x.ProgressMade, f => f.Random.Float())
                .RuleFor(x => x.TotalProgressInGame, f => f.Random.Float())
                .RuleFor(x => x.KgsOfExplosivesUsed, f => f.Random.Int())
                .RuleFor(x => x.BulletsThatHit, f => f.Random.Int())
                .RuleFor(x => x.HeadsPopped, f => f.Random.Int())
                .RuleFor(x => x.WantedStarsAttained, f => f.Random.Int())
                .RuleFor(x => x.WantedStarsEvaded, f => f.Random.Int())
                .RuleFor(x => x.TimesArrested, f => f.Random.Int())
                .RuleFor(x => x.TimesDied, f => f.Random.Int())
                .RuleFor(x => x.DaysPassed, f => f.Random.Int())
                .RuleFor(x => x.SafeHouseVisits, f => f.Random.Int())
                .RuleFor(x => x.Sprayings, f => f.Random.Int())
                .RuleFor(x => x.MaximumJumpDistance, f => f.Random.Float())
                .RuleFor(x => x.MaximumJumpHeight, f => f.Random.Float())
                .RuleFor(x => x.MaximumJumpFlips, f => f.Random.Int())
                .RuleFor(x => x.MaximumJumpSpins, f => f.Random.Int())
                .RuleFor(x => x.BestStuntJump, f => f.Random.Int())
                .RuleFor(x => x.NumberOfUniqueJumpsFound, f => f.Random.Int())
                .RuleFor(x => x.TotalNumberOfUniqueJumps, f => f.Random.Int())
                .RuleFor(x => x.MissionsGiven, f => f.Random.Int())
                .RuleFor(x => x.PassengersDroppedOffWithTaxi, f => f.Random.Int())
                .RuleFor(x => x.MoneyMadeWithTaxi, f => f.Random.Int())
                .RuleFor(x => x.IndustrialPassed, f => f.Random.Bool())
                .RuleFor(x => x.CommercialPassed, f => f.Random.Bool())
                .RuleFor(x => x.SuburbanPassed, f => f.Random.Bool())
                .RuleFor(x => x.PamphletMissionPassed, f => f.Random.Bool())
                .RuleFor(x => x.NoMoreHurricanes, f => f.Random.Bool())
                .RuleFor(x => x.DistanceTravelledOnFoot, f => f.Random.Float())
                .RuleFor(x => x.DistanceTravelledByCar, f => f.Random.Float())
                .RuleFor(x => x.DistanceTravelledByBike, f => f.Random.Float())
                .RuleFor(x => x.DistanceTravelledByBoat, f => f.Random.Float())
                .RuleFor(x => x.DistanceTravelledByGolfCart, f => f.Random.Float())
                .RuleFor(x => x.DistanceTravelledByHelicopter, f => f.Random.Float())
                .RuleFor(x => x.DistanceTravelledByPlane, f => f.Random.Float())
                .RuleFor(x => x.LivesSavedWithAmbulance, f => f.Random.Int())
                .RuleFor(x => x.CriminalsCaught, f => f.Random.Int())
                .RuleFor(x => x.FiresExtinguished, f => f.Random.Int())
                .RuleFor(x => x.HighestLevelVigilanteMission, f => f.Random.Int())
                .RuleFor(x => x.HighestLevelAmbulanceMission, f => f.Random.Int())
                .RuleFor(x => x.HighestLevelFireMission, f => f.Random.Int())
                .RuleFor(x => x.PhotosTaken, f => f.Random.Int())
                .RuleFor(x => x.NumberKillFrenziesPassed, f => f.Random.Int())
                .RuleFor(x => x.TotalNumberKillFrenzies, f => f.Random.Int())
                .RuleFor(x => x.TotalNumberMissions, f => f.Random.Int())
                .RuleFor(x => x.FlightTime, f => f.Random.Int())
                .RuleFor(x => x.TimesDrowned, f => f.Random.Int())
                .RuleFor(x => x.SeagullsKilled, f => f.Random.Int())
                .RuleFor(x => x.WeaponBudget, f => f.Random.Float())
                .RuleFor(x => x.FashionBudget, f => f.Random.Float())
                .RuleFor(x => x.LoanSharks, f => f.Random.Float())
                .RuleFor(x => x.StoresKnockedOff, f => f.Random.Float())
                .RuleFor(x => x.MovieStunts, f => f.Random.Float())
                .RuleFor(x => x.Assassinations, f => f.Random.Float())
                .RuleFor(x => x.PizzasDelivered, f => f.Random.Float())
                .RuleFor(x => x.GarbagePickups, f => f.Random.Float())
                .RuleFor(x => x.IceCreamSold, f => f.Random.Float())
                .RuleFor(x => x.TopShootingRangeScore, f => f.Random.Float())
                .RuleFor(x => x.ShootingRank, f => f.Random.Float())
                .RuleFor(x => x.LongestWheelie, f => f.Random.Int())
                .RuleFor(x => x.LongestStoppie, f => f.Random.Int())
                .RuleFor(x => x.Longest2Wheel, f => f.Random.Int())
                .RuleFor(x => x.LongestWheelieDist, f => f.Random.Float())
                .RuleFor(x => x.LongestStoppieDist, f => f.Random.Float())
                .RuleFor(x => x.Longest2WheelDist, f => f.Random.Float())
                .RuleFor(x => x.PropertyBudget, f => f.Random.Float())
                .RuleFor(x => x.AutoPaintingBudget, f => f.Random.Float())
                .RuleFor(x => x.PropertyDestroyed, f => f.Random.Int())
                .RuleFor(x => x.NumPropertyOwned, f => f.Random.Int())
                .RuleFor(x => x.BloodRingKills, f => f.Random.Int())
                .RuleFor(x => x.BloodRingTime, f => f.Random.Int())
                .RuleFor(x => x.PropertyOwned, f => Generator.Array(Stats.NumProperties, g => f.Random.Bool()))
                .RuleFor(x => x.HighestChaseValue, f => f.Random.Float())
                .RuleFor(x => x.FastestTimes, f => Generator.Array(Stats.NumFastestTimes, g => f.Random.Int()))
                .RuleFor(x => x.HighestScores, f => Generator.Array(Stats.NumHighestScores, g => f.Random.Int()))
                .RuleFor(x => x.BestPositions, f => Generator.Array(Stats.NumBestPositions, g => f.Random.Int()))
                .RuleFor(x => x.KillsSinceLastCheckpoint, f => f.Random.Int())
                .RuleFor(x => x.TotalLegitimateKills, f => f.Random.Int())
                .RuleFor(x => x.LastMissionPassedName, f => Generator.AsciiString(f, Stats.LastMissionPassedNameLength - 1))
                .RuleFor(x => x.CheatedCount, f => f.Random.Int())
                .RuleFor(x => x.FavoriteRadioStationList, f => Generator.Array(Stats.NumRadioStations, g => f.Random.Float()));

            return model.Generate();
        }

        [Theory]
        [MemberData(nameof(FileFormats))]
        public void RandomDataSerialization(FileFormat format)
        {
            Stats x0 = GenerateTestObject(format);
            Stats x1 = CreateSerializedCopy(x0, format, out byte[] data);

            Assert.Equal(x0.PeopleKilledByPlayer, x1.PeopleKilledByPlayer);
            Assert.Equal(x0.PeopleKilledByOthers, x1.PeopleKilledByOthers);
            Assert.Equal(x0.CarsExploded, x1.CarsExploded);
            Assert.Equal(x0.BoatsExploded, x1.BoatsExploded);
            Assert.Equal(x0.TyresPopped, x1.TyresPopped);
            Assert.Equal(x0.RoundsFiredByPlayer, x1.RoundsFiredByPlayer);
            Assert.Equal(x0.PedsKilledOfThisType, x1.PedsKilledOfThisType);
            Assert.Equal(x0.HelisDestroyed, x1.HelisDestroyed);
            Assert.Equal(x0.ProgressMade, x1.ProgressMade);
            Assert.Equal(x0.TotalProgressInGame, x1.TotalProgressInGame);
            Assert.Equal(x0.KgsOfExplosivesUsed, x1.KgsOfExplosivesUsed);
            Assert.Equal(x0.BulletsThatHit, x1.BulletsThatHit);
            Assert.Equal(x0.HeadsPopped, x1.HeadsPopped);
            Assert.Equal(x0.WantedStarsAttained, x1.WantedStarsAttained);
            Assert.Equal(x0.WantedStarsEvaded, x1.WantedStarsEvaded);
            Assert.Equal(x0.TimesArrested, x1.TimesArrested);
            Assert.Equal(x0.TimesDied, x1.TimesDied);
            Assert.Equal(x0.DaysPassed, x1.DaysPassed);
            Assert.Equal(x0.SafeHouseVisits, x1.SafeHouseVisits);
            Assert.Equal(x0.Sprayings, x1.Sprayings);
            Assert.Equal(x0.MaximumJumpDistance, x1.MaximumJumpDistance);
            Assert.Equal(x0.MaximumJumpHeight, x1.MaximumJumpHeight);
            Assert.Equal(x0.MaximumJumpFlips, x1.MaximumJumpFlips);
            Assert.Equal(x0.MaximumJumpSpins, x1.MaximumJumpSpins);
            Assert.Equal(x0.BestStuntJump, x1.BestStuntJump);
            Assert.Equal(x0.NumberOfUniqueJumpsFound, x1.NumberOfUniqueJumpsFound);
            Assert.Equal(x0.TotalNumberOfUniqueJumps, x1.TotalNumberOfUniqueJumps);
            Assert.Equal(x0.MissionsGiven, x1.MissionsGiven);
            Assert.Equal(x0.PassengersDroppedOffWithTaxi, x1.PassengersDroppedOffWithTaxi);
            Assert.Equal(x0.MoneyMadeWithTaxi, x1.MoneyMadeWithTaxi);
            Assert.Equal(x0.IndustrialPassed, x1.IndustrialPassed);
            Assert.Equal(x0.CommercialPassed, x1.CommercialPassed);
            Assert.Equal(x0.SuburbanPassed, x1.SuburbanPassed);
            Assert.Equal(x0.PamphletMissionPassed, x1.PamphletMissionPassed);
            Assert.Equal(x0.NoMoreHurricanes, x1.NoMoreHurricanes);
            Assert.Equal(x0.DistanceTravelledOnFoot, x1.DistanceTravelledOnFoot);
            Assert.Equal(x0.DistanceTravelledByCar, x1.DistanceTravelledByCar);
            Assert.Equal(x0.DistanceTravelledByBike, x1.DistanceTravelledByBike);
            Assert.Equal(x0.DistanceTravelledByBoat, x1.DistanceTravelledByBoat);
            Assert.Equal(x0.DistanceTravelledByGolfCart, x1.DistanceTravelledByGolfCart);
            Assert.Equal(x0.DistanceTravelledByHelicopter, x1.DistanceTravelledByHelicopter);
            Assert.Equal(x0.DistanceTravelledByPlane, x1.DistanceTravelledByPlane);
            Assert.Equal(x0.LivesSavedWithAmbulance, x1.LivesSavedWithAmbulance);
            Assert.Equal(x0.CriminalsCaught, x1.CriminalsCaught);
            Assert.Equal(x0.FiresExtinguished, x1.FiresExtinguished);
            Assert.Equal(x0.HighestLevelVigilanteMission, x1.HighestLevelVigilanteMission);
            Assert.Equal(x0.HighestLevelAmbulanceMission, x1.HighestLevelAmbulanceMission);
            Assert.Equal(x0.HighestLevelFireMission, x1.HighestLevelFireMission);
            Assert.Equal(x0.PhotosTaken, x1.PhotosTaken);
            Assert.Equal(x0.NumberKillFrenziesPassed, x1.NumberKillFrenziesPassed);
            Assert.Equal(x0.TotalNumberKillFrenzies, x1.TotalNumberKillFrenzies);
            Assert.Equal(x0.TotalNumberMissions, x1.TotalNumberMissions);
            Assert.Equal(x0.FlightTime, x1.FlightTime);
            Assert.Equal(x0.TimesDrowned, x1.TimesDrowned);
            Assert.Equal(x0.SeagullsKilled, x1.SeagullsKilled);
            Assert.Equal(x0.WeaponBudget, x1.WeaponBudget);
            Assert.Equal(x0.FashionBudget, x1.FashionBudget);
            Assert.Equal(x0.LoanSharks, x1.LoanSharks);
            Assert.Equal(x0.StoresKnockedOff, x1.StoresKnockedOff);
            Assert.Equal(x0.MovieStunts, x1.MovieStunts);
            Assert.Equal(x0.Assassinations, x1.Assassinations);
            Assert.Equal(x0.PizzasDelivered, x1.PizzasDelivered);
            Assert.Equal(x0.GarbagePickups, x1.GarbagePickups);
            Assert.Equal(x0.IceCreamSold, x1.IceCreamSold);
            Assert.Equal(x0.TopShootingRangeScore, x1.TopShootingRangeScore);
            Assert.Equal(x0.ShootingRank, x1.ShootingRank);
            Assert.Equal(x0.LongestWheelie, x1.LongestWheelie);
            Assert.Equal(x0.LongestStoppie, x1.LongestStoppie);
            Assert.Equal(x0.Longest2Wheel, x1.Longest2Wheel);
            Assert.Equal(x0.LongestWheelieDist, x1.LongestWheelieDist);
            Assert.Equal(x0.LongestStoppieDist, x1.LongestStoppieDist);
            Assert.Equal(x0.Longest2WheelDist, x1.Longest2WheelDist);
            Assert.Equal(x0.PropertyBudget, x1.PropertyBudget);
            Assert.Equal(x0.AutoPaintingBudget, x1.AutoPaintingBudget);
            Assert.Equal(x0.PropertyDestroyed, x1.PropertyDestroyed);
            Assert.Equal(x0.NumPropertyOwned, x1.NumPropertyOwned);
            Assert.Equal(x0.BloodRingKills, x1.BloodRingKills);
            Assert.Equal(x0.BloodRingTime, x1.BloodRingTime);
            Assert.Equal(x0.PropertyOwned, x1.PropertyOwned);
            Assert.Equal(x0.HighestChaseValue, x1.HighestChaseValue);
            Assert.Equal(x0.FastestTimes, x1.FastestTimes);
            Assert.Equal(x0.HighestScores, x1.HighestScores);
            Assert.Equal(x0.BestPositions, x1.BestPositions);
            Assert.Equal(x0.KillsSinceLastCheckpoint, x1.KillsSinceLastCheckpoint);
            Assert.Equal(x0.TotalLegitimateKills, x1.TotalLegitimateKills);
            Assert.Equal(x0.LastMissionPassedName, x1.LastMissionPassedName);
            Assert.Equal(x0.CheatedCount, x1.CheatedCount);
            Assert.Equal(x0.FavoriteRadioStationList, x1.FavoriteRadioStationList);

            Assert.Equal(x0, x1);
            Assert.Equal(GetSizeOfTestObject(x0, format), data.Length);
        }

        [Fact]
        public void CopyConstructor()
        {
            Stats x0 = GenerateTestObject();
            Stats x1 = new Stats(x0);

            Assert.Equal(x0, x1);
        }
    }
}
