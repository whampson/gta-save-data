namespace GTASaveData.Common
{
    public interface ICarGenerator
    {
        int Model { get; set; }

        Vector3d Position { get; set; }

        float Heading { get; set; }

        int Color1 { get; set; }

        int Color2 { get; set; }

        bool Enabled { get; set; }
    }
}
