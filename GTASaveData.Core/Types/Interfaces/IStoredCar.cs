namespace GTASaveData.Types.Interfaces
{
    public interface IStoredCar
    {
        int Model { get; set; }
        Vector3D Position { get; set; }
        int Flags { get; set; }
        int Color1 { get; set; }
        int Color2 { get; set; }
        int Radio { get; set; }
        int Extra1 { get; set; }
        int Extra2 { get; set; }
    }
}
