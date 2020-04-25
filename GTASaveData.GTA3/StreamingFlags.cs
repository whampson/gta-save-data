using System;

namespace GTASaveData.GTA3
{
    [Flags]
    public enum StreamingFlags : byte
    {
        None,
        DontRemove = 1,
        ScriptOwned = 2,
        Dependency = 4,
        Priority = 8,
        NoFade = 16,
        CantRemove = DontRemove | ScriptOwned,
        KeepInMemory = DontRemove | ScriptOwned | Dependency
    }
}
