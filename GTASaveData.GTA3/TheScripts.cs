using GTASaveData.Serialization;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3.Blocks
{
    public class TheScripts : SerializableObject,
        IEquatable<TheScripts>
    {
        public static class Limits
        {
            public const int ContactsCount = 16;
            public const int CollectivesCount = 32;
            public const int BuildingSwapsCount = 25;
            public const int InvisibilitySettingsCount = 20;
        }

        private Array<uint> m_globalVariables;
        private int m_onAMissionFlag;
        private Array<ContactInfo> m_contacts;
        private Array<Collective> m_collectives;
        private int m_nextFreeCollectiveIndex;
        private Array<StaticReplacement> m_buildingSwaps;
        private Array<InvisibleObject> m_invisibilitySettings;
        private bool m_usingAMultiScriptFile;
        private int m_mainScriptSize;
        private int m_largestMissionScriptSize;
        private short m_numberOfMissionScripts;
        private Array<Thread> m_runningScripts;

        public Array<uint> GlobalVariables
        {
            get { return m_globalVariables; }
            set { m_globalVariables = value; OnPropertyChanged(); }
        }

        public int OnAMissionFlag
        {
            get { return m_onAMissionFlag; }
            set { m_onAMissionFlag = value; OnPropertyChanged(); }
        }

        public Array<ContactInfo> Contacts
        {
            get { return m_contacts; }
            set { m_contacts = value; OnPropertyChanged(); }
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

        public Array<StaticReplacement> BuildingSwaps
        {
            get { return m_buildingSwaps; }
            set { m_buildingSwaps = value; OnPropertyChanged(); }
        }

        public Array<InvisibleObject> InvisibilitySettings
        {
            get { return m_invisibilitySettings; }
            set { m_invisibilitySettings = value; OnPropertyChanged(); }
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

        public Array<Thread> RunningScripts
        {
            get { return m_runningScripts; }
            set { m_runningScripts = value; OnPropertyChanged(); }
        }

        public TheScripts()
        {
            m_globalVariables = new Array<uint>();
            m_contacts = new Array<ContactInfo>();
            m_collectives = new Array<Collective>();
            m_buildingSwaps = new Array<StaticReplacement>();
            m_invisibilitySettings = new Array<InvisibleObject>();
            m_runningScripts = new Array<Thread>();
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            int sizeOfScriptSpace = r.ReadInt32();
            m_globalVariables = r.ReadArray<uint>(sizeOfScriptSpace / 4);
            int constant3C8h = r.ReadInt32();
            Debug.Assert(constant3C8h == 0x3C8);
            m_onAMissionFlag = r.ReadInt32();
            m_contacts = r.ReadArray<ContactInfo>(Limits.ContactsCount);
            m_collectives = r.ReadArray<Collective>(Limits.CollectivesCount);
            m_nextFreeCollectiveIndex = r.ReadInt32();
            m_buildingSwaps = r.ReadArray<StaticReplacement>(Limits.BuildingSwapsCount);
            m_invisibilitySettings = r.ReadArray<InvisibleObject>(Limits.InvisibilitySettingsCount);
            m_usingAMultiScriptFile = r.ReadBool();
            r.Align();
            m_mainScriptSize = r.ReadInt32();
            m_largestMissionScriptSize = r.ReadInt32();
            m_numberOfMissionScripts = r.ReadInt16();
            r.Align();
            int numRunningScripts = r.ReadInt32();
            m_runningScripts = r.ReadArray<Thread>(numRunningScripts, fmt);
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_globalVariables.Count * 4);
            w.Write(m_globalVariables.ToArray());
            w.Write(0x3C8);
            w.Write(m_onAMissionFlag);
            w.Write(m_contacts.ToArray(), Limits.ContactsCount);
            w.Write(m_collectives.ToArray(), Limits.CollectivesCount);
            w.Write(m_nextFreeCollectiveIndex);
            w.Write(m_buildingSwaps.ToArray(), Limits.BuildingSwapsCount);
            w.Write(m_invisibilitySettings.ToArray(), Limits.InvisibilitySettingsCount);
            w.Write(m_usingAMultiScriptFile);
            w.Align();
            w.Write(m_mainScriptSize);
            w.Write(m_largestMissionScriptSize);
            w.Write(m_numberOfMissionScripts);
            w.Align();
            w.Write(m_runningScripts.Count);
            w.Write(m_runningScripts.ToArray(), format: fmt);
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

            return m_globalVariables.SequenceEqual(other.m_globalVariables)
                && m_onAMissionFlag.Equals(other.m_onAMissionFlag)
                && m_contacts.SequenceEqual(other.m_contacts)
                && m_collectives.SequenceEqual(other.m_collectives)
                && m_nextFreeCollectiveIndex.Equals(other.m_nextFreeCollectiveIndex)
                && m_buildingSwaps.SequenceEqual(other.m_buildingSwaps)
                && m_invisibilitySettings.SequenceEqual(other.m_invisibilitySettings)
                && m_usingAMultiScriptFile.Equals(other.m_usingAMultiScriptFile)
                && m_mainScriptSize.Equals(other.m_mainScriptSize)
                && m_largestMissionScriptSize.Equals(other.m_largestMissionScriptSize)
                && m_numberOfMissionScripts.Equals(other.m_numberOfMissionScripts)
                && m_runningScripts.SequenceEqual(other.m_runningScripts);
        }
    }
}
