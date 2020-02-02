using GTASaveData.Extensions;
using GTASaveData.Helpers;
using GTASaveData.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Represents a Grand Theft Auto III save data file.
    /// </summary>
    /// <remarks>
    /// File structure consists of multiple nested data blocks, each
    /// block beginning with the number of bytes as a 32-bit integer
    /// which is sometimes preceded by a 4-byte ID string.
    /// </remarks>
    public sealed class GTA3Save : SaveData,
        IEquatable<GTA3Save>
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
        private Scripts m_scripts;
        private GenericBlock m_pedPool;
        private Garages m_garages;
        private VehiclePool m_vehicles;
        private GenericBlock m_objects;
        private GenericBlock m_pathFind;
        private GenericBlock m_cranes;
        private Pickups m_pickups;
        private GenericBlock m_phoneInfo;
        private GenericBlock m_restarts;
        private GenericBlock m_radarBlips;
        private GenericBlock m_zones;
        private GenericBlock m_gangData;
        private CarGeneratorsBlock m_carGenerators;
        private GenericBlock m_particles;
        private GenericBlock m_audioScriptObjects;
        private GenericBlock m_playerInfo;
        private GenericBlock m_stats;
        private GenericBlock m_streaming;
        private GenericBlock m_pedTypeInfo;

        public SimpleVars SimpleVars
        {
            get { return m_simpleVars; }
            set { m_simpleVars = value; OnPropertyChanged(); }
        }

        public Scripts Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; OnPropertyChanged(); }
        }

        public GenericBlock PedPool
        {
            get { return m_pedPool; }
            set { m_pedPool = value; OnPropertyChanged(); }
        }

        public Garages Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public VehiclePool Vehicles
        {
            get { return m_vehicles; }
            set { m_vehicles = value; OnPropertyChanged(); }
        }

        public GenericBlock Objects
        {
            get { return m_objects; }
            set { m_objects = value; OnPropertyChanged(); }
        }

        public GenericBlock PathFind
        {
            get { return m_pathFind; }
            set { m_pathFind = value; OnPropertyChanged(); }
        }

        public GenericBlock Cranes
        {
            get { return m_cranes; }
            set { m_cranes = value; OnPropertyChanged(); }
        }

        public Pickups Pickups
        {
            get { return m_pickups; }
            set { m_pickups = value; OnPropertyChanged(); }
        }

        public GenericBlock PhoneInfo
        {
            get { return m_phoneInfo; }
            set { m_phoneInfo = value; OnPropertyChanged(); }
        }

        public GenericBlock Restarts
        {
            get { return m_restarts; }
            set { m_restarts = value; OnPropertyChanged(); }
        }

        public GenericBlock RadarBlips
        {
            get { return m_radarBlips; }
            set { m_radarBlips = value; OnPropertyChanged(); }
        }

        public GenericBlock Zones
        {
            get { return m_zones; }
            set { m_zones = value; OnPropertyChanged(); }
        }

        public GenericBlock GangData
        {
            get { return m_gangData; }
            set { m_gangData = value; OnPropertyChanged(); }
        }

        public CarGeneratorsBlock CarGenerators
        {
            get { return m_carGenerators; }
            set { m_carGenerators = value; OnPropertyChanged(); }
        }

        public GenericBlock Particles
        {
            get { return m_particles; }
            set { m_particles = value; OnPropertyChanged(); }
        }

        public GenericBlock AudioScriptObjects
        {
            get { return m_audioScriptObjects; }
            set { m_audioScriptObjects = value; OnPropertyChanged(); }
        }

        public GenericBlock PlayerInfo
        {
            get { return m_playerInfo; }
            set { m_playerInfo = value; OnPropertyChanged(); }
        }

        public GenericBlock Stats
        {
            get { return m_stats; }
            set { m_stats = value; OnPropertyChanged(); }
        }

        public GenericBlock Streaming
        {
            get { return m_streaming; }
            set { m_streaming = value; OnPropertyChanged(); }
        }

        public GenericBlock PedTypeInfo
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
            m_vehicles,
            m_objects,
            m_pathFind,
            m_cranes,
            m_pickups,
            m_phoneInfo,
            m_restarts,
            m_radarBlips,
            m_zones,
            m_gangData,
            m_carGenerators,
            m_particles,
            m_audioScriptObjects,
            m_playerInfo,
            m_stats,
            m_streaming,
            m_pedTypeInfo
        }.AsReadOnly();

        protected override Dictionary<FileFormat, int> MaxBlockSize => new Dictionary<FileFormat, int>
        {
            { FileFormats.Android, 0xD6D8 },
            { FileFormats.IOS, 0xD6D8 },
            { FileFormats.PC, 0xD6D8 },
            { FileFormats.PS2AU, 0xC350 },
            { FileFormats.PS2JP, 0xC350 },
            { FileFormats.PS2NAEU, 0xC350 },
            { FileFormats.Xbox, 0xD6D8 }
        };

        protected override Dictionary<FileFormat, int> SimpleVarsSize => new Dictionary<FileFormat, int>
        {
            { FileFormats.Android, 0xB0 },
            { FileFormats.IOS, 0xB0 },
            { FileFormats.PC, 0xBC },
            { FileFormats.PS2AU, 0xA8 },
            { FileFormats.PS2JP, 0xB0 },
            { FileFormats.PS2NAEU, 0xB0 },
            { FileFormats.Xbox, 0xBC }
        };

        public GTA3Save()
        {
            m_simpleVars = new SimpleVars();
            m_scripts = new Scripts();
            m_pedPool = new GenericBlock();
            m_garages = new Garages();
            m_vehicles = new VehiclePool();
            m_objects = new GenericBlock();
            m_pathFind = new GenericBlock();
            m_cranes = new GenericBlock();
            m_pickups = new Pickups();
            m_phoneInfo = new GenericBlock();
            m_restarts = new GenericBlock();
            m_radarBlips = new GenericBlock();
            m_zones = new GenericBlock();
            m_gangData = new GenericBlock();
            m_carGenerators = new CarGeneratorsBlock();
            m_particles = new GenericBlock();
            m_audioScriptObjects = new GenericBlock();
            m_playerInfo = new GenericBlock();
            m_stats = new GenericBlock();
            m_streaming = new GenericBlock();
            m_pedTypeInfo = new GenericBlock();
        }

        private GTA3Save(SaveDataSerializer serializer, FileFormat format)
        {
            if (!FileFormats.GetAll().Contains(format))
            {
                throw new SaveDataSerializationException(
                    string.Format("'{0}' is not a valid file format for GTA3 save data.", format));
            }

            FileFormat = format;

            int outerBlockCount = 0;
            bool doneReading = false;

            ByteBuffer simpleVars = null;
            ByteBuffer scripts = null;
            ByteBuffer pedPool = null;
            ByteBuffer garages = null;
            ByteBuffer vehicles = null;
            ByteBuffer objects = null;
            ByteBuffer pathFind = null;
            ByteBuffer cranes = null;
            ByteBuffer pickups = null;
            ByteBuffer phoneInfo = null;
            ByteBuffer restarts = null;
            ByteBuffer radarBlips = null;
            ByteBuffer zones = null;
            ByteBuffer gangData = null;
            ByteBuffer carGenerators = null;
            ByteBuffer particles = null;
            ByteBuffer audioScriptObjects = null;
            ByteBuffer playerInfo = null;
            ByteBuffer stats = null;
            ByteBuffer streaming = null;
            ByteBuffer pedTypeInfo = null;
            ByteBuffer tmp;

            while (!doneReading)
            {
                tmp = ReadBlock(serializer, null);
                using (SaveDataSerializer blockStream = CreateSerializer(new MemoryStream(tmp)))
                {
                    if (format.IsPS2)
                    {
                        switch (outerBlockCount)
                        {
                            case 0:
                                simpleVars = blockStream.ReadBytes(SimpleVarsSize[format]);
                                scripts = ReadBlock(blockStream, ScrTag);
                                pedPool = ReadBlock(blockStream);
                                garages = ReadBlock(blockStream);
                                vehicles = ReadBlock(blockStream);
                                break;
                            case 1:
                                objects = ReadBlock(blockStream);
                                pathFind = ReadBlock(blockStream);
                                cranes = ReadBlock(blockStream);
                                break;
                            case 2:
                                pickups = ReadBlock(blockStream);
                                phoneInfo = ReadBlock(blockStream);
                                restarts = ReadBlock(blockStream, RstTag);
                                radarBlips = ReadBlock(blockStream, RdrTag);
                                zones = ReadBlock(blockStream, ZnsTag);
                                gangData = ReadBlock(blockStream, GngTag);
                                carGenerators = ReadBlock(blockStream, CgnTag);
                                particles = ReadBlock(blockStream);
                                audioScriptObjects = ReadBlock(blockStream, AudTag);
                                playerInfo = ReadBlock(blockStream);
                                stats = ReadBlock(blockStream);
                                streaming = ReadBlock(blockStream);
                                pedTypeInfo = ReadBlock(blockStream, PtpTag);
                                doneReading = true;
                                break;
                        }
                    }
                    else
                    {
                        switch (outerBlockCount)
                        {
                            case 0:
                                simpleVars = blockStream.ReadBytes(SimpleVarsSize[format]);
                                scripts = ReadBlock(blockStream, ScrTag);
                                break;
                            case 1: pedPool = ReadBlock(blockStream); break;
                            case 2: garages = ReadBlock(blockStream); break;
                            case 3: vehicles = ReadBlock(blockStream); break;
                            case 4: objects = ReadBlock(blockStream); break;
                            case 5: pathFind = ReadBlock(blockStream); break;
                            case 6: cranes = ReadBlock(blockStream); break;
                            case 7: pickups = ReadBlock(blockStream); break;
                            case 8: phoneInfo = ReadBlock(blockStream); break;
                            case 9: restarts = ReadBlock(blockStream, RstTag); break;
                            case 10: radarBlips = ReadBlock(blockStream, RdrTag); break;
                            case 11: zones = ReadBlock(blockStream, ZnsTag); break;
                            case 12: gangData = ReadBlock(blockStream, GngTag); break;
                            case 13: carGenerators = ReadBlock(blockStream, CgnTag); break;
                            case 14: particles = ReadBlock(blockStream); break;
                            case 15: audioScriptObjects = ReadBlock(blockStream, AudTag); break;
                            case 16: playerInfo = ReadBlock(blockStream); break;
                            case 17: stats = ReadBlock(blockStream); break;
                            case 18: streaming = ReadBlock(blockStream); break;
                            case 19:
                                pedTypeInfo = ReadBlock(blockStream, PtpTag);
                                doneReading = true;
                                break;
                        }
                    }

                    outerBlockCount++;
                }
            }

            m_simpleVars = Deserialize<SimpleVars>(simpleVars, format);
            m_scripts = Deserialize<Scripts>(scripts, format);
            m_pedPool = new GenericBlock(pedPool);
            m_garages = Deserialize<Garages>(garages, format);
            m_vehicles = Deserialize<VehiclePool>(vehicles, format);
            m_objects = new GenericBlock(objects);
            m_pathFind = new GenericBlock(pathFind);
            m_cranes = new GenericBlock(cranes);
            m_pickups = Deserialize<Pickups>(pickups, format);
            m_phoneInfo = new GenericBlock(phoneInfo);
            m_restarts = new GenericBlock(restarts);
            m_radarBlips = new GenericBlock(radarBlips);
            m_zones = new GenericBlock(zones);
            m_gangData = new GenericBlock(gangData);
            m_carGenerators = Deserialize<CarGeneratorsBlock>(carGenerators);
            m_particles = new GenericBlock(particles);
            m_audioScriptObjects = new GenericBlock(audioScriptObjects);
            m_playerInfo = new GenericBlock(playerInfo);
            m_stats = new GenericBlock(stats);
            m_streaming = new GenericBlock(streaming);
            m_pedTypeInfo = new GenericBlock(pedTypeInfo);
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            FileFormat = format;

            int dataBytesWritten = 0;
            int blockCount = 0;
            int checksum = 0;

            ByteBuffer simpleVars = Serialize(m_simpleVars, format);
            ByteBuffer scripts = CreateBlock(ScrTag, Serialize(m_scripts, format));
            ByteBuffer pedPool = CreateBlock(Serialize(m_pedPool, format));
            ByteBuffer garages = CreateBlock(Serialize(m_garages, format));
            ByteBuffer vehicles = CreateBlock(Serialize(m_vehicles, format));
            ByteBuffer objects = CreateBlock(Serialize(m_objects, format));
            ByteBuffer pathFind = CreateBlock(Serialize(m_pathFind, format));
            ByteBuffer cranes = CreateBlock(Serialize(m_cranes, format));
            ByteBuffer pickups = CreateBlock(Serialize(m_pickups, format));
            ByteBuffer phoneInfo = CreateBlock(Serialize(m_phoneInfo, format));
            ByteBuffer restarts = CreateBlock(RstTag, Serialize(m_restarts, format));
            ByteBuffer radarBlips = CreateBlock(RdrTag, Serialize(m_radarBlips, format));
            ByteBuffer zones = CreateBlock(ZnsTag, Serialize(m_zones, format));
            ByteBuffer gangData = CreateBlock(GngTag, Serialize(m_gangData, format));
            ByteBuffer carGenerators = CreateBlock(CgnTag, Serialize(m_carGenerators, format));
            ByteBuffer particles = CreateBlock(Serialize(m_particles, format));
            ByteBuffer audioScriptObjects = CreateBlock(AudTag, Serialize(m_audioScriptObjects, format));
            ByteBuffer playerInfo = CreateBlock(Serialize(m_playerInfo, format));
            ByteBuffer stats = CreateBlock(Serialize(m_stats, format));
            ByteBuffer streaming = CreateBlock(Serialize(m_streaming, format));
            ByteBuffer pedTypeInfo = CreateBlock(PtpTag, Serialize(m_pedTypeInfo, format));

            while (dataBytesWritten < TotalBlockDataSize)
            {
                ByteBuffer payload = new ByteBuffer(0);
                bool padding = false;

                if (format.IsPS2)
                {
                    switch (blockCount)
                    {
                        case 0:
                            payload = CreateBlock(
                                simpleVars,
                                scripts,
                                pedPool,
                                garages,
                                vehicles);
                            break;
                        case 1:
                            payload = CreateBlock(
                                objects,
                                pathFind,
                                cranes);
                            break;
                        case 2:
                            payload = CreateBlock(
                                pickups,
                                phoneInfo,
                                restarts,
                                radarBlips,
                                zones,
                                gangData,
                                carGenerators,
                                particles,
                                audioScriptObjects,
                                playerInfo,
                                stats,
                                streaming,
                                pedTypeInfo);
                            break;
                        default:
                            padding = true;
                            break;
                    }
                }
                else
                {
                    switch (blockCount)
                    {
                        case 0: payload = CreateBlock(simpleVars, scripts); break;
                        case 1: payload = CreateBlock(pedPool); break;
                        case 2: payload = CreateBlock(garages); break;
                        case 3: payload = CreateBlock(vehicles); break;
                        case 4: payload = CreateBlock(objects); break;
                        case 5: payload = CreateBlock(pathFind); break;
                        case 6: payload = CreateBlock(cranes); break;
                        case 7: payload = CreateBlock(pickups); break;
                        case 8: payload = CreateBlock(phoneInfo); break;
                        case 9: payload = CreateBlock(restarts); break;
                        case 10: payload = CreateBlock(radarBlips); break;
                        case 11: payload = CreateBlock(zones); break;
                        case 12: payload = CreateBlock(gangData); break;
                        case 13: payload = CreateBlock(carGenerators); break;
                        case 14: payload = CreateBlock(particles); break;
                        case 15: payload = CreateBlock(audioScriptObjects); break;
                        case 16: payload = CreateBlock(playerInfo); break;
                        case 17: payload = CreateBlock(stats); break;
                        case 18: payload = CreateBlock(streaming); break;
                        case 19: payload = CreateBlock(pedTypeInfo); break;
                        default:
                            padding = true;
                            break;
                    }
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(GTA3Save other)
        {
            if (other == null)
            {
                return false;
            }

            return m_simpleVars.Equals(other.m_simpleVars)
                && m_scripts.Equals(other.m_scripts)
                && m_pedPool.Equals(other.m_pedPool)
                && m_garages.Equals(other.m_garages)
                && m_vehicles.Equals(other.m_vehicles)
                && m_objects.Equals(other.m_objects)
                && m_pathFind.Equals(other.m_pathFind)
                && m_cranes.Equals(other.m_cranes)
                && m_pickups.Equals(other.m_pickups)
                && m_phoneInfo.Equals(other.m_phoneInfo)
                && m_restarts.Equals(other.m_restarts)
                && m_radarBlips.Equals(other.m_radarBlips)
                && m_zones.Equals(other.m_zones)
                && m_gangData.Equals(other.m_gangData)
                && m_carGenerators.Equals(other.m_carGenerators)
                && m_particles.Equals(other.m_particles)
                && m_audioScriptObjects.Equals(other.m_audioScriptObjects)
                && m_playerInfo.Equals(other.m_playerInfo)
                && m_stats.Equals(other.m_stats)
                && m_streaming.Equals(other.m_streaming)
                && m_pedTypeInfo.Equals(other.m_pedTypeInfo);
        }

        protected override FileFormat DetectFileFormat(string path)
        {
            if (path == null)
            {
                return null;
            }

            bool isMobile = false;
            bool isPcOrXbox = false;

            byte[] data = File.ReadAllBytes(path);

            int fileId = data.FindFirst(BitConverter.GetBytes(0x31401));
            int fileIdJP = data.FindFirst(BitConverter.GetBytes(0x31400));
            int scr = data.FindFirst("SCR\0".GetAsciiBytes());

            int blk1Size;
            using (SaveDataSerializer s = new SaveDataSerializer(new MemoryStream(data)))
            {
                s.Skip(s.ReadInt32());
                blk1Size = s.ReadInt32();
            }

            if (scr == 0xB0 && fileId == 0x04)
            {
                // PS2, Austra
                return FileFormats.PS2AU;
            }
            else if (scr == 0xB8)
            {
                if (fileIdJP == 0x04)
                {
                    // PS2, Japan
                    return FileFormats.PS2JP;
                }
                else if (fileId == 0x04)
                {
                    // PS2, North America/Europe
                    return FileFormats.PS2NAEU;
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
                    // iOS
                    return FileFormats.IOS;
                }
                else if (blk1Size == 0x64C)
                {
                    // Android
                    return FileFormats.Android;
                }
            }
            else if (isPcOrXbox)
            {
                if (blk1Size == 0x624)
                {
                    // PC (Windows, macOS)
                    return FileFormats.PC;
                }
                else if (blk1Size == 0x628)
                {
                    // Xbox
                    return FileFormats.Xbox;
                }
            }

            return null;
        }

        public static class FileFormats
        {
            public static readonly FileFormat Android = new FileFormat(
                "Android", ConsoleType.Android
            );

            public static readonly FileFormat IOS = new FileFormat(
                "iOS", ConsoleType.IOS
            );

            public static readonly FileFormat PC = new FileFormat(
                "PC (Windows/macOS)", ConsoleType.PC
            );

            public static readonly FileFormat PS2AU = new FileFormat(
                "PS2 (Australia)", ConsoleType.PS2, ConsoleFlags.Australia
            );

            public static readonly FileFormat PS2JP = new FileFormat(
                "PS2 (Japan)", ConsoleType.PS2, ConsoleFlags.Japan
            );

            public static readonly FileFormat PS2NAEU = new FileFormat(
                "PS2 (North America/Europe)", ConsoleType.PS2, ConsoleFlags.NorthAmerica | ConsoleFlags.Europe
            );

            public static readonly FileFormat Xbox = new FileFormat(
                "Xbox", ConsoleType.Xbox
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { Android, IOS, PC, PS2AU, PS2JP, PS2NAEU, Xbox };
            }
        }
    }
}
