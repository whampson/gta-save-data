using System.Numerics;

namespace GTASaveData.Interfaces
{
    public interface IGarage
    {
        int Type { get; set; }
        int State { get; set; }
        Vector3 PositionMin { get; set; }
        Vector3 PositionMax { get; set; }
        Vector3 Door1Position { get; set; }
        Vector3 Door2Position { get; set; }
    }
}
