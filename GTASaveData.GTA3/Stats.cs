using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class Stats : SaveDataObject, 
        IEquatable<Stats>, IDeepClonable<Stats>
    {
        public const int MaxNumPedTypes = 23;
        public const int MaxNumFastestTimes = 16;
        public const int MaxNumHighestScores = 16;
        public const int MaxLastMissionPassedNameLength = 8;

        private int m_peopleKilledByPlayer;
        private int m_peopleKilledByOthers;
        private int m_carsExploded;
        private int m_roundsFiredByPlayer;
        private Array<int> m_pedsKilledOfThisType;
        private int m_helisDestroyed;
        private int m_progressMade;
        private int m_totalProgressInGame;
        private int m_kgsOfExplosivesUsed;
        private int m_instantHitsFiredByPlayer;
        private int m_instantHitsHitByPlayer;
        private int m_carsCrushed;
        private int m_headsPopped;
        private int m_timesArrested;
        private int m_timesDied;
        private int m_daysPassed;
        private int m_mmRain;
        private float m_maximumJumpDistance;
        private float m_maximumJumpHeight;
        private int m_maximumJumpFlips;
        private int m_maximumJumpSpins;
        private int m_bestStuntJump;
        private int m_numberOfUniqueJumpsFound;
        private int m_totalNumberOfUniqueJumps;
        private int m_missionsGiven;
        private int m_missionsPassed;
        private int m_passengersDroppedOffWithTaxi;
        private int m_moneyMadeWithTaxi;
        private bool m_industrialPassed;
        private bool m_commercialPassed;
        private bool m_suburbanPassed;
        private int m_elBurroTime;
        private float m_distanceTravelledOnFoot;
        private float m_distanceTravelledInVehicle;
        private int m_record4x4One;
        private int m_record4x4Two;
        private int m_record4x4Three;
        private int m_record4x4Mayhem;
        private int m_livesSavedWithAmbulance;
        private int m_criminalsCaught;
        private int m_highestLevelAmbulanceMission;
        private int m_firesExtinguished;
        private int m_longestFlightInDodo;
        private int m_timeTakenDefuseMission;
        private int m_numberKillFrenziesPassed;
        private int m_totalNumberKillFrenzies;
        private int m_totalNumberMissions;
        private Array<int> m_fastestTimes;
        private Array<int> m_highestScores;
        private int m_killsSinceLastCheckpoint;
        private int m_totalLegitimateKills;
        private string m_lastMissionPassedName;

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

        public int ProgressMade
        {
            get { return m_progressMade; }
            set { m_progressMade = value; OnPropertyChanged(); }
        }

        public int TotalProgressInGame
        {
            get { return m_totalProgressInGame; }
            set { m_totalProgressInGame = value; OnPropertyChanged(); }
        }

        public int KgsOfExplosivesUsed
        {
            get { return m_kgsOfExplosivesUsed; }
            set { m_kgsOfExplosivesUsed = value; OnPropertyChanged(); }
        }

        public int InstantHitsFiredByPlayer
        {
            get { return m_instantHitsFiredByPlayer; }
            set { m_instantHitsFiredByPlayer = value; OnPropertyChanged(); }
        }

        public int InstantHitsHitByPlayer
        {
            get { return m_instantHitsHitByPlayer; }
            set { m_instantHitsHitByPlayer = value; OnPropertyChanged(); }
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

        public int MmRain
        {
            get { return m_mmRain; }
            set { m_mmRain = value; OnPropertyChanged(); }
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

        public int MissionsPassed
        {
            get { return m_missionsPassed; }
            set { m_missionsPassed = value; OnPropertyChanged(); }
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

        public int ElBurroTime
        {
            get { return m_elBurroTime; }
            set { m_elBurroTime = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledOnFoot
        {
            get { return m_distanceTravelledOnFoot; }
            set { m_distanceTravelledOnFoot = value; OnPropertyChanged(); }
        }

        public float DistanceTravelledInVehicle
        {
            get { return m_distanceTravelledInVehicle; }
            set { m_distanceTravelledInVehicle = value; OnPropertyChanged(); }
        }

        public int Record4x4One
        {
            get { return m_record4x4One; }
            set { m_record4x4One = value; OnPropertyChanged(); }
        }

        public int Record4x4Two
        {
            get { return m_record4x4Two; }
            set { m_record4x4Two = value; OnPropertyChanged(); }
        }

        public int Record4x4Three
        {
            get { return m_record4x4Three; }
            set { m_record4x4Three = value; OnPropertyChanged(); }
        }

        public int Record4x4Mayhem
        {
            get { return m_record4x4Mayhem; }
            set { m_record4x4Mayhem = value; OnPropertyChanged(); }
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

        public int HighestLevelAmbulanceMission
        {
            get { return m_highestLevelAmbulanceMission; }
            set { m_highestLevelAmbulanceMission = value; OnPropertyChanged(); }
        }

        public int FiresExtinguished
        {
            get { return m_firesExtinguished; }
            set { m_firesExtinguished = value; OnPropertyChanged(); }
        }

        public int LongestFlightInDodo
        {
            get { return m_longestFlightInDodo; }
            set { m_longestFlightInDodo = value; OnPropertyChanged(); }
        }

        public int TimeTakenDefuseMission
        {
            get { return m_timeTakenDefuseMission; }
            set { m_timeTakenDefuseMission = value; OnPropertyChanged(); }
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

        public Stats()
        {
            PedsKilledOfThisType = ArrayHelper.CreateArray<int>(MaxNumPedTypes);
            FastestTimes = ArrayHelper.CreateArray<int>(MaxNumFastestTimes);
            HighestScores = ArrayHelper.CreateArray<int>(MaxNumHighestScores);
            LastMissionPassedName = "";
        }

        public Stats(Stats other)
        {
            PeopleKilledByPlayer = other.PeopleKilledByPlayer;
            PeopleKilledByOthers = other.PeopleKilledByOthers;
            CarsExploded = other.CarsExploded;
            RoundsFiredByPlayer = other.RoundsFiredByPlayer;
            PedsKilledOfThisType = ArrayHelper.DeepClone(other.PedsKilledOfThisType);
            HelisDestroyed = other.HelisDestroyed;
            ProgressMade = other.ProgressMade;
            TotalProgressInGame = other.TotalProgressInGame;
            KgsOfExplosivesUsed = other.KgsOfExplosivesUsed;
            InstantHitsFiredByPlayer = other.InstantHitsFiredByPlayer;
            InstantHitsHitByPlayer = other.InstantHitsHitByPlayer;
            CarsCrushed = other.CarsCrushed;
            HeadsPopped = other.HeadsPopped;
            TimesArrested = other.TimesArrested;
            TimesDied = other.TimesDied;
            DaysPassed = other.DaysPassed;
            MmRain = other.MmRain;
            MaximumJumpDistance = other.MaximumJumpDistance;
            MaximumJumpHeight = other.MaximumJumpHeight;
            MaximumJumpFlips = other.MaximumJumpFlips;
            MaximumJumpSpins = other.MaximumJumpSpins;
            BestStuntJump = other.BestStuntJump;
            NumberOfUniqueJumpsFound = other.NumberOfUniqueJumpsFound;
            TotalNumberOfUniqueJumps = other.TotalNumberOfUniqueJumps;
            MissionsGiven = other.MissionsGiven;
            MissionsPassed = other.MissionsPassed;
            PassengersDroppedOffWithTaxi = other.PassengersDroppedOffWithTaxi;
            MoneyMadeWithTaxi = other.MoneyMadeWithTaxi;
            IndustrialPassed = other.IndustrialPassed;
            CommercialPassed = other.CommercialPassed;
            SuburbanPassed = other.SuburbanPassed;
            ElBurroTime = other.ElBurroTime;
            DistanceTravelledOnFoot = other.DistanceTravelledOnFoot;
            DistanceTravelledInVehicle = other.DistanceTravelledInVehicle;
            Record4x4One = other.Record4x4One;
            Record4x4Two = other.Record4x4Two;
            Record4x4Three = other.Record4x4Three;
            Record4x4Mayhem = other.Record4x4Mayhem;
            LivesSavedWithAmbulance = other.LivesSavedWithAmbulance;
            CriminalsCaught = other.CriminalsCaught;
            HighestLevelAmbulanceMission = other.HighestLevelAmbulanceMission;
            FiresExtinguished = other.FiresExtinguished;
            LongestFlightInDodo = other.LongestFlightInDodo;
            TimeTakenDefuseMission = other.TimeTakenDefuseMission;
            NumberKillFrenziesPassed = other.NumberKillFrenziesPassed;
            TotalNumberKillFrenzies = other.TotalNumberKillFrenzies;
            TotalNumberMissions = other.TotalNumberMissions;
            FastestTimes = ArrayHelper.DeepClone(other.FastestTimes);
            HighestScores = ArrayHelper.DeepClone(other.HighestScores);
            KillsSinceLastCheckpoint = other.KillsSinceLastCheckpoint;
            TotalLegitimateKills = other.TotalLegitimateKills;
            LastMissionPassedName = other.LastMissionPassedName;
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            PeopleKilledByPlayer = buf.ReadInt32();
            PeopleKilledByOthers = buf.ReadInt32();
            CarsExploded = buf.ReadInt32();
            RoundsFiredByPlayer = buf.ReadInt32();
            PedsKilledOfThisType = buf.Read<int>(MaxNumPedTypes);
            HelisDestroyed = buf.ReadInt32();
            ProgressMade = buf.ReadInt32();
            TotalProgressInGame = buf.ReadInt32();
            KgsOfExplosivesUsed = buf.ReadInt32();
            InstantHitsFiredByPlayer = buf.ReadInt32();
            InstantHitsHitByPlayer = buf.ReadInt32();
            CarsCrushed = buf.ReadInt32();
            HeadsPopped = buf.ReadInt32();
            TimesArrested = buf.ReadInt32();
            TimesDied = buf.ReadInt32();
            DaysPassed = buf.ReadInt32();
            MmRain = buf.ReadInt32();
            MaximumJumpDistance = buf.ReadFloat();
            MaximumJumpHeight = buf.ReadFloat();
            MaximumJumpFlips = buf.ReadInt32();
            MaximumJumpSpins = buf.ReadInt32();
            BestStuntJump = buf.ReadInt32();
            NumberOfUniqueJumpsFound = buf.ReadInt32();
            TotalNumberOfUniqueJumps = buf.ReadInt32();
            MissionsGiven = buf.ReadInt32();
            MissionsPassed = buf.ReadInt32();
            PassengersDroppedOffWithTaxi = buf.ReadInt32();
            MoneyMadeWithTaxi = buf.ReadInt32();
            IndustrialPassed = buf.ReadBool(4);
            CommercialPassed = buf.ReadBool(4);
            SuburbanPassed = buf.ReadBool(4);
            ElBurroTime = buf.ReadInt32();
            DistanceTravelledOnFoot = buf.ReadFloat();
            DistanceTravelledInVehicle = buf.ReadFloat();
            Record4x4One = buf.ReadInt32();
            Record4x4Two = buf.ReadInt32();
            Record4x4Three = buf.ReadInt32();
            Record4x4Mayhem = buf.ReadInt32();
            LivesSavedWithAmbulance = buf.ReadInt32();
            CriminalsCaught = buf.ReadInt32();
            HighestLevelAmbulanceMission = buf.ReadInt32();
            FiresExtinguished = buf.ReadInt32();
            LongestFlightInDodo = buf.ReadInt32();
            TimeTakenDefuseMission = buf.ReadInt32();
            NumberKillFrenziesPassed = buf.ReadInt32();
            TotalNumberKillFrenzies = buf.ReadInt32();
            TotalNumberMissions = buf.ReadInt32();
            FastestTimes = buf.Read<int>(MaxNumFastestTimes);
            HighestScores = buf.Read<int>(MaxNumHighestScores);
            KillsSinceLastCheckpoint = buf.ReadInt32();
            TotalLegitimateKills = buf.ReadInt32();
            LastMissionPassedName = buf.ReadString(MaxLastMissionPassedNameLength);

            Debug.Assert(buf.Offset == SizeOfType<Stats>());
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            buf.Write(PeopleKilledByPlayer);
            buf.Write(PeopleKilledByOthers);
            buf.Write(CarsExploded);
            buf.Write(RoundsFiredByPlayer);
            buf.Write(PedsKilledOfThisType, MaxNumPedTypes);
            buf.Write(HelisDestroyed);
            buf.Write(ProgressMade);
            buf.Write(TotalProgressInGame);
            buf.Write(KgsOfExplosivesUsed);
            buf.Write(InstantHitsFiredByPlayer);
            buf.Write(InstantHitsHitByPlayer);
            buf.Write(CarsCrushed);
            buf.Write(HeadsPopped);
            buf.Write(TimesArrested);
            buf.Write(TimesDied);
            buf.Write(DaysPassed);
            buf.Write(MmRain);
            buf.Write(MaximumJumpDistance);
            buf.Write(MaximumJumpHeight);
            buf.Write(MaximumJumpFlips);
            buf.Write(MaximumJumpSpins);
            buf.Write(BestStuntJump);
            buf.Write(NumberOfUniqueJumpsFound);
            buf.Write(TotalNumberOfUniqueJumps);
            buf.Write(MissionsGiven);
            buf.Write(MissionsPassed);
            buf.Write(PassengersDroppedOffWithTaxi);
            buf.Write(MoneyMadeWithTaxi);
            buf.Write(IndustrialPassed, 4);
            buf.Write(CommercialPassed, 4);
            buf.Write(SuburbanPassed, 4);
            buf.Write(ElBurroTime);
            buf.Write(DistanceTravelledOnFoot);
            buf.Write(DistanceTravelledInVehicle);
            buf.Write(Record4x4One);
            buf.Write(Record4x4Two);
            buf.Write(Record4x4Three);
            buf.Write(Record4x4Mayhem);
            buf.Write(LivesSavedWithAmbulance);
            buf.Write(CriminalsCaught);
            buf.Write(HighestLevelAmbulanceMission);
            buf.Write(FiresExtinguished);
            buf.Write(LongestFlightInDodo);
            buf.Write(TimeTakenDefuseMission);
            buf.Write(NumberKillFrenziesPassed);
            buf.Write(TotalNumberKillFrenzies);
            buf.Write(TotalNumberMissions);
            buf.Write(FastestTimes, MaxNumFastestTimes);
            buf.Write(HighestScores, MaxNumHighestScores);
            buf.Write(KillsSinceLastCheckpoint);
            buf.Write(TotalLegitimateKills);
            buf.Write(LastMissionPassedName, MaxLastMissionPassedNameLength);

            Debug.Assert(buf.Offset == SizeOfType<Stats>());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return 420;
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
                && RoundsFiredByPlayer.Equals(other.RoundsFiredByPlayer)
                && PedsKilledOfThisType.SequenceEqual(other.PedsKilledOfThisType)
                && HelisDestroyed.Equals(other.HelisDestroyed)
                && ProgressMade.Equals(other.ProgressMade)
                && TotalProgressInGame.Equals(other.TotalProgressInGame)
                && KgsOfExplosivesUsed.Equals(other.KgsOfExplosivesUsed)
                && InstantHitsFiredByPlayer.Equals(other.InstantHitsFiredByPlayer)
                && InstantHitsHitByPlayer.Equals(other.InstantHitsHitByPlayer)
                && CarsCrushed.Equals(other.CarsCrushed)
                && HeadsPopped.Equals(other.HeadsPopped)
                && TimesArrested.Equals(other.TimesArrested)
                && TimesDied.Equals(other.TimesDied)
                && DaysPassed.Equals(other.DaysPassed)
                && MmRain.Equals(other.MmRain)
                && MaximumJumpDistance.Equals(other.MaximumJumpDistance)
                && MaximumJumpHeight.Equals(other.MaximumJumpHeight)
                && MaximumJumpFlips.Equals(other.MaximumJumpFlips)
                && MaximumJumpSpins.Equals(other.MaximumJumpSpins)
                && BestStuntJump.Equals(other.BestStuntJump)
                && NumberOfUniqueJumpsFound.Equals(other.NumberOfUniqueJumpsFound)
                && TotalNumberOfUniqueJumps.Equals(other.TotalNumberOfUniqueJumps)
                && MissionsGiven.Equals(other.MissionsGiven)
                && MissionsPassed.Equals(other.MissionsPassed)
                && PassengersDroppedOffWithTaxi.Equals(other.PassengersDroppedOffWithTaxi)
                && MoneyMadeWithTaxi.Equals(other.MoneyMadeWithTaxi)
                && IndustrialPassed.Equals(other.IndustrialPassed)
                && CommercialPassed.Equals(other.CommercialPassed)
                && SuburbanPassed.Equals(other.SuburbanPassed)
                && ElBurroTime.Equals(other.ElBurroTime)
                && DistanceTravelledOnFoot.Equals(other.DistanceTravelledOnFoot)
                && DistanceTravelledInVehicle.Equals(other.DistanceTravelledInVehicle)
                && Record4x4One.Equals(other.Record4x4One)
                && Record4x4Two.Equals(other.Record4x4Two)
                && Record4x4Three.Equals(other.Record4x4Three)
                && Record4x4Mayhem.Equals(other.Record4x4Mayhem)
                && LivesSavedWithAmbulance.Equals(other.LivesSavedWithAmbulance)
                && CriminalsCaught.Equals(other.CriminalsCaught)
                && HighestLevelAmbulanceMission.Equals(other.HighestLevelAmbulanceMission)
                && FiresExtinguished.Equals(other.FiresExtinguished)
                && LongestFlightInDodo.Equals(other.LongestFlightInDodo)
                && TimeTakenDefuseMission.Equals(other.TimeTakenDefuseMission)
                && NumberKillFrenziesPassed.Equals(other.NumberKillFrenziesPassed)
                && TotalNumberKillFrenzies.Equals(other.TotalNumberKillFrenzies)
                && TotalNumberMissions.Equals(other.TotalNumberMissions)
                && FastestTimes.SequenceEqual(other.FastestTimes)
                && HighestScores.SequenceEqual(other.HighestScores)
                && KillsSinceLastCheckpoint.Equals(other.KillsSinceLastCheckpoint)
                && TotalLegitimateKills.Equals(other.TotalLegitimateKills)
                && LastMissionPassedName.Equals(other.LastMissionPassedName);
        }

        public Stats DeepClone()
        {
            return new Stats(this);
        }
    }
}
