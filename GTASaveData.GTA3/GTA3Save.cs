using GTASaveData.Extensions;
using GTASaveData.Helpers;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Represents a saved <i>Grand Theft Auto III</i> game.
    /// </summary>
    public class GTA3Save : GTA3VCSave, ISaveData,
        IEquatable<GTA3Save>, IDeepClonable<GTA3Save>
    {
        private const int MaxNumPaddingBlocks = 4;

        private static readonly byte[] XboxTitleKey =
            { 0xFF, 0x3B, 0x8F, 0x5C, 0xDE, 0x53, 0xF3, 0x25, 0x9E, 0x70, 0x09, 0x54, 0xEF, 0xDC, 0xA8, 0xDC };

        private SimpleVariables m_simpleVars;
        private ScriptData m_scripts;
        private PlayerPedPool m_pedPool;
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

        public PlayerPedPool PlayerPeds
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
            get { return (DateTime) SimpleVars.TimeStamp; }
            set { SimpleVars.TimeStamp = new SystemTime(value); OnPropertyChanged(); }
        }

        bool ISaveData.HasCarGenerators => true;
        ICarGeneratorData ISaveData.CarGenerators
        {
            get { return CarGenerators; }
            set { CarGenerators = (CarGeneratorData) value; OnPropertyChanged(); }
        }

        IReadOnlyList<ISaveDataObject> ISaveData.Blocks => new List<SaveDataObject>()
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

        //private int SizeOfDataInBytes => (FileFormat.IsPS2 && FileFormat.IsJapanese) ? 0x31400 : 0x31401;

        public GTA3Save()
        {
            SimpleVars = new SimpleVariables();
            Scripts = new ScriptData();
            PlayerPeds = new PlayerPedPool();
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

        public GTA3Save(GTA3Save other)
        {
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Scripts = new ScriptData(other.Scripts);
            PlayerPeds = new PlayerPedPool(other.PlayerPeds);
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

        protected override void LoadAllData(StreamBuffer file)
        {
            int totalSize = 0;

            if (FileFormat.IsPS2)
            {
                totalSize += ReadBlock(file);
                SimpleVars = WorkBuff.Read<SimpleVariables>(FileFormat);
                Scripts = LoadType<ScriptData>();
                PlayerPeds = LoadType<PlayerPedPool>();
                Garages = LoadType<GarageData>();
                Vehicles = LoadType<VehiclePool>();
                totalSize += ReadBlock(file);
                Objects = LoadType<ObjectPool>();
                Paths = LoadTypePreAlloc<PathData>();
                Cranes = LoadType<CraneData>();
                totalSize += ReadBlock(file);
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
                totalSize += ReadBlock(file);
            }
            else
            {
                totalSize += ReadBlock(file);
                SimpleVars = WorkBuff.Read<SimpleVariables>(FileFormat);
                Scripts = LoadType<ScriptData>();
                totalSize += ReadBlock(file); PlayerPeds = LoadType<PlayerPedPool>();
                totalSize += ReadBlock(file); Garages = LoadType<GarageData>();
                totalSize += ReadBlock(file); Vehicles = LoadType<VehiclePool>();
                totalSize += ReadBlock(file); Objects = LoadType<ObjectPool>();
                totalSize += ReadBlock(file); Paths = LoadTypePreAlloc<PathData>();
                totalSize += ReadBlock(file); Cranes = LoadType<CraneData>();
                totalSize += ReadBlock(file); Pickups = LoadType<PickupData>();
                totalSize += ReadBlock(file); PhoneInfo = LoadType<PhoneData>();
                totalSize += ReadBlock(file); RestartPoints = LoadType<RestartData>();
                totalSize += ReadBlock(file); RadarBlips = LoadType<RadarData>();
                totalSize += ReadBlock(file); Zones = LoadType<ZoneData>();
                totalSize += ReadBlock(file); Gangs = LoadType<GangData>();
                totalSize += ReadBlock(file); CarGenerators = LoadType<CarGeneratorData>();
                totalSize += ReadBlock(file); ParticleObjects = LoadType<ParticleData>();
                totalSize += ReadBlock(file); AudioScriptObjects = LoadType<AudioScriptData>();
                totalSize += ReadBlock(file); PlayerInfo = LoadType<PlayerInfo>();
                totalSize += ReadBlock(file); Stats = LoadType<Stats>();
                totalSize += ReadBlock(file); Streaming = LoadType<Streaming>();
                totalSize += ReadBlock(file); PedTypeInfo = LoadType<PedTypeData>();
            }

            // Skip over padding
            int dataLen = file.Length;
            if (FileFormat.IsXbox) dataLen -= XboxHelper.SignatureLength;
            while (file.Position < dataLen - 4)
            {
                totalSize += ReadBlock(file);
            }

            Debug.WriteLine("Load successful!");
            Debug.Assert(totalSize == ((SimpleVars.SizeOfGameInBytes - 1) & 0x7FFFFFFC));
        }

        protected override void SaveAllData(StreamBuffer file)
        {
            int totalSize = 0;
            int size;

            WorkBuff.Reset();
            CheckSum = 0;

            if (FileFormat.IsPS2)
            {
                SimpleVars.SizeOfGameInBytes = (FileFormat.IsJapanese) ? 0x31400 : 0x31401;
                WorkBuff.Write(SimpleVars, FileFormat);
                SaveObject(Scripts);
                SaveObject(PlayerPeds);
                SaveObject(Garages);
                SaveObject(Vehicles);
                totalSize += WriteBlock(file);
                SaveObject(Objects);
                SaveObject(Paths);
                SaveObject(Cranes);
                totalSize += WriteBlock(file);
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
                totalSize += WriteBlock(file);
            }
            else
            {
                SimpleVars.SizeOfGameInBytes = 0x31401;
                WorkBuff.Write(SimpleVars, FileFormat);
                SaveObject(Scripts); totalSize += WriteBlock(file);
                SaveObject(PlayerPeds); totalSize += WriteBlock(file);
                SaveObject(Garages); totalSize += WriteBlock(file);
                SaveObject(Vehicles); totalSize += WriteBlock(file);
                SaveObject(Objects); totalSize += WriteBlock(file);
                SaveObject(Paths); totalSize += WriteBlock(file);
                SaveObject(Cranes); totalSize += WriteBlock(file);
                SaveObject(Pickups); totalSize += WriteBlock(file);
                SaveObject(PhoneInfo); totalSize += WriteBlock(file);
                SaveObject(RestartPoints); totalSize += WriteBlock(file);
                SaveObject(RadarBlips); totalSize += WriteBlock(file);
                SaveObject(Zones); totalSize += WriteBlock(file);
                SaveObject(Gangs); totalSize += WriteBlock(file);
                SaveObject(CarGenerators); totalSize += WriteBlock(file);
                SaveObject(ParticleObjects); totalSize += WriteBlock(file);
                SaveObject(AudioScriptObjects); totalSize += WriteBlock(file);
                SaveObject(PlayerInfo); totalSize += WriteBlock(file);
                SaveObject(Stats); totalSize += WriteBlock(file);
                SaveObject(Streaming); totalSize += WriteBlock(file);
                SaveObject(PedTypeInfo); totalSize += WriteBlock(file);
            }

            for (int i = 0; i < MaxNumPaddingBlocks; i++)
            {
                // really stupid 'size remaining' calculation that the game uses
                size = (SimpleVars.SizeOfGameInBytes - totalSize - 1) & 0x7FFFFFFC;
                if (size > GetBufferSize())
                {
                    size = GetBufferSize();
                }
                if (size > 4)
                {
                    WorkBuff.Reset();
                    WorkBuff.Pad(size);
                    totalSize += WriteBlock(file);
                }
            }

            Debug.WriteLine("Save successful!");
            Debug.Assert(totalSize == ((SimpleVars.SizeOfGameInBytes - 1) & 0x7FFFFFFC));

            file.Write(CheckSum);

            if (FileFormat.IsXbox)
            {
                byte[] data = file.GetBytes();
                byte[] sig = XboxHelper.CalculateGameSaveSignature(XboxTitleKey, data, 0, data.Length - 4);
                file.Write(sig);
            }
        }

        protected override bool DetectFileFormat(byte[] data, out FileFormat fmt)
        {
            bool isMobile = false;
            bool isPcOrXbox = false;

            int saveSizeOffset = data.FindFirst(BitConverter.GetBytes(0x31401));
            int saveSizeOffsetJP = data.FindFirst(BitConverter.GetBytes(0x31400));
            int scrOffset = data.FindFirst("SCR\0".GetAsciiBytes());

            if ((saveSizeOffset < 0 && saveSizeOffsetJP < 0) || scrOffset < 0)
            {
                fmt = FileFormat.Default;
                return false;
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
            using (StreamBuffer s = new StreamBuffer(data))
            {
                int block0Size = s.ReadInt32();
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
            int numRemaining = ((SimpleVars.SizeOfGameInBytes - 1) & 0x7FFFFFFC) - size;
            int numPadding = (numRemaining / (GetBufferSize() - sizeof(int))) + 1;
            size += numRemaining;

            size += sizeof(int);
            if (fmt.IsPS2) size += sizeof(int) * (3 + numPadding);
            else size += sizeof(int) * (20 + numPadding);
            if (fmt.IsXbox) size += XboxHelper.SignatureLength;

            return size;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GTA3Save);
        }

        public bool Equals(GTA3Save other)
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

        public GTA3Save DeepClone()
        {
            return new GTA3Save(this);
        }

        public static class FileFormats
        {
            public static readonly FileFormat Android = new FileFormat(
                "Android", GameConsole.Android
            );

            public static readonly FileFormat iOS = new FileFormat(
                "iOS", GameConsole.iOS
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
                "Xbox", GameConsole.Xbox
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
        VehiclePool,
        ObjectPool,
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
