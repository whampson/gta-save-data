using System;

namespace GTASaveData.Types.Interfaces
{
    public interface IGTASave
    {
        string Name { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
