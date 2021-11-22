using GTASaveData.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.VCS
{
    public class SaveFileVCS : SaveFile, ISaveFile,
        IEquatable<SaveFileVCS>, IDeepClonable<SaveFileVCS>
    {
        private const int SizeOfGameInBytes = 0x18000;

        private int m_checkSum;
        private string m_name;

        private SimpleVariables m_simpleVars;
        private ScriptData m_scripts;
        private Dummy m_garages;
        private Dummy m_playerInfo;
        private Dummy m_stats;
        private Dummy m_over;

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

        public Dummy Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public Dummy PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; OnPropertyChanged(); }
        }

        public Dummy Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public override string Title
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public override DateTime TimeStamp
        {
            get { return (DateTime) SimpleVars.TimeStamp; }
            set { SimpleVars.TimeStamp = new Date(value); OnPropertyChanged(); }
        }

        bool ISaveFile.HasSimpleVariables => true;
        bool ISaveFile.HasScriptData => true;
        bool ISaveFile.HasGarageData => false;      // TODO
        bool ISaveFile.HasCarGenerators => false;
        bool ISaveFile.HasPlayerInfo => false;       // TODO
        bool ISaveFile.HasStats => false;       // tODO

        ISimpleVariables ISaveFile.SimpleVars => SimpleVars;
        IScriptData ISaveFile.ScriptData => Scripts;
        IGarageData ISaveFile.GarageData => throw new NotImplementedException();
        ICarGeneratorData ISaveFile.CarGenerators => throw new NotImplementedException();
        IPlayerInfo ISaveFile.PlayerInfo => throw new NotImplementedException();
        IStats ISaveFile.Stats => throw new NotImplementedException();

        IReadOnlyList<ISaveDataObject> ISaveFile.Blocks => new List<SaveDataObject>()
        {
            SimpleVars,
            Scripts,
            Garages,
            PlayerInfo,
            Stats
        };

        public static SaveFileVCS Load(string path)
        {
            return Load<SaveFileVCS>(path);
        }

        public static SaveFileVCS Load(string path, FileFormat fmt)
        {
            return Load<SaveFileVCS>(path, fmt);
        }

        public SaveFileVCS()
        {
            SimpleVars = new SimpleVariables();
            Scripts = new ScriptData();
            Garages = new Dummy();
            PlayerInfo = new Dummy();
            Stats = new Dummy();
            m_over = new Dummy();
        }

        public SaveFileVCS(SaveFileVCS other)
        {
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Scripts = new ScriptData(other.Scripts);
            Garages = new Dummy(other.Garages);
            PlayerInfo = new Dummy(other.PlayerInfo);
            Stats = new Dummy(other.Stats);
            m_over = new Dummy(other.m_over);
        }

        private int ReadDataBlock<T>(DataBuffer file, string tag, out T obj)
            where T : SaveDataObject, new()
        {
            file.Mark();

            string savedTag = file.ReadString(4);
            Debug.Assert(savedTag == tag);

            int size = file.ReadInt32();
            Debug.Assert(file.Position + size < file.Length);

            obj = file.ReadObject<T>(FileType);
            file.Align4();

            return file.Offset;
        }

        private int ReadDummyBlock(DataBuffer file, string tag, out Dummy obj)
        {
            file.Mark();

            string savedTag = file.ReadString(4);
            Debug.Assert(savedTag == tag);

            int size = file.ReadInt32();
            Debug.Assert(file.Position + size <= file.Length);

            obj = new Dummy(size);
            Serializer.Read(obj, file, FileType);
            file.Align4();

            return file.Offset;
        }

        private int WriteDataBlock<T>(DataBuffer file, string tag, T obj)
            where T : SaveDataObject
        {
            int size = SerializeData(obj, out byte[] data);
            int sizeAligned = Align4(size);

            file.Mark();
            file.Write(tag, length: 4, zeroTerminate: false);
            file.Write(size);
            file.Write(data, FileType);
            file.Align4();

            Debug.Assert(file.Offset == sizeAligned + 8);
            m_checkSum += file.GetBytesFromMark().Sum(x => x);

            return size + 8;
        }

        protected override void Load(DataBuffer file)
        {
            int totalSize = 0;

            totalSize += Align4(ReadDataBlock(file, "SIMP", out SimpleVariables simp));
            SimpleVars = simp;
            totalSize += Align4(ReadDataBlock(file, "SRPT", out ScriptData srpt));
            Scripts = srpt;
            totalSize += Align4(ReadDummyBlock(file, "GRGE", out Dummy grge));
            Garages = grge;
            totalSize += Align4(ReadDummyBlock(file, "PLYR", out Dummy plyr));
            PlayerInfo = plyr;
            totalSize += Align4(ReadDummyBlock(file, "STAT", out Dummy stat));
            Stats = stat;
            totalSize += Align4(ReadDummyBlock(file, "OVER", out Dummy over));
            m_over = over;

            if (FileType.IsPS2)
            {
                file.Skip(SizeOfGameInBytes - totalSize);
            }

            Debug.WriteLine("Load successful!");
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
            totalSize += Align4(WriteDataBlock(file, "OVER", m_over));

            if (FileType.IsPS2)
            {
                file.Mark();
                totalSize += file.Pad(SizeOfGameInBytes - totalSize - 4);

                m_checkSum += file.GetBytesFromMark().Sum(x => x);
                totalSize += file.Write(m_checkSum);

                Debug.Assert(totalSize == SizeOfGameInBytes);
            }

            Debug.WriteLine("Save successful!");
        }

        protected override bool DetectFileType(byte[] data, out FileFormat fmt)
        {
            const int SimpSizePS2 = 0x104;
            const int SimpSizePSP = 0xC8;

            using (DataBuffer buf = new DataBuffer(data))
            {
                if (buf.Length < 0x1000) goto DetectionFailed;

                string simp = buf.ReadString(4);
                if (simp != "SIMP") goto DetectionFailed;
                int simpSize = buf.ReadInt32();
                int skip = simpSize;
                if (buf.Position + skip > buf.Length) goto DetectionFailed;

                buf.Skip(skip);
                string srpt = buf.ReadString(4);
                if (srpt != "SRPT") goto DetectionFailed;
                int srptSize = buf.ReadInt32();
                skip = srptSize;
                if (buf.Position + skip > buf.Length) goto DetectionFailed;

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
            size += Align4(SizeOf(SimpleVars, fmt)) + 8;
            size += Align4(SizeOf(Scripts, fmt)) + 8;
            size += Align4(SizeOf(Garages, fmt)) + 8;
            size += Align4(SizeOf(PlayerInfo, fmt)) + 8;
            size += Align4(SizeOf(Stats, fmt)) + 8;
            size += Align4(SizeOf(m_over, fmt)) + 8;

            if (fmt.IsPS2) size += (SizeOfGameInBytes - size);

            return size;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SaveFileVCS);
        }

        public bool Equals(SaveFileVCS other)
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

        public SaveFileVCS DeepClone()
        {
            return new SaveFileVCS(this);
        }

        public static class FileFormats
        {
            public static readonly FileFormat PS2 = new FileFormat(
                "PS2", "PS2", "PlayStation 2",
                GameSystem.PS2
            );

            public static readonly FileFormat PSP = new FileFormat(
                "PSP", "PSP", "PlayStation Portable",
                GameSystem.PSP
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { PS2, PSP };
            }
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
}
