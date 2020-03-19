using GTASaveData.Types;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3.Blocks
{
    public class TheScripts : GTAObject,
        IEquatable<TheScripts>
    {
        public static class Limits
        {
            public const int NumberOfContacts = 16;
            public const int NumberOfCollectives = 32;
            public const int NumberOfBuildingSwaps = 25;
            public const int NumberOfInvisibilitySettings = 20;
        }

        private const int ScriptDataSize = 968;

        private Array<byte> m_scriptSpace;
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

        public Array<ContactInfo> Contacts
        {
            get { return m_contactArray; }
            set { m_contactArray = value; OnPropertyChanged(); }
        }

        public Array<Collective> Collectives
        {
            get { return m_collectiveArray; }
            set { m_collectiveArray = value; OnPropertyChanged(); }
        }

        public int NextFreeCollectiveIndex
        {
            get { return m_nextFreeCollectiveIndex; }
            set { m_nextFreeCollectiveIndex = value; OnPropertyChanged(); }
        }

        public Array<StaticReplacement> BuildingSwaps
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

        public Array<RunningScript> RunningScripts
        {
            get { return m_activeScripts; }
            set { m_activeScripts = value; OnPropertyChanged(); }
        }

        public TheScripts()
        {
            m_scriptSpace = new Array<byte>();
            m_contactArray = new Array<ContactInfo>();
            m_collectiveArray = new Array<Collective>();
            m_buildingSwapArray = new Array<StaticReplacement>();
            m_invisibilitySettingArray = new Array<InvisibleObject>();
            m_activeScripts = new Array<RunningScript>();
        }

        public int GetVariable(int index)
        {
            return m_scriptSpace[index + 3] << 24
                 | m_scriptSpace[index + 2] << 16
                 | m_scriptSpace[index + 1] << 8
                 | m_scriptSpace[index];
        }

        public float GetVariableAsFloat(int index)
        {
            byte[] floatBits = new byte[]
            {
                m_scriptSpace[index + 3],
                m_scriptSpace[index + 2],
                m_scriptSpace[index + 1],
                m_scriptSpace[index]
            };

            return BitConverter.ToSingle(floatBits, 0);
        }

        public void SetVariable(int index, int value)
        {
            m_scriptSpace[index + 3] = (byte) (value >> 24);
            m_scriptSpace[index + 2] = (byte) (value >> 16);
            m_scriptSpace[index + 1] = (byte) (value >> 8);
            m_scriptSpace[index] = (byte) value;
        }

        public void SetVariable(int index, float value)
        {
            // TODO: test this
            byte[] floatBits = BitConverter.GetBytes(value);
            Array.Copy(floatBits, 0, m_scriptSpace, index, 4);
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "SCR\0");
            int varSpace = buf.ReadInt32();
            m_scriptSpace = buf.ReadArray<byte>(varSpace);
            int scriptDataSize = buf.ReadInt32();
            Debug.Assert(scriptDataSize == ScriptDataSize);
            m_onAMissionFlag = buf.ReadInt32();
            m_contactArray = buf.ReadArray<ContactInfo>(Limits.NumberOfContacts);
            m_collectiveArray = buf.ReadArray<Collective>(Limits.NumberOfCollectives);
            m_nextFreeCollectiveIndex = buf.ReadInt32();
            m_buildingSwapArray = buf.ReadArray<StaticReplacement>(Limits.NumberOfBuildingSwaps);
            m_invisibilitySettingArray = buf.ReadArray<InvisibleObject>(Limits.NumberOfInvisibilitySettings);
            m_usingAMultiScriptFile = buf.ReadBool();
            buf.ReadByte();
            buf.ReadUInt16();
            m_mainScriptSize = buf.ReadInt32();
            m_largestMissionScriptSize = buf.ReadInt32();
            m_numberOfMissionScripts = buf.ReadInt16();
            buf.ReadUInt16();
            int runningScripts = buf.ReadInt32();
            m_activeScripts = buf.ReadArray<RunningScript>(runningScripts, fmt);

            Debug.WriteLine(buf.Offset == size);
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            int size = GetSerializedSize();

            GTA3Save.WriteSaveHeader(buf, "SCR\0", size - GTA3Save.SaveHeaderSize);
            buf.Write(m_scriptSpace.Count);
            buf.Write(m_scriptSpace.ToArray());
            buf.Write(ScriptDataSize);
            buf.Write(m_onAMissionFlag);
            buf.Write(m_contactArray.ToArray(), Limits.NumberOfContacts);
            buf.Write(m_collectiveArray.ToArray(), Limits.NumberOfCollectives);
            buf.Write(m_nextFreeCollectiveIndex);
            buf.Write(m_buildingSwapArray.ToArray(), Limits.NumberOfBuildingSwaps);
            buf.Write(m_invisibilitySettingArray.ToArray(), Limits.NumberOfInvisibilitySettings);
            buf.Write(m_usingAMultiScriptFile);
            buf.Write((byte) 0);
            buf.Write((short) 0);
            buf.Write(m_mainScriptSize);
            buf.Write(m_largestMissionScriptSize);
            buf.Write(m_numberOfMissionScripts);
            buf.Write((short) 0);
            buf.Write(m_activeScripts.Count);
            buf.Write(m_activeScripts.ToArray(), format: fmt);

            Debug.WriteLine(buf.Offset == size);
        }

        private int GetSerializedSize()
        {
            return SizeOf<RunningScript>() * m_activeScripts.Count + m_scriptSpace.Count + ScriptDataSize + GTA3Save.SaveHeaderSize + 3 * sizeof(int);
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

            return m_scriptSpace.SequenceEqual(other.m_scriptSpace)
                && m_onAMissionFlag.Equals(other.m_onAMissionFlag)
                && m_contactArray.SequenceEqual(other.m_contactArray)
                && m_collectiveArray.SequenceEqual(other.m_collectiveArray)
                && m_nextFreeCollectiveIndex.Equals(other.m_nextFreeCollectiveIndex)
                && m_buildingSwapArray.SequenceEqual(other.m_buildingSwapArray)
                && m_invisibilitySettingArray.SequenceEqual(other.m_invisibilitySettingArray)
                && m_usingAMultiScriptFile.Equals(other.m_usingAMultiScriptFile)
                && m_mainScriptSize.Equals(other.m_mainScriptSize)
                && m_largestMissionScriptSize.Equals(other.m_largestMissionScriptSize)
                && m_numberOfMissionScripts.Equals(other.m_numberOfMissionScripts)
                && m_activeScripts.SequenceEqual(other.m_activeScripts);
        }
    }
}
