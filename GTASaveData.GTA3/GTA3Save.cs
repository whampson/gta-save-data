using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Represents a <i>Grand Theft Auto III</i> save file.
    /// </summary>
    public class GTA3Save : SaveFile, IGTASaveFile, IEquatable<GTA3Save>, IDisposable
    {
        public const int SaveHeaderSize = 8;
        public const int SizeOfOneGameInBytes = 201729;
        public const int MaxBufferSize = 55000;

        private readonly DataBuffer m_workBuff;
        private bool m_blockSizeChecks;
        private bool m_disposed;
        private int m_checkSum;

        private int BufferSize => (FileFormat.SupportedOnPS2) ? 50000 : 55000;

        private SimpleVariables m_simpleVars;
        private ScriptData m_scripts;
        private DummyObject m_pedPool;  // PedPool
        private GarageData m_garages;
        private DummyObject m_vehiclePool;  // VehiclePool
        private DummyObject m_objectPool;   // ObjectPool
        private PathData m_paths;
        private DummyObject m_cranes;   // Cranes
        private PickupData m_pickups;
        private DummyObject m_phoneInfo;    // PhoneInfo
        private DummyObject m_restartPoints;    // Restarts
        private DummyObject m_radarBlips;   // Radar
        private DummyObject m_zones;    // TheZones
        private GangData m_gangs;
        private CarGeneratorData m_carGenerators;
        private DummyObject m_particles;  // ParticleObjects
        private DummyObject m_audioScript;   // AudioScriptObjects
        private DummyObject m_player;   // PlayerInfo
        private DummyObject m_stats;    // Stats
        private DummyObject m_streaming;    // Streaming
        private PedTypeData m_pedType;

        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public ScriptData ScriptData
        {
            get { return m_scripts; }
            set { m_scripts = value; OnPropertyChanged(); }
        }

        public DummyObject PedPool
        {
            get { return m_pedPool; }
            set { m_pedPool = value; OnPropertyChanged(); }
        }

        public GarageData GarageData
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public DummyObject VehiclePool
        {
            get { return m_vehiclePool; }
            set { m_vehiclePool = value; OnPropertyChanged(); }
        }

        public DummyObject ObjectPool
        {
            get { return m_objectPool; }
            set { m_objectPool = value; OnPropertyChanged(); }
        }

        public PathData PathData
        {
            get { return m_paths; }
            set { m_paths = value; OnPropertyChanged(); }
        }

        public DummyObject CraneData
        {
            get { return m_cranes; }
            set { m_cranes = value; OnPropertyChanged(); }
        }

        public PickupData PickupData
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
        }

        public DummyObject PhoneData
        {
            get { return m_phoneInfo; }
            set { m_phoneInfo = value; OnPropertyChanged(); }
        }

        public DummyObject RestartData
        {
            get { return m_restartPoints; }
            set { m_restartPoints = value; OnPropertyChanged(); }
        }

        public DummyObject RadarData
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; OnPropertyChanged(); }
        }

        public DummyObject ZoneData
        {
            get { return m_zones; }
            set { m_zones = value; OnPropertyChanged(); }
        }

        public GangData GangData
        {
            get { return m_gangs; }
            set { m_gangs = value; OnPropertyChanged(); }
        }

        public CarGeneratorData CarGeneratorData
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; OnPropertyChanged(); }
        }

        public DummyObject ParticleData
        {
            get { return m_particles; }
            set { m_particles = value; OnPropertyChanged(); }
        }

        public DummyObject AudioScriptData
        {
            get { return m_audioScript; }
            set { m_audioScript = value; OnPropertyChanged(); }
        }

        public DummyObject PlayerData
        {
            get { return m_player; }
            set { m_player = value; OnPropertyChanged(); }
        }

        public DummyObject Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public DummyObject StreamingData
        {
            get { return m_streaming; }
            set { m_streaming = value; OnPropertyChanged(); }
        }

        public PedTypeData PedTypeData
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
            get { return SimpleVars.LastMissionPassedName; }
            set { SimpleVars.LastMissionPassedName = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public override DateTime TimeLastSaved
        {
            get { return SimpleVars.TimeLastSaved; }
            set { SimpleVars.TimeLastSaved = new SystemTime(value); OnPropertyChanged(); }
        }

        [JsonIgnore]
        public override IReadOnlyList<SaveDataObject> Blocks => new List<SaveDataObject>()
        {
            SimpleVars,
            ScriptData,
            PedPool,
            GarageData,
            VehiclePool,
            ObjectPool,
            PathData,
            CraneData,
            PickupData,
            PhoneData,
            RestartData,
            RadarData,
            ZoneData,
            GangData,
            CarGeneratorData,
            ParticleData,
            AudioScriptData,
            PlayerData,
            Stats,
            StreamingData,
            PedTypeData
        };

        public GTA3Save()
        {
            m_disposed = false;
            m_workBuff = new DataBuffer(new byte[MaxBufferSize]);

            SimpleVars = new SimpleVariables();
            ScriptData = new ScriptData();
            PedPool = new DummyObject();
            GarageData = new GarageData();
            VehiclePool = new DummyObject();
            ObjectPool = new DummyObject();
            PathData = new PathData();
            CraneData = new DummyObject();
            PickupData = new PickupData();
            PhoneData = new DummyObject();
            RestartData = new DummyObject();
            RadarData = new DummyObject();
            ZoneData = new DummyObject();
            GangData = new GangData();
            CarGeneratorData = new CarGeneratorData();
            ParticleData = new DummyObject();
            AudioScriptData = new DummyObject();
            PlayerData = new DummyObject();
            Stats = new DummyObject();
            StreamingData = new DummyObject();
            PedTypeData = new PedTypeData();

        #if !DEBUG
            BlockSizeChecks = true;
        #endif
        }

        #region Shared between GTA3/VC
        public static int ReadSaveHeader(DataBuffer buf, string tag)
        {
            string readTag = buf.ReadString(4);
            int size = buf.ReadInt32();

            Debug.Assert(readTag == tag, "Invalid block tag (expected: {0}, actual: {1})", tag, readTag);
            return size;
        }

        public static void WriteSaveHeader(DataBuffer buf, string tag, int size)
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
            // Pre-allocate object space, only applies to some types
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
                
            preSize = m_workBuff.Position;
            m_workBuff.Skip(4);
            
            size = Serializer.Write(m_workBuff, o, FileFormat);
            postData = m_workBuff.Position;

            m_workBuff.Seek(preSize);
            m_workBuff.Write(size);
            m_workBuff.Seek(postData);
            m_workBuff.Align4Bytes();

            Debug.WriteLine("{0}: {1} bytes written", o.GetType().Name, size);
        }

        private int ReadBlock(DataBuffer file)
        {
            file.MarkPosition();
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

        private int WriteBlock(DataBuffer file)
        {
            file.MarkPosition();

            byte[] data = m_workBuff.GetBytesUpToCursor();
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

        protected override void LoadAllData(DataBuffer file)
        {
            int totalSize = 0;

            totalSize += ReadBlock(file);
            LoadSimpleVars();                
            ScriptData = Load<ScriptData>();
            totalSize += ReadBlock(file); PedPool = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); GarageData = Load<GarageData>();
            totalSize += ReadBlock(file); VehiclePool = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); ObjectPool = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); PathData = LoadPreAlloc<PathData>();
            totalSize += ReadBlock(file); CraneData = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); PickupData = Load<PickupData>();
            totalSize += ReadBlock(file); PhoneData = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); RestartData = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); RadarData = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); ZoneData = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); GangData = Load<GangData>();
            totalSize += ReadBlock(file); CarGeneratorData = Load<CarGeneratorData>();
            totalSize += ReadBlock(file); ParticleData = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); AudioScriptData = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); PlayerData = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); Stats = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); StreamingData = LoadPreAlloc<DummyObject>();
            totalSize += ReadBlock(file); PedTypeData = Load<PedTypeData>();

            while (file.Position < file.Length - 4)
            {
                totalSize += ReadBlock(file);
            }

            Debug.WriteLine("Load successful!");
            Debug.WriteLine("Size of game data: {0} bytes", totalSize);
            Debug.Assert(totalSize == (SizeOfOneGameInBytes & 0xFFFFFFFE));
        }

        protected override void SaveAllData(DataBuffer file)
        {
            int totalSize = 0;
            int size;

            m_workBuff.Reset();
            m_checkSum = 0;

            SaveSimpleVars();
            Save(ScriptData); totalSize += WriteBlock(file);
            Save(PedPool); totalSize += WriteBlock(file);
            Save(GarageData); totalSize += WriteBlock(file);
            Save(VehiclePool); totalSize += WriteBlock(file);
            Save(ObjectPool); totalSize += WriteBlock(file);
            Save(PathData); totalSize += WriteBlock(file);
            Save(CraneData); totalSize += WriteBlock(file);
            Save(PickupData); totalSize += WriteBlock(file);
            Save(PhoneData); totalSize += WriteBlock(file);
            Save(RestartData); totalSize += WriteBlock(file);
            Save(RadarData); totalSize += WriteBlock(file);
            Save(ZoneData); totalSize += WriteBlock(file);
            Save(GangData); totalSize += WriteBlock(file);
            Save(CarGeneratorData); totalSize += WriteBlock(file);
            Save(ParticleData); totalSize += WriteBlock(file);
            Save(AudioScriptData); totalSize += WriteBlock(file);
            Save(PlayerData); totalSize += WriteBlock(file);
            Save(Stats); totalSize += WriteBlock(file);
            Save(StreamingData); totalSize += WriteBlock(file);
            Save(PedTypeData); totalSize += WriteBlock(file);

            for (int i = 0; i < 4; i++)
            {
                size = DataBuffer.Align4Bytes(SizeOfOneGameInBytes - totalSize - 4);
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

        protected override bool DetectFileFormat(byte[] data, out SaveFileFormat fmt)
        {
            bool isMobile = false;
            bool isPcOrXbox = false;

            int fileId = data.FindFirst(BitConverter.GetBytes(0x31401));
            int fileIdJP = data.FindFirst(BitConverter.GetBytes(0x31400));
            int scr = data.FindFirst("SCR\0".GetAsciiBytes());

            if (fileId == -1 || scr == -1)
            {
                fmt = SaveFileFormat.Default;
                return false;
            }

            int blk1Size;
            using (DataBuffer wb = new DataBuffer(data))
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

            fmt = SaveFileFormat.Default;
            return false;
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
                && ScriptData.Equals(other.ScriptData)
                && PedPool.Equals(other.PedPool)
                && GarageData.Equals(other.GarageData)
                && VehiclePool.Equals(other.VehiclePool)
                && ObjectPool.Equals(other.ObjectPool)
                //&& Paths.Equals(other.Paths)  // TODO: equality fails due to list padding during save
                && CraneData.Equals(other.CraneData)
                && PickupData.Equals(other.PickupData)
                && PhoneData.Equals(other.PhoneData)
                && RestartData.Equals(other.RestartData)
                && RadarData.Equals(other.RadarData)
                && ZoneData.Equals(other.ZoneData)
                && GangData.Equals(other.GangData)
                && CarGeneratorData.Equals(other.CarGeneratorData)
                && ParticleData.Equals(other.ParticleData)
                && AudioScriptData.Equals(other.AudioScriptData)
                && PlayerData.Equals(other.PlayerData)
                && Stats.Equals(other.Stats)
                && StreamingData.Equals(other.StreamingData)
                && PedTypeData.Equals(other.PedTypeData);
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
            public static readonly SaveFileFormat Android = new SaveFileFormat(
                "Android", "Android", "Android",
                new GameConsole(ConsoleType.Android)
            );

            public static readonly SaveFileFormat iOS = new SaveFileFormat(
                "iOS", "iOS", "iOS",
                new GameConsole(ConsoleType.iOS)
            );

            public static readonly SaveFileFormat PC = new SaveFileFormat(
                "PC", "PC", "Windows, macOS",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.MacOS)
            );

            public static readonly SaveFileFormat PS2_AU = new SaveFileFormat(
                "PS2_AU", "PS2 (Australia)", "PlayStation 2 (PAL Australia)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.Australia)
            );

            public static readonly SaveFileFormat PS2_JP = new SaveFileFormat(
                "PS2_JP", "PS2 (Japan)", "PlayStation 2 (NTSC-J)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.Japan)
            );

            public static readonly SaveFileFormat PS2_NAEU = new SaveFileFormat(
                "PS2_NAEU", "PS2", "PlayStation 2 (NTSC-U/C, PAL Europe)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.NorthAmerica | ConsoleFlags.Europe)
            );

            public static readonly SaveFileFormat Xbox = new SaveFileFormat(
                "Xbox", "Xbox", "Xbox",
                new GameConsole(ConsoleType.Xbox)
            );

            public static SaveFileFormat[] GetAll()
            {
                return new SaveFileFormat[] { Android, iOS, PC, PS2_AU, PS2_JP, PS2_NAEU, Xbox };
            }
        }
    }
}
