using GTASaveData.Extensions;
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
    public sealed class VCSave : SaveData,
        IEquatable<VCSave>
    {
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
        private Block m_scripts;
        private Block m_pedPool;
        private Block m_garages;
        private Block m_gameLogic;
        private Block m_vehiclePool;
        private Block m_objectPool;
        private Block m_pathFind;         // maybe
        private Block m_cranes;
        private Block m_pickups;
        private Block m_phoneInfo;
        private Block m_restartPoints;
        private Block m_radarBlips;
        private Block m_zones;
        private Block m_gangData;
        private CarGeneratorsBlock m_carGenerators;
        private Block m_particles;        // maybe
        private Block m_audioScriptObjects;
        private Block m_scriptPaths;
        private Block m_playerInfo;
        private Block m_stats;
        private Block m_setPieces;
        private Block m_streaming;
        private Block m_pedTypeInfo;

        public SimpleVars SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public Block Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; OnPropertyChanged(); }
        }

        public Block PedPool
        {
            get { return m_pedPool; }
            set { m_pedPool = value; OnPropertyChanged(); }
        }

        public Block Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public Block GameLogic
        {
            get { return m_gameLogic; }
            set { m_gameLogic = value; OnPropertyChanged(); }
        }

        public Block Vehicles
        {
            get { return m_vehiclePool; }
            set { m_vehiclePool = value; OnPropertyChanged(); }
        }

        public Block Objects
        {
            get { return m_objectPool; }
            set { m_objectPool = value; OnPropertyChanged(); }
        }

        public Block PathFind
        {
            get { return m_pathFind; }
            set { m_pathFind = value; OnPropertyChanged(); }
        }

        public Block Cranes
        {
            get { return m_cranes; }
            set { m_cranes = value; OnPropertyChanged(); }
        }

        public Block Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
        }

        public Block PhoneInfo
        {
            get { return m_phoneInfo; }
            set { m_phoneInfo = value; OnPropertyChanged(); }
        }

        public Block Restarts
        {
            get { return m_restartPoints; }
            set { m_restartPoints = value; OnPropertyChanged(); }
        }

        public Block RadarBlips
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; OnPropertyChanged(); }
        }

        public Block Zones
        {
            get { return m_zones; }
            set { m_zones = value; OnPropertyChanged(); }
        }

        public Block GangData
        {
            get { return m_gangData; }
            set { m_gangData = value; OnPropertyChanged(); }
        }

        public CarGeneratorsBlock CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; OnPropertyChanged(); }
        }

        public Block Particles
        {
            get { return m_particles; }
            set { m_particles = value; OnPropertyChanged(); }
        }

        public Block AudioScriptObjects
        {
            get { return m_audioScriptObjects; }
            set { m_audioScriptObjects = value; OnPropertyChanged(); }
        }

        public Block ScriptPaths
        {
            get { return m_scriptPaths; }
            set { m_scriptPaths = value; OnPropertyChanged(); }
        }

        public Block PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; OnPropertyChanged(); }
        }

        public Block Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public Block SetPieces
        {
            get { return m_setPieces; }
            set { m_setPieces = value; OnPropertyChanged(); }
        }

        public Block Streaming
        {
            get { return m_streaming; }
            set { m_streaming = value; OnPropertyChanged(); }
        }

        public Block PedTypeInfo
        {
            get { return m_pedTypeInfo; }
            set { m_pedTypeInfo = value; OnPropertyChanged(); }
        }

        public override string Name => SimpleVars.LastMissionPassedName;

        public override IReadOnlyList<Chunk> Blocks => new List<Chunk>
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
        }.AsReadOnly();

        protected override Dictionary<FileFormat, int> MaxBlockSize => new Dictionary<FileFormat, int>
        {
            // TODO: verify
            { FileFormats.PCRetail, 0xD6D8 },
            { FileFormats.PCSteam, 0xD6D8 },
            //{ FileFormats.PS2, 0xD6D8 },
        };

        protected override Dictionary<FileFormat, int> SimpleVarsSize => new Dictionary<FileFormat, int>
        {
            { FileFormats.PCRetail, 0xE4 },
            { FileFormats.PCSteam, 0xE8 },
            //{ FileFormats.PS2, 0x1C0 },
        };

        public VCSave()
        {
            m_simpleVars = new SimpleVars();
            m_scripts = new Block();
            m_pedPool = new Block();
            m_garages = new Block();
            m_gameLogic = new Block();
            m_vehiclePool = new Block();
            m_objectPool = new Block();
            m_pathFind = new Block();
            m_cranes = new Block();
            m_pickups = new Block();
            m_phoneInfo = new Block();
            m_restartPoints = new Block();
            m_radarBlips = new Block();
            m_zones = new Block();
            m_gangData = new Block();
            m_carGenerators = new CarGeneratorsBlock();
            m_particles = new Block();
            m_audioScriptObjects = new Block();
            m_scriptPaths = new Block();
            m_playerInfo = new Block();
            m_stats = new Block();
            m_setPieces = new Block();
            m_streaming = new Block();
            m_pedTypeInfo = new Block();
        }

        private VCSave(Serializer serializer, FileFormat format)
        {
            if (!FileFormats.GetAll().Contains(format))
            {
                throw new SerializationException(
                    string.Format("'{0}' is not a valid file format for GTA VC save data.", format));
            }

            FileFormat = format;

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
                using (Serializer blockStream = CreateSerializer(new MemoryStream(tmp)))
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

            m_simpleVars = ReadData<SimpleVars>(simpleVars, format);
            m_scripts = new Block(scripts);
            m_pedPool = new Block(pedPool);
            m_garages = new Block(garages);
            m_gameLogic = new Block(gameLogic);
            m_vehiclePool  = new Block(vehiclePool);
            m_objectPool = new Block(objectPool);
            m_pathFind = new Block(pathFind);
            m_cranes = new Block(cranes);
            m_pickups = new Block(pickups);
            m_phoneInfo = new Block(phoneInfo);
            m_restartPoints = new Block(restartPoints);
            m_radarBlips = new Block(radarBlips);
            m_zones = new Block(zones);
            m_gangData = new Block(gangData);
            m_carGenerators = ReadData<CarGeneratorsBlock>(carGenerators);
            m_particles = new Block(particles);
            m_audioScriptObjects = new Block(audioScriptObjects);
            m_scriptPaths = new Block(scriptPaths);
            m_playerInfo = new Block(playerInfo);
            m_stats = new Block(stats);
            m_setPieces = new Block(setPieces);
            m_streaming = new Block(streaming);
            m_pedTypeInfo = new Block(pedTypeInfo);
        }

        protected override void WriteObjectData(Serializer serializer, FileFormat format)
        {
            FileFormat = format;

            int dataBytesWritten = 0;
            int blockCount = 0;
            int checksum = 0;

            ByteBuffer simpleVars = WriteData(m_simpleVars, format);
            ByteBuffer scripts = CreateBlock(ScrTag, WriteData(m_scripts, format));
            ByteBuffer pedPool = CreateBlock(WriteData(m_pedPool, format));
            ByteBuffer garages = CreateBlock(WriteData(m_garages, format));
            ByteBuffer gameLogic = CreateBlock(WriteData(m_gameLogic, format));
            ByteBuffer vehiclePool = CreateBlock(WriteData(m_vehiclePool, format));
            ByteBuffer objectPool = CreateBlock(WriteData(m_objectPool, format));
            ByteBuffer pathFind = CreateBlock(WriteData(m_pathFind, format));
            ByteBuffer cranes = CreateBlock(WriteData(m_cranes, format));
            ByteBuffer pickups = CreateBlock(WriteData(m_pickups, format));
            ByteBuffer phoneInfo = CreateBlock(WriteData(m_phoneInfo, format));
            ByteBuffer restartPoints = CreateBlock(RstTag, WriteData(m_restartPoints, format));
            ByteBuffer radarBlips = CreateBlock(RdrTag, WriteData(m_radarBlips, format));
            ByteBuffer zones = CreateBlock(ZnsTag, WriteData(m_zones, format));
            ByteBuffer gangData = CreateBlock(GngTag, WriteData(m_gangData, format));
            ByteBuffer carGenerators = CreateBlock(CgnTag, WriteData(m_carGenerators, format));
            ByteBuffer particles = CreateBlock(WriteData(m_particles, format));
            ByteBuffer audioScriptObjects = CreateBlock(AudTag, WriteData(m_audioScriptObjects, format));
            ByteBuffer scriptPaths = CreateBlock(WriteData(m_scriptPaths, format));
            ByteBuffer playerInfo = CreateBlock(WriteData(m_playerInfo, format));
            ByteBuffer stats = CreateBlock(WriteData(m_stats, format));
            ByteBuffer setPieces = CreateBlock(WriteData(m_setPieces, format));
            ByteBuffer streaming = CreateBlock(WriteData(m_streaming, format));
            ByteBuffer pedTypeInfo = CreateBlock(PtpTag, WriteData(m_pedTypeInfo, format));

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

                serializer.Write((byte[]) payload);
                dataBytesWritten += payload.Length - 4; // Block sizes are not counted 
                checksum += payload.ToArray().Sum(x => x);
                blockCount++;
            }

            serializer.Write(checksum);

            Debug.Assert(dataBytesWritten == TotalBlockDataSize);
            Debug.Assert(serializer.BaseStream.Position == TotalBlockDataSize + (blockCount * 4) + 4);
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

        protected override FileFormat DetectFileFormat(string path)
        {
            if (path == null)
            {
                return null;
            }

            byte[] data = File.ReadAllBytes(path);
            int blockCount = GetBlockCount(data);

            int fileId = data.FindFirst(BitConverter.GetBytes(0x31401));
            int scr = data.FindFirst("SCR\0".GetAsciiBytes());

            // Likely PS2 if block count is 8

            if (fileId == 0x44)
            {
                if (scr == 0xEC)
                {
                    return FileFormats.PCRetail;
                }
                else if (scr == 0xF0)
                {
                    return FileFormats.PCSteam;
                }
            }

            return null;
        }

        public static class FileFormats
        {
            // TODO: revamp this a bit
            public static readonly FileFormat PCRetail = new FileFormat(
                "PC (Retail)", ConsoleType.PC
            );

            public static readonly FileFormat PCSteam = new FileFormat(
                "PC (Steam)", ConsoleType.PC, ConsoleFlags.Steam
            );

            public static readonly FileFormat PS2 = new FileFormat(
                "PS2", ConsoleType.PS2
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { PCRetail, PCSteam, PS2 };
            }
        }
    }
}
