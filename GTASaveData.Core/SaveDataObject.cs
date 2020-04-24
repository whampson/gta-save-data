using System;
using System.Diagnostics;

namespace GTASaveData
{
    /// <summary>
    /// A <see cref="GTAObject"/> that can be stored in a save data file.
    /// </summary>
    public abstract class SaveDataObject : GTAObject, ISerializable
    {
        int ISerializable.ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.Mark;
            buf.MarkCurrentPosition();
            start = buf.Cursor;

            ReadObjectData(buf, fmt);

            len = buf.Cursor - start;
            buf.Mark = oldMark;

            return len;
        }

        int ISerializable.WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.Mark;
            buf.MarkCurrentPosition();
            start = buf.Cursor;

            WriteObjectData(buf, fmt);

            len = buf.Cursor - start;
            buf.Mark = oldMark;

            return len;
        }

        protected abstract void ReadObjectData(StreamBuffer buf, DataFormat fmt);
        protected abstract void WriteObjectData(StreamBuffer buf, DataFormat fmt);

        protected virtual int GetSize(DataFormat fmt)
        {
            Debug.WriteLine("Warning: {0}#GetSize() has not been overridden! Calling {0}#WriteObjectData() to compute size.", (object) GetType().Name);
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
            return SizeOf(obj, DataFormat.Default);
        }

        public static int SizeOf<T>(DataFormat fmt) where T : SaveDataObject, new()
        {
            return SizeOf(new T(), fmt);
        }

        public static int SizeOf<T>(T obj, DataFormat fmt) where T : SaveDataObject
        {
            return obj.GetSize(fmt);
        }
    }
}
