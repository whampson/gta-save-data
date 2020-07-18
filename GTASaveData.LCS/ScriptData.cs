using GTASaveData.Core.Types;
using GTASaveData.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.LCS
{
    public class ScriptData : SaveDataObject,
        IEquatable<ScriptData>, IDeepClonable<ScriptData>
    {
        public const int NumCollectives = 32;
        public const int NumBuildingSwaps = 80;
        public const int NumInvisibilitySettings = 52;

        private const int ScriptDataSize = 0x6B8;

        private Array<byte> m_scriptSpace;
        private int m_onAMissionFlag;
        private uint m_lastMissionPassedTime;
        private Array<Collective> m_collectives;    // not used
        private int m_nextFreeCollectiveIndex;      // not used
        private Array<BuildingSwap> m_buildingSwapArray;
        private Array<InvisibleObject> m_invisibilitySettingArray;
        private bool m_usingAMultiScriptFile;
        private bool m_playerHasMetDebbieHarry;
        private int m_mainScriptSize;
        private int m_largestMissionScriptSize;
        private short m_numberOfMissionScripts;
        private Array<RunningScript> m_activeScripts;

        [JsonIgnore]
        public int NumGlobalVariables => ScriptSpace.Count / 4;

        [JsonConverter(typeof(ByteArrayConverter))]
        public Array<byte> ScriptSpace
        {
            get { return m_scriptSpace; }
            set { m_scriptSpace = value; OnPropertyChanged(); }
        }

        public int OnAMissionFlag
        {
            get { return m_onAMissionFlag; }
            set { m_onAMissionFlag = value; OnPropertyChanged(); }
        }

        public uint LastMissionPassedTime
        {
            get { return m_lastMissionPassedTime; }
            set { m_lastMissionPassedTime = value; OnPropertyChanged(); }
        }

        public Array<Collective> Collectives
        {
            get { return m_collectives; }
            set { m_collectives = value; OnPropertyChanged(); }
        }

        public int NextFreeCollectiveIndex
        {
            get { return m_nextFreeCollectiveIndex; }
            set { m_nextFreeCollectiveIndex = value; OnPropertyChanged(); }
        }

        public Array<BuildingSwap> BuildingSwaps
        {
            get { return m_buildingSwapArray; }
            set { m_buildingSwapArray = value; OnPropertyChanged(); }
        }

        public Array<InvisibleObject> InvisibilitySettings
        {
            get { return m_invisibilitySettingArray; }
            set { m_invisibilitySettingArray = value; OnPropertyChanged(); }
        }

        public bool UsingAMultiScriptFile
        {
            get { return m_usingAMultiScriptFile; }
            set { m_usingAMultiScriptFile = value; OnPropertyChanged(); }
        }

        public bool PlayerHasMetDebbieHarry
        {
            get { return m_playerHasMetDebbieHarry; }
            set { m_playerHasMetDebbieHarry = value; OnPropertyChanged(); }
        }

        public int MainScriptSize
        {
            get { return m_mainScriptSize; }
            set { m_mainScriptSize = value; OnPropertyChanged(); }
        }

        public int LargestMissionScriptSize
        {
            get { return m_largestMissionScriptSize; }
            set { m_largestMissionScriptSize = value; OnPropertyChanged(); }
        }

        public short NumberOfMissionScripts
        {
            get { return m_numberOfMissionScripts; }
            set { m_numberOfMissionScripts = value; OnPropertyChanged(); }
        }

        public Array<RunningScript> Threads
        {
            get { return m_activeScripts; }
            set { m_activeScripts = value; OnPropertyChanged(); }
        }

        public ScriptData()
        {
            ScriptSpace = new Array<byte>();
            Collectives = ArrayHelper.CreateArray<Collective>(NumCollectives);
            BuildingSwaps = ArrayHelper.CreateArray<BuildingSwap>(NumBuildingSwaps);
            InvisibilitySettings = ArrayHelper.CreateArray<InvisibleObject>(NumInvisibilitySettings);
            Threads = new Array<RunningScript>();
        }

        public ScriptData(ScriptData other)
        {
            ScriptSpace = ArrayHelper.DeepClone(other.ScriptSpace);
            OnAMissionFlag = other.OnAMissionFlag;
            LastMissionPassedTime = other.LastMissionPassedTime;
            Collectives = ArrayHelper.DeepClone(other.Collectives);
            NextFreeCollectiveIndex = other.NextFreeCollectiveIndex;
            BuildingSwaps = ArrayHelper.DeepClone(other.BuildingSwaps);
            InvisibilitySettings = ArrayHelper.DeepClone(other.InvisibilitySettings);
            UsingAMultiScriptFile = other.UsingAMultiScriptFile;
            PlayerHasMetDebbieHarry = other.PlayerHasMetDebbieHarry;
            MainScriptSize = other.MainScriptSize;
            LargestMissionScriptSize = other.LargestMissionScriptSize;
            NumberOfMissionScripts = other.NumberOfMissionScripts;
            Threads = ArrayHelper.DeepClone(other.Threads);
        }

        public int GetGlobal(int index)
        {
            byte[] intBits = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                intBits[i] = ScriptSpace[(index * 4) + i];
            }

            return BitConverter.ToInt32(intBits, 0);
        }

        public float GetGlobalAsFloat(int index)
        {
            byte[] floatBits = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                floatBits[i] = ScriptSpace[(index * 4) + i];
            }

            return BitConverter.ToSingle(floatBits, 0);
        }

        public void SetGlobal(int index, int value)
        {
            byte[] intBits = BitConverter.GetBytes(value);
            for (int i = 0; i < sizeof(int); i++)
            {
                ScriptSpace[(index * sizeof(int)) + i] = intBits[i];
            }
        }

        public void SetGlobal(int index, float value)
        {
            byte[] floatBits = BitConverter.GetBytes(value);
            for (int i = 0; i < sizeof(float); i++)
            {
                ScriptSpace[(index * sizeof(float)) + i] = floatBits[i];
            }
        }

        public int Read1ByteFromScript(int offset, out byte value)
        {
            value = ScriptSpace[offset];
            return sizeof(byte);
        }

        public int Read2BytesFromScript(int offset, out ushort value)
        {
            byte[] data = new byte[sizeof(ushort)];
            for (int i = 0; i < sizeof(ushort); i++)
            {
                data[i] = ScriptSpace[offset + i];
            }
            value = BitConverter.ToUInt16(data, 0);
            return sizeof(ushort);
        }

        public int Read4BytesFromScript(int offset, out uint value)
        {
            byte[] data = new byte[sizeof(uint)];
            for (int i = 0; i < sizeof(uint); i++)
            {
                data[i] = ScriptSpace[offset + i];
            }
            value = BitConverter.ToUInt32(data, 0);
            return sizeof(uint);
        }

        public int ReadFloatFromScript(int offset, out float value)
        {
            byte[] data = new byte[sizeof(float)];
            for (int i = 0; i < sizeof(float); i++)
            {
                data[i] = ScriptSpace[offset + i];
            }
            value = BitConverter.ToSingle(data, 0);
            return sizeof(float);
        }

        public int Write1ByteToScript(int offset, byte value)
        {
            ScriptSpace[offset] = value;
            return sizeof(byte);
        }

        public int Write2BytesToScript(int offset, ushort value)
        {
            byte[] data = BitConverter.GetBytes(value);
            for (int i = 0; i < sizeof(ushort); i++)
            {
                ScriptSpace[offset + i] = data[i];
            }
            return sizeof(ushort);
        }

        public int Write4BytesToScript(int offset, uint value)
        {
            byte[] data = BitConverter.GetBytes(value);
            for (int i = 0; i < sizeof(uint); i++)
            {
                ScriptSpace[offset + i] = data[i];
            }
            return sizeof(uint);
        }

        public int WriteFloatToScript(int offset, float value)
        {
            byte[] data = BitConverter.GetBytes(value);
            for (int i = 0; i < sizeof(float); i++)
            {
                ScriptSpace[offset + i] = data[i];
            }
            return sizeof(float);
        }

        public RunningScript GetRunningScript(string name)
        {
            return Threads.Where(x => x.Name == name).FirstOrDefault();
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int size = GTA3VCSave.ReadBlockHeader(buf, "SCR");

            int varSpace = buf.ReadInt32();
            ScriptSpace = buf.Read<byte>(varSpace);
            buf.Align4();
            int scriptDataSize = buf.ReadInt32();
            Debug.Assert(scriptDataSize == ScriptDataSize);
            OnAMissionFlag = buf.ReadInt32();
            LastMissionPassedTime = buf.ReadUInt32();
            Collectives = buf.Read<Collective>(NumCollectives);
            NextFreeCollectiveIndex = buf.ReadInt32();
            BuildingSwaps = buf.Read<BuildingSwap>(NumBuildingSwaps);
            InvisibilitySettings = buf.Read<InvisibleObject>(NumInvisibilitySettings);
            UsingAMultiScriptFile = buf.ReadBool();
            PlayerHasMetDebbieHarry = buf.ReadBool();
            buf.ReadUInt16();
            MainScriptSize = buf.ReadInt32();
            LargestMissionScriptSize = buf.ReadInt32();
            NumberOfMissionScripts = buf.ReadInt16();
            buf.ReadUInt16();
            int runningScripts = buf.ReadInt32();
            Threads = buf.Read<RunningScript>(runningScripts, fmt);

            Debug.Assert(buf.Offset == size + GTA3VCSave.BlockHeaderSize);
            Debug.Assert(size == SizeOfObject(this, fmt) - GTA3VCSave.BlockHeaderSize);
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            int size = SizeOfObject(this, fmt);
            GTA3VCSave.WriteBlockHeader(buf, "SCR", size - GTA3VCSave.BlockHeaderSize);

            buf.Write(ScriptSpace.Count);
            buf.Write(ScriptSpace);
            buf.Align4();
            buf.Write(ScriptDataSize);      // wrong value in save, actually is +0x104
            buf.Write(OnAMissionFlag);
            buf.Write(LastMissionPassedTime);
            buf.Write(Collectives, NumCollectives);
            buf.Write(NextFreeCollectiveIndex);
            buf.Write(BuildingSwaps, NumBuildingSwaps);
            buf.Write(InvisibilitySettings, NumInvisibilitySettings);
            buf.Write(UsingAMultiScriptFile);
            buf.Write(PlayerHasMetDebbieHarry);
            buf.Write((short) 0);
            buf.Write(MainScriptSize);
            buf.Write(LargestMissionScriptSize);
            buf.Write(NumberOfMissionScripts);
            buf.Write((short) 0);
            buf.Write(Threads.Count);
            buf.Write(Threads, fmt);

            Debug.Assert(buf.Offset == size);
        }

        protected override int GetSize(FileFormat fmt)
        {
            return SizeOfType<RunningScript>(fmt) * Threads.Count
                + StreamBuffer.Align4(ScriptSpace.Count)
                + ScriptDataSize + 0x104
                + GTA3VCSave.BlockHeaderSize
                + 3 * sizeof(int);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ScriptData);
        }

        public bool Equals(ScriptData other)
        {
            if (other == null)
            {
                return false;
            }

            return ScriptSpace.SequenceEqual(other.ScriptSpace)
                && OnAMissionFlag.Equals(other.OnAMissionFlag)
                && LastMissionPassedTime.Equals(other.LastMissionPassedTime)
                && Collectives.SequenceEqual(other.Collectives)
                && NextFreeCollectiveIndex.Equals(other.NextFreeCollectiveIndex)
                && BuildingSwaps.SequenceEqual(other.BuildingSwaps)
                && InvisibilitySettings.SequenceEqual(other.InvisibilitySettings)
                && UsingAMultiScriptFile.Equals(other.UsingAMultiScriptFile)
                && PlayerHasMetDebbieHarry.Equals(other.PlayerHasMetDebbieHarry)
                && MainScriptSize.Equals(other.MainScriptSize)
                && LargestMissionScriptSize.Equals(other.LargestMissionScriptSize)
                && NumberOfMissionScripts.Equals(other.NumberOfMissionScripts)
                && Threads.SequenceEqual(other.Threads);
        }

        public ScriptData DeepClone()
        {
            return new ScriptData(this);
        }
    }

    public enum PoolType
    {
        None,
        Treadable,
        Building,
        Object,
        Dummy
    }
}
