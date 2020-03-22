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
        private TheScripts m_theScripts;

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

        public TheScripts TheScripts
        {
            get { return m_theScripts; }
            set { m_theScripts = value; OnPropertyChanged(); }
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
            m_theScripts = new TheScripts();
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

        private int LoadData<T>(WorkBuffer buf, out T o)
            where T : SaveDataObject, new()
        {
            int size = buf.ReadInt32();
            Serializer.Read(buf, FileFormat, out o);

            return size;
        }

        private int SaveData(WorkBuffer buf, SaveDataObject o)
        {
            int size;
            int preSize, postData;
                
            preSize = buf.Position;
            buf.Skip(4);
            
            size = Serializer.Write(buf, o, FileFormat);
            postData = buf.Position;

            buf.Seek(preSize);
            buf.Write(size);
            buf.Seek(postData);
            buf.Align4Bytes();

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

            m_checksum = 0;

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
            int scriptsSize = LoadData(m_workBuf, out m_theScripts);
            Debug.Assert(m_workBuf.Offset - SizeOfSimpleVars == scriptsSize + 4);
            Debug.Assert(m_workBuf.Offset == size);
            totalSize += size;

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
            SaveData(m_workBuf, m_theScripts);
            totalSize += WriteBlock(file);

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
