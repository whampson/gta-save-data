using GTASaveData.Extensions;
using GTASaveData.Helpers;
using GTASaveData.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Represents a saved <i>Grand Theft Auto III</i> game.
    /// </summary>
    public class SaveFileGTA3 : SaveFileGTA3VC, ISaveFile,
        IEquatable<SaveFileGTA3>, IDeepClonable<SaveFileGTA3>
    {
        private const int MaxNumPaddingBlocks = 4;
        private const int NumOuterBlocks = 20;
        private const int NumOuterBlocksPS2 = 3;
        private const int DataSize = 0x31400;

        private static readonly byte[] XboxTitleKey =
        {
            0xFF, 0x3B, 0x8F, 0x5C, 0xDE, 0x53, 0xF3, 0x25,
            0x9E, 0x70, 0x09, 0x54, 0xEF, 0xDC, 0xA8, 0xDC
        };

        private SimpleVariables m_simpleVars;
        private ScriptData m_scripts;
        private PedPool m_pedPool;
        private GarageData m_garages;
        private VehiclePool m_vehiclePool;
        private ObjectPool m_objectPool;
        private PathData m_paths;
        private CraneData m_cranes;
        private PickupData m_pickups;
        private PhoneData m_phoneInfo;
        private RestartData m_restartPoints;
        private RadarData m_radarBlips;
        private ZoneData m_zones;
        private GangData m_gangs;
        private CarGeneratorData m_carGenerators;
        private ParticleData m_particleObjects;
        private AudioScriptData m_audioScriptObjects;
        private PlayerInfo m_playerInfo;
        private Stats m_stats;
        private Streaming m_streaming;
        private PedTypeData m_pedType;

        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public ScriptData Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; OnPropertyChanged(); }
        }

        public PedPool PlayerPeds
        {
            get { return m_pedPool; }
            set { m_pedPool = value; OnPropertyChanged(); }
        }

        public GarageData Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public VehiclePool Vehicles
        {
            get { return m_vehiclePool; }
            set { m_vehiclePool = value; OnPropertyChanged(); }
        }

        public ObjectPool Objects
        {
            get { return m_objectPool; }
            set { m_objectPool = value; OnPropertyChanged(); }
        }

        public PathData Paths
        {
            get { return m_paths; }
            set { m_paths = value; OnPropertyChanged(); }
        }

        public CraneData Cranes
        {
            get { return m_cranes; }
            set { m_cranes = value; OnPropertyChanged(); }
        }

        public PickupData Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
        }

        public PhoneData PhoneInfo
        {
            get { return m_phoneInfo; }
            set { m_phoneInfo = value; OnPropertyChanged(); }
        }

        public RestartData RestartPoints
        {
            get { return m_restartPoints; }
            set { m_restartPoints = value; OnPropertyChanged(); }
        }

        public RadarData RadarBlips
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; OnPropertyChanged(); }
        }

        public ZoneData Zones
        {
            get { return m_zones; }
            set { m_zones = value; OnPropertyChanged(); }
        }

        public GangData Gangs
        {
            get { return m_gangs; }
            set { m_gangs = value; OnPropertyChanged(); }
        }

        public CarGeneratorData CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; OnPropertyChanged(); }
        }

        public ParticleData ParticleObjects
        {
            get { return m_particleObjects; }
            set { m_particleObjects = value; OnPropertyChanged(); }
        }

        public AudioScriptData AudioScriptObjects
        {
            get { return m_audioScriptObjects; }
            set { m_audioScriptObjects = value; OnPropertyChanged(); }
        }

        public PlayerInfo PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; OnPropertyChanged(); }
        }

        public Stats Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public Streaming Streaming
        {
            get { return m_streaming; }
            set { m_streaming = value; OnPropertyChanged(); }
        }

        public PedTypeData PedTypeInfo
        {
            get { return m_pedType; }
            set { m_pedType = value; OnPropertyChanged(); }
        }

        public override string Name
        {
            get { return SimpleVars.LastMissionPassedName; }
            set { SimpleVars.LastMissionPassedName = value; OnPropertyChanged(); }
        }

        public override DateTime TimeStamp
        {
            get { return SimpleVars.TimeStamp; }
            set { SimpleVars.TimeStamp = new SystemTime(value); OnPropertyChanged(); }
        }

        bool ISaveFile.HasSimpleVariables => true;
        bool ISaveFile.HasScriptData => true;
        bool ISaveFile.HasGarageData => true;
        bool ISaveFile.HasCarGenerators => true;
        bool ISaveFile.HasPlayerInfo => true;
        bool ISaveFile.HasStats => true;

        ISimpleVariables ISaveFile.SimpleVars => SimpleVars;
        IScriptData ISaveFile.ScriptData => Scripts;
        IGarageData ISaveFile.GarageData => Garages;
        ICarGeneratorData ISaveFile.CarGenerators => CarGenerators;
        IPlayerInfo ISaveFile.PlayerInfo => PlayerInfo;
        IStats ISaveFile.Stats => Stats;

        IReadOnlyList<ISaveDataObject> ISaveFile.Blocks => new List<SaveDataObject>()
        {
            SimpleVars,
            Scripts,
            PlayerPeds,
            Garages,
            Vehicles,
            Objects,
            Paths,
            Cranes,
            Pickups,
            PhoneInfo,
            RestartPoints,
            RadarBlips,
            Zones,
            Gangs,
            CarGenerators,
            ParticleObjects,
            AudioScriptObjects,
            PlayerInfo,
            Stats,
            Streaming,
            PedTypeInfo
        };

        public static SaveFileGTA3 Load(string path)
        {
            return Load<SaveFileGTA3>(path);
        }

        public static SaveFileGTA3 Load(string path, FileFormat fmt)
        {
            return Load<SaveFileGTA3>(path, fmt);
        }

        public SaveFileGTA3()
        {
            SimpleVars = new SimpleVariables();
            Scripts = new ScriptData();
            PlayerPeds = new PedPool();
            Garages = new GarageData();
            Vehicles = new VehiclePool();
            Objects = new ObjectPool();
            Paths = new PathData();
            Cranes = new CraneData();
            Pickups = new PickupData();
            PhoneInfo = new PhoneData();
            RestartPoints = new RestartData();
            RadarBlips = new RadarData();
            Zones = new ZoneData();
            Gangs = new GangData();
            CarGenerators = new CarGeneratorData();
            ParticleObjects = new ParticleData();
            AudioScriptObjects = new AudioScriptData();
            PlayerInfo = new PlayerInfo();
            Stats = new Stats();
            Streaming = new Streaming();
            PedTypeInfo = new PedTypeData();
        }

        public SaveFileGTA3(SaveFileGTA3 other)
        {
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Scripts = new ScriptData(other.Scripts);
            PlayerPeds = new PedPool(other.PlayerPeds);
            Garages = new GarageData(other.Garages);
            Vehicles = new VehiclePool(other.Vehicles);
            Objects = new ObjectPool(other.Objects);
            Paths = new PathData(other.Paths);
            Cranes = new CraneData(other.Cranes);
            Pickups = new PickupData(other.Pickups);
            PhoneInfo = new PhoneData(other.PhoneInfo);
            RestartPoints = new RestartData(other.RestartPoints);
            RadarBlips = new RadarData(other.RadarBlips);
            Zones = new ZoneData(other.Zones);
            Gangs = new GangData(other.Gangs);
            CarGenerators = new CarGeneratorData(other.CarGenerators);
            ParticleObjects = new ParticleData(other.ParticleObjects);
            AudioScriptObjects = new AudioScriptData(other.AudioScriptObjects);
            PlayerInfo = new PlayerInfo(other.PlayerInfo);
            Stats = new Stats(other.Stats);
            Streaming = new Streaming(other.Streaming);
            PedTypeInfo = new PedTypeData(other.PedTypeInfo);
        }

        protected override void LoadAllData(DataBuffer file)
        {
            int dataSize = 0;
            int numOuterBlocks;

            if (FileFormat.IsPS2)
            {
                dataSize += ReadBlock(file);
                SimpleVars = WorkBuff.ReadObject<SimpleVariables>(FileFormat);
                Scripts = LoadType<ScriptData>();
                PlayerPeds = LoadType<PedPool>();
                Garages = LoadType<GarageData>();
                Vehicles = LoadType<VehiclePool>();
                dataSize += ReadBlock(file);
                Objects = LoadType<ObjectPool>();
                Paths = LoadTypePreAlloc<PathData>();
                Cranes = LoadType<CraneData>();
                dataSize += ReadBlock(file);
                Pickups = LoadType<PickupData>();
                PhoneInfo = LoadType<PhoneData>();
                RestartPoints = LoadType<RestartData>();
                RadarBlips = LoadType<RadarData>();
                Zones = LoadType<ZoneData>();
                Gangs = LoadType<GangData>();
                CarGenerators = LoadType<CarGeneratorData>();
                ParticleObjects = LoadType<ParticleData>();
                AudioScriptObjects = LoadType<AudioScriptData>();
                PlayerInfo = LoadType<PlayerInfo>();
                Stats = LoadType<Stats>();
                Streaming = LoadType<Streaming>();
                PedTypeInfo = LoadType<PedTypeData>();
                numOuterBlocks = NumOuterBlocksPS2;
            }
            else
            {
                dataSize += ReadBlock(file);
                SimpleVars = WorkBuff.ReadObject<SimpleVariables>(FileFormat);
                Scripts = LoadType<ScriptData>();
                dataSize += ReadBlock(file); PlayerPeds = LoadType<PedPool>();
                dataSize += ReadBlock(file); Garages = LoadType<GarageData>();
                dataSize += ReadBlock(file); Vehicles = LoadType<VehiclePool>();
                dataSize += ReadBlock(file); Objects = LoadType<ObjectPool>();
                dataSize += ReadBlock(file); Paths = LoadTypePreAlloc<PathData>();
                dataSize += ReadBlock(file); Cranes = LoadType<CraneData>();
                dataSize += ReadBlock(file); Pickups = LoadType<PickupData>();
                dataSize += ReadBlock(file); PhoneInfo = LoadType<PhoneData>();
                dataSize += ReadBlock(file); RestartPoints = LoadType<RestartData>();
                dataSize += ReadBlock(file); RadarBlips = LoadType<RadarData>();
                dataSize += ReadBlock(file); Zones = LoadType<ZoneData>();
                dataSize += ReadBlock(file); Gangs = LoadType<GangData>();
                dataSize += ReadBlock(file); CarGenerators = LoadType<CarGeneratorData>();
                dataSize += ReadBlock(file); ParticleObjects = LoadType<ParticleData>();
                dataSize += ReadBlock(file); AudioScriptObjects = LoadType<AudioScriptData>();
                dataSize += ReadBlock(file); PlayerInfo = LoadType<PlayerInfo>();
                dataSize += ReadBlock(file); Stats = LoadType<Stats>();
                dataSize += ReadBlock(file); Streaming = LoadType<Streaming>();
                dataSize += ReadBlock(file); PedTypeInfo = LoadType<PedTypeData>();
                numOuterBlocks = NumOuterBlocks;
            }

            // Skip over padding
            int numPaddingBlocks = 0;
            int size = file.Length;
            if (FileFormat.IsXbox) size -= XboxHelper.SignatureLength;
            while (file.Position < size - 4)
            {
                dataSize += ReadBlock(file);
                numPaddingBlocks++;
            }

            // Size checks
            int expectedDataSize = DataSize;
            if (!(FileFormat.IsPS2 && FileFormat.IsJapanese)) expectedDataSize += 1;
            expectedDataSize = (expectedDataSize - 1) & 0x7FFFFFFC;
            int expectedFileSize = expectedDataSize + ((numOuterBlocks + numPaddingBlocks) * sizeof(int)) + sizeof(int);
            if (FileFormat.IsXbox) expectedFileSize += XboxHelper.SignatureLength;
            Debug.Assert(expectedDataSize == dataSize);
            Debug.Assert(expectedFileSize == file.Length);
            Debug.WriteLine("Load successful!");
        }

        protected override void SaveAllData(DataBuffer file)
        {
            int dataSize = 0;
            int size;
            int numOuterBlocks;

            WorkBuff.Reset();
            CheckSum = 0;
            SimpleVars.SizeOfGameInBytes = DataSize;

            if (FileFormat.IsPS2)
            {
                if (!FileFormat.IsJapanese) SimpleVars.SizeOfGameInBytes += 1;
                WorkBuff.Write(SimpleVars, FileFormat);
                SaveObject(Scripts);
                SaveObject(PlayerPeds);
                SaveObject(Garages);
                SaveObject(Vehicles);
                dataSize += WriteBlock(file);
                SaveObject(Objects);
                SaveObject(Paths);
                SaveObject(Cranes);
                dataSize += WriteBlock(file);
                SaveObject(Pickups);
                SaveObject(PhoneInfo);
                SaveObject(RestartPoints);
                SaveObject(RadarBlips);
                SaveObject(Zones);
                SaveObject(Gangs);
                SaveObject(CarGenerators);
                SaveObject(ParticleObjects);
                SaveObject(AudioScriptObjects);
                SaveObject(PlayerInfo);
                SaveObject(Stats);
                SaveObject(Streaming);
                SaveObject(PedTypeInfo);
                dataSize += WriteBlock(file);
                numOuterBlocks = NumOuterBlocksPS2;
            }
            else
            {
                SimpleVars.SizeOfGameInBytes += 1;
                WorkBuff.Write(SimpleVars, FileFormat);
                SaveObject(Scripts); dataSize += WriteBlock(file);
                SaveObject(PlayerPeds); dataSize += WriteBlock(file);
                SaveObject(Garages); dataSize += WriteBlock(file);
                SaveObject(Vehicles); dataSize += WriteBlock(file);
                SaveObject(Objects); dataSize += WriteBlock(file);
                SaveObject(Paths); dataSize += WriteBlock(file);
                SaveObject(Cranes); dataSize += WriteBlock(file);
                SaveObject(Pickups); dataSize += WriteBlock(file);
                SaveObject(PhoneInfo); dataSize += WriteBlock(file);
                SaveObject(RestartPoints); dataSize += WriteBlock(file);
                SaveObject(RadarBlips); dataSize += WriteBlock(file);
                SaveObject(Zones); dataSize += WriteBlock(file);
                SaveObject(Gangs); dataSize += WriteBlock(file);
                SaveObject(CarGenerators); dataSize += WriteBlock(file);
                SaveObject(ParticleObjects); dataSize += WriteBlock(file);
                SaveObject(AudioScriptObjects); dataSize += WriteBlock(file);
                SaveObject(PlayerInfo); dataSize += WriteBlock(file);
                SaveObject(Stats); dataSize += WriteBlock(file);
                SaveObject(Streaming); dataSize += WriteBlock(file);
                SaveObject(PedTypeInfo); dataSize += WriteBlock(file);
                numOuterBlocks = NumOuterBlocks;
            }

            int numPaddingBlocks = 0;
            for (int i = 0; i < MaxNumPaddingBlocks; i++)
            {
                size = (SimpleVars.SizeOfGameInBytes - dataSize - 1) & 0x7FFFFFFC;
                if (size > GetBufferSize())
                {
                    size = GetBufferSize();
                }
                if (size > 4)
                {
                    WorkBuff.Reset();
                    WorkBuff.Pad(size);
                    dataSize += WriteBlock(file);
                    numPaddingBlocks++;
                }
            }

            file.Write(CheckSum);
            if (FileFormat.IsXbox)
            {
                byte[] data = file.GetBytes();
                byte[] sig = XboxHelper.CalculateGameSaveSignature(XboxTitleKey, data, 0, data.Length - 4);
                file.Write(sig);
            }

            // Size checks
            int expectedDataSize = DataSize;
            if (!(FileFormat.IsPS2 && FileFormat.IsJapanese)) expectedDataSize += 1;
            expectedDataSize = (expectedDataSize - 1) & 0x7FFFFFFC;
            int expectedFileSize = expectedDataSize + ((numOuterBlocks + numPaddingBlocks) * sizeof(int)) + sizeof(int);
            if (FileFormat.IsXbox) expectedFileSize += XboxHelper.SignatureLength;
            Debug.Assert(expectedDataSize == dataSize);
            Debug.Assert(expectedFileSize == file.Length);
            Debug.WriteLine("Save successful!");
        }

        protected override bool DetectFileFormat(byte[] data, out FileFormat fmt)
        {
            if (data.Length < 0x10000)
            {
                goto DetectionFailed;
            }

            bool isMobile = false;
            bool isPcOrXbox = false;

            int saveSizeOffset = data.FindFirst(BitConverter.GetBytes(DataSize + 1));
            int saveSizeOffsetJP = data.FindFirst(BitConverter.GetBytes(DataSize));
            int scrOffset = data.FindFirst("SCR\0".GetAsciiBytes());

            if ((saveSizeOffset < 0 && saveSizeOffsetJP < 0) || scrOffset < 0)
            {
                goto DetectionFailed;
            }

            if (scrOffset == 0xB0 && saveSizeOffset == 0x04)
            {
                fmt = FileFormats.PS2_AU;
                return true;
            }
            else if (scrOffset == 0xB8)
            {
                if (saveSizeOffsetJP == 0x04)
                {
                    fmt = FileFormats.PS2_JP;
                    return true;
                }
                else if (saveSizeOffset == 0x04)
                {
                    fmt = FileFormats.PS2;
                    return true;
                }
                else if (saveSizeOffset == 0x34)
                {
                    isMobile = true;
                }
            }
            else if (scrOffset == 0xC4 && saveSizeOffset == 0x44)
            {
                isPcOrXbox = true;
            }

            int sizeOfPlayerPed;
            using (DataBuffer s = new DataBuffer(data))
            {
                int block0Size = s.ReadInt32();
                if (block0Size > s.Length) goto DetectionFailed;
                s.Skip(block0Size + sizeof(int));
                int sizeOfPedPool = s.ReadInt32() - sizeof(int);
                int numPlayerPeds = s.ReadInt32();
                sizeOfPlayerPed = sizeOfPedPool / numPlayerPeds;
            }

            if (isMobile)
            {
                if (sizeOfPlayerPed == 0x63E)
                {
                    fmt = FileFormats.iOS;
                    return true;
                }
                else if (sizeOfPlayerPed == 0x642)
                {
                    fmt = FileFormats.Android;
                    return true;
                }
            }
            else if (isPcOrXbox)
            {
                if (sizeOfPlayerPed == 0x61A)
                {
                    fmt = FileFormats.PC;
                    return true;
                }
                else if (sizeOfPlayerPed == 0x61E)
                {
                    fmt = FileFormats.Xbox;
                    return true;
                }
            }

        DetectionFailed:
            fmt = FileFormat.Default;
            return false;
        }

        protected override int GetBufferSize()
        {
            if (FileFormat.IsPS2)
            {
                return 50000;
            }

            return 55000;
        }

        protected override int GetSize(FileFormat fmt)
        {
            int size = 0;

            // data blocks
            size += SizeOfObject(SimpleVars, fmt);
            size += SizeOfObject(Scripts, fmt) + sizeof(int);
            size += SizeOfObject(PlayerPeds, fmt) + sizeof(int);
            size += SizeOfObject(Garages, fmt) + sizeof(int);
            size += SizeOfObject(Vehicles, fmt) + sizeof(int);
            size += SizeOfObject(Objects, fmt) + sizeof(int);
            size += SizeOfObject(Paths, fmt) + sizeof(int);
            size += SizeOfObject(Cranes, fmt) + sizeof(int);
            size += SizeOfObject(Pickups, fmt) + sizeof(int);
            size += SizeOfObject(PhoneInfo, fmt) + sizeof(int);
            size += SizeOfObject(RestartPoints, fmt) + sizeof(int);
            size += SizeOfObject(RadarBlips, fmt) + sizeof(int);
            size += SizeOfObject(Zones, fmt) + sizeof(int);
            size += SizeOfObject(Gangs, fmt) + sizeof(int);
            size += SizeOfObject(CarGenerators, fmt) + sizeof(int);
            size += SizeOfObject(ParticleObjects, fmt) + sizeof(int);
            size += SizeOfObject(AudioScriptObjects, fmt) + sizeof(int);
            size += SizeOfObject(PlayerInfo, fmt) + sizeof(int);
            size += SizeOfObject(Stats, fmt) + sizeof(int);
            size += SizeOfObject(Streaming, fmt) + sizeof(int);
            size += SizeOfObject(PedTypeInfo, fmt) + sizeof(int);

            // padding blocks
            int totalDataSize = DataSize;
            if (!(fmt.IsPS2 && fmt.IsJapanese)) totalDataSize += 1;
            int numRemaining = ((totalDataSize - 1) & 0x7FFFFFFC) - size;
            int numPadding = (numRemaining / (GetBufferSize() - sizeof(int))) + 1;
            size += numRemaining;

            // "outer" block sizes
            if (fmt.IsPS2) size += sizeof(int) * (NumOuterBlocksPS2 + numPadding);
            else size += sizeof(int) * (NumOuterBlocks + numPadding);

            // checksum
            size += sizeof(int);
            
            // Xbox signature
            if (fmt.IsXbox) size += XboxHelper.SignatureLength;

            return size;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SaveFileGTA3);
        }

        public bool Equals(SaveFileGTA3 other)
        {
            if (other == null)
            {
                return false;
            }

            return SimpleVars.Equals(other.SimpleVars)
                && Scripts.Equals(other.Scripts)
                && PlayerPeds.Equals(other.PlayerPeds)
                && Garages.Equals(other.Garages)
                && Vehicles.Equals(other.Vehicles)
                && Objects.Equals(other.Objects)
                && Paths.Equals(other.Paths)
                && Cranes.Equals(other.Cranes)
                && Pickups.Equals(other.Pickups)
                && PhoneInfo.Equals(other.PhoneInfo)
                && RestartPoints.Equals(other.RestartPoints)
                && RadarBlips.Equals(other.RadarBlips)
                && Zones.Equals(other.Zones)
                && Gangs.Equals(other.Gangs)
                && CarGenerators.Equals(other.CarGenerators)
                && ParticleObjects.Equals(other.ParticleObjects)
                && AudioScriptObjects.Equals(other.AudioScriptObjects)
                && PlayerInfo.Equals(other.PlayerInfo)
                && Stats.Equals(other.Stats)
                && Streaming.Equals(other.Streaming)
                && PedTypeInfo.Equals(other.PedTypeInfo);
        }

        public SaveFileGTA3 DeepClone()
        {
            return new SaveFileGTA3(this);
        }

        public static class FileFormats
        {
            public static readonly FileFormat Android = new FileFormat(
                "Android", "Android", "Android OS",
                GameConsole.Android
            );

            public static readonly FileFormat iOS = new FileFormat(
                "iOS", "iOS", "Apple iOS",
                GameConsole.iOS
            );

            public static readonly FileFormat PC = new FileFormat(
                "PC", "PC", "Windows, MacOS",
                GameConsole.Win32,
                GameConsole.MacOS
            );

            public static readonly FileFormat PS2 = new FileFormat(
                "PS2", "PS2", "PlayStation 2 (North America/Europe)",
                GameConsole.PS2
            );

            public static readonly FileFormat PS2_AU = new FileFormat(
                "PS2_AU", "PS2 (Australia)", "PlayStation 2 (Australia)",
                FileFormatFlags.Australian,
                GameConsole.PS2
            );

            public static readonly FileFormat PS2_JP = new FileFormat(
                "PS2_JP", "PS2 (Japan)", "PlayStation 2 (Japan)",
                FileFormatFlags.Japanese,
                GameConsole.PS2
            );

            public static readonly FileFormat Xbox = new FileFormat(
                "Xbox",
                GameConsole.Xbox
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { Android, iOS, PC, PS2, PS2_AU, PS2_JP, Xbox };
            }
        }
    }

    public enum DataBlock
    {
        SimpleVars,
        Scripts,
        PedPool,
        Garages,
        Vehicles,
        Objects,
        PathFind,
        Cranes,
        Pickups,
        PhoneInfo,
        RestartPoints,
        RadarBlips,
        Zones,
        Gangs,
        CarGenerators,
        ParticleObjects,
        AudioScriptObjects,
        PlayerInfo,
        Stats,
        Streaming,
        PedTypeInfo
    }
}
