using GTASaveData.Extensions;
using GTASaveData.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GTASaveData.VC
{
    /// <summary>
    /// Represents a saved <i>Grand Theft Auto: Vice City</i> game.
    /// </summary>
    public class SaveFileVC : SaveFileGTA3VC, ISaveFile,
        IEquatable<SaveFileVC>, IDeepClonable<SaveFileVC>
    {
        public const int MaxNumPaddingBlocks = 4;
        private const int NumOuterBlocks = 23;
        private const int DataSize = 0x31400;

        private SimpleVariables m_simpleVars;
        private Dummy m_scripts;
        private PedPool m_pedPool;
        private Dummy m_garages;
        private Dummy m_gameLogic;
        private Dummy m_vehiclePool;
        private ObjectPool m_objectPool;
        private Dummy m_paths;
        private Dummy m_cranes;
        private Dummy m_pickups;
        private Dummy m_phoneInfo;
        private Dummy m_restartPoints;
        private Dummy m_radarBlips;
        private Dummy m_zones;
        private Dummy m_gangs;
        private CarGeneratorData m_carGenerators;
        private Dummy m_particleObjects;
        private Dummy m_audioScriptObjects;
        private Dummy m_scriptPaths;
        private Dummy m_playerInfo;
        private Dummy m_stats;
        private Dummy m_setPieces;
        private Dummy m_streaming;
        private Dummy m_pedType;

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

        public PedPool PedPool
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

        public Dummy Vehicles
        {
            get { return m_vehiclePool; }
            set { m_vehiclePool = value; OnPropertyChanged(); }
        }

        public ObjectPool Objects
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

        public Dummy Gangs
        {
            get { return m_gangs; }
            set { m_gangs = value; OnPropertyChanged(); }
        }

        public CarGeneratorData CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; OnPropertyChanged(); }
        }

        public Dummy ParticleObjects
        {
            get { return m_particleObjects; }
            set { m_particleObjects = value; OnPropertyChanged(); }
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

        bool ISaveFile.HasSimpleVariables => true;
        bool ISaveFile.HasScriptData => false;      // TODO
        bool ISaveFile.HasGarageData => false;      // TODO
        bool ISaveFile.HasCarGenerators => true;    // TODO
        bool ISaveFile.HasPlayerInfo => false;      // TODO

        ISimpleVariables ISaveFile.SimpleVars => SimpleVars;
        IScriptData ISaveFile.ScriptData => throw new NotImplementedException();
        IGarageData ISaveFile.GarageData => throw new NotImplementedException();
        ICarGeneratorData ISaveFile.CarGenerators => CarGenerators;
        IPlayerInfo ISaveFile.PlayerInfo => throw new NotImplementedException();

        IReadOnlyList<ISaveDataObject> ISaveFile.Blocks => new List<SaveDataObject>()
        {
            SimpleVars,
            Scripts,
            PedPool,
            Garages,
            GameLogic,
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
            ScriptPaths,
            PlayerInfo,
            Stats,
            SetPieces,
            Streaming,
            PedTypeInfo
        };

        public static SaveFileVC Load(string path)
        {
            return Load<SaveFileVC>(path);
        }

        public static SaveFileVC Load(string path, FileFormat fmt)
        {
            return Load<SaveFileVC>(path, fmt);
        }

        public SaveFileVC()
        {
            SimpleVars = new SimpleVariables();
            Scripts = new Dummy();
            PedPool = new PedPool();
            Garages = new Dummy();
            GameLogic = new Dummy();
            Vehicles = new Dummy();
            Objects = new ObjectPool();
            Paths = new Dummy();
            Cranes = new Dummy();
            Pickups = new Dummy();
            PhoneInfo = new Dummy();
            RestartPoints = new Dummy();
            RadarBlips = new Dummy();
            Zones = new Dummy();
            Gangs = new Dummy();
            CarGenerators = new CarGeneratorData();
            ParticleObjects = new Dummy();
            AudioScriptObjects = new Dummy();
            ScriptPaths = new Dummy();
            PlayerInfo = new Dummy();
            Stats = new Dummy();
            SetPieces = new Dummy();
            Streaming = new Dummy();
            PedTypeInfo = new Dummy();
        }

        public SaveFileVC(SaveFileVC other)
        {
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Scripts = new Dummy(other.Scripts);
            PedPool = new PedPool(other.PedPool);
            Garages = new Dummy(other.Garages);
            GameLogic = new Dummy(other.GameLogic);
            Vehicles = new Dummy(other.Vehicles);
            Objects = new ObjectPool(other.Objects);
            Paths = new Dummy(other.Paths);
            Cranes = new Dummy(other.Cranes);
            Pickups = new Dummy(other.Pickups);
            PhoneInfo = new Dummy(other.PhoneInfo);
            RestartPoints = new Dummy(other.RestartPoints);
            RadarBlips = new Dummy(other.RadarBlips);
            Zones = new Dummy(other.Zones);
            Gangs = new Dummy(other.Gangs);
            CarGenerators = new CarGeneratorData(other.CarGenerators);
            ParticleObjects = new Dummy(other.ParticleObjects);
            AudioScriptObjects = new Dummy(other.AudioScriptObjects);
            ScriptPaths = new Dummy(other.ScriptPaths);
            PlayerInfo = new Dummy(other.PlayerInfo);
            Stats = new Dummy(other.Stats);
            SetPieces = new Dummy(other.SetPieces);
            Streaming = new Dummy(other.Streaming);
            PedTypeInfo = new Dummy(other.PedTypeInfo);
        }

        protected override void LoadAllData(DataBuffer file)
        {
            int dataSize = 0;

            dataSize += ReadBlock(file);
            SimpleVars = WorkBuff.ReadObject<SimpleVariables>(FileFormat);
            Scripts = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); PedPool = LoadType<PedPool>();
            dataSize += ReadBlock(file); Garages = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); GameLogic = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); Vehicles = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); Objects = LoadType<ObjectPool>();
            dataSize += ReadBlock(file); Paths = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); Cranes = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); Pickups = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); PhoneInfo = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); RestartPoints = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); RadarBlips = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); Zones = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); Gangs = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); CarGenerators = LoadType<CarGeneratorData>();
            dataSize += ReadBlock(file); ParticleObjects = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); AudioScriptObjects = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); ScriptPaths = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); PlayerInfo = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); Stats = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); SetPieces = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); Streaming = LoadTypePreAlloc<Dummy>();
            dataSize += ReadBlock(file); PedTypeInfo = LoadTypePreAlloc<Dummy>();

            // Skip over padding
            int numPaddingBlocks = 0;
            while (file.Position < file.Length - 4)
            {
                dataSize += ReadBlock(file);
                numPaddingBlocks++;
            }

            // Size checks
            int expectedDataSize = DataSize;
            int expectedFileSize = expectedDataSize + ((NumOuterBlocks + numPaddingBlocks) * sizeof(int)) + sizeof(int);
            Debug.Assert(expectedDataSize == dataSize);
            Debug.Assert(expectedFileSize == file.Length);
            Debug.WriteLine("Load successful!");
        }

        protected override void SaveAllData(DataBuffer file)
        {
            int dataSize = 0;
            int size;

            WorkBuff.Reset();
            CheckSum = 0;
            SimpleVars.SizeOfGameInBytes = DataSize + 1;

            WorkBuff.Write(SimpleVars, FileFormat);
            SaveObject(Scripts); dataSize += WriteBlock(file);
            SaveObject(PedPool); dataSize += WriteBlock(file);
            SaveObject(Garages); dataSize += WriteBlock(file);
            SaveObject(GameLogic); dataSize += WriteBlock(file);
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
            SaveObject(ScriptPaths); dataSize += WriteBlock(file);
            SaveObject(PlayerInfo); dataSize += WriteBlock(file);
            SaveObject(Stats); dataSize += WriteBlock(file);
            SaveObject(SetPieces); dataSize += WriteBlock(file);
            SaveObject(Streaming); dataSize += WriteBlock(file);
            SaveObject(PedTypeInfo); dataSize += WriteBlock(file);

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

            // Size checks
            int expectedDataSize = DataSize;
            int expectedFileSize = expectedDataSize + ((NumOuterBlocks + numPaddingBlocks) * sizeof(int)) + sizeof(int);
            Debug.Assert(expectedDataSize == dataSize);
            Debug.Assert(expectedFileSize == file.Length);
            Debug.WriteLine("Save successful!");
        }

        protected override bool DetectFileFormat(byte[] data, out FileFormat fmt)
        {
            // TODO: PS2, Xbox

            bool isMobile = false;

            int saveSizeOffset = data.FindFirst(BitConverter.GetBytes(DataSize + 1));
            int saveSizeOffsetJP = data.FindFirst(BitConverter.GetBytes(DataSize));
            int scrOffset = data.FindFirst("SCR\0".GetAsciiBytes());

            if ((saveSizeOffset < 0 && saveSizeOffsetJP < 0) || scrOffset < 0)
            {
                goto DetectionFailed;
            }

            if (saveSizeOffset == 0x40)
            {
                isMobile = true;
            }
            else if (saveSizeOffset == 0x44)
            {
                if (scrOffset == 0xEC)
                {
                    fmt = FileFormats.PC;
                    return true;
                }
                else if (scrOffset == 0xF0)
                {
                    fmt = FileFormats.PC_Steam;
                    return true;
                }
            }

            int sizeOfPlayerPed;
            using (DataBuffer s = new DataBuffer(data))
            {
                int block0Size = s.ReadInt32();
                if (block0Size > s.Length) goto DetectionFailed;
                s.Skip(block0Size + sizeof(int));
                int sizeOfPedPool = DataBuffer.Align4(s.ReadInt32() - sizeof(int));
                int numPlayerPeds = s.ReadInt32();
                sizeOfPlayerPed = sizeOfPedPool / numPlayerPeds;
            }

            if (isMobile)
            {
                if (sizeOfPlayerPed == 0x754 || sizeOfPlayerPed == 0x758)
                {
                    fmt = FileFormats.iOS;
                    return true;
                }
                else if (sizeOfPlayerPed == 0x75C)
                {
                    fmt = FileFormats.Android;
                    return true;
                }
            }

        DetectionFailed:
            fmt = FileFormat.Default;
            return false;
        }

        protected override int GetBufferSize()
        {
            if (FileFormat.IsMobile)
            {
                return 0x10000;
            }

            return 55000;
        }

        protected override int GetSize(FileFormat fmt)
        {
            int size = 0;

            // data blocks
            size += SizeOfObject(SimpleVars, fmt);
            size += SizeOfObject(Scripts, fmt) + sizeof(int);
            size += SizeOfObject(PedPool, fmt) + sizeof(int);
            size += SizeOfObject(Garages, fmt) + sizeof(int);
            size += SizeOfObject(GameLogic, fmt) + sizeof(int);
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
            size += SizeOfObject(ScriptPaths, fmt) + sizeof(int);
            size += SizeOfObject(PlayerInfo, fmt) + sizeof(int);
            size += SizeOfObject(Stats, fmt) + sizeof(int);
            size += SizeOfObject(SetPieces, fmt) + sizeof(int);
            size += SizeOfObject(Streaming, fmt) + sizeof(int);
            size += SizeOfObject(PedTypeInfo, fmt) + sizeof(int);

            // padding blocks
            int numRemaining = DataSize - size;
            int numPadding = (numRemaining / (GetBufferSize() - sizeof(int))) + 1;
            size += numRemaining;

            // "outer" block sizes
            size += sizeof(int) * (NumOuterBlocks + numPadding);

            // checksum
            size += sizeof(int);

            return size;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SaveFileVC);
        }

        public bool Equals(SaveFileVC other)
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
                && ScriptPaths.Equals(other.ScriptPaths)
                && PlayerInfo.Equals(other.PlayerInfo)
                && Stats.Equals(other.Stats)
                && SetPieces.Equals(other.SetPieces)
                && Streaming.Equals(other.Streaming)
                && PedTypeInfo.Equals(other.PedTypeInfo);
        }

        public SaveFileVC DeepClone()
        {
            return new SaveFileVC(this);
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
                "PC", "PC", "Windows (Retail Version), Mac OS",
                GameConsole.Win32,
                GameConsole.MacOS
            );

            public static readonly FileFormat PC_Steam = new FileFormat(
                "PC_Steam", "PC (Steam)", "Windows (Steam Version)",
                FileFormatFlags.Steam,
                GameConsole.Win32
            );

            public static readonly FileFormat PS2 = new FileFormat(
                "PS2", "PS2", "PlayStation 2",
                GameConsole.PS2
            );

            public static readonly FileFormat Xbox = new FileFormat(
                "Xbox",
                GameConsole.Xbox
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { Android, iOS, PC, PC_Steam, PS2, Xbox };
            }
        }
    }

    public enum DataBlock
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
    }
}
