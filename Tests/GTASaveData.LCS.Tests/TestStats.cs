using Bogus;
using System;
using TestFramework;
using Xunit;

namespace GTASaveData.LCS.Tests
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
                .RuleFor(x => x.CarsCrushed, f => f.Random.Int())
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
                .RuleFor(x => x.DistanceTravelledByPlane, f => f.Random.Float())
                .RuleFor(x => x.LivesSavedWithAmbulance, f => f.Random.Int())
                .RuleFor(x => x.CriminalsCaught, f => f.Random.Int())
                .RuleFor(x => x.FiresExtinguished, f => f.Random.Int())
                .RuleFor(x => x.HighestLevelVigilanteMission, f => f.Random.Int())
                .RuleFor(x => x.HighestLevelAmbulanceMission, f => f.Random.Int())
                .RuleFor(x => x.HighestLevelFireMission, f => f.Random.Int())
                .RuleFor(x => x.PhotosTaken, f => f.Random.Int())
                .RuleFor(x => x.NumberKillFrenziesPassed, f => f.Random.Int())
                .RuleFor(x => x.MaxSecondsOnCarnageLeft, f => f.Random.Int())
                .RuleFor(x => x.MaxKillsOnRcTriad, f => f.Random.Int())
                .RuleFor(x => x.TotalNumberKillFrenzies, f => f.Random.Int())
                .RuleFor(x => x.TotalNumberMissions, f => f.Random.Int())
                .RuleFor(x => x.TimesDrowned, f => f.Random.Int())
                .RuleFor(x => x.SeagullsKilled, f => f.Random.Int())
                .RuleFor(x => x.WeaponBudget, f => f.Random.Float())
                .RuleFor(x => x.LoanSharks, f => f.Random.Int())
                .RuleFor(x => x.MovieStunts, f => f.Random.Int())
                .RuleFor(x => x.PizzasDelivered, f => f.Random.Float())
                .RuleFor(x => x.NoodlesDelivered, f => f.Random.Float())
                .RuleFor(x => x.MoneyMadeFromTourist, f => f.Random.Float())
                .RuleFor(x => x.TouristsTakenToSpots, f => f.Random.Float())
                .RuleFor(x => x.GarbagePickups, f => f.Random.Int())
                .RuleFor(x => x.IceCreamSold, f => f.Random.Int())
                .RuleFor(x => x.TopShootingRangeScore, f => f.Random.Int())
                .RuleFor(x => x.ShootingRank, f => f.Random.Int())
                .RuleFor(x => x.TopScrapyardChallengeScore, f => f.Random.Float())
                .RuleFor(x => x.Top9mmMayhemScore, f => f.Random.Float())
                .RuleFor(x => x.TopScooterShooterScore, f => f.Random.Float())
                .RuleFor(x => x.TopWichitaWipeoutScore, f => f.Random.Float())
                .RuleFor(x => x.LongestWheelie, f => f.Random.Int())
                .RuleFor(x => x.LongestStoppie, f => f.Random.Int())
                .RuleFor(x => x.Longest2Wheel, f => f.Random.Int())
                .RuleFor(x => x.LongestWheelieDist, f => f.Random.Float())
                .RuleFor(x => x.LongestStoppieDist, f => f.Random.Float())
                .RuleFor(x => x.Longest2WheelDist, f => f.Random.Float())
                .RuleFor(x => x.LongestFacePlantDist, f => f.Random.Float())
                .RuleFor(x => x.AutoPaintingBudget, f => f.Random.Float())
                .RuleFor(x => x.PropertyDestroyed, f => f.Random.Int())
                .RuleFor(x => x.NumPropertyOwned, f => f.Random.Int())
                .RuleFor(x => x.UnlockedCostumes, f => f.PickRandom<UnlockedCostumes>())
                .RuleFor(x => x.BloodringKills, f => f.Random.Int())
                .RuleFor(x => x.BloodringTime, f => f.Random.Int())
                .RuleFor(x => x.PropertyOwned, f => Generator.Array(Stats.NumProperties, g => f.Random.Byte()))
                .RuleFor(x => x.HighestChaseValue, f => f.Random.Float())
                .RuleFor(x => x.FastestTimes, f => Generator.Array(Stats.NumFastestTimes, g => f.Random.Int()))
                .RuleFor(x => x.HighestScores, f => Generator.Array(Stats.NumHighestScores, g => f.Random.Int()))
                .RuleFor(x => x.BestPositions, f => f.Random.Int())
                .RuleFor(x => x.KillsSinceLastCheckpoint, f => f.Random.Int())
                .RuleFor(x => x.TotalLegitimateKills, f => f.Random.Int())
                .RuleFor(x => x.LastMissionPassedName, f => Generator.AsciiString(f, Stats.LastMissionPassedNameLength - 1))
                .RuleFor(x => x.CheatedCount, f => f.Random.Int())
                .RuleFor(x => x.CarsSold, f => f.Random.Int())
                .RuleFor(x => x.MoneyMadeWithCarSales, f => f.Random.Int())
                .RuleFor(x => x.BikesSold, f => f.Random.Int())
                .RuleFor(x => x.MoneyMadeWithBikeSales, f => f.Random.Int())
                .RuleFor(x => x.NumberOfExportedCars, f => f.Random.Int())
                .RuleFor(x => x.TotalNumberOfCarExport, f => f.Random.Int())
                .RuleFor(x => x.HighestLevelSlashTv, f => f.Random.Int())
                .RuleFor(x => x.MoneyMadeWithSlashTv, f => f.Random.Int())
                .RuleFor(x => x.TotalKillsOnSlashTv, f => f.Random.Int())
                .RuleFor(x => x.PackagesSmuggled, f => f.Random.Int())
                .RuleFor(x => x.SmugglersWasted, f => f.Random.Int())
                .RuleFor(x => x.FastestSmugglingTime, f => f.Random.Int())
                .RuleFor(x => x.MoneyMadeInCoach, f => f.Random.Int())
                .RuleFor(x => x.CashMadeCollectingTrash, f => f.Random.Int())
                .RuleFor(x => x.HitmenKilled, f => f.Random.Int())
                .RuleFor(x => x.HighestGuardianAngelJusticeDished, f => f.Random.Int())
                .RuleFor(x => x.GuardianAngelMissionsPassed, f => f.Random.Int())
                .RuleFor(x => x.GuardianAngelHighestLevelInd, f => f.Random.Int())
                .RuleFor(x => x.GuardianAngelHighestLevelCom, f => f.Random.Int())
                .RuleFor(x => x.GuardianAngelHighestLevelSub, f => f.Random.Int())
                .RuleFor(x => x.MostTimeLeftTrainRace, f => f.Random.Int())
                .RuleFor(x => x.BestTimeGoGoFaggio, f => f.Random.Int())
                .RuleFor(x => x.HighestTrainCashEarned, f => f.Random.Int())
                .RuleFor(x => x.DirtBikeMostAir, f => f.Random.Int())
                .RuleFor(x => x.FastestHeliRaceTime, f => f.Random.Int())
                .RuleFor(x => x.BestHeliRacePosition, f => f.Random.Int())
                .RuleFor(x => x.NumberOutfitChanges, f => f.Random.Int())
                .RuleFor(x => x.BestBanditLapTimes, f => Generator.Array(Stats.NumBanditRaces, g => f.Random.Int()))
                .RuleFor(x => x.BestBanditPositions, f => Generator.Array(Stats.NumBanditRaces, g => f.Random.Int()))
                .RuleFor(x => x.BestStreetRacePositions, f => Generator.Array(Stats.NumStreetRaces, g => f.Random.Int()))
                .RuleFor(x => x.FastestStreetRaceLapTimes, f => Generator.Array(Stats.NumStreetRaces, g => f.Random.Int()))
                .RuleFor(x => x.FastestStreetRaceTimes, f => Generator.Array(Stats.NumStreetRaces, g => f.Random.Int()))
                .RuleFor(x => x.FastestDirtBikeLapTimes, f => Generator.Array(Stats.NumDirtBikeRaces, g => f.Random.Int()))
                .RuleFor(x => x.FastestDirtBikeTimes, f => Generator.Array(Stats.NumDirtBikeRaces, g => f.Random.Int()))
                .RuleFor(x => x.FavoriteRadioStationList, f => Generator.Array((format.IsMobile) ? Stats.NumRadioStationsMobile : Stats.NumRadioStations, g => f.Random.Float()));

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
            Assert.Equal(x0.CarsCrushed, x1.CarsCrushed);
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
            Assert.Equal(x0.DistanceTravelledByPlane, x1.DistanceTravelledByPlane);
            Assert.Equal(x0.LivesSavedWithAmbulance, x1.LivesSavedWithAmbulance);
            Assert.Equal(x0.CriminalsCaught, x1.CriminalsCaught);
            Assert.Equal(x0.FiresExtinguished, x1.FiresExtinguished);
            Assert.Equal(x0.HighestLevelVigilanteMission, x1.HighestLevelVigilanteMission);
            Assert.Equal(x0.HighestLevelAmbulanceMission, x1.HighestLevelAmbulanceMission);
            Assert.Equal(x0.HighestLevelFireMission, x1.HighestLevelFireMission);
            Assert.Equal(x0.PhotosTaken, x1.PhotosTaken);
            Assert.Equal(x0.NumberKillFrenziesPassed, x1.NumberKillFrenziesPassed);
            Assert.Equal(x0.MaxSecondsOnCarnageLeft, x1.MaxSecondsOnCarnageLeft);
            Assert.Equal(x0.MaxKillsOnRcTriad, x1.MaxKillsOnRcTriad);
            Assert.Equal(x0.TotalNumberKillFrenzies, x1.TotalNumberKillFrenzies);
            Assert.Equal(x0.TotalNumberMissions, x1.TotalNumberMissions);
            Assert.Equal(x0.TimesDrowned, x1.TimesDrowned);
            Assert.Equal(x0.SeagullsKilled, x1.SeagullsKilled);
            Assert.Equal(x0.WeaponBudget, x1.WeaponBudget);
            Assert.Equal(x0.LoanSharks, x1.LoanSharks);
            Assert.Equal(x0.MovieStunts, x1.MovieStunts);
            Assert.Equal(x0.PizzasDelivered, x1.PizzasDelivered);
            Assert.Equal(x0.NoodlesDelivered, x1.NoodlesDelivered);
            Assert.Equal(x0.MoneyMadeFromTourist, x1.MoneyMadeFromTourist);
            Assert.Equal(x0.TouristsTakenToSpots, x1.TouristsTakenToSpots);
            Assert.Equal(x0.GarbagePickups, x1.GarbagePickups);
            Assert.Equal(x0.IceCreamSold, x1.IceCreamSold);
            Assert.Equal(x0.TopShootingRangeScore, x1.TopShootingRangeScore);
            Assert.Equal(x0.ShootingRank, x1.ShootingRank);
            Assert.Equal(x0.TopScrapyardChallengeScore, x1.TopScrapyardChallengeScore);
            Assert.Equal(x0.Top9mmMayhemScore, x1.Top9mmMayhemScore);
            Assert.Equal(x0.TopScooterShooterScore, x1.TopScooterShooterScore);
            Assert.Equal(x0.TopWichitaWipeoutScore, x1.TopWichitaWipeoutScore);
            Assert.Equal(x0.LongestWheelie, x1.LongestWheelie);
            Assert.Equal(x0.LongestStoppie, x1.LongestStoppie);
            Assert.Equal(x0.Longest2Wheel, x1.Longest2Wheel);
            Assert.Equal(x0.LongestWheelieDist, x1.LongestWheelieDist);
            Assert.Equal(x0.LongestStoppieDist, x1.LongestStoppieDist);
            Assert.Equal(x0.Longest2WheelDist, x1.Longest2WheelDist);
            Assert.Equal(x0.LongestFacePlantDist, x1.LongestFacePlantDist);
            Assert.Equal(x0.AutoPaintingBudget, x1.AutoPaintingBudget);
            Assert.Equal(x0.PropertyDestroyed, x1.PropertyDestroyed);
            Assert.Equal(x0.NumPropertyOwned, x1.NumPropertyOwned);
            Assert.Equal(x0.UnlockedCostumes, x1.UnlockedCostumes);
            Assert.Equal(x0.BloodringKills, x1.BloodringKills);
            Assert.Equal(x0.BloodringTime, x1.BloodringTime);
            Assert.Equal(x0.PropertyOwned, x1.PropertyOwned);
            Assert.Equal(x0.HighestChaseValue, x1.HighestChaseValue);
            Assert.Equal(x0.FastestTimes, x1.FastestTimes);
            Assert.Equal(x0.HighestScores, x1.HighestScores);
            Assert.Equal(x0.BestPositions, x1.BestPositions);
            Assert.Equal(x0.KillsSinceLastCheckpoint, x1.KillsSinceLastCheckpoint);
            Assert.Equal(x0.TotalLegitimateKills, x1.TotalLegitimateKills);
            Assert.Equal(x0.LastMissionPassedName, x1.LastMissionPassedName);
            Assert.Equal(x0.CheatedCount, x1.CheatedCount);
            Assert.Equal(x0.CarsSold, x1.CarsSold);
            Assert.Equal(x0.MoneyMadeWithCarSales, x1.MoneyMadeWithCarSales);
            Assert.Equal(x0.BikesSold, x1.BikesSold);
            Assert.Equal(x0.MoneyMadeWithBikeSales, x1.MoneyMadeWithBikeSales);
            Assert.Equal(x0.NumberOfExportedCars, x1.NumberOfExportedCars);
            Assert.Equal(x0.TotalNumberOfCarExport, x1.TotalNumberOfCarExport);
            Assert.Equal(x0.HighestLevelSlashTv, x1.HighestLevelSlashTv);
            Assert.Equal(x0.MoneyMadeWithSlashTv, x1.MoneyMadeWithSlashTv);
            Assert.Equal(x0.TotalKillsOnSlashTv, x1.TotalKillsOnSlashTv);
            Assert.Equal(x0.PackagesSmuggled, x1.PackagesSmuggled);
            Assert.Equal(x0.SmugglersWasted, x1.SmugglersWasted);
            Assert.Equal(x0.FastestSmugglingTime, x1.FastestSmugglingTime);
            Assert.Equal(x0.MoneyMadeInCoach, x1.MoneyMadeInCoach);
            Assert.Equal(x0.CashMadeCollectingTrash, x1.CashMadeCollectingTrash);
            Assert.Equal(x0.HitmenKilled, x1.HitmenKilled);
            Assert.Equal(x0.HighestGuardianAngelJusticeDished, x1.HighestGuardianAngelJusticeDished);
            Assert.Equal(x0.GuardianAngelMissionsPassed, x1.GuardianAngelMissionsPassed);
            Assert.Equal(x0.GuardianAngelHighestLevelInd, x1.GuardianAngelHighestLevelInd);
            Assert.Equal(x0.GuardianAngelHighestLevelCom, x1.GuardianAngelHighestLevelCom);
            Assert.Equal(x0.GuardianAngelHighestLevelSub, x1.GuardianAngelHighestLevelSub);
            Assert.Equal(x0.MostTimeLeftTrainRace, x1.MostTimeLeftTrainRace);
            Assert.Equal(x0.BestTimeGoGoFaggio, x1.BestTimeGoGoFaggio);
            Assert.Equal(x0.HighestTrainCashEarned, x1.HighestTrainCashEarned);
            Assert.Equal(x0.DirtBikeMostAir, x1.DirtBikeMostAir);
            Assert.Equal(x0.FastestHeliRaceTime, x1.FastestHeliRaceTime);
            Assert.Equal(x0.BestHeliRacePosition, x1.BestHeliRacePosition);
            Assert.Equal(x0.NumberOutfitChanges, x1.NumberOutfitChanges);
            Assert.Equal(x0.BestBanditLapTimes, x1.BestBanditLapTimes);
            Assert.Equal(x0.BestBanditPositions, x1.BestBanditPositions);
            Assert.Equal(x0.BestStreetRacePositions, x1.BestStreetRacePositions);
            Assert.Equal(x0.FastestStreetRaceLapTimes, x1.FastestStreetRaceLapTimes);
            Assert.Equal(x0.FastestStreetRaceTimes, x1.FastestStreetRaceTimes);
            Assert.Equal(x0.FastestDirtBikeLapTimes, x1.FastestDirtBikeLapTimes);
            Assert.Equal(x0.FastestDirtBikeTimes, x1.FastestDirtBikeTimes);
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
