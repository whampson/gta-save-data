using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GTASaveData.GTA4
{
    /// <summary>
    /// Represents a <i>Grand Theft Auto IV</i> save file.
    /// </summary>
    public class GTA4Save : SaveData, ISaveData, IEquatable<GTA4Save>
    {
        public static class Limits
        {
            public const int MaxNameLength = 128;
        }

        private DataBuffer m_file;

        private int m_saveVersion;
        private int m_saveSizeInBytes;
        private int m_scriptSpace;
        private string m_lastMissionPassedName;
        private DateTime m_timeLastSaved;   // Not in savefile

        private SimpleVariables m_simpleVars;
        private Dummy m_playerInfo;
        private Dummy m_extraContent;
        private Dummy m_scripts;
        private Dummy m_garages;
        private Dummy m_gameLogic;
        private Dummy m_paths;
        private Pickups m_pickups;
        private Dummy m_restartPoints;
        private Dummy m_radarBlips;
        private Dummy m_zones;
        private Dummy m_gangData;
        private Dummy m_carGenerators;
        private Dummy m_stats;
        private Dummy m_iplStore;
        private Dummy m_stuntJumps;
        private Dummy m_radio;
        private Dummy m_objects;
        private Dummy m_relationships;
        private Dummy m_inventory;
        private Dummy m_unusedPools;
        private Dummy m_unusedPhoneInfo;
        private Dummy m_unusedAudioScript;
        private Dummy m_unusedSetPieces;
        private Dummy m_unusedStreaming;
        private Dummy m_unusedPedTypeInfo;
        private Dummy m_unusedTags;
        private Dummy m_unusedShopping;
        private Dummy m_unusedGangWars;
        private Dummy m_unusedEntryExits;
        private Dummy m_unused3dMarkers;
        private Dummy m_unusedVehicles;
        private Dummy m_unusedExtraBlock;
        private Dummy m_gfwlData;

        public override string Name
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
        }

        public override DateTime TimeStamp
        {
            get { return m_timeLastSaved; }
            set { m_timeLastSaved = value; OnPropertyChanged(); }
        }

        public int SaveVersion
        { 
            get { return m_saveVersion; }
            set { m_saveVersion = value; OnPropertyChanged(); }
        }

        public int SaveSizeInBytes
        { 
            get { return m_saveSizeInBytes; }
            set { m_saveSizeInBytes = value; OnPropertyChanged(); }
        }

        public int ScriptSpace
        { 
            get { return m_scriptSpace; }
            set { m_scriptSpace = value; OnPropertyChanged(); }
        }

        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public Dummy PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; OnPropertyChanged(); }
        }

        public Dummy ExtraContent
        {
            get { return m_extraContent; }
            set { m_extraContent = value; OnPropertyChanged(); }
        }

        public Dummy Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; OnPropertyChanged(); }
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

        public Dummy Paths
        {
            get { return m_paths; }
            set { m_paths = value; OnPropertyChanged(); }
        }

        public Pickups Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
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

        public Dummy Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public Dummy IplStore
        {
            get { return m_iplStore; }
            set { m_iplStore = value; OnPropertyChanged(); }
        }

        public Dummy StuntJumps
        {
            get { return m_stuntJumps; }
            set { m_stuntJumps = value; OnPropertyChanged(); }
        }

        public Dummy Radio
        {
            get { return m_radio; }
            set { m_radio = value; OnPropertyChanged(); }
        }

        public Dummy Objects
        {
            get { return m_objects; }
            set { m_objects = value; OnPropertyChanged(); }
        }

        public Dummy Relationships
        {
            get { return m_relationships; }
            set { m_relationships = value; OnPropertyChanged(); }
        }

        public Dummy Inventory
        {
            get { return m_inventory; }
            set { m_inventory = value; OnPropertyChanged(); }
        }

        public Dummy UnusedPools
        {
            get { return m_unusedPools; }
            set { m_unusedPools = value; OnPropertyChanged(); }
        }

        public Dummy UnusedPhoneInfo
        {
            get { return m_unusedPhoneInfo; }
            set { m_unusedPhoneInfo = value; OnPropertyChanged(); }
        }

        public Dummy UnusedAudioScript
        {
            get { return m_unusedAudioScript; }
            set { m_unusedAudioScript = value; OnPropertyChanged(); }
        }

        public Dummy UnusedSetPieces
        {
            get { return m_unusedSetPieces; }
            set { m_unusedSetPieces = value; OnPropertyChanged(); }
        }

        public Dummy UnusedStreaming
        {
            get { return m_unusedStreaming; }
            set { m_unusedStreaming = value; OnPropertyChanged(); }
        }

        public Dummy UnusedPedTypeInfo
        {
            get { return m_unusedPedTypeInfo; }
            set { m_unusedPedTypeInfo = value; OnPropertyChanged(); }
        }

        public Dummy UnusedTags
        {
            get { return m_unusedTags; }
            set { m_unusedTags = value; OnPropertyChanged(); }
        }

        public Dummy UnusedShopping
        {
            get { return m_unusedShopping; }
            set { m_unusedShopping = value; OnPropertyChanged(); }
        }

        public Dummy UnusedGangWars
        {
            get { return m_unusedGangWars; }
            set { m_unusedGangWars = value; OnPropertyChanged(); }
        }

        public Dummy UnusedEntryExits
        {
            get { return m_unusedEntryExits; }
            set { m_unusedEntryExits = value; OnPropertyChanged(); }
        }

        public Dummy Unused3dMarkers
        {
            get { return m_unused3dMarkers; }
            set { m_unused3dMarkers = value; OnPropertyChanged(); }
        }

        public Dummy UnusedVehicles
        {
            get { return m_unusedVehicles; }
            set { m_unusedVehicles = value; OnPropertyChanged(); }
        }

        public Dummy UnusedExtraBlock
        {
            get { return m_unusedExtraBlock; }
            set { m_unusedExtraBlock = value; OnPropertyChanged(); }
        }

        public Dummy GfwlData
        {
            get { return m_gfwlData; }
            set { m_gfwlData = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public override IReadOnlyList<SaveDataObject> Blocks => new List<SaveDataObject>()
        {
            SimpleVars,
            PlayerInfo,
            ExtraContent,
            Scripts,
            Garages,
            GameLogic,
            Paths,
            Pickups,
            RestartPoints,
            RadarBlips,
            Zones,
            GangData,
            CarGenerators,
            Stats,
            IplStore,
            StuntJumps,
            Radio,
            Objects,
            Relationships,
            Inventory,
            UnusedPools,
            UnusedPhoneInfo,
            UnusedAudioScript,
            UnusedSetPieces,
            UnusedStreaming,
            UnusedPedTypeInfo,
            UnusedTags,
            UnusedShopping,
            UnusedGangWars,
            UnusedEntryExits,
            Unused3dMarkers,
            UnusedVehicles,
            UnusedExtraBlock,
            GfwlData
        };

        public GTA4Save()
        {
            Name = "";
            SimpleVars = new SimpleVariables();
            PlayerInfo = new Dummy();
            ExtraContent = new Dummy();
            Scripts = new Dummy();
            Garages = new Dummy();
            GameLogic = new Dummy();
            Paths = new Dummy();
            Pickups = new Pickups();
            RestartPoints = new Dummy();
            RadarBlips = new Dummy();
            Zones = new Dummy();
            GangData = new Dummy();
            CarGenerators = new Dummy();
            Stats = new Dummy();
            IplStore = new Dummy();
            StuntJumps = new Dummy();
            Radio = new Dummy();
            Objects = new Dummy();
            Relationships = new Dummy();
            Inventory = new Dummy();
            UnusedPools = new Dummy();
            UnusedPhoneInfo = new Dummy();
            UnusedAudioScript = new Dummy();
            UnusedSetPieces = new Dummy();
            UnusedStreaming = new Dummy();
            UnusedPedTypeInfo = new Dummy();
            UnusedTags = new Dummy();
            UnusedShopping = new Dummy();
            UnusedGangWars = new Dummy();
            UnusedEntryExits = new Dummy();
            Unused3dMarkers = new Dummy();
            UnusedVehicles = new Dummy();
            UnusedExtraBlock = new Dummy();
            GfwlData = new Dummy();
        }

        private void LoadFileHeader()
        {
            SaveVersion = m_file.ReadInt32();
            SaveSizeInBytes = m_file.ReadInt32();
            ScriptSpace = m_file.ReadInt32();

            string sig = m_file.ReadString(4);
            if (FileFormat.IsWin32)
            {
                Name = m_file.ReadString(Limits.MaxNameLength, unicode: true);
            }

            Debug.Assert(sig == "SAVE", "Invalid 'SAVE' signature!");
            Debug.Assert(SaveSizeInBytes == m_file.Length, "SaveSizeInBytes value incorrect!");
        }

        private void LoadFileFooter()
        {
            string sig = m_file.ReadString(4);
            Debug.Assert(sig == "END", "Invalid 'END' signature!");

            if (FileFormat.IsWin32)
            {
                int size = m_file.Length - m_file.Position;
                GfwlData = new Dummy(m_file.ReadBytes(size));
            }
        }

        private T LoadData<T>() where T : SaveDataObject, new()
        {
            string sig = m_file.ReadString(5);
            int size = m_file.ReadInt32();
            Debug.Assert(sig == "BLOCK", "Invalid 'BLOCK' signature!");

            m_file.Mark();
            T obj = m_file.ReadObject<T>();
            Debug.Assert(m_file.Offset == size - 9);

            return obj;
        }

        private Dummy LoadDummy(byte[] data)
        {
            return new Dummy(data);
        }

        private byte[] LoadBlockData()
        {
            string sig = m_file.ReadString(5);
            int size = m_file.ReadInt32();
            Debug.Assert(sig == "BLOCK", "Invalid 'BLOCK' signature!");
            
            m_file.Mark();
            byte[] data = m_file.ReadBytes(size - 9);
            Debug.Assert(m_file.Offset == size - 9);
            
            return data;
        }

        protected override void LoadAllData(DataBuffer file)
        {
            m_file = file;
            m_file.BigEndian = (FileFormat.IsXbox360 || FileFormat.IsPS3);

            LoadFileHeader();

            int blockCount = (FileFormat.IsPS3) ? 33 : 32;
            int index = 0;

            while (index < blockCount)
            {
                switch (index++)
                {
                    case 0: SimpleVars = LoadData<SimpleVariables>(); break;
                    case 1: PlayerInfo = LoadDummy(LoadBlockData()); break;     // lol fix this syntax
                    case 2: ExtraContent = LoadDummy(LoadBlockData()); break;
                    case 3: Scripts = LoadDummy(LoadBlockData()); break;
                    case 4: Garages = LoadDummy(LoadBlockData()); break;
                    case 5: GameLogic = LoadDummy(LoadBlockData()); break;
                    case 6: Paths = LoadDummy(LoadBlockData()); break;
                    case 7: Pickups = LoadData<Pickups>(); break;
                    case 8: RestartPoints = LoadDummy(LoadBlockData()); break;
                    case 9: RadarBlips = LoadDummy(LoadBlockData()); break;
                    case 10: Zones = LoadDummy(LoadBlockData()); break;
                    case 11: GangData = LoadDummy(LoadBlockData()); break;
                    case 12: CarGenerators = LoadDummy(LoadBlockData()); break;
                    case 13: Stats = LoadDummy(LoadBlockData()); break;
                    case 14: IplStore = LoadDummy(LoadBlockData()); break;
                    case 15: StuntJumps = LoadDummy(LoadBlockData()); break;
                    case 16: Radio = LoadDummy(LoadBlockData()); break;
                    case 17: Objects = LoadDummy(LoadBlockData()); break;
                    case 18: Relationships = LoadDummy(LoadBlockData()); break;
                    case 19: Inventory = LoadDummy(LoadBlockData()); break;
                    case 20: UnusedPools = LoadDummy(LoadBlockData()); break;
                    case 21: UnusedPhoneInfo = LoadDummy(LoadBlockData()); break;
                    case 22: UnusedAudioScript = LoadDummy(LoadBlockData()); break;
                    case 23: UnusedSetPieces = LoadDummy(LoadBlockData()); break;
                    case 24: UnusedStreaming = LoadDummy(LoadBlockData()); break;
                    case 25: UnusedPedTypeInfo = LoadDummy(LoadBlockData()); break;
                    case 26: UnusedTags = LoadDummy(LoadBlockData()); break;
                    case 27: UnusedShopping = LoadDummy(LoadBlockData()); break;
                    case 28: UnusedGangWars = LoadDummy(LoadBlockData()); break;
                    case 29: UnusedEntryExits = LoadDummy(LoadBlockData()); break;
                    case 30: Unused3dMarkers = LoadDummy(LoadBlockData()); break;
                    case 31: UnusedVehicles = LoadDummy(LoadBlockData()); break;
                    case 32: UnusedExtraBlock = LoadDummy(LoadBlockData()); break;
                }
            }

            m_file.ReadInt32();     // checksum
            LoadFileFooter();
        }

        protected override void SaveAllData(DataBuffer file)
        {
            throw new NotImplementedException();
        }

        protected override bool DetectFileFormat(byte[] data, out FileFormat fmt)
        {
            using (DataBuffer b = new DataBuffer(data))
            {
                int version = b.ReadInt32();
                if (version == 0x39)
                {
                    fmt = FileFormats.PC;
                    return true;
                }
                else if (version == 0x38000000)
                {
                    int start = 0x10;
                    int blockCount = 0;

                    while (start < data.Length)
                    {
                        int off = data.FindFirst("BLOCK".GetAsciiBytes(), start);
                        if (off == -1)
                        {
                            break;
                        }

                        blockCount++;
                        start = off + 5;
                    }

                    fmt = (blockCount == 33) ? FileFormats.PS3 : fmt = FileFormats.Xbox360;
                    return true;
                }
            }

            fmt = FileFormat.Default;
            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GTA4Save);
        }

        public bool Equals(GTA4Save other)
        {
            if (other == null)
            {
                return false;
            }

            return Name.Equals(other.Name)
                && SaveVersion.Equals(other.SaveVersion)
                && SaveSizeInBytes.Equals(other.SaveSizeInBytes)
                && ScriptSpace.Equals(other.ScriptSpace)
                && SimpleVars.Equals(other.SimpleVars)
                && PlayerInfo.Equals(other.PlayerInfo)
                && ExtraContent.Equals(other.ExtraContent)
                && Scripts.Equals(other.Scripts)
                && Garages.Equals(other.Garages)
                && GameLogic.Equals(other.GameLogic)
                && Paths.Equals(other.Paths)
                && Pickups.Equals(other.Pickups)
                && RestartPoints.Equals(other.RestartPoints)
                && RadarBlips.Equals(other.RadarBlips)
                && Zones.Equals(other.Zones)
                && GangData.Equals(other.GangData)
                && CarGenerators.Equals(other.CarGenerators)
                && Stats.Equals(other.Stats)
                && IplStore.Equals(other.IplStore)
                && StuntJumps.Equals(other.StuntJumps)
                && Radio.Equals(other.Radio)
                && Objects.Equals(other.Objects)
                && Relationships.Equals(other.Relationships)
                && Inventory.Equals(other.Inventory)
                && UnusedPools.Equals(other.UnusedPools)
                && UnusedPhoneInfo.Equals(other.UnusedPhoneInfo)
                && UnusedAudioScript.Equals(other.UnusedAudioScript)
                && UnusedSetPieces.Equals(other.UnusedSetPieces)
                && UnusedStreaming.Equals(other.UnusedStreaming)
                && UnusedPedTypeInfo.Equals(other.UnusedPedTypeInfo)
                && UnusedTags.Equals(other.UnusedTags)
                && UnusedShopping.Equals(other.UnusedShopping)
                && UnusedGangWars.Equals(other.UnusedGangWars)
                && UnusedEntryExits.Equals(other.UnusedEntryExits)
                && Unused3dMarkers.Equals(other.Unused3dMarkers)
                && UnusedVehicles.Equals(other.UnusedVehicles)
                && UnusedExtraBlock.Equals(other.UnusedExtraBlock)
                && GfwlData.Equals(other.GfwlData);
        }

        public static class FileFormats
        {
            public static readonly FileFormat PC = new FileFormat(
                "PC", "PC", "Windows",
                new GameConsole(GameConsoleType.Win32),
                new GameConsole(GameConsoleType.Win32, ConsoleFlags.Steam)
            );

            public static readonly FileFormat PS3 = new FileFormat(
                "PS3", "PS3", "PlayStation 3",
                new GameConsole(GameConsoleType.PS3)
            );

            public static readonly FileFormat Xbox360 = new FileFormat(
                "Xbox360", "Xbox 360", "Xbox 360",
                new GameConsole(GameConsoleType.Xbox360)
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { PC, PS3, Xbox360 };
            }
        }
    }
}
