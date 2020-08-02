using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData.VCS
{
    public class VCSSave : SaveData, ISaveData,
        IEquatable<VCSSave>, IDeepClonable<VCSSave>
    {
        private const int SizeOfGameInBytes = 0x18000;
        private const int OverSize = 2400;

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

        public override string Name
        {
            get { return m_name; }
            set { m_name = value; OnPropertyChanged(); }
        }

        public override DateTime TimeStamp
        {
            get { return (DateTime) SimpleVars.TimeStamp; }
            set { SimpleVars.TimeStamp = new Date(value); OnPropertyChanged(); }
        }

        bool ISaveData.HasSimpleVariables => true;
        bool ISaveData.HasScriptData => true;
        bool ISaveData.HasGarageData => false;      // TODO
        bool ISaveData.HasCarGenerators => false;
        bool ISaveData.HasPlayerInfo => true;       // TODO

        ISimpleVariables ISaveData.SimpleVars => SimpleVars;
        IScriptData ISaveData.ScriptData => Scripts;
        IGarageData ISaveData.GarageData => throw new NotSupportedException();
        ICarGeneratorData ISaveData.CarGenerators => throw new NotSupportedException();
        IPlayerInfo ISaveData.PlayerInfo => throw new NotSupportedException();

        IReadOnlyList<ISaveDataObject> ISaveData.Blocks => new List<SaveDataObject>()
        {
            SimpleVars,
            Scripts,
            Garages,
            PlayerInfo,
            Stats
        };

        public VCSSave()
        {
            SimpleVars = new SimpleVariables();
            Scripts = new ScriptData();
            Garages = new Dummy();
            PlayerInfo = new Dummy();
            Stats = new Dummy();
            m_over = new Dummy();
        }

        public VCSSave(VCSSave other)
        {
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Scripts = new ScriptData(other.Scripts);
            Garages = new Dummy(other.Garages);
            PlayerInfo = new Dummy(other.PlayerInfo);
            Stats = new Dummy(other.Stats);
            m_over = new Dummy(other.m_over);
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

        private int ReadDummyBlock(StreamBuffer file, string tag, out Dummy obj)
        {
            file.Mark();

            string savedTag = file.ReadString(4);
            Debug.Assert(savedTag == tag);

            int size = file.ReadInt32();
            Debug.Assert(file.Position + size <= file.Length);

            obj = new Dummy(size);
            Serializer.Read(obj, file, FileFormat);
            file.Align4();

            return file.Offset;
        }

        //private int ReadOverBlock(StreamBuffer file)
        //{
        //    file.Mark();

        //    string savedTag = file.ReadString(4);
        //    Debug.Assert(savedTag == "OVER");

        //    Debug.Assert(file.Position + OverSize < file.Length);

        //    m_over = new Dummy(OverSize);
        //    Serializer.Read(m_over, file, FileFormat);
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
            file.Write(data, FileFormat);
            file.Align4();

            Debug.Assert(file.Offset == sizeAligned + 8);
            m_checkSum += file.GetBytesFromMark().Sum(x => x);

            return size + 8;
        }

        //private int WriteOverBlock(StreamBuffer file)
        //{
        //    Debug.Assert(m_over.Data.Count == OverSize);

        //    file.Mark();
        //    file.Write("OVER", length: 4, zeroTerminate: false);
        //    file.Write(m_over.Data);
        //    file.Align4();

        //    Debug.Assert(file.Offset == OverSize + 4);
        //    m_checkSum += file.GetBytesFromMark().Sum(x => x);

        //    return OverSize + 4;
        //}

        protected override void LoadAllData(StreamBuffer file)
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
            totalSize += Align4(WriteDataBlock(file, "OVER", m_over));

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
            const int SimpSizePS2 = 0x104;
            const int SimpSizePSP = 0xC8;

            using (StreamBuffer buf = new StreamBuffer(data))
            {
                if (buf.Length < 0x1000) goto DetectionFailed;
                buf.Skip(4);

                // TODO: more rigorous file verification

                int simpSize = buf.ReadInt32();
                int skip = simpSize + 4;
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
            size += Align4(SizeOfObject(SimpleVars, fmt)) + 8;
            size += Align4(SizeOfObject(Scripts, fmt)) + 8;
            size += Align4(SizeOfObject(Garages, fmt)) + 8;
            size += Align4(SizeOfObject(PlayerInfo, fmt)) + 8;
            size += Align4(SizeOfObject(Stats, fmt)) + 8;
            size += Align4(SizeOfObject(m_over, fmt)) + 8;

            if (fmt.IsPS2) size += (SizeOfGameInBytes - size);

            return size;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VCSSave);
        }

        public bool Equals(VCSSave other)
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

        public VCSSave DeepClone()
        {
            return new VCSSave(this);
        }

        public static class FileFormats
        {
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
