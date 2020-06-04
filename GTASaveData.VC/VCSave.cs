using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GTASaveData.VC
{
    /// <summary>
    /// Represents a saved <i>Grand Theft Auto: Vice City</i> game.
    /// </summary>
    public class VCSave : GTA3VCSave, ISaveData, IEquatable<VCSave>
    {
        public const int SizeOfGameInBytes = 201728;
        public const int MaxNumPaddingBlocks = 4;
        public const int SteamId = 0x3DF5C2FD;      // constant?

        protected override Dictionary<FileFormat, int> BufferSizes => new Dictionary<FileFormat, int>()
        {
            { FileFormats.PC, 55000 },
            { FileFormats.PC_Steam, 55000 },
            { FileFormats.Android, 0x10000 },
            { FileFormats.iOS, 0x10000 },
        };

        private SimpleVariables m_simpleVars;
        private Dummy m_scripts;
        private Dummy m_pedPool;
        private Dummy m_garages;
        private Dummy m_gameLogic;
        private Dummy m_vehiclePool;
        private Dummy m_objectPool;
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

        public Dummy PlayerPeds
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

        public VCSave()
        {
            SimpleVars = new SimpleVariables();
            Scripts = new Dummy();
            PlayerPeds = new Dummy();
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

        protected override void LoadAllData(StreamBuffer file)
        {
            int totalSize = 0;

            totalSize += ReadBlock(file);
            SimpleVars = WorkBuff.Read<SimpleVariables>(FileFormat);
            Scripts = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); PlayerPeds = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); Garages = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); GameLogic = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); VehiclePool = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); ObjectPool = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); Paths = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); Cranes = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); Pickups = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); PhoneInfo = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); RestartPoints = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); RadarBlips = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); Zones = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); Gangs = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); CarGenerators = LoadType<CarGeneratorData>();
            totalSize += ReadBlock(file); ParticleObjects = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); AudioScriptObjects = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); ScriptPaths = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); PlayerInfo = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); Stats = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); SetPieces = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); Streaming = LoadTypePreAlloc<Dummy>();
            totalSize += ReadBlock(file); PedTypeInfo = LoadTypePreAlloc<Dummy>();

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
            SaveObject(GameLogic); totalSize += WriteBlock(file);
            SaveObject(VehiclePool); totalSize += WriteBlock(file);
            SaveObject(ObjectPool); totalSize += WriteBlock(file);
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
            SaveObject(ScriptPaths); totalSize += WriteBlock(file);
            SaveObject(PlayerInfo); totalSize += WriteBlock(file);
            SaveObject(Stats); totalSize += WriteBlock(file);
            SaveObject(SetPieces); totalSize += WriteBlock(file);
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
                    fmt = FileFormats.PC;
                    return true;
                }
                else if (scr == 0xF0)
                {
                    fmt = FileFormats.PC_Steam;
                    return true;
                }
            }

            fmt = FileFormat.Default;
            return false;
        }

        protected override int GetSize(FileFormat fmt)
        {
            // TODO
            throw SizeNotDefined(fmt);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VCSave);
        }

        public bool Equals(VCSave other)
        {
            if (other == null)
            {
                return false;
            }

            return SimpleVars.Equals(other.SimpleVars)
                && Scripts.Equals(other.Scripts)
                && PlayerPeds.Equals(other.PlayerPeds)
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
