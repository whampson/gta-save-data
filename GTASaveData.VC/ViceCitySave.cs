using GTASaveData.Common;
using GTASaveData.Extensions;
using GTASaveData.Serialization;
using GTASaveData.VC.Blocks;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData.VC
{
    /// <summary>
    /// Represents a save file for <i>Grand Theft Auto: Vice City</i>.
    /// </summary>
    public sealed class ViceCitySave : GrandTheftAutoSave,
        IGrandTheftAutoSave,
        IEquatable<ViceCitySave>
    {
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

        public Block Scripts
        {
            get { return m_blocks[1] as Block; }
            set { m_blocks[1] = value; OnPropertyChanged(); }
        }

        public Block PedPool
        {
            get { return m_blocks[2] as Block; }
            set { m_blocks[2] = value; OnPropertyChanged(); }
        }

        public Block Garages
        {
            get { return m_blocks[3] as Block; }
            set { m_blocks[3] = value; OnPropertyChanged(); }
        }

        public Block GameLogic
        {
            get { return m_blocks[4] as Block; }
            set { m_blocks[4] = value; OnPropertyChanged(); }
        }

        public Block VehiclePool
        {
            get { return m_blocks[5] as Block; }
            set { m_blocks[5] = value; OnPropertyChanged(); }
        }

        public Block ObjectPool
        {
            get { return m_blocks[6] as Block; }
            set { m_blocks[6] = value; OnPropertyChanged(); }
        }

        public Block PathFind
        {
            get { return m_blocks[7] as Block; }
            set { m_blocks[7] = value; OnPropertyChanged(); }
        }

        public Block Cranes
        {
            get { return m_blocks[8] as Block; }
            set { m_blocks[8] = value; OnPropertyChanged(); }
        }

        public Block Pickups
        {
            get { return m_blocks[9] as Block; }
            set { m_blocks[9] = value; OnPropertyChanged(); }
        }

        public Block PhoneInfo
        {
            get { return m_blocks[10] as Block; }
            set { m_blocks[10] = value; OnPropertyChanged(); }
        }

        public Block RestartPoints
        {
            get { return m_blocks[11] as Block; }
            set { m_blocks[11] = value; OnPropertyChanged(); }
        }

        public Block RadarBlips
        {
            get { return m_blocks[12] as Block; }
            set { m_blocks[12] = value; OnPropertyChanged(); }
        }

        public Block Zones
        {
            get { return m_blocks[13] as Block; }
            set { m_blocks[13] = value; OnPropertyChanged(); }
        }

        public Block GangData
        {
            get { return m_blocks[14] as Block; }
            set { m_blocks[14] = value; OnPropertyChanged(); }
        }

        public CarGeneratorBlock CarGenerators
        {
            get { return m_blocks[15] as CarGeneratorBlock; }
            set { m_blocks[15] = value; OnPropertyChanged(); }
        }

        public Block Particles
        {
            get { return m_blocks[16] as Block; }
            set { m_blocks[16] = value; OnPropertyChanged(); }
        }

        public Block AudioScriptObjects
        {
            get { return m_blocks[17] as Block; }
            set { m_blocks[17] = value; OnPropertyChanged(); }
        }

        public Block ScriptPaths
        {
            get { return m_blocks[18] as Block; }
            set { m_blocks[18] = value; OnPropertyChanged(); }
        }

        public Block PlayerInfo
        {
            get { return m_blocks[19] as Block; }
            set { m_blocks[19] = value; OnPropertyChanged(); }
        }

        public Block Stats
        {
            get { return m_blocks[20] as Block; }
            set { m_blocks[20] = value; OnPropertyChanged(); }
        }

        public Block SetPieces
        {
            get { return m_blocks[21] as Block; }
            set { m_blocks[21] = value; OnPropertyChanged(); }
        }

        public Block Streaming
        {
            get { return m_blocks[22] as Block; }
            set { m_blocks[22] = value; OnPropertyChanged(); }
        }

        public Block PedTypeInfo
        {
            get { return m_blocks[23] as Block; }
            set { m_blocks[23] = value; OnPropertyChanged(); }
        }

        ISimpleVars IGrandTheftAutoSave.SimpleVars => SimpleVars;

<<<<<<< HEAD
<<<<<<< Updated upstream
        public override string Name => SimpleVars.LastMissionPassedName;
=======
        ICarGeneratorBlock IGrandTheftAutoSave.CarGenerators => CarGenerators;

        public override string Name => SimpleVars.SaveName;
>>>>>>> Stashed changes
=======
        public override string Name => SimpleVars.SaveName;
>>>>>>> san-andreas

        protected override int MaxBlockSize => 0xD6D8;      // TODO: PS2 

        protected override int BlockCount => 24;

        protected override int SectionCount
        {
            get
            {
                if (FileFormat.SupportsPC)
                {
                    return 23;
                }

                throw new InvalidOperationException("Not implemented!");
            }
        }

        protected override int SimpleVarsSize
        {
            get
            {
                if (FileFormat == FileFormats.PCRetail)
                {
                    return 0xE4;
                }
                else if (FileFormat == FileFormats.PCSteam)
                {
                    return 0xE8;
                }

                throw new InvalidOperationException("Not implemented!");
            }
        }

        public ViceCitySave()
        {
            m_blocks[0] = new SimpleVars();
            m_blocks[1] = new Block();
            m_blocks[2] = new Block();
            m_blocks[3] = new Block();
            m_blocks[4] = new Block();
            m_blocks[5] = new Block();
            m_blocks[6] = new Block();
            m_blocks[7] = new Block();
            m_blocks[8] = new Block();
            m_blocks[9] = new Block();
            m_blocks[10] = new Block();
            m_blocks[11] = new Block();
            m_blocks[12] = new Block();
            m_blocks[13] = new Block();
            m_blocks[14] = new Block();
            m_blocks[15] = new CarGeneratorBlock();
            m_blocks[16] = new Block();
            m_blocks[17] = new Block();
            m_blocks[18] = new Block();
            m_blocks[19] = new Block();
            m_blocks[20] = new Block();
            m_blocks[21] = new Block();
            m_blocks[22] = new Block();
            m_blocks[23] = new Block();
        }

        protected override FileFormat DetectFileFormat(byte[] data)
        {
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

            throw new InvalidOperationException("Not implemented!");
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

        protected override void LoadSection(int index, byte[] data)
        {
            using (Serializer r = CreateSerializer(new MemoryStream(data)))
            {
                // TODO: PS2
                switch (index)
                {
                    case 0:
                        SimpleVars = Deserialize<SimpleVars>(r.ReadBytes(SimpleVarsSize));
                        Scripts = ReadBlock(r, ScrTag);
                        break;
                    case 1: PedPool = ReadBlock(r, null); break;
                    case 2: Garages = ReadBlock(r, null); break;
                    case 3: GameLogic = ReadBlock(r, null); break;
                    case 4: VehiclePool = ReadBlock(r, null); break;
                    case 5: ObjectPool = ReadBlock(r, null); break;
                    case 6: PathFind = ReadBlock(r, null); break;
                    case 7: Cranes = ReadBlock(r, null); break;
                    case 8: Pickups = ReadBlock(r, null); break;
                    case 9: PhoneInfo = ReadBlock(r, null); break;
                    case 10: RestartPoints = ReadBlock(r, RstTag); break;
                    case 11: RadarBlips = ReadBlock(r, RdrTag); break;
                    case 12: Zones = ReadBlock(r, ZnsTag); break;
                    case 13: GangData = ReadBlock(r, GngTag); break;
                    case 14: CarGenerators = Deserialize<CarGeneratorBlock>(ReadBlock(r, CgnTag)); break;
                    case 15: Particles = ReadBlock(r, null); break;
                    case 16: AudioScriptObjects = ReadBlock(r, AudTag); break;
                    case 17: ScriptPaths = ReadBlock(r, null); break;
                    case 18: PlayerInfo = ReadBlock(r, null); break;
                    case 19: Stats = ReadBlock(r, null); break;
                    case 20: SetPieces = ReadBlock(r, null); break;
                    case 21: Streaming = ReadBlock(r, null); break;
                    case 22: PedTypeInfo = ReadBlock(r, PtpTag); break;
                }
            }
        }

        protected override byte[] SaveSection(int index)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (Serializer w = CreateSerializer(m))
                {
                    // TODO: PS2
                    switch (index)
                    {
                        case 0:
                            w.Write(Serialize(SimpleVars));
                            w.Write(CreateBlock(ScrTag, Scripts));
                            break;
                        case 1: w.Write(CreateBlock(null, PedPool)); break;
                        case 2: w.Write(CreateBlock(null, Garages)); break;
                        case 3: w.Write(CreateBlock(null, GameLogic)); break;
                        case 4: w.Write(CreateBlock(null, VehiclePool)); break;
                        case 5: w.Write(CreateBlock(null, ObjectPool)); break;
                        case 6: w.Write(CreateBlock(null, PathFind)); break;
                        case 7: w.Write(CreateBlock(null, Cranes)); break;
                        case 8: w.Write(CreateBlock(null, Pickups)); break;
                        case 9: w.Write(CreateBlock(null, PhoneInfo)); break;
                        case 10: w.Write(CreateBlock(RstTag, RestartPoints)); break;
                        case 11: w.Write(CreateBlock(RdrTag, RadarBlips)); break;
                        case 12: w.Write(CreateBlock(ZnsTag, Zones)); break;
                        case 13: w.Write(CreateBlock(GngTag, GangData)); break;
                        case 14: w.Write(CreateBlock(CgnTag, Serialize(CarGenerators))); break;
                        case 15: w.Write(CreateBlock(null, Particles)); break;
                        case 16: w.Write(CreateBlock(AudTag, AudioScriptObjects)); break;
                        case 17: w.Write(CreateBlock(null, ScriptPaths)); break;
                        case 18: w.Write(CreateBlock(null, PlayerInfo)); break;
                        case 19: w.Write(CreateBlock(null, Stats)); break;
                        case 20: w.Write(CreateBlock(null, SetPieces)); break;
                        case 21: w.Write(CreateBlock(null, Streaming)); break;
                        case 22: w.Write(CreateBlock(PtpTag, PedTypeInfo)); break;
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
            Debug.WriteLine("Loaded Vice City -- total bytes: {0}, total sections: {1}, padding: {2}",
                bytesRead, numSectionsRead, numSectionsRead - index);

            int sizeOfGame = (int) Serializer.GetAlignedAddress(SimpleVars.SizeOfGameInBytes);
            Debug.Assert(bytesRead == (sizeOfGame - 4));
            Debug.Assert(r.BaseStream.Position == sizeOfGame + (4 * numSectionsRead) - 4);
#endif
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

                lengthSum = Serializer.Serialize(data.Length).Sum(x => x);
                checksum += data.Sum(x => x);
                bytesWritten += data.Length;
                numSectionsWritten++;
            }

            Debug.WriteLine("Saved Vice City -- total bytes: {0}, total sections: {1}, padding: {2}",
                bytesWritten, numSectionsWritten, numSectionsWritten - index);

            Debug.Assert(bytesWritten == (sizeOfGame - 4));
            Debug.Assert(w.BaseStream.Position == sizeOfGame + (4 * numSectionsWritten) - 4);

            w.Write(checksum);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ViceCitySave);
        }

        public bool Equals(ViceCitySave other)
        {
            if (other == null)
            {
                return false;
            }

            return m_blocks.SequenceEqual(other.m_blocks);
        }

        public static class FileFormats
        {
            public static readonly FileFormat PCRetail = new FileFormat(
                "PC_Retail", "PC (Windows/macOS)",
                new GameConsole(ConsoleType.Win32),
                new GameConsole(ConsoleType.MacOS, ConsoleFlags.Steam)
            );

            public static readonly FileFormat PCSteam = new FileFormat(
                "PC_Steam", "PC (Windows, Steam)",
                new GameConsole(ConsoleType.Win32, ConsoleFlags.Steam)
            );

            public static FileFormat[] GetAll()
            {
                return new FileFormat[] { PCRetail, PCSteam };
            }
        }
    }
}
