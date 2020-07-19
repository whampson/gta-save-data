using System.Collections.Generic;

namespace GTASaveData.Types.Interfaces
{
    public interface IScriptData
    {
        IEnumerable<int> GlobalVariables { get; }
        IEnumerable<IBuildingSwap> BuildingSwaps { get; }
        IEnumerable<IInvisibleObject> InvisibilitySettings { get; }
        IEnumerable<IRunningScript> ActiveScripts { get; }

        IRunningScript GetScript(string name);
        int GetGlobal(int index);
        float GetGlobalAsFloat(int index);
        void SetGlobal(int index, int value);
        void SetGlobal(int index, float value);
    }
}
