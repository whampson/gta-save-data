using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Represents a save file for <i>Grand Theft Auto III</i>.
    /// </summary>
    public class GTA3Save : SaveFile,
        IGTASave, IEquatable<GTA3Save>
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
        private int m_isQuickSave;              // TODO: enum?

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

        public int IsQuickSave
        {
            get { return m_isQuickSave; }
            set { m_isQuickSave = value; OnPropertyChanged(); }
        }

        // PS2 stuff
        //private int m_prefsMusicVolume;
        //private int m_prefsSfxVolume;
        //private bool m_prefsUseVibration;
        //private bool m_prefsStereoMono;
        //private RadioStation m_prefsRadioStation;
        //private int m_prefsBrightness;
        //private bool m_prefsUseWideScreen;
        //private bool m_prefsShowTrails;
        //private bool m_prefsShowSubtitles;
        //private Language m_prefsLanguage;

        //public int PrefsMusicVolume
        //{
        //    get { return m_prefsMusicVolume; }
        //    set { m_prefsMusicVolume = value; OnPropertyChanged(); }
        //}

        //public int PrefsSfxVolume
        //{
        //    get { return m_prefsSfxVolume; }
        //    set { m_prefsSfxVolume = value; OnPropertyChanged(); }
        //}

        //public bool PrefsUseVibration
        //{
        //    get { return m_prefsUseVibration; }
        //    set { m_prefsUseVibration = value; OnPropertyChanged(); }
        //}

        //public bool PrefsStereoMono
        //{
        //    get { return m_prefsStereoMono; }
        //    set { m_prefsStereoMono = value; OnPropertyChanged(); }
        //}

        //public RadioStation PrefsRadioStation
        //{
        //    get { return m_prefsRadioStation; }
        //    set { m_prefsRadioStation = value; OnPropertyChanged(); }
        //}

        //public int PrefsBrightness
        //{
        //    get { return m_prefsBrightness; }
        //    set { m_prefsBrightness = value; OnPropertyChanged(); }
        //}

        //public bool PrefsUseWideScreen
        //{
        //    get { return m_prefsUseWideScreen; }
        //    set { m_prefsUseWideScreen = value; OnPropertyChanged(); }
        //}

        //public bool PrefsShowTrails
        //{
        //    get { return m_prefsShowTrails; }
        //    set { m_prefsShowTrails = value; OnPropertyChanged(); }
        //}

        //public bool PrefsShowSubtitles
        //{
        //    get { return m_prefsShowSubtitles; }
        //    set { m_prefsShowSubtitles = value; OnPropertyChanged(); }
        //}

        //public Language PrefsLanguage
        //{
        //    get { return m_prefsLanguage; }
        //    set { m_prefsLanguage = value; OnPropertyChanged(); }
        //}



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

            Debug.Assert(tag == readTag);
            return size;
        }

        public static void WriteSaveHeader(WorkBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4);
            buf.Write(size);
        }

        private int Save(WorkBuffer buf, GTAObject o, SaveFileFormat fmt)
        {
            int size;
            int preSize, postData;
                
            preSize = buf.Position;
            buf.Skip(4);
                
            size = ((IGTAObject) o).WriteObjectData(buf, fmt);
            if (size > BufferSize)
            {
                throw new SerializationException(string.Format("Exceeded maximum block size of {0}.", BufferSize));
            }

            postData = buf.Position;

            buf.Seek(preSize);
            buf.Write(size);
            buf.Seek(postData);
            buf.Align4Bytes();

            size = WorkBuffer.Align4Bytes(size);
            Debug.Assert(size == buf.Offset);

            return size;
        }

        protected override int WriteBlock(WorkBuffer buf)
        {
            byte[] data = m_workBuf.ToArray(m_workBuf.Position);

            buf.Write(data.Length);
            buf.Write(data);
            buf.Align4Bytes();

            m_checksum += BitConverter.GetBytes(data.Length).Sum(x => x);
            m_checksum += data.Sum(x => x);

            m_workBuf.Seek(0);

            return buf.Offset;
        }

        protected override void LoadAllData(WorkBuffer buf)
        {
            //throw new NotImplementedException();
        }

        protected override void SaveAllData(WorkBuffer buf)
        {
            int totalSize = 0;
            int actualSize = 0;

            m_workBuf.Seek(0);
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
            m_workBuf.Write((short) Weather.OldWeatherType);
            m_workBuf.Write((short) Weather.NewWeatherType);
            m_workBuf.Write((short) Weather.ForcedWeatherType);
            m_workBuf.Write(Weather.InterpolationValue);
            m_workBuf.Write(CompileDateAndTime);
            m_workBuf.Write(Weather.WeatherTypeInList);
            m_workBuf.Write(TheCamera.CarZoomIndicator);
            m_workBuf.Write(TheCamera.PedZoomIndicator);
            Debug.Assert(m_workBuf.Offset == SizeOfSimpleVars);
            totalSize = Save(m_workBuf, m_theScripts, FileFormat);
            totalSize += SizeOfSimpleVars;
            actualSize += WriteBlock(buf);



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
