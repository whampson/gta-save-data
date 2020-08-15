using GTASaveData.JsonConverters;
using GTASaveData.Types;
using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
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

        private Array<int> m_globals;
        private int m_onAMissionFlag;
        private int m_lastMissionPassedTime;
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

        [JsonConverter(typeof(IntArrayConverter))]
        public Array<int> GlobalVariables
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
        public int LastMissionPassedTime
        {
            get { return m_lastMissionPassedTime; }
            set { m_lastMissionPassedTime = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public Array<Collective> Collectives
        {
            get { return m_collectives; }
            set { m_collectives = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public int NextFreeCollectiveIndex
        {
            get { return m_nextFreeCollectiveIndex; }
            set { m_nextFreeCollectiveIndex = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public Array<BuildingSwap> BuildingSwaps
        {
            get { return m_buildingSwapArray; }
            set { m_buildingSwapArray = value; OnPropertyChanged(); }
        }

        [Obsolete("Not loaded by the game.")]
        public Array<InvisibleObject> InvisibilitySettings
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
        public Array<RunningScript> ActiveScripts
        {
            get { return m_activeScripts; }
            set { m_activeScripts = value; OnPropertyChanged(); }
        }

        IEnumerable<int> IScriptData.GlobalVariables => m_globals;
        IEnumerable<IBuildingSwap> IScriptData.BuildingSwaps => m_buildingSwapArray;
        IEnumerable<IInvisibleObject> IScriptData.InvisibilitySettings => m_invisibilitySettingArray;
        IEnumerable<IRunningScript> IScriptData.ActiveScripts => m_activeScripts;

        public ScriptData()
        {
            GlobalVariables = new Array<int>();
            Collectives = ArrayHelper.CreateArray<Collective>(NumCollectives);
            BuildingSwaps = ArrayHelper.CreateArray<BuildingSwap>(NumBuildingSwaps);
            InvisibilitySettings = ArrayHelper.CreateArray<InvisibleObject>(NumInvisibilitySettings);
            ActiveScripts = new Array<RunningScript>();
        }

        public ScriptData(ScriptData other)
        {
            GlobalVariables = ArrayHelper.DeepClone(other.GlobalVariables);
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
            ActiveScripts = ArrayHelper.DeepClone(other.ActiveScripts);
        }

        public RunningScript GetScript(string name)
        {
            return ActiveScripts.Where(x => x.Name == name).FirstOrDefault();
        }

        IRunningScript IScriptData.GetScript(string name) => GetScript(name);

        public int GetGlobal(int index)
        {
            return GlobalVariables[index];
        }

        public float GetGlobalAsFloat(int index)
        {
            byte[] floatBits = BitConverter.GetBytes(GetGlobal(index));
            return BitConverter.ToSingle(floatBits, 0);
        }

        public void SetGlobal(int index, int value)
        {
            GlobalVariables[index] = value;
        }

        public void SetGlobal(int index, float value)
        {
            byte[] floatBits = BitConverter.GetBytes(value);
            GlobalVariables[index] = BitConverter.ToInt32(floatBits, 0);
        }

        protected override void ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int size = GTA3VCSave.ReadBlockHeader(buf, "SCR");

            int varSpace = buf.ReadInt32();
            GlobalVariables = buf.Read<int>(varSpace / sizeof(int));
            buf.Align4();
            int scriptDataSize = buf.ReadInt32();
            Debug.Assert(scriptDataSize == ScriptDataSize);
            OnAMissionFlag = buf.ReadInt32();
            LastMissionPassedTime = buf.ReadInt32();
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
            ActiveScripts = buf.Read<RunningScript>(runningScripts, fmt);

            Debug.Assert(buf.Offset == size + GTA3VCSave.BlockHeaderSize);
            Debug.Assert(size == SizeOfObject(this, fmt) - GTA3VCSave.BlockHeaderSize);
        }

        protected override void WriteData(StreamBuffer buf, FileFormat fmt)
        {
            int size = SizeOfObject(this, fmt);
            GTA3VCSave.WriteBlockHeader(buf, "SCR", size - GTA3VCSave.BlockHeaderSize);

            buf.Write(GlobalVariables.Count * sizeof(int));
            buf.Write(GlobalVariables);
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
            buf.Write(ActiveScripts.Count);
            buf.Write(ActiveScripts, fmt);

            Debug.Assert(buf.Offset == size);
        }

        protected override int GetSize(FileFormat fmt)
        {
            return SizeOfType<RunningScript>(fmt) * ActiveScripts.Count
                + GlobalVariables.Count * sizeof(int)
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

            return GlobalVariables.SequenceEqual(other.GlobalVariables)
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
                && ActiveScripts.SequenceEqual(other.ActiveScripts);
        }

        public ScriptData DeepClone()
        {
            return new ScriptData(this);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete