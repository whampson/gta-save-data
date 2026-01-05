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
    public class ScriptBlock : SaveDataObject,
        IEquatable<ScriptBlock>, IDeepClonable<ScriptBlock>
    {
        /// <summary>
        /// The number of bytes a GXT key occupies in mission script code.
        /// </summary>
        public const int KeyLengthInScript = 8;

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
        /// A chunk of <c>MAIN.SCM</c> which normally stores the game's global variables.
        /// </summary>
        /// <remarks>
        /// When the game is saved, the first chunk of <c>MAIN.SCM</c> from the
        /// game's memory is saved in this array. This chunk is normally used to
        /// store the global variables. Each DWORD in this area represents one global
        /// variable, except for the first two, which are used to store the total size
        /// of the space. When the game is loaded from a save file, the global variable
        /// space from the save is pasted over the game's in-memory copy of <c>MAIN.SCM</c>.
        /// <para>
        /// This leaves room for an interesting exploit; the game does not do any size-checking
        /// when loading this buffer into memory, meaning we can make it as large as we want
        /// (or as large as the save file permits) and insert our own SCM code! This is why
        /// this variable is called <c>ScriptSpace</c>. The function
        /// <see cref="SetGlobalVariableSpaceSize(int)"/> can be used to control how much of
        /// the script space is used for global variables. Everything in the buffer that follows
        /// after the global variables can be used for run-once SCM code. If you want SCM code to
        /// persist in the save, insert it within the global variable space, but be sure not to
        /// trample over any existing variables if you still want the missions to function normally
        /// alongside your custom code. Happy hacking! :)
        /// </para>
        /// </remarks>
        /// <seealso cref="GrowScriptSpace(int)"/>
        /// <seealso cref="ShrinkScriptSpace(int)"/>
        /// <seealso cref="GetScriptSpaceSize"/>
        /// <seealso cref="GetGlobalVariableSpaceSize"/>
        /// <seealso cref="SetGlobalVariableSpaceSize(int)"/>
        /// <seealso cref="Read1ByteFromScript(int)"/>
        /// <seealso cref="Read2BytesFromScript(int)"/>
        /// <seealso cref="Read4BytesFromScript(int)"/>
        /// <seealso cref="ReadFloatFromScript(int)"/>
        /// <seealso cref="ReadTextLabelFromScript(int)"/>
        /// <seealso cref="Write1ByteToScript(int, byte)"/>
        /// <seealso cref="Write2BytesToScript(int, short)"/>
        /// <seealso cref="Write4BytesToScript(int, int)"/>
        /// <seealso cref="WriteFloatToScript(int, float)"/>
        /// <seealso cref="WriteTextLabelToScript(int, string)"/>
        [JsonConverter(typeof(ByteArrayConverter))]
        public ObservableArray<byte> ScriptSpace
        {
            get { return m_scriptSpace; }
            set { m_scriptSpace = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Global variables list.
        /// </summary>
        /// <remarks>
        /// Each global variable is a DWORD from the <see cref="ScriptSpace"/>.
        /// Control the number of globals by calling <see cref="SetGlobalVariableSpaceSize(int)"/>.
        /// </remarks>
        /// <seealso cref="GetGlobalVariable(int)"/>
        /// <seealso cref="SetGlobalVariable(int, int)"/>
        /// <seealso cref="SetGlobalVariable(int, float)"/>
        /// <seealso cref="GetGlobalVariableSpaceSize"/>
        /// <seealso cref="SetGlobalVariableSpaceSize(int)"/>
        [JsonIgnore]
        public IEnumerable<int> GlobalVariables
        {
            get
            {
                for (int i = 0; i < GetGlobalVariableSpaceSize() / 4; i++)
                {
                    yield return GetGlobalVariable(i);
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

        public ScriptBlock()
        {
            ScriptInfo = new ScriptInfo();
            ScriptSpace = new ObservableArray<byte>();
            Contacts = new ObservableArray<Contact>();
            Collectives = new ObservableArray<Collective>();
            BuildingSwaps = new ObservableArray<BuildingSwap>();
            InvisibilitySettings = new ObservableArray<InvisibleObject>();
            RunningScripts = new ObservableArray<RunningScript>();
        }

        public ScriptBlock(ScriptBlock other)
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

        /// <summary>
        /// Returns the first building swap object containing the specified model,
        /// or <c>null</c> if none exist.
        /// </summary>
        public BuildingSwap FindBuildingSwap(int model)
        {
            return BuildingSwaps.FirstOrDefault(x => x.OldModel == model || x.NewModel == model);
        }

        /// <summary>
        /// Returns the first building swap object containing the specified handle,
        /// or <c>null</c> if none exist.
        /// </summary>
        public BuildingSwap FindBuildingSwapByHandle(int handle)
        {
            return BuildingSwaps.FirstOrDefault(x => x.Handle == handle);
        }

        /// <summary>
        /// Returns the next available building swap object,
        /// or <c>null</c> if none exist.
        /// </summary>
        public BuildingSwap FindNextFreeBuildingSwapSlot()
        {
            return FindBuildingSwapByHandle(0);
        }

        /// <summary>
        /// Replaces a building model.
        /// </summary>
        /// <param name="handle">The building pool handle.</param>
        /// <param name="oldModel">The old building model number.</param>
        /// <param name="newModel">the new building model number.</param>
        public void SwapBuildingModel(int handle, int oldModel, int newModel)
        {
            BuildingSwap swap = FindBuildingSwapByHandle(handle);
            if (swap != null)
            {
                if (oldModel == newModel)
                {
                    swap.Clear();
                    return;
                }
                swap.Type = EntityClassType.Building;
                swap.OldModel = oldModel;
                swap.NewModel = newModel;
                return;
            }
            
            swap = FindNextFreeBuildingSwapSlot();
            if (swap != null && oldModel != newModel)
            {
                swap.Handle = handle;
                swap.Type = EntityClassType.Building;
                swap.OldModel = oldModel;
                swap.NewModel = newModel;
            }
        }

        /// <summary>
        /// Finds the invisible object containing the matching handle, or <c>null</c> if none exist.
        /// </summary>
        public InvisibleObject FindInvisibleObject(int handle)
        {
            return InvisibilitySettings.FirstOrDefault(x => x.Handle == handle);
        }

        /// <summary>
        /// Finds the next available invisible object, or <c>null</c> if none exist.
        /// </summary>
        public InvisibleObject FindNextFreeInvisibleObjectSlot()
        {
            return FindInvisibleObject(0);
        }

        
        /// <summary>
        /// Changes a building's visibility.
        /// </summary>
        public void SetBuildingVisibility(int handle, bool visible)
        {
            SetEntityVisibility(EntityClassType.Building, handle, visible);
        }

        /// <summary>
        /// Changes an object's visibility.
        /// </summary>
        public void SetEntityVisibility(EntityClassType type, int handle, bool visible)
        {
            if (visible)
            {
                InvisibleObject obj = FindInvisibleObject(handle);
                if (obj != null)
                {
                    obj.Clear();
                }
            }
            else
            {
                InvisibleObject obj = FindNextFreeInvisibleObjectSlot();
                if (obj != null)
                {
                    obj.Handle = handle;
                    obj.Type = type;
                }
            }
        }

        /// <summary>
        /// Adds a new <see cref="RunningScript"/> to the running scripts array.
        /// </summary>
        /// <param name="ip">The initial instruction pointer.</param>
        /// <returns>The newly-created <see cref="RunningScript"/>.</returns>
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
        public int GrowScriptSpace(int amount)
        {
            return amount >= 0
                ? ResizeScriptSpace(amount)
                : throw new ArgumentOutOfRangeException(nameof(amount));
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
        public int ShrinkScriptSpace(int amount)
        {
            return amount >= 0
                ? ResizeScriptSpace(-amount)
                : throw new ArgumentOutOfRangeException(nameof(amount));
        }

        private int ResizeScriptSpace(int amount)
        {
            int oldSize = ScriptSpace.Count;
            int newSize = oldSize + amount;
            byte[] newSciptSpace = new byte[newSize];

            ScriptSpace.CopyTo(0, newSciptSpace, 0, Math.Min(oldSize, newSize));
            ScriptSpace = newSciptSpace;
            return newSize;
        }

        /// <summary>
        /// Gets the total size of the saved script space.
        /// </summary>
        /// <returns></returns>
        public int GetScriptSpaceSize()
        {
            return ScriptSpace.Count;
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
        public int GetGlobalVariableSpaceSize()
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
        public void SetGlobalVariableSpaceSize(int size)
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
            Write4BytesToScript(3, size);
        }

        /// <summary>
        /// Gets the value of a global variable.
        /// </summary>
        public int GetGlobalVariable(int index)
        {
            return Read4BytesFromScript(index * 4);
        }

        /// <summary>
        /// Gets the value of a global variable as a float.
        /// </summary>
        public float GetGlobalVariableFloat(int index)
        {
            int bits = Read4BytesFromScript(index * 4);
            return BitConverter.Int32BitsToSingle(bits);
        }

        /// <summary>
        /// Sets the value of a global variable.
        /// </summary>
        public void SetGlobalVariable(int index, int value)
        {
            Write4BytesToScript(index * 4, value);
        }

        /// <summary>
        /// Sets the value of a global variable as a float.
        /// </summary>
        public void SetGlobalVariable(int index, float value)
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

            if (p.IsDE)
            {
                ScriptInfo = buf.ReadObject<ScriptInfo>();
                // TODO: artificially create this for other script versions
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

            if (p.IsDE)
            {
                buf.Write(ScriptInfo, prm);
            }

            int size = SizeOf(this, p);
            int unusedSize = sizeof(int)
                + SizeOf<Contact>(prm) * p.NumContacts
                + SizeOf<Collective>(prm) * p.NumCollectives
                + sizeof(int)
                + SizeOf<BuildingSwap>(prm) * p.NumBuildingSwaps
                + SizeOf<InvisibleObject>(prm) * p.NumInvisibilitySettings
                + 4 * sizeof(int);

            buf.Mark();
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
            return Equals(obj as ScriptBlock);
        }

        public bool Equals(ScriptBlock other)
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

        public ScriptBlock DeepClone()
        {
            return new ScriptBlock(this);
        }
    }
}
