using System.Collections.Generic;

namespace GTASaveData.Interfaces
{
    public interface IScriptData
    {
        IEnumerable<int> Globals { get; }
        IEnumerable<IBuildingSwap> BuildingSwaps { get; }
        IEnumerable<IInvisibleObject> InvisibilitySettings { get; }
        IEnumerable<IRunningScript> Threads { get; }

        int GetGlobal(int index);
        float GetGlobalAsFloat(int index);
        void SetGlobal(int index, int value);
        void SetGlobal(int index, float value);
        IRunningScript GetThread(string name);
    }
}
