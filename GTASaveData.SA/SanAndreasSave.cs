using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GTASaveData.SA
{
    public class SanAndreasSave : SaveFile, IGTASaveFile, IEquatable<SanAndreasSave>
    {
        public const int SizeOfOneGameInBytes = 202752;
        public const int BlockHeaderSize = 5;
        private const string BlockTagName = "BLOCK";

        private DataBuffer m_file;

        private DummyObject m_simpleVars;   // SimpleVariables
        private DummyObject m_scripts;      // TheScripts
        private DummyObject m_pools;   // Pools
        private DummyObject m_garages;   // Garages
        private DummyObject m_gameLogic;   // GameLogic
        private DummyObject m_pathFind;   // PathFind
        private DummyObject m_pickups;   // Pickups
        private DummyObject m_phoneInfo;   // PhoneInfo
        private DummyObject m_restart;   // Restart
        private DummyObject m_radar;   // Radar
        private DummyObject m_zones;   // TheZones
        private DummyObject m_gangs;   // Gangs
        private DummyObject m_carGenerators;   // TheCarGenerators
        private DummyObject m_pedGenerators;   // PedGenerators (empty)
        private DummyObject m_audioScriptObjects;   // AudioScriptObject (empty)
        private DummyObject m_playerInfo;   // PlayerInfo
        private DummyObject m_stats;   // Stats
        private DummyObject m_setPieces;   // SetPieces
        private DummyObject m_streaming;   // Streaming
        private DummyObject m_pedTypeInfo;   // PedType
        private DummyObject m_tagManager;   // TagManager
        private DummyObject m_iplStore;   // IplStore
        private DummyObject m_shopping;   // Shopping
        private DummyObject m_gangWars;   // GangWars
        private DummyObject m_stuntJumpManager;   // StuntJumpManager
        private DummyObject m_entryExitManager;   // EntryExitManager
        private DummyObject m_radioTrackManager;   // AERadioTrackManager
        private DummyObject m_user3dMarkers;   // User3dMarkers
        private DummyObject m_postEffects;   // PostEffects (Mobile only?)

        public DummyObject SimpleVars
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

        public DummyObject PathFind
        {
            get { return m_pathFind; }
            set { m_pathFind = value; }
        }

        public DummyObject Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; }
        }

        public DummyObject PhoneInfo
        {
            get { return m_phoneInfo; }
            set { m_phoneInfo = value; }
        }

        public DummyObject Restart
        {
            get { return m_restart; }
            set { m_restart = value; }
        }

        public DummyObject Radar
        {
            get { return m_radar; }
            set { m_radar = value; }
        }

        public DummyObject Zones
        {
            get { return m_zones; }
            set { m_zones = value; }
        }

        public DummyObject Gangs
        {
            get { return m_gangs; }
            set { m_gangs = value; }
        }

        public DummyObject CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; }
        }

        public DummyObject PedGenerators
        {
            get { return m_pedGenerators; }
            set { m_pedGenerators = value; }
        }

        public DummyObject AudioScriptObjects
        {
            get { return m_audioScriptObjects; }
            set { m_audioScriptObjects = value; }
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

        public DummyObject TagManager
        {
            get { return m_tagManager; }
            set { m_tagManager = value; }
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

        public DummyObject StuntJumpManager
        {
            get { return m_stuntJumpManager; }
            set { m_stuntJumpManager = value; }
        }

        public DummyObject EntryExitManager
        {
            get { return m_entryExitManager; }
            set { m_entryExitManager = value; }
        }

        public DummyObject RadioTrackManager
        {
            get { return m_radioTrackManager; }
            set { m_radioTrackManager = value; }
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

        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override DateTime TimeLastSaved { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override int BufferSize => (FileFormat.SupportedOnMobile) ? 65000 : 51200;

        public SanAndreasSave()
        {
            SimpleVars = new DummyObject();
            Scripts = new DummyObject();
            Pools = new DummyObject();
            Garages = new DummyObject();
            GameLogic = new DummyObject();
            PathFind = new DummyObject();
            Pickups = new DummyObject();
            PhoneInfo = new DummyObject();
            Restart = new DummyObject();
            Radar = new DummyObject();
            Zones = new DummyObject();
            Gangs = new DummyObject();
            CarGenerators = new DummyObject();
            PedGenerators = new DummyObject();
            AudioScriptObjects = new DummyObject();
            PlayerInfo = new DummyObject();
            Stats = new DummyObject();
            SetPieces = new DummyObject();
            Streaming = new DummyObject();
            PedTypeInfo = new DummyObject();
            TagManager = new DummyObject();
            IplStore = new DummyObject();
            Shopping = new DummyObject();
            GangWars = new DummyObject();
            StuntJumpManager = new DummyObject();
            EntryExitManager = new DummyObject();
            RadioTrackManager = new DummyObject();
            User3dMarkers = new DummyObject();
            PostEffects = new DummyObject();
        }

        private DummyObject LoadDummy(int size)
        {
            LoadDataFromWorkBuffer(size, out byte[] data);
            DummyObject obj = new DummyObject(size);
            int count = Serializer.Read(obj, data, FileFormat);

            Debug.Assert(count == size);

            return obj;
        }

        private int SaveObject(SaveDataObject o)
        {
            int count = Serializer.Write(o, FileFormat, out byte[] data);
            int written = SaveDataToWorkBuffer(data, count);

            Debug.Assert(written == count);

            return written;
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

        protected override void LoadAllData(DataBuffer file)
        {
            List<int> blockSizes;
            byte[] fileData;
            int index;

            m_file = file;
            fileData = file.GetBytes();
            blockSizes = new List<int>();
            
            index = fileData.FindFirst(BlockTagName.GetAsciiBytes(), 0);
            Debug.Assert(index == 0);
            index += BlockHeaderSize;

            do
            {
                int mark = index;
                index = fileData.FindFirst(BlockTagName.GetAsciiBytes(), mark);
                blockSizes.Add(index - mark);
                index += BlockHeaderSize;
            } while (index - BlockHeaderSize > -1);

            WorkBuff.Seek(BufferSize);
            for (index = 0; index < 28; index++)            // TODO: 29 on android?
            {
                LoadDataFromWorkBuffer(5, out byte[] tagBytes);
                string tag = Encoding.ASCII.GetString(tagBytes);
                Debug.Assert(tag == BlockTagName);

                int size = blockSizes[index];
                switch (index)
                {
                    case 0: SimpleVars = LoadDummy(size); break;
                    case 1: Scripts = LoadDummy(size); break;
                    case 2: Pools = LoadDummy(size); break;
                    case 3: Garages = LoadDummy(size); break;
                    case 4: GameLogic = LoadDummy(size); break;
                    case 5: PathFind = LoadDummy(size); break;
                    case 6: Pickups = LoadDummy(size); break;
                    case 7: PhoneInfo = LoadDummy(size); break;
                    case 8: Restart = LoadDummy(size); break;
                    case 9: Radar = LoadDummy(size); break;
                    case 10: Zones = LoadDummy(size); break;
                    case 11: Gangs = LoadDummy(size); break;
                    case 12: CarGenerators = LoadDummy(size); break;
                    case 13: PedGenerators = LoadDummy(size); break;
                    case 14: AudioScriptObjects = LoadDummy(size); break;
                    case 15: PlayerInfo = LoadDummy(size); break;
                    case 16: Stats = LoadDummy(size); break;
                    case 17: SetPieces = LoadDummy(size); break;
                    case 18: Streaming = LoadDummy(size); break;
                    case 19: PedTypeInfo = LoadDummy(size); break;
                    case 20: TagManager = LoadDummy(size); break;
                    case 21: IplStore  = LoadDummy(size); break;
                    case 22: Shopping = LoadDummy(size); break;
                    case 23: GangWars = LoadDummy(size); break;
                    case 24: StuntJumpManager= LoadDummy(size); break;
                    case 25: EntryExitManager = LoadDummy(size); break;
                    case 26: RadioTrackManager = LoadDummy(size); break;
                    case 27: User3dMarkers = LoadDummy(size); break;
                    case 28: PostEffects = LoadDummy(size); break;      // ???
                }
                Debug.WriteLine("Read {0} bytes of block data.", size);
            }

            Debug.Assert(m_file.Position == SizeOfOneGameInBytes);
        }

        protected override void SaveAllData(DataBuffer file)
        {
            int index;
            int size;

            m_file = file;
            CheckSum = 0;
            WorkBuff.Seek(0);

            for (index = 0; index < 28; index++)            // TODO: 29 on android?
            {
                SaveDataToWorkBuffer(BlockTagName.GetAsciiBytes(), 5);

                size = 0;
                switch (index)
                {
                    case 0: size = SaveObject(SimpleVars); break;
                    case 1: size = SaveObject(Scripts); break;
                    case 2: size = SaveObject(Pools); break;
                    case 3: size = SaveObject(Garages); break;
                    case 4: size = SaveObject(GameLogic); break;
                    case 5: size = SaveObject(PathFind); break;
                    case 6: size = SaveObject(Pickups); break;
                    case 7: size = SaveObject(PhoneInfo); break;
                    case 8: size = SaveObject(Restart); break;
                    case 9: size = SaveObject(Radar); break;
                    case 10: size = SaveObject(Zones); break;
                    case 11: size = SaveObject(Gangs); break;
                    case 12: size = SaveObject(CarGenerators); break;
                    case 13: size = SaveObject(PedGenerators); break;
                    case 14: size = SaveObject(AudioScriptObjects); break;
                    case 15: size = SaveObject(PlayerInfo); break;
                    case 16: size = SaveObject(Stats); break;
                    case 17: size = SaveObject(SetPieces); break;
                    case 18: size = SaveObject(Streaming); break;
                    case 19: size = SaveObject(PedTypeInfo); break;
                    case 20: size = SaveObject(TagManager); break;
                    case 21: size = SaveObject(IplStore); break;
                    case 22: size = SaveObject(Shopping); break;
                    case 23: size = SaveObject(GangWars); break;
                    case 24: size = SaveObject(StuntJumpManager); break;
                    case 25: size = SaveObject(EntryExitManager); break;
                    case 26: size = SaveObject(RadioTrackManager); break;
                    case 27: size = SaveObject(User3dMarkers); break;
                    case 28: size = SaveObject(PostEffects); break;      // ???
                }
                Debug.WriteLine("Wrote {0} bytes of block data.", size);
            }

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

        protected override List<SaveDataObject> GetBlocks()
        {
            return new List<SaveDataObject>()
            {
                SimpleVars,
                Scripts,
                Pools,
                Garages,
                GameLogic,
                PathFind,
                Pickups,
                PhoneInfo,
                Restart,
                Radar,
                Zones,
                Gangs,
                CarGenerators,
                PedGenerators,
                AudioScriptObjects,
                PlayerInfo,
                Stats,
                SetPieces,
                Streaming,
                PedTypeInfo,
                TagManager,
                IplStore,
                Shopping,
                GangWars,
                StuntJumpManager,
                EntryExitManager,
                RadioTrackManager,
                User3dMarkers,
                PostEffects
            };
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
                && PathFind.Equals(other.PathFind)
                && Pickups.Equals(other.Pickups)
                && PhoneInfo.Equals(other.PhoneInfo)
                && Restart.Equals(other.Restart)
                && Radar.Equals(other.Radar)
                && Zones.Equals(other.Zones)
                && Gangs.Equals(other.Gangs)
                && CarGenerators.Equals(other.CarGenerators)
                && PedGenerators.Equals(other.PedGenerators)
                && AudioScriptObjects.Equals(other.AudioScriptObjects)
                && PlayerInfo.Equals(other.PlayerInfo)
                && Stats.Equals(other.Stats)
                && SetPieces.Equals(other.SetPieces)
                && Streaming.Equals(other.Streaming)
                && PedTypeInfo.Equals(other.PedTypeInfo)
                && TagManager.Equals(other.TagManager)
                && IplStore.Equals(other.IplStore)
                && Shopping.Equals(other.Shopping)
                && GangWars.Equals(other.GangWars)
                && StuntJumpManager.Equals(other.StuntJumpManager)
                && EntryExitManager.Equals(other.EntryExitManager)
                && RadioTrackManager.Equals(other.RadioTrackManager)
                && User3dMarkers.Equals(other.User3dMarkers)
                && PostEffects.Equals(other.PostEffects);
        }
    }

    public static class FileFormats
    {
        public static readonly SaveFileFormat PC = new SaveFileFormat(
            "PC", "PC (Windows/macOS)",
            new GameConsole(ConsoleType.Win32),
            new GameConsole(ConsoleType.MacOS)
        );

        public static SaveFileFormat[] GetAll()
        {
            return new SaveFileFormat[] { PC };
        }
    }
}
