using GTASaveData.Common;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.GTA3
{
    public sealed class Scripts : GTAObject,
        IEquatable<Scripts>
    {
        public const int ContactCount = 16;
        public const int CollectiveCount = 32;
        public const int BuildingSwapCount = 25;
        public const int InvisibilitySettingCount = 20;

        private ObservableCollection<uint> m_globalVariables;
        private int m_onAMissionFlag;
        private FullyObservableCollection<ContactInfo> m_contacts;
        private FullyObservableCollection<Collective> m_collectives;
        private int m_nextFreeCollectiveIndex;
        private FullyObservableCollection<BuildingSwap> m_buildingSwaps;
        private FullyObservableCollection<InvisibilitySetting> m_invisibilitySettings;
        private bool m_usingAMultiScriptFile;
        private int m_mainScriptSize;
        private int m_largestMissionScriptSize;
        private short m_numberOfMissionScripts;
        private FullyObservableCollection<RunningScript> m_runningScripts;

        public ObservableCollection<uint> GlobalVariables
        {
            get { return m_globalVariables; }
            set { m_globalVariables = value; OnPropertyChanged(); }
        }

        public int OnAMissionFlag
        {
            get { return m_onAMissionFlag; }
            set { m_onAMissionFlag = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<ContactInfo> Contacts
        {
            get { return m_contacts; }
            set { m_contacts = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<Collective> Collectives
        {
            get { return m_collectives; }
            set { m_collectives = value; OnPropertyChanged(); }
        }

        public int NextFreeCollectiveIndex
        {
            get { return m_nextFreeCollectiveIndex; }
            set { m_nextFreeCollectiveIndex = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<BuildingSwap> BuildingSwaps
        {
            get { return m_buildingSwaps; }
            set { m_buildingSwaps = value; OnPropertyChanged(); }
        }

        public FullyObservableCollection<InvisibilitySetting> InvisibilitySettings
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

        public FullyObservableCollection<RunningScript> RunningScripts
        {
            get { return m_runningScripts; }
            set { m_runningScripts = value; OnPropertyChanged(); }
        }

        public Scripts()
        {
            m_globalVariables = new ObservableCollection<uint>();
            m_contacts = new FullyObservableCollection<ContactInfo>();
            m_collectives = new FullyObservableCollection<Collective>();
            m_buildingSwaps = new FullyObservableCollection<BuildingSwap>();
            m_invisibilitySettings = new FullyObservableCollection<InvisibilitySetting>();
            m_runningScripts = new FullyObservableCollection<RunningScript>();
        }

        private Scripts(Serializer serializer, SystemType system)
            : this()
        {
            int sizeOfScriptSpace = serializer.ReadInt32();
            m_globalVariables = new ObservableCollection<uint>(serializer.ReadArray<uint>(sizeOfScriptSpace / 4));
            int constant = serializer.ReadInt32();
            Debug.Assert(constant == 0x3C8);
            m_onAMissionFlag = serializer.ReadInt32();
            m_contacts = new FullyObservableCollection<ContactInfo>(serializer.ReadArray<ContactInfo>(ContactCount));
            m_collectives = new FullyObservableCollection<Collective>(serializer.ReadArray<Collective>(CollectiveCount));
            m_nextFreeCollectiveIndex = serializer.ReadInt32();
            m_buildingSwaps = new FullyObservableCollection<BuildingSwap>(serializer.ReadArray<BuildingSwap>(BuildingSwapCount));
            m_invisibilitySettings = new FullyObservableCollection<InvisibilitySetting>(serializer.ReadArray<InvisibilitySetting>(InvisibilitySettingCount));
            m_usingAMultiScriptFile = serializer.ReadBool();
            serializer.Align();
            m_mainScriptSize = serializer.ReadInt32();
            m_largestMissionScriptSize = serializer.ReadInt32();
            m_numberOfMissionScripts = serializer.ReadInt16();
            serializer.Align();
            int numRunningScripts = serializer.ReadInt32();
            m_runningScripts = new FullyObservableCollection<RunningScript>(serializer.ReadArray<RunningScript>(numRunningScripts, system));
        }

        private void Serialize(Serializer serializer, SystemType system)
        {
            serializer.Write(m_globalVariables.Count * 4);
            serializer.WriteArray(m_globalVariables);
            serializer.Write(0x3C8);
            serializer.Write(m_onAMissionFlag);
            serializer.WriteArray(m_contacts, ContactCount);
            serializer.WriteArray(m_collectives, CollectiveCount);
            serializer.Write(m_nextFreeCollectiveIndex);
            serializer.WriteArray(m_buildingSwaps, BuildingSwapCount);
            serializer.WriteArray(m_invisibilitySettings, InvisibilitySettingCount);
            serializer.Write(m_usingAMultiScriptFile);
            serializer.Align();
            serializer.Write(m_mainScriptSize);
            serializer.Write(m_largestMissionScriptSize);
            serializer.Write(m_numberOfMissionScripts);
            serializer.Align();
            serializer.Write(m_runningScripts.Count);
            serializer.WriteArray(m_runningScripts, systemType: system);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Scripts);
        }

        public bool Equals(Scripts other)
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

        public override string ToString()
        {
            return BuildToString(new (string, object)[]
            {
                (nameof(GlobalVariables), GlobalVariables),
                (nameof(OnAMissionFlag), OnAMissionFlag),
                (nameof(Contacts), Contacts),
                (nameof(Collectives), Collectives),
                (nameof(NextFreeCollectiveIndex), NextFreeCollectiveIndex),
                (nameof(BuildingSwaps), BuildingSwaps),
                (nameof(InvisibilitySettings), InvisibilitySettings),
                (nameof(UsingAMultiScriptFile), UsingAMultiScriptFile),
                (nameof(MainScriptSize), MainScriptSize),
                (nameof(LargestMissionScriptSize), LargestMissionScriptSize),
                (nameof(NumberOfMissionScripts), NumberOfMissionScripts),
                (nameof(RunningScripts), RunningScripts)
            });
        }
    }
}
