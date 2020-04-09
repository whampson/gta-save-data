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
    public class GTA4Save : SaveFile, IGTASaveFile, IEquatable<GTA4Save>
    {
        public static class Limits
        {
            public const int MaxSaveNameLength = 128;
        }

        private DataBuffer m_file;

        private int m_saveVersion;
        private int m_saveSizeInBytes;
        private int m_scriptSpace;
        private string m_lastMissionPassedName;

        private SimpleVariables m_simpleVars;
        private DummyObject m_playerInfo;
        private DummyObject m_extraContent;
        private DummyObject m_scripts;
        private DummyObject m_garages;
        private DummyObject m_gameLogic;
        private DummyObject m_paths;
        private Pickups m_pickups;
        private DummyObject m_restartPoints;
        private DummyObject m_radarBlips;
        private DummyObject m_zones;
        private DummyObject m_gangData;
        private DummyObject m_carGenerators;
        private DummyObject m_stats;
        private DummyObject m_iplStore;
        private DummyObject m_stuntJumps;
        private DummyObject m_radio;
        private DummyObject m_objects;
        private DummyObject m_relationships;
        private DummyObject m_inventory;
        private DummyObject m_unusedPools;
        private DummyObject m_unusedPhoneInfo;
        private DummyObject m_unusedAudioScript;
        private DummyObject m_unusedSetPieces;
        private DummyObject m_unusedStreaming;
        private DummyObject m_unusedPedTypeInfo;
        private DummyObject m_unusedTags;
        private DummyObject m_unusedShopping;
        private DummyObject m_unusedGangWars;
        private DummyObject m_unusedEntryExits;
        private DummyObject m_unused3dMarkers;
        private DummyObject m_unusedVehicles;
        private DummyObject m_unusedExtraBlock;
        private DummyObject m_gfwlData;

        public override string Name
        {
            get { return m_lastMissionPassedName; }
            set { m_lastMissionPassedName = value; OnPropertyChanged(); }
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

        public DummyObject PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; OnPropertyChanged(); }
        }

        public DummyObject ExtraContent
        {
            get { return m_extraContent; }
            set { m_extraContent = value; OnPropertyChanged(); }
        }

        public DummyObject Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; OnPropertyChanged(); }
        }

        public DummyObject Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public DummyObject GameLogic
        {
            get { return m_gameLogic; }
            set { m_gameLogic = value; OnPropertyChanged(); }
        }

        public DummyObject Paths
        {
            get { return m_paths; }
            set { m_paths = value; OnPropertyChanged(); }
        }

        public Pickups Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
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

        public DummyObject Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public DummyObject IplStore
        {
            get { return m_iplStore; }
            set { m_iplStore = value; OnPropertyChanged(); }
        }

        public DummyObject StuntJumps
        {
            get { return m_stuntJumps; }
            set { m_stuntJumps = value; OnPropertyChanged(); }
        }

        public DummyObject Radio
        {
            get { return m_radio; }
            set { m_radio = value; OnPropertyChanged(); }
        }

        public DummyObject Objects
        {
            get { return m_objects; }
            set { m_objects = value; OnPropertyChanged(); }
        }

        public DummyObject Relationships
        {
            get { return m_relationships; }
            set { m_relationships = value; OnPropertyChanged(); }
        }

        public DummyObject Inventory
        {
            get { return m_inventory; }
            set { m_inventory = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedPools
        {
            get { return m_unusedPools; }
            set { m_unusedPools = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedPhoneInfo
        {
            get { return m_unusedPhoneInfo; }
            set { m_unusedPhoneInfo = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedAudioScript
        {
            get { return m_unusedAudioScript; }
            set { m_unusedAudioScript = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedSetPieces
        {
            get { return m_unusedSetPieces; }
            set { m_unusedSetPieces = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedStreaming
        {
            get { return m_unusedStreaming; }
            set { m_unusedStreaming = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedPedTypeInfo
        {
            get { return m_unusedPedTypeInfo; }
            set { m_unusedPedTypeInfo = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedTags
        {
            get { return m_unusedTags; }
            set { m_unusedTags = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedShopping
        {
            get { return m_unusedShopping; }
            set { m_unusedShopping = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedGangWars
        {
            get { return m_unusedGangWars; }
            set { m_unusedGangWars = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedEntryExits
        {
            get { return m_unusedEntryExits; }
            set { m_unusedEntryExits = value; OnPropertyChanged(); }
        }

        public DummyObject Unused3dMarkers
        {
            get { return m_unused3dMarkers; }
            set { m_unused3dMarkers = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedVehicles
        {
            get { return m_unusedVehicles; }
            set { m_unusedVehicles = value; OnPropertyChanged(); }
        }

        public DummyObject UnusedExtraBlock
        {
            get { return m_unusedExtraBlock; }
            set { m_unusedExtraBlock = value; OnPropertyChanged(); }
        }

        public DummyObject GfwlData
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
            PlayerInfo = new DummyObject();
            ExtraContent = new DummyObject();
            Scripts = new DummyObject();
            Garages = new DummyObject();
            GameLogic = new DummyObject();
            Paths = new DummyObject();
            Pickups = new Pickups();
            RestartPoints = new DummyObject();
            RadarBlips = new DummyObject();
            Zones = new DummyObject();
            GangData = new DummyObject();
            CarGenerators = new DummyObject();
            Stats = new DummyObject();
            IplStore = new DummyObject();
            StuntJumps = new DummyObject();
            Radio = new DummyObject();
            Objects = new DummyObject();
            Relationships = new DummyObject();
            Inventory = new DummyObject();
            UnusedPools = new DummyObject();
            UnusedPhoneInfo = new DummyObject();
            UnusedAudioScript = new DummyObject();
            UnusedSetPieces = new DummyObject();
            UnusedStreaming = new DummyObject();
            UnusedPedTypeInfo = new DummyObject();
            UnusedTags = new DummyObject();
            UnusedShopping = new DummyObject();
            UnusedGangWars = new DummyObject();
            UnusedEntryExits = new DummyObject();
            Unused3dMarkers = new DummyObject();
            UnusedVehicles = new DummyObject();
            UnusedExtraBlock = new DummyObject();
            GfwlData = new DummyObject();
        }

        private void LoadFileHeader()
        {
            SaveVersion = m_file.ReadInt32();
            SaveSizeInBytes = m_file.ReadInt32();
            ScriptSpace = m_file.ReadInt32();

            string sig = m_file.ReadString(4);
            if (FileFormat.SupportedOnWin32)
            {
                Name = m_file.ReadString(Limits.MaxSaveNameLength, unicode: true);
            }

            Debug.Assert(sig == "SAVE", "Invalid 'SAVE' signature!");
            Debug.Assert(SaveSizeInBytes == m_file.Length, "SaveSizeInBytes value incorrect!");
        }

        private void LoadFileFooter()
        {
            string sig = m_file.ReadString(4);
            Debug.Assert(sig == "END", "Invalid 'END' signature!");

            if (FileFormat.SupportedOnWin32)
            {
                int size = m_file.Length - m_file.Position;
                GfwlData = new DummyObject(m_file.ReadBytes(size));
            }
        }

        private T LoadData<T>() where T : SaveDataObject, new()
        {
            string sig = m_file.ReadString(5);
            int size = m_file.ReadInt32();
            Debug.Assert(sig == "BLOCK", "Invalid 'BLOCK' signature!");

            m_file.MarkPosition();
            T obj = m_file.Read<T>();
            Debug.Assert(m_file.Offset == size - 9);

            return obj;
        }

        private DummyObject LoadDummy(byte[] data)
        {
            return new DummyObject(data);
        }

        private byte[] LoadBlockData()
        {
            string sig = m_file.ReadString(5);
            int size = m_file.ReadInt32();
            Debug.Assert(sig == "BLOCK", "Invalid 'BLOCK' signature!");
            
            m_file.MarkPosition();
            byte[] data = m_file.ReadBytes(size - 9);
            Debug.Assert(m_file.Offset == size - 9);
            
            return data;
        }

        protected override void LoadAllData(DataBuffer file)
        {
            m_file = file;
            m_file.BigEndian = (FileFormat.SupportedOnXbox360 || FileFormat.SupportedOnPS3);

            LoadFileHeader();

            int blockCount = (FileFormat.SupportedOnPS3) ? 33 : 32;
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

        protected override bool DetectFileFormat(byte[] data, out SaveFileFormat fmt)
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

            fmt = SaveFileFormat.Default;
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
            public static readonly SaveFileFormat PC = new SaveFileFormat(
                "PC", "PC", "Windows",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.Win32, ConsoleFlags.Steam)
            );

            public static readonly SaveFileFormat PS3 = new SaveFileFormat(
                "PS3", "PS3", "PlayStation 3",
                new GameConsole(ConsoleType.PS3)
            );

            public static readonly SaveFileFormat Xbox360 = new SaveFileFormat(
                "Xbox360", "Xbox 360", "Xbox 360",
                new GameConsole(ConsoleType.Xbox360)
            );

            public static SaveFileFormat[] GetAll()
            {
                return new SaveFileFormat[] { PC, PS3, Xbox360 };
            }
        }
    }
}
