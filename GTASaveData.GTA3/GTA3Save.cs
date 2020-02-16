using GTASaveData.Extensions;
using GTASaveData.GTA3.Blocks;
using GTASaveData.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// Represents a save file for <i>Grand Theft Auto III</i>.
    /// </summary>
    public class GTA3Save : GrandTheftAutoSave,
        IEquatable<GTA3Save>
    {
        // The number of bytes in all first-level blocks, excluding the size header.
        private const int SizeOfGameInBytes = 0x31400;

        // Block IDs for tagged blocks.
        private const string ScrTag = "SCR";
        private const string RstTag = "RST";
        private const string RdrTag = "RDR";
        private const string ZnsTag = "ZNS";
        private const string GngTag = "GNG";
        private const string CgnTag = "CGN";
        private const string AudTag = "AUD";
        private const string PtpTag = "PTP";

        public SimpleVars SimpleVars
        {
            get { return m_blocks[0] as SimpleVars; }
            set { m_blocks[0] = value; OnPropertyChanged(); }
        }

        public ScriptBlock Scripts
        {
            get { return m_blocks[1] as ScriptBlock; }
            set { m_blocks[1] = value; OnPropertyChanged(); }
        }

        public Block PedPool
        {
            get { return m_blocks[2] as Block; }
            set { m_blocks[2] = value; OnPropertyChanged(); }
        }

        public GarageBlock Garages
        {
            get { return m_blocks[3] as GarageBlock; }
            set { m_blocks[3] = value; OnPropertyChanged(); }
        }

        public VehiclePool Vehicles
        {
            get { return m_blocks[4] as VehiclePool; }
            set { m_blocks[4] = value; OnPropertyChanged(); }
        }

        public Block Objects
        {
            get { return m_blocks[5] as Block; }
            set { m_blocks[5] = value; OnPropertyChanged(); }
        }

        public Block PathFind
        {
            get { return m_blocks[6] as Block; }
            set { m_blocks[6] = value; OnPropertyChanged(); }
        }

        public Block Cranes
        {
            get { return m_blocks[7] as Block; }
            set { m_blocks[7] = value; OnPropertyChanged(); }
        }

        public PickupBlock Pickups
        {
            get { return m_blocks[8] as PickupBlock; }
            set { m_blocks[8] = value; OnPropertyChanged(); }
        }

        public Block PhoneInfo
        {
            get { return m_blocks[9] as Block; }
            set { m_blocks[9] = value; OnPropertyChanged(); }
        }

        public Block Restarts
        {
            get { return m_blocks[10] as Block; }
            set { m_blocks[10] = value; OnPropertyChanged(); }
        }

        public Block RadarBlips
        {
            get { return m_blocks[11] as Block; }
            set { m_blocks[11] = value; OnPropertyChanged(); }
        }

        public Block Zones
        {
            get { return m_blocks[12] as Block; }
            set { m_blocks[12] = value; OnPropertyChanged(); }
        }

        public Block GangData
        {
            get { return m_blocks[13] as Block; }
            set { m_blocks[13] = value; OnPropertyChanged(); }
        }

        public CarGeneratorBlock CarGenerators
        {
            get { return m_blocks[14] as CarGeneratorBlock; }
            set { m_blocks[14] = value; OnPropertyChanged(); }
        }

        public Block Particles
        {
            get { return m_blocks[15] as Block; }
            set { m_blocks[15] = value; OnPropertyChanged(); }
        }

        public Block AudioScriptObjects
        {
            get { return m_blocks[16] as Block; }
            set { m_blocks[16] = value; OnPropertyChanged(); }
        }

        public Block PlayerInfo
        {
            get { return m_blocks[17] as Block; }
            set { m_blocks[17] = value; OnPropertyChanged(); }
        }

        public Block Stats
        {
            get { return m_blocks[18] as Block; }
            set { m_blocks[18] = value; OnPropertyChanged(); }
        }

        public Block Streaming
        {
            get { return m_blocks[19] as Block; }
            set { m_blocks[19] = value; OnPropertyChanged(); }
        }

        public Block PedTypeInfo
        {
            get { return m_blocks[20] as Block; }
            set { m_blocks[20] = value; OnPropertyChanged(); }
        }

        public override string Name => SimpleVars.LastMissionPassedName;

        protected override int MaxBlockSize => (FileFormat.IsPS2) ? 50000 : 55000;

        protected override int BlockCount => 21;

        protected override int SectionCount => (FileFormat.IsPS2) ? 3 : 20;

        protected override int SimpleVarsSize
        {
            get
            {
                if (FileFormat.IsMobile || FileFormat.IsPS2)
                {
                    return (FileFormat.HasFlag(ConsoleFlags.Australia)) ? 0xA8 : 0xB0;
                }
                else if (FileFormat.IsPC || FileFormat.IsXbox)
                {
                    return 0xBC;
                }

                throw new InvalidOperationException("Not implemented!");
            }
        }

        public GTA3Save()
        {
            m_blocks[0] = new SimpleVars();
            m_blocks[1] = new ScriptBlock();
            m_blocks[2] = new Block();
            m_blocks[3] = new GarageBlock();
            m_blocks[4] = new VehiclePool();
            m_blocks[5] = new Block();
            m_blocks[6] = new Block();
            m_blocks[7] = new Block();
            m_blocks[8] = new PickupBlock();
            m_blocks[9] = new Block();
            m_blocks[10] = new Block();
            m_blocks[11] = new Block();
            m_blocks[12] = new Block();
            m_blocks[13] = new Block();
            m_blocks[14] = new CarGeneratorBlock();
            m_blocks[15] = new Block();
            m_blocks[16] = new Block();
            m_blocks[17] = new Block();
            m_blocks[18] = new Block();
            m_blocks[19] = new Block();
            m_blocks[20] = new Block();
        }

        protected override FileFormat DetectFileFormat(byte[] data)
        {
            bool isMobile = false;
            bool isPcOrXbox = false;

            int fileId = data.FindFirst(BitConverter.GetBytes(0x31401));
            int fileIdJP = data.FindFirst(BitConverter.GetBytes(0x31400));
            int scr = data.FindFirst("SCR\0".GetAsciiBytes());

            int blk1Size;
            using (Serializer s = new Serializer(new MemoryStream(data)))
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

        protected override byte[] ReadBlock(Serializer r, string tag)
        {
            int length = r.ReadInt32();
            Debug.WriteLineIf(length > MaxBlockSize, "ReadBlock: Maximum block size exceeded!");

            if (tag != null)
            {
                string str = r.ReadString(4);
                int innerLength = r.ReadInt32();
                Debug.Assert(str == tag, "ReadBlock: Invalid tag!", "Expected: {0}, Actual: {1}", tag, str);
                Debug.Assert(innerLength == (length - 8));
                length = innerLength;
            }

            byte[] data = r.ReadBytes(length);
            r.Align();

            return data;
        }

        protected override byte[] CreateBlock(string tag, byte[] data)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer w = CreateSerializer(m))
                {
                    int totalLength = data.Length;

                    if (tag != null)
                    {
                        totalLength += 8;
                        w.Write(totalLength);
                        w.Write(tag, 4);
                    }

                    w.Write(data.Length);
                    w.Write(data);
                    w.Align();

                    Debug.WriteLineIf(totalLength > MaxBlockSize, "CreateBlock: Maximum block size exceeded!");
                }

                return m.ToArray();
            }
        }

        protected override byte[] CreatePadding(int length)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer w = CreateSerializer(m))
                {
                    int padLength = Math.Min(length, MaxBlockSize);
                    w.WritePadding(padLength);
                }

                return m.ToArray();
            }
        }

        protected override void LoadSection(int index, byte[] data)
        {
            using (Serializer r = CreateSerializer(new MemoryStream(data)))
            {
                if (FileFormat.IsPS2)
                {
                    switch (index)
                    {
                        case 0:
                            SimpleVars = Deserialize<SimpleVars>(r.ReadBytes(SimpleVarsSize));
                            Scripts = Deserialize<ScriptBlock>(ReadBlock(r, ScrTag));
                            PedPool = ReadBlock(r, null);
                            Garages = Deserialize<GarageBlock>(ReadBlock(r, null));
                            Vehicles = Deserialize<VehiclePool>(ReadBlock(r, null));
                            break;
                        case 1:
                            Objects = ReadBlock(r, null);
                            PathFind = ReadBlock(r, null);
                            Cranes = ReadBlock(r, null);
                            break;
                        case 2:
                            Pickups = Deserialize<PickupBlock>(ReadBlock(r, null));
                            PhoneInfo = ReadBlock(r, null);
                            Restarts = ReadBlock(r, RstTag);
                            RadarBlips = ReadBlock(r, RdrTag);
                            Zones = ReadBlock(r, ZnsTag);
                            GangData = ReadBlock(r, GngTag);
                            CarGenerators = Deserialize<CarGeneratorBlock>(ReadBlock(r, CgnTag));
                            Particles = ReadBlock(r, null);
                            AudioScriptObjects = ReadBlock(r, AudTag);
                            PlayerInfo = ReadBlock(r, null);
                            Stats = ReadBlock(r, null);
                            Streaming = ReadBlock(r, null);
                            PedTypeInfo = ReadBlock(r, PtpTag);
                            break;
                    }
                }
                else
                {
                    switch (index)
                    {
                        case 0:
                            SimpleVars = Deserialize<SimpleVars>(r.ReadBytes(SimpleVarsSize));
                            Scripts = Deserialize<ScriptBlock>(ReadBlock(r, ScrTag));
                            break;
                        case 1: PedPool = ReadBlock(r, null); break;
                        case 2: Garages = Deserialize<GarageBlock>(ReadBlock(r, null)); break;
                        case 3: Vehicles = Deserialize<VehiclePool>(ReadBlock(r, null)); break;
                        case 4: Objects = ReadBlock(r, null); break;
                        case 5: PathFind = ReadBlock(r, null); break;
                        case 6: Cranes = ReadBlock(r, null); break;
                        case 7: Pickups = Deserialize<PickupBlock>(ReadBlock(r, null)); break;
                        case 8: PhoneInfo = ReadBlock(r, null); break;
                        case 9: Restarts = ReadBlock(r, RstTag); break;
                        case 10: RadarBlips = ReadBlock(r, RdrTag); break;
                        case 11: Zones = ReadBlock(r, ZnsTag); break;
                        case 12: GangData = ReadBlock(r, GngTag); break;
                        case 13: CarGenerators = Deserialize<CarGeneratorBlock>(ReadBlock(r, CgnTag)); break;
                        case 14: Particles = ReadBlock(r, null); break;
                        case 15: AudioScriptObjects = ReadBlock(r, AudTag); break;
                        case 16: PlayerInfo = ReadBlock(r, null); break;
                        case 17: Stats = ReadBlock(r, null); break;
                        case 18: Streaming = ReadBlock(r, null); break;
                        case 19: PedTypeInfo = ReadBlock(r, PtpTag); break;
                    }
                }
            }
        }

        protected override byte[] SaveSection(int index)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer w = CreateSerializer(m))
                {
                    if (FileFormat.IsPS2)
                    {
                        switch (index)
                        {
                            case 0:
                                w.Write(Serialize(SimpleVars));
                                w.Write(CreateBlock(ScrTag, Scripts));
                                w.Write(CreateBlock(null, PedPool));
                                w.Write(CreateBlock(null, Vehicles));
                                break;
                            case 1:
                                CreateBlock(null, Objects);
                                CreateBlock(null, PathFind);
                                CreateBlock(null, Cranes);
                                break;
                            case 2:
                                CreateBlock(null, Pickups);
                                CreateBlock(null, PhoneInfo);
                                CreateBlock(RdrTag, RadarBlips);
                                CreateBlock(ZnsTag, Zones);
                                CreateBlock(GngTag, GangData);                                
                                CreateBlock(CgnTag, CarGenerators);
                                CreateBlock(null, Particles);
                                CreateBlock(AudTag, AudioScriptObjects);
                                CreateBlock(null, PlayerInfo);
                                CreateBlock(null, Stats);
                                CreateBlock(null, Streaming);
                                CreateBlock(PtpTag, PedTypeInfo);
                                break;
                        }
                    }
                    else
                    {
                        switch (index)
                        {
                            case 0:
                                w.Write(Serialize(SimpleVars));
                                w.Write(CreateBlock(ScrTag, Scripts));
                                break;
                            case 1: w.Write(CreateBlock(null, PedPool)); break;
                            case 2: w.Write(CreateBlock(null, Garages)); break;
                            case 3: w.Write(CreateBlock(null, Vehicles)); break;
                            case 4: w.Write(CreateBlock(null, Objects)); break;
                            case 5: w.Write(CreateBlock(null, PathFind)); break;
                            case 6: w.Write(CreateBlock(null, Cranes)); break;
                            case 7: w.Write(CreateBlock(null, Pickups)); break;
                            case 8: w.Write(CreateBlock(null, PhoneInfo)); break;
                            case 9: w.Write(CreateBlock(RstTag, Restarts)); break;
                            case 10: w.Write(CreateBlock(RdrTag, RadarBlips)); break;
                            case 11: w.Write(CreateBlock(ZnsTag, Zones)); break;
                            case 12: w.Write(CreateBlock(GngTag, GangData)); break;
                            case 13: w.Write(CreateBlock(CgnTag, CarGenerators)); break;
                            case 14: w.Write(CreateBlock(null, Particles)); break;
                            case 15: w.Write(CreateBlock(AudTag, AudioScriptObjects)); break;
                            case 16: w.Write(CreateBlock(null, PlayerInfo)); break;
                            case 17: w.Write(CreateBlock(null, Stats)); break;
                            case 18: w.Write(CreateBlock(null, Streaming)); break;
                            case 19: w.Write(CreateBlock(PtpTag, PedTypeInfo)); break;
                        }
                    }
                }

                return m.ToArray();
            }
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            //if (!FileFormats.GetAll().Contains(fmt))
            //{
            //    throw new SerializationException(
            //        string.Format("'{0}' is not a valid file format for GTA VC save data.", fmt));
            //}

            FileFormat = fmt;

            int index = 0;

            int numSectionsRead = 0;

            while (r.BaseStream.Position < r.BaseStream.Length - 4)
            {
                int length = r.ReadInt32();
                byte[] data = r.ReadBytes(length);

                if (index < SectionCount)
                {
                    LoadSection(index++, data);
                }

                numSectionsRead++;
            }

            Debug.WriteLine("Read {0} sections ({1} padding).", numSectionsRead, numSectionsRead - index);
            Debug.Assert(r.BaseStream.Position == SizeOfGameInBytes + (4 * numSectionsRead));
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            //if (!FileFormats.GetAll().Contains(fmt))
            //{
            //    throw new SerializationException(
            //        string.Format("'{0}' is not a valid file format for GTA VC save data.", fmt));
            //}

            FileFormat = fmt;

            int index = 0;
            int checksum = 0;
            int numSectionsRead = 0;
            int bytesWritten = 0;

            while (bytesWritten < SizeOfGameInBytes)
            {
                byte[] data;

                if (index < SectionCount)
                {
                    data = SaveSection(index++);
                }
                else
                {
                    data = CreatePadding(SizeOfGameInBytes - bytesWritten);
                }

                w.Write(data.Length);
                w.Write(data);

                checksum += data.Sum(x => x);
                bytesWritten += data.Length;
                numSectionsRead++;
            }

            Debug.WriteLine("Wrote {0} sections ({1} padding).", numSectionsRead, numSectionsRead - index);

            Debug.Assert(bytesWritten == SizeOfGameInBytes);
            Debug.Assert(w.BaseStream.Position == SizeOfGameInBytes + (4 * numSectionsRead));

            w.Write(checksum);
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

            return m_blocks.SequenceEqual(other.m_blocks);
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
