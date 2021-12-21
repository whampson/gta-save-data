using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Helpers;
using GTASaveData.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// A saved <i>Grand Theft Auto III</i> game.
    /// </summary>
    public class SaveFileGTA3 : SaveFileGTA3VC, ISaveFile,
        IEquatable<SaveFileGTA3>, IDeepClonable<SaveFileGTA3>
    {
        private const int MaxNumPaddingBlocks = 4;
        private const int NumOuterBlocks = 20;
        private const int NumOuterBlocksPS2 = 3;
        private const int SizeOfGameDataInBytes = 0x31400;

        private SimpleVariables m_simpleVars;
        private ScriptsBlock m_scripts;
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

        /// <summary>
        /// Miscellaneous variables related to weather, time, the camera,
        /// PS2 settings, and others.
        /// </summary>
        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Mission script data like global variables, active mission scripts,
        /// and swapped building model info.
        /// </summary>
        public ScriptsBlock Scripts
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

        public override string Title
        {
            get { return SimpleVars.LastMissionPassedName; }
            set { SimpleVars.LastMissionPassedName = value; OnPropertyChanged(); }
        }

        public override DateTime TimeStamp
        {
            get { return (DateTime) SimpleVars.TimeStamp; }
            set { SimpleVars.TimeStamp = new SystemTime(value); OnPropertyChanged(); }
        }

        public static SaveFileGTA3 Load(string path)
        {
            return Load<SaveFileGTA3>(path);
        }

        public static SaveFileGTA3 Load(string path, FileFormat fmt)
        {
            return Load<SaveFileGTA3>(path, fmt);
        }

        public SaveFileGTA3()
            : base(Game.GTA3)
        {
            SimpleVars = new SimpleVariables();
            Scripts = new ScriptsBlock();
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

            TimeStamp = DateTime.Now;
        }

        public SaveFileGTA3(SaveFileGTA3 other)
            : base(Game.GTA3)
        {
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Scripts = new ScriptsBlock(other.Scripts);
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

        protected override void Load(DataBuffer file)
        {
            int dataSize = 0;
            int numOuterBlocks;

            if (IsDefinitiveEdition)
            {
                LoadDefinitiveEdition(file);
                return;
            }

            if (IsPS2)
            {
                dataSize += FillWorkBuffer(file);
                SimpleVars = WorkBuff.ReadObject<SimpleVariables>(FileType);
                Scripts = Get<ScriptsBlock>();
                PlayerPeds = Get<PedPool>();
                Garages = Get<GarageData>();
                Vehicles = Get<VehiclePool>();

                dataSize += FillWorkBuffer(file);
                Objects = Get<ObjectPool>();
                Paths = Get<PathData>();
                Cranes = Get<CraneData>();

                dataSize += FillWorkBuffer(file);
                Pickups = Get<PickupData>();
                PhoneInfo = Get<PhoneData>();
                RestartPoints = Get<RestartData>();
                RadarBlips = Get<RadarData>();
                Zones = Get<ZoneData>();
                Gangs = Get<GangData>();
                CarGenerators = Get<CarGeneratorData>();
                ParticleObjects = Get<ParticleData>();
                AudioScriptObjects = Get<AudioScriptData>();
                PlayerInfo = Get<PlayerInfo>();
                Stats = Get<Stats>();
                Streaming = Get<Streaming>();
                PedTypeInfo = Get<PedTypeData>();

                numOuterBlocks = NumOuterBlocksPS2;
            }
            else
            {
                dataSize += FillWorkBuffer(file);
                SimpleVars = WorkBuff.ReadObject<SimpleVariables>(FileType);
                Scripts = Get<ScriptsBlock>();
                dataSize += FillWorkBuffer(file); PlayerPeds = Get<PedPool>();
                dataSize += FillWorkBuffer(file); Garages = Get<GarageData>();
                dataSize += FillWorkBuffer(file); Vehicles = Get<VehiclePool>();
                dataSize += FillWorkBuffer(file); Objects = Get<ObjectPool>();
                dataSize += FillWorkBuffer(file); Paths = Get<PathData>();
                dataSize += FillWorkBuffer(file); Cranes = Get<CraneData>();
                dataSize += FillWorkBuffer(file); Pickups = Get<PickupData>();
                dataSize += FillWorkBuffer(file); PhoneInfo = Get<PhoneData>();
                dataSize += FillWorkBuffer(file); RestartPoints = Get<RestartData>();
                dataSize += FillWorkBuffer(file); RadarBlips = Get<RadarData>();
                dataSize += FillWorkBuffer(file); Zones = Get<ZoneData>();
                dataSize += FillWorkBuffer(file); Gangs = Get<GangData>();
                dataSize += FillWorkBuffer(file); CarGenerators = Get<CarGeneratorData>();
                dataSize += FillWorkBuffer(file); ParticleObjects = Get<ParticleData>();
                dataSize += FillWorkBuffer(file); AudioScriptObjects = Get<AudioScriptData>();
                dataSize += FillWorkBuffer(file); PlayerInfo = Get<PlayerInfo>();
                dataSize += FillWorkBuffer(file); Stats = Get<Stats>();
                dataSize += FillWorkBuffer(file); Streaming = Get<Streaming>();
                dataSize += FillWorkBuffer(file); PedTypeInfo = Get<PedTypeData>();

                numOuterBlocks = NumOuterBlocks;
            }

            // Skip over padding
            int numPaddingBlocks = 0;
            int size = file.Length;
            if (IsXbox) size -= XboxHelper.SignatureLength;
            while (file.Position < size - 4)
            {
                dataSize += FillWorkBuffer(file);
                numPaddingBlocks++;
            }

#if DEBUG
            // Size checks
            int expectedDataSize = SizeOfGameDataInBytes;
            if (!IsPS2Japan) expectedDataSize += 1;
            expectedDataSize = (expectedDataSize - 1) & 0x7FFFFFFC;
            int expectedFileSize = expectedDataSize + ((numOuterBlocks + numPaddingBlocks) * sizeof(int)) + sizeof(int);
            if (IsXbox) expectedFileSize += XboxHelper.SignatureLength;
            Debug.Assert(expectedDataSize == dataSize);
            Debug.Assert(expectedFileSize == file.Length);
#endif
        }

        private void LoadDefinitiveEdition(DataBuffer file)
        {
            long mark = file.Mark();
            SimpleVars = file.ReadObject<SimpleVariables>(FileType);
            Scripts = file.ReadObject<ScriptsBlock>(FileType);

            long size = file.Position - mark;
            Debug.WriteLine("Size: " + size);
        }

        protected override void Save(DataBuffer file)
        {
            if (FileType.FlagDE)
            {
                throw new NotSupportedException("Definitive Edition not supported yet!");
            }

            int dataSize = 0;
            int size;
            int numOuterBlocks;

            WorkBuff.Reset();
            CheckSum = 0;
            SimpleVars.SizeOfGameInBytes = SizeOfGameDataInBytes;   // TODO: consider not overwriting this

            if (IsPS2)
            {
                if (!IsPS2Japan) SimpleVars.SizeOfGameInBytes += 1;
                WorkBuff.Write(SimpleVars, FileType);
                Put(Scripts);
                Put(PlayerPeds);
                Put(Garages);
                Put(Vehicles);
                dataSize += FlushWorkBuffer(file);

                Put(Objects);
                Put(Paths);
                Put(Cranes);
                dataSize += FlushWorkBuffer(file);

                Put(Pickups);
                Put(PhoneInfo);
                Put(RestartPoints);
                Put(RadarBlips);
                Put(Zones);
                Put(Gangs);
                Put(CarGenerators);
                Put(ParticleObjects);
                Put(AudioScriptObjects);
                Put(PlayerInfo);
                Put(Stats);
                Put(Streaming);
                Put(PedTypeInfo);
                dataSize += FlushWorkBuffer(file);

                numOuterBlocks = NumOuterBlocksPS2;
            }
            else
            {
                SimpleVars.SizeOfGameInBytes += 1;
                WorkBuff.Write(SimpleVars, FileType);
                Put(Scripts); dataSize += FlushWorkBuffer(file);
                Put(PlayerPeds); dataSize += FlushWorkBuffer(file);
                Put(Garages); dataSize += FlushWorkBuffer(file);
                Put(Vehicles); dataSize += FlushWorkBuffer(file);
                Put(Objects); dataSize += FlushWorkBuffer(file);
                Put(Paths); dataSize += FlushWorkBuffer(file);
                Put(Cranes); dataSize += FlushWorkBuffer(file);
                Put(Pickups); dataSize += FlushWorkBuffer(file);
                Put(PhoneInfo); dataSize += FlushWorkBuffer(file);
                Put(RestartPoints); dataSize += FlushWorkBuffer(file);
                Put(RadarBlips); dataSize += FlushWorkBuffer(file);
                Put(Zones); dataSize += FlushWorkBuffer(file);
                Put(Gangs); dataSize += FlushWorkBuffer(file);
                Put(CarGenerators); dataSize += FlushWorkBuffer(file);
                Put(ParticleObjects); dataSize += FlushWorkBuffer(file);
                Put(AudioScriptObjects); dataSize += FlushWorkBuffer(file);
                Put(PlayerInfo); dataSize += FlushWorkBuffer(file);
                Put(Stats); dataSize += FlushWorkBuffer(file);
                Put(Streaming); dataSize += FlushWorkBuffer(file);
                Put(PedTypeInfo); dataSize += FlushWorkBuffer(file);

                numOuterBlocks = NumOuterBlocks;
            }

            int numPaddingBlocks = 0;
            for (int i = 0; i < MaxNumPaddingBlocks; i++)
            {
                size = (SimpleVars.SizeOfGameInBytes - dataSize - 1) & 0x7FFFFFFC;
                if (size > GetWorkBufferSize())
                {
                    size = GetWorkBufferSize();
                }
                if (size > 4)
                {
                    WorkBuff.Reset();
                    WorkBuff.Pad(size);
                    dataSize += FlushWorkBuffer(file);
                    numPaddingBlocks++;
                }
            }

            file.Write(CheckSum);
            if (IsXbox)
            {
                byte[] data = file.GetBytes();
                byte[] sig = XboxHelper.GenerateSignature(XboxTitleKey, data, 0, data.Length - 4);
                file.Write(sig);
            }

#if DEBUG
            // Size checks
            int expectedDataSize = SizeOfGameDataInBytes;
            if (!IsPS2Japan) expectedDataSize += 1;
            expectedDataSize = (expectedDataSize - 1) & 0x7FFFFFFC;
            int expectedFileSize = expectedDataSize + ((numOuterBlocks + numPaddingBlocks) * sizeof(int)) + sizeof(int);
            if (IsXbox) expectedFileSize += XboxHelper.SignatureLength;
            Debug.Assert(expectedDataSize == dataSize);
            Debug.Assert(expectedFileSize == file.Length);
#endif
        }

        protected override void OnFileLoad(string path)
        {
            base.OnFileLoad(path);

            if (IsPS2)
            {
                Title = Path.GetFileName(path);
            }

            if (!IsXbox && !IsPC)
            {
                TimeStamp = File.GetLastWriteTime(path);
            }
        }

        protected override void OnFileSave(string path)
        {
            base.OnFileSave(path);

            if (IsPS2)
            {
                Title = Path.GetFileName(path);
            }
        }

        protected override bool DetectFileType(byte[] data, out FileFormat fmt)
        {
            if (data.Length < 0x10000)
            {
                goto DetectionFailed;
            }

            bool isMobile = false;
            bool isPcOrXbox = false;

            int saveSizeOffset = data.FindFirst(BitConverter.GetBytes(SizeOfGameDataInBytes + 1));
            int saveSizeOffsetJP = data.FindFirst(BitConverter.GetBytes(SizeOfGameDataInBytes));
            int scrOffset = data.FindFirst("SCR\0".GetAsciiBytes());

            if ((saveSizeOffset < 0 && saveSizeOffsetJP < 0) || scrOffset < 0)
            {
                goto DetectionFailed;
            }

            if (scrOffset == 0xB0 && saveSizeOffset == 0x04)
            {
                fmt = FileFormats.PS2AU;
                return true;
            }
            else if (scrOffset == 0xB8)
            {
                if (saveSizeOffsetJP == 0x04)
                {
                    fmt = FileFormats.PS2JP;
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

        protected override int GetWorkBufferSize()
        {
            if (IsPS2)
            {
                return 50000;
            }

            return 55000;
        }

        protected override int GetSize(FileFormat fmt)
        {
            if (FileType.FlagDE)
            {
                throw new NotSupportedException("Definitive Edition not supported yet!");
            }

            int size = 0;

            // data blocks
            size += SizeOf(SimpleVars, fmt);
            size += SizeOf(Scripts, fmt) + sizeof(int);
            size += SizeOf(PlayerPeds, fmt) + sizeof(int);
            size += SizeOf(Garages, fmt) + sizeof(int);
            size += SizeOf(Vehicles, fmt) + sizeof(int);
            size += SizeOf(Objects, fmt) + sizeof(int);
            size += SizeOf(Paths, fmt) + sizeof(int);
            size += SizeOf(Cranes, fmt) + sizeof(int);
            size += SizeOf(Pickups, fmt) + sizeof(int);
            size += SizeOf(PhoneInfo, fmt) + sizeof(int);
            size += SizeOf(RestartPoints, fmt) + sizeof(int);
            size += SizeOf(RadarBlips, fmt) + sizeof(int);
            size += SizeOf(Zones, fmt) + sizeof(int);
            size += SizeOf(Gangs, fmt) + sizeof(int);
            size += SizeOf(CarGenerators, fmt) + sizeof(int);
            size += SizeOf(ParticleObjects, fmt) + sizeof(int);
            size += SizeOf(AudioScriptObjects, fmt) + sizeof(int);
            size += SizeOf(PlayerInfo, fmt) + sizeof(int);
            size += SizeOf(Stats, fmt) + sizeof(int);
            size += SizeOf(Streaming, fmt) + sizeof(int);
            size += SizeOf(PedTypeInfo, fmt) + sizeof(int);

            // padding blocks
            int totalDataSize = SizeOfGameDataInBytes;
            if (!(fmt.IsPS2 && fmt.FlagJapan)) totalDataSize += 1;
            int numRemaining = ((totalDataSize - 1) & 0x7FFFFFFC) - size;
            int numPadding = (numRemaining / (GetWorkBufferSize() - sizeof(int))) + 1;
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
                nameof(Android),
                GameSystem.Android
            );

            public static readonly FileFormat iOS = new FileFormat(
                nameof(iOS),
                GameSystem.iOS
            );

            public static readonly FileFormat PC = new FileFormat(
                nameof(PC), "Windows PC", "Microsoft Windows",
                GameSystem.Windows
            );

            public static readonly FileFormat PS2 = new FileFormat(
                nameof(PS2), "PlayStation 2", "PlayStation 2",
                GameSystem.PS2
            );

            public static readonly FileFormat PS2AU = new FileFormat(
                nameof(PS2AU), "PlayStation 2 (Australia)", "PlayStation 2 (Australia release)",
                FileFormatFlags.Australia,
                GameSystem.PS2
            );

            public static readonly FileFormat PS2JP = new FileFormat(
                nameof(PS2JP), "PlayStation 2 (Japan)", "PlayStation 2 (Japan release)",
                FileFormatFlags.Japan,
                GameSystem.PS2
            );

            public static readonly FileFormat Xbox = new FileFormat(
                nameof(Xbox), "Xbox", "Microsoft Xbox",
                GameSystem.Xbox
            );

            public static readonly FileFormat DefinitiveEdition = new FileFormat(
                nameof(DefinitiveEdition), "Definitive Edition", "Grand Theft Auto: The Trilogy - The Definitive Edition (all versions)",
                FileFormatFlags.DE,
                GameSystem.PS4,
                GameSystem.PS5,
                GameSystem.Switch,
                GameSystem.XboxOne,
                GameSystem.Windows
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { Android, iOS, PC, PS2, PS2AU, PS2JP, Xbox, DefinitiveEdition };
            }
        }

        [JsonIgnore] public bool IsAndroid => FileType.IsAndroid;
        [JsonIgnore] public bool IsiOS=> FileType.IsiOS;
        [JsonIgnore] public bool IsPC => FileType.IsPC;
        [JsonIgnore] public bool IsPS2 => FileType.IsPS2;
        [JsonIgnore] public bool IsPS2Japan => FileType.IsPS2 && FileType.FlagJapan;
        [JsonIgnore] public bool IsPS2Australia => FileType.IsPS2 && FileType.FlagAustralia;
        [JsonIgnore] public bool IsXbox => FileType.IsXbox;
        [JsonIgnore] public bool IsDefinitiveEdition => FileType.FlagDE;


        private static readonly byte[] XboxTitleKey =
        {
            0xFF, 0x3B, 0x8F, 0x5C, 0xDE, 0x53, 0xF3, 0x25,
            0x9E, 0x70, 0x09, 0x54, 0xEF, 0xDC, 0xA8, 0xDC
        };

    }
}
