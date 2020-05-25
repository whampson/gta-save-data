namespace GTASaveData.Types.Interfaces
{
    public interface ICarGenerator
    {
        int Model { get; set; }
        Vector3D Position { get; set; }
        float Heading { get; set; }
        int Color1 { get; set; }
        int Color2 { get; set; }
        int AlarmChance { get; set; }
        int LockedChance { get; set; }
        uint Timer { get; set; }
        bool Enabled { get; set; }
    }
}
