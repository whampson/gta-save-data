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
    public class GTA3Save : SaveFile, IGTASaveFile, IEquatable<GTA3Save>, IDisposable
    {
        public const int SaveHeaderSize = 8;
        public const int SizeOfOneGameInBytes = 201729;
        public const int MaxBufferSize = 55000;

        private readonly DataBuffer m_workBuff;
        private int m_bufferSize => (FileFormat.SupportedOnPS2) ? 50000 : 55000;
        private int m_checkSum;
        private bool m_blockSizeChecks;
        private bool m_disposed;

        private SimpleVariables m_simpleVars;   // SimpleVariables
        private TheScripts m_scripts;  // TheScripts
        private DummyObject m_pedPool;  // PedPool
        private DummyObject m_garages;  // Garages
        private DummyObject m_vehiclePool;  // VehiclePool
        private DummyObject m_objectPool;   // ObjectPool
        private DummyObject m_paths;    // PathFind
        private DummyObject m_cranes;   // Cranes
        private DummyObject m_pickups;  // Pickups
        private DummyObject m_phoneInfo;    // PhoneInfo
        private DummyObject m_restartPoints;    // Restarts
        private DummyObject m_radarBlips;   // Radar
        private DummyObject m_zones;    // TheZones
        private DummyObject m_gangData; // Gangs
        private DummyObject m_carGenerators;    // TheCarGenerators
        private DummyObject m_particleObjects;  // ParticleObjects
        private DummyObject m_audioScriptObjects;   // AudioScriptObjects
        private DummyObject m_playerInfo;   // PlayerInfo
        private DummyObject m_stats;    // Stats
        private DummyObject m_streaming;    // Streaming
        private DummyObject m_pedTypeInfo;  // PedTypeInfo

        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public TheScripts Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; OnPropertyChanged(); }
        }

        public DummyObject PedPool
        {
            get { return m_pedPool; }
            set { m_pedPool = value; OnPropertyChanged(); }
        }

        public DummyObject Garages
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

        public DummyObject Paths
        {
            get { return m_paths; }
            set { m_paths = value; OnPropertyChanged(); }
        }

        public DummyObject Cranes
        {
            get { return m_cranes; }
            set { m_cranes = value; OnPropertyChanged(); }
        }

        public DummyObject Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
        }

        public DummyObject PhoneInfo
        {
            get { return m_phoneInfo; }
            set { m_phoneInfo = value; OnPropertyChanged(); }
        }

        public DummyObject RestartPoints
        {
            get { return m_restartPoints; }
            set { m_restartPoints = value; OnPropertyChanged(); }
        }

        public DummyObject RadarBlips
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; OnPropertyChanged(); }
        }

        public DummyObject Zones
        {
            get { return m_zones; }
            set { m_zones = value; OnPropertyChanged(); }
        }

        public DummyObject GangData
        {
            get { return m_gangData; }
            set { m_gangData = value; OnPropertyChanged(); }
        }

        public DummyObject CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; OnPropertyChanged(); }
        }

        public DummyObject ParticleObjects
        {
            get { return m_particleObjects; }
            set { m_particleObjects = value; OnPropertyChanged(); }
        }

        public DummyObject AudioScriptObjects
        {
            get { return m_audioScriptObjects; }
            set { m_audioScriptObjects = value; OnPropertyChanged(); }
        }

        public DummyObject PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; OnPropertyChanged(); }
        }

        public DummyObject Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public DummyObject Streaming
        {
            get { return m_streaming; }
            set { m_streaming = value; OnPropertyChanged(); }
        }

        public DummyObject PedTypeInfo
        {
            get { return m_pedTypeInfo; }
            set { m_pedTypeInfo = value; OnPropertyChanged(); }
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
            GangData,
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
            m_workBuff = new DataBuffer(new byte[MaxBufferSize]);

            SimpleVars = new SimpleVariables();
            Scripts = new TheScripts();
            PedPool = new DummyObject();
            Garages = new DummyObject();
            VehiclePool = new DummyObject();
            ObjectPool = new DummyObject();
            Paths = new DummyObject();
            Cranes = new DummyObject();
            Pickups = new DummyObject();
            PhoneInfo = new DummyObject();
            RestartPoints = new DummyObject();
            RadarBlips = new DummyObject();
            Zones = new DummyObject();
            GangData = new DummyObject();
            CarGenerators = new DummyObject();
            ParticleObjects = new DummyObject();
            AudioScriptObjects = new DummyObject();
            PlayerInfo = new DummyObject();
            Stats = new DummyObject();
            Streaming = new DummyObject();
            PedTypeInfo = new DummyObject();

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

        private DummyObject LoadDummy()
        {
            int size = m_workBuff.ReadInt32();
            var o = new DummyObject(size);
            int bytesRead = Serializer.Read(o, m_workBuff, FileFormat);

            Debug.Assert(bytesRead == size);
            return o;
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

        private T LoadData<T>() where T : SaveDataObject, new()
        {
            int size = m_workBuff.ReadInt32();
            int bytesRead = Serializer.Read(m_workBuff, FileFormat, out T obj);

            Debug.Assert(bytesRead == size);
            return obj;
        }

        private void SaveData(SaveDataObject o)
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
        }

        private int ReadBlock(DataBuffer file)
        {
            file.MarkPosition();
            m_workBuff.Reset();

            int size = file.ReadInt32();
            if ((uint) size > m_bufferSize)
            {
                Debug.WriteLine("Maximum block size exceeded! (value = {0}, max = {1})", (uint) size, m_bufferSize);
                if (BlockSizeChecks)
                {
                    throw BlockSizeExceededException((uint) size, m_bufferSize);
                }
            }

            m_workBuff.Write(file.ReadBytes(size));

            Debug.Assert(file.Offset == size + 4);
            Debug.WriteLine("Read {0} bytes of block data.", size);

            m_workBuff.Reset();
            return size;
        }

        private int WriteBlock(DataBuffer file)
        {
            file.MarkPosition();

            byte[] data = m_workBuff.GetBytesUpToCursor();
            int size = data.Length;
            if ((uint) size > m_bufferSize)
            {
                Debug.WriteLine("Maximum block size exceeded! (value = {0}, max = {1})", (uint) size, m_bufferSize);
                if (BlockSizeChecks)
                {
                    throw BlockSizeExceededException((uint) size, m_bufferSize);
                }
            }

            file.Write(size);
            file.Write(data);
            file.Align4Bytes();

            Debug.Assert(file.Offset == size + 4);
            Debug.WriteLine("Wrote {0} bytes of block data.", size);

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
            Scripts = LoadData<TheScripts>();
            totalSize += ReadBlock(file); PedPool = LoadDummy();
            totalSize += ReadBlock(file); Garages = LoadDummy();
            totalSize += ReadBlock(file); VehiclePool = LoadDummy();
            totalSize += ReadBlock(file); ObjectPool = LoadDummy();
            totalSize += ReadBlock(file); Paths = LoadDummy();
            totalSize += ReadBlock(file); Cranes = LoadDummy();
            totalSize += ReadBlock(file); Pickups = LoadDummy();
            totalSize += ReadBlock(file); PhoneInfo = LoadDummy();
            totalSize += ReadBlock(file); RestartPoints = LoadDummy();
            totalSize += ReadBlock(file); RadarBlips = LoadDummy();
            totalSize += ReadBlock(file); Zones = LoadDummy();
            totalSize += ReadBlock(file); GangData = LoadDummy();
            totalSize += ReadBlock(file); CarGenerators = LoadDummy();
            totalSize += ReadBlock(file); ParticleObjects = LoadDummy();
            totalSize += ReadBlock(file); AudioScriptObjects = LoadDummy();
            totalSize += ReadBlock(file); PlayerInfo = LoadDummy();
            totalSize += ReadBlock(file); Stats = LoadDummy();
            totalSize += ReadBlock(file); Streaming = LoadDummy();
            totalSize += ReadBlock(file); PedTypeInfo = LoadDummy();

            while (file.Position < file.Length - 4)
            {
                totalSize += ReadBlock(file);
            }

            Debug.WriteLine("Save size: {0}", totalSize);
            Debug.Assert(totalSize == (SizeOfOneGameInBytes & 0xFFFFFFFE));
        }

        protected override void SaveAllData(DataBuffer file)
        {
            int totalSize = 0;
            int size;

            m_workBuff.Reset();
            m_checkSum = 0;

            SaveSimpleVars();
            SaveData(Scripts); totalSize += WriteBlock(file);
            SaveData(PedPool); totalSize += WriteBlock(file);
            SaveData(Garages); totalSize += WriteBlock(file);
            SaveData(VehiclePool); totalSize += WriteBlock(file);
            SaveData(ObjectPool); totalSize += WriteBlock(file);
            SaveData(Paths); totalSize += WriteBlock(file);
            SaveData(Cranes); totalSize += WriteBlock(file);
            SaveData(Pickups); totalSize += WriteBlock(file);
            SaveData(PhoneInfo); totalSize += WriteBlock(file);
            SaveData(RestartPoints); totalSize += WriteBlock(file);
            SaveData(RadarBlips); totalSize += WriteBlock(file);
            SaveData(Zones); totalSize += WriteBlock(file);
            SaveData(GangData); totalSize += WriteBlock(file);
            SaveData(CarGenerators); totalSize += WriteBlock(file);
            SaveData(ParticleObjects); totalSize += WriteBlock(file);
            SaveData(AudioScriptObjects); totalSize += WriteBlock(file);
            SaveData(PlayerInfo); totalSize += WriteBlock(file);
            SaveData(Stats); totalSize += WriteBlock(file);
            SaveData(Streaming); totalSize += WriteBlock(file);
            SaveData(PedTypeInfo); totalSize += WriteBlock(file);

            for (int i = 0; i < 4; i++)
            {
                size = DataBuffer.Align4Bytes(SizeOfOneGameInBytes - totalSize - 4);
                if (size > m_bufferSize)
                {
                    size = m_bufferSize;
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

            Debug.WriteLine("Save size: {0}", totalSize);
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
                && GangData.Equals(other.GangData)
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
