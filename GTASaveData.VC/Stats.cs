using System;
using System.Diagnostics;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData.VC
{
    public class Stats : SaveDataObject, IStats,
        IEquatable<Stats>, IDeepClonable<Stats>
    {
        public const int NumPedTypes = 23;
        public const int NumProperties = 15;
        public const int NumFastestTimes = 23;
        public const int NumHighestScores = 5;
        public const int NumBestPositions = 1;
        public const int NumRadioStations = 10;
        public const int LastMissionPassedNameLength = 8;

        private int m_peopleKilledByPlayer;
        private int m_peopleKilledByOthers;
        private int m_carsExploded;
        private int m_boatsExploded;
        private int m_tyresPopped;
        private int m_roundsFiredByPlayer;
        private ObservableArray<int> m_pedsKilledOfThisType;
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
        private bool m_pamphletMissionPassed;
        private bool m_noMoreHurricanes;
        private float m_distanceTravelledOnFoot;
        private float m_distanceTravelledByCar;
        private float m_distanceTravelledByBike;
        private float m_distanceTravelledByBoat;
        private float m_distanceTravelledByGolfCart;
        private float m_distanceTravelledByHelicopter;
        private float m_distanceTravelledByPlane;
        private int m_livesSavedWithAmbulance;
        private int m_criminalsCaught;
        private int m_firesExtinguished;
        private int m_highestLevelVigilanteMission;
        private int m_highestLevelAmbulanceMission;
        private int m_highestLevelFireMission;
        private int m_photosTaken;
        private int m_numberKillFrenziesPassed;
        private int m_totalNumberKillFrenzies;
        private int m_totalNumberMissions;
        private int m_flightTime;
        private int m_timesDrowned;
        private int m_seagullsKilled;
        private float m_weaponBudget;
        private float m_fashionBudget;
        private float m_loanSharks;
        private float m_storesKnockedOff;
        private float m_movieStunts;
        private float m_assassinations;
        private float m_pizzasDelivered;
        private float m_garbagePickups;
        private float m_iceCreamSold;
        private float m_topShootingRangeScore;
        private float m_shootingRank;
        private int m_longestWheelie;
        private int m_longestStoppie;
        private int m_longest2Wheel;
        private float m_longestWheelieDist;
        private float m_longestStoppieDist;
        private float m_longest2WheelDist;
        private float m_longestFacePlantDist;
        private float m_propertyBudget;
        private float m_autoPaintingBudget;
        private int m_propertyDestroyed;
        private int m_numPropertyOwned;
        private int m_bloodRingKills;
        private int m_bloodRingTime;
        private ObservableArray<bool> m_propertyOwned;
        private float m_highestChaseValue;
        private ObservableArray<int> m_fastestTimes;
        private ObservableArray<int> m_highestScores;
        private ObservableArray<int> m_bestPositions;
        private int m_killsSinceLastCheckpoint; // always 0 on save
        private int m_totalLegitimateKills;
        private string m_lastMissionPassedName;
        private int m_cheatedCount;
        private ObservableArray<float> m_favoriteRadioStationList;

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

        public ObservableArray<int> PedsKilledOfThisType
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

        public float DistanceTravelledByGolfCart
        {
            get { return m_distanceTravelledByGolfCart; }
            set { m_distanceTravelledByGolfCart = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledByHelicopter
        {
            get { return m_distanceTravelledByHelicopter; }
            set { m_distanceTravelledByHelicopter = value; OnPropertyChanged(); }
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

        public int FlightTime
        {
            get { return m_flightTime; }
            set { m_flightTime = value; OnPropertyChanged(); }
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

        public float FashionBudget
        {
            get { return m_fashionBudget; }
            set { m_fashionBudget = value; OnPropertyChanged(); }
        }

        public float LoanSharks
        {
            get { return m_loanSharks; }
            set { m_loanSharks = value; OnPropertyChanged(); }
        }

        public float StoresKnockedOff
        {
            get { return m_storesKnockedOff; }
            set { m_storesKnockedOff = value; OnPropertyChanged(); }
        }

        public float MovieStunts
        {
            get { return m_movieStunts; }
            set { m_movieStunts = value; OnPropertyChanged(); }
        }

        public float Assassinations
        {
            get { return m_assassinations; }
            set { m_assassinations = value; OnPropertyChanged(); }
        }

        public float PizzasDelivered
        {
            get { return m_pizzasDelivered; }
            set { m_pizzasDelivered = value; OnPropertyChanged(); }
        }

        public float GarbagePickups
        {
            get { return m_garbagePickups; }
            set { m_garbagePickups = value; OnPropertyChanged(); }
        }

        public float IceCreamSold
        {
            get { return m_iceCreamSold; }
            set { m_iceCreamSold = value; OnPropertyChanged(); }
        }

        public float TopShootingRangeScore
        {
            get { return m_topShootingRangeScore; }
            set { m_topShootingRangeScore = value; OnPropertyChanged(); }
        }

        public float ShootingRank
        {
            get { return m_shootingRank; }
            set { m_shootingRank = value; OnPropertyChanged(); }
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

        public float PropertyBudget
        {
            get { return m_propertyBudget; }
            set { m_propertyBudget = value; OnPropertyChanged(); }
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

        public int BloodRingKills
        {
            get { return m_bloodRingKills; }
            set { m_bloodRingKills = value; OnPropertyChanged(); }
        }

        public int BloodRingTime
        {
            get { return m_bloodRingTime; }
            set { m_bloodRingTime = value; OnPropertyChanged(); }
        }

        public ObservableArray<bool> PropertyOwned
        {
            get { return m_propertyOwned; }
            set { m_propertyOwned = value; OnPropertyChanged(); }
        }

        public float HighestChaseValue
        {
            get { return m_highestChaseValue; }
            set { m_highestChaseValue = value; OnPropertyChanged(); }
        }

        public ObservableArray<int> FastestTimes
        {
            get { return m_fastestTimes; }
            set { m_fastestTimes = value; OnPropertyChanged(); }
        }

        public ObservableArray<int> HighestScores
        {
            get { return m_highestScores; }
            set { m_highestScores = value; OnPropertyChanged(); }
        }

        public ObservableArray<int> BestPositions
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

        public ObservableArray<float> FavoriteRadioStationList
        {
            get { return m_favoriteRadioStationList; }
            set { m_favoriteRadioStationList = value; OnPropertyChanged(); }
        }

        public Stats()
        {
            PedsKilledOfThisType = ArrayHelper.CreateArray<int>(NumPedTypes);
            PropertyOwned = ArrayHelper.CreateArray<bool>(NumProperties);
            FastestTimes = ArrayHelper.CreateArray<int>(NumFastestTimes);
            HighestScores = ArrayHelper.CreateArray<int>(NumHighestScores);
            BestPositions = ArrayHelper.CreateArray<int>(NumBestPositions);
            FavoriteRadioStationList = ArrayHelper.CreateArray<float>(NumRadioStations);
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
            DistanceTravelledByGolfCart = other.DistanceTravelledByGolfCart;
            DistanceTravelledByHelicopter = other.DistanceTravelledByHelicopter;
            DistanceTravelledByPlane = other.DistanceTravelledByPlane;
            LivesSavedWithAmbulance = other.LivesSavedWithAmbulance;
            CriminalsCaught = other.CriminalsCaught;
            FiresExtinguished = other.FiresExtinguished;
            HighestLevelVigilanteMission = other.HighestLevelVigilanteMission;
            HighestLevelAmbulanceMission = other.HighestLevelAmbulanceMission;
            HighestLevelFireMission = other.HighestLevelFireMission;
            PhotosTaken = other.PhotosTaken;
            NumberKillFrenziesPassed = other.NumberKillFrenziesPassed;
            TotalNumberKillFrenzies = other.TotalNumberKillFrenzies;
            TotalNumberMissions = other.TotalNumberMissions;
            FlightTime = other.FlightTime;
            TimesDrowned = other.TimesDrowned;
            SeagullsKilled = other.SeagullsKilled;
            WeaponBudget = other.WeaponBudget;
            FashionBudget = other.FashionBudget;
            LoanSharks = other.LoanSharks;
            StoresKnockedOff = other.StoresKnockedOff;
            MovieStunts = other.MovieStunts;
            Assassinations = other.Assassinations;
            PizzasDelivered = other.PizzasDelivered;
            GarbagePickups = other.GarbagePickups;
            IceCreamSold = other.IceCreamSold;
            TopShootingRangeScore = other.TopShootingRangeScore;
            ShootingRank = other.ShootingRank;
            LongestWheelie = other.LongestWheelie;
            LongestStoppie = other.LongestStoppie;
            Longest2Wheel = other.Longest2Wheel;
            LongestWheelieDist = other.LongestWheelieDist;
            LongestStoppieDist = other.LongestStoppieDist;
            Longest2WheelDist = other.Longest2WheelDist;
            PropertyBudget = other.PropertyBudget;
            AutoPaintingBudget = other.AutoPaintingBudget;
            PropertyDestroyed = other.PropertyDestroyed;
            NumPropertyOwned = other.NumPropertyOwned;
            BloodRingKills = other.BloodRingKills;
            BloodRingTime = other.BloodRingTime;
            PropertyOwned = ArrayHelper.DeepClone(other.PropertyOwned);
            HighestChaseValue = other.HighestChaseValue;
            FastestTimes = ArrayHelper.DeepClone(other.FastestTimes);
            HighestScores = ArrayHelper.DeepClone(other.HighestScores);
            BestPositions = ArrayHelper.DeepClone(other.BestPositions);
            KillsSinceLastCheckpoint = other.KillsSinceLastCheckpoint;
            TotalLegitimateKills = other.TotalLegitimateKills;
            LastMissionPassedName = other.LastMissionPassedName;
            CheatedCount = other.CheatedCount;
            FavoriteRadioStationList = ArrayHelper.DeepClone(other.FavoriteRadioStationList);
        }

        protected override void ReadData(DataBuffer buf, FileType fmt)
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
            DistanceTravelledByGolfCart = buf.ReadFloat();
            DistanceTravelledByHelicopter = buf.ReadFloat();
            DistanceTravelledByPlane = buf.ReadFloat();
            LivesSavedWithAmbulance = buf.ReadInt32();
            CriminalsCaught = buf.ReadInt32();
            FiresExtinguished = buf.ReadInt32();
            HighestLevelVigilanteMission = buf.ReadInt32();
            HighestLevelAmbulanceMission = buf.ReadInt32();
            HighestLevelFireMission = buf.ReadInt32();
            PhotosTaken = buf.ReadInt32();
            NumberKillFrenziesPassed = buf.ReadInt32();
            TotalNumberKillFrenzies = buf.ReadInt32();
            TotalNumberMissions = buf.ReadInt32();
            FlightTime = buf.ReadInt32();
            TimesDrowned = buf.ReadInt32();
            SeagullsKilled = buf.ReadInt32();
            WeaponBudget = buf.ReadFloat();
            FashionBudget = buf.ReadFloat();
            LoanSharks = buf.ReadFloat();
            StoresKnockedOff = buf.ReadFloat();
            MovieStunts = buf.ReadFloat();
            Assassinations = buf.ReadFloat();
            PizzasDelivered = buf.ReadFloat();
            GarbagePickups = buf.ReadFloat();
            IceCreamSold = buf.ReadFloat();
            TopShootingRangeScore = buf.ReadFloat();
            ShootingRank = buf.ReadFloat();
            LongestWheelie = buf.ReadInt32();
            LongestStoppie = buf.ReadInt32();
            Longest2Wheel = buf.ReadInt32();
            LongestWheelieDist = buf.ReadFloat();
            LongestStoppieDist = buf.ReadFloat();
            Longest2WheelDist = buf.ReadFloat();
            PropertyBudget = buf.ReadFloat();
            AutoPaintingBudget = buf.ReadFloat();
            PropertyDestroyed = buf.ReadInt32();
            NumPropertyOwned = buf.ReadInt32();
            BloodRingKills = buf.ReadInt32();
            BloodRingTime = buf.ReadInt32();
            PropertyOwned = buf.ReadArray<bool>(NumProperties);
            HighestChaseValue = buf.ReadFloat();
            FastestTimes = buf.ReadArray<int>(NumFastestTimes);
            HighestScores = buf.ReadArray<int>(NumHighestScores);
            BestPositions = buf.ReadArray<int>(NumBestPositions);
            KillsSinceLastCheckpoint = buf.ReadInt32();
            TotalLegitimateKills = buf.ReadInt32();
            LastMissionPassedName = buf.ReadString(LastMissionPassedNameLength);
            CheatedCount = buf.ReadInt32();
            FavoriteRadioStationList = buf.ReadArray<float>(NumRadioStations);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override void WriteData(DataBuffer buf, FileType fmt)
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
            buf.Write(DistanceTravelledByGolfCart);
            buf.Write(DistanceTravelledByHelicopter);
            buf.Write(DistanceTravelledByPlane);
            buf.Write(LivesSavedWithAmbulance);
            buf.Write(CriminalsCaught);
            buf.Write(FiresExtinguished);
            buf.Write(HighestLevelVigilanteMission);
            buf.Write(HighestLevelAmbulanceMission);
            buf.Write(HighestLevelFireMission);
            buf.Write(PhotosTaken);
            buf.Write(NumberKillFrenziesPassed);
            buf.Write(TotalNumberKillFrenzies);
            buf.Write(TotalNumberMissions);
            buf.Write(FlightTime);
            buf.Write(TimesDrowned);
            buf.Write(SeagullsKilled);
            buf.Write(WeaponBudget);
            buf.Write(FashionBudget);
            buf.Write(LoanSharks);
            buf.Write(StoresKnockedOff);
            buf.Write(MovieStunts);
            buf.Write(Assassinations);
            buf.Write(PizzasDelivered);
            buf.Write(GarbagePickups);
            buf.Write(IceCreamSold);
            buf.Write(TopShootingRangeScore);
            buf.Write(ShootingRank);
            buf.Write(LongestWheelie);
            buf.Write(LongestStoppie);
            buf.Write(Longest2Wheel);
            buf.Write(LongestWheelieDist);
            buf.Write(LongestStoppieDist);
            buf.Write(Longest2WheelDist);
            buf.Write(PropertyBudget);
            buf.Write(AutoPaintingBudget);
            buf.Write(PropertyDestroyed);
            buf.Write(NumPropertyOwned);
            buf.Write(BloodRingKills);
            buf.Write(BloodRingTime);
            buf.Write(PropertyOwned, NumProperties);
            buf.Write(HighestChaseValue);
            buf.Write(FastestTimes, NumFastestTimes);
            buf.Write(HighestScores, NumHighestScores);
            buf.Write(BestPositions, NumBestPositions);
            buf.Write(KillsSinceLastCheckpoint);
            buf.Write(TotalLegitimateKills);
            buf.Write(LastMissionPassedName, LastMissionPassedNameLength);
            buf.Write(CheatedCount);
            buf.Write(FavoriteRadioStationList, NumRadioStations);

            Debug.Assert(buf.Offset == GetSize(fmt));
        }

        protected override int GetSize(FileType fmt)
        {
            return 0x253;
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
                && DistanceTravelledByGolfCart.Equals(other.DistanceTravelledByGolfCart)
                && DistanceTravelledByHelicopter.Equals(other.DistanceTravelledByHelicopter)
                && DistanceTravelledByPlane.Equals(other.DistanceTravelledByPlane)
                && LivesSavedWithAmbulance.Equals(other.LivesSavedWithAmbulance)
                && CriminalsCaught.Equals(other.CriminalsCaught)
                && FiresExtinguished.Equals(other.FiresExtinguished)
                && HighestLevelVigilanteMission.Equals(other.HighestLevelVigilanteMission)
                && HighestLevelAmbulanceMission.Equals(other.HighestLevelAmbulanceMission)
                && HighestLevelFireMission.Equals(other.HighestLevelFireMission)
                && PhotosTaken.Equals(other.PhotosTaken)
                && NumberKillFrenziesPassed.Equals(other.NumberKillFrenziesPassed)
                && TotalNumberKillFrenzies.Equals(other.TotalNumberKillFrenzies)
                && TotalNumberMissions.Equals(other.TotalNumberMissions)
                && FlightTime.Equals(other.FlightTime)
                && TimesDrowned.Equals(other.TimesDrowned)
                && SeagullsKilled.Equals(other.SeagullsKilled)
                && WeaponBudget.Equals(other.WeaponBudget)
                && FashionBudget.Equals(other.FashionBudget)
                && LoanSharks.Equals(other.LoanSharks)
                && StoresKnockedOff.Equals(other.StoresKnockedOff)
                && MovieStunts.Equals(other.MovieStunts)
                && Assassinations.Equals(other.Assassinations)
                && PizzasDelivered.Equals(other.PizzasDelivered)
                && GarbagePickups.Equals(other.GarbagePickups)
                && IceCreamSold.Equals(other.IceCreamSold)
                && TopShootingRangeScore.Equals(other.TopShootingRangeScore)
                && ShootingRank.Equals(other.ShootingRank)
                && LongestWheelie.Equals(other.LongestWheelie)
                && LongestStoppie.Equals(other.LongestStoppie)
                && Longest2Wheel.Equals(other.Longest2Wheel)
                && LongestWheelieDist.Equals(other.LongestWheelieDist)
                && LongestStoppieDist.Equals(other.LongestStoppieDist)
                && Longest2WheelDist.Equals(other.Longest2WheelDist)
                && PropertyBudget.Equals(other.PropertyBudget)
                && AutoPaintingBudget.Equals(other.AutoPaintingBudget)
                && PropertyDestroyed.Equals(other.PropertyDestroyed)
                && NumPropertyOwned.Equals(other.NumPropertyOwned)
                && BloodRingKills.Equals(other.BloodRingKills)
                && BloodRingTime.Equals(other.BloodRingTime)
                && PropertyOwned.SequenceEqual(other.PropertyOwned)
                && HighestChaseValue.Equals(other.HighestChaseValue)
                && FastestTimes.SequenceEqual(other.FastestTimes)
                && HighestScores.SequenceEqual(other.HighestScores)
                && BestPositions.SequenceEqual(other.BestPositions)
                && KillsSinceLastCheckpoint.Equals(other.KillsSinceLastCheckpoint)
                && TotalLegitimateKills.Equals(other.TotalLegitimateKills)
                && LastMissionPassedName.Equals(other.LastMissionPassedName)
                && CheatedCount.Equals(other.CheatedCount)
                && FavoriteRadioStationList.SequenceEqual(other.FavoriteRadioStationList);
        }

        public Stats DeepClone()
        {
            return new Stats(this);
        }
    }
}
