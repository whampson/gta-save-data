namespace GTASaveData.Interfaces
{
    public interface IBuildingSwap
    {
        int Type { get; set; }
        int Handle { get; set; }
        int NewModel { get; set; }
        int OldModel { get; set; }
    }
}
