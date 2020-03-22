using System;

namespace GTASaveData.Types.Interfaces
{
    public interface IGTASaveFile : IDisposable
    {
        string Name { get; set; }
        DateTime TimeLastSaved { get; set; }
    }
}
