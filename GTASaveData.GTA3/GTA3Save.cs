using GTASaveData.Extensions;
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
    public class GTA3Save : GTA3VCSave, ISaveData, IEquatable<GTA3Save>
    {
        public const int SizeOfGameInBytes = 201728;
        public const int BufferSizePC = 55000;
        public const int BufferSizePS2 = 50000;
        public const int MaxNumPaddingBlocks = 4;

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
            get { return SimpleVars.SaveName; }
            set { SimpleVars.SaveName = value; OnPropertyChanged(); }
        }

        public override DateTime TimeStamp
        {
            get { return (DateTime) SimpleVars.TimeLastSaved; }
            set { SimpleVars.TimeLastSaved = new SystemTime(value); OnPropertyChanged(); }
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

        protected override void OnReading()
        {
            base.OnReading();
            
            BufferSize = (FileFormat.IsPS2) ? BufferSizePS2 : BufferSizePC;
            CreateWorkBuff();
        }

        protected override void OnWriting()
        {
            base.OnWriting();
            
            BufferSize = (FileFormat.IsPS2) ? BufferSizePS2 : BufferSizePC;
            CreateWorkBuff();
        }

        protected override void LoadAllData(StreamBuffer file)
        {
            int totalSize = 0;

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

            // Skip over padding
            while (file.Position < file.Length - 4)
            {
                totalSize += ReadBlock(file);
            }

            Debug.WriteLine("Load successful!");
            Debug.Assert(totalSize == SizeOfGameInBytes);
        }

        protected override void SaveAllData(StreamBuffer file)
        {
            int totalSize = 0;
            int size;

            WorkBuff.Reset();
            CheckSum = 0;

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

            for (int i = 0; i < MaxNumPaddingBlocks; i++)
            {
                size = StreamBuffer.Align4((SizeOfGameInBytes - 3) - totalSize);
                if (size > BufferSize)
                {
                    size = BufferSize;
                }
                if (size > 4)
                {
                    WorkBuff.Reset();
                    WorkBuff.Pad(size);
                    totalSize += WriteBlock(file);
                }
            }

            file.Write(CheckSum);

            Debug.WriteLine("Save successful!");
            Debug.Assert(totalSize == SizeOfGameInBytes);
        }

        protected override bool DetectFileFormat(byte[] data, out FileFormat fmt)
        {
            bool isMobile = false;
            bool isPcOrXbox = false;

            int saveSizeOffset = data.FindFirst(BitConverter.GetBytes(0x31401));
            int saveSizeOffsetJP = data.FindFirst(BitConverter.GetBytes(0x31400));
            int scrOffset = data.FindFirst("SCR\0".GetAsciiBytes());

            if (saveSizeOffset < 0 || scrOffset < 0)
            {
                // Invalid
                fmt = FileFormat.Default;
                return false;
            }

            int sizeOfPlayerPed;
            using (StreamBuffer s = new StreamBuffer(data))
            {
                int block0Size = s.ReadInt32();
                s.Skip(block0Size + 4);
                int sizeOfPedPool = s.ReadInt32() - 4;
                int numPlayerPeds = s.ReadInt32();
                sizeOfPlayerPed = sizeOfPedPool / numPlayerPeds;
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

            // Invalid
            fmt = FileFormat.Default;
            return false;
        }

        protected override int GetSize(FileFormat fmt)
        {
            // TODO:
            throw SizeNotDefined(fmt);
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

        public static class FileFormats
        {
            public static readonly FileFormat Android = new FileFormat(
                "Android", GameConsole.Android
            );

            public static readonly FileFormat iOS = new FileFormat(
                "iOS", GameConsole.iOS
            );

            public static readonly FileFormat PC = new FileFormat(
                "PC", "PC", "Windows, macOS",
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
