using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Represents a save file for <i>Grand Theft Auto III</i>.
    /// </summary>
    public class GTA3Save : SaveFile, IGTASaveFile, IEquatable<GTA3Save>
    {
        public const int SaveHeaderSize = 8;
        public const int SizeOfOneGameInBytes = 201729;

        private SimpleVariables m_simpleVars;
        private TheScripts m_scripts;
        private DummyObject m_pedPool;
        private DummyObject m_garages;
        private DummyObject m_vehiclePool;
        private DummyObject m_objectPool;
        private DummyObject m_paths;
        private DummyObject m_cranes;
        private DummyObject m_pickups;
        private DummyObject m_phoneInfo;
        private DummyObject m_restartPoints;
        private DummyObject m_radarBlips;
        private DummyObject m_zones;
        private DummyObject m_gangData;
        private DummyObject m_carGenerators;
        private DummyObject m_particleObjects;
        private DummyObject m_audioScriptObjects;
        private DummyObject m_playerInfo;
        private DummyObject m_stats;
        private DummyObject m_streaming;
        private DummyObject m_pedTypeInfo;

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

        public override string Name
        {
            get { return SimpleVars.SaveName; }
            set { SimpleVars.SaveName = value; OnPropertyChanged(); }
        }

        public override DateTime TimeLastSaved
        {
            get { return SimpleVars.TimeLastSaved.ToDateTime(); }
            set { SimpleVars.TimeLastSaved = new SystemTime(value); OnPropertyChanged(); }
        }

        protected override int BufferSize => (FileFormat.SupportedOnPS2) ? 50000 : 55000;

        public GTA3Save()
        {
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
        }

        public static int ReadSaveHeader(DataBuffer buf, string tag)
        {
            string readTag = buf.ReadString(4);
            int size = buf.ReadInt32();

            Debug.Assert(tag == readTag, "Invalid block tag (expected: {0}, actual: {1})", tag, readTag);
            return size;
        }

        public static void WriteSaveHeader(DataBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4, zeroTerminate: true);
            buf.Write(size);
        }

        private T LoadData<T>() where T : SaveDataObject, new()
        {
            return LoadData<T>(out int _);
        }

        private T LoadData<T>(out int size) where T : SaveDataObject, new()
        {
            size = WorkBuff.ReadInt32();
            int bytesRead = Serializer.Read(WorkBuff, FileFormat, out T obj);

            Debug.Assert(size == bytesRead);
            return obj;
        }

        private DummyObject LoadDummy()
        {
            return LoadDummy(out int _);
        }

        private DummyObject LoadDummy(out int size)
        {
            size = WorkBuff.ReadInt32();
            DummyObject obj = new DummyObject(size);
            ((ISaveDataObject) obj).ReadObjectData(WorkBuff);

            return obj;
        }

        private void SaveData(SaveDataObject o)
        {
            int size;
            int preSize, postData;
                
            preSize = WorkBuff.Position;
            WorkBuff.Skip(4);
            
            size = Serializer.Write(WorkBuff, o, FileFormat);
            postData = WorkBuff.Position;

            WorkBuff.Seek(preSize);
            WorkBuff.Write(size);
            WorkBuff.Seek(postData);
            WorkBuff.Align4Bytes();
        }

        private void LoadSimpleVars()
        {
            SimpleVars = WorkBuff.ReadObject<SimpleVariables>(FileFormat);
        }

        private void SaveSimpleVars()
        {
            SimpleVars.SaveSize = SizeOfOneGameInBytes;
            WorkBuff.Write(SimpleVars, FileFormat);
        }

        private int ReadBlock(DataBuffer file)
        {
            file.MarkPosition();
            WorkBuff.Reset();

            int size = file.ReadInt32();
            if (size > BufferSize)
            {
                // TODO: BlockSizeChecks flag?
                Debug.WriteLine("Maximum block size exceeded: {0}", size);
            }

            WorkBuff.Write(file.ReadBytes(size));

            Debug.Assert(file.Offset == size + 4);
            Debug.WriteLine("Read {0} bytes of block data.", size);

            WorkBuff.Reset();
            return size;
        }

        private int WriteBlock(DataBuffer file)
        {
            file.MarkPosition();

            byte[] data = WorkBuff.GetBytesUpToCursor();
            int size = data.Length;
            if (size > BufferSize)
            {
                // TODO: BlockSizeChecks flag?
                Debug.WriteLine("Maximum block size exceeded: {0}", size);
            }

            file.Write(size);
            file.Write(data);
            file.Align4Bytes();

            Debug.Assert(file.Offset == size + 4);
            Debug.WriteLine("Wrote {0} bytes of block data.", size);

            CheckSum += (uint) BitConverter.GetBytes(size).Sum(x => x);
            CheckSum += (uint) data.Sum(x => x);

            WorkBuff.Reset();
            return size;
        }

        protected override void LoadAllData(DataBuffer file)
        {
            int totalSize = 0;

            totalSize += ReadBlock(file); LoadSimpleVars(); Scripts = LoadData<TheScripts>();
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
            totalSize += LoadUserDefinedBlocks(file);

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

            WorkBuff.Reset();
            CheckSum = 0;

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
            totalSize += SaveUserDefinedBlocks(file);

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
                        WorkBuff.Reset();
                        WorkBuff.Write(GenerateSpecialPadding(size));
                    }
                    WorkBuff.Seek(size);
                    totalSize += WriteBlock(file);
                }
            }

            file.Write(CheckSum);

            Debug.WriteLine("Save size: {0}", totalSize);
            Debug.Assert(totalSize == (SizeOfOneGameInBytes & 0xFFFFFFFE));
        }

        private int LoadUserDefinedBlocks(DataBuffer buf)
        {
            int size = 0;
            foreach (var block in UserDefinedBlocks)
            {
                size += ReadBlock(buf);
                ((ISaveDataObject) block).ReadObjectData(WorkBuff, FileFormat);
            }

            return size;
        }

        private int SaveUserDefinedBlocks(DataBuffer buf)
        {
            int size = 0;
            foreach (var block in UserDefinedBlocks)
            {
                ((ISaveDataObject) block).WriteObjectData(WorkBuff, FileFormat);
                size += WriteBlock(buf);
            }

            return size;
        }

        protected override bool DetectFileFormat(byte[] data, out SaveFileFormat fmt)
        {
            bool isMobile = false;
            bool isPcOrXbox = false;

            int fileId = data.FindFirst(BitConverter.GetBytes(0x31401));
            int fileIdJP = data.FindFirst(BitConverter.GetBytes(0x31400));
            int scr = data.FindFirst("SCR\0".GetAsciiBytes());

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

        protected override List<SaveDataObject> GetBlocks()
        {
            return new List<SaveDataObject>()
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

        public static class FileFormats
        {
            public static readonly SaveFileFormat Android = new SaveFileFormat(
                "Android", "Android",
                new GameConsole(ConsoleType.Android)
            );

            public static readonly SaveFileFormat iOS = new SaveFileFormat(
                "iOS", "iOS",
                new GameConsole(ConsoleType.iOS)
            );

            public static readonly SaveFileFormat PC = new SaveFileFormat(
                "PC", "PC (Windows/macOS)",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.MacOS),
                new GameConsole(ConsoleType.Win32, ConsoleFlags.Steam),
                new GameConsole(ConsoleType.MacOS, ConsoleFlags.Steam)
            );

            public static readonly SaveFileFormat PS2_AU = new SaveFileFormat(
                "PS2_AU", "PS2 (PAL, Australia)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.Australia)
            );

            public static readonly SaveFileFormat PS2_JP = new SaveFileFormat(
                "PS2_JP", "PS2 (NTSC-J)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.Japan)
            );

            public static readonly SaveFileFormat PS2_NAEU = new SaveFileFormat(
                "PS2_NAEU", "PS2 (NTSC-U/C & PAL)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.NorthAmerica | ConsoleFlags.Europe)
            );

            public static readonly SaveFileFormat Xbox = new SaveFileFormat(
                "Xbox", "Xbox",
                new GameConsole(ConsoleType.Xbox)
            );

            public static SaveFileFormat[] GetAll()
            {
                return new SaveFileFormat[] { Android, iOS, PC, PS2_AU, PS2_JP, PS2_NAEU, Xbox };
            }
        }
    }
}
