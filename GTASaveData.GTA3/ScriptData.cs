using GTASaveData.JsonConverters;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3
{
    public class ScriptData : SaveDataObject, IScriptData,
        IEquatable<ScriptData>, IDeepClonable<ScriptData>
    {
        public const int NumContacts = 16;
        public const int NumCollectives = 32;
        public const int NumBuildingSwaps = 25;
        public const int NumInvisibilitySettings = 20;

        private const int ScriptDataSize = 968;

        private Array<byte> m_scriptSpace;
        private int m_onAMissionFlag;
        private Array<Contact> m_contacts;
        private Array<Collective> m_collectives;    // not used
        private int m_nextFreeCollectiveIndex;      // not used
        private Array<BuildingSwap> m_buildingSwapArray;
        private Array<InvisibleObject> m_invisibilitySettingArray;
        private bool m_usingAMultiScriptFile;
        private int m_mainScriptSize;
        private int m_largestMissionScriptSize;
        private short m_numberOfMissionScripts;
        private Array<RunningScript> m_activeScripts;

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

        public Array<Contact> Contacts
        {
            get { return m_contacts; }
            set { m_contacts = value; OnPropertyChanged(); }
        }

        [Obsolete("Not used by the game.")]
        public Array<Collective> Collectives
        {
            get { return m_collectives; }
            set { m_collectives = value; OnPropertyChanged(); }
        }

        [Obsolete("Not used by the game.")]
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

        public Array<RunningScript> ActiveScripts
        {
            get { return m_activeScripts; }
            set { m_activeScripts = value; OnPropertyChanged(); }
        }

        public IEnumerable<int> GlobalVariables
        {
            get
            {
                for (int i = 0; i < ScriptSpace.Count / 4; i++) yield return GetGlobal(i);
            }
        }

        IEnumerable<IBuildingSwap> IScriptData.BuildingSwaps => m_buildingSwapArray;

        IEnumerable<IInvisibleObject> IScriptData.InvisibilitySettings => m_invisibilitySettingArray;

        IEnumerable<IRunningScript> IScriptData.ActiveScripts => m_activeScripts;

        public ScriptData()
        {
            ScriptSpace = new Array<byte>();
            Contacts = ArrayHelper.CreateArray<Contact>(NumContacts);
            Collectives = ArrayHelper.CreateArray<Collective>(NumCollectives);
            BuildingSwaps = ArrayHelper.CreateArray<BuildingSwap>(NumBuildingSwaps);
            InvisibilitySettings = ArrayHelper.CreateArray<InvisibleObject>(NumInvisibilitySettings);
            ActiveScripts = new Array<RunningScript>();
        }

        public ScriptData(ScriptData other)
        {
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
            ActiveScripts = ArrayHelper.DeepClone(other.ActiveScripts);
        }

        public RunningScript GetScript(string name)
        {
            return ActiveScripts.Where(x => x.Name == name).FirstOrDefault();
        }

        IRunningScript IScriptData.GetScript(string name) => GetScript(name);

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

        public int Read2BytesFromScript(int offset, out short value)
        {
            byte[] data = new byte[sizeof(short)];
            for (int i = 0; i < sizeof(short); i++)
            {
                data[i] = ScriptSpace[offset + i];
            }
            value = BitConverter.ToInt16(data, 0);
            return sizeof(short);
        }

        public int Read4BytesFromScript(int offset, out int value)
        {
            byte[] data = new byte[sizeof(int)];
            for (int i = 0; i < sizeof(int); i++)
            {
                data[i] = ScriptSpace[offset + i];
            }
            value = BitConverter.ToInt32(data, 0);
            return sizeof(int);
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

        public int Write2BytesToScript(int offset, short value)
        {
            byte[] data = BitConverter.GetBytes(value);
            for (int i = 0; i < sizeof(short); i++)
            {
                ScriptSpace[offset + i] = data[i];
            }
            return sizeof(short);
        }

        public int Write4BytesToScript(int offset, int value)
        {
            byte[] data = BitConverter.GetBytes(value);
            for (int i = 0; i < sizeof(int); i++)
            {
                ScriptSpace[offset + i] = data[i];
            }
            return sizeof(int);
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

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int size = GTA3VCSave.ReadBlockHeader(buf, "SCR");

            int varSpace = buf.ReadInt32();
            ScriptSpace = buf.Read<byte>(varSpace);
            buf.Align4();
            int scriptDataSize = buf.ReadInt32();
            Debug.Assert(scriptDataSize == ScriptDataSize);
            OnAMissionFlag = buf.ReadInt32();
            Contacts = buf.Read<Contact>(NumContacts);
            Collectives = buf.Read<Collective>(NumCollectives);
            NextFreeCollectiveIndex = buf.ReadInt32();
            BuildingSwaps = buf.Read<BuildingSwap>(NumBuildingSwaps);
            InvisibilitySettings = buf.Read<InvisibleObject>(NumInvisibilitySettings);
            UsingAMultiScriptFile = buf.ReadBool();
            buf.ReadByte();
            buf.ReadUInt16();
            MainScriptSize = buf.ReadInt32();
            LargestMissionScriptSize = buf.ReadInt32();
            NumberOfMissionScripts = buf.ReadInt16();
            buf.ReadUInt16();
            int runningScripts = buf.ReadInt32();
            ActiveScripts = buf.Read<RunningScript>(runningScripts, fmt);

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
            buf.Write(ScriptDataSize);
            buf.Write(OnAMissionFlag);
            buf.Write(Contacts, NumContacts);
            buf.Write(Collectives, NumCollectives);
            buf.Write(NextFreeCollectiveIndex);
            buf.Write(BuildingSwaps, NumBuildingSwaps);
            buf.Write(InvisibilitySettings, NumInvisibilitySettings);
            buf.Write(UsingAMultiScriptFile);
            buf.Write((byte) 0);
            buf.Write((short) 0);
            buf.Write(MainScriptSize);
            buf.Write(LargestMissionScriptSize);
            buf.Write(NumberOfMissionScripts);
            buf.Write((short) 0);
            buf.Write(ActiveScripts.Count);
            buf.Write(ActiveScripts, fmt);

            Debug.Assert(buf.Offset == size);
        }

        protected override int GetSize(FileFormat fmt)
        {
            return SizeOfType<RunningScript>(fmt) * ActiveScripts.Count
                + StreamBuffer.Align4(ScriptSpace.Count)
                + ScriptDataSize
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
                && Contacts.SequenceEqual(other.Contacts)
                && Collectives.SequenceEqual(other.Collectives)
                && NextFreeCollectiveIndex.Equals(other.NextFreeCollectiveIndex)
                && BuildingSwaps.SequenceEqual(other.BuildingSwaps)
                && InvisibilitySettings.SequenceEqual(other.InvisibilitySettings)
                && UsingAMultiScriptFile.Equals(other.UsingAMultiScriptFile)
                && MainScriptSize.Equals(other.MainScriptSize)
                && LargestMissionScriptSize.Equals(other.LargestMissionScriptSize)
                && NumberOfMissionScripts.Equals(other.NumberOfMissionScripts)
                && ActiveScripts.SequenceEqual(other.ActiveScripts);
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
#pragma warning restore CS0618 // Type or member is obsolete
