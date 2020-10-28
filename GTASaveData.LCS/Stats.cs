using System;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.LCS
{
    public class Stats : SaveDataObject, IStats,
        IEquatable<Stats>, IDeepClonable<Stats>
    {
        public const int NumPedTypes = 23;
        public const int NumProperties = 15;
        public const int NumFastestTimes = 23;
        public const int NumHighestScores = 5;
        public const int NumBanditRaces = 3;
        public const int NumStreetRaces = 6;
        public const int NumDirtBikeRaces = 10;
        public const int NumRadioStations = 10;
        public const int NumRadioStationsMobile = 11;
        public const int LastMissionPassedNameLength = 8;

        private int m_peopleKilledByPlayer;
        private int m_peopleKilledByOthers;
        private int m_carsExploded;
        private int m_boatsExploded;
        private int m_tyresPopped;
        private int m_roundsFiredByPlayer;
        private Array<int> m_pedsKilledOfThisType;
        private int m_helisDestroyed;
        private float m_progressMade;
        private float m_totalProgressInGame;
        private int m_kgsOfExplosivesUsed;
        private int m_bulletsThatHit;
        private int m_carsCrushed;
        private int m_headsPopped;
        private int m_wantedStarsAttained;
        private int m_wantedStarsEvaded;
        private int m_timesArrested;
        private int m_timesDied;
        private int m_daysPassed;
        private int m_safeHouseVisits;
        private int m_sprayings;
        private float m_maximumJumpDistance;
        private float m_maximumJumpHeight;
        private int m_maximumJumpFlips;
        private int m_maximumJumpSpins;
        private int m_bestStuntJump;
        private int m_numberOfUniqueJumpsFound;
        private int m_totalNumberOfUniqueJumps;
        private int m_missionsGiven;
        private int m_passengersDroppedOffWithTaxi;
        private int m_moneyMadeWithTaxi;
        private bool m_industrialPassed;
        private bool m_commercialPassed;
        private bool m_suburbanPassed;
        private bool m_pamphletMissionPassed; // vc leftover
        private bool m_noMoreHurricanes; // vc leftover; no effect?
        private float m_distanceTravelledOnFoot;
        private float m_distanceTravelledByCar;
        private float m_distanceTravelledByBike;
        private float m_distanceTravelledByBoat;
        private float m_distanceTravelledByPlane; // vc leftover
        private int m_livesSavedWithAmbulance;
        private int m_criminalsCaught;
        private int m_firesExtinguished;
        private int m_highestLevelVigilanteMission;
        private int m_highestLevelAmbulanceMission;
        private int m_highestLevelFireMission;
        private int m_photosTaken;
        private int m_numberKillFrenziesPassed;
        private int m_maxSecondsOnCarnageLeft;
        private int m_maxKillsOnRcTriad;
        private int m_totalNumberKillFrenzies;
        private int m_totalNumberMissions;
        private int m_timesDrowned;
        private int m_seagullsKilled;
        private float m_weaponBudget;
        private int m_loanSharks; // vc leftover
        private int m_movieStunts; // vc leftover
        private float m_pizzasDelivered;
        private float m_noodlesDelivered;
        private float m_moneyMadeFromTourist;
        private float m_touristsTakenToSpots;
        private int m_garbagePickups; // vc leftover; no relation to Trash Dash
        private int m_iceCreamSold; // vc leftover?
        private int m_topShootingRangeScore; // vc leftover
        private int m_shootingRank; // vc leftover
        private float m_topScrapyardChallengeScore;
        private float m_top9mmMayhemScore;
        private float m_topScooterShooterScore;
        private float m_topWichitaWipeoutScore; // AWOL Angel
        private int m_longestWheelie;
        private int m_longestStoppie;
        private int m_longest2Wheel;
        private float m_longestWheelieDist;
        private float m_longestStoppieDist;
        private float m_longest2WheelDist;
        private float m_longestFacePlantDist;
        private float m_autoPaintingBudget;
        private int m_propertyDestroyed;
        private int m_numPropertyOwned; // vc leftover
        private PlayerOutfitFlags m_unlockedCostumes; // doesn't actually unlock outfits, just shows them in stats
        private int m_bloodringKills; // vc leftover
        private int m_bloodringTime; // vc leftover
        private Array<byte> m_propertyOwned; // vc leftover
        private float m_highestChaseValue;
        private Array<int> m_fastestTimes; // vc leftover
        private Array<int> m_highestScores; // vc leftover
        private int m_bestPositions; // vc leftover
        private int m_killsSinceLastCheckpoint; // always 0 on save
        private int m_totalLegitimateKills;
        private string m_lastMissionPassedName;
        private int m_cheatedCount;
        private int m_carsSold;
        private int m_moneyMadeWithCarSales;
        private int m_bikesSold;
        private int m_moneyMadeWithBikeSales;
        private int m_numberOfExportedCars;
        private int m_totalNumberOfCarExport;
        private int m_highestLevelSlashTv;
        private int m_moneyMadeWithSlashTv;
        private int m_totalKillsOnSlashTv;
        private int m_packagesSmuggled; // beta/multiplayer?
        private int m_smugglersWasted; // beta/multiplayer?
        private int m_fastestSmugglingTime; // beta/multiplayer?
        private int m_moneyMadeInCoach; // beta/multiplayer?
        private int m_cashMadeCollectingTrash;
        private int m_hitmenKilled;
        private int m_highestGuardianAngelJusticeDished;
        private int m_guardianAngelMissionsPassed;
        private int m_guardianAngelHighestLevelInd;
        private int m_guardianAngelHighestLevelCom;
        private int m_guardianAngelHighestLevelSub;
        private int m_mostTimeLeftTrainRace;
        private int m_bestTimeGoGoFaggio;
        private int m_dirtBikeMostAir;
        private int m_highestTrainCashEarned; // bugged, doesn't show up in stats
        private int m_fastestHeliRaceTime; // beta/multiplayer?
        private int m_bestHeliRacePosition; // beta/multiplayer?
        private int m_numberOutfitChanges;
        private Array<int> m_bestBanditLapTimes;
        private Array<int> m_bestBanditPositions;
        private Array<int> m_bestStreetRacePositions;
        private Array<int> m_fastestStreetRaceLapTimes;
        private Array<int> m_fastestStreetRaceTimes;
        private Array<int> m_fastestDirtBikeLapTimes;
        private Array<int> m_fastestDirtBikeTimes;
        private Array<float> m_favoriteRadioStationList;

        public int PeopleKilledByPlayer
        {
            get { return m_peopleKilledByPlayer; }
            set { m_peopleKilledByPlayer = value; OnPropertyChanged(); }
        }

        public int PeopleKilledByOthers
        {
            get { return m_peopleKilledByOthers; }
            set { m_peopleKilledByOthers = value; OnPropertyChanged(); }
        }

        public int CarsExploded
        {
            get { return m_carsExploded; }
            set { m_carsExploded = value; OnPropertyChanged(); }
        }

        public int BoatsExploded
        {
            get { return m_boatsExploded; }
            set { m_boatsExploded = value; OnPropertyChanged(); }
        }

        public int TyresPopped
        {
            get { return m_tyresPopped; }
            set { m_tyresPopped = value; OnPropertyChanged(); }
        }

        public int RoundsFiredByPlayer
        {
            get { return m_roundsFiredByPlayer; }
            set { m_roundsFiredByPlayer = value; OnPropertyChanged(); }
        }

        public Array<int> PedsKilledOfThisType
        {
            get { return m_pedsKilledOfThisType; }
            set { m_pedsKilledOfThisType = value; OnPropertyChanged(); }
        }

        public int HelisDestroyed
        {
            get { return m_helisDestroyed; }
            set { m_helisDestroyed = value; OnPropertyChanged(); }
        }

        public float ProgressMade
        {
            get { return m_progressMade; }
            set { m_progressMade = value; OnPropertyChanged(); }
        }

        public float TotalProgressInGame
        {
            get { return m_totalProgressInGame; }
            set { m_totalProgressInGame = value; OnPropertyChanged(); }
        }

        public int KgsOfExplosivesUsed
        {
            get { return m_kgsOfExplosivesUsed; }
            set { m_kgsOfExplosivesUsed = value; OnPropertyChanged(); }
        }

        public int BulletsThatHit
        {
            get { return m_bulletsThatHit; }
            set { m_bulletsThatHit = value; OnPropertyChanged(); }
        }

        public int CarsCrushed
        {
            get { return m_carsCrushed; }
            set { m_carsCrushed = value; OnPropertyChanged(); }
        }

        public int HeadsPopped
        {
            get { return m_headsPopped; }
            set { m_headsPopped = value; OnPropertyChanged(); }
        }

        public int WantedStarsAttained
        {
            get { return m_wantedStarsAttained; }
            set { m_wantedStarsAttained = value; OnPropertyChanged(); }
        }

        public int WantedStarsEvaded
        {
            get { return m_wantedStarsEvaded; }
            set { m_wantedStarsEvaded = value; OnPropertyChanged(); }
        }

        public int TimesArrested
        {
            get { return m_timesArrested; }
            set { m_timesArrested = value; OnPropertyChanged(); }
        }

        public int TimesDied
        {
            get { return m_timesDied; }
            set { m_timesDied = value; OnPropertyChanged(); }
        }

        public int DaysPassed
        {
            get { return m_daysPassed; }
            set { m_daysPassed = value; OnPropertyChanged(); }
        }

        public int SafeHouseVisits
        {
            get { return m_safeHouseVisits; }
            set { m_safeHouseVisits = value; OnPropertyChanged(); }
        }

        public int Sprayings
        {
            get { return m_sprayings; }
            set { m_sprayings = value; OnPropertyChanged(); }
        }

        public float MaximumJumpDistance
        {
            get { return m_maximumJumpDistance; }
            set { m_maximumJumpDistance = value; OnPropertyChanged(); }
        }

        public float MaximumJumpHeight
        {
            get { return m_maximumJumpHeight; }
            set { m_maximumJumpHeight = value; OnPropertyChanged(); }
        }

        public int MaximumJumpFlips
        {
            get { return m_maximumJumpFlips; }
            set { m_maximumJumpFlips = value; OnPropertyChanged(); }
        }

        public int MaximumJumpSpins
        {
            get { return m_maximumJumpSpins; }
            set { m_maximumJumpSpins = value; OnPropertyChanged(); }
        }

        public int BestStuntJump
        {
            get { return m_bestStuntJump; }
            set { m_bestStuntJump = value; OnPropertyChanged(); }
        }

        public int NumberOfUniqueJumpsFound
        {
            get { return m_numberOfUniqueJumpsFound; }
            set { m_numberOfUniqueJumpsFound = value; OnPropertyChanged(); }
        }

        public int TotalNumberOfUniqueJumps
        {
            get { return m_totalNumberOfUniqueJumps; }
            set { m_totalNumberOfUniqueJumps = value; OnPropertyChanged(); }
        }

        public int MissionsGiven
        {
            get { return m_missionsGiven; }
            set { m_missionsGiven = value; OnPropertyChanged(); }
        }

        public int PassengersDroppedOffWithTaxi
        {
            get { return m_passengersDroppedOffWithTaxi; }
            set { m_passengersDroppedOffWithTaxi = value; OnPropertyChanged(); }
        }

        public int MoneyMadeWithTaxi
        {
            get { return m_moneyMadeWithTaxi; }
            set { m_moneyMadeWithTaxi = value; OnPropertyChanged(); }
        }

        public bool IndustrialPassed
        {
            get { return m_industrialPassed; }
            set { m_industrialPassed = value; OnPropertyChanged(); }
        }

        public bool CommercialPassed
        {
            get { return m_commercialPassed; }
            set { m_commercialPassed = value; OnPropertyChanged(); }
        }

        public bool SuburbanPassed
        {
            get { return m_suburbanPassed; }
            set { m_suburbanPassed = value; OnPropertyChanged(); }
        }

        public bool PamphletMissionPassed
        {
            get { return m_pamphletMissionPassed; }
            set { m_pamphletMissionPassed = value; OnPropertyChanged(); }
        }

        public bool NoMoreHurricanes
        {
            get { return m_noMoreHurricanes; }
            set { m_noMoreHurricanes = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledOnFoot
        {
            get { return m_distanceTravelledOnFoot; }
            set { m_distanceTravelledOnFoot = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledByCar
        {
            get { return m_distanceTravelledByCar; }
            set { m_distanceTravelledByCar = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledByBike
        {
            get { return m_distanceTravelledByBike; }
            set { m_distanceTravelledByBike = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledByBoat
        {
            get { return m_distanceTravelledByBoat; }
            set { m_distanceTravelledByBoat = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledByPlane
        {
            get { return m_distanceTravelledByPlane; }
            set { m_distanceTravelledByPlane = value; OnPropertyChanged(); }
        }

        public int LivesSavedWithAmbulance
        {
            get { return m_livesSavedWithAmbulance; }
            set { m_livesSavedWithAmbulance = value; OnPropertyChanged(); }
        }

        public int CriminalsCaught
        {
            get { return m_criminalsCaught; }
            set { m_criminalsCaught = value; OnPropertyChanged(); }
        }

        public int FiresExtinguished
        {
            get { return m_firesExtinguished; }
            set { m_firesExtinguished = value; OnPropertyChanged(); }
        }

        public int HighestLevelVigilanteMission
        {
            get { return m_highestLevelVigilanteMission; }
            set { m_highestLevelVigilanteMission = value; OnPropertyChanged(); }
        }

        public int HighestLevelAmbulanceMission
        {
            get { return m_highestLevelAmbulanceMission; }
            set { m_highestLevelAmbulanceMission = value; OnPropertyChanged(); }
        }

        public int HighestLevelFireMission
        {
            get { return m_highestLevelFireMission; }
            set { m_highestLevelFireMission = value; OnPropertyChanged(); }
        }

        public int PhotosTaken
        {
            get { return m_photosTaken; }
            set { m_photosTaken = value; OnPropertyChanged(); }
        }

        public int NumberKillFrenziesPassed
        {
            get { return m_numberKillFrenziesPassed; }
            set { m_numberKillFrenziesPassed = value; OnPropertyChanged(); }
        }

        public int MaxSecondsOnCarnageLeft
        {
            get { return m_maxSecondsOnCarnageLeft; }
            set { m_maxSecondsOnCarnageLeft = value; OnPropertyChanged(); }
        }

        public int MaxKillsOnRcTriad
        {
            get { return m_maxKillsOnRcTriad; }
            set { m_maxKillsOnRcTriad = value; OnPropertyChanged(); }
        }

        public int TotalNumberKillFrenzies
        {
            get { return m_totalNumberKillFrenzies; }
            set { m_totalNumberKillFrenzies = value; OnPropertyChanged(); }
        }

        public int TotalNumberMissions
        {
            get { return m_totalNumberMissions; }
            set { m_totalNumberMissions = value; OnPropertyChanged(); }
        }

        public int TimesDrowned
        {
            get { return m_timesDrowned; }
            set { m_timesDrowned = value; OnPropertyChanged(); }
        }

        public int SeagullsKilled
        {
            get { return m_seagullsKilled; }
            set { m_seagullsKilled = value; OnPropertyChanged(); }
        }

        public float WeaponBudget
        {
            get { return m_weaponBudget; }
            set { m_weaponBudget = value; OnPropertyChanged(); }
        }

        public int LoanSharks
        {
            get { return m_loanSharks; }
            set { m_loanSharks = value; OnPropertyChanged(); }
        }

        public int MovieStunts
        {
            get { return m_movieStunts; }
            set { m_movieStunts = value; OnPropertyChanged(); }
        }

        public float PizzasDelivered
        {
            get { return m_pizzasDelivered; }
            set { m_pizzasDelivered = value; OnPropertyChanged(); }
        }

        public float NoodlesDelivered
        {
            get { return m_noodlesDelivered; }
            set { m_noodlesDelivered = value; OnPropertyChanged(); }
        }

        public float MoneyMadeFromTourist
        {
            get { return m_moneyMadeFromTourist; }
            set { m_moneyMadeFromTourist = value; OnPropertyChanged(); }
        }

        public float TouristsTakenToSpots
        {
            get { return m_touristsTakenToSpots; }
            set { m_touristsTakenToSpots = value; OnPropertyChanged(); }
        }

        public int GarbagePickups
        {
            get { return m_garbagePickups; }
            set { m_garbagePickups = value; OnPropertyChanged(); }
        }

        public int IceCreamSold
        {
            get { return m_iceCreamSold; }
            set { m_iceCreamSold = value; OnPropertyChanged(); }
        }

        public int TopShootingRangeScore
        {
            get { return m_topShootingRangeScore; }
            set { m_topShootingRangeScore = value; OnPropertyChanged(); }
        }

        public int ShootingRank
        {
            get { return m_shootingRank; }
            set { m_shootingRank = value; OnPropertyChanged(); }
        }

        public float TopScrapyardChallengeScore
        {
            get { return m_topScrapyardChallengeScore; }
            set { m_topScrapyardChallengeScore = value; OnPropertyChanged(); }
        }

        public float Top9mmMayhemScore
        {
            get { return m_top9mmMayhemScore; }
            set { m_top9mmMayhemScore = value; OnPropertyChanged(); }
        }

        public float TopScooterShooterScore
        {
            get { return m_topScooterShooterScore; }
            set { m_topScooterShooterScore = value; OnPropertyChanged(); }
        }

        public float TopWichitaWipeoutScore
        {
            get { return m_topWichitaWipeoutScore; }
            set { m_topWichitaWipeoutScore = value; OnPropertyChanged(); }
        }

        public int LongestWheelie
        {
            get { return m_longestWheelie; }
            set { m_longestWheelie = value; OnPropertyChanged(); }
        }

        public int LongestStoppie
        {
            get { return m_longestStoppie; }
            set { m_longestStoppie = value; OnPropertyChanged(); }
        }

        public int Longest2Wheel
        {
            get { return m_longest2Wheel; }
            set { m_longest2Wheel = value; OnPropertyChanged(); }
        }

        public float LongestWheelieDist
        {
            get { return m_longestWheelieDist; }
            set { m_longestWheelieDist = value; OnPropertyChanged(); }
        }

        public float LongestStoppieDist
        {
            get { return m_longestStoppieDist; }
            set { m_longestStoppieDist = value; OnPropertyChanged(); }
        }

        public float Longest2WheelDist
        {
            get { return m_longest2WheelDist; }
            set { m_longest2WheelDist = value; OnPropertyChanged(); }
        }

        public float LongestFacePlantDist
        {
            get { return m_longestFacePlantDist; }
            set { m_longestFacePlantDist = value; OnPropertyChanged(); }
        }

        public float AutoPaintingBudget
        {
            get { return m_autoPaintingBudget; }
            set { m_autoPaintingBudget = value; OnPropertyChanged(); }
        }

        public int PropertyDestroyed
        {
            get { return m_propertyDestroyed; }
            set { m_propertyDestroyed = value; OnPropertyChanged(); }
        }

        public int NumPropertyOwned
        {
            get { return m_numPropertyOwned; }
            set { m_numPropertyOwned = value; OnPropertyChanged(); }
        }

        public PlayerOutfitFlags UnlockedCostumes
        {
            get { return m_unlockedCostumes; }
            set { m_unlockedCostumes = value; OnPropertyChanged(); }
        }

        public int BloodringKills
        {
            get { return m_bloodringKills; }
            set { m_bloodringKills = value; OnPropertyChanged(); }
        }

        public int BloodringTime
        {
            get { return m_bloodringTime; }
            set { m_bloodringTime = value; OnPropertyChanged(); }
        }

        public Array<byte> PropertyOwned
        {
            get { return m_propertyOwned; }
            set { m_propertyOwned = value; OnPropertyChanged(); }
        }

        public float HighestChaseValue
        {
            get { return m_highestChaseValue; }
            set { m_highestChaseValue = value; OnPropertyChanged(); }
        }

        public Array<int> FastestTimes
        {
            get { return m_fastestTimes; }
            set { m_fastestTimes = value; OnPropertyChanged(); }
        }

        public Array<int> HighestScores
        {
            get { return m_highestScores; }
            set { m_highestScores = value; OnPropertyChanged(); }
        }

        public int BestPositions
        {
            get { return m_bestPositions; }
            set { m_bestPositions = value; OnPropertyChanged(); }
        }

        public int KillsSinceLastCheckpoint
        {
            get { return m_killsSinceLastCheckpoint; }
            set { m_killsSinceLastCheckpoint = value; OnPropertyChanged(); }
        }

        public int TotalLegitimateKills
        {
            get { return m_totalLegitimateKills; }
            set { m_totalLegitimateKills = value; OnPropertyChanged(); }
        }

        public string LastMissionPassedName
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
        }

        public int CheatedCount
        {
            get { return m_cheatedCount; }
            set { m_cheatedCount = value; OnPropertyChanged(); }
        }

        public int CarsSold
        {
            get { return m_carsSold; }
            set { m_carsSold = value; OnPropertyChanged(); }
        }

        public int MoneyMadeWithCarSales
        {
            get { return m_moneyMadeWithCarSales; }
            set { m_moneyMadeWithCarSales = value; OnPropertyChanged(); }
        }

        public int BikesSold
        {
            get { return m_bikesSold; }
            set { m_bikesSold = value; OnPropertyChanged(); }
        }

        public int MoneyMadeWithBikeSales
        {
            get { return m_moneyMadeWithBikeSales; }
            set { m_moneyMadeWithBikeSales = value; OnPropertyChanged(); }
        }

        public int NumberOfExportedCars
        {
            get { return m_numberOfExportedCars; }
            set { m_numberOfExportedCars = value; OnPropertyChanged(); }
        }

        public int TotalNumberOfCarExport
        {
            get { return m_totalNumberOfCarExport; }
            set { m_totalNumberOfCarExport = value; OnPropertyChanged(); }
        }

        public int HighestLevelSlashTv
        {
            get { return m_highestLevelSlashTv; }
            set { m_highestLevelSlashTv = value; OnPropertyChanged(); }
        }

        public int MoneyMadeWithSlashTv
        {
            get { return m_moneyMadeWithSlashTv; }
            set { m_moneyMadeWithSlashTv = value; OnPropertyChanged(); }
        }

        public int TotalKillsOnSlashTv
        {
            get { return m_totalKillsOnSlashTv; }
            set { m_totalKillsOnSlashTv = value; OnPropertyChanged(); }
        }

        public int PackagesSmuggled
        {
            get { return m_packagesSmuggled; }
            set { m_packagesSmuggled = value; OnPropertyChanged(); }
        }

        public int SmugglersWasted
        {
            get { return m_smugglersWasted; }
            set { m_smugglersWasted = value; OnPropertyChanged(); }
        }

        public int FastestSmugglingTime
        {
            get { return m_fastestSmugglingTime; }
            set { m_fastestSmugglingTime = value; OnPropertyChanged(); }
        }

        public int MoneyMadeInCoach
        {
            get { return m_moneyMadeInCoach; }
            set { m_moneyMadeInCoach = value; OnPropertyChanged(); }
        }

        public int CashMadeCollectingTrash
        {
            get { return m_cashMadeCollectingTrash; }
            set { m_cashMadeCollectingTrash = value; OnPropertyChanged(); }
        }

        public int HitmenKilled
        {
            get { return m_hitmenKilled; }
            set { m_hitmenKilled = value; OnPropertyChanged(); }
        }

        public int HighestGuardianAngelJusticeDished
        {
            get { return m_highestGuardianAngelJusticeDished; }
            set { m_highestGuardianAngelJusticeDished = value; OnPropertyChanged(); }
        }

        public int GuardianAngelMissionsPassed
        {
            get { return m_guardianAngelMissionsPassed; }
            set { m_guardianAngelMissionsPassed = value; OnPropertyChanged(); }
        }

        public int GuardianAngelHighestLevelInd
        {
            get { return m_guardianAngelHighestLevelInd; }
            set { m_guardianAngelHighestLevelInd = value; OnPropertyChanged(); }
        }

        public int GuardianAngelHighestLevelCom
        {
            get { return m_guardianAngelHighestLevelCom; }
            set { m_guardianAngelHighestLevelCom = value; OnPropertyChanged(); }
        }

        public int GuardianAngelHighestLevelSub
        {
            get { return m_guardianAngelHighestLevelSub; }
            set { m_guardianAngelHighestLevelSub = value; OnPropertyChanged(); }
        }

        public int MostTimeLeftTrainRace
        {
            get { return m_mostTimeLeftTrainRace; }
            set { m_mostTimeLeftTrainRace = value; OnPropertyChanged(); }
        }

        public int BestTimeGoGoFaggio
        {
            get { return m_bestTimeGoGoFaggio; }
            set { m_bestTimeGoGoFaggio = value; OnPropertyChanged(); }
        }

        public int DirtBikeMostAir
        {
            get { return m_dirtBikeMostAir; }
            set { m_dirtBikeMostAir = value; OnPropertyChanged(); }
        }

        public int HighestTrainCashEarned
        {
            get { return m_highestTrainCashEarned; }
            set { m_highestTrainCashEarned = value; OnPropertyChanged(); }
        }

        public int FastestHeliRaceTime
        {
            get { return m_fastestHeliRaceTime; }
            set { m_fastestHeliRaceTime = value; OnPropertyChanged(); }
        }

        public int BestHeliRacePosition
        {
            get { return m_bestHeliRacePosition; }
            set { m_bestHeliRacePosition = value; OnPropertyChanged(); }
        }

        public int NumberOutfitChanges
        {
            get { return m_numberOutfitChanges; }
            set { m_numberOutfitChanges = value; OnPropertyChanged(); }
        }

        public Array<int> BestBanditLapTimes
        {
            get { return m_bestBanditLapTimes; }
            set { m_bestBanditLapTimes = value; OnPropertyChanged(); }
        }

        public Array<int> BestBanditPositions
        {
            get { return m_bestBanditPositions; }
            set { m_bestBanditPositions = value; OnPropertyChanged(); }
        }

        public Array<int> BestStreetRacePositions
        {
            get { return m_bestStreetRacePositions; }
            set { m_bestStreetRacePositions = value; OnPropertyChanged(); }
        }

        public Array<int> FastestStreetRaceLapTimes
        {
            get { return m_fastestStreetRaceLapTimes; }
            set { m_fastestStreetRaceLapTimes = value; OnPropertyChanged(); }
        }

        public Array<int> FastestStreetRaceTimes
        {
            get { return m_fastestStreetRaceTimes; }
            set { m_fastestStreetRaceTimes = value; OnPropertyChanged(); }
        }

        public Array<int> FastestDirtBikeLapTimes
        {
            get { return m_fastestDirtBikeLapTimes; }
            set { m_fastestDirtBikeLapTimes = value; OnPropertyChanged(); }
        }

        public Array<int> FastestDirtBikeTimes
        {
            get { return m_fastestDirtBikeTimes; }
            set { m_fastestDirtBikeTimes = value; OnPropertyChanged(); }
        }

        public Array<float> FavoriteRadioStationList
        {
            get { return m_favoriteRadioStationList; }
            set { m_favoriteRadioStationList = value; OnPropertyChanged(); }
        }

        public Stats()
        {
            PedsKilledOfThisType = ArrayHelper.CreateArray<int>(NumPedTypes);
            PropertyOwned = ArrayHelper.CreateArray<byte>(NumProperties);
            FastestTimes = ArrayHelper.CreateArray<int>(NumFastestTimes);
            HighestScores = ArrayHelper.CreateArray<int>(NumHighestScores);
            BestBanditLapTimes = ArrayHelper.CreateArray<int>(NumBanditRaces);
            BestBanditPositions = ArrayHelper.CreateArray<int>(NumBanditRaces);
            BestStreetRacePositions = ArrayHelper.CreateArray<int>(NumStreetRaces);
            FastestStreetRaceLapTimes = ArrayHelper.CreateArray<int>(NumStreetRaces);
            FastestStreetRaceTimes = ArrayHelper.CreateArray<int>(NumStreetRaces);
            FastestDirtBikeLapTimes = ArrayHelper.CreateArray<int>(NumDirtBikeRaces);
            FastestDirtBikeTimes = ArrayHelper.CreateArray<int>(NumDirtBikeRaces);
            FavoriteRadioStationList = ArrayHelper.CreateArray<float>(NumRadioStationsMobile);
            LastMissionPassedName = "";
        }

        public Stats(Stats other)
        {
            PeopleKilledByPlayer = other.PeopleKilledByPlayer;
            PeopleKilledByOthers = other.PeopleKilledByOthers;
            CarsExploded = other.CarsExploded;
            BoatsExploded = other.BoatsExploded;
            TyresPopped = other.TyresPopped;
            RoundsFiredByPlayer = other.RoundsFiredByPlayer;
            PedsKilledOfThisType = ArrayHelper.DeepClone(other.PedsKilledOfThisType);
            HelisDestroyed = other.HelisDestroyed;
            ProgressMade = other.ProgressMade;
            TotalProgressInGame = other.TotalProgressInGame;
            KgsOfExplosivesUsed = other.KgsOfExplosivesUsed;
            BulletsThatHit = other.BulletsThatHit;
            CarsCrushed = other.CarsCrushed;
            HeadsPopped = other.HeadsPopped;
            WantedStarsAttained = other.WantedStarsAttained;
            WantedStarsEvaded = other.WantedStarsEvaded;
            TimesArrested = other.TimesArrested;
            TimesDied = other.TimesDied;
            DaysPassed = other.DaysPassed;
            SafeHouseVisits = other.SafeHouseVisits;
            Sprayings = other.Sprayings;
            MaximumJumpDistance = other.MaximumJumpDistance;
            MaximumJumpHeight = other.MaximumJumpHeight;
            MaximumJumpFlips = other.MaximumJumpFlips;
            MaximumJumpSpins = other.MaximumJumpSpins;
            BestStuntJump = other.BestStuntJump;
            NumberOfUniqueJumpsFound = other.NumberOfUniqueJumpsFound;
            TotalNumberOfUniqueJumps = other.TotalNumberOfUniqueJumps;
            MissionsGiven = other.MissionsGiven;
            PassengersDroppedOffWithTaxi = other.PassengersDroppedOffWithTaxi;
            MoneyMadeWithTaxi = other.MoneyMadeWithTaxi;
            IndustrialPassed = other.IndustrialPassed;
            CommercialPassed = other.CommercialPassed;
            SuburbanPassed = other.SuburbanPassed;
            PamphletMissionPassed = other.PamphletMissionPassed;
            NoMoreHurricanes = other.NoMoreHurricanes;
            DistanceTravelledOnFoot = other.DistanceTravelledOnFoot;
            DistanceTravelledByCar = other.DistanceTravelledByCar;
            DistanceTravelledByBike = other.DistanceTravelledByBike;
            DistanceTravelledByBoat = other.DistanceTravelledByBoat;
            DistanceTravelledByPlane = other.DistanceTravelledByPlane;
            LivesSavedWithAmbulance = other.LivesSavedWithAmbulance;
            CriminalsCaught = other.CriminalsCaught;
            FiresExtinguished = other.FiresExtinguished;
            HighestLevelVigilanteMission = other.HighestLevelVigilanteMission;
            HighestLevelAmbulanceMission = other.HighestLevelAmbulanceMission;
            HighestLevelFireMission = other.HighestLevelFireMission;
            PhotosTaken = other.PhotosTaken;
            NumberKillFrenziesPassed = other.NumberKillFrenziesPassed;
            MaxSecondsOnCarnageLeft = other.MaxSecondsOnCarnageLeft;
            MaxKillsOnRcTriad = other.MaxKillsOnRcTriad;
            TotalNumberKillFrenzies = other.TotalNumberKillFrenzies;
            TotalNumberMissions = other.TotalNumberMissions;
            TimesDrowned = other.TimesDrowned;
            SeagullsKilled = other.SeagullsKilled;
            WeaponBudget = other.WeaponBudget;
            LoanSharks = other.LoanSharks;
            MovieStunts = other.MovieStunts;
            PizzasDelivered = other.PizzasDelivered;
            NoodlesDelivered = other.NoodlesDelivered;
            MoneyMadeFromTourist = other.MoneyMadeFromTourist;
            TouristsTakenToSpots = other.TouristsTakenToSpots;
            GarbagePickups = other.GarbagePickups;
            IceCreamSold = other.IceCreamSold;
            TopShootingRangeScore = other.TopShootingRangeScore;
            ShootingRank = other.ShootingRank;
            TopScrapyardChallengeScore = other.TopScrapyardChallengeScore;
            Top9mmMayhemScore = other.Top9mmMayhemScore;
            TopScooterShooterScore = other.TopScooterShooterScore;
            TopWichitaWipeoutScore = other.TopWichitaWipeoutScore;
            LongestWheelie = other.LongestWheelie;
            LongestStoppie = other.LongestStoppie;
            Longest2Wheel = other.Longest2Wheel;
            LongestWheelieDist = other.LongestWheelieDist;
            LongestStoppieDist = other.LongestStoppieDist;
            Longest2WheelDist = other.Longest2WheelDist;
            LongestFacePlantDist = other.LongestFacePlantDist;
            AutoPaintingBudget = other.AutoPaintingBudget;
            PropertyDestroyed = other.PropertyDestroyed;
            NumPropertyOwned = other.NumPropertyOwned;
            UnlockedCostumes = other.UnlockedCostumes;
            BloodringKills = other.BloodringKills;
            BloodringTime = other.BloodringTime;
            PropertyOwned = ArrayHelper.DeepClone(other.PropertyOwned);
            HighestChaseValue = other.HighestChaseValue;
            FastestTimes = ArrayHelper.DeepClone(other.FastestTimes);
            HighestScores = ArrayHelper.DeepClone(other.HighestScores);
            BestPositions = other.BestPositions;
            KillsSinceLastCheckpoint = other.KillsSinceLastCheckpoint;
            TotalLegitimateKills = other.TotalLegitimateKills;
            LastMissionPassedName = other.LastMissionPassedName;
            CheatedCount = other.CheatedCount;
            CarsSold = other.CarsSold;
            MoneyMadeWithCarSales = other.MoneyMadeWithCarSales;
            BikesSold = other.BikesSold;
            MoneyMadeWithBikeSales = other.MoneyMadeWithBikeSales;
            NumberOfExportedCars = other.NumberOfExportedCars;
            TotalNumberOfCarExport = other.TotalNumberOfCarExport;
            HighestLevelSlashTv = other.HighestLevelSlashTv;
            MoneyMadeWithSlashTv = other.MoneyMadeWithSlashTv;
            TotalKillsOnSlashTv = other.TotalKillsOnSlashTv;
            PackagesSmuggled = other.PackagesSmuggled;
            SmugglersWasted = other.SmugglersWasted;
            FastestSmugglingTime = other.FastestSmugglingTime;
            MoneyMadeInCoach = other.MoneyMadeInCoach;
            CashMadeCollectingTrash = other.CashMadeCollectingTrash;
            HitmenKilled = other.HitmenKilled;
            HighestGuardianAngelJusticeDished = other.HighestGuardianAngelJusticeDished;
            GuardianAngelMissionsPassed = other.GuardianAngelMissionsPassed;
            GuardianAngelHighestLevelInd = other.GuardianAngelHighestLevelInd;
            GuardianAngelHighestLevelCom = other.GuardianAngelHighestLevelCom;
            GuardianAngelHighestLevelSub = other.GuardianAngelHighestLevelSub;
            MostTimeLeftTrainRace = other.MostTimeLeftTrainRace;
            BestTimeGoGoFaggio = other.BestTimeGoGoFaggio;
            HighestTrainCashEarned = other.HighestTrainCashEarned;
            DirtBikeMostAir = other.DirtBikeMostAir;
            FastestHeliRaceTime = other.FastestHeliRaceTime;
            BestHeliRacePosition = other.BestHeliRacePosition;
            NumberOutfitChanges = other.NumberOutfitChanges;
            BestBanditLapTimes = ArrayHelper.DeepClone(other.BestBanditLapTimes);
            BestBanditPositions = ArrayHelper.DeepClone(other.BestBanditPositions);
            BestStreetRacePositions = ArrayHelper.DeepClone(other.BestStreetRacePositions);
            FastestStreetRaceLapTimes = ArrayHelper.DeepClone(other.FastestStreetRaceLapTimes);
            FastestStreetRaceTimes = ArrayHelper.DeepClone(other.FastestStreetRaceTimes);
            FastestDirtBikeLapTimes = ArrayHelper.DeepClone(other.FastestDirtBikeLapTimes);
            FastestDirtBikeTimes = ArrayHelper.DeepClone(other.FastestDirtBikeTimes);
            FavoriteRadioStationList = ArrayHelper.DeepClone(other.FavoriteRadioStationList);
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            PeopleKilledByPlayer = buf.ReadInt32();
            PeopleKilledByOthers = buf.ReadInt32();
            CarsExploded = buf.ReadInt32();
            BoatsExploded = buf.ReadInt32();
            TyresPopped = buf.ReadInt32();
            RoundsFiredByPlayer = buf.ReadInt32();
            PedsKilledOfThisType = buf.ReadArray<int>(NumPedTypes);
            HelisDestroyed = buf.ReadInt32();
            ProgressMade = buf.ReadFloat();
            TotalProgressInGame = buf.ReadFloat();
            KgsOfExplosivesUsed = buf.ReadInt32();
            BulletsThatHit = buf.ReadInt32();
            CarsCrushed = buf.ReadInt32();
            HeadsPopped = buf.ReadInt32();
            WantedStarsAttained = buf.ReadInt32();
            WantedStarsEvaded = buf.ReadInt32();
            TimesArrested = buf.ReadInt32();
            TimesDied = buf.ReadInt32();
            DaysPassed = buf.ReadInt32();
            SafeHouseVisits = buf.ReadInt32();
            Sprayings = buf.ReadInt32();
            MaximumJumpDistance = buf.ReadFloat();
            MaximumJumpHeight = buf.ReadFloat();
            MaximumJumpFlips = buf.ReadInt32();
            MaximumJumpSpins = buf.ReadInt32();
            BestStuntJump = buf.ReadInt32();
            NumberOfUniqueJumpsFound = buf.ReadInt32();
            TotalNumberOfUniqueJumps = buf.ReadInt32();
            MissionsGiven = buf.ReadInt32();
            PassengersDroppedOffWithTaxi = buf.ReadInt32();
            MoneyMadeWithTaxi = buf.ReadInt32();
            IndustrialPassed = buf.ReadBool(4);
            CommercialPassed = buf.ReadBool(4);
            SuburbanPassed = buf.ReadBool(4);
            PamphletMissionPassed = buf.ReadBool(4);
            NoMoreHurricanes = buf.ReadBool(4);
            DistanceTravelledOnFoot = buf.ReadFloat();
            DistanceTravelledByCar = buf.ReadFloat();
            DistanceTravelledByBike = buf.ReadFloat();
            DistanceTravelledByBoat = buf.ReadFloat();
            DistanceTravelledByPlane = buf.ReadFloat();
            LivesSavedWithAmbulance = buf.ReadInt32();
            CriminalsCaught = buf.ReadInt32();
            FiresExtinguished = buf.ReadInt32();
            HighestLevelVigilanteMission = buf.ReadInt32();
            HighestLevelAmbulanceMission = buf.ReadInt32();
            HighestLevelFireMission = buf.ReadInt32();
            PhotosTaken = buf.ReadInt32();
            NumberKillFrenziesPassed = buf.ReadInt32();
            MaxSecondsOnCarnageLeft = buf.ReadInt32();
            MaxKillsOnRcTriad = buf.ReadInt32();
            TotalNumberKillFrenzies = buf.ReadInt32();
            TotalNumberMissions = buf.ReadInt32();
            TimesDrowned = buf.ReadInt32();
            SeagullsKilled = buf.ReadInt32();
            WeaponBudget = buf.ReadFloat();
            LoanSharks = buf.ReadInt32();
            MovieStunts = buf.ReadInt32();
            PizzasDelivered = buf.ReadFloat();
            NoodlesDelivered = buf.ReadFloat();
            MoneyMadeFromTourist = buf.ReadFloat();
            TouristsTakenToSpots = buf.ReadFloat();
            GarbagePickups = buf.ReadInt32();
            IceCreamSold = buf.ReadInt32();
            TopShootingRangeScore = buf.ReadInt32();
            ShootingRank = buf.ReadInt32();
            TopScrapyardChallengeScore = buf.ReadFloat();
            Top9mmMayhemScore = buf.ReadFloat();
            TopScooterShooterScore = buf.ReadFloat();
            TopWichitaWipeoutScore = buf.ReadFloat();
            LongestWheelie = buf.ReadInt32();
            LongestStoppie = buf.ReadInt32();
            Longest2Wheel = buf.ReadInt32();
            LongestWheelieDist = buf.ReadFloat();
            LongestStoppieDist = buf.ReadFloat();
            Longest2WheelDist = buf.ReadFloat();
            LongestFacePlantDist = buf.ReadFloat();
            AutoPaintingBudget = buf.ReadFloat();
            PropertyDestroyed = buf.ReadInt32();
            NumPropertyOwned = buf.ReadInt32();
            UnlockedCostumes = (PlayerOutfitFlags) buf.ReadUInt16();
            BloodringKills = buf.ReadInt32();
            BloodringTime = buf.ReadInt32();
            PropertyOwned = buf.ReadArray<byte>(NumProperties);
            HighestChaseValue = buf.ReadFloat();
            FastestTimes = buf.ReadArray<int>(NumFastestTimes);
            HighestScores = buf.ReadArray<int>(NumHighestScores);
            BestPositions = buf.ReadInt32();
            KillsSinceLastCheckpoint = buf.ReadInt32();
            TotalLegitimateKills = buf.ReadInt32();
            LastMissionPassedName = buf.ReadString(LastMissionPassedNameLength);
            CheatedCount = buf.ReadInt32();
            CarsSold = buf.ReadInt32();
            MoneyMadeWithCarSales = buf.ReadInt32();
            BikesSold = buf.ReadInt32();
            MoneyMadeWithBikeSales = buf.ReadInt32();
            NumberOfExportedCars = buf.ReadInt32();
            TotalNumberOfCarExport = buf.ReadInt32();
            HighestLevelSlashTv = buf.ReadInt32();
            MoneyMadeWithSlashTv = buf.ReadInt32();
            TotalKillsOnSlashTv = buf.ReadInt32();
            PackagesSmuggled = buf.ReadInt32();
            SmugglersWasted = buf.ReadInt32();
            FastestSmugglingTime = buf.ReadInt32();
            MoneyMadeInCoach = buf.ReadInt32();
            CashMadeCollectingTrash = buf.ReadInt32();
            HitmenKilled = buf.ReadInt32();
            HighestGuardianAngelJusticeDished = buf.ReadInt32();
            GuardianAngelMissionsPassed = buf.ReadInt32();
            GuardianAngelHighestLevelInd = buf.ReadInt32();
            GuardianAngelHighestLevelCom = buf.ReadInt32();
            GuardianAngelHighestLevelSub = buf.ReadInt32();
            MostTimeLeftTrainRace = buf.ReadInt32();
            BestTimeGoGoFaggio = buf.ReadInt32();
            DirtBikeMostAir = buf.ReadInt32();
            HighestTrainCashEarned = buf.ReadInt32();
            FastestHeliRaceTime = buf.ReadInt32();
            BestHeliRacePosition = buf.ReadInt32();
            NumberOutfitChanges = buf.ReadInt32();
            BestBanditLapTimes = buf.ReadArray<int>(NumBanditRaces);
            BestBanditPositions = buf.ReadArray<int>(NumBanditRaces);
            BestStreetRacePositions = buf.ReadArray<int>(NumStreetRaces);
            FastestStreetRaceLapTimes = buf.ReadArray<int>(NumStreetRaces);
            FastestStreetRaceTimes = buf.ReadArray<int>(NumStreetRaces);
            FastestDirtBikeLapTimes = buf.ReadArray<int>(NumDirtBikeRaces);
            FastestDirtBikeTimes = buf.ReadArray<int>(NumDirtBikeRaces);
            FavoriteRadioStationList = buf.ReadArray<float>((fmt.IsMobile) ? NumRadioStationsMobile : NumRadioStations);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(PeopleKilledByPlayer);
            buf.Write(PeopleKilledByOthers);
            buf.Write(CarsExploded);
            buf.Write(BoatsExploded);
            buf.Write(TyresPopped);
            buf.Write(RoundsFiredByPlayer);
            buf.Write(PedsKilledOfThisType, NumPedTypes);
            buf.Write(HelisDestroyed);
            buf.Write(ProgressMade);
            buf.Write(TotalProgressInGame);
            buf.Write(KgsOfExplosivesUsed);
            buf.Write(BulletsThatHit);
            buf.Write(CarsCrushed);
            buf.Write(HeadsPopped);
            buf.Write(WantedStarsAttained);
            buf.Write(WantedStarsEvaded);
            buf.Write(TimesArrested);
            buf.Write(TimesDied);
            buf.Write(DaysPassed);
            buf.Write(SafeHouseVisits);
            buf.Write(Sprayings);
            buf.Write(MaximumJumpDistance);
            buf.Write(MaximumJumpHeight);
            buf.Write(MaximumJumpFlips);
            buf.Write(MaximumJumpSpins);
            buf.Write(BestStuntJump);
            buf.Write(NumberOfUniqueJumpsFound);
            buf.Write(TotalNumberOfUniqueJumps);
            buf.Write(MissionsGiven);
            buf.Write(PassengersDroppedOffWithTaxi);
            buf.Write(MoneyMadeWithTaxi);
            buf.Write(IndustrialPassed, 4);
            buf.Write(CommercialPassed, 4);
            buf.Write(SuburbanPassed, 4);
            buf.Write(PamphletMissionPassed, 4);
            buf.Write(NoMoreHurricanes, 4);
            buf.Write(DistanceTravelledOnFoot);
            buf.Write(DistanceTravelledByCar);
            buf.Write(DistanceTravelledByBike);
            buf.Write(DistanceTravelledByBoat);
            buf.Write(DistanceTravelledByPlane);
            buf.Write(LivesSavedWithAmbulance);
            buf.Write(CriminalsCaught);
            buf.Write(FiresExtinguished);
            buf.Write(HighestLevelVigilanteMission);
            buf.Write(HighestLevelAmbulanceMission);
            buf.Write(HighestLevelFireMission);
            buf.Write(PhotosTaken);
            buf.Write(NumberKillFrenziesPassed);
            buf.Write(MaxSecondsOnCarnageLeft);
            buf.Write(MaxKillsOnRcTriad);
            buf.Write(TotalNumberKillFrenzies);
            buf.Write(TotalNumberMissions);
            buf.Write(TimesDrowned);
            buf.Write(SeagullsKilled);
            buf.Write(WeaponBudget);
            buf.Write(LoanSharks);
            buf.Write(MovieStunts);
            buf.Write(PizzasDelivered);
            buf.Write(NoodlesDelivered);
            buf.Write(MoneyMadeFromTourist);
            buf.Write(TouristsTakenToSpots);
            buf.Write(GarbagePickups);
            buf.Write(IceCreamSold);
            buf.Write(TopShootingRangeScore);
            buf.Write(ShootingRank);
            buf.Write(TopScrapyardChallengeScore);
            buf.Write(Top9mmMayhemScore);
            buf.Write(TopScooterShooterScore);
            buf.Write(TopWichitaWipeoutScore);
            buf.Write(LongestWheelie);
            buf.Write(LongestStoppie);
            buf.Write(Longest2Wheel);
            buf.Write(LongestWheelieDist);
            buf.Write(LongestStoppieDist);
            buf.Write(Longest2WheelDist);
            buf.Write(LongestFacePlantDist);
            buf.Write(AutoPaintingBudget);
            buf.Write(PropertyDestroyed);
            buf.Write(NumPropertyOwned);
            buf.Write((ushort) UnlockedCostumes);
            buf.Write(BloodringKills);
            buf.Write(BloodringTime);
            buf.Write(PropertyOwned, NumProperties);
            buf.Write(HighestChaseValue);
            buf.Write(FastestTimes, NumFastestTimes);
            buf.Write(HighestScores, NumHighestScores);
            buf.Write(BestPositions);
            buf.Write(KillsSinceLastCheckpoint);
            buf.Write(TotalLegitimateKills);
            buf.Write(LastMissionPassedName, LastMissionPassedNameLength);
            buf.Write(CheatedCount);
            buf.Write(CarsSold);
            buf.Write(MoneyMadeWithCarSales);
            buf.Write(BikesSold);
            buf.Write(MoneyMadeWithBikeSales);
            buf.Write(NumberOfExportedCars);
            buf.Write(TotalNumberOfCarExport);
            buf.Write(HighestLevelSlashTv);
            buf.Write(MoneyMadeWithSlashTv);
            buf.Write(TotalKillsOnSlashTv);
            buf.Write(PackagesSmuggled);
            buf.Write(SmugglersWasted);
            buf.Write(FastestSmugglingTime);
            buf.Write(MoneyMadeInCoach);
            buf.Write(CashMadeCollectingTrash);
            buf.Write(HitmenKilled);
            buf.Write(HighestGuardianAngelJusticeDished);
            buf.Write(GuardianAngelMissionsPassed);
            buf.Write(GuardianAngelHighestLevelInd);
            buf.Write(GuardianAngelHighestLevelCom);
            buf.Write(GuardianAngelHighestLevelSub);
            buf.Write(MostTimeLeftTrainRace);
            buf.Write(BestTimeGoGoFaggio);
            buf.Write(DirtBikeMostAir);
            buf.Write(HighestTrainCashEarned);
            buf.Write(FastestHeliRaceTime);
            buf.Write(BestHeliRacePosition);
            buf.Write(NumberOutfitChanges);
            buf.Write(BestBanditLapTimes, NumBanditRaces);
            buf.Write(BestBanditPositions, NumBanditRaces);
            buf.Write(BestStreetRacePositions, NumStreetRaces);
            buf.Write(FastestStreetRaceLapTimes, NumStreetRaces);
            buf.Write(FastestStreetRaceTimes, NumStreetRaces);
            buf.Write(FastestDirtBikeLapTimes, NumDirtBikeRaces);
            buf.Write(FastestDirtBikeTimes, NumDirtBikeRaces);
            buf.Write(FavoriteRadioStationList, (fmt.IsMobile) ? NumRadioStationsMobile : NumRadioStations);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (fmt.IsMobile) return 0x385;
            if (fmt.IsPSP || fmt.IsPS2) return 0x381;
            throw SizeNotDefined(fmt);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Stats);
        }

        public bool Equals(Stats other)
        {
            if (other == null)
            {
                return false;
            }

            return PeopleKilledByPlayer.Equals(other.PeopleKilledByPlayer)
                && PeopleKilledByOthers.Equals(other.PeopleKilledByOthers)
                && CarsExploded.Equals(other.CarsExploded)
                && BoatsExploded.Equals(other.BoatsExploded)
                && TyresPopped.Equals(other.TyresPopped)
                && RoundsFiredByPlayer.Equals(other.RoundsFiredByPlayer)
                && PedsKilledOfThisType.SequenceEqual(other.PedsKilledOfThisType)
                && HelisDestroyed.Equals(other.HelisDestroyed)
                && ProgressMade.Equals(other.ProgressMade)
                && TotalProgressInGame.Equals(other.TotalProgressInGame)
                && KgsOfExplosivesUsed.Equals(other.KgsOfExplosivesUsed)
                && BulletsThatHit.Equals(other.BulletsThatHit)
                && CarsCrushed.Equals(other.CarsCrushed)
                && HeadsPopped.Equals(other.HeadsPopped)
                && WantedStarsAttained.Equals(other.WantedStarsAttained)
                && WantedStarsEvaded.Equals(other.WantedStarsEvaded)
                && TimesArrested.Equals(other.TimesArrested)
                && TimesDied.Equals(other.TimesDied)
                && DaysPassed.Equals(other.DaysPassed)
                && SafeHouseVisits.Equals(other.SafeHouseVisits)
                && Sprayings.Equals(other.Sprayings)
                && MaximumJumpDistance.Equals(other.MaximumJumpDistance)
                && MaximumJumpHeight.Equals(other.MaximumJumpHeight)
                && MaximumJumpFlips.Equals(other.MaximumJumpFlips)
                && MaximumJumpSpins.Equals(other.MaximumJumpSpins)
                && BestStuntJump.Equals(other.BestStuntJump)
                && NumberOfUniqueJumpsFound.Equals(other.NumberOfUniqueJumpsFound)
                && TotalNumberOfUniqueJumps.Equals(other.TotalNumberOfUniqueJumps)
                && MissionsGiven.Equals(other.MissionsGiven)
                && PassengersDroppedOffWithTaxi.Equals(other.PassengersDroppedOffWithTaxi)
                && MoneyMadeWithTaxi.Equals(other.MoneyMadeWithTaxi)
                && IndustrialPassed.Equals(other.IndustrialPassed)
                && CommercialPassed.Equals(other.CommercialPassed)
                && SuburbanPassed.Equals(other.SuburbanPassed)
                && PamphletMissionPassed.Equals(other.PamphletMissionPassed)
                && NoMoreHurricanes.Equals(other.NoMoreHurricanes)
                && DistanceTravelledOnFoot.Equals(other.DistanceTravelledOnFoot)
                && DistanceTravelledByCar.Equals(other.DistanceTravelledByCar)
                && DistanceTravelledByBike.Equals(other.DistanceTravelledByBike)
                && DistanceTravelledByBoat.Equals(other.DistanceTravelledByBoat)
                && DistanceTravelledByPlane.Equals(other.DistanceTravelledByPlane)
                && LivesSavedWithAmbulance.Equals(other.LivesSavedWithAmbulance)
                && CriminalsCaught.Equals(other.CriminalsCaught)
                && FiresExtinguished.Equals(other.FiresExtinguished)
                && HighestLevelVigilanteMission.Equals(other.HighestLevelVigilanteMission)
                && HighestLevelAmbulanceMission.Equals(other.HighestLevelAmbulanceMission)
                && HighestLevelFireMission.Equals(other.HighestLevelFireMission)
                && PhotosTaken.Equals(other.PhotosTaken)
                && NumberKillFrenziesPassed.Equals(other.NumberKillFrenziesPassed)
                && MaxSecondsOnCarnageLeft.Equals(other.MaxSecondsOnCarnageLeft)
                && MaxKillsOnRcTriad.Equals(other.MaxKillsOnRcTriad)
                && TotalNumberKillFrenzies.Equals(other.TotalNumberKillFrenzies)
                && TotalNumberMissions.Equals(other.TotalNumberMissions)
                && TimesDrowned.Equals(other.TimesDrowned)
                && SeagullsKilled.Equals(other.SeagullsKilled)
                && WeaponBudget.Equals(other.WeaponBudget)
                && LoanSharks.Equals(other.LoanSharks)
                && MovieStunts.Equals(other.MovieStunts)
                && PizzasDelivered.Equals(other.PizzasDelivered)
                && NoodlesDelivered.Equals(other.NoodlesDelivered)
                && MoneyMadeFromTourist.Equals(other.MoneyMadeFromTourist)
                && TouristsTakenToSpots.Equals(other.TouristsTakenToSpots)
                && GarbagePickups.Equals(other.GarbagePickups)
                && IceCreamSold.Equals(other.IceCreamSold)
                && TopShootingRangeScore.Equals(other.TopShootingRangeScore)
                && ShootingRank.Equals(other.ShootingRank)
                && TopScrapyardChallengeScore.Equals(other.TopScrapyardChallengeScore)
                && Top9mmMayhemScore.Equals(other.Top9mmMayhemScore)
                && TopScooterShooterScore.Equals(other.TopScooterShooterScore)
                && TopWichitaWipeoutScore.Equals(other.TopWichitaWipeoutScore)
                && LongestWheelie.Equals(other.LongestWheelie)
                && LongestStoppie.Equals(other.LongestStoppie)
                && Longest2Wheel.Equals(other.Longest2Wheel)
                && LongestWheelieDist.Equals(other.LongestWheelieDist)
                && LongestStoppieDist.Equals(other.LongestStoppieDist)
                && Longest2WheelDist.Equals(other.Longest2WheelDist)
                && LongestFacePlantDist.Equals(other.LongestFacePlantDist)
                && AutoPaintingBudget.Equals(other.AutoPaintingBudget)
                && PropertyDestroyed.Equals(other.PropertyDestroyed)
                && NumPropertyOwned.Equals(other.NumPropertyOwned)
                && UnlockedCostumes.Equals(other.UnlockedCostumes)
                && BloodringKills.Equals(other.BloodringKills)
                && BloodringTime.Equals(other.BloodringTime)
                && PropertyOwned.SequenceEqual(other.PropertyOwned)
                && HighestChaseValue.Equals(other.HighestChaseValue)
                && FastestTimes.SequenceEqual(other.FastestTimes)
                && HighestScores.SequenceEqual(other.HighestScores)
                && BestPositions.Equals(other.BestPositions)
                && KillsSinceLastCheckpoint.Equals(other.KillsSinceLastCheckpoint)
                && TotalLegitimateKills.Equals(other.TotalLegitimateKills)
                && LastMissionPassedName.Equals(other.LastMissionPassedName)
                && CheatedCount.Equals(other.CheatedCount)
                && CarsSold.Equals(other.CarsSold)
                && MoneyMadeWithCarSales.Equals(other.MoneyMadeWithCarSales)
                && BikesSold.Equals(other.BikesSold)
                && MoneyMadeWithBikeSales.Equals(other.MoneyMadeWithBikeSales)
                && NumberOfExportedCars.Equals(other.NumberOfExportedCars)
                && TotalNumberOfCarExport.Equals(other.TotalNumberOfCarExport)
                && HighestLevelSlashTv.Equals(other.HighestLevelSlashTv)
                && MoneyMadeWithSlashTv.Equals(other.MoneyMadeWithSlashTv)
                && TotalKillsOnSlashTv.Equals(other.TotalKillsOnSlashTv)
                && PackagesSmuggled.Equals(other.PackagesSmuggled)
                && SmugglersWasted.Equals(other.SmugglersWasted)
                && FastestSmugglingTime.Equals(other.FastestSmugglingTime)
                && MoneyMadeInCoach.Equals(other.MoneyMadeInCoach)
                && CashMadeCollectingTrash.Equals(other.CashMadeCollectingTrash)
                && HitmenKilled.Equals(other.HitmenKilled)
                && HighestGuardianAngelJusticeDished.Equals(other.HighestGuardianAngelJusticeDished)
                && GuardianAngelMissionsPassed.Equals(other.GuardianAngelMissionsPassed)
                && GuardianAngelHighestLevelInd.Equals(other.GuardianAngelHighestLevelInd)
                && GuardianAngelHighestLevelCom.Equals(other.GuardianAngelHighestLevelCom)
                && GuardianAngelHighestLevelSub.Equals(other.GuardianAngelHighestLevelSub)
                && MostTimeLeftTrainRace.Equals(other.MostTimeLeftTrainRace)
                && BestTimeGoGoFaggio.Equals(other.BestTimeGoGoFaggio)
                && HighestTrainCashEarned.Equals(other.HighestTrainCashEarned)
                && DirtBikeMostAir.Equals(other.DirtBikeMostAir)
                && FastestHeliRaceTime.Equals(other.FastestHeliRaceTime)
                && BestHeliRacePosition.Equals(other.BestHeliRacePosition)
                && NumberOutfitChanges.Equals(other.NumberOutfitChanges)
                && BestBanditLapTimes.SequenceEqual(other.BestBanditLapTimes)
                && BestBanditPositions.SequenceEqual(other.BestBanditPositions)
                && BestStreetRacePositions.SequenceEqual(other.BestStreetRacePositions)
                && FastestStreetRaceLapTimes.SequenceEqual(other.FastestStreetRaceLapTimes)
                && FastestStreetRaceTimes.SequenceEqual(other.FastestStreetRaceTimes)
                && FastestDirtBikeLapTimes.SequenceEqual(other.FastestDirtBikeLapTimes)
                && FastestDirtBikeTimes.SequenceEqual(other.FastestDirtBikeTimes)
                && FavoriteRadioStationList.SequenceEqual(other.FavoriteRadioStationList);
        }

        public Stats DeepClone()
        {
            return new Stats(this);
        }
    }
}
