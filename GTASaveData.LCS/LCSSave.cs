using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.LCS
{
    public class LCSSave : SaveData, ISaveData,
        IEquatable<LCSSave>, IDeepClonable<LCSSave>
    {
        private const int SizeOfGameInBytes = 0x19000;

        private int m_checkSum;

        private SimpleVariables m_simpleVars;
        private ScriptData m_scripts;
        private GarageData m_garages;
        private PlayerInfo m_playerInfo;
        private Stats m_stats;

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

        bool ISaveData.HasCarGenerators => false;

        ICarGeneratorData ISaveData.CarGenerators
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        IReadOnlyList<ISaveDataObject> ISaveData.Blocks => new List<SaveDataObject>()
        {
            SimpleVars,
            Scripts,
            Garages,
            PlayerInfo,
            Stats
        };

        public override string Name
        {
            get { return SimpleVars.LastMissionPassedName; }
            set { SimpleVars.LastMissionPassedName = value; OnPropertyChanged(); }
        }

        public override DateTime TimeStamp
        {
            get { return (DateTime) SimpleVars.TimeStamp; }
            set { SimpleVars.TimeStamp = new Date(value); OnPropertyChanged(); }
        }

        public LCSSave()
        {
            SimpleVars = new SimpleVariables();
            Scripts = new ScriptData();
            Garages = new GarageData();
            PlayerInfo = new PlayerInfo();
            Stats = new Stats();
        }

        public LCSSave(LCSSave other)
        {
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Scripts = new ScriptData(other.Scripts);
            Garages = new GarageData(other.Garages);
            PlayerInfo = new PlayerInfo(other.PlayerInfo);
            Stats = new Stats(other.Stats);
        }

        private int ReadDataBlock<T>(StreamBuffer file, string tag, out T obj)
            where T : SaveDataObject, new()
        {
            file.Mark();

            string savedTag = file.ReadString(4);
            Debug.Assert(savedTag == tag);

            int size = file.ReadInt32();
            Debug.Assert(file.Position + size < file.Length);

            obj = file.Read<T>(FileFormat);
            file.Align4();

            return file.Offset;
        }

        //private int ReadDummyBlock(StreamBuffer file, string tag, out Dummy obj)
        //{
        //    file.Mark();

        //    string savedTag = file.ReadString(4);
        //    Debug.Assert(savedTag == tag);

        //    int size = file.ReadInt32();
        //    Debug.Assert(file.Position + size <= file.Length);

        //    obj = new Dummy(size);
        //    Serializer.Read(obj, file, FileFormat);
        //    file.Align4();

        //    return file.Offset;
        //}

        private int WriteDataBlock<T>(StreamBuffer file, string tag, T obj)
            where T : SaveDataObject
        {
            int size = SerializeData(obj, out byte[] data);
            int sizeAligned = Align4(size);

            file.Mark();
            file.Write(tag, length: 4, zeroTerminate: false);
            file.Write(size);
            file.Write(data);
            file.Align4();

            Debug.Assert(file.Offset == sizeAligned + 8);
            m_checkSum += file.GetBytesFromMark().Sum(x => x);

            return size + 8;
        }

        protected override void LoadAllData(StreamBuffer file)
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

            if (FileFormat.IsPS2)
            {
                file.Skip(SizeOfGameInBytes - totalSize);
            }

            Debug.WriteLine("Load successful!");
        }

        protected override void SaveAllData(StreamBuffer file)
        {
            int totalSize = 0;
            m_checkSum = 0;

            totalSize += Align4(WriteDataBlock(file, "SIMP", SimpleVars));
            totalSize += Align4(WriteDataBlock(file, "SRPT", Scripts));
            totalSize += Align4(WriteDataBlock(file, "GRGE", Garages));
            totalSize += Align4(WriteDataBlock(file, "PLYR", PlayerInfo));
            totalSize += Align4(WriteDataBlock(file, "STAT", Stats));

            if (FileFormat.IsPS2)
            {
                file.Mark();
                totalSize += file.Pad(SizeOfGameInBytes - totalSize - 4);

                m_checkSum += file.GetBytesFromMark().Sum(x => x);
                totalSize += file.Write(m_checkSum);

                Debug.Assert(totalSize == SizeOfGameInBytes);
            }

            Debug.WriteLine("Save successful!");
        }

        protected override bool DetectFileFormat(byte[] data, out FileFormat fmt)
        {
            const int SimpSizePS2 = 0xF8;
            const int SimpSizePSP = 0xBC;
            const int RunningScriptSizeAndroid = 0x21C;
            const int RunningScriptSizeiOS = 0x228;

            using (StreamBuffer buf = new StreamBuffer(data))
            {
                if (buf.Length < 8) goto DetectionFailed;
                buf.Skip(4);

                int simpSize = buf.ReadInt32();
                int skip = simpSize + 4;
                if (buf.Position + skip > buf.Length) goto DetectionFailed;
                buf.Skip(skip);

                int srptSize = buf.ReadInt32();
                int srptOffset = buf.Position;
                buf.Skip(8);

                int globalsSize = buf.ReadInt32();
                skip = globalsSize + 0x7C0;
                if (buf.Position + skip > buf.Length) goto DetectionFailed;
                buf.Skip(skip);

                int numRunningScripts = buf.ReadInt32();
                int runningScriptSize = (srptOffset + srptSize - buf.Position) / numRunningScripts;

                if (simpSize == SimpSizePS2)
                {
                    fmt = FileFormats.PS2;
                    return true;
                }
                if (simpSize == SimpSizePSP)
                {
                    fmt = FileFormats.PSP;
                    return true;
                }
                if (runningScriptSize == RunningScriptSizeAndroid)
                {
                    fmt = FileFormats.Android;
                    return true;
                }
                if (runningScriptSize == RunningScriptSizeiOS)
                {
                    fmt = FileFormats.iOS;
                    return true;
                }
            }

        DetectionFailed:
            fmt = FileFormat.Default;
            return false;
        }

        private static int Align4(int addr)
        {
            return (int) (addr + 3 & 0xFFFFFFFC);
        }

        protected override int GetSize(FileFormat fmt)
        {
            int size = 0;
            size += Align4(SizeOfObject(SimpleVars, fmt)) + 8;
            size += Align4(SizeOfObject(Scripts, fmt)) + 8;
            size += Align4(SizeOfObject(Garages, fmt)) + 8;
            size += Align4(SizeOfObject(PlayerInfo, fmt)) + 8;
            size += Align4(SizeOfObject(Stats, fmt)) + 8;

            if (fmt.IsPS2) size += (SizeOfGameInBytes - size);

            return size;
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

        public static class FileFormats
        {
            public static readonly FileFormat Android = new FileFormat(
                "Android", "Android", "Android OS",
                GameConsole.Android
            );

            public static readonly FileFormat iOS = new FileFormat(
                "iOS", "iOS", "Apple iOS",
                GameConsole.iOS
            );

            public static readonly FileFormat PS2 = new FileFormat(
                "PS2", "PS2", "PlayStation 2",
                GameConsole.PS2
            );

            public static readonly FileFormat PSP = new FileFormat(
                "PSP", "PSP", "PlayStation Portable",
                GameConsole.PSP
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { Android, iOS, PS2, PSP };
            }
        }
    }
}
