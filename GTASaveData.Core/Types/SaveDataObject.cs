using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;

namespace GTASaveData.Types
{
    public abstract class PreAllocatedSaveDataObject : SaveDataObject
    {
        protected PreAllocatedSaveDataObject(int size)
        {
            PreAllocate(size);
        }

        protected abstract void PreAllocate(int size);
    }

    public abstract class SaveDataObject : GTAObject, ISaveDataObject
    {
        int ISaveDataObject.ReadObjectData(DataBuffer buf)
        {
            int oldMark, start, len;

            oldMark = buf.Mark;
            buf.MarkPosition();
            start = buf.Position;

            ReadObjectData(buf, SaveFileFormat.Default);

            len = buf.Position - start;
            buf.Mark = oldMark;

            return len;
        }

        int ISaveDataObject.ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.Mark;
            buf.MarkPosition();
            start = buf.Position;

            ReadObjectData(buf, fmt);

            len = buf.Position - start;
            buf.Mark = oldMark;

            return len;
        }

        protected abstract void ReadObjectData(DataBuffer buf, SaveFileFormat fmt);

        int ISaveDataObject.WriteObjectData(DataBuffer buf)
        {
            int oldMark, start, len;

            oldMark = buf.Mark;
            buf.MarkPosition();
            start = buf.Position;

            WriteObjectData(buf, SaveFileFormat.Default);

            len = buf.Position - start;
            buf.Mark = oldMark;

            return len;
        }

        int ISaveDataObject.WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.Mark;
            buf.MarkPosition();
            start = buf.Position;

            WriteObjectData(buf, fmt);

            len = buf.Position - start;
            buf.Mark = oldMark;

            return len;
        }

        protected abstract void WriteObjectData(DataBuffer buf, SaveFileFormat fmt);

        protected virtual int GetSize(SaveFileFormat fmt)
        {
            Debug.WriteLine("Warning: {0}#GetSize() has not been overridden. Calling WriteObjectData() to compute size.", GetType().Name);
            return Serializer.Write(this, fmt, out byte[] _);
        }

        public static int SizeOf<T>() where T : SaveDataObject, new()
        {
            SizeAttribute sizeAttr = (SizeAttribute) Attribute.GetCustomAttribute(typeof(T), typeof(SizeAttribute));
            if (sizeAttr == null)
            {
                return SizeOf(new T());
            }

            return sizeAttr.Size;
        }

        public static int SizeOf<T>(T obj)  where T : SaveDataObject
        {
            return SizeOf(obj, SaveFileFormat.Default);
        }

        public static int SizeOf<T>(SaveFileFormat fmt) where T : SaveDataObject, new()
        {
            return SizeOf(new T(), fmt);
        }

        public static int SizeOf<T>(T obj, SaveFileFormat fmt) where T : SaveDataObject
        {
            return obj.GetSize(fmt);
        }
    }
}
