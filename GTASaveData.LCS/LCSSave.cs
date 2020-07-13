using GTASaveData.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace GTASaveData.LCS
{
    public class LCSSave : SaveData, ISaveData,
        IEquatable<LCSSave>, IDeepClonable<LCSSave>
    {
        private const int SizeOfGameInBytes = 0x19000;

        private int m_checkSum;

        private Dummy m_simpleVars;
        private Dummy m_scripts;
        private Dummy m_garages;
        private Dummy m_playerInfo;
        private Dummy m_stats;

        public Dummy SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public Dummy Scripts
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

        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override DateTime TimeStamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LCSSave()
        {
            SimpleVars = new Dummy();
            Scripts = new Dummy();
            Garages = new Dummy();
            PlayerInfo = new Dummy();
            Stats = new Dummy();
        }

        public LCSSave(LCSSave other)
        {
            //SimpleVars = new SimpleVariables(other.SimpleVars);
            // TODO
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

        private int WriteDataBlock<T>(StreamBuffer file, string tag, T obj)
            where T : SaveDataObject
        {
            int size = SerializeData(obj, out byte[] data);
            size = (int) (size + 3 & 0xFFFFFFFC);

            file.Mark();
            file.Write(tag, length: 4, zeroTerminate: false);
            file.Write(size);
            file.Write(data);
            file.Align4();

            Debug.Assert(file.Offset == size + 8);
            m_checkSum += file.GetBytesFromMark().Sum(x => x);

            return file.Offset;
        }

        protected override void LoadAllData(StreamBuffer file)
        {
            int totalSize = 0;

            totalSize += ReadDataBlock(file, "SIMP", out Dummy simp);
            SimpleVars = simp;
            totalSize += ReadDataBlock(file, "SRPT", out Dummy srpt);
            Scripts = srpt;
            totalSize += ReadDataBlock(file, "GRGE", out Dummy grge);
            Garages = grge;
            totalSize += ReadDataBlock(file, "PLYR", out Dummy plyr);
            PlayerInfo = plyr;
            totalSize += ReadDataBlock(file, "STAT", out Dummy stat);
            Stats = stat;

            if (FileFormat.IsPS2)
            {
                file.Skip(SizeOfGameInBytes - totalSize);
            }
            else
            {
                file.Skip(3);
            }

            Debug.WriteLine("Load successful!");
        }

        protected override void SaveAllData(StreamBuffer file)
        {
            int totalSize = 0;
            m_checkSum = 0;

            totalSize += WriteDataBlock(file, "SIMP", SimpleVars);
            totalSize += WriteDataBlock(file, "SRPT", Scripts);
            totalSize += WriteDataBlock(file, "GRGE", Garages);
            totalSize += WriteDataBlock(file, "PLYR", PlayerInfo);
            totalSize += WriteDataBlock(file, "STAT", Stats);

            if (FileFormat.IsPS2)
            {
                file.Pad(SizeOfGameInBytes - totalSize - 4);
                file.Write(m_checkSum);
            }
            else
            {
                file.Write(new byte[3]);
            }

            Debug.WriteLine("Save successful!");
        }

        protected override bool DetectFileFormat(byte[] data, out FileFormat fmt)
        {
            // TODO
            throw new NotImplementedException();
        }

        protected override int GetSize(FileFormat fmt)
        {
            int size = 0;
            size += SizeOfObject(SimpleVars, fmt) + 8;
            size += SizeOfObject(Scripts, fmt) + 8;
            size += SizeOfObject(Garages, fmt) + 8;
            size += SizeOfObject(PlayerInfo, fmt) + 8;
            size += SizeOfObject(Stats, fmt) + 8;

            if (fmt.IsPS2) size += (SizeOfGameInBytes - size);
            return size + 3;
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
