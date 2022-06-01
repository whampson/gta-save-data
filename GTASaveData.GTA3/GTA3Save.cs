using GTASaveData.Extensions;
using GTASaveData.Types;
using GTASaveData.Helpers;
using GTASaveData.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// A saved <i>Grand Theft Auto III</i> game.
    /// </summary>
    public class GTA3Save : SaveFileGTA3VC<GTA3SaveParams>, ISaveFile,
        IEquatable<GTA3Save>, IDeepClonable<GTA3Save>
    {
        private const int NumOuterBlocks = 20;
        private const int NumOuterBlocksPS2 = 3;
        private const int SizeOfGameDataInBytes = 0x31400;

        private SimpleVariables m_simpleVars;
        private ScriptBlock m_theScripts;
        private PedPool m_pedPool;
        private GarageBlock m_garages;
        private VehiclePool m_vehiclePool;
        private ObjectPool m_objectPool;
        private PathData m_paths;
        private CraneData m_cranes;
        private PickupData m_pickups;
        private PhoneData m_phoneInfo;
        private RestartData m_restartPoints;
        private RadarData m_radarBlips;
        private ZoneData m_zones;
        private GangData m_gangs;
        private CarGeneratorData m_carGenerators;
        private ParticleData m_particleObjects;
        private AudioScriptData m_audioScriptObjects;
        private PlayerInfo m_playerInfo;
        private Stats m_stats;
        private Streaming m_streaming;
        private PedTypeData m_pedType;

        /// <summary>
        /// Miscellaneous variables related to weather, time, the camera,
        /// PS2 settings, and others.
        /// </summary>
        public SimpleVariables SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Mission script data like global variables, <c>MAIN.SCM</c> info,
        /// the saved state of active mission scripts, and others.
        /// </summary>
        public ScriptBlock Script
        {
            get { return m_theScripts; }
            set { m_theScripts = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Contains the player peds. Here you can control the health, armor,
        /// weapons, max wanted level, stamina, and model of the player. You can
        /// also add multiple player peds.
        /// </summary>
        public PedPool PedPool
        {
            get { return m_pedPool; }
            set { m_pedPool = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Garage data, including stored vehicles and script-controlled garage info.
        /// </summary>
        public GarageBlock Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public VehiclePool Vehicles
        {
            get { return m_vehiclePool; }
            set { m_vehiclePool = value; OnPropertyChanged(); }
        }

        public ObjectPool Objects
        {
            get { return m_objectPool; }
            set { m_objectPool = value; OnPropertyChanged(); }
        }

        public PathData Paths
        {
            get { return m_paths; }
            set { m_paths = value; OnPropertyChanged(); }
        }

        public CraneData Cranes
        {
            get { return m_cranes; }
            set { m_cranes = value; OnPropertyChanged(); }
        }

        public PickupData Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
        }

        public PhoneData PhoneInfo
        {
            get { return m_phoneInfo; }
            set { m_phoneInfo = value; OnPropertyChanged(); }
        }

        public RestartData RestartPoints
        {
            get { return m_restartPoints; }
            set { m_restartPoints = value; OnPropertyChanged(); }
        }

        public RadarData RadarBlips
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; OnPropertyChanged(); }
        }

        public ZoneData Zones
        {
            get { return m_zones; }
            set { m_zones = value; OnPropertyChanged(); }
        }

        public GangData Gangs
        {
            get { return m_gangs; }
            set { m_gangs = value; OnPropertyChanged(); }
        }

        public CarGeneratorData CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; OnPropertyChanged(); }
        }

        public ParticleData ParticleObjects
        {
            get { return m_particleObjects; }
            set { m_particleObjects = value; OnPropertyChanged(); }
        }

        public AudioScriptData AudioScriptObjects
        {
            get { return m_audioScriptObjects; }
            set { m_audioScriptObjects = value; OnPropertyChanged(); }
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

        public Streaming Streaming
        {
            get { return m_streaming; }
            set { m_streaming = value; OnPropertyChanged(); }
        }

        public PedTypeData PedTypeInfo
        {
            get { return m_pedType; }
            set { m_pedType = value; OnPropertyChanged(); }
        }

        public override string Title
        {
            get { return SimpleVars.LastMissionPassedName; }
            set { SimpleVars.LastMissionPassedName = value; OnPropertyChanged(); }
        }

        public override DateTime TimeStamp
        {
            get { return (DateTime) SimpleVars.TimeStamp; }
            set { SimpleVars.TimeStamp = new SystemTime(value); OnPropertyChanged(); }
        }

        public static bool TryGetFileType(string path, out FileType t)
        {
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                if (info.Length > MaxFileSize)
                {
                    goto Fail;
                }

                byte[] data = File.ReadAllBytes(path);
                return TryGetFileType(data, out t);
            }

        Fail:
            t = FileType.Default;
            return false;
        }

        public static bool TryGetFileType(byte[] buf, out FileType t)
        {
            return new GTA3Save().DetectFileType(buf, out t);
        }

        public static bool TryLoad(string path, out GTA3Save saveFile)
        {
            return TryLoad(path, FileType.Default, out saveFile);
        }

        public static bool TryLoad(string path, FileType t, out GTA3Save saveFile)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            try
            {
                saveFile = Load(path, t);
                return true;
            }
            catch (Exception e)
            {
                if (!(e is InvalidDataException && e is SerializationException))
                {
                    throw;
                }
            }

            saveFile = null;
            return false;
        }

        public static bool TryLoad(string path, GTA3SaveParams param, out GTA3Save saveFile)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (param == null) throw new ArgumentNullException(nameof(param));

            try
            {
                saveFile = Load(path, param);
                return true;
            }
            catch (Exception e)
            {
                if (!(e is InvalidDataException && e is SerializationException))
                {
                    throw;
                }
            }

            saveFile = null;
            return false;
        }

        public static bool TryLoad(byte[] buf, GTA3SaveParams param, out GTA3Save saveFile)
        {
            if (buf == null) throw new ArgumentNullException(nameof(buf));
            if (param == null) throw new ArgumentNullException(nameof(param));

            try
            {
                saveFile = Load(buf, param);
                return true;
            }
            catch (Exception e)
            {
                if (!(e is InvalidDataException && e is SerializationException))
                {
                    throw;
                }
            }

            saveFile = null;
            return false;
        }

        public static GTA3Save Load(string path)
        {
            return Load(path, FileType.Default);
        }

        public static GTA3Save Load(string path, FileType t)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (t == FileType.Default)
            {
                if (!TryGetFileType(path, out t))
                {
                    throw new InvalidDataException("Unable to detect file type!");
                }
            }

            var s = new GTA3Save();
            var p = GTA3SaveParams.GetDefaults(t);
            if (p == null)
            {
                return null;
            }

            s.Params = p;
            s.LoadInternal(path);

            return s;
        }

        public static GTA3Save Load(string path, GTA3SaveParams param)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (param == null) throw new ArgumentNullException(nameof(param));

            GTA3Save s = new GTA3Save { Params = param };
            s.LoadInternal(path);

            return s;
        }

        public static GTA3Save Load(byte[] buf, GTA3SaveParams param)
        {
            if (buf == null) throw new ArgumentNullException(nameof(buf));
            if (param == null) throw new ArgumentNullException(nameof(param));

            GTA3Save s = new GTA3Save { Params = param };
            s.LoadInternal(buf);

            return s;
        }

        public GTA3Save()
            : base(Game.GTA3)
        {
            SimpleVars = new SimpleVariables();
            Script = new ScriptBlock();
            PedPool = new PedPool();
            Garages = new GarageBlock();
            Vehicles = new VehiclePool();
            Objects = new ObjectPool();
            Paths = new PathData();
            Cranes = new CraneData();
            Pickups = new PickupData();
            PhoneInfo = new PhoneData();
            RestartPoints = new RestartData();
            RadarBlips = new RadarData();
            Zones = new ZoneData();
            Gangs = new GangData();
            CarGenerators = new CarGeneratorData();
            ParticleObjects = new ParticleData();
            AudioScriptObjects = new AudioScriptData();
            PlayerInfo = new PlayerInfo();
            Stats = new Stats();
            Streaming = new Streaming();
            PedTypeInfo = new PedTypeData();

            TimeStamp = DateTime.Now;
        }

        public GTA3Save(GTA3Save other)
            : base(Game.GTA3)
        {
            SimpleVars = new SimpleVariables(other.SimpleVars);
            Script = new ScriptBlock(other.Script);
            PedPool = new PedPool(other.PedPool);
            Garages = new GarageBlock(other.Garages);
            Vehicles = new VehiclePool(other.Vehicles);
            Objects = new ObjectPool(other.Objects);
            Paths = new PathData(other.Paths);
            Cranes = new CraneData(other.Cranes);
            Pickups = new PickupData(other.Pickups);
            PhoneInfo = new PhoneData(other.PhoneInfo);
            RestartPoints = new RestartData(other.RestartPoints);
            RadarBlips = new RadarData(other.RadarBlips);
            Zones = new ZoneData(other.Zones);
            Gangs = new GangData(other.Gangs);
            CarGenerators = new CarGeneratorData(other.CarGenerators);
            ParticleObjects = new ParticleData(other.ParticleObjects);
            AudioScriptObjects = new AudioScriptData(other.AudioScriptObjects);
            PlayerInfo = new PlayerInfo(other.PlayerInfo);
            Stats = new Stats(other.Stats);
            Streaming = new Streaming(other.Streaming);
            PedTypeInfo = new PedTypeData(other.PedTypeInfo);
        }

        protected override void Load(DataBuffer file)
        {
            int dataSize = 0;
            int numOuterBlocks;

            if (IsDE)
            {
                LoadDE(file);
                return;
            }

            if (IsPS2)
            {
                dataSize += FillWorkBuffer(file);
                SimpleVars = WorkBuff.ReadObject<SimpleVariables>(Params);
                Script = ReadBlock<ScriptBlock>();
                PedPool = ReadBlock<PedPool>();
                Garages = ReadBlock<GarageBlock>();
                Vehicles = ReadBlock<VehiclePool>();

                dataSize += FillWorkBuffer(file);
                Objects = ReadBlock<ObjectPool>();
                Paths = ReadBlock<PathData>();
                Cranes = ReadBlock<CraneData>();

                dataSize += FillWorkBuffer(file);
                Pickups = ReadBlock<PickupData>();
                PhoneInfo = ReadBlock<PhoneData>();
                RestartPoints = ReadBlock<RestartData>();
                RadarBlips = ReadBlock<RadarData>();
                Zones = ReadBlock<ZoneData>();
                Gangs = ReadBlock<GangData>();
                CarGenerators = ReadBlock<CarGeneratorData>();
                ParticleObjects = ReadBlock<ParticleData>();
                AudioScriptObjects = ReadBlock<AudioScriptData>();
                PlayerInfo = ReadBlock<PlayerInfo>();
                Stats = ReadBlock<Stats>();
                Streaming = ReadBlock<Streaming>();
                PedTypeInfo = ReadBlock<PedTypeData>();

                numOuterBlocks = NumOuterBlocksPS2;
            }
            else
            {
                dataSize += FillWorkBuffer(file);
                SimpleVars = WorkBuff.ReadObject<SimpleVariables>(Params);
                Script = ReadBlock<ScriptBlock>();
                dataSize += FillWorkBuffer(file); PedPool = ReadBlock<PedPool>();
                dataSize += FillWorkBuffer(file); Garages = ReadBlock<GarageBlock>();
                dataSize += FillWorkBuffer(file); Vehicles = ReadBlock<VehiclePool>();
                dataSize += FillWorkBuffer(file); Objects = ReadBlock<ObjectPool>();
                dataSize += FillWorkBuffer(file); Paths = ReadBlock<PathData>();
                dataSize += FillWorkBuffer(file); Cranes = ReadBlock<CraneData>();
                dataSize += FillWorkBuffer(file); Pickups = ReadBlock<PickupData>();
                dataSize += FillWorkBuffer(file); PhoneInfo = ReadBlock<PhoneData>();
                dataSize += FillWorkBuffer(file); RestartPoints = ReadBlock<RestartData>();
                dataSize += FillWorkBuffer(file); RadarBlips = ReadBlock<RadarData>();
                dataSize += FillWorkBuffer(file); Zones = ReadBlock<ZoneData>();
                dataSize += FillWorkBuffer(file); Gangs = ReadBlock<GangData>();
                dataSize += FillWorkBuffer(file); CarGenerators = ReadBlock<CarGeneratorData>();
                dataSize += FillWorkBuffer(file); ParticleObjects = ReadBlock<ParticleData>();
                dataSize += FillWorkBuffer(file); AudioScriptObjects = ReadBlock<AudioScriptData>();
                dataSize += FillWorkBuffer(file); PlayerInfo = ReadBlock<PlayerInfo>();
                dataSize += FillWorkBuffer(file); Stats = ReadBlock<Stats>();
                dataSize += FillWorkBuffer(file); Streaming = ReadBlock<Streaming>();
                dataSize += FillWorkBuffer(file); PedTypeInfo = ReadBlock<PedTypeData>();

                numOuterBlocks = NumOuterBlocks;
            }

            // Skip over padding
            int numPaddingBlocks = 0;
            int size = file.Length;
            if (IsXbox) size -= XboxHelper.SignatureLength;
            while (file.Position < size - 4)
            {
                dataSize += FillWorkBuffer(file);
                numPaddingBlocks++;
            }

#if DEBUG
            // Size checks
            int expectedDataSize = SizeOfGameDataInBytes;
            if (IsPS2JP) expectedDataSize -= 4;
            int expectedFileSize = expectedDataSize + ((numOuterBlocks + numPaddingBlocks) * sizeof(int)) + sizeof(int);
            if (IsXbox) expectedFileSize += XboxHelper.SignatureLength;
            Debug.Assert(expectedDataSize == dataSize);
            Debug.Assert(expectedFileSize == file.Length);
#endif
        }

        protected override void Save(DataBuffer file)
        {
            if (IsDE)
            {
                SaveDE(file);
                return;
            }

            int dataSize = 0;
            int size;
            int numOuterBlocks;

            WorkBuff.Reset();
            CheckSum = 0;

            if (IsPS2)
            {
                WorkBuff.Write(SimpleVars, Params);
                WriteBlock(Script);
                WriteBlock(PedPool);
                WriteBlock(Garages);
                WriteBlock(Vehicles);
                dataSize += FlushWorkBuffer(file);

                WriteBlock(Objects);
                WriteBlock(Paths);
                WriteBlock(Cranes);
                dataSize += FlushWorkBuffer(file);

                WriteBlock(Pickups);
                WriteBlock(PhoneInfo);
                WriteBlock(RestartPoints);
                WriteBlock(RadarBlips);
                WriteBlock(Zones);
                WriteBlock(Gangs);
                WriteBlock(CarGenerators);
                WriteBlock(ParticleObjects);
                WriteBlock(AudioScriptObjects);
                WriteBlock(PlayerInfo);
                WriteBlock(Stats);
                WriteBlock(Streaming);
                WriteBlock(PedTypeInfo);
                dataSize += FlushWorkBuffer(file);

                numOuterBlocks = NumOuterBlocksPS2;
            }
            else
            {
                WorkBuff.Write(SimpleVars, Params);
                WriteBlock(Script); dataSize += FlushWorkBuffer(file);
                WriteBlock(PedPool); dataSize += FlushWorkBuffer(file);
                WriteBlock(Garages); dataSize += FlushWorkBuffer(file);
                WriteBlock(Vehicles); dataSize += FlushWorkBuffer(file);
                WriteBlock(Objects); dataSize += FlushWorkBuffer(file);
                WriteBlock(Paths); dataSize += FlushWorkBuffer(file);
                WriteBlock(Cranes); dataSize += FlushWorkBuffer(file);
                WriteBlock(Pickups); dataSize += FlushWorkBuffer(file);
                WriteBlock(PhoneInfo); dataSize += FlushWorkBuffer(file);
                WriteBlock(RestartPoints); dataSize += FlushWorkBuffer(file);
                WriteBlock(RadarBlips); dataSize += FlushWorkBuffer(file);
                WriteBlock(Zones); dataSize += FlushWorkBuffer(file);
                WriteBlock(Gangs); dataSize += FlushWorkBuffer(file);
                WriteBlock(CarGenerators); dataSize += FlushWorkBuffer(file);
                WriteBlock(ParticleObjects); dataSize += FlushWorkBuffer(file);
                WriteBlock(AudioScriptObjects); dataSize += FlushWorkBuffer(file);
                WriteBlock(PlayerInfo); dataSize += FlushWorkBuffer(file);
                WriteBlock(Stats); dataSize += FlushWorkBuffer(file);
                WriteBlock(Streaming); dataSize += FlushWorkBuffer(file);
                WriteBlock(PedTypeInfo); dataSize += FlushWorkBuffer(file);

                numOuterBlocks = NumOuterBlocks;
            }

            int totalDataSize = SizeOfGameDataInBytes;
            if (IsPS2JP) totalDataSize -= 1;

            int numPaddingBlocks = 0;
            for (int i = 0; i < Params.MaxNumPaddingBlocks; i++)
            {
                size = (totalDataSize - dataSize) & 0x7FFFFFFC;
                if (size > Params.WorkBufferSize)
                {
                    size = Params.WorkBufferSize;
                }
                if (size > 4)
                {
                    WorkBuff.Reset();
                    WorkBuff.Pad(size);
                    dataSize += FlushWorkBuffer(file);
                    numPaddingBlocks++;
                }
            }

            file.Write(CheckSum);
            if (IsXbox)
            {
                byte[] data = file.GetBytes();
                byte[] sig = XboxHelper.GenerateSignature(TitleKey, data, 0, data.Length - 4);
                file.Write(sig);
            }

#if DEBUG
            // Size checks
            int expectedDataSize = SizeOfGameDataInBytes;
            if (IsPS2JP) expectedDataSize -= 4;
            int expectedFileSize = expectedDataSize + ((numOuterBlocks + numPaddingBlocks) * sizeof(int)) + sizeof(int);
            if (IsXbox) expectedFileSize += XboxHelper.SignatureLength;
            Debug.Assert(expectedDataSize == dataSize);
            Debug.Assert(expectedFileSize == file.Length);
#endif
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0x24)]
        private struct DEHeader
        {
            public int Field00h;       // 0xff00ff00
            public int Field04h;       // maybe byte-order mark, 0x0000000b or 0xb0000000
            public long Md5Lo;
            public long Md5Hi;
            public int Field18h;       // maybe duplicate byte-order mark?
            public long Field1Ch;      // maybe timestamp
        }

        private class Block : BufferedObject,
            IEquatable<Block>, IDeepClonable<Block>
        {
            public Block() { }
            public Block(Block other) : base(other) { }

            protected override void ReadData(DataBuffer buf, SerializationParams p)
            {
                int size = buf.ReadInt32();
                WorkBuffer = buf.ReadBytes(size);
            }

            protected override void WriteData(DataBuffer buf, SerializationParams p)
            {
                buf.Write(WorkBuffer.Count);
                buf.Write(WorkBuffer);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Block);
            }

            public bool Equals(Block other)
            {
                return other != null && WorkBuffer.SequenceEqual(other.WorkBuffer);
            }

            public Block DeepClone()
            {
                return new Block(this);
            }
        }

        private class PaddingBlock : BufferedObject,
           IEquatable<PaddingBlock>, IDeepClonable<PaddingBlock>
        {
            public PaddingBlock(int size) : base(size) { }
            public PaddingBlock(PaddingBlock other) : base(other) { }

            protected override void ReadData(DataBuffer buf, SerializationParams p)
            {
                int size = WorkBuffer.Count;
                WorkBuffer = buf.ReadBytes(size);
            }

            protected override void WriteData(DataBuffer buf, SerializationParams p)
            {
                buf.Write(WorkBuffer);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as PaddingBlock);
            }

            public bool Equals(PaddingBlock other)
            {
                return other != null && WorkBuffer.SequenceEqual(other.WorkBuffer);
            }

            public PaddingBlock DeepClone()
            {
                return new PaddingBlock(this);
            }
        }

        private DEHeader m_deHeader;
        private List<Block> m_deBlocks;
        private PaddingBlock m_dePadding;

        private T ReadBlockDE<T>(DataBuffer file) where T : SaveDataObject, new()
        {
            long mark = file.Position;
            int size = file.ReadInt32();
            T block = file.ReadObject<T>(Params);

            Debug.Assert(size + 4 == file.Position - mark);

            return block;
        }

        private void WriteBlockDE<T>(DataBuffer file, T block) where T : SaveDataObject, new()
        {
            byte[] buf = Serializer.Write(block, Params);
            file.Write(buf.Length);
            file.Write(buf);
        }

        private void LoadDE(DataBuffer file)
        {
            m_deHeader = file.ReadStruct<DEHeader>();
            SimpleVars = file.ReadObject<SimpleVariables>(Params);
            Script = file.ReadObject<ScriptBlock>(Params);
            PedPool = ReadBlockDE<PedPool>(file);
            Garages = ReadBlockDE<GarageBlock>(file);

            m_deBlocks = new List<Block>();
            for (int i = 3; i < 20; i++)
            {
                m_deBlocks.Add(file.ReadObject<Block>(Params));
            }
            
            m_dePadding = new PaddingBlock(file.Length - file.Position);
            file.ReadObject(m_dePadding, Params);
        }

        private void SaveDE(DataBuffer file)
        {
            DEHeader hdr = new DEHeader
            {
                Field18h = m_deHeader.Field18h,
                Field1Ch = m_deHeader.Field1Ch
            };

            file.Write(hdr);
            file.Write(SimpleVars, Params);
            file.Write(Script, Params);
            WriteBlockDE(file, PedPool);
            WriteBlockDE(file, Garages);

            foreach (var block in m_deBlocks)
            {
                file.Write(block, Params);
            }

            file.Write(m_dePadding, Params);

            using MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(file.GetBuffer());
            
            file.Seek(0);
            file.Write(m_deHeader.Field00h);
            file.Write(m_deHeader.Field04h);
            file.Write(hashBytes);
        }

        protected override void OnFileLoad(string path)
        {
            base.OnFileLoad(path);

            if (IsPS2)
            {
                Title = Path.GetFileName(path);
            }

            if (!IsXbox && !IsPC)
            {
                TimeStamp = File.GetLastWriteTime(path);
            }
        }

        protected override void OnFileSave(string path)
        {
            base.OnFileSave(path);

            if (IsPS2)
            {
                Title = Path.GetFileName(path);
            }
        }

        private bool DetectFileType(byte[] data, out FileType t)
        {
            if (data.Length < 0x10000)
            {
                goto DetectionFailed;
            }

            bool isMobile = false;
            bool isPcOrXbox = false;

            int saveSizeOffset = data.FindFirst(BitConverter.GetBytes(SizeOfGameDataInBytes + 1));
            int saveSizeOffsetJP = data.FindFirst(BitConverter.GetBytes(SizeOfGameDataInBytes));
            int scrOffset = data.FindFirst("SCR\0".GetAsciiBytes());

            int deTitleSize = 0;
            using (DataBuffer buf = new DataBuffer(data))
            {
                buf.Seek(0x24);
                deTitleSize = buf.ReadInt32();
            }

            if ((saveSizeOffset < 0 && saveSizeOffsetJP < 0) || scrOffset < 0)
            {
                goto DetectionFailed;
            }

            if (saveSizeOffset == 0x28 + deTitleSize)
            {
                t = FileTypes.PCDE;
                return true;
            }
            else if (scrOffset == 0xB0 && saveSizeOffset == 0x04)
            {
                t = FileTypes.PS2AU;
                return true;
            }
            else if (scrOffset == 0xB8)
            {
                if (saveSizeOffsetJP == 0x04)
                {
                    t = FileTypes.PS2JP;
                    return true;
                }
                else if (saveSizeOffset == 0x04)
                {
                    t = FileTypes.PS2;
                    return true;
                }
                else if (saveSizeOffset == 0x34)
                {
                    isMobile = true;
                }
            }
            else if (scrOffset == 0xC4 && saveSizeOffset == 0x44)
            {
                isPcOrXbox = true;
            }

            int sizeOfPlayerPed;
            using (DataBuffer s = new DataBuffer(data))
            {
                int block0Size = s.ReadInt32();
                if (block0Size > s.Length) goto DetectionFailed;
                s.Skip(block0Size + sizeof(int));
                int sizeOfPedPool = s.ReadInt32() - sizeof(int);
                int numPlayerPeds = s.ReadInt32();
                sizeOfPlayerPed = sizeOfPedPool / numPlayerPeds;
            }

            if (isMobile)
            {
                if (sizeOfPlayerPed == 0x63E)
                {
                    t = FileTypes.iOS;
                    return true;
                }
                else if (sizeOfPlayerPed == 0x642)
                {
                    t = FileTypes.Android;
                    return true;
                }
            }
            else if (isPcOrXbox)
            {
                if (sizeOfPlayerPed == 0x61A)
                {
                    t = FileTypes.PC;
                    return true;
                }
                else if (sizeOfPlayerPed == 0x61E)
                {
                    t = FileTypes.Xbox;
                    return true;
                }
            }

        DetectionFailed:
            t = FileType.Default;
            return false;
        }

        protected override int GetSize(SerializationParams prm)
        {
            var p = (GTA3SaveParams) prm;
            var t = p.FileType;

            if (p.IsDE)
            {
                throw new NotSupportedException("Definitive Edition not supported yet!");
            }

            int size = 0;

            // data blocks
            size += SizeOf(SimpleVars, prm);
            size += SizeOf(Script, prm) + sizeof(int);
            size += SizeOf(PedPool, prm) + sizeof(int);
            size += SizeOf(Garages, prm) + sizeof(int);
            size += SizeOf(Vehicles, prm) + sizeof(int);
            size += SizeOf(Objects, prm) + sizeof(int);
            size += SizeOf(Paths, prm) + sizeof(int);
            size += SizeOf(Cranes, prm) + sizeof(int);
            size += SizeOf(Pickups, prm) + sizeof(int);
            size += SizeOf(PhoneInfo, prm) + sizeof(int);
            size += SizeOf(RestartPoints, prm) + sizeof(int);
            size += SizeOf(RadarBlips, prm) + sizeof(int);
            size += SizeOf(Zones, prm) + sizeof(int);
            size += SizeOf(Gangs, prm) + sizeof(int);
            size += SizeOf(CarGenerators, prm) + sizeof(int);
            size += SizeOf(ParticleObjects, prm) + sizeof(int);
            size += SizeOf(AudioScriptObjects, prm) + sizeof(int);
            size += SizeOf(PlayerInfo, prm) + sizeof(int);
            size += SizeOf(Stats, prm) + sizeof(int);
            size += SizeOf(Streaming, prm) + sizeof(int);
            size += SizeOf(PedTypeInfo, prm) + sizeof(int);

            // padding blocks
            int totalDataSize = SizeOfGameDataInBytes;
            if (!p.IsPS2JP) totalDataSize += 1;
            int numRemaining = ((totalDataSize - 1) & 0x7FFFFFFC) - size;
            int numPadding = (numRemaining / (p.WorkBufferSize - sizeof(int))) + 1;
            size += numRemaining;

            // "outer" block sizes
            if (t.IsPS2) size += sizeof(int) * (NumOuterBlocksPS2 + numPadding);
            else size += sizeof(int) * (NumOuterBlocks + numPadding);

            // checksum
            size += sizeof(int);
            
            // Xbox signature
            if (t.IsXbox) size += XboxHelper.SignatureLength;

            return size;
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

            return SimpleVars.Equals(other.SimpleVars)
                && Script.Equals(other.Script)
                && PedPool.Equals(other.PedPool)
                && Garages.Equals(other.Garages)
                && Vehicles.Equals(other.Vehicles)
                && Objects.Equals(other.Objects)
                && Paths.Equals(other.Paths)
                && Cranes.Equals(other.Cranes)
                && Pickups.Equals(other.Pickups)
                && PhoneInfo.Equals(other.PhoneInfo)
                && RestartPoints.Equals(other.RestartPoints)
                && RadarBlips.Equals(other.RadarBlips)
                && Zones.Equals(other.Zones)
                && Gangs.Equals(other.Gangs)
                && CarGenerators.Equals(other.CarGenerators)
                && ParticleObjects.Equals(other.ParticleObjects)
                && AudioScriptObjects.Equals(other.AudioScriptObjects)
                && PlayerInfo.Equals(other.PlayerInfo)
                && Stats.Equals(other.Stats)
                && Streaming.Equals(other.Streaming)
                && PedTypeInfo.Equals(other.PedTypeInfo);
        }

        public GTA3Save DeepClone()
        {
            return new GTA3Save(this);
        }

        /// <summary>
        /// Supported <see cref="FileType"/>s for GTA3 saves.
        /// </summary>
        public static class FileTypes
        {
            public static readonly FileType Android = new FileType(
                nameof(Android),
                GameVersionId.Android
            );

            public static readonly FileType iOS = new FileType(
                nameof(iOS),
                GameVersionId.iOS
            );

            public static readonly FileType PC = new FileType(
                nameof(PC),
                GameVersionId.Windows
            );

            public static readonly FileType PCDE = new FileType(
                nameof(PCDE),
                GameVersionId.DefinitiveEdition
            );

            public static readonly FileType PS2 = new FileType(
                nameof(PS2),
                GameVersionId.PS2
            );

            public static readonly FileType PS2AU = new FileType(
                nameof(PS2AU),
                GTA3SaveParams.FlagPS2AU,
                GameVersionId.PS2
            );

            public static readonly FileType PS2JP = new FileType(
                nameof(PS2JP),
                GTA3SaveParams.FlagPS2JP,
                GameVersionId.PS2
            );

            public static readonly FileType Xbox = new FileType(
                nameof(Xbox),
                GameVersionId.Xbox
            );

            public static FileType[] GetAll()
            {
                return new FileType[] { Android, iOS, PC, PCDE, PS2, PS2AU, PS2JP, Xbox };
            }
        }

        [JsonIgnore] public bool IsAndroid => Params.IsAndroid;
        [JsonIgnore] public bool IsiOS => Params.IsiOS;
        [JsonIgnore] public bool IsMobile => Params.IsMobile;
        [JsonIgnore] public bool IsPC => Params.IsPC;
        [JsonIgnore] public bool IsPS2 => Params.IsPS2;
        [JsonIgnore] public bool IsPS2JP => Params.IsPS2JP;
        [JsonIgnore] public bool IsPS2AU => Params.IsPS2AU;
        [JsonIgnore] public bool IsXbox => Params.IsXbox;
        [JsonIgnore] public bool IsDE => Params.IsDE;


        private static readonly byte[] TitleKey =
        {
            0xFF, 0x3B, 0x8F, 0x5C, 0xDE, 0x53, 0xF3, 0x25,
            0x9E, 0x70, 0x09, 0x54, 0xEF, 0xDC, 0xA8, 0xDC
        };
    }
}
