using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Represents a <i>Grand Theft Auto III</i> save file.
    /// </summary>
    public class GTA3Save : SaveFile, ISaveFile, IEquatable<GTA3Save>, IDisposable
    {
        public const int SaveHeaderSize = 8;
        public const int SizeOfOneGameInBytes = 201729;
        public const int MaxBufferSize = 55000;

        private readonly StreamBuffer m_workBuff;
        private bool m_blockSizeChecks;
        private bool m_disposed;
        private int m_checkSum;

        private int BufferSize => (FileFormat.PS2) ? 50000 : 55000;

        private SimpleVariables m_simpleVars;
        private ScriptData m_scripts;
        private Dummy m_pedPool;  // PedPool
        private GarageData m_garages;
        private Dummy m_vehiclePool;  // VehiclePool
        private Dummy m_objectPool;   // ObjectPool
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

        public Dummy PedPool
        {
            get { return m_pedPool; }
            set { m_pedPool = value; OnPropertyChanged(); }
        }

        public GarageData Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public Dummy VehiclePool
        {
            get { return m_vehiclePool; }
            set { m_vehiclePool = value; OnPropertyChanged(); }
        }

        public Dummy ObjectPool
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

        [JsonIgnore]
        public bool BlockSizeChecks
        {
            get { return m_blockSizeChecks; }
            set { m_blockSizeChecks = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public override string Name
        {
            get { return SimpleVars.SaveName; }
            set { SimpleVars.SaveName = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public override DateTime TimeLastSaved
        {
            get { return (DateTime) SimpleVars.TimeLastSaved; }
            set { SimpleVars.TimeLastSaved = new SystemTime(value); OnPropertyChanged(); }
        }

        [JsonIgnore]
        public override IReadOnlyList<SaveDataObject> Blocks => new List<SaveDataObject>()
        {
            SimpleVars,
            Scripts,
            PedPool,
            Garages,
            VehiclePool,
            ObjectPool,
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
            m_disposed = false;
            m_workBuff = new StreamBuffer(new byte[MaxBufferSize]);

            SimpleVars = new SimpleVariables();
            Scripts = new ScriptData();
            PedPool = new Dummy();
            Garages = new GarageData();
            VehiclePool = new Dummy();
            ObjectPool = new Dummy();
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

        #if !DEBUG
            BlockSizeChecks = true;
        #endif
        }
        
        // TODO: move to base class (GTA3VCSave)
        #region Shared between GTA3/VC
        public static int ReadSaveHeader(StreamBuffer buf, string tag)
        {
            string readTag = buf.ReadString(4);
            int size = buf.ReadInt32();

            Debug.Assert(readTag == tag, "Invalid block tag (expected: {0}, actual: {1})", tag, readTag);
            return size;
        }

        public static void WriteSaveHeader(StreamBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4, zeroTerminate: true);
            buf.Write(size);
        }

        private void LoadSimpleVars()
        {
            SimpleVars = m_workBuff.Read<SimpleVariables>(FileFormat);
        }

        private void SaveSimpleVars()
        {
            SimpleVars.SaveSize = SizeOfOneGameInBytes;
            m_workBuff.Write(SimpleVars, FileFormat);
        }

        private T Load<T>() where T : SaveDataObject, new()
        {
            T obj = new T();
            Load(obj);

            return obj;
        }
        private int Load<T>(T obj) where T : SaveDataObject
        {
            m_workBuff.ReadInt32();     // Ignore size
            int bytesRead = Serializer.Read(obj, m_workBuff, FileFormat);

            Debug.WriteLine("{0}: {1} bytes read", typeof(T).Name, bytesRead);
            return bytesRead;
        }

        private T LoadPreAlloc<T>() where T : SaveDataObject
        {
            int size = m_workBuff.ReadInt32();
            if (!(Activator.CreateInstance(typeof(T), size) is T obj))
            {
                throw new SerializationException("Object cannot be pre-allocated.");
            }
            Debug.WriteLine("{0}: {1} bytes pre-allocated", typeof(T).Name, size);

            int bytesRead = Serializer.Read(obj, m_workBuff, FileFormat);
            Debug.WriteLine("{0}: {1} bytes read", typeof(T).Name, bytesRead);

            return obj;
        }

        private void Save(SaveDataObject o)
        {
            int size, preSize, postData;
                
            preSize = m_workBuff.Cursor;
            m_workBuff.Skip(4);
            
            size = Serializer.Write(m_workBuff, o, FileFormat);
            postData = m_workBuff.Cursor;

            m_workBuff.Seek(preSize);
            m_workBuff.Write(size);
            m_workBuff.Seek(postData);
            m_workBuff.Align4Bytes();

            Debug.WriteLine("{0}: {1} bytes written", o.GetType().Name, size);
        }

        private int ReadBlock(StreamBuffer file)
        {
            file.MarkCurrentPosition();
            m_workBuff.Reset();

            int size = file.ReadInt32();
            if ((uint) size > BufferSize)
            {
                Debug.WriteLine("Maximum block size exceeded! (value = {0}, max = {1})", (uint) size, BufferSize);
                if (BlockSizeChecks)
                {
                    throw BlockSizeExceededException((uint) size, BufferSize);
                }
            }

            m_workBuff.Write(file.ReadBytes(size));
            Debug.Assert(file.Offset == size + 4);

            m_workBuff.Reset();
            return size;
        }

        private int WriteBlock(StreamBuffer file)
        {
            file.MarkCurrentPosition();

            byte[] data = m_workBuff.GetBytes();
            int size = data.Length;
            if ((uint) size > BufferSize)
            {
                Debug.WriteLine("Maximum block size exceeded! (value = {0}, max = {1})", (uint) size, BufferSize);
                if (BlockSizeChecks)
                {
                    throw BlockSizeExceededException((uint) size, BufferSize);
                }
            }

            file.Write(size);
            file.Write(data);
            file.Align4Bytes();

            Debug.Assert(file.Offset == size + 4);

            m_checkSum += BitConverter.GetBytes(size).Sum(x => x);
            m_checkSum += data.Sum(x => x);

            m_workBuff.Reset();
            return size;
        }
        #endregion

        protected override void LoadAllData(StreamBuffer file)
        {
            int totalSize = 0;

            totalSize += ReadBlock(file);
            LoadSimpleVars();                
            Scripts = Load<ScriptData>();
            totalSize += ReadBlock(file); PedPool = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Garages = Load<GarageData>();
            totalSize += ReadBlock(file); VehiclePool = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); ObjectPool = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Paths = LoadPreAlloc<PathData>();
            totalSize += ReadBlock(file); Cranes = Load<CraneData>();
            totalSize += ReadBlock(file); Pickups = Load<PickupData>();
            totalSize += ReadBlock(file); PhoneInfo = Load<PhoneData>();
            totalSize += ReadBlock(file); RestartPoints = Load<RestartData>();
            totalSize += ReadBlock(file); RadarBlips = Load<RadarData>();
            totalSize += ReadBlock(file); Zones = Load<ZoneData>();
            totalSize += ReadBlock(file); Gangs = Load<GangData>();
            totalSize += ReadBlock(file); CarGenerators = Load<CarGeneratorData>();
            totalSize += ReadBlock(file); ParticleObjects = Load<ParticleData>();
            totalSize += ReadBlock(file); AudioScriptObjects = Load<AudioScriptData>();
            totalSize += ReadBlock(file); PlayerInfo = Load<PlayerInfo>();
            totalSize += ReadBlock(file); Stats = Load<Stats>();
            totalSize += ReadBlock(file); Streaming = Load<Streaming>();
            totalSize += ReadBlock(file); PedTypeInfo = Load<PedTypeData>();

            while (file.Cursor < file.Length - 4)
            {
                totalSize += ReadBlock(file);
            }

            Debug.WriteLine("Load successful!");
            Debug.WriteLine("Size of game data: {0} bytes", totalSize);
            Debug.Assert(totalSize == (SizeOfOneGameInBytes & 0xFFFFFFFE));
        }

        protected override void SaveAllData(StreamBuffer file)
        {
            int totalSize = 0;
            int size;

            m_workBuff.Reset();
            m_checkSum = 0;

            SaveSimpleVars();
            Save(Scripts); totalSize += WriteBlock(file);
            Save(PedPool); totalSize += WriteBlock(file);
            Save(Garages); totalSize += WriteBlock(file);
            Save(VehiclePool); totalSize += WriteBlock(file);
            Save(ObjectPool); totalSize += WriteBlock(file);
            Save(Paths); totalSize += WriteBlock(file);
            Save(Cranes); totalSize += WriteBlock(file);
            Save(Pickups); totalSize += WriteBlock(file);
            Save(PhoneInfo); totalSize += WriteBlock(file);
            Save(RestartPoints); totalSize += WriteBlock(file);
            Save(RadarBlips); totalSize += WriteBlock(file);
            Save(Zones); totalSize += WriteBlock(file);
            Save(Gangs); totalSize += WriteBlock(file);
            Save(CarGenerators); totalSize += WriteBlock(file);
            Save(ParticleObjects); totalSize += WriteBlock(file);
            Save(AudioScriptObjects); totalSize += WriteBlock(file);
            Save(PlayerInfo); totalSize += WriteBlock(file);
            Save(Stats); totalSize += WriteBlock(file);
            Save(Streaming); totalSize += WriteBlock(file);
            Save(PedTypeInfo); totalSize += WriteBlock(file);

            for (int i = 0; i < 4; i++)
            {
                size = StreamBuffer.Align4Bytes(SizeOfOneGameInBytes - totalSize - 4);
                if (size > BufferSize)
                {
                    size = BufferSize;
                }
                if (size > 4)
                {
                    if (Padding != PaddingType.Default)
                    {
                        m_workBuff.Reset();
                        m_workBuff.Write(GenerateSpecialPadding(size));
                    }
                    m_workBuff.Seek(size);
                    totalSize += WriteBlock(file);
                }
            }

            file.Write(m_checkSum);

            Debug.WriteLine("Save successful!");
            Debug.WriteLine("Size of game data: {0} bytes", totalSize);
            Debug.Assert(totalSize == (SizeOfOneGameInBytes & 0xFFFFFFFE));
        }

        protected override bool DetectFileFormat(byte[] data, out DataFormat fmt)
        {
            bool isMobile = false;
            bool isPcOrXbox = false;

            int fileId = data.FindFirst(BitConverter.GetBytes(0x31401));
            int fileIdJP = data.FindFirst(BitConverter.GetBytes(0x31400));
            int scr = data.FindFirst("SCR\0".GetAsciiBytes());

            if (fileId == -1 || scr == -1)
            {
                fmt = DataFormat.Default;
                return false;
            }

            int blk1Size;
            using (StreamBuffer wb = new StreamBuffer(data))
            {
                wb.Skip(wb.ReadInt32());
                blk1Size = wb.ReadInt32();
            }

            if (scr == 0xB0 && fileId == 0x04)
            {
                fmt = FileFormats.PS2_AU;
                return true;
            }
            else if (scr == 0xB8)
            {
                if (fileIdJP == 0x04)
                {
                    fmt = FileFormats.PS2_JP;
                    return true;
                }
                else if (fileId == 0x04)
                {
                    fmt = FileFormats.PS2_NAEU;
                    return true;
                }
                else if (fileId == 0x34)
                {
                    isMobile = true;
                }
            }
            else if (scr == 0xC4 && fileId == 0x44)
            {
                isPcOrXbox = true;
            }

            if (isMobile)
            {
                if (blk1Size == 0x648)
                {
                    fmt = FileFormats.iOS;
                    return true;
                }
                else if (blk1Size == 0x64C)
                {
                    fmt = FileFormats.Android;
                    return true;
                }
            }
            else if (isPcOrXbox)
            {
                if (blk1Size == 0x624)
                {
                    fmt = FileFormats.PC;
                    return true;
                }
                else if (blk1Size == 0x628)
                {
                    fmt = FileFormats.Xbox;
                    return true;
                }
            }

            fmt = DataFormat.Default;
            return false;
        }

        protected override int GetSize(DataFormat fmt)
        {
            // TODO:
            throw new NotImplementedException();
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
                && PedPool.Equals(other.PedPool)
                && Garages.Equals(other.Garages)
                && VehiclePool.Equals(other.VehiclePool)
                && ObjectPool.Equals(other.ObjectPool)
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

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_workBuff.Dispose();
                m_disposed = true;
            }
        }

        private SerializationException BlockSizeExceededException(uint value, int max)
        {
            return new SerializationException(Strings.Error_Serialization_BlockSizeExceeded, value, max);
        }

        public static class FileFormats
        {
            public static readonly DataFormat Android = new DataFormat(
                "Android", "Android", "Android",
                new GameConsole(ConsoleType.Android)
            );

            public static readonly DataFormat iOS = new DataFormat(
                "iOS", "iOS", "iOS",
                new GameConsole(ConsoleType.iOS)
            );

            public static readonly DataFormat PC = new DataFormat(
                "PC", "PC", "Windows, macOS",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.MacOS)
            );

            public static readonly DataFormat PS2_AU = new DataFormat(
                "PS2_AU", "PS2 (Australia)", "PlayStation 2 (PAL Australia)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.Australia)
            );

            public static readonly DataFormat PS2_JP = new DataFormat(
                "PS2_JP", "PS2 (Japan)", "PlayStation 2 (NTSC-J)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.Japan)
            );

            public static readonly DataFormat PS2_NAEU = new DataFormat(
                "PS2_NAEU", "PS2", "PlayStation 2 (NTSC-U/C, PAL Europe)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.NorthAmerica | ConsoleFlags.Europe)
            );

            public static readonly DataFormat Xbox = new DataFormat(
                "Xbox", "Xbox", "Xbox",
                new GameConsole(ConsoleType.Xbox)
            );

            public static DataFormat[] GetAll()
            {
                return new DataFormat[] { Android, iOS, PC, PS2_AU, PS2_JP, PS2_NAEU, Xbox };
            }
        }

        public static bool IsAusrtalianPS2(DataFormat fmt)
        {
            return fmt.IsSupportedOn(ConsoleType.PS2, ConsoleFlags.Australia);
        }

        public static bool IsJapanesePS2(DataFormat fmt)
        {
            return fmt.IsSupportedOn(ConsoleType.PS2, ConsoleFlags.Japan);
        }
    }
}
