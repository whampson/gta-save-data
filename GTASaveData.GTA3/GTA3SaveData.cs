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
    public sealed class GTA3SaveData : SaveDataFile,
        IEquatable<GTA3SaveData>
    {
        public static class FileFormats
        {
            public static readonly FileFormat Android = new FileFormat(
                "Android", ConsoleType.Android
            );

            public static readonly FileFormat IOS = new FileFormat(
                "iOS", ConsoleType.IOS
            );

            public static readonly FileFormat PC = new FileFormat(
                "PC (Windows, macOS)", ConsoleType.PC
            );

            public static readonly FileFormat PS2 = new FileFormat(
                "PS2", ConsoleType.PS2
            );

            public static readonly FileFormat PS2AU = new FileFormat(
                "PS2 (Australia)", ConsoleType.PS2, ConsoleFlags.Australia
            );

            public static readonly FileFormat PS2JP = new FileFormat(
                "PS2 (Japan)", ConsoleType.PS2, ConsoleFlags.Japan
            );

            public static readonly FileFormat Xbox = new FileFormat(
                "Xbox", ConsoleType.Xbox
            );

            public static FileFormat[] GetSupportedFileFormats()
            {
                return new FileFormat[] { Android, IOS, PC, PS2, PS2AU, PS2JP, Xbox };
            }
        }

        private static readonly Dictionary<FileFormat, int> SimpleVarsSize = new Dictionary<FileFormat, int>
        {
            { FileFormats.Android, 0xB0 },
            { FileFormats.IOS, 0xB0 },
            { FileFormats.PC, 0xBC },
            { FileFormats.PS2, 0xB0 },
            { FileFormats.PS2AU, 0xA8 },
            { FileFormats.PS2JP, 0xB0 },
            { FileFormats.Xbox, 0xBC }
        };

        private static readonly Dictionary<FileFormat, int> MaxBlockSize = new Dictionary<FileFormat, int>
        {
            { FileFormats.Android, 0xD6D8 },
            { FileFormats.IOS, 0xD6D8 },
            { FileFormats.PC, 0xD6D8 },
            { FileFormats.PS2, 0xC350 },
            { FileFormats.PS2AU, 0xC350 },
            { FileFormats.PS2JP, 0xC350 },
            { FileFormats.Xbox, 0xD6D8 }
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
        private Scripts m_scripts;
        private DummyObject m_pedPool;
        private Garages m_garages;
        private DummyObject m_vehicles;
        private DummyObject m_objects;
        private DummyObject m_pathFind;
        private DummyObject m_cranes;
        private DummyObject m_pickups;
        private DummyObject m_phoneInfo;
        private DummyObject m_restarts;
        private DummyObject m_radarBlips;
        private DummyObject m_zones;
        private DummyObject m_gangData;
        private DummyObject m_carGenerators;
        private DummyObject m_particles;
        private DummyObject m_audioScriptObjects;
        private DummyObject m_playerInfo;
        private DummyObject m_stats;
        private DummyObject m_streaming;
        private DummyObject m_pedTypeInfo;

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

        public DummyObject PedPool
        {
            get { return m_pedPool; }
            set { m_pedPool = value; OnPropertyChanged(); }
        }

        public Garages Garages
        {
            get { return m_garages; }
            set { m_garages = value; OnPropertyChanged(); }
        }

        public DummyObject Vehicles
        {
            get { return m_vehicles; }
            set { m_vehicles = value; OnPropertyChanged(); }
        }

        public DummyObject Objects
        {
            get { return m_objects; }
            set { m_objects = value; OnPropertyChanged(); }
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
            get { return m_restarts; }
            set { m_restarts = value; OnPropertyChanged(); }
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

        public GTA3SaveData()
        {
            m_simpleVars = new SimpleVars();
            m_scripts = new Scripts();
            m_pedPool = new DummyObject();
            m_garages = new Garages();
            m_vehicles = new DummyObject();
            m_objects = new DummyObject();
            m_pathFind = new DummyObject();
            m_cranes = new DummyObject();
            m_pickups = new DummyObject();
            m_phoneInfo = new DummyObject();
            m_restarts = new DummyObject();
            m_radarBlips = new DummyObject();
            m_zones = new DummyObject();
            m_gangData = new DummyObject();
            m_carGenerators = new DummyObject();
            m_particles = new DummyObject();
            m_audioScriptObjects = new DummyObject();
            m_playerInfo = new DummyObject();
            m_stats = new DummyObject();
            m_streaming = new DummyObject();
            m_pedTypeInfo = new DummyObject();
        }

        public static GTA3SaveData Load(string path, FileFormat format)
        {
            byte[] data = File.ReadAllBytes(path);
            using (MemoryStream m = new MemoryStream(data))
            {
                using (SaveDataSerializer s = new SaveDataSerializer(m))
                {
                    return s.ReadObject<GTA3SaveData>(format);
                }
            }
        }

        private GTA3SaveData(SaveDataSerializer serializer, FileFormat format)
        {
            if (!FileFormats.GetSupportedFileFormats().Contains(format))
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
            m_pedPool = new DummyObject(pedPool);
            m_garages = Deserialize<Garages>(garages);
            m_vehicles = new DummyObject(vehicles);
            m_objects = new DummyObject(objects);
            m_pathFind = new DummyObject(pathFind);
            m_cranes = new DummyObject(cranes);
            m_pickups = new DummyObject(pickups);
            m_phoneInfo = new DummyObject(phoneInfo);
            m_restarts = new DummyObject(restarts);
            m_radarBlips = new DummyObject(radarBlips);
            m_zones = new DummyObject(zones);
            m_gangData = new DummyObject(gangData);
            m_carGenerators = new DummyObject(carGenerators);
            m_particles = new DummyObject(particles);
            m_audioScriptObjects = new DummyObject(audioScriptObjects);
            m_playerInfo = new DummyObject(playerInfo);
            m_stats = new DummyObject(stats);
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(GTA3SaveData other)
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
