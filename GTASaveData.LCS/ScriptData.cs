using GTASaveData.Interfaces;
using GTASaveData.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.LCS
{
    public class ScriptData : SaveDataObject, IScriptData,
        IEquatable<ScriptData>, IDeepClonable<ScriptData>
    {
        public const int NumCollectives = 32;
        public const int NumBuildingSwaps = 80;
        public const int NumInvisibilitySettings = 52;

        private const int ScriptDataSize = 0x6B8;

        // Only the global variables are editable :(

        private ObservableArray<int> m_globals;
        private int m_onAMissionFlag;
        private int m_lastMissionPassedTime;
        private ObservableArray<Collective> m_collectives;    // not used
        private int m_nextFreeCollectiveIndex;      // not used
        private ObservableArray<BuildingSwap> m_buildingSwapArray;
        private ObservableArray<InvisibleObject> m_invisibilitySettingArray;
        private bool m_usingAMultiScriptFile;
        private bool m_playerHasMetDebbieHarry;
        private int m_mainScriptSize;
        private int m_largestMissionScriptSize;
        private short m_numberOfMissionScripts;
        private ObservableArray<RunningScript> m_activeScripts;

        [JsonConverter(typeof(IntArrayConverter))]
        public ObservableArray<int> Globals
        {
            get { return m_globals; }
            set { m_globals = value; OnPropertyChanged(); }
        }

        public int OnAMissionFlag
        {
            get { return m_onAMissionFlag; }
            set { m_onAMissionFlag = value; OnPropertyChanged(); }
        }

        public int LastMissionPassedTime
        {
            get { return m_lastMissionPassedTime; }
            set { m_lastMissionPassedTime = value; OnPropertyChanged(); }
        }

        public ObservableArray<Collective> Collectives
        {
            get { return m_collectives; }
            set { m_collectives = value; OnPropertyChanged(); }
        }

        public int NextFreeCollectiveIndex
        {
            get { return m_nextFreeCollectiveIndex; }
            set { m_nextFreeCollectiveIndex = value; OnPropertyChanged(); }
        }

        public ObservableArray<BuildingSwap> BuildingSwaps
        {
            get { return m_buildingSwapArray; }
            set { m_buildingSwapArray = value; OnPropertyChanged(); }
        }

        public ObservableArray<InvisibleObject> InvisibilitySettings
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

        public ObservableArray<RunningScript> Threads
        {
            get { return m_activeScripts; }
            set { m_activeScripts = value; OnPropertyChanged(); }
        }

        IEnumerable<int> IScriptData.Globals => m_globals;
        IEnumerable<IBuildingSwap> IScriptData.BuildingSwaps => m_buildingSwapArray;
        IEnumerable<IInvisibleObject> IScriptData.InvisibilitySettings => m_invisibilitySettingArray;
        IEnumerable<IRunningScript> IScriptData.Threads => m_activeScripts;

        public ScriptData()
        {
            Globals = new ObservableArray<int>();
            Collectives = ArrayHelper.CreateArray<Collective>(NumCollectives);
            BuildingSwaps = ArrayHelper.CreateArray<BuildingSwap>(NumBuildingSwaps);
            InvisibilitySettings = ArrayHelper.CreateArray<InvisibleObject>(NumInvisibilitySettings);
            Threads = new ObservableArray<RunningScript>();
        }

        public ScriptData(ScriptData other)
        {
            Globals = ArrayHelper.DeepClone(other.Globals);
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

        public RunningScript GetThread(string name)
        {
            return Threads.Where(x => x.Name == name).FirstOrDefault();
        }

        IRunningScript IScriptData.GetThread(string name) => GetThread(name);

        public int GetGlobal(int index)
        {
            return Globals[index];
        }

        public float GetGlobalAsFloat(int index)
        {
            byte[] floatBits = BitConverter.GetBytes(GetGlobal(index));
            return BitConverter.ToSingle(floatBits, 0);
        }

        public void SetGlobal(int index, int value)
        {
            Globals[index] = value;
        }

        public void SetGlobal(int index, float value)
        {
            byte[] floatBits = BitConverter.GetBytes(value);
            Globals[index] = BitConverter.ToInt32(floatBits, 0);
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            int size = SaveFileGTA3VC.ReadBlockHeader(buf, "SCR");

            int varSpace = buf.ReadInt32();
            Globals = buf.ReadArray<int>(varSpace / sizeof(int));
            buf.Align4();
            int scriptDataSize = buf.ReadInt32();
            Debug.Assert(scriptDataSize == ScriptDataSize);
            OnAMissionFlag = buf.ReadInt32();
            LastMissionPassedTime = buf.ReadInt32();
            Collectives = buf.ReadArray<Collective>(NumCollectives);
            NextFreeCollectiveIndex = buf.ReadInt32();
            BuildingSwaps = buf.ReadArray<BuildingSwap>(NumBuildingSwaps);
            InvisibilitySettings = buf.ReadArray<InvisibleObject>(NumInvisibilitySettings);
            UsingAMultiScriptFile = buf.ReadBool();
            PlayerHasMetDebbieHarry = buf.ReadBool();
            buf.ReadUInt16();
            MainScriptSize = buf.ReadInt32();
            LargestMissionScriptSize = buf.ReadInt32();
            NumberOfMissionScripts = buf.ReadInt16();
            buf.ReadUInt16();
            int runningScripts = buf.ReadInt32();
            Threads = buf.ReadArray<RunningScript>(runningScripts, fmt);

            Debug.Assert(buf.Offset == size + SaveFileGTA3VC.BlockHeaderSize);
            Debug.Assert(size == SizeOf(this, fmt) - SaveFileGTA3VC.BlockHeaderSize);
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            int size = SizeOf(this, fmt);
            SaveFileGTA3VC.WriteBlockHeader(buf, "SCR", size - SaveFileGTA3VC.BlockHeaderSize);

            buf.Write(Globals.Count * sizeof(int));
            buf.Write(Globals);
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
            return SizeOf<RunningScript>(fmt) * Threads.Count
                + Globals.Count * sizeof(int)
                + ScriptDataSize + 0x104
                + SaveFileGTA3VC.BlockHeaderSize
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

            return Globals.SequenceEqual(other.Globals)
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
}