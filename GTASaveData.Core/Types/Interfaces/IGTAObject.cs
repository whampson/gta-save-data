namespace GTASaveData.Types.Interfaces
{
    public interface IGTAObject
    {
        int ReadObjectData(WorkBuffer buf);
        int ReadObjectData(WorkBuffer buf, SaveFileFormat fmt);

        int WriteObjectData(WorkBuffer buf);
        int WriteObjectData(WorkBuffer buf, SaveFileFormat fmt);
    }
}
