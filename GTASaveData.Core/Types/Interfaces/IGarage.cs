namespace GTASaveData.Types.Interfaces
{
    public interface IGarage
    {
        int Type { get; set; }
        int State { get; set; }
        Vector3D PositionMin { get; set; }
        Vector3D PositionMax { get; set; }
        Vector3D Door1Position { get; set; }
        Vector3D Door2Position { get; set; }
    }
}
