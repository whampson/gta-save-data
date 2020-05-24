using System;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// A GTA data structure that can be stored in a save file.
    /// </summary>
    public abstract class SaveDataObject : ObservableObject, ISerializable
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

        int ISerializable.GetSize(DataFormat fmt)
        {
            return GetSize(fmt);
        }

        protected abstract void ReadObjectData(StreamBuffer buf, DataFormat fmt);

        protected abstract void WriteObjectData(StreamBuffer buf, DataFormat fmt);

        protected abstract int GetSize(DataFormat fmt);

        protected static int SizeOf<T>() where T : new()
        {
            return Serializer.SizeOf<T>();
        }

        protected static int SizeOf<T>(DataFormat fmt) where T : new()
        {
            return Serializer.SizeOf<T>(fmt);
        }

        protected static int SizeOf<T>(T obj)
        {
            return Serializer.SizeOf(obj);
        }

        protected static int SizeOf<T>(T obj, DataFormat fmt)
        {
            return Serializer.SizeOf(obj, fmt);
        }

        protected NotSupportedException SizeNotDefined(DataFormat fmt)
        {
            return new NotSupportedException(string.Format(Strings.Error_SizeNotDefined, fmt.FormatName));
        }
    }
}
