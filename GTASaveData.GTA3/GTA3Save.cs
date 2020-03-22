using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Represents a save file for <i>Grand Theft Auto III</i>.
    /// </summary>
    public class GTA3Save : SaveFile,
        IGTASaveFile, IEquatable<GTA3Save>
    {
        public static class Limits
        {
            public const int MaxNameLength = 24;
        }

        public const int SaveHeaderSize = 8;
        public const int SizeOfOneGameInBytes = 201729;
        public const int SizeOfSimpleVars = 188;
        public const int BufferSize = 55000;

        private string m_name;
        private SystemTime m_timeLastSaved;
        private int m_saveSize;
        private Game m_game;
        private Camera m_theCamera;
        private Clock m_clock;
        private Pad m_pad;
        private Timer m_timer;
        private TimeStep m_timeStep;
        private Weather m_weather;
        private Date m_compileDateAndTime;
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

        public override string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public override DateTime TimeLastSaved
        {
            get { return m_timeLastSaved.ToDateTime(); }
            set { m_timeLastSaved = new SystemTime(value); OnPropertyChanged(); }
        }

        public int SaveSize
        {
            get { return m_saveSize; }
            set { m_saveSize = value; OnPropertyChanged(); }
        }

        public Game Game
        {
            get { return m_game; }
            set { m_game = value; OnPropertyChanged(); }
        }

        public Camera TheCamera
        {
            get { return m_theCamera; }
            set { m_theCamera = value; OnPropertyChanged(); }
        }

        public Clock Clock
        {
            get { return m_clock; }
            set { m_clock = value; OnPropertyChanged(); }
        }

        public Pad Pad
        {
            get { return m_pad; }
            set { m_pad = value; OnPropertyChanged(); }
        }

        public Timer Timer
        {
            get { return m_timer; }
            set { m_timer = value; OnPropertyChanged(); }
        }

        public TimeStep TimeStep
        {
            get { return m_timeStep; }
            set { m_timeStep = value; OnPropertyChanged(); }
        }

        public Weather Weather
        {
            get { return m_weather; }
            set { m_weather = value; OnPropertyChanged(); }
        }

        public Date CompileDateAndTime
        {
            get { return m_compileDateAndTime; }
            set { m_compileDateAndTime = value; OnPropertyChanged(); }
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


        public GTA3Save()
        {
            m_workBuf = new WorkBuffer(new byte[BufferSize]);
            m_timeLastSaved = new SystemTime();
            m_game = new Game();
            m_theCamera = new Camera();
            m_clock = new Clock();
            m_pad = new Pad();
            m_timer = new Timer();
            m_timeStep = new TimeStep();
            m_weather = new Weather();
            m_compileDateAndTime = new Date();
            m_scripts = new TheScripts();
        }

        public static int ReadSaveHeader(WorkBuffer buf, string tag)
        {
            string readTag = buf.ReadString(4);
            int size = buf.ReadInt32();

            Debug.Assert(tag == readTag, "Invalid block tag (expected: {0}, actual: {1})", tag, readTag);
            return size;
        }

        public static void WriteSaveHeader(WorkBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4);
            buf.Write(size);
        }

        private int LoadData<T>(out T o)
            where T : SaveDataObject, new()
        {
            int size = m_workBuf.ReadInt32();
            Serializer.Read(m_workBuf, FileFormat, out o);

            return size;
        }

        private int LoadDummy(out DummyObject o)
        {
            int size = m_workBuf.ReadInt32();
            o = new DummyObject(size);
            ((ISaveDataObject) o).ReadObjectData(m_workBuf);

            return size;
        }

        private int SaveData(SaveDataObject o)
        {
            int size;
            int preSize, postData;
                
            preSize = m_workBuf.Position;
            m_workBuf.Skip(4);
            
            size = Serializer.Write(m_workBuf, o, FileFormat);
            postData = m_workBuf.Position;

            m_workBuf.Seek(preSize);
            m_workBuf.Write(size);
            m_workBuf.Seek(postData);
            m_workBuf.Align4Bytes();

            size = WorkBuffer.Align4Bytes(size);
            return size;
        }

        protected override int ReadBlock(WorkBuffer file)
        {
            file.MarkPosition();
            m_workBuf.Reset();

            int size = file.ReadInt32();
            if (size > BufferSize)
            {
                // TODO: BlockSizeChecks flag?
                Debug.WriteLine("Maximum block size exceeded: {0}", size);
            }

            m_workBuf.Write(file.ReadBytes(size));

            Debug.Assert(file.Offset == size + 4);
            Debug.WriteLine("Read {0} bytes of block data.", size);

            m_workBuf.Reset();
            return size;
        }

        protected override int WriteBlock(WorkBuffer file)
        {
            file.MarkPosition();

            byte[] data = m_workBuf.ToArray(m_workBuf.Position);
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

            m_checksum += BitConverter.GetBytes(size).Sum(x => x);
            m_checksum += data.Sum(x => x);

            m_workBuf.Reset();
            return size;
        }

        protected override void LoadAllData(WorkBuffer file)
        {
            int totalSize = 0;
            int size;

            // Read simplevars and scripts
            size = ReadBlock(file);
            Name = m_workBuf.ReadString(Limits.MaxNameLength, true);
            TimeLastSaved = m_workBuf.ReadObject<SystemTime>().ToDateTime();
            SaveSize = m_workBuf.ReadInt32();
            Game.CurrLevel = (LevelType) m_workBuf.ReadInt32();
            TheCamera.Position = m_workBuf.ReadObject<Vector>();
            Clock.MillisecondsPerGameMinute = m_workBuf.ReadInt32();
            Clock.LastClockTick = m_workBuf.ReadUInt32();
            Clock.GameClockHours = (byte) m_workBuf.ReadInt32();
            Clock.GameClockMinutes = (byte) m_workBuf.ReadInt32();
            Pad.Mode = m_workBuf.ReadInt32();
            Timer.TimeInMilliseconds = m_workBuf.ReadUInt32();
            Timer.TimeScale = m_workBuf.ReadSingle();
            Timer.TimeStep = m_workBuf.ReadSingle();
            Timer.TimeStepNonClipped = m_workBuf.ReadSingle();
            Timer.FrameCounter = m_workBuf.ReadInt32();
            TimeStep.Step = m_workBuf.ReadSingle();
            TimeStep.FramesPerUpdate = m_workBuf.ReadSingle();
            TimeStep.TimeScale = m_workBuf.ReadSingle();
            Weather.OldWeatherType = (WeatherType) m_workBuf.ReadInt32();
            Weather.NewWeatherType = (WeatherType) m_workBuf.ReadInt32();
            Weather.ForcedWeatherType = (WeatherType) m_workBuf.ReadInt32();
            Weather.InterpolationValue = m_workBuf.ReadSingle();
            CompileDateAndTime = m_workBuf.ReadObject<Date>();
            Weather.WeatherTypeInList = m_workBuf.ReadInt32();
            TheCamera.CarZoomIndicator = m_workBuf.ReadSingle();
            TheCamera.PedZoomIndicator = m_workBuf.ReadSingle();
            Debug.Assert(m_workBuf.Offset == SizeOfSimpleVars);
            int scriptsSize = LoadData(out m_scripts);
            Debug.Assert(m_workBuf.Offset - SizeOfSimpleVars == scriptsSize + 4);
            Debug.Assert(m_workBuf.Offset == size);
            totalSize += size;

            // Read the rest
            totalSize += ReadBlock(file);
            LoadDummy(out m_pedPool);
            totalSize += ReadBlock(file);
            LoadDummy(out m_garages);
            totalSize += ReadBlock(file);
            LoadDummy(out m_vehiclePool);
            totalSize += ReadBlock(file);
            LoadDummy(out m_objectPool);
            totalSize += ReadBlock(file);
            LoadDummy(out m_paths);
            totalSize += ReadBlock(file);
            LoadDummy(out m_cranes);
            totalSize += ReadBlock(file);
            LoadDummy(out m_pickups);
            totalSize += ReadBlock(file);
            LoadDummy(out m_phoneInfo);
            totalSize += ReadBlock(file);
            LoadDummy(out m_restartPoints);
            totalSize += ReadBlock(file);
            LoadDummy(out m_radarBlips);
            totalSize += ReadBlock(file);
            LoadDummy(out m_zones);
            totalSize += ReadBlock(file);
            LoadDummy(out m_gangData);
            totalSize += ReadBlock(file);
            LoadDummy(out m_carGenerators);
            totalSize += ReadBlock(file);
            LoadDummy(out m_particleObjects);
            totalSize += ReadBlock(file);
            LoadDummy(out m_audioScriptObjects);
            totalSize += ReadBlock(file);
            LoadDummy(out m_playerInfo);
            totalSize += ReadBlock(file);
            LoadDummy(out m_stats);
            totalSize += ReadBlock(file);
            LoadDummy(out m_streaming);
            totalSize += ReadBlock(file);
            LoadDummy(out m_pedTypeInfo);

            // TODO: "meta blocks"

            // Read-out remaining bytes
            while (file.Position < file.Length - 4)
            {
                totalSize += ReadBlock(file);
            }

            Debug.WriteLine("Save size: {0}", totalSize);
            Debug.Assert(totalSize == (SizeOfOneGameInBytes & 0xFFFFFFFE));
        }

        protected override void SaveAllData(WorkBuffer file)
        {
            int totalSize = 0;
            int size;

            m_workBuf.Reset();
            m_checksum = 0;

            // Write simplevars and scripts
            m_workBuf.Write(Name, Limits.MaxNameLength, true);
            m_workBuf.Write(new SystemTime(TimeLastSaved));
            m_workBuf.Write(SizeOfOneGameInBytes);
            m_workBuf.Write((int) Game.CurrLevel);
            m_workBuf.Write(TheCamera.Position);
            m_workBuf.Write(Clock.MillisecondsPerGameMinute);
            m_workBuf.Write(Clock.LastClockTick);
            m_workBuf.Write(Clock.GameClockHours);
            m_workBuf.Write(Clock.GameClockMinutes);
            m_workBuf.Write(Pad.Mode);
            m_workBuf.Write(Timer.TimeInMilliseconds);
            m_workBuf.Write(Timer.TimeScale);
            m_workBuf.Write(Timer.TimeStep);
            m_workBuf.Write(Timer.TimeStepNonClipped);
            m_workBuf.Write(Timer.FrameCounter);
            m_workBuf.Write(TimeStep.Step);
            m_workBuf.Write(TimeStep.FramesPerUpdate);
            m_workBuf.Write(TimeStep.TimeScale);
            m_workBuf.Write((int) Weather.OldWeatherType);
            m_workBuf.Write((int) Weather.NewWeatherType);
            m_workBuf.Write((int) Weather.ForcedWeatherType);
            m_workBuf.Write(Weather.InterpolationValue);
            m_workBuf.Write(CompileDateAndTime);
            m_workBuf.Write(Weather.WeatherTypeInList);
            m_workBuf.Write(TheCamera.CarZoomIndicator);
            m_workBuf.Write(TheCamera.PedZoomIndicator);
            Debug.Assert(m_workBuf.Offset == SizeOfSimpleVars);
            SaveData(Scripts);
            totalSize += WriteBlock(file);

            // Write the rest
            SaveData(PedPool);
            totalSize += WriteBlock(file);
            SaveData(Garages);
            totalSize += WriteBlock(file);
            SaveData(VehiclePool);
            totalSize += WriteBlock(file);
            SaveData(ObjectPool);
            totalSize += WriteBlock(file);
            SaveData(Paths);
            totalSize += WriteBlock(file);
            SaveData(Cranes);
            totalSize += WriteBlock(file);
            SaveData(Pickups);
            totalSize += WriteBlock(file);
            SaveData(PhoneInfo);
            totalSize += WriteBlock(file);
            SaveData(RestartPoints);
            totalSize += WriteBlock(file);
            SaveData(RadarBlips);
            totalSize += WriteBlock(file);
            SaveData(Zones);
            totalSize += WriteBlock(file);
            SaveData(GangData);
            totalSize += WriteBlock(file);
            SaveData(CarGenerators);
            totalSize += WriteBlock(file);
            SaveData(ParticleObjects);
            totalSize += WriteBlock(file);
            SaveData(AudioScriptObjects);
            totalSize += WriteBlock(file);
            SaveData(PlayerInfo);
            totalSize += WriteBlock(file);
            SaveData(Stats);
            totalSize += WriteBlock(file);
            SaveData(Streaming);
            totalSize += WriteBlock(file);
            SaveData(PedTypeInfo);
            totalSize += WriteBlock(file);

            // TODO: "meta blocks", extra user-defined blocks

            // Write padding
            for (int i = 0; i < 4; i++)
            {
                size = WorkBuffer.Align4Bytes(SizeOfOneGameInBytes - totalSize - 4);
                if (size > BufferSize)
                {
                    size = BufferSize;
                }
                if (size > 4)
                {
                    m_workBuf.Reset();
                    m_workBuf.Write(GetPaddingBytes(size));
                    totalSize += WriteBlock(file);
                }
            }

            // Write checksum
            file.Write(m_checksum);

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

            int blk1Size;
            using (WorkBuffer wb = new WorkBuffer(data))
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

            // TODO
            return false;
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
