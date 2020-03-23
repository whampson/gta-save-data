using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.VC
{
    /// <summary>
    /// Represents a save file for <i>Grand Theft Auto: Vice City</i>.
    /// </summary>
    public class ViceCitySave : SaveFile, IGTASaveFile, IEquatable<ViceCitySave>
    {
        public static class Limits
        {
            public const int MaxNameLength = 24;
            public const int RadioStationListCount = 10;
        }

        public const int SaveHeaderSize = 8;
        public const int SizeOfOneGameInBytes = 201729;
        public const int BufferSize = 0x10000;
        public const int NumberOfBlocks = 23;

        private string m_name;
        private SystemTime m_timeLastSaved;
        private int m_saveSize;
        private int m_steamOnlyValue;
        private Game m_game;
        private Camera m_theCamera;
        private Clock m_clock;
        private Pad m_pad;
        private Timer m_timer;
        private TimeStep m_timeStep;
        private Weather m_weather;
        private TimeCycle m_timeCycle;
        private Array<int> m_radioStationPositionList;
        private DummyObject m_scripts;
        private DummyObject m_pedPool;
        private DummyObject m_garages;
        private DummyObject m_gameLogic;
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
        private DummyObject m_scriptPaths;
        private DummyObject m_playerInfo;
        private DummyObject m_stats;
        private DummyObject m_setPieces;
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

        public int SteamOnlyValue
        {
            get { return m_steamOnlyValue; }
            set { m_steamOnlyValue = value; OnPropertyChanged(); }
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

        public TimeCycle TimeCycle
        {
            get { return m_timeCycle; }
            set { m_timeCycle = value; OnPropertyChanged(); }
        }

        public Array<int> RadioStationPositionList
        {
            get { return m_radioStationPositionList; }
            set { m_radioStationPositionList = value; OnPropertyChanged(); }
        }

        public DummyObject Scripts
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

        public DummyObject GameLogic
        {
            get { return m_gameLogic; }
            set { m_gameLogic = value; OnPropertyChanged(); }
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

        public DummyObject ScriptPaths
        {
            get { return m_scriptPaths; }
            set { m_scriptPaths = value; OnPropertyChanged(); }
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

        public DummyObject SetPieces
        {
            get { return m_setPieces; }
            set { m_setPieces = value; OnPropertyChanged(); }
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

        public override IReadOnlyList<SaveDataObject> Blocks => new SaveDataObject[]
        {
            Scripts,
            PedPool,
            GameLogic,
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
            ScriptPaths,
            PlayerInfo,
            Stats,
            SetPieces,
            Streaming,
            PedTypeInfo
        };

        public ViceCitySave()
        {
            WorkBuff = new WorkBuffer(new byte[BufferSize]);
            Name = string.Empty;
            TimeLastSaved = DateTime.Now;
            Game = new Game();
            TheCamera = new Camera();
            Clock = new Clock();
            Pad = new Pad();
            Timer = new Timer();
            TimeStep = new TimeStep();
            Weather = new Weather();
            TimeCycle = new TimeCycle();
            RadioStationPositionList = CreateArray<int>(Limits.RadioStationListCount);
            Scripts = new DummyObject();
            PedPool = new DummyObject();
            Garages = new DummyObject();
            GameLogic = new DummyObject();
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
            ScriptPaths = new DummyObject();
            PlayerInfo = new DummyObject();
            Stats = new DummyObject();
            SetPieces = new DummyObject();
            Streaming = new DummyObject();
            PedTypeInfo = new DummyObject();
        }

        private bool IsSteam
        {
            get { return FileFormat.IsSupportedOn(ConsoleType.Win32, ConsoleFlags.Steam); }
        }

        public int GetSizeOfSimpleVars()
        {
            // TODO: other platforms
            return (IsSteam) ? 232 : 228;
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

        protected override int ReadBlock(WorkBuffer file)
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

        protected override int WriteBlock(WorkBuffer file)
        {
            file.MarkPosition();

            byte[] data = WorkBuff.ToArray(WorkBuff.Position);
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

        protected override void LoadAllData(WorkBuffer file)
        {
            int totalSize = 0;
            int size;

            size = ReadBlock(file);
            Name = WorkBuff.ReadString(Limits.MaxNameLength, true);
            TimeLastSaved = WorkBuff.ReadObject<SystemTime>().ToDateTime();
            SaveSize = WorkBuff.ReadInt32();
            Game.CurrLevel = (LevelType) WorkBuff.ReadInt32();
            TheCamera.Position = WorkBuff.ReadObject<Vector>();
            if (IsSteam) SteamOnlyValue = WorkBuff.ReadInt32();
            Clock.MillisecondsPerGameMinute = WorkBuff.ReadInt32();
            Clock.LastClockTick = WorkBuff.ReadUInt32();
            Clock.GameClockHours = (byte) WorkBuff.ReadInt32();
            WorkBuff.Align4Bytes();
            Clock.GameClockMinutes = (byte) WorkBuff.ReadInt32();
            WorkBuff.Align4Bytes();
            Pad.Mode = WorkBuff.ReadInt16();
            WorkBuff.Align4Bytes();
            Timer.TimeInMilliseconds = WorkBuff.ReadUInt32();
            Timer.TimeScale = WorkBuff.ReadSingle();
            Timer.TimeStep = WorkBuff.ReadSingle();
            Timer.TimeStepNonClipped = WorkBuff.ReadSingle();
            Timer.FrameCounter = WorkBuff.ReadUInt32();
            TimeStep.TimeStepValue = WorkBuff.ReadSingle();
            TimeStep.FramesPerUpdate = WorkBuff.ReadSingle();
            TimeStep.TimeScale = WorkBuff.ReadSingle();
            Weather.OldWeatherType = (WeatherType) WorkBuff.ReadInt16();
            WorkBuff.Align4Bytes();
            Weather.NewWeatherType = (WeatherType) WorkBuff.ReadInt16();
            WorkBuff.Align4Bytes();
            Weather.ForcedWeatherType = (WeatherType) WorkBuff.ReadInt16();
            WorkBuff.Align4Bytes();
            Weather.InterpolationValue = WorkBuff.ReadSingle();
            Weather.WeatherTypeInList = WorkBuff.ReadInt32();
            TheCamera.CarZoomIndicator = WorkBuff.ReadSingle();
            TheCamera.PedZoomIndicator = WorkBuff.ReadSingle();
            Game.CurrArea = (AreaType) WorkBuff.ReadInt32();
            Game.AllTaxisHaveNitro = WorkBuff.ReadBool();
            WorkBuff.Align4Bytes();
            Pad.InvertLook4Pad = WorkBuff.ReadBool();
            WorkBuff.Align4Bytes();
            TimeCycle.ExtraColour = WorkBuff.ReadInt32();
            TimeCycle.ExtraColourOn = WorkBuff.ReadBool(4);
            TimeCycle.ExtraColourInter = WorkBuff.ReadSingle();
            RadioStationPositionList = WorkBuff.ReadArray<int>(Limits.RadioStationListCount);
            Debug.Assert(WorkBuff.Offset == GetSizeOfSimpleVars());
            Scripts = LoadDummy(out int scriptsSize);
            Debug.Assert(WorkBuff.Offset - GetSizeOfSimpleVars() == scriptsSize + 4);
            Debug.Assert(WorkBuff.Offset == size);
            totalSize += size;
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

            // TODO: user-defined blocks

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

            WorkBuff.Reset();
            CheckSum = 0;

            WorkBuff.Write(Name, Limits.MaxNameLength, true);
            WorkBuff.Write(new SystemTime(TimeLastSaved));
            WorkBuff.Write(SizeOfOneGameInBytes);
            WorkBuff.Write((int) Game.CurrLevel);
            WorkBuff.Write(TheCamera.Position);
            if (IsSteam) WorkBuff.Write(SteamOnlyValue);
            WorkBuff.Write(Clock.MillisecondsPerGameMinute);
            WorkBuff.Write(Clock.LastClockTick);
            WorkBuff.Write(Clock.GameClockHours);
            WorkBuff.Align4Bytes();
            WorkBuff.Write(Clock.GameClockMinutes);
            WorkBuff.Align4Bytes();
            WorkBuff.Write(Pad.Mode);
            WorkBuff.Align4Bytes();
            WorkBuff.Write(Timer.TimeInMilliseconds);
            WorkBuff.Write(Timer.TimeScale);
            WorkBuff.Write(Timer.TimeStep);
            WorkBuff.Write(Timer.TimeStepNonClipped);
            WorkBuff.Write(Timer.FrameCounter);
            WorkBuff.Write(TimeStep.TimeStepValue);
            WorkBuff.Write(TimeStep.FramesPerUpdate);
            WorkBuff.Write(TimeStep.TimeScale);
            WorkBuff.Write((short) Weather.OldWeatherType);
            WorkBuff.Align4Bytes();
            WorkBuff.Write((short) Weather.NewWeatherType);
            WorkBuff.Align4Bytes();
            WorkBuff.Write((short) Weather.ForcedWeatherType);
            WorkBuff.Align4Bytes();
            WorkBuff.Write(Weather.InterpolationValue);
            WorkBuff.Write(Weather.WeatherTypeInList);
            WorkBuff.Write(TheCamera.CarZoomIndicator);
            WorkBuff.Write(TheCamera.PedZoomIndicator);
            WorkBuff.Write((int) Game.CurrArea);
            WorkBuff.Write(Game.AllTaxisHaveNitro);
            WorkBuff.Align4Bytes();
            WorkBuff.Write(Pad.InvertLook4Pad);
            WorkBuff.Align4Bytes();
            WorkBuff.Write(TimeCycle.ExtraColour);
            WorkBuff.Write(TimeCycle.ExtraColourOn, 4);
            WorkBuff.Write(TimeCycle.ExtraColourInter);
            WorkBuff.Write(RadioStationPositionList.ToArray(), Limits.RadioStationListCount);
            Debug.Assert(WorkBuff.Offset == GetSizeOfSimpleVars());
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

            // TODO: user-defined blocks            

            // Padding
            for (int i = 0; i < 4; i++)
            {
                size = WorkBuffer.Align4Bytes(SizeOfOneGameInBytes - totalSize - 4);
                if (size > BufferSize)
                {
                    size = BufferSize;
                }
                if (size > 4)
                {
                    WorkBuff.Reset();
                    WorkBuff.Write(GetPaddingBytes(size));
                    totalSize += WriteBlock(file);
                }
            }

            file.Write(CheckSum);

            Debug.WriteLine("Save size: {0}", totalSize);
            Debug.Assert(totalSize == (SizeOfOneGameInBytes & 0xFFFFFFFE));
        }

        protected override bool DetectFileFormat(byte[] data, out SaveFileFormat fmt)
        {
            // TODO: implement

            fmt = FileFormats.PC_Retail;
            return true;
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

            return Name.Equals(other.Name)
                && TimeLastSaved.Equals(other.TimeLastSaved)
                && SaveSize.Equals(other.SaveSize)
                && SteamOnlyValue.Equals(other.SteamOnlyValue)
                && Game.Equals(other.Game)
                && TheCamera.Equals(other.TheCamera)
                && Clock.Equals(other.Clock)
                && Pad.Equals(other.Pad)
                && Timer.Equals(other.Timer)
                && TimeStep.Equals(other.TimeStep)
                && Weather.Equals(other.Weather)
                && TimeCycle.Equals(other.TimeCycle)
                && RadioStationPositionList.SequenceEqual(other.RadioStationPositionList)
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
            public static readonly SaveFileFormat PC_Retail = new SaveFileFormat(
                "PC_Retail", "PC (Windows/macOS)",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.MacOS, ConsoleFlags.Steam)
            );

            public static readonly SaveFileFormat PC_Steam = new SaveFileFormat(
                "PC_Steam", "PC (Windows, Steam)",
                new GameConsole(ConsoleType.Win32, ConsoleFlags.Steam)
            );

            public static SaveFileFormat[] GetAll()
            {
                return new SaveFileFormat[] { PC_Retail, PC_Steam };
            }
        }
    }
}
