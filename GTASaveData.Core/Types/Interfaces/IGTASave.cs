using System;

namespace GTASaveData.Types.Interfaces
{
    public interface IGTASave : IDisposable
    {
        string Name { get; set; }
        DateTime TimeLastSaved { get; set; }
    }
}
