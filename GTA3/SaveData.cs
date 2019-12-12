using GTASaveData.Common;
using System;
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
    public sealed class SaveData : GTAObject/*, IGTASaveData*/
    {
        // TODO: padding manipulation
   
        // The number of bytes in all first-level blocks, excluding the size header.
        private const int TotalBlockSize = 201728;

        private const int MaxPaddingBlockSize = 55000;
        private const int MaxPaddingBlockSizePS2 = 50000;

        // Block IDs for select blocks.
        private const string ScrTag = "SCR";
        private const string RstTag = "RST";
        private const string RdrTag = "RDR";
        private const string ZnsTag = "ZNS";
        private const string GngTag = "GNG";
        private const string CgnTag = "CGN";
        private const string AudTag = "AUD";
        private const string PtpTag = "PTP";

        public static readonly SystemType[] SupportedSystems =
        {
            SystemType.Android,
            SystemType.IOS,
            SystemType.PC,
            SystemType.PS2,
            SystemType.PS2AU,
            SystemType.PS2JP,
            SystemType.Xbox
        };

        private SystemType m_systemType;

        private SimpleVars m_simpleVars;
        private Scripts m_scripts;
        private DummyObject m_pedPool;
        private DummyObject m_garages;
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

        private int MaxBlockSize
        {
            get
            {
                return m_systemType.HasFlag(SystemType.PS2) ? MaxPaddingBlockSizePS2 : MaxPaddingBlockSize;
            }
        }

        public SystemType SystemType
        {
            get { return m_systemType; }
            set
            {
                if (!SupportedSystems.Contains(value))
                {
                    throw new ArgumentException(
                        string.Format("'{0}' is not a valid system type for GTA3 save data.", value));
                }
                m_systemType = value;
                OnPropertyChanged();
            }
        }

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

        public DummyObject Garages
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

        public SaveData(SystemType sysType)
        {
            SystemType = sysType;

            m_simpleVars = new SimpleVars();
            m_scripts = new Scripts();
        }

        private SaveData(Serializer serializer, SystemType system)
            : this(system)
        {
            Deserialize(serializer);
        }

        public static SaveData Load(string path, SystemType system)
        {
            byte[] data = File.ReadAllBytes(path);
            return Serializer.Deserialize<SaveData>(data, system: system);
        }

        public void Store(string path)
        {
            byte[] data = Serializer.Serialize(this, m_systemType);
            File.WriteAllBytes(path, data);
        }

        public void Deserialize(Serializer serializer)
        {
            int outerBlockCount = 0;
            bool doneReading = false;

            Payload simpleVars = null;
            Payload scripts = null;
            Payload pedPool = null;
            Payload garages = null;
            Payload vehicles = null;
            Payload objects = null;
            Payload pathFind = null;
            Payload cranes = null;
            Payload pickups = null;
            Payload phoneInfo = null;
            Payload restarts = null;
            Payload radarBlips = null;
            Payload zones = null;
            Payload gangData = null;
            Payload carGenerators = null;
            Payload particles = null;
            Payload audioScriptObjects = null;
            Payload playerInfo = null;
            Payload stats = null;
            Payload streaming = null;
            Payload pedTypeInfo = null;
            Payload tmp;

            while (!doneReading)
            {
                tmp = ReadBlock(serializer, null);
                using (Serializer blockStream = new Serializer(new MemoryStream(tmp)))
                {
                    if (m_systemType.HasFlag(SystemType.PS2))
                    {
                        switch (outerBlockCount)
                        {
                            case 0:
                                simpleVars = blockStream.ReadBytes(SimpleVars.GetSize(m_systemType));
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
                                simpleVars = blockStream.ReadBytes(SimpleVars.GetSize(m_systemType));
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

            m_simpleVars = Serializer.Deserialize<SimpleVars>(simpleVars, m_systemType);
            m_scripts = Serializer.Deserialize<Scripts>(scripts, m_systemType);
            m_pedPool = new DummyObject(pedPool);
            m_garages = new DummyObject(garages);
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

        private void Serialize(Serializer serializer)
        {
            int dataBytesWritten = 0;
            int blockCount = 0;
            int checksum = 0;
            
            Payload simpleVars = Serializer.Serialize(m_simpleVars, m_systemType);
            Payload scripts = CreateBlock(ScrTag, Serializer.Serialize(m_scripts, m_systemType));
            Payload pedPool = CreateBlock(Serializer.Serialize(m_pedPool, m_systemType));
            Payload garages = CreateBlock(Serializer.Serialize(m_garages, m_systemType));
            Payload vehicles = CreateBlock(Serializer.Serialize(m_vehicles, m_systemType));
            Payload objects = CreateBlock(Serializer.Serialize(m_objects, m_systemType));
            Payload pathFind = CreateBlock(Serializer.Serialize(m_pathFind, m_systemType));
            Payload cranes = CreateBlock(Serializer.Serialize(m_cranes, m_systemType));
            Payload pickups = CreateBlock(Serializer.Serialize(m_pickups, m_systemType));
            Payload phoneInfo = CreateBlock(Serializer.Serialize(m_phoneInfo, m_systemType));
            Payload restarts = CreateBlock(RstTag, Serializer.Serialize(m_restarts, m_systemType));
            Payload radarBlips = CreateBlock(RdrTag, Serializer.Serialize(m_radarBlips, m_systemType));
            Payload zones = CreateBlock(ZnsTag, Serializer.Serialize(m_zones, m_systemType));
            Payload gangData = CreateBlock(GngTag, Serializer.Serialize(m_gangData, m_systemType));
            Payload carGenerators = CreateBlock(CgnTag, Serializer.Serialize(m_carGenerators, m_systemType));
            Payload particles = CreateBlock(Serializer.Serialize(m_particles, m_systemType));
            Payload audioScriptObjects = CreateBlock(AudTag, Serializer.Serialize(m_audioScriptObjects, m_systemType));
            Payload playerInfo = CreateBlock(Serializer.Serialize(m_playerInfo, m_systemType));
            Payload stats = CreateBlock(Serializer.Serialize(m_stats, m_systemType));
            Payload streaming = CreateBlock(Serializer.Serialize(m_streaming, m_systemType));
            Payload pedTypeInfo = CreateBlock(PtpTag, Serializer.Serialize(m_pedTypeInfo, m_systemType));

            while (dataBytesWritten < TotalBlockSize)
            {
                Payload payload = new Payload(0);
                bool padding = false;

                if (m_systemType.HasFlag(SystemType.PS2))
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
                        case 0:  payload = CreateBlock(simpleVars, scripts); break;
                        case 1:  payload = CreateBlock(pedPool); break;
                        case 2:  payload = CreateBlock(garages); break;
                        case 3:  payload = CreateBlock(vehicles); break;
                        case 4:  payload = CreateBlock(objects); break;
                        case 5:  payload = CreateBlock(pathFind); break;
                        case 6:  payload = CreateBlock(cranes); break;
                        case 7:  payload = CreateBlock(pickups); break;
                        case 8:  payload = CreateBlock(phoneInfo); break;
                        case 9:  payload = CreateBlock(restarts); break;
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
                    int length = TotalBlockSize - dataBytesWritten;
                    if (length > MaxBlockSize)
                    {
                        length = MaxBlockSize;
                    }
                    payload = CreateBlock(new Payload(length));
                }

                serializer.Write(payload);
                dataBytesWritten += payload.Length - 4;
                checksum += payload.ToByteArray().Sum(x => x);
                blockCount++;
            }

            serializer.Write(checksum);

            Debug.Assert(dataBytesWritten == TotalBlockSize);
            Debug.Assert(serializer.BaseStream.Position == TotalBlockSize + (blockCount * 4) + 4);
        }

        private byte[] CreateBlock(params Payload[] chunks)
        {
            return CreateBlock(null, chunks);
        }

        private byte[] CreateBlock(string tag, params Payload[] chunks)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer s = new Serializer(m))
                {
                    WriteBlock(s, tag, chunks);
                }

                return m.ToArray();
            }
        }

        private Payload ReadBlock(Serializer s, string tag = null)
        {
            int length = s.ReadInt32();
            if (length > MaxBlockSize)
            {
                throw new SerializationException("Invalid block size. Size read exceeds the maximum allowed block size.");
            }

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

        private int WriteBlock(Serializer s, string tag, params Payload[] chunks)
        {
            long mark = s.BaseStream.Position;

            int payloadSize = chunks.Sum(x => x.Length);
            if (tag != null)
            {
                s.Write(payloadSize + 8);
                s.Write(tag, 4);
            }
            s.Write(payloadSize);
            foreach (Payload chunk in chunks)
            {
                s.Write(chunk);
            }
            s.Align();

            return (int) (s.BaseStream.Position - mark);
        }

        /// <summary>
        /// A byte array wrapper, made purely so byte[] can be
        /// manipulated as an object.
        /// </summary>
        private class Payload
        {
            private byte[] m_data;

            public Payload(int count)
            {
                m_data = new byte[count];
            }

            public Payload(byte[] data)
            {
                m_data = data;
            }

            public byte this[int i]
            {
                get { return m_data[i]; }
                set { m_data[i] = value; }
            }

            public int Length
            {
                get { return m_data.Length; }
            }

            public byte[] ToByteArray()
            {
                return m_data;
            }

            public static implicit operator byte[] (Payload c)
            {
                return c.m_data;
            }

            public static implicit operator Payload(byte[] data)
            {
                return new Payload(data);
            }
        }
    }

    public sealed class DummyObject : GTAObject
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

        private void Serialize(Serializer serializer)
        {
            serializer.Write(Data);
        }
    }
}
