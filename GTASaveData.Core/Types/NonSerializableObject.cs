using System;

namespace GTASaveData.Types
{
    public abstract class NonSerializableObject : GTAObject
    {
        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            throw new InvalidOperationException("This object is not serializable");
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            throw new InvalidOperationException("This object is not serializable");
        }
    }
}
