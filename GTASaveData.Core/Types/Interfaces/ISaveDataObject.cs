namespace GTASaveData.Types.Interfaces
{
    public interface ISaveDataObject
    {
        int ReadObjectData(DataBuffer buf);
        int ReadObjectData(DataBuffer buf, SaveFileFormat fmt);

        int WriteObjectData(DataBuffer buf);
        int WriteObjectData(DataBuffer buf, SaveFileFormat fmt);
    }
}
