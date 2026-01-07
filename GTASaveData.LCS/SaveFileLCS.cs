using GTASaveData.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData.LCS
{
    public class LCSSaveParams : SerializationParams
    {
        public FileType FileType { get; set; }

        public static LCSSaveParams GetDefaults(FileType t)
        {
            bool isValid = LCSSave.FileTypes.GetAll().Select(x => x == t).Any();
            if (!isValid)
                return null;

            LCSSaveParams p = new LCSSaveParams() {
                FileType = t,
                PaddingType = PaddingScheme.Skip,
                BigEndian = false,
            };
            return p;
        }
    }

    public enum DataBlock
    {
        SimpleVars,
        Scripts,
        Garages,
        PlayerInfo,
        Stats
    }

    public class LCSSave : SaveFile<LCSSaveParams>,
        IEquatable<LCSSave>, IDeepClonable<LCSSave>
    {
        private const int SizeOfGameInBytes = 0x19000;

        private int m_checkSum;

        private string m_name;
        private SimpleVariables m_simpleVars;
        private ScriptData m_scripts;
        private GarageData m_garages;
        private PlayerInfo m_playerInfo;
        private Stats m_stats;

        public FileType FileType
        {
            get { return Params.FileType; }
            set { Params.FileType = value; OnPropertyChanged(); }
        }

        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public ScriptData Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; OnPropertyChanged(); }
        }

        public GarageData Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public PlayerInfo PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; OnPropertyChanged(); }
        }

        public Stats Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public DateTime TimeStamp
        {
            get { return SimpleVars.TimeStamp; }
            set { SimpleVars.TimeStamp = value; OnPropertyChanged(); }
        }

        public static LCSSave LoadFromFile(string path, FileType t)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (t == FileType.Default)
            {
                if (!TryGetFileType<LCSSave>(path, out t))
                {
                    throw new InvalidDataException("Unable to detect file type!");
                }
            }

            LCSSaveParams p = LCSSaveParams.GetDefaults(t)
                ?? throw new InvalidDataException($"Invalid file type! ({t})");

            return LoadFromFile<LCSSave>(path, p, out _);
        }

        public static bool TryLoadFromFile(string path, FileType t, out LCSSave saveData)
        {
            if (t == FileType.Default)
            {
                if (!TryGetFileType<LCSSave>(path, out t))
                {
                    throw new InvalidDataException("Unable to detect file type!");
                }
            }

            LCSSaveParams p = LCSSaveParams.GetDefaults(t)
                ?? throw new InvalidDataException($"Invalid file type! ({t})");

            return TryLoadFromFile(path, p, out saveData);
        }

        public static bool TryGetFileType(string path, out FileType t)
        {
            return TryGetFileType<LCSSave>(path, out t);
        }

        public LCSSave()
            : base(Game.LCS)
        {
            SimpleVars = new SimpleVariables();
            Scripts = new ScriptData();
            Garages = new GarageData();
            PlayerInfo = new PlayerInfo();
            Stats = new Stats();
        }

        public LCSSave(LCSSave other)
            : base(Game.LCS)
        {
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Scripts = new ScriptData(other.Scripts);
            Garages = new GarageData(other.Garages);
            PlayerInfo = new PlayerInfo(other.PlayerInfo);
            Stats = new Stats(other.Stats);
        }

        private int ReadDataBlock<T>(DataBuffer file, string tag, out T obj)
            where T : SaveDataObject, new()
        {
            int oldMark = file.Mark();

            string savedTag = file.ReadString(4);
            Debug.Assert(savedTag == tag);

            int size = file.ReadInt32();
            Debug.Assert(file.Position + size < file.Length);

            obj = file.ReadObject<T>();

            file.Align4();
            int totalSize = file.Offset;

            file.MarkedPosition = oldMark;
            return totalSize;
        }

        private int WriteDataBlock<T>(DataBuffer file, string tag, T obj)
            where T : SaveDataObject
        {
            int size = Serializer.Write(obj, Params, out byte[] data);
            int sizeAligned = Align4(size);

            int oldMark = file.Mark();

            file.Write(tag, length: 4, zeroTerminate: false);
            file.Write(size);
            file.Write(data);

            file.Align4();
            int totalSize = file.Offset;

            Debug.Assert(totalSize == sizeAligned + 8);
            m_checkSum += file.GetBytesFromMark().Sum(x => x);

            file.MarkedPosition = oldMark;
            return totalSize;
        }

        protected override void Load(DataBuffer file)
        {
            int totalSize = 0;

            totalSize += Align4(ReadDataBlock(file, "SIMP", out SimpleVariables simp));
            SimpleVars = simp;
            totalSize += Align4(ReadDataBlock(file, "SRPT", out ScriptData srpt));
            Scripts = srpt;
            totalSize += Align4(ReadDataBlock(file, "GRGE", out GarageData grge));
            Garages = grge;
            totalSize += Align4(ReadDataBlock(file, "PLYR", out PlayerInfo plyr));
            PlayerInfo = plyr;
            totalSize += Align4(ReadDataBlock(file, "STAT", out Stats stat));
            Stats = stat;

            if (FileType.IsPS2)
            {
                file.Skip(SizeOfGameInBytes - totalSize);
            }
        }

        protected override void Save(DataBuffer file)
        {
            int totalSize = 0;
            m_checkSum = 0;

            totalSize += Align4(WriteDataBlock(file, "SIMP", SimpleVars));
            totalSize += Align4(WriteDataBlock(file, "SRPT", Scripts));
            totalSize += Align4(WriteDataBlock(file, "GRGE", Garages));
            totalSize += Align4(WriteDataBlock(file, "PLYR", PlayerInfo));
            totalSize += Align4(WriteDataBlock(file, "STAT", Stats));

            if (FileType.IsPS2)
            {
                int oldMark = file.Mark();

                totalSize += file.Pad(SizeOfGameInBytes - totalSize - 4);

                m_checkSum += file.GetBytesFromMark().Sum(x => x);
                totalSize += file.Write(m_checkSum);
                Debug.Assert(totalSize == SizeOfGameInBytes);

                file.MarkedPosition = oldMark;
            }
        }

        protected override bool DetectFileType(byte[] data, out FileType fmt)
        {
            const int SimpSizePS2 = 0xF8;
            const int SimpSizePSP = 0xBC;
            const int RunningScriptSizeAndroid = 0x21C;
            const int RunningScriptSizeiOS = 0x228;

            using (DataBuffer buf = new DataBuffer(data))
            {
                if (buf.Length < 0x1000) goto DetectionFailed;
                buf.Skip(4);

                int simpSize = buf.ReadInt32();
                int skip = simpSize + 4;
                if (skip < 0 || buf.Position + skip + 4 > buf.Length) goto DetectionFailed;
                buf.Skip(skip);

                int srptSize = buf.ReadInt32();
                int srptOffset = buf.Position;
                if (buf.Position + 12 > buf.Length) goto DetectionFailed;
                buf.Skip(8);

                int globalsSize = buf.ReadInt32();
                skip = globalsSize + 0x7C0;
                if (skip < 0 || buf.Position + skip + 4 > buf.Length) goto DetectionFailed;
                buf.Skip(skip);

                int numRunningScripts = buf.ReadInt32();
                if (numRunningScripts <= 0) goto DetectionFailed;
                int runningScriptSize = (srptOffset + srptSize - buf.Position) / numRunningScripts;

                if (simpSize == SimpSizePS2)
                {
                    fmt = FileTypes.PS2;
                    return true;
                }
                if (simpSize == SimpSizePSP)
                {
                    fmt = FileTypes.PSP;
                    return true;
                }
                if (runningScriptSize == RunningScriptSizeAndroid)
                {
                    fmt = FileTypes.Android;
                    return true;
                }
                if (runningScriptSize == RunningScriptSizeiOS)
                {
                    fmt = FileTypes.iOS;
                    return true;
                }
            }

        DetectionFailed:
            fmt = FileType.Default;
            return false;
        }

        protected override int GetSize(SerializationParams p)
        {
            if (p is LCSSaveParams param)
            {
                int size = 0;
                size += Align4(SizeOf(SimpleVars, param)) + 8;
                size += Align4(SizeOf(Scripts, param)) + 8;
                size += Align4(SizeOf(Garages, param)) + 8;
                size += Align4(SizeOf(PlayerInfo, param)) + 8;
                size += Align4(SizeOf(Stats, param)) + 8;

                if (param.FileType.IsPS2) size += (SizeOfGameInBytes - size);

                return size;
            }
            throw new InvalidDataException($"The type of SerializationParameters ({p.GetType().Name}) is invalid for this type ({GetType().Name}).");
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LCSSave);
        }

        public bool Equals(LCSSave other)
        {
            if (other == null)
            {
                return false;
            }

            return SimpleVars.Equals(other.SimpleVars)
                && Scripts.Equals(other.Scripts)
                && Garages.Equals(other.Garages)
                && PlayerInfo.Equals(other.PlayerInfo)
                && Stats.Equals(other.Stats);
        }

        public LCSSave DeepClone()
        {
            return new LCSSave(this);
        }

        public static class FileTypes
        {
            public static readonly FileType Android = new FileType(
                "Android", GameVersionId.Android
            );

            public static readonly FileType iOS = new FileType(
                "iOS", GameVersionId.iOS
            );

            public static readonly FileType PS2 = new FileType(
                "PS2", GameVersionId.PS2
            );

            public static readonly FileType PSP = new FileType(
                "PSP", GameVersionId.PSP
            );

            public static FileType[] GetAll()
            {
                return new FileType[] { Android, iOS, PS2, PSP };
            }

            public static FileType GetByGameConsole(GameVersionId console)
            {
                switch (console)
                {
                    case GameVersionId.Android: return Android;
                    case GameVersionId.iOS: return iOS;
                    case GameVersionId.PS2: return PS2;
                    case GameVersionId.PSP: return PSP;
                    default: return FileType.Default;
                }
            }
        }
    }
}
