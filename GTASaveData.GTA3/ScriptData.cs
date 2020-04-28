using GTASaveData.Converters;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.GTA3
{
    public class ScriptData : SaveDataObject, IEquatable<ScriptData>
    {
        public static class Limits
        {
            public const int MaxNumContacts = 16;
            public const int MaxNumCollectives = 32;
            public const int MaxNumBuildingSwaps = 25;
            public const int MaxNumInvisibilitySettings = 20;
        }

        private const int ScriptDataSize = 968;

        private Array<byte> m_scriptSpace;
        private int m_onAMissionFlag;
        private Array<Contact> m_contacts;
        private Array<Collective> m_collectives;    // not used
        private int m_nextFreeCollectiveIndex;      // not used
        private Array<BuildingSwap> m_buildingSwapArray;
        private Array<InvisibleEntity> m_invisibilitySettingArray;
        private bool m_usingAMultiScriptFile;
        private int m_mainScriptSize;
        private int m_largestMissionScriptSize;
        private short m_numberOfMissionScripts;
        private Array<RunningScript> m_activeScripts;

        [JsonIgnore]
        public int NumberOfGlobalVariables => ScriptSpace.Count / 4;

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

        public Array<InvisibleEntity> InvisibilitySettings
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

        public ScriptData()
        {
            ScriptSpace = new Array<byte>();
            Contacts = new Array<Contact>();
            Collectives = new Array<Collective>();
            BuildingSwaps = new Array<BuildingSwap>();
            InvisibilitySettings = new Array<InvisibleEntity>();
            ActiveScripts = new Array<RunningScript>();
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
            for (int i = 0; i < 4; i++)
            {
                ScriptSpace[(index * 4) + i] = intBits[i];
            }
        }

        public void SetGlobal(int index, float value)
        {
            byte[] floatBits = BitConverter.GetBytes(value);
            for (int i = 0; i < 4; i++)
            {
                ScriptSpace[(index * 4) + i] = floatBits[i];
            }
        }

        // TODO: script read/write byte, short, int functions

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int size = GTA3Save.ReadSaveHeader(buf, "SCR");

            int varSpace = buf.ReadInt32();
            ScriptSpace = buf.Read<byte>(varSpace);
            buf.Align4Bytes();
            int scriptDataSize = buf.ReadInt32();
            Debug.Assert(scriptDataSize == ScriptDataSize);
            OnAMissionFlag = buf.ReadInt32();
            Contacts = buf.Read<Contact>(Limits.MaxNumContacts);
            Collectives = buf.Read<Collective>(Limits.MaxNumCollectives);
            NextFreeCollectiveIndex = buf.ReadInt32();
            BuildingSwaps = buf.Read<BuildingSwap>(Limits.MaxNumBuildingSwaps);
            InvisibilitySettings = buf.Read<InvisibleEntity>(Limits.MaxNumInvisibilitySettings);
            UsingAMultiScriptFile = buf.ReadBool();
            buf.ReadByte();
            buf.ReadUInt16();
            MainScriptSize = buf.ReadInt32();
            LargestMissionScriptSize = buf.ReadInt32();
            NumberOfMissionScripts = buf.ReadInt16();
            buf.ReadUInt16();
            int runningScripts = buf.ReadInt32();
            ActiveScripts = buf.Read<RunningScript>(runningScripts, fmt);

            Debug.Assert(buf.Offset == size + GTA3Save.SaveHeaderSize);
            Debug.Assert(size == SizeOf(this, fmt) - GTA3Save.SaveHeaderSize);
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int size = SizeOf(this, fmt);
            GTA3Save.WriteSaveHeader(buf, "SCR", size - GTA3Save.SaveHeaderSize);

            buf.Write(ScriptSpace.Count);
            buf.Write(ScriptSpace.ToArray());
            buf.Align4Bytes();
            buf.Write(ScriptDataSize);
            buf.Write(OnAMissionFlag);
            buf.Write(Contacts.ToArray(), Limits.MaxNumContacts);
            buf.Write(Collectives.ToArray(), Limits.MaxNumCollectives);
            buf.Write(NextFreeCollectiveIndex);
            buf.Write(BuildingSwaps.ToArray(), Limits.MaxNumBuildingSwaps);
            buf.Write(InvisibilitySettings.ToArray(), Limits.MaxNumInvisibilitySettings);
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

        protected override int GetSize(DataFormat fmt)
        {
            return SizeOf<RunningScript>(fmt) * ActiveScripts.Count
                + StreamBuffer.Align4Bytes(ScriptSpace.Count)
                + ScriptDataSize
                + GTA3Save.SaveHeaderSize
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
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
