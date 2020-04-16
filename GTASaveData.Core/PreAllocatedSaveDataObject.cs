using System;
using System.Collections.Generic;
using System.Text;

namespace GTASaveData
{
    /// <summary>
    /// A <see cref="SaveDataObject"/> whose memory must be allocated before being deserialized.
    /// </summary>
    public abstract class PreAllocatedSaveDataObject : SaveDataObject
    {
        protected PreAllocatedSaveDataObject(int size)
        {
            PreAllocate(size);
        }

        protected abstract void PreAllocate(int size);
    }
}
