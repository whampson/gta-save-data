using GTASaveData.Helpers;
using GTASaveData.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData.VC
{
    /// <summary>
    /// Represents a Grand Theft Auto Vice City save data file.
    /// </summary>
    public sealed class VCSave : SaveDataFile,
        IEquatable<VCSave>
    {
        public static class FileFormats
        {
            // TODO: revamp this a bit
            public static readonly FileFormat PCRetail = new FileFormat(
                "Retail (Windows) or Steam (macOS)", ConsoleType.PC
            );

            public static readonly FileFormat PCSteam = new FileFormat(
                "Retail (Windows) or Steam (macOS)", ConsoleType.PC, ConsoleFlags.Steam
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { PCRetail, PCSteam };
            }
        }

        private static readonly Dictionary<FileFormat, int> SimpleVarsSize = new Dictionary<FileFormat, int>
        {
            { FileFormats.PCRetail, 0xE4 },
            { FileFormats.PCSteam, 0xE8 },
        };

        private static readonly Dictionary<FileFormat, int> MaxBlockSize = new Dictionary<FileFormat, int>
        {
            // TODO: verify
            { FileFormats.PCRetail, 0xD6D8 },
            { FileFormats.PCSteam, 0xD6D8 },
        };

        // The number of bytes in all first-level blocks, excluding the size header.
        private const int TotalBlockDataSize = 0x31400;

        // Block IDs for tagged blocks.
        private const string ScrTag = "SCR";
        private const string RstTag = "RST";
        private const string RdrTag = "RDR";
        private const string ZnsTag = "ZNS";
        private const string GngTag = "GNG";
        private const string CgnTag = "CGN";
        private const string AudTag = "AUD";
        private const string PtpTag = "PTP";

        private SimpleVars m_simpleVars;
        private DummyObject m_scripts;
        private DummyObject m_pedPool;
        private DummyObject m_garages;
        private DummyObject m_gameLogic;
        private DummyObject m_vehiclePool;
        private DummyObject m_objectPool;
        private DummyObject m_pathFind;         // maybe
        private DummyObject m_cranes;
        private DummyObject m_pickups;
        private DummyObject m_phoneInfo;
        private DummyObject m_restartPoints;
        private DummyObject m_radarBlips;
        private DummyObject m_zones;
        private DummyObject m_gangData;
        private DummyObject m_carGenerators;
        private DummyObject m_particles;        // maybe
        private DummyObject m_audioScriptObjects;
        private DummyObject m_scriptPaths;
        private DummyObject m_playerInfo;
        private DummyObject m_stats;
        private DummyObject m_setPieces;
        private DummyObject m_streaming;
        private DummyObject m_pedTypeInfo;

        public SimpleVars SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
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
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public DummyObject Vehicles
        {
            get { return m_vehiclePool; }
            set { m_vehiclePool = value; OnPropertyChanged(); }
        }

        public DummyObject Objects
        {
            get { return m_objectPool; }
            set { m_objectPool = value; OnPropertyChanged(); }
        }

        public DummyObject PathFind
        {
            get { return m_pathFind; }
            set { m_pathFind = value; OnPropertyChanged(); }
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

        public DummyObject Restarts
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

        public DummyObject Particles
        {
            get { return m_particles; }
            set { m_particles = value; OnPropertyChanged(); }
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

        public VCSave()
        {
            m_simpleVars = new SimpleVars();
            m_scripts = new DummyObject();
            m_pedPool = new DummyObject();
            m_garages = new DummyObject();
            m_gameLogic = new DummyObject();
            m_vehiclePool = new DummyObject();
            m_objectPool = new DummyObject();
            m_pathFind = new DummyObject();
            m_cranes = new DummyObject();
            m_pickups = new DummyObject();
            m_phoneInfo = new DummyObject();
            m_restartPoints = new DummyObject();
            m_radarBlips = new DummyObject();
            m_zones = new DummyObject();
            m_gangData = new DummyObject();
            m_carGenerators = new DummyObject();
            m_particles = new DummyObject();
            m_audioScriptObjects = new DummyObject();
            m_scriptPaths = new DummyObject();
            m_playerInfo = new DummyObject();
            m_stats = new DummyObject();
            m_setPieces = new DummyObject();
            m_streaming = new DummyObject();
            m_pedTypeInfo = new DummyObject();
        }

        //public static FileFormat GetFileFormat(string path)
        //{
        //    if (path == null)
        //    {
        //        return null;
        //    }

        //    bool isMobile = false;
        //    bool isPcOrXbox = false;

        //    byte[] data = File.ReadAllBytes(path);

        //    int fileId = data.FindFirst(BitConverter.GetBytes(0x31401));
        //    int fileIdJP = data.FindFirst(BitConverter.GetBytes(0x31400));
        //    int scr = data.FindFirst("SCR\0".GetAsciiBytes());

        //    int blk1Size;
        //    using (SaveDataSerializer s = new SaveDataSerializer(new MemoryStream(data)))
        //    {
        //        s.Skip(s.ReadInt32());
        //        blk1Size = s.ReadInt32();
        //    }

        //    if (scr == 0xB0 && fileId == 0x04)
        //    {
        //        // PS2, Austra
        //        return FileFormats.PS2AU;
        //    }
        //    else if (scr == 0xB8)
        //    {
        //        if (fileIdJP == 0x04)
        //        {
        //            // PS2, Japan
        //            return FileFormats.PS2JP;
        //        }
        //        else if (fileId == 0x04)
        //        {
        //            // PS2, North America/Europe
        //            return FileFormats.PS2NAEU;
        //        }
        //        else if (fileId == 0x34)
        //        {
        //            isMobile = true;
        //        }
        //    }
        //    else if (scr == 0xC4 && fileId == 0x44)
        //    {
        //        isPcOrXbox = true;
        //    }

        //    if (isMobile)
        //    {
        //        if (blk1Size == 0x648)
        //        {
        //            // iOS
        //            return FileFormats.IOS;
        //        }
        //        else if (blk1Size == 0x64C)
        //        {
        //            // Android
        //            return FileFormats.Android;
        //        }
        //    }
        //    else if (isPcOrXbox)
        //    {
        //        if (blk1Size == 0x624)
        //        {
        //            // PC (Windows, macOS)
        //            return FileFormats.PC;
        //        }
        //        else if (blk1Size == 0x628)
        //        {
        //            // Xbox
        //            return FileFormats.Xbox;
        //        }
        //    }

        //    return null;
        //}

        //public static VCSave Load(string path)
        //{
        //    return Load(path, GetFileFormat(path));
        //}

        public static VCSave Load(string path, FileFormat format)
        {
            if (path == null || format == null)
            {
                return null;
            }

            byte[] data = File.ReadAllBytes(path);
            using (MemoryStream m = new MemoryStream(data))
            {
                using (SaveDataSerializer s = new SaveDataSerializer(m))
                {
                    return s.ReadObject<VCSave>(format);
                }
            }
        }

        private VCSave(SaveDataSerializer serializer, FileFormat format)
        {
            if (!FileFormats.GetAll().Contains(format))
            {
                throw new SaveDataSerializationException(
                    string.Format("'{0}' is not a valid file format for GTA3 save data.", format));
            }

            CurrentFormat = format;

            int outerBlockCount = 0;
            bool doneReading = false;

            ByteBuffer simpleVars = null;
            ByteBuffer scripts = null;
            ByteBuffer pedPool = null;
            ByteBuffer garages = null;
            ByteBuffer gameLogic = null;
            ByteBuffer vehiclePool = null;
            ByteBuffer objectPool = null;
            ByteBuffer pathFind = null;
            ByteBuffer cranes = null;
            ByteBuffer pickups = null;
            ByteBuffer phoneInfo = null;
            ByteBuffer restartPoints = null;
            ByteBuffer radarBlips = null;
            ByteBuffer zones = null;
            ByteBuffer gangData = null;
            ByteBuffer carGenerators = null;
            ByteBuffer particles = null;
            ByteBuffer audioScriptObjects = null;
            ByteBuffer scriptPaths = null;
            ByteBuffer playerInfo = null;
            ByteBuffer stats = null;
            ByteBuffer setPieces = null;
            ByteBuffer streaming = null;
            ByteBuffer pedTypeInfo = null;
            ByteBuffer tmp;

            while (!doneReading)
            {
                tmp = ReadBlock(serializer, null);
                using (SaveDataSerializer blockStream = CreateSerializer(new MemoryStream(tmp)))
                {
                    switch (outerBlockCount)
                    {
                        case 0:
                            simpleVars = blockStream.ReadBytes(SimpleVarsSize[format]);
                            scripts = ReadBlock(blockStream, ScrTag);
                            break;
                        case 1: pedPool = ReadBlock(blockStream); break;
                        case 2: garages = ReadBlock(blockStream); break;
                        case 3: gameLogic = ReadBlock(blockStream); break;
                        case 4: vehiclePool = ReadBlock(blockStream); break;
                        case 5: objectPool = ReadBlock(blockStream); break;
                        case 6: pathFind = ReadBlock(blockStream); break;
                        case 7: cranes = ReadBlock(blockStream); break;
                        case 8: pickups = ReadBlock(blockStream); break;
                        case 9: phoneInfo = ReadBlock(blockStream); break;
                        case 10: restartPoints = ReadBlock(blockStream, RstTag); break;
                        case 11: radarBlips = ReadBlock(blockStream, RdrTag); break;
                        case 12: zones = ReadBlock(blockStream, ZnsTag); break;
                        case 13: gangData = ReadBlock(blockStream, GngTag); break;
                        case 14: carGenerators = ReadBlock(blockStream, CgnTag); break;
                        case 15: particles = ReadBlock(blockStream); break;
                        case 16: audioScriptObjects = ReadBlock(blockStream, AudTag); break;
                        case 17: scriptPaths = ReadBlock(blockStream); break;
                        case 18: playerInfo = ReadBlock(blockStream); break;
                        case 19: stats = ReadBlock(blockStream); break;
                        case 20: setPieces = ReadBlock(blockStream); break;
                        case 21: streaming = ReadBlock(blockStream); break;
                        case 22:
                            pedTypeInfo = ReadBlock(blockStream, PtpTag);
                            doneReading = true;
                            break;
                    }
                }

                outerBlockCount++;
            }

            m_simpleVars = Deserialize<SimpleVars>(simpleVars, format);
            m_scripts = new DummyObject(scripts);
            m_pedPool = new DummyObject(pedPool);
            m_garages = new DummyObject(garages);
            m_gameLogic = new DummyObject(gameLogic);
            m_vehiclePool  = new DummyObject(vehiclePool);
            m_objectPool = new DummyObject(objectPool);
            m_pathFind = new DummyObject(pathFind);
            m_cranes = new DummyObject(cranes);
            m_pickups = new DummyObject(pickups);
            m_phoneInfo = new DummyObject(phoneInfo);
            m_restartPoints = new DummyObject(restartPoints);
            m_radarBlips = new DummyObject(radarBlips);
            m_zones = new DummyObject(zones);
            m_gangData = new DummyObject(gangData);
            m_carGenerators = new DummyObject(carGenerators);
            m_particles = new DummyObject(particles);
            m_audioScriptObjects = new DummyObject(audioScriptObjects);
            m_scriptPaths = new DummyObject(scriptPaths);
            m_playerInfo = new DummyObject(playerInfo);
            m_stats = new DummyObject(stats);
            m_setPieces = new DummyObject(setPieces);
            m_streaming = new DummyObject(streaming);
            m_pedTypeInfo = new DummyObject(pedTypeInfo);
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            CurrentFormat = format;

            int dataBytesWritten = 0;
            int blockCount = 0;
            int checksum = 0;

            ByteBuffer simpleVars = Serialize(m_simpleVars, format);
            ByteBuffer scripts = CreateBlock(ScrTag, Serialize(m_scripts, format));
            ByteBuffer pedPool = CreateBlock(Serialize(m_pedPool, format));
            ByteBuffer garages = CreateBlock(Serialize(m_garages, format));
            ByteBuffer gameLogic = CreateBlock(Serialize(m_gameLogic, format));
            ByteBuffer vehiclePool = CreateBlock(Serialize(m_vehiclePool, format));
            ByteBuffer objectPool = CreateBlock(Serialize(m_objectPool, format));
            ByteBuffer pathFind = CreateBlock(Serialize(m_pathFind, format));
            ByteBuffer cranes = CreateBlock(Serialize(m_cranes, format));
            ByteBuffer pickups = CreateBlock(Serialize(m_pickups, format));
            ByteBuffer phoneInfo = CreateBlock(Serialize(m_phoneInfo, format));
            ByteBuffer restartPoints = CreateBlock(RstTag, Serialize(m_restartPoints, format));
            ByteBuffer radarBlips = CreateBlock(RdrTag, Serialize(m_radarBlips, format));
            ByteBuffer zones = CreateBlock(ZnsTag, Serialize(m_zones, format));
            ByteBuffer gangData = CreateBlock(GngTag, Serialize(m_gangData, format));
            ByteBuffer carGenerators = CreateBlock(CgnTag, Serialize(m_carGenerators, format));
            ByteBuffer particles = CreateBlock(Serialize(m_particles, format));
            ByteBuffer audioScriptObjects = CreateBlock(AudTag, Serialize(m_audioScriptObjects, format));
            ByteBuffer scriptPaths = CreateBlock(Serialize(m_scriptPaths, format));
            ByteBuffer playerInfo = CreateBlock(Serialize(m_playerInfo, format));
            ByteBuffer stats = CreateBlock(Serialize(m_stats, format));
            ByteBuffer setPieces = CreateBlock(Serialize(m_setPieces, format));
            ByteBuffer streaming = CreateBlock(Serialize(m_streaming, format));
            ByteBuffer pedTypeInfo = CreateBlock(PtpTag, Serialize(m_pedTypeInfo, format));

            while (dataBytesWritten < TotalBlockDataSize)
            {
                ByteBuffer payload = new ByteBuffer(0);
                bool padding = false;

                switch (blockCount)
                {
                    case 0: payload = CreateBlock(simpleVars, scripts); break;
                    case 1: payload = CreateBlock(pedPool); break;
                    case 2: payload = CreateBlock(garages); break;
                    case 3: payload = CreateBlock(gameLogic); break;
                    case 4: payload = CreateBlock(vehiclePool); break;
                    case 5: payload = CreateBlock(objectPool); break;
                    case 6: payload = CreateBlock(pathFind); break;
                    case 7: payload = CreateBlock(cranes); break;
                    case 8: payload = CreateBlock(pickups); break;
                    case 9: payload = CreateBlock(phoneInfo); break;
                    case 10: payload = CreateBlock(restartPoints); break;
                    case 11: payload = CreateBlock(radarBlips); break;
                    case 12: payload = CreateBlock(zones); break;
                    case 13: payload = CreateBlock(gangData); break;
                    case 14: payload = CreateBlock(carGenerators); break;
                    case 15: payload = CreateBlock(particles); break;
                    case 16: payload = CreateBlock(audioScriptObjects); break;
                    case 17: payload = CreateBlock(scriptPaths); break;
                    case 18: payload = CreateBlock(playerInfo); break;
                    case 19: payload = CreateBlock(stats); break;
                    case 20: payload = CreateBlock(setPieces); break;
                    case 21: payload = CreateBlock(streaming); break;
                    case 22: payload = CreateBlock(pedTypeInfo); break;
                    default:
                        padding = true;
                        break;
                }

                if (padding)
                {
                    int length = TotalBlockDataSize - dataBytesWritten;
                    int maxBlockSize = MaxBlockSize[format];
                    if (length > maxBlockSize)
                    {
                        length = maxBlockSize;
                    }
                    payload = CreatePaddingBlock(length);
                }

                serializer.Write(payload);
                dataBytesWritten += payload.Length - 4; // Block sizes are not counted 
                checksum += payload.ToArray().Sum(x => x);
                blockCount++;
            }

            serializer.Write(checksum);

            Debug.Assert(dataBytesWritten == TotalBlockDataSize);
            Debug.Assert(serializer.BaseStream.Position == TotalBlockDataSize + (blockCount * 4) + 4);
        }

        // TODO: move to base class 
        private byte[] CreateBlock(params ByteBuffer[] chunks)
        {
            return CreateBlock(null, chunks);
        }

        private byte[] CreateBlock(string tag, params ByteBuffer[] chunks)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (SaveDataSerializer s = CreateSerializer(m))
                {
                    WriteBlock(s, tag, chunks);
                }

                return m.ToArray();
            }
        }

        private byte[] CreatePaddingBlock(int length)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (SaveDataSerializer s = CreateSerializer(m))
                {
                    s.WritePadding(length);
                }

                return CreateBlock(m.ToArray());
            }
        }

        private ByteBuffer ReadBlock(SaveDataSerializer s, string tag = null)
        {
            int length = s.ReadInt32();
            Debug.WriteLineIf(length > MaxBlockSize[CurrentFormat], "Maximum allowed block size exceeded!");

            if (tag != null)
            {
                string str = s.ReadString(4);
                int dataLength = s.ReadInt32();
                Debug.Assert(str == tag, "Invalid block tag!", "Expected: {0}, Actual: {1}", tag, str);
                Debug.Assert(dataLength == length - 8);
                length = dataLength;
            }

            byte[] data = s.ReadBytes(length);
            s.Align();

            return data;
        }

        private int WriteBlock(SaveDataSerializer s, string tag, params ByteBuffer[] chunks)
        {
            long mark = s.BaseStream.Position;

            int payloadSize = chunks.Sum(x => x.Length);
            int totalSize = (tag != null) ? (payloadSize + 8) : payloadSize;
            Debug.WriteLineIf(totalSize > MaxBlockSize[CurrentFormat], "Maximum allowed block size exceeded!");

            if (tag != null)
            {
                s.Write(payloadSize + 8);
                s.Write(tag, 4);
            }
            s.Write(payloadSize);
            foreach (ByteBuffer chunk in chunks)
            {
                s.Write((byte[]) chunk);
            }
            s.Align();

            return (int) (s.BaseStream.Position - mark);
        }
        // End TODO

        public override IList<SaveDataObject> GetAllBlocks()
        {
            return new SaveDataObject[]
            {
                m_simpleVars,
                m_scripts,
                m_pedPool,
                m_garages,
                m_gameLogic,
                m_vehiclePool,
                m_objectPool,
                m_pathFind,
                m_cranes,
                m_pickups,
                m_phoneInfo,
                m_restartPoints,
                m_radarBlips,
                m_zones,
                m_gangData,
                m_carGenerators,
                m_particles,
                m_audioScriptObjects,
                m_scriptPaths,
                m_playerInfo,
                m_stats,
                m_setPieces,
                m_streaming,
                m_pedTypeInfo
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(VCSave other)
        {
            if (other == null)
            {
                return false;
            }

            return m_simpleVars.Equals(other.m_simpleVars)
                && m_scripts.Equals(other.m_scripts)
                && m_pedPool.Equals(other.m_pedPool)
                && m_garages.Equals(other.m_garages)
                && m_gameLogic.Equals(other.m_gameLogic)
                && m_vehiclePool.Equals(other.m_vehiclePool)
                && m_objectPool.Equals(other.m_objectPool)
                && m_pathFind.Equals(other.m_pathFind)
                && m_cranes.Equals(other.m_cranes)
                && m_pickups.Equals(other.m_pickups)
                && m_phoneInfo.Equals(other.m_phoneInfo)
                && m_restartPoints.Equals(other.m_restartPoints)
                && m_radarBlips.Equals(other.m_radarBlips)
                && m_zones.Equals(other.m_zones)
                && m_gangData.Equals(other.m_gangData)
                && m_carGenerators.Equals(other.m_carGenerators)
                && m_particles.Equals(other.m_particles)
                && m_audioScriptObjects.Equals(other.m_audioScriptObjects)
                && m_scriptPaths.Equals(other.m_scriptPaths)
                && m_playerInfo.Equals(other.m_playerInfo)
                && m_stats.Equals(other.m_stats)
                && m_setPieces.Equals(other.m_setPieces)
                && m_streaming.Equals(other.m_streaming)
                && m_pedTypeInfo.Equals(other.m_pedTypeInfo);
        }

        public override string ToString()
        {
            return string.Format("VCSave: [ SaveName = {0} ]", SimpleVars.LastMissionPassedName);
        }

        public sealed class DummyObject : SaveDataObject,
            IEquatable<DummyObject>
        {
            public byte[] Data
            {
                get;
                set;
            }

            public DummyObject()
                : this(new byte[0])
            { }

            public DummyObject(byte[] data)
            {
                Data = data;
            }

            private DummyObject(SaveDataSerializer serializer, FileFormat format)
            {
                // nop
            }

            protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
            {
                serializer.Write(Data);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as DummyObject);
            }

            public bool Equals(DummyObject other)
            {
                if (other == null)
                {
                    return false;
                }

                return Data.SequenceEqual(other.Data);
            }
        }
    }
}
