using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GTASaveData.SA
{
    /// <summary>
    /// Represents a <i>Grand Theft Auto: San Andreas</i> save file.
    /// </summary>
    public class SanAndreasSave : SaveFile, IGTASaveFile, IEquatable<SanAndreasSave>
    {
        public const int SizeOfOneGameInBytes = 202752;
        private const int BlockHeaderSize = 5;
        private const int BlockCount = 28;
        private const int BlockCountMobile = 29;
        private const string BlockTagName = "BLOCK";

        protected override int BufferSize => (FileFormat.SupportedOnMobile) ? 65000 : 51200;

        private DataBuffer m_file;
        private SimpleVariables m_simpleVars;
        private DummyObject m_scripts;      // TheScripts
        private DummyObject m_pools;   // Pools
        private DummyObject m_garages;   // Garages
        private DummyObject m_gameLogic;   // GameLogic
        private DummyObject m_paths;   // PathFind
        private DummyObject m_pickups;   // Pickups
        private DummyObject m_block7;   // empty
        private DummyObject m_restartPoints;   // Restart
        private DummyObject m_radarBlips;   // Radar
        private DummyObject m_zones;   // TheZones
        private DummyObject m_gangData;   // Gangs
        private DummyObject m_carGenerators;   // TheCarGenerators
        private DummyObject m_block13;   // empty
        private DummyObject m_block14;   // empty
        private DummyObject m_playerInfo;   // PlayerInfo
        private DummyObject m_stats;   // Stats
        private DummyObject m_setPieces;   // SetPieces
        private DummyObject m_streaming;   // Streaming
        private DummyObject m_pedTypeInfo;   // PedType
        private DummyObject m_tags;   // TagManager
        private DummyObject m_iplStore;   // IplStore
        private DummyObject m_shopping;   // Shopping
        private DummyObject m_gangWars;   // GangWars
        private DummyObject m_stuntJumps;   // StuntJumpManager
        private DummyObject m_entryExits;   // EntryExitManager
        private DummyObject m_radio;   // AERadioTrackManager
        private DummyObject m_user3dMarkers;   // User3dMarkers
        private DummyObject m_postEffects;   // PostEffects (Mobile only)

        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; }
        }

        public DummyObject Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; }
        }

        public DummyObject Pools
        {
            get { return m_pools; }
            set { m_pools = value; }
        }

        public DummyObject Garages
        {
            get { return m_garages; }
            set { m_garages = value; }
        }

        public DummyObject GameLogic
        {
            get { return m_gameLogic; }
            set { m_gameLogic = value; }
        }

        public DummyObject Paths
        {
            get { return m_paths; }
            set { m_paths = value; }
        }

        public DummyObject Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; }
        }

        public DummyObject PhoneInfo
        {
            get { return m_block7; }
            set { m_block7 = value; }
        }

        public DummyObject RestartPoints
        {
            get { return m_restartPoints; }
            set { m_restartPoints = value; }
        }

        public DummyObject RadarBlips
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; }
        }

        public DummyObject Zones
        {
            get { return m_zones; }
            set { m_zones = value; }
        }

        public DummyObject GangData
        {
            get { return m_gangData; }
            set { m_gangData = value; }
        }

        public DummyObject CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; }
        }

        public DummyObject PedGenerators
        {
            get { return m_block13; }
            set { m_block13 = value; }
        }

        public DummyObject AudioScriptObjects
        {
            get { return m_block14; }
            set { m_block14 = value; }
        }

        public DummyObject PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; }
        }

        public DummyObject Stats
        {
            get { return m_stats; }
            set { m_stats = value; }
        }

        public DummyObject SetPieces
        {
            get { return m_setPieces; }
            set { m_setPieces = value; }
        }

        public DummyObject Streaming
        {
            get { return m_streaming; }
            set { m_streaming = value; }
        }

        public DummyObject PedTypeInfo
        {
            get { return m_pedTypeInfo; }
            set { m_pedTypeInfo = value; }
        }

        public DummyObject Tags
        {
            get { return m_tags; }
            set { m_tags = value; }
        }

        public DummyObject IplStore
        {
            get { return m_iplStore; }
            set { m_iplStore = value; }
        }

        public DummyObject Shopping
        {
            get { return m_shopping; }
            set { m_shopping = value; }
        }

        public DummyObject GangWars
        {
            get { return m_gangWars; }
            set { m_gangWars = value; }
        }

        public DummyObject StuntJumps
        {
            get { return m_stuntJumps; }
            set { m_stuntJumps = value; }
        }

        public DummyObject EntryExits
        {
            get { return m_entryExits; }
            set { m_entryExits = value; }
        }

        public DummyObject Radio
        {
            get { return m_radio; }
            set { m_radio = value; }
        }

        public DummyObject User3dMarkers
        {
            get { return m_user3dMarkers; }
            set { m_user3dMarkers = value; }
        }

        public DummyObject PostEffects
        {
            get { return m_postEffects; }
            set { m_postEffects = value; }
        }

        public override string Name
        {
            get { return SimpleVars.LastMissionPassedName; }
            set { SimpleVars.LastMissionPassedName = value; OnPropertyChanged(); }
        }

        public override DateTime TimeLastSaved
        {
            get { return SimpleVars.TimeLastSaved.ToDateTime(); }
            set { SimpleVars.TimeLastSaved = new SystemTime(value); OnPropertyChanged(); }
        }

        public override IReadOnlyList<SaveDataObject> Blocks => new List<SaveDataObject>()
        {
            SimpleVars,
            Scripts,
            Pools,
            Garages,
            GameLogic,
            Paths,
            Pickups,
            PhoneInfo,
            RestartPoints,
            RadarBlips,
            Zones,
            GangData,
            CarGenerators,
            PedGenerators,
            AudioScriptObjects,
            PlayerInfo,
            Stats,
            SetPieces,
            Streaming,
            PedTypeInfo,
            Tags,
            IplStore,
            Shopping,
            GangWars,
            StuntJumps,
            EntryExits,
            Radio,
            User3dMarkers,
            PostEffects
        };

        public SanAndreasSave()
        {
            SimpleVars = new SimpleVariables();
            Scripts = new DummyObject();
            Pools = new DummyObject();
            Garages = new DummyObject();
            GameLogic = new DummyObject();
            Paths = new DummyObject();
            Pickups = new DummyObject();
            PhoneInfo = new DummyObject();
            RestartPoints = new DummyObject();
            RadarBlips = new DummyObject();
            Zones = new DummyObject();
            GangData = new DummyObject();
            CarGenerators = new DummyObject();
            PedGenerators = new DummyObject();
            AudioScriptObjects = new DummyObject();
            PlayerInfo = new DummyObject();
            Stats = new DummyObject();
            SetPieces = new DummyObject();
            Streaming = new DummyObject();
            PedTypeInfo = new DummyObject();
            Tags = new DummyObject();
            IplStore = new DummyObject();
            Shopping = new DummyObject();
            GangWars = new DummyObject();
            StuntJumps = new DummyObject();
            EntryExits = new DummyObject();
            Radio = new DummyObject();
            User3dMarkers = new DummyObject(0x8C);
            PostEffects = new DummyObject(0x160);
        }

        private int LoadBlockHeader()
        {
            int count = LoadDataFromWorkBuffer(5, out byte[] tagBytes);
            string tag = Encoding.ASCII.GetString(tagBytes);

            Debug.Assert(tag == BlockTagName);
            return count;
        }

        private int SaveBlockHeader()
        {
            return SaveDataToWorkBuffer(BlockTagName.GetAsciiBytes(), 5);
        }

        private DummyObject LoadDummy(int size)
        {
            LoadDataFromWorkBuffer(size, out byte[] data);
            DummyObject obj = new DummyObject(size);
            int count = Serializer.Read(obj, data, FileFormat);

            Debug.Assert(count == size);
            return obj;
        }


        private T LoadObject<T>(int size) where T : SaveDataObject, new()
        {
            LoadDataFromWorkBuffer(size, out byte[] data);
            int count = Serializer.Read(data, FileFormat, out T obj);

            Debug.Assert(size == count);
            return obj;
        }

        private int SaveObject(SaveDataObject o)
        {
            int count = Serializer.Write(o, FileFormat, out byte[] data);
            int written = SaveDataToWorkBuffer(data, count);

            Debug.Assert(written == count);

            return written;
        }

        private int LoadDataFromWorkBuffer(int count, out byte[] data)
        {
            if (count <= 0)
            {
                data = new byte[0];
                return 0;
            }

            int total = 0;
            int index = 0;

            data = new byte[count];
            if (WorkBuff.Position + count > BufferSize)
            {
                int len = BufferSize - WorkBuff.Position;
                total += LoadDataFromWorkBuffer(len, out byte[] tmp);
                Array.Copy(tmp, 0, data, 0, len);
                LoadWorkBuffer();
                count -= len;
                index += len;
            }


            total += WorkBuff.Read(data, index, count);
            return total;
        }

        private int SaveDataToWorkBuffer(byte[] data, int count)
        {
            if (count <= 0)
            {
                return 0;
            }

            int total = 0;
            int index = 0;

            if (WorkBuff.Position + count > BufferSize)
            {
                int len = BufferSize - WorkBuff.Position;
                total += SaveDataToWorkBuffer(data, len);
                SaveWorkBuffer(false);
                count -= len;
                index += len;
            }

            total += WorkBuff.Write(data, index, count);
            return total;
        }

        private void LoadWorkBuffer()
        {
            int count;

            count = BufferSize;
            if (m_file.Position + count > m_file.Length)
            {
                count = m_file.Length - m_file.Position;
            }

            if (count != 0 && count == DataBuffer.Align4Bytes(count))
            {
                WorkBuff.Reset();
                WorkBuff.Write(m_file.ReadBytes(count));
                WorkBuff.Reset();
            }
        }

        private void SaveWorkBuffer(bool writeCheckSum)
        {
            CheckSum += (uint) WorkBuff.GetBytesUpToCursor().Sum(x => x);
            if (writeCheckSum)
            {
                if (WorkBuff.Position > BufferSize - 4)
                {
                    SaveWorkBuffer(false);
                }
                WorkBuff.Write(CheckSum);
            }

            m_file.Write(WorkBuff.GetBytesUpToCursor());
            WorkBuff.Reset();
        }

        protected override void LoadAllData(DataBuffer file)
        {
            List<int> blockSizes;
            byte[] fileData;
            int numBlocks, numCounted;
            int size, offset;
            int mark;

            m_file = file;
            fileData = file.GetBytes();
            blockSizes = new List<int>();
            
            offset = fileData.FindFirst(BlockTagName.GetAsciiBytes(), 0);
            numCounted = 0;

            // Get block sizes
            while (offset > -1)
            {
                offset += BlockHeaderSize;
                mark = offset;
                offset = fileData.FindFirst(BlockTagName.GetAsciiBytes(), mark);

                // Padding after final block has no clear beginning marker,
                // need to know exact size of last block
                if (numCounted == 27 && !FileFormat.SupportedOnMobile)
                {
                    size = 0x8C;    // SizeOf<C3dMarkers>()
                }
                else if (numCounted == 28 && FileFormat.SupportedOnMobile)
                {
                    size = 0x160;   // SizeOf<PostEffects>();
                }
                else
                {
                    size = offset - mark;
                }

                blockSizes.Add(size);
                numCounted++;
            }

            numBlocks = (FileFormat.SupportedOnMobile) ? BlockCountMobile : BlockCount;
            Debug.Assert(numCounted >= numBlocks);

            // Init pointer at end so loader thinks buffer is full and refills it
            WorkBuff.Seek(BufferSize);
            
            for (offset = 0; offset < numBlocks; offset++)
            {
                size = blockSizes[offset];
                LoadBlockHeader();

                switch (offset)
                {
                    case 0: SimpleVars = LoadObject<SimpleVariables>(size); break;
                    case 1: Scripts = LoadDummy(size); break;
                    case 2: Pools = LoadDummy(size); break;
                    case 3: Garages = LoadDummy(size); break;
                    case 4: GameLogic = LoadDummy(size); break;
                    case 5: Paths = LoadDummy(size); break;
                    case 6: Pickups = LoadDummy(size); break;
                    case 7: PhoneInfo = LoadDummy(size); break;
                    case 8: RestartPoints = LoadDummy(size); break;
                    case 9: RadarBlips = LoadDummy(size); break;
                    case 10: Zones = LoadDummy(size); break;
                    case 11: GangData = LoadDummy(size); break;
                    case 12: CarGenerators = LoadDummy(size); break;
                    case 13: PedGenerators = LoadDummy(size); break;
                    case 14: AudioScriptObjects = LoadDummy(size); break;
                    case 15: PlayerInfo = LoadDummy(size); break;
                    case 16: Stats = LoadDummy(size); break;
                    case 17: SetPieces = LoadDummy(size); break;
                    case 18: Streaming = LoadDummy(size); break;
                    case 19: PedTypeInfo = LoadDummy(size); break;
                    case 20: Tags = LoadDummy(size); break;
                    case 21: IplStore = LoadDummy(size); break;
                    case 22: Shopping = LoadDummy(size); break;
                    case 23: GangWars = LoadDummy(size); break;
                    case 24: StuntJumps = LoadDummy(size); break;
                    case 25: EntryExits = LoadDummy(size); break;
                    case 26: Radio = LoadDummy(size); break;
                    case 27: User3dMarkers = LoadDummy(size); break;
                    case 28: PostEffects = LoadDummy(size); break;      // mobile only
                }
                Debug.WriteLine("Read {0} bytes of block data.", size);
            }

            // (mobile) TODO: briefs??
        }

        protected override void SaveAllData(DataBuffer file)
        {
            int index;
            int size;
            int count;

            m_file = file;
            CheckSum = 0;
            WorkBuff.Seek(0);

            count = (FileFormat.SupportedOnMobile) ? BlockCountMobile : BlockCount;
            for (index = 0; index < count; index++)
            {
                size = 0;
                SaveBlockHeader();

                switch (index)
                {
                    case 0: size = SaveObject(SimpleVars); break;
                    case 1: size = SaveObject(Scripts); break;
                    case 2: size = SaveObject(Pools); break;
                    case 3: size = SaveObject(Garages); break;
                    case 4: size = SaveObject(GameLogic); break;
                    case 5: size = SaveObject(Paths); break;
                    case 6: size = SaveObject(Pickups); break;
                    case 7: size = SaveObject(PhoneInfo); break;
                    case 8: size = SaveObject(RestartPoints); break;
                    case 9: size = SaveObject(RadarBlips); break;
                    case 10: size = SaveObject(Zones); break;
                    case 11: size = SaveObject(GangData); break;
                    case 12: size = SaveObject(CarGenerators); break;
                    case 13: size = SaveObject(PedGenerators); break;
                    case 14: size = SaveObject(AudioScriptObjects); break;
                    case 15: size = SaveObject(PlayerInfo); break;
                    case 16: size = SaveObject(Stats); break;
                    case 17: size = SaveObject(SetPieces); break;
                    case 18: size = SaveObject(Streaming); break;
                    case 19: size = SaveObject(PedTypeInfo); break;
                    case 20: size = SaveObject(Tags); break;
                    case 21: size = SaveObject(IplStore); break;
                    case 22: size = SaveObject(Shopping); break;
                    case 23: size = SaveObject(GangWars); break;
                    case 24: size = SaveObject(StuntJumps); break;
                    case 25: size = SaveObject(EntryExits); break;
                    case 26: size = SaveObject(Radio); break;
                    case 27: size = SaveObject(User3dMarkers); break;
                    case 28: size = SaveObject(PostEffects); break;     // mobile only
                }
                Debug.WriteLine("Wrote {0} bytes of block data.", size);
            }

            // (mobile) TODO: briefs??

            // Android
            //{
            //    // Padding
            //    if (WorkBuff.Position > BufferSize - 4)
            //    {
            //        WorkBuff.Seek(BufferSize);
            //        SaveWorkBuffer(false);
            //    }
            //    else
            //    {
            //        WorkBuff.Seek(BufferSize - 4);
            //    }
            //    // Checksum
            //    SaveWorkBuffer(true);
            //}

            // PC
            size = WorkBuff.Position + file.Position;
            while (size < SizeOfOneGameInBytes - 4)
            {
                int remaining = SizeOfOneGameInBytes - file.Position - WorkBuff.Position - 4;
                if (WorkBuff.Position + remaining < BufferSize)
                {
                    WorkBuff.Skip(remaining);
                    break;
                }

                WorkBuff.Seek(BufferSize);
                SaveWorkBuffer(false);
                size = WorkBuff.Position + file.Position;
            }
            SaveWorkBuffer(true);

            Debug.Assert(m_file.Position == SizeOfOneGameInBytes);
        }

        protected override bool DetectFileFormat(byte[] data, out SaveFileFormat fmt)
        {
            // TODO
            fmt = FileFormats.PC;
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SanAndreasSave);
        }

        public bool Equals(SanAndreasSave other)
        {
            if (other == null)
            {
                return false;
            }

            return SimpleVars.Equals(other.SimpleVars)
                && Scripts.Equals(other.Scripts)
                && Pools.Equals(other.Pools)
                && Garages.Equals(other.Garages)
                && GameLogic.Equals(other.GameLogic)
                && Paths.Equals(other.Paths)
                && Pickups.Equals(other.Pickups)
                && PhoneInfo.Equals(other.PhoneInfo)
                && RestartPoints.Equals(other.RestartPoints)
                && RadarBlips.Equals(other.RadarBlips)
                && Zones.Equals(other.Zones)
                && GangData.Equals(other.GangData)
                && CarGenerators.Equals(other.CarGenerators)
                && PedGenerators.Equals(other.PedGenerators)
                && AudioScriptObjects.Equals(other.AudioScriptObjects)
                && PlayerInfo.Equals(other.PlayerInfo)
                && Stats.Equals(other.Stats)
                && SetPieces.Equals(other.SetPieces)
                && Streaming.Equals(other.Streaming)
                && PedTypeInfo.Equals(other.PedTypeInfo)
                && Tags.Equals(other.Tags)
                && IplStore.Equals(other.IplStore)
                && Shopping.Equals(other.Shopping)
                && GangWars.Equals(other.GangWars)
                && StuntJumps.Equals(other.StuntJumps)
                && EntryExits.Equals(other.EntryExits)
                && Radio.Equals(other.Radio)
                && User3dMarkers.Equals(other.User3dMarkers)
                && PostEffects.Equals(other.PostEffects);
        }

        public static class FileFormats
        {
            // TODO: 1.05 and 1.06 different?
            public static readonly SaveFileFormat Mobile = new SaveFileFormat(
                "Mobile", "Mobile", "Android, iOS",
                new GameConsole(ConsoleType.Android),
                new GameConsole(ConsoleType.iOS)
            );

            public static readonly SaveFileFormat PC = new SaveFileFormat(
                "PC", "PC", "Windows, macOS",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.MacOS)
            );

            public static readonly SaveFileFormat PS2 = new SaveFileFormat(
                "PS2", "PS2", "PlayStation 2",
                new GameConsole(ConsoleType.PS2)
            );

            public static readonly SaveFileFormat Xbox = new SaveFileFormat(
                "Xbox", "Xbox", "Xbox",
                new GameConsole(ConsoleType.Xbox)
            );

            public static SaveFileFormat[] GetAll()
            {
                return new SaveFileFormat[] { Mobile, PC, PS2, Xbox };
            }
        }
    }
}
