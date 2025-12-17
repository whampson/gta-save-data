using GTASaveData.Extensions;
using GTASaveData.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData.IV
{
    public class GTA4SaveParams : SerializationParams
    {
        public FileType FileType { get; set; }

        // TODO: add custom params here!

        public static GTA4SaveParams GetDefaults(FileType t)
        {
            bool isValid = GTA4Save.FileTypes.GetAll().Select(x => x == t).Any();
            if (!isValid)
            {
                return null;
            }

            GTA4SaveParams p = new GTA4SaveParams() { FileType = t };
            if (t.IsXbox || t.IsPS3)
            {
                p.BigEndian = true;
            }

            return p;
        }
    }

    public enum BlockId : int
    {
        SimpleVars = 0,
        Pickups = 7,
    }

    /// <summary>
    /// Represents a <i>Grand Theft Auto IV</i> save file.
    /// </summary>
    public class GTA4Save : SaveFile<GTA4SaveParams>, ISaveFile,
        IEquatable<GTA4Save>, IDeepClonable<GTA4Save>
    {
        public const int MaxNameLength = 128;

        public struct SaveHeader
        {
            public int Version;
            public int SaveSizeInBytes;    // TODO: is this accurate or have any use at all?
            public int ScriptSpaceSize;    // TODO: is this accurate or have any use at all?
            public string LastMissionPassedName;
        }

        SaveHeader m_header;
        private DateTime m_timestamp; // not saved in file
        private ObservableArray<Block> m_blockData;

        public SaveHeader Header
        {
            get { return m_header; }
        }

        public string Title
        {
            get { return m_header.LastMissionPassedName; }
            set { m_header.LastMissionPassedName = value; OnPropertyChanged(); }
        }

        public DateTime Timestamp
        {
            get { return m_timestamp; }
            set { m_timestamp = value; OnPropertyChanged(); }
        }

        public SimpleVariables SimpleVars
        {
            get { return m_blockData[0] as SimpleVariables; }
            set { m_blockData[0] = value; OnPropertyChanged(); }
        }

        public PickupData Pickups
        {
            get { return m_blockData[(int) BlockId.Pickups] as PickupData; }
            set { m_blockData[(int) BlockId.Pickups] = value; OnPropertyChanged(); }
        }

        public static GTA4Save LoadFromFile(string path, FileType t)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (t == FileType.Default)
            {
                if (!TryGetFileType<GTA4Save>(path, out t))
                {
                    throw new InvalidDataException("Unable to detect file type!");
                }
            }

            GTA4SaveParams p = GTA4SaveParams.GetDefaults(t)
                ?? throw new InvalidDataException($"Invalid file type! ({t})");

            return LoadFromFile<GTA4Save>(path, Game.IV, p, out _);
        }

        public static bool TryLoadFromFile(string path, FileType t, out GTA4Save saveData)
        {
            if (t == FileType.Default)
            {
                if (!TryGetFileType<GTA4Save>(path, out t))
                {
                    throw new InvalidDataException("Unable to detect file type!");
                }
            }

            GTA4SaveParams p = GTA4SaveParams.GetDefaults(t)
                ?? throw new InvalidDataException($"Invalid file type! ({t})");

            return TryLoadFromFile(path, Game.IV, p, out saveData);
        }

        public static bool TryGetFileType(string path, out FileType t)
        {
            return TryGetFileType<GTA4Save>(path, out t);
        }

        public GTA4Save()
            : base(Game.IV)
        {
            Title = "";
            m_blockData = new ObservableArray<Block>();
        }

        public GTA4Save(GTA4Save other)
            : base(Game.IV)
        {
            m_header = other.m_header;
            m_timestamp = other.Timestamp;
            m_blockData = new ObservableArray<Block>(other.m_blockData);
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Pickups = new PickupData(other.Pickups);
        }

        private SaveHeader ReadSaveHeader(DataBuffer file)
        {
            SaveHeader head = new SaveHeader
            {
                Version = file.ReadInt32(),
                SaveSizeInBytes = file.ReadInt32(),
                ScriptSpaceSize = file.ReadInt32()
            };

            string tag = file.ReadString(4);
            Debug.Assert(tag == "SAVE");

            if (Params.FileType.IsWindows)
            {
                head.LastMissionPassedName = file.ReadString(128, unicode: true);
            }

            return head;
        }

        protected override void Load(DataBuffer file)
        {
            byte[] data = file.GetBuffer();
            int pSaveHeader = data.FindFirst("SAVE".GetAsciiBytes()) - 12;
            int pConsoleHeader = data.FindFirst("CON ".GetAsciiBytes());
            int[] pBlocks = data.FindAll("BLOCK".GetAsciiBytes());

            file.Seek(pSaveHeader);
            m_header = ReadSaveHeader(file);

            for (int i = 0; i <  pBlocks.Length; i++)
            {
                Block b;
                file.Seek(pBlocks[i]);
                switch ((BlockId) i)
                {
                    case BlockId.SimpleVars:
                        b = file.ReadObject<SimpleVariables>();
                        break;
                    case BlockId.Pickups:
                        b = file.ReadObject<PickupData>();
                        break;
                    default:
                        b = file.ReadObject<Block>();
                        break;
                }
                Debug.Assert(b != null);
                m_blockData.Add(b);
            }
        }

        protected override void Save(DataBuffer file)
        {
            throw new NotImplementedException();        // TODO
        }

        protected override bool DetectFileType(byte[] data, out FileType t)
        {
            int pSaveHeader = data.FindFirst("SAVE".GetAsciiBytes()) - 12;
            int pConsoleHeader = data.FindFirst("CON ".GetAsciiBytes());

            if (pSaveHeader < 0)
            {
                goto Invalid;
            }

            using (DataBuffer buf = new DataBuffer(data))
            {
                t = FileType.Default;
                Console.WriteLine($"pSaveHeader={pSaveHeader}");

                buf.Seek(pSaveHeader);
                int version = buf.ReadInt32();
                if (version == 0x38000000)  // PS3/X360 (console) save
                {
                    buf.BigEndian = true;
                    t = pConsoleHeader >= 0
                        ? FileTypes.Xbox360
                        : FileTypes.PS3;

                    // TODO: further distinguish X360 and PS3
                }
                else if (version == 0x39)   // PC save
                {
                    t = FileTypes.PC;
                }

                int[] pBlocks = data.FindAll("BLOCK".GetAsciiBytes()).ToArray();
                if (pBlocks.Length < 30)
                {
                    goto Invalid;
                }

                return true;
            }

        Invalid:
            t = FileType.Default;
            return false;
        }

        protected override int GetSize(SerializationParams p)
        {
            throw new NotImplementedException();        // TODO
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GTA4Save);
        }

        public bool Equals(GTA4Save other)
        {
            if (other == null)
            {
                return false;
            }

            return Title.Equals(other.Title)
                && Timestamp.Equals(other.Timestamp)
                && Header.Equals(other.Header)
                && SimpleVars.Equals(other.SimpleVars)
                && Pickups.Equals(other.Pickups)
                && m_blockData.Equals(other.m_blockData);
        }

        public GTA4Save DeepClone()
        {
            return new GTA4Save(this);
        }

        public static class FileTypes
        {
            public static readonly FileType PC = new FileType(
                nameof(PC),
                GameVersionId.Windows
            );

            public static readonly FileType PS3 = new FileType(
                nameof(PS3),
                GameVersionId.PS3
            );

            public static readonly FileType Xbox360 = new FileType(
                nameof(Xbox360),
                GameVersionId.Xbox
            );

            public static FileType[] GetAll()
            {
                return new FileType[] { PC, PS3, Xbox360 };
            }
        }
    }
}
