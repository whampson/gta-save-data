using GTASaveData.Types;
using GTASaveData.Interfaces;
using GTASaveData.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using GTASaveData.Extensions;

namespace GTASaveData.GTA3
{
    /// <summary>
    /// The data block in GTA3 save files that stores mission script state.
    /// </summary>
    public class ScriptsBlock : SaveDataObject,
        IEquatable<ScriptsBlock>, IDeepClonable<ScriptsBlock>
    {
        private const int KeyLengthInScript = 8;

        private const int ScriptDataSize = 968;

        private ScriptInfo m_scriptInfo;
        private ObservableArray<byte> m_scriptSpace;
        private int m_onAMissionFlag;
        private ObservableArray<Contact> m_contacts;
        private ObservableArray<Collective> m_collectives;    // not used
        private int m_nextFreeCollectiveIndex;      // not used
        private ObservableArray<BuildingSwap> m_buildingSwapArray;
        private ObservableArray<InvisibleObject> m_invisibilitySettingArray;
        private bool m_usingAMultiScriptFile;
        private int m_mainScriptSize;
        private int m_largestMissionScriptSize;
        private short m_numberOfMissionScripts;
        private ObservableArray<RunningScript> m_activeScripts;

        /// <summary>
        /// MAIN.SCM metadata including label names, global variable names, and
        /// mission offsets.
        /// </summary>
        /// <remarks>
        /// This data is only available on saves compatible with
        /// <i>Grand Theft Auto III - The Definitive Edition</i>.
        /// </remarks>
        public ScriptInfo ScriptInfo
        {
            get { return m_scriptInfo; }
            set { m_scriptInfo = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// A chunk of MAIN.SCM which stores the game's global variables.
        /// </summary>
        /// <remarks>
        /// Due to the nature of how the game saves and loads global variables, it's actually
        /// possible to embed SCM code in this array. Have fun! :-)
        /// </remarks>
        [JsonConverter(typeof(ByteArrayConverter))]
        public ObservableArray<byte> ScriptSpace
        {
            get { return m_scriptSpace; }
            set { m_scriptSpace = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Global variables.
        /// </summary>
        /// <remarks>
        /// Each global variable is a DWORD from the <see cref="ScriptSpace"/>.
        /// Control the number of globals by calling <see cref="SetSizeOfVariableSpace(int)"/>.
        /// </remarks>
        [JsonIgnore]
        public IEnumerable<int> Globals
        {
            get
            {
                for (int i = 0; i < GetSizeOfVariableSpace() / 4; i++)
                {
                    yield return GetGlobal(i);
                }
            }
        }

        /// <summary>
        /// The offset in <see cref="ScriptSpace"/> of the $ONMISSION variable.
        /// </summary>
        public int OnAMissionFlag
        {
            get { return m_onAMissionFlag; }
            set { m_onAMissionFlag = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Data for a cut feature that would have shown a different wasted/busted message
        /// for each mission contact when the player died or got busted on a mission.
        /// </summary>
        /// <remarks>
        /// This data is not used in the vanilla game.
        /// </remarks>
        public ObservableArray<Contact> Contacts
        {
            get { return m_contacts; }
            set { m_contacts = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Data for an unused feature releated to peds.
        /// </summary>
        /// <remarks>
        /// This data is not used in the vanilla game (to my knowledge).
        /// </remarks>
        public ObservableArray<Collective> Collectives
        {
            get { return m_collectives; }
            set { m_collectives = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The next available index in the <see cref="Collectives"/> array.
        /// </summary>
        /// <remarks>
        /// This data is not used in the vanilla game (to my knowledge).
        /// </remarks>
        public int NextFreeCollectiveIndex
        {
            get { return m_nextFreeCollectiveIndex; }
            set { m_nextFreeCollectiveIndex = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Building model info for script-controlled building model swaps.
        /// (e.g. Callahan Bridge parts, Triad fish factory, Yakuza cafe)
        /// </summary>
        public ObservableArray<BuildingSwap> BuildingSwaps
        {
            get { return m_buildingSwapArray; }
            set { m_buildingSwapArray = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Model info for objects whose visibility is controlled by the script.
        /// (e.g. bridge barriers)
        /// </summary>
        public ObservableArray<InvisibleObject> InvisibilitySettings
        {
            get { return m_invisibilitySettingArray; }
            set { m_invisibilitySettingArray = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indicates whether the game script was compiled from multiple smaller scripts.
        /// </summary>
        /// <remarks>
        /// This should be set to true in a vanilla game.
        /// </remarks>
        public bool UsingAMultiScriptFile
        {
            get { return m_usingAMultiScriptFile; }
            set { m_usingAMultiScriptFile = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The size of the 'MAIN' script code in bytes.
        /// </summary>
        public int MainScriptSize
        {
            get { return m_mainScriptSize; }
            set { m_mainScriptSize = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The size of the largest mission script code in bytes.
        /// </summary>
        public int LargestMissionScriptSize
        {
            get { return m_largestMissionScriptSize; }
            set { m_largestMissionScriptSize = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The total number of mission scripts compiled into MAIN.SCM.
        /// </summary>
        public short NumberOfMissionScripts
        {
            get { return m_numberOfMissionScripts; }
            set { m_numberOfMissionScripts = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The saved state of every active mission script.
        /// </summary>
        public ObservableArray<RunningScript> RunningScripts
        {
            get { return m_activeScripts; }
            set { m_activeScripts = value; OnPropertyChanged(); }
        }

        public ScriptsBlock()
        {
            ScriptInfo = new ScriptInfo();
            ScriptSpace = new ObservableArray<byte>();
            Contacts = new ObservableArray<Contact>();
            Collectives = new ObservableArray<Collective>();
            BuildingSwaps = new ObservableArray<BuildingSwap>();
            InvisibilitySettings = new ObservableArray<InvisibleObject>();
            RunningScripts = new ObservableArray<RunningScript>();
        }

        public ScriptsBlock(ScriptsBlock other)
        {
            ScriptInfo = new ScriptInfo(other.ScriptInfo);
            ScriptSpace = ArrayHelper.DeepClone(other.ScriptSpace);
            OnAMissionFlag = other.OnAMissionFlag;
            Contacts = ArrayHelper.DeepClone(other.Contacts);
            Collectives = ArrayHelper.DeepClone(other.Collectives);
            NextFreeCollectiveIndex = other.NextFreeCollectiveIndex;
            BuildingSwaps = ArrayHelper.DeepClone(other.BuildingSwaps);
            InvisibilitySettings = ArrayHelper.DeepClone(other.InvisibilitySettings);
            UsingAMultiScriptFile = other.UsingAMultiScriptFile;
            MainScriptSize = other.MainScriptSize;
            LargestMissionScriptSize = other.LargestMissionScriptSize;
            NumberOfMissionScripts = other.NumberOfMissionScripts;
            RunningScripts = ArrayHelper.DeepClone(other.RunningScripts);
        }

        public RunningScript StartNewScript(int ip)
        {
            RunningScript script = new RunningScript() { IP = ip };
            RunningScripts.Add(script);

            return script;
        }

        /// <summary>
        /// Returns the <see cref="RunningScript"/> with the specified name,
        /// or null if none can be found. NOTE: if multiple scripts share
        /// a name, the first script in the list with the matching name will
        /// be returned.
        /// </summary>
        public RunningScript GetRunningScriptByName(string name)
        {
            return RunningScripts.Where(x => x.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Grows or shrinks the <see cref="ScriptSpace"/> by the
        /// specified amount. An <paramref name="amount"/> greater
        /// than zero will expand, while an <paramref name="amount"/>
        /// less than zero will shrink.
        /// </summary>
        /// <remarks>
        /// The game may crash or fail to load the save file if the
        /// script space is too large.
        /// </remarks>
        public int ResizeScriptSpace(int amount)
        {
            int oldSize = ScriptSpace.Count;
            int newSize = oldSize + amount;
            byte[] newSciptSpace = new byte[newSize];

            ScriptSpace.CopyTo(0, newSciptSpace, 0, Math.Min(oldSize, newSize));
            ScriptSpace = newSciptSpace;
            return newSize;
        }

        /// <summary>
        /// Gets the size of the global variable pool in the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <remarks>
        /// By default, this is equal to the size of the script space,
        /// but this value can be manipulated to separate global variable
        /// space from injected script code. This value occupies global variable
        /// indices 0 and 1, hence why these are not valid global variables.
        /// </remarks>
        public int GetSizeOfVariableSpace()
        {
            return Read4BytesFromScript(3);
        }

        /// <summary>
        /// Sets the size of the global variable pool in the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <remarks>
        /// By default, this is equal to the size of the script space,
        /// but this value can be manipulated to separate global variable
        /// space from injected script code. This value occupies global variable
        /// indices 0 and 1, hence why these are not valid global variables.
        /// </remarks>
        public void SetSizeOfVariableSpace(int size)
        {
            // Globals, object names, and some script info are stored in MAIN.SCM
            // in header chunks before any actual script code. Each chunk starts with a
            // 'GOTO <next_chunk_address>' command so the command processor can skip
            // over these chunks when the game initializes. In a bit of a clever design,
            // the game derives the size of the global variable chunk from the GOTO
            // address stored at the start of the chunk (this is why you can't use
            // global variables 0 and 1). The GOTO opcode takes up the first two bytes,
            // followed by the operand type for one byte, and finally a four-byte address
            // beginning at offset 3. (02 00 01 xx xx xx xx)
            Write4BytesToScript(size, 3);
        }

        /// <summary>
        /// Gets the value of a global variable.
        /// </summary>
        public int GetGlobal(int index)
        {
            return Read4BytesFromScript(index * 4);
        }

        /// <summary>
        /// Gets the value of a global variable as a float.
        /// </summary>
        public float GetGlobalAsFloat(int index)
        {
            int bits = Read4BytesFromScript(index * 4);
            return BitConverter.Int32BitsToSingle(bits);
        }

        /// <summary>
        /// Sets the value of a global variable.
        /// </summary>
        public void SetGlobal(int index, int value)
        {
            Write4BytesToScript(index * 4, value);
        }

        /// <summary>
        /// Sets the value of a global variable as a float.
        /// </summary>
        public void SetGlobal(int index, float value)
        {
            int bits = BitConverter.SingleToInt32Bits(value);
            Write4BytesToScript(index * 4, bits);
        }

        /// <summary>
        /// Gets the value of the <c>$ONMISSION</c> variable.
        /// </summary>
        public bool GetPlayerIsOnAMission()
        {
            return Read4BytesFromScript(OnAMissionFlag) == 1;
        }

        /// <summary>
        /// Sets the value of the <c>$ONMISSION</c> variable.
        /// </summary>
        public void SetPlayerIsOnAMission(bool onMission)
        {
            Write4BytesToScript(OnAMissionFlag, (onMission) ? 1 : 0);
        }

        /// <summary>
        /// Reads a 4-byte value from the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        public int Read4BytesFromScript(int ip)
        {
            _ = Read4BytesFromScript(ip, out int value);
            return value;
        }

        /// <summary>
        /// Reads a 4-byte value from the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        /// <param name="value">
        /// The value read.
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int Read4BytesFromScript(int ip, out int value)
        {
            value = ScriptSpace[ip++]
                | ScriptSpace[ip++] << 8
                | ScriptSpace[ip++] << 16
                | ScriptSpace[ip++] << 24;
            return 4;
        }

        /// <summary>
        /// Reads a 2-byte value from the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        public short Read2BytesFromScript(int ip)
        {
            _ = Read2BytesFromScript(ip, out short value);
            return value;
        }

        /// <summary>
        /// Reads a 2-byte value from the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        /// <param name="value">
        /// The value read.
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int Read2BytesFromScript(int ip, out short value)
        {
            value = (short) (ScriptSpace[ip++] | ScriptSpace[ip++] << 8);
            return 2;
        }

        /// <summary>
        /// Reads a 1-byte value from the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        public byte Read1ByteFromScript(int ip)
        {
            _ = Read1ByteFromScript(ip, out byte value);
            return value;
        }

        /// <summary>
        /// Reads a 1-byte value from the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        /// <param name="value">
        /// The value read.
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int Read1ByteFromScript(int ip, out byte value)
        {
            value = ScriptSpace[ip++];
            return 1;
        }

        /// <summary>
        /// Reads a floating-point value to the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <remarks>
        /// NOTE: this really a 16-bit fixed-point value.
        /// </remarks>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        public float ReadFloatFromScript(int ip)
        {
            _ = ReadFloatFromScript(ip, out float value);
            return value;
        }

        /// <summary>
        /// Reads a floating-point value to the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <remarks>
        /// NOTE: this really a 16-bit fixed-point value.
        /// </remarks>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        /// <param name="value">
        /// The value read.
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int ReadFloatFromScript(int ip, out float value)
        {
            int ret = Read2BytesFromScript(ip, out short s);
            value = s / 16.0f;
            return ret;

        }

        /// <summary>
        /// Reads a GXT key from the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        public string ReadTextLabelFromScript(int ip)
        {
            _ = ReadTextLabelFromScript(ip, out string value);
            return value;
        }

        /// <summary>
        /// Reads a GXT key from the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to read (instruction pointer).
        /// </param>
        /// <param name="value">
        /// The value read.
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int ReadTextLabelFromScript(int ip, out string value)
        {
            byte[] data = new byte[KeyLengthInScript];
            ScriptSpace.CopyTo(ip, data, 0, KeyLengthInScript);

            value = Encoding.ASCII.GetString(data).TrimFromZero();
            return KeyLengthInScript;
        }

        /// <summary>
        /// Writes a 4-byte value to the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to write (instruction pointer).
        /// </param>
        /// <param name="value">
        /// The value to write.
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int Write4BytesToScript(int ip, int value)
        {
            ScriptSpace[ip++] = (byte) (value & 0xFF);
            ScriptSpace[ip++] = (byte) ((value >> 8) & 0xFF);
            ScriptSpace[ip++] = (byte) ((value >> 16) & 0xFF);
            ScriptSpace[ip++] = (byte) ((value >> 24) & 0xFF);
            return 4;
        }

        /// <summary>
        /// Writes a 2-byte value to the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to write (instruction pointer).
        /// </param>
        /// <param name="value">
        /// The value to write.
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int Write2BytesToScript(int ip, short value)
        {
            ScriptSpace[ip++] = (byte) (value & 0xFF);
            ScriptSpace[ip++] = (byte) ((value >> 8) & 0xFF);
            return 2;
        }

        /// <summary>
        /// Writes a 1-byte value to the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to write (instruction pointer).
        /// </param>
        /// <param name="value">
        /// The value to write.
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int Write1ByteToScript(int ip, byte value)
        {
            ScriptSpace[ip++] = value;
            return 1;
        }

        /// <summary>
        /// Writes a float value to the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <remarks>
        /// NOTE: this really a 16-bit fixed-point value.
        /// </remarks>
        /// <param name="ip">
        /// The byte offset of the value to write (instruction pointer).
        /// </param>
        /// <param name="value">
        /// The value to write.
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int WriteFloatToScript(int ip, float value)
        {
            return Write2BytesToScript(ip, (short) (value * 16.0f));
        }

        /// <summary>
        /// Writes a GXT key to the <see cref="ScriptSpace"/>.
        /// </summary>
        /// <param name="ip">
        /// The byte offset of the value to write (instruction pointer).
        /// </param>
        /// <param name="label">
        /// The text value to write. Max length: 8 (NUL-terminated)
        /// </param>
        /// <returns>
        /// The number of bytes read.
        /// </returns>
        public int WriteTextLabelToScript(int ip, string label)
        {
            int len = Math.Min(label.Length, KeyLengthInScript - 1);
            for (int i = 0; i < KeyLengthInScript; i++)
            {
                ScriptSpace[ip++] = (i < len)
                    ? (byte) label[i]
                    : (byte) 0;
            }

            return KeyLengthInScript;
        }

        protected override void ReadData(DataBuffer buf, SerializationParams prm)
        {
            var p = (GTA3SaveParams) prm;
            var t = p.FileType;

            if (t.FlagDE)
            {
                ScriptInfo = buf.ReadObject<ScriptInfo>();
            }

            buf.Mark();
            int size = GTA3Save.ReadBlockHeader(buf, out string tag);
            Debug.Assert(tag == "SCR");

            int varSpace = buf.ReadInt32();
            ScriptSpace = buf.ReadArray<byte>(varSpace);
            int unusedSize = buf.ReadInt32();
            Debug.Assert(unusedSize == sizeof(int)
                + SizeOf<Contact>(prm) * p.NumContacts
                + SizeOf<Collective>(prm) * p.NumCollectives
                + sizeof(int)
                + SizeOf<BuildingSwap>(prm) * p.NumBuildingSwaps
                + SizeOf<InvisibleObject>(prm) * p.NumInvisibilitySettings
                + 4 * sizeof(int));
            OnAMissionFlag = buf.ReadInt32();
            Contacts = buf.ReadArray<Contact>(p.NumContacts);
            Collectives = buf.ReadArray<Collective>(p.NumCollectives);
            NextFreeCollectiveIndex = buf.ReadInt32();
            BuildingSwaps = buf.ReadArray<BuildingSwap>(p.NumBuildingSwaps);
            InvisibilitySettings = buf.ReadArray<InvisibleObject>(p.NumInvisibilitySettings);
            UsingAMultiScriptFile = buf.ReadBool();
            buf.ReadByte();
            buf.ReadUInt16();
            MainScriptSize = buf.ReadInt32();
            LargestMissionScriptSize = buf.ReadInt32();
            NumberOfMissionScripts = buf.ReadInt16();
            buf.ReadUInt16();
            int runningScripts = buf.ReadInt32();
            RunningScripts = buf.ReadArray<RunningScript>(runningScripts, p);

            Debug.Assert(buf.Offset == size + GTA3Save.BlockHeaderSize);
            Debug.Assert(size == SizeOf(this, p) - GTA3Save.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, SerializationParams prm)
        {
            var p = (GTA3SaveParams) prm;

            int size = SizeOf(this, p);
            int unusedSize = sizeof(int)
                + SizeOf<Contact>(prm) * p.NumContacts
                + SizeOf<Collective>(prm) * p.NumCollectives
                + sizeof(int)
                + SizeOf<BuildingSwap>(prm) * p.NumBuildingSwaps
                + SizeOf<InvisibleObject>(prm) * p.NumInvisibilitySettings
                + 4 * sizeof(int);

            GTA3Save.WriteBlockHeader(buf, "SCR", size - GTA3Save.BlockHeaderSize);

            buf.Write(ScriptSpace.Count);
            buf.Write(ScriptSpace);
            buf.Write(unusedSize);
            buf.Write(OnAMissionFlag);
            buf.Write(Contacts, p.NumContacts);
            buf.Write(Collectives, p.NumCollectives);
            buf.Write(NextFreeCollectiveIndex);
            buf.Write(BuildingSwaps, p.NumBuildingSwaps);
            buf.Write(InvisibilitySettings, p.NumInvisibilitySettings);
            buf.Write(UsingAMultiScriptFile);
            buf.Write((byte) 0);
            buf.Write((short) 0);
            buf.Write(MainScriptSize);
            buf.Write(LargestMissionScriptSize);
            buf.Write(NumberOfMissionScripts);
            buf.Write((short) 0);
            buf.Write(RunningScripts.Count);
            buf.Write(RunningScripts, p);

            Debug.Assert(buf.Offset == size);
        }

        protected override int GetSize(SerializationParams prm)
        {
            var p = (GTA3SaveParams) prm;
            if (prm.FileType.FlagDE) throw new NotImplementedException();

            return GTA3Save.BlockHeaderSize
                + sizeof(int)
                + ScriptSpace.Count
                + sizeof(int)
                + sizeof(int)
                + SizeOf<Contact>(prm) * p.NumContacts
                + SizeOf<Collective>(prm) * p.NumCollectives
                + sizeof(int)
                + SizeOf<BuildingSwap>(prm) * p.NumBuildingSwaps
                + SizeOf<InvisibleObject>(prm) * p.NumInvisibilitySettings
                + 5 * sizeof(int)
                + SizeOf<RunningScript>(prm) * RunningScripts.Count;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ScriptsBlock);
        }

        public bool Equals(ScriptsBlock other)
        {
            if (other == null)
            {
                return false;
            }

            return ScriptSpace.SequenceEqual(other.ScriptSpace)
                && OnAMissionFlag.Equals(other.OnAMissionFlag)
                && Contacts.SequenceEqual(other.Contacts)
                && Collectives.SequenceEqual(other.Collectives)
                && NextFreeCollectiveIndex.Equals(other.NextFreeCollectiveIndex)
                && BuildingSwaps.SequenceEqual(other.BuildingSwaps)
                && InvisibilitySettings.SequenceEqual(other.InvisibilitySettings)
                && UsingAMultiScriptFile.Equals(other.UsingAMultiScriptFile)
                && MainScriptSize.Equals(other.MainScriptSize)
                && LargestMissionScriptSize.Equals(other.LargestMissionScriptSize)
                && NumberOfMissionScripts.Equals(other.NumberOfMissionScripts)
                && RunningScripts.SequenceEqual(other.RunningScripts);
        }

        public ScriptsBlock DeepClone()
        {
            return new ScriptsBlock(this);
        }
    }
}
