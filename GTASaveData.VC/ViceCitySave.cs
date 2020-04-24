using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.VC
{
    /// <summary>
    /// Represents a <i>Grand Theft Auto: Vice City</i> save file.
    /// </summary>
    public class ViceCitySave : SaveFile, ISaveFile, IEquatable<ViceCitySave>, IDisposable
    {
        public const int SaveHeaderSize = 8;
        public const int SizeOfOneGameInBytes = 201729;
        public const int MaxBufferSize = 65536;

        private readonly StreamBuffer m_workBuff;
        private int m_bufferSize => (FileFormat.Mobile) ? 65536 : 55000;
        private int m_checkSum;
        private bool m_blockSizeChecks;
        private bool m_disposed;

        private SimpleVariables m_simpleVars;   // SimpleVariables
        private Dummy m_scripts;  // TheScripts
        private Dummy m_pedPool;  // PedPool
        private Dummy m_garages;  // Garages
        private Dummy m_gameLogic;    // GameLogic
        private Dummy m_vehiclePool;  // VehiclePool
        private Dummy m_objectPool;   // ObjectPool
        private Dummy m_paths;    // PathFind
        private Dummy m_cranes;   // Cranes
        private Dummy m_pickups;  // Pickups
        private Dummy m_phoneInfo;    // PhoneInfo
        private Dummy m_restartPoints;    // Restarts
        private Dummy m_radarBlips;   // Radar
        private Dummy m_zones;    // TheZones
        private Dummy m_gangData; // Gangs
        private Dummy m_carGenerators;    // TheCarGenerators
        private Dummy m_particles;  // Particles
        private Dummy m_audioScriptObjects;   // AudioScriptObjects
        private Dummy m_scriptPaths;  // ScriptPaths
        private Dummy m_playerInfo;   // PlayerInfo
        private Dummy m_stats;    // Stats
        private Dummy m_setPieces; // SetPieces
        private Dummy m_streaming;    // Streaming
        private Dummy m_pedTypeInfo;  // PedTypeInfo

        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public Dummy Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; OnPropertyChanged(); }
        }

        public Dummy PedPool
        {
            get { return m_pedPool; }
            set { m_pedPool = value; OnPropertyChanged(); }
        }

        public Dummy Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public Dummy GameLogic
        {
            get { return m_gameLogic; }
            set { m_gameLogic = value; OnPropertyChanged(); }
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

        public Dummy Paths
        {
            get { return m_paths; }
            set { m_paths = value; OnPropertyChanged(); }
        }

        public Dummy Cranes
        {
            get { return m_cranes; }
            set { m_cranes = value; OnPropertyChanged(); }
        }

        public Dummy Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
        }

        public Dummy PhoneInfo
        {
            get { return m_phoneInfo; }
            set { m_phoneInfo = value; OnPropertyChanged(); }
        }

        public Dummy RestartPoints
        {
            get { return m_restartPoints; }
            set { m_restartPoints = value; OnPropertyChanged(); }
        }

        public Dummy RadarBlips
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; OnPropertyChanged(); }
        }

        public Dummy Zones
        {
            get { return m_zones; }
            set { m_zones = value; OnPropertyChanged(); }
        }

        public Dummy GangData
        {
            get { return m_gangData; }
            set { m_gangData = value; OnPropertyChanged(); }
        }

        public Dummy CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; OnPropertyChanged(); }
        }

        public Dummy ParticleObjects
        {
            get { return m_particles; }
            set { m_particles = value; OnPropertyChanged(); }
        }

        public Dummy AudioScriptObjects
        {
            get { return m_audioScriptObjects; }
            set { m_audioScriptObjects = value; OnPropertyChanged(); }
        }

        public Dummy ScriptPaths
        {
            get { return m_scriptPaths; }
            set { m_scriptPaths = value; OnPropertyChanged(); }
        }

        public Dummy PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; OnPropertyChanged(); }
        }

        public Dummy Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public Dummy SetPieces
        {
            get { return m_setPieces; }
            set { m_setPieces = value; OnPropertyChanged(); }
        }

        public Dummy Streaming
        {
            get { return m_streaming; }
            set { m_streaming = value; OnPropertyChanged(); }
        }

        public Dummy PedTypeInfo
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
            GameLogic,
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
            ScriptPaths,
            PlayerInfo,
            Stats,
            SetPieces,
            Streaming,
            PedTypeInfo
        };

        public ViceCitySave()
        {
            m_disposed = false;
            m_workBuff = new StreamBuffer(new byte[MaxBufferSize]);

            SimpleVars = new SimpleVariables();
            Scripts = new Dummy();
            PedPool = new Dummy();
            Garages = new Dummy();
            GameLogic = new Dummy();
            VehiclePool = new Dummy();
            ObjectPool = new Dummy();
            Paths = new Dummy();
            Cranes = new Dummy();
            Pickups = new Dummy();
            PhoneInfo = new Dummy();
            RestartPoints = new Dummy();
            RadarBlips = new Dummy();
            Zones = new Dummy();
            GangData = new Dummy();
            CarGenerators = new Dummy();
            ParticleObjects = new Dummy();
            AudioScriptObjects = new Dummy();
            ScriptPaths = new Dummy();
            PlayerInfo = new Dummy();
            Stats = new Dummy();
            SetPieces = new Dummy();
            Streaming = new Dummy();
            PedTypeInfo = new Dummy();

        #if !DEBUG
            BlockSizeChecks = true;
        #endif
        }

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

        private Dummy LoadDummy()
        {
            int size = m_workBuff.ReadInt32();
            var o = new Dummy(size);
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

            preSize = m_workBuff.Cursor;
            m_workBuff.Skip(4);

            size = Serializer.Write(m_workBuff, o, FileFormat);
            postData = m_workBuff.Cursor;

            m_workBuff.Seek(preSize);
            m_workBuff.Write(size);
            m_workBuff.Seek(postData);
            m_workBuff.Align4Bytes();
        }

        private int ReadBlock(StreamBuffer file)
        {
            file.MarkCurrentPosition();
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

        private int WriteBlock(StreamBuffer file)
        {
            file.MarkCurrentPosition();

            byte[] data = m_workBuff.GetBytes();
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

        protected override void LoadAllData(StreamBuffer file)
        {
            int totalSize = 0;

            totalSize += ReadBlock(file);
            LoadSimpleVars();
            Scripts = LoadDummy();
            totalSize += ReadBlock(file); PedPool = LoadDummy();
            totalSize += ReadBlock(file); Garages = LoadDummy();
            totalSize += ReadBlock(file); GameLogic = LoadDummy();
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
            totalSize += ReadBlock(file); ScriptPaths = LoadDummy();
            totalSize += ReadBlock(file); PlayerInfo = LoadDummy();
            totalSize += ReadBlock(file); Stats = LoadDummy();
            totalSize += ReadBlock(file); SetPieces = LoadDummy();
            totalSize += ReadBlock(file); Streaming = LoadDummy();
            totalSize += ReadBlock(file); PedTypeInfo = LoadDummy();

            while (file.Cursor < file.Length - 4)
            {
                totalSize += ReadBlock(file);
            }

            Debug.WriteLine("Save size: {0}", totalSize);
            Debug.Assert(totalSize == (SizeOfOneGameInBytes & 0xFFFFFFFE));
        }

        protected override void SaveAllData(StreamBuffer file)
        {
            int totalSize = 0;
            int size;

            m_workBuff.Reset();
            m_checkSum = 0;

            SaveSimpleVars();
            SaveData(Scripts); totalSize += WriteBlock(file);
            SaveData(PedPool); totalSize += WriteBlock(file);
            SaveData(Garages); totalSize += WriteBlock(file);
            SaveData(GameLogic); totalSize += WriteBlock(file);
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
            SaveData(ScriptPaths); totalSize += WriteBlock(file);
            SaveData(PlayerInfo); totalSize += WriteBlock(file);
            SaveData(Stats); totalSize += WriteBlock(file);
            SaveData(SetPieces); totalSize += WriteBlock(file);
            SaveData(Streaming); totalSize += WriteBlock(file);
            SaveData(PedTypeInfo); totalSize += WriteBlock(file);

            for (int i = 0; i < 4; i++)
            {
                size = StreamBuffer.Align4Bytes(SizeOfOneGameInBytes - totalSize - 4);
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

        protected override bool DetectFileFormat(byte[] data, out DataFormat fmt)
        {
            // TODO: Android, iOS, PS2, Xbox

            int fileId = data.FindFirst(BitConverter.GetBytes(0x31401));
            int fileIdJP = data.FindFirst(BitConverter.GetBytes(0x31400));  // TODO: confirm this even exists
            int scr = data.FindFirst("SCR\0".GetAsciiBytes());

            int blk1Size;
            using (StreamBuffer wb = new StreamBuffer(data))
            {
                wb.Skip(wb.ReadInt32());
                blk1Size = wb.ReadInt32();
            }

            if (fileId == 0x44)
            {
                if (scr == 0xEC)
                {
                    fmt = FileFormats.PC_Retail;
                    return true;
                }
                else if (scr == 0xF0)
                {
                    fmt = FileFormats.PC_Steam;
                    return true;
                }
            }

            fmt = DataFormat.Default;
            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ViceCitySave);
        }

        public bool Equals(ViceCitySave other)
        {
            if (other == null)
            {
                return false;
            }

            return SimpleVars.Equals(other.SimpleVars)
                && Scripts.Equals(other.Scripts)
                && PedPool.Equals(other.PedPool)
                && Garages.Equals(other.Garages)
                && GameLogic.Equals(other.GameLogic)
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
                && ScriptPaths.Equals(other.ScriptPaths)
                && PlayerInfo.Equals(other.PlayerInfo)
                && Stats.Equals(other.Stats)
                && SetPieces.Equals(other.SetPieces)
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

            public static readonly DataFormat PC_Retail = new DataFormat(
                "PC_Retail", "PC", "Windows (Retail Version), macOS",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.MacOS)
            );

            public static readonly DataFormat PC_Steam = new DataFormat(
                "PC_Steam", "PC (Steam)", "Windows (Steam Version)",
                new GameConsole(ConsoleType.Win32, ConsoleFlags.Steam)
            );

            public static readonly DataFormat PS2 = new DataFormat(
                "PS2", "PS2", "PlayStation 2",
                new GameConsole(ConsoleType.PS2)
            );

            public static readonly DataFormat Xbox = new DataFormat(
                "Xbox", "Xbox", "Xbox",
                new GameConsole(ConsoleType.Xbox)
            );

            public static DataFormat[] GetAll()
            {
                return new DataFormat[] { Android, iOS, PC_Retail, PC_Steam, PS2, Xbox };
            }
        }
    }
}
