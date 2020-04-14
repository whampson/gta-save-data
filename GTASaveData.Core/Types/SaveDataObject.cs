using GTASaveData.Types.Interfaces;
using System;
using System.Diagnostics;

namespace GTASaveData.Types
{
    public abstract class SaveDataObject : GTAObject, ISaveDataObject
    {
        int ISaveDataObject.ReadObjectData(DataBuffer buf)
        {
            int mrk, len;

            mrk = buf.Mark;
            buf.MarkPosition();
            ReadObjectData(buf, SaveFileFormat.Default);
            buf.Mark = mrk;
            len = buf.Offset;

            return len;
        }

        int ISaveDataObject.ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            int mrk, len;

            mrk = buf.Mark;
            buf.MarkPosition();
            ReadObjectData(buf, fmt);
            buf.Mark = mrk;
            len = buf.Offset;

            return len;
        }

        protected abstract void ReadObjectData(DataBuffer buf, SaveFileFormat fmt);

        int ISaveDataObject.WriteObjectData(DataBuffer buf)
        {
            int mrk, len;

            mrk = buf.Mark;
            buf.MarkPosition();
            WriteObjectData(buf, SaveFileFormat.Default);
            buf.Mark = mrk;
            len = buf.Offset;

            return len;
        }

        int ISaveDataObject.WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            int mrk, len;

            mrk = buf.Mark;
            buf.MarkPosition();
            WriteObjectData(buf, fmt);
            buf.Mark = mrk;
            len = buf.Offset;

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

        public static int SizeOf<T>(T obj)  where T : SaveDataObject, new()
        {
            return SizeOf(obj, SaveFileFormat.Default);
        }

        public static int SizeOf<T>(SaveFileFormat fmt) where T : SaveDataObject, new()
        {
            return SizeOf(new T(), fmt);
        }

        public static int SizeOf<T>(T obj, SaveFileFormat fmt) where T : SaveDataObject, new()
        {
            return obj.GetSize(fmt);
        }
    }
}
