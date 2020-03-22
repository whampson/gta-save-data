using GTASaveData.Types.Interfaces;

namespace GTASaveData.Types
{
    public abstract class SaveDataObject : GTAObject, ISaveDataObject
    {
        int ISaveDataObject.ReadObjectData(WorkBuffer buf)
        {
#if DEBUG
            int oldMark = buf.Mark;
            buf.MarkPosition();
            ReadObjectData(buf, SaveFileFormat.Default);
            buf.Mark = oldMark;
#else
            ReadObjectData(buf, SaveFileFormat.Default);
#endif

            return buf.Offset;
        }

        int ISaveDataObject.ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
#if DEBUG
            int oldMark = buf.Mark;
            buf.MarkPosition();
            ReadObjectData(buf, fmt);
            buf.Mark = oldMark;
#else
            ReadObjectData(buf, fmt);
#endif

            return buf.Offset;
        }

        protected abstract void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt);

        int ISaveDataObject.WriteObjectData(WorkBuffer buf)
        {
#if DEBUG
            int oldMark = buf.Mark;
            buf.MarkPosition();
            WriteObjectData(buf, SaveFileFormat.Default);
            buf.Mark = oldMark;
#else
            WriteObjectData(buf, SaveFileFormat.Default);
#endif

            return buf.Offset;
        }

        int ISaveDataObject.WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
#if DEBUG
            int oldMark = buf.Mark;
            buf.MarkPosition();
            WriteObjectData(buf, fmt);
            buf.Mark = oldMark;
#else
            WriteObjectData(buf, fmt);
#endif

            return buf.Offset;
        }

        protected abstract void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt);
    }
}
