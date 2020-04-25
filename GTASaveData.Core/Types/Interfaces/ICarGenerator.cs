namespace GTASaveData.Types.Interfaces
{
    /// <summary>
    /// A genericized car generator.
    /// </summary>
    public interface ICarGenerator
    {
        int Model { get; set; }
        Vector3D Position { get; set; }
        float Angle { get; set; }
        int Color1 { get; set; }
        int Color2 { get; set; }
        bool Enabled { get; set; }
    }
}
