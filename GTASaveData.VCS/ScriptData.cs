using GTASaveData.Interfaces;
using GTASaveData.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
namespace GTASaveData.VCS
{
    public class ScriptData : SaveDataObject, IScriptData,
        IEquatable<ScriptData>, IDeepClonable<ScriptData>
    {
        public const int NumCollectives = 32;
        public const int NumBuildingSwaps = 80;
        public const int NumInvisibilitySettings = 52;

        private const int ScriptDataSize = 0x6B8;

        // TODO: port from LCS

        // Only the global variables are editable :(

        private ObservableArray<int> m_globals;
        private int m_onAMissionFlag;
        private uint m_lastMissionPassedTime;
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

        [Obsolete("Not loaded by the game.")]
        public int OnAMissionFlag
        {
            get { return m_onAMissionFlag; }
            set { m_onAMissionFlag = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public uint LastMissionPassedTime
        {
            get { return m_lastMissionPassedTime; }
            set { m_lastMissionPassedTime = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public ObservableArray<BuildingSwap> BuildingSwaps
        {
            get { return m_buildingSwapArray; }
            set { m_buildingSwapArray = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public ObservableArray<InvisibleObject> InvisibilitySettings
        {
            get { return m_invisibilitySettingArray; }
            set { m_invisibilitySettingArray = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public bool UsingAMultiScriptFile
        {
            get { return m_usingAMultiScriptFile; }
            set { m_usingAMultiScriptFile = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public bool PlayerHasMetDebbieHarry
        {
            get { return m_playerHasMetDebbieHarry; }
            set { m_playerHasMetDebbieHarry = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public int MainScriptSize
        {
            get { return m_mainScriptSize; }
            set { m_mainScriptSize = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public int LargestMissionScriptSize
        {
            get { return m_largestMissionScriptSize; }
            set { m_largestMissionScriptSize = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public short NumberOfMissionScripts
        {
            get { return m_numberOfMissionScripts; }
            set { m_numberOfMissionScripts = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
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
            BuildingSwaps = ArrayHelper.CreateArray<BuildingSwap>(NumBuildingSwaps);
            InvisibilitySettings = ArrayHelper.CreateArray<InvisibleObject>(NumInvisibilitySettings);
            Threads = new ObservableArray<RunningScript>();
        }

        public ScriptData(ScriptData other)
        {
            Globals = ArrayHelper.DeepClone(other.Globals);
            OnAMissionFlag = other.OnAMissionFlag;
            LastMissionPassedTime = other.LastMissionPassedTime;
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
            LastMissionPassedTime = buf.ReadUInt32();
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
            buf.Write(ScriptDataSize);
            buf.Write(OnAMissionFlag);
            buf.Write(LastMissionPassedTime);
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
                + ScriptDataSize
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
#pragma warning restore CS0618 // Type or member is obsolete