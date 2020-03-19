using GTASaveData.Common;
using GTASaveData.Common.Blocks;
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
    public class GTA3Save : SaveFile,
        IGTASave,
        IEquatable<GTA3Save>
    {
        public const int SaveHeaderSize = 8;

        public static int ReadSaveHeader(WorkBuffer buf, string tag)
        {
            string readTag = buf.ReadString(4);
            int size = buf.ReadInt32();

            Debug.Assert(tag == readTag);
            return size;
        }

        public static void WriteSaveHeader(WorkBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4);
            buf.Write(size);
        }



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

        public TheScripts Scripts
        {
            get { return m_blocks[1] as TheScripts; }
            set { m_blocks[1] = value; OnPropertyChanged(); }
        }

        public Dummy PedPool
        {
            get { return m_blocks[2] as Dummy; }
            set { m_blocks[2] = value; OnPropertyChanged(); }
        }

        public Garages Garages
        {
            get { return m_blocks[3] as Garages; }
            set { m_blocks[3] = value; OnPropertyChanged(); }
        }

        public VehiclePool VehiclePool
        {
            get { return m_blocks[4] as VehiclePool; }
            set { m_blocks[4] = value; OnPropertyChanged(); }
        }

        public Dummy ObjectPool
        {
            get { return m_blocks[5] as Dummy; }
            set { m_blocks[5] = value; OnPropertyChanged(); }
        }

        public Dummy PathFind
        {
            get { return m_blocks[6] as Dummy; }
            set { m_blocks[6] = value; OnPropertyChanged(); }
        }

        public Dummy Cranes
        {
            get { return m_blocks[7] as Dummy; }
            set { m_blocks[7] = value; OnPropertyChanged(); }
        }

        public Pickups Pickups
        {
            get { return m_blocks[8] as Pickups; }
            set { m_blocks[8] = value; OnPropertyChanged(); }
        }

        public Dummy PhoneInfo
        {
            get { return m_blocks[9] as Dummy; }
            set { m_blocks[9] = value; OnPropertyChanged(); }
        }

        public Dummy RestartPoints
        {
            get { return m_blocks[10] as Dummy; }
            set { m_blocks[10] = value; OnPropertyChanged(); }
        }

        public Dummy RadarBlips
        {
            get { return m_blocks[11] as Dummy; }
            set { m_blocks[11] = value; OnPropertyChanged(); }
        }

        public Dummy Zones
        {
            get { return m_blocks[12] as Dummy; }
            set { m_blocks[12] = value; OnPropertyChanged(); }
        }

        public Dummy GangData
        {
            get { return m_blocks[13] as Dummy; }
            set { m_blocks[13] = value; OnPropertyChanged(); }
        }

        public TheCarGenerators CarGenerators
        {
            get { return m_blocks[14] as TheCarGenerators; }
            set { m_blocks[14] = value; OnPropertyChanged(); }
        }

        public Dummy Particles
        {
            get { return m_blocks[15] as Dummy; }
            set { m_blocks[15] = value; OnPropertyChanged(); }
        }

        public Dummy AudioScriptObjects
        {
            get { return m_blocks[16] as Dummy; }
            set { m_blocks[16] = value; OnPropertyChanged(); }
        }

        public Dummy PlayerInfo
        {
            get { return m_blocks[17] as Dummy; }
            set { m_blocks[17] = value; OnPropertyChanged(); }
        }

        public Dummy Stats
        {
            get { return m_blocks[18] as Dummy; }
            set { m_blocks[18] = value; OnPropertyChanged(); }
        }

        public Dummy Streaming
        {
            get { return m_blocks[19] as Dummy; }
            set { m_blocks[19] = value; OnPropertyChanged(); }
        }

        public Dummy PedTypeInfo
        {
            get { return m_blocks[20] as Dummy; }
            set { m_blocks[20] = value; OnPropertyChanged(); }
        }

        ISimpleVars IGTASave.SimpleVars => SimpleVars;

        //ICarGeneratorBlock IGrandTheftAutoSave.CarGenerators => CarGenerators;

        public override string Name => SimpleVars.SaveName;

        protected override int MaxBlockSize => (Format.SupportsPS2) ? 50000 : 55000;

        protected override int BlockCount => 21;

        protected override int SectionCount => (Format.SupportsPS2) ? 3 : 20;

        protected override int SimpleVarsSize
        {
            get
            {
                if (Format.SupportedOnMobile || Format.SupportsPS2)
                {
                    return (Format.IsSupportedOn(ConsoleType.PS2, ConsoleFlags.Australia)) ? 0xA8 : 0xB0;
                }
                else if (Format.SupportsPC || Format.SupportsXbox)
                {
                    return 0xBC;
                }

                throw new InvalidOperationException("Not implemented!");
            }
        }

        public GTA3Save()
        {
            m_blocks[0] = new SimpleVars();
            m_blocks[1] = new TheScripts();
            m_blocks[2] = new Dummy();
            m_blocks[3] = new Garages();
            m_blocks[4] = new VehiclePool();
            m_blocks[5] = new Dummy();
            m_blocks[6] = new Dummy();
            m_blocks[7] = new Dummy();
            m_blocks[8] = new Pickups();
            m_blocks[9] = new Dummy();
            m_blocks[10] = new Dummy();
            m_blocks[11] = new Dummy();
            m_blocks[12] = new Dummy();
            m_blocks[13] = new Dummy();
            m_blocks[14] = new TheCarGenerators();
            m_blocks[15] = new Dummy();
            m_blocks[16] = new Dummy();
            m_blocks[17] = new Dummy();
            m_blocks[18] = new Dummy();
            m_blocks[19] = new Dummy();
            m_blocks[20] = new Dummy();
        }

        protected override SaveFileFormat DetectFileFormat(byte[] data)
        {
            bool isMobile = false;
            bool isPcOrXbox = false;

            int fileId = data.FindFirst(BitConverter.GetBytes(0x31401));
            int fileIdJP = data.FindFirst(BitConverter.GetBytes(0x31400));
            int scr = data.FindFirst("SCR\0".GetAsciiBytes());

            int blk1Size;
            using (Serializer s = new Serializer(new MemoryStream(data)))
            {
                s.Position += s.ReadInt32();
                blk1Size = s.ReadInt32();
            }

            if (scr == 0xB0 && fileId == 0x04)
            {
                // PS2, Austra
                return FileFormats.PS2_AU;
            }
            else if (scr == 0xB8)
            {
                if (fileIdJP == 0x04)
                {
                    // PS2, Japan
                    return FileFormats.PS2_JP;
                }
                else if (fileId == 0x04)
                {
                    // PS2, North America/Europe
                    return FileFormats.PS2_NAEU;
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
                    return FileFormats.iOS;
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
                if (Format.SupportsPS2)
                {
                    switch (index)
                    {
                        case 0:
                            SimpleVars = ReadObject<SimpleVars>(r.ReadBytes(SimpleVarsSize));
                            Scripts = ReadObject<TheScripts>(ReadBlock(r, ScrTag));
                            PedPool = ReadBlock(r, null);
                            Garages = ReadObject<Garages>(ReadBlock(r, null));
                            VehiclePool = ReadObject<VehiclePool>(ReadBlock(r, null));
                            break;
                        case 1:
                            ObjectPool = ReadBlock(r, null);
                            PathFind = ReadBlock(r, null);
                            Cranes = ReadBlock(r, null);
                            break;
                        case 2:
                            Pickups = ReadObject<Pickups>(ReadBlock(r, null));
                            PhoneInfo = ReadBlock(r, null);
                            RestartPoints = ReadBlock(r, RstTag);
                            RadarBlips = ReadBlock(r, RdrTag);
                            Zones = ReadBlock(r, ZnsTag);
                            GangData = ReadBlock(r, GngTag);
                            CarGenerators = ReadObject<TheCarGenerators>(ReadBlock(r, CgnTag));
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
                            SimpleVars = ReadObject<SimpleVars>(r.ReadBytes(SimpleVarsSize));
                            Scripts = ReadObject<TheScripts>(ReadBlock(r, ScrTag));
                            break;
                        case 1: PedPool = ReadBlock(r, null); break;
                        case 2: Garages = ReadObject<Garages>(ReadBlock(r, null)); break;
                        case 3: VehiclePool = ReadObject<VehiclePool>(ReadBlock(r, null)); break;
                        case 4: ObjectPool = ReadBlock(r, null); break;
                        case 5: PathFind = ReadBlock(r, null); break;
                        case 6: Cranes = ReadBlock(r, null); break;
                        case 7: Pickups = ReadObject<Pickups>(ReadBlock(r, null)); break;
                        case 8: PhoneInfo = ReadBlock(r, null); break;
                        case 9: RestartPoints = ReadBlock(r, RstTag); break;
                        case 10: RadarBlips = ReadBlock(r, RdrTag); break;
                        case 11: Zones = ReadBlock(r, ZnsTag); break;
                        case 12: GangData = ReadBlock(r, GngTag); break;
                        case 13: CarGenerators = ReadObject<TheCarGenerators>(ReadBlock(r, CgnTag)); break;
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
                    if (Format.SupportsPS2)
                    {
                        switch (index)
                        {
                            case 0:
                                w.Write(WriteObject(SimpleVars));
                                w.Write(CreateBlock(ScrTag, Scripts));
                                w.Write(CreateBlock(null, PedPool));
                                w.Write(CreateBlock(null, Garages));
                                w.Write(CreateBlock(null, VehiclePool));
                                break;
                            case 1:
                                w.Write(CreateBlock(null, ObjectPool));
                                w.Write(CreateBlock(null, PathFind));
                                w.Write(CreateBlock(null, Cranes));
                                break;
                            case 2:
                                w.Write(CreateBlock(null, Pickups));
                                w.Write(CreateBlock(null, PhoneInfo));
                                w.Write(CreateBlock(RstTag, RestartPoints));
                                w.Write(CreateBlock(RdrTag, RadarBlips));
                                w.Write(CreateBlock(ZnsTag, Zones));
                                w.Write(CreateBlock(GngTag, GangData));
                                w.Write(CreateBlock(CgnTag, CarGenerators));
                                w.Write(CreateBlock(null, Particles));
                                w.Write(CreateBlock(AudTag, AudioScriptObjects));
                                w.Write(CreateBlock(null, PlayerInfo));
                                w.Write(CreateBlock(null, Stats));
                                w.Write(CreateBlock(null, Streaming));
                                w.Write(CreateBlock(PtpTag, PedTypeInfo));
                                break;
                        }
                    }
                    else
                    {
                        switch (index)
                        {
                            case 0:
                                w.Write(WriteObject(SimpleVars));
                                w.Write(CreateBlock(ScrTag, Scripts));
                                break;
                            case 1: w.Write(CreateBlock(null, PedPool)); break;
                            case 2: w.Write(CreateBlock(null, Garages)); break;
                            case 3: w.Write(CreateBlock(null, VehiclePool)); break;
                            case 4: w.Write(CreateBlock(null, ObjectPool)); break;
                            case 5: w.Write(CreateBlock(null, PathFind)); break;
                            case 6: w.Write(CreateBlock(null, Cranes)); break;
                            case 7: w.Write(CreateBlock(null, Pickups)); break;
                            case 8: w.Write(CreateBlock(null, PhoneInfo)); break;
                            case 9: w.Write(CreateBlock(RstTag, RestartPoints)); break;
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

        protected override void ReadObjectData(Serializer r, SaveFileFormat fmt)
        {
            //if (!FileFormats.GetAll().Contains(fmt))
            //{
            //    throw new SerializationException(
            //        string.Format("'{0}' is not a valid file format for GTA VC save data.", fmt));
            //}

            Format = fmt;

            int index = 0;
            int numSectionsRead = 0;
            int bytesRead = 0;

            while (r.BaseStream.Position < r.BaseStream.Length - 4)
            {
                int length = r.ReadInt32();
                byte[] data = r.ReadBytes(length);

                if (index < SectionCount)
                {
                    LoadSection(index++, data);
                }

                numSectionsRead++;
                bytesRead += data.Length;
            }

#if DEBUG
            Debug.WriteLine("Loaded GTA3 -- total bytes: {0}, total sections: {1}, padding: {2}",
                bytesRead, numSectionsRead, numSectionsRead - index);

            int sizeOfGame = (int) Serializer.GetAlignedAddress(SimpleVars.SizeOfGameInBytes);
            Debug.Assert(bytesRead == (sizeOfGame - 4));
            Debug.Assert(r.BaseStream.Position == sizeOfGame + (4 * numSectionsRead) - 4);
#endif
        }

        protected override void WriteObjectData(Serializer w, SaveFileFormat fmt)
        {
            //if (!FileFormats.GetAll().Contains(fmt))
            //{
            //    throw new SerializationException(
            //        string.Format("'{0}' is not a valid file format for GTA VC save data.", fmt));
            //}

            Format = fmt;

            int index = 0;
            int checksum = 0;
            int numSectionsWritten = 0;
            int bytesWritten = 0;

            int sizeOfGame = (int) Serializer.GetAlignedAddress(SimpleVars.SizeOfGameInBytes);

            while (bytesWritten < sizeOfGame - 4)
            {
                byte[] data;
                int lengthSum;

                if (index < SectionCount)
                {
                    data = SaveSection(index++);
                }
                else
                {
                    data = CreatePadding(sizeOfGame - bytesWritten - 4);
                }

                w.Write(data.Length);
                w.Write(data);

                lengthSum = Serializer.Write(data.Length).Sum(x => x);
                checksum += data.Sum(x => x) + lengthSum;
                bytesWritten += data.Length;
                numSectionsWritten++;
            }

            Debug.WriteLine("Saved GTA3 -- total bytes: {0}, total sections: {1}, padding: {2}",
                bytesWritten, numSectionsWritten, numSectionsWritten - index);

            Debug.Assert(bytesWritten == (sizeOfGame - 4));
            Debug.Assert(w.BaseStream.Position == sizeOfGame + (4 * numSectionsWritten) - 4);

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

        [Flags]
        public enum FileTypeFlags
        {
            None,
            GTA3_Android = 1 << 0,
            GTA3_iOS = 1 << 1,
            GTA3_PC = 1 << 2,
            GTA3_PS2_AU = 1 << 3,
            GTA3_PS2_JP = 1 << 4,
            GTA3_PS2_NAEU = 1 << 5,
            GTA3_Xbox = 1 << 6
        }

        // TODO: renamme to FileTypes
        public static class FileFormats
        {
            public static readonly SaveFileFormat Android = new SaveFileFormat(
                "Android", "Android",
                new GameConsole(ConsoleType.Android)
            );

            public static readonly SaveFileFormat iOS = new SaveFileFormat(
                "iOS", "iOS",
                new GameConsole(ConsoleType.iOS)
            );

            public static readonly SaveFileFormat PC = new SaveFileFormat(
                "PC", "PC (Windows/macOS)",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.MacOS),
                new GameConsole(ConsoleType.Win32, ConsoleFlags.Steam),
                new GameConsole(ConsoleType.MacOS, ConsoleFlags.Steam)
            );

            public static readonly SaveFileFormat PS2_AU = new SaveFileFormat(
                "PS2_AU", "PS2 (PAL, Australia)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.Australia)
            );

            public static readonly SaveFileFormat PS2_JP = new SaveFileFormat(
                "PS2_JP", "PS2 (NTSC-J)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.Japan)
            );

            public static readonly SaveFileFormat PS2_NAEU = new SaveFileFormat(
                "PS2_NAEU", "PS2 (NTSC-U/C & PAL)",
                new GameConsole(ConsoleType.PS2, ConsoleFlags.NorthAmerica | ConsoleFlags.Europe)
            );

            public static readonly SaveFileFormat Xbox = new SaveFileFormat(
                "Xbox", "Xbox",
                new GameConsole(ConsoleType.Xbox)
            );

            public static SaveFileFormat[] GetAll()
            {
                return new SaveFileFormat[] { Android, iOS, PC, PS2_AU, PS2_JP, PS2_NAEU, Xbox };
            }
        }
    }
}
