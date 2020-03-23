using GTASaveData.Converters;
using GTASaveData.Types;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public class TheScripts : SaveDataObject, IEquatable<TheScripts>
    {
        public static class Limits
        {
            public const int NumberOfContacts = 16;
            public const int NumberOfCollectives = 32;
            public const int NumberOfBuildingSwaps = 25;
            public const int NumberOfInvisibilitySettings = 20;
        }

        private const int ScriptDataSize = 968;

        private Array<byte> m_scriptSpace;          // TODO: hmmm, keep this a byte array or make it uint32s?
        private int m_onAMissionFlag;
        private Array<ContactInfo> m_contactArray;
        private Array<Collective> m_collectiveArray;
        private int m_nextFreeCollectiveIndex;
        private Array<StaticReplacement> m_buildingSwapArray;
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

        public Array<ContactInfo> ContactArray
        {
            get { return m_contactArray; }
            set { m_contactArray = value; OnPropertyChanged(); }
        }

        public Array<Collective> CollectiveArray
        {
            get { return m_collectiveArray; }
            set { m_collectiveArray = value; OnPropertyChanged(); }
        }

        public int NextFreeCollectiveIndex
        {
            get { return m_nextFreeCollectiveIndex; }
            set { m_nextFreeCollectiveIndex = value; OnPropertyChanged(); }
        }

        public Array<StaticReplacement> BuildingSwapArray
        {
            get { return m_buildingSwapArray; }
            set { m_buildingSwapArray = value; OnPropertyChanged(); }
        }

        public Array<InvisibleObject> InvisibilitySettingArray
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

        public TheScripts()
        {
            ScriptSpace = new Array<byte>();
            ContactArray = CreateArray<ContactInfo>(Limits.NumberOfContacts);
            CollectiveArray = CreateArray<Collective>(Limits.NumberOfCollectives);
            BuildingSwapArray = CreateArray<StaticReplacement>(Limits.NumberOfBuildingSwaps);
            InvisibilitySettingArray = CreateArray<InvisibleObject>(Limits.NumberOfInvisibilitySettings);
            ActiveScripts = new Array<RunningScript>();
        }

        public int GetVariable(int index)
        {
            return ScriptSpace[index + 3] << 24
                 | ScriptSpace[index + 2] << 16
                 | ScriptSpace[index + 1] << 8
                 | ScriptSpace[index];
        }

        public float GetVariableAsFloat(int index)
        {
            byte[] floatBits = new byte[]
            {
                ScriptSpace[index + 3],
                ScriptSpace[index + 2],
                ScriptSpace[index + 1],
                ScriptSpace[index]
            };

            return BitConverter.ToSingle(floatBits, 0);
        }

        public void SetVariable(int index, int value)
        {
            ScriptSpace[index + 3] = (byte) (value >> 24);
            ScriptSpace[index + 2] = (byte) (value >> 16);
            ScriptSpace[index + 1] = (byte) (value >> 8);
            ScriptSpace[index] = (byte) value;
        }

        public void SetVariable(int index, float value)
        {
            // TODO: test this
            byte[] floatBits = BitConverter.GetBytes(value);
            Array.Copy(floatBits, 0, ScriptSpace, index, 4);
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "SCR");
            int varSpace = buf.ReadInt32();
            ScriptSpace = buf.ReadArray<byte>(varSpace);
            buf.Align4Bytes();
            int scriptDataSize = buf.ReadInt32();
            Debug.Assert(scriptDataSize == ScriptDataSize);
            OnAMissionFlag = buf.ReadInt32();
            ContactArray = buf.ReadArray<ContactInfo>(Limits.NumberOfContacts);
            CollectiveArray = buf.ReadArray<Collective>(Limits.NumberOfCollectives);
            NextFreeCollectiveIndex = buf.ReadInt32();
            BuildingSwapArray = buf.ReadArray<StaticReplacement>(Limits.NumberOfBuildingSwaps);
            InvisibilitySettingArray = buf.ReadArray<InvisibleObject>(Limits.NumberOfInvisibilitySettings);
            UsingAMultiScriptFile = buf.ReadBool();
            buf.ReadByte();
            buf.ReadUInt16();
            MainScriptSize = buf.ReadInt32();
            LargestMissionScriptSize = buf.ReadInt32();
            NumberOfMissionScripts = buf.ReadInt16();
            buf.ReadUInt16();
            int runningScripts = buf.ReadInt32();
            ActiveScripts = buf.ReadArray<RunningScript>(runningScripts, fmt);

            Debug.Assert(buf.Offset - GTA3Save.SaveHeaderSize == size);
            Debug.Assert(size + GTA3Save.SaveHeaderSize == GetSerializedSize(fmt));
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            int size = GetSerializedSize(fmt);

            GTA3Save.WriteSaveHeader(buf, "SCR\0", size - GTA3Save.SaveHeaderSize);
            buf.Write(ScriptSpace.Count);
            buf.Write(ScriptSpace.ToArray());
            buf.Align4Bytes();
            buf.Write(ScriptDataSize);
            buf.Write(OnAMissionFlag);
            buf.Write(ContactArray.ToArray(), Limits.NumberOfContacts);
            buf.Write(CollectiveArray.ToArray(), Limits.NumberOfCollectives);
            buf.Write(NextFreeCollectiveIndex);
            buf.Write(BuildingSwapArray.ToArray(), Limits.NumberOfBuildingSwaps);
            buf.Write(InvisibilitySettingArray.ToArray(), Limits.NumberOfInvisibilitySettings);
            buf.Write(UsingAMultiScriptFile);
            buf.Write((byte) 0);
            buf.Write((short) 0);
            buf.Write(MainScriptSize);
            buf.Write(LargestMissionScriptSize);
            buf.Write(NumberOfMissionScripts);
            buf.Write((short) 0);
            buf.Write(ActiveScripts.Count);
            buf.Write(ActiveScripts.ToArray(), format: fmt);

            Debug.Assert(buf.Offset == size);
        }

        public int GetSerializedSize(SaveFileFormat fmt)
        {
            return SizeOf<RunningScript>(fmt) * ActiveScripts.Count + WorkBuffer.Align4Bytes(ScriptSpace.Count) + ScriptDataSize + GTA3Save.SaveHeaderSize + 3 * sizeof(int);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TheScripts);
        }

        public bool Equals(TheScripts other)
        {
            if (other == null)
            {
                return false;
            }

            return ScriptSpace.SequenceEqual(other.ScriptSpace)
                && OnAMissionFlag.Equals(other.OnAMissionFlag)
                && ContactArray.SequenceEqual(other.ContactArray)
                && CollectiveArray.SequenceEqual(other.CollectiveArray)
                && NextFreeCollectiveIndex.Equals(other.NextFreeCollectiveIndex)
                && BuildingSwapArray.SequenceEqual(other.BuildingSwapArray)
                && InvisibilitySettingArray.SequenceEqual(other.InvisibilitySettingArray)
                && UsingAMultiScriptFile.Equals(other.UsingAMultiScriptFile)
                && MainScriptSize.Equals(other.MainScriptSize)
                && LargestMissionScriptSize.Equals(other.LargestMissionScriptSize)
                && NumberOfMissionScripts.Equals(other.NumberOfMissionScripts)
                && ActiveScripts.SequenceEqual(other.ActiveScripts);
        }
    }
}
