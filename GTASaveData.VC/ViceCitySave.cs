﻿using GTASaveData.Extensions;
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
    public class ViceCitySave : GTA3VCSave, IGTASaveFile, IEquatable<ViceCitySave>, IDisposable
    {
        public const int SizeOfOneGameInBytes = 201729;
        public const int MaxBufferSize = 65536;

        private bool m_disposed;

        protected override int BufferSize => (FileFormat.IsMobile) ? 65536 : 55000;

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

        public ViceCitySave() : base(MaxBufferSize)
        {
            m_disposed = false;

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

        #if !DEBUG
            BlockSizeChecks = true;
        #endif
        }

        protected override void LoadSimpleVars()
        {
            SimpleVars = WorkBuff.Read<SimpleVariables>(FileFormat);
        }

        protected override void SaveSimpleVars()
        {
            SimpleVars.SaveSize = SizeOfOneGameInBytes;
            WorkBuff.Write(SimpleVars, FileFormat);
        }

        protected override void LoadAllData(StreamBuffer file)
        {
            int totalSize = 0;

            totalSize += ReadBlock(file);
            LoadSimpleVars();
            Scripts = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); PedPool = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Garages = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); GameLogic = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); VehiclePool = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); ObjectPool = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Paths = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Cranes = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Pickups = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); PhoneInfo = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); RestartPoints = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); RadarBlips = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Zones = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Gangs = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); CarGenerators = Load<CarGeneratorData>();
            totalSize += ReadBlock(file); ParticleObjects = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); AudioScriptObjects = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); ScriptPaths = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); PlayerInfo = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Stats = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); SetPieces = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); Streaming = LoadPreAlloc<Dummy>();
            totalSize += ReadBlock(file); PedTypeInfo = LoadPreAlloc<Dummy>();

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

            WorkBuff.Reset();
            CheckSum = 0;

            SaveSimpleVars();
            Save(Scripts); totalSize += WriteBlock(file);
            Save(PedPool); totalSize += WriteBlock(file);
            Save(Garages); totalSize += WriteBlock(file);
            Save(GameLogic); totalSize += WriteBlock(file);
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
            Save(ScriptPaths); totalSize += WriteBlock(file);
            Save(PlayerInfo); totalSize += WriteBlock(file);
            Save(Stats); totalSize += WriteBlock(file);
            Save(SetPieces); totalSize += WriteBlock(file);
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
                        WorkBuff.Reset();
                        WorkBuff.Write(GenerateSpecialPadding(size));
                    }
                    WorkBuff.Seek(size);
                    totalSize += WriteBlock(file);
                }
            }

            file.Write(CheckSum);

            Debug.WriteLine("Save successful!");
            Debug.WriteLine("Size of game data: {0} bytes", totalSize);
            Debug.Assert(totalSize == (SizeOfOneGameInBytes & 0xFFFFFFFE));
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
                    fmt = FileFormats.PC_Retail;
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

        public void Dispose()
        {
            if (!m_disposed)
            {
                WorkBuff.Dispose();
                m_disposed = true;
            }
        }

        private SerializationException BlockSizeExceededException(uint value, int max)
        {
            return new SerializationException(Strings.Error_Serialization_BlockSizeExceeded, value, max);
        }

        public static class FileFormats
        {
            public static readonly FileFormat Android = new FileFormat(
                "Android", "Android", "Android",
                new GameConsole(ConsoleType.Android)
            );

            public static readonly FileFormat iOS = new FileFormat(
                "iOS", "iOS", "iOS",
                new GameConsole(ConsoleType.iOS)
            );

            public static readonly FileFormat PC_Retail = new FileFormat(
                "PC_Retail", "PC", "Windows (Retail Version), macOS",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.MacOS)
            );

            public static readonly FileFormat PC_Steam = new FileFormat(
                "PC_Steam", "PC (Steam)", "Windows (Steam Version)",
                new GameConsole(ConsoleType.Win32, ConsoleFlags.Steam)
            );

            public static readonly FileFormat PS2 = new FileFormat(
                "PS2", "PS2", "PlayStation 2",
                new GameConsole(ConsoleType.PS2)
            );

            public static readonly FileFormat Xbox = new FileFormat(
                "Xbox", "Xbox", "Xbox",
                new GameConsole(ConsoleType.Xbox)
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { Android, iOS, PC_Retail, PC_Steam, PS2, Xbox };
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
