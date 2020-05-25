using System;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// A GTA data structure that can be stored in a save file.
    /// </summary>
    public abstract class SaveDataObject : ObservableObject, ISerializable
    {
        int ISerializable.ReadData(StreamBuffer buf, SaveDataFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.Mark;
            buf.MarkCurrentPosition();
            start = buf.Cursor;

            ReadData(buf, fmt);

            len = buf.Cursor - start;
            buf.Mark = oldMark;

            return len;
        }

        int ISerializable.WriteData(StreamBuffer buf, SaveDataFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.Mark;
            buf.MarkCurrentPosition();
            start = buf.Cursor;

            WriteData(buf, fmt);

            len = buf.Cursor - start;
            buf.Mark = oldMark;

            return len;
        }

        int ISerializable.GetSize(SaveDataFormat fmt)
        {
            return GetSize(fmt);
        }

        protected abstract void ReadData(StreamBuffer buf, SaveDataFormat fmt);

        protected abstract void WriteData(StreamBuffer buf, SaveDataFormat fmt);

        protected abstract int GetSize(SaveDataFormat fmt);

        protected static int SizeOf<T>() where T : new()
        {
            // TODO: remove this so it's required to check against fmt
            return Serializer.SizeOf<T>();
        }

        protected static int SizeOf<T>(SaveDataFormat fmt) where T : new()
        {
            return Serializer.SizeOf<T>(fmt);
        }

        protected static int SizeOf<T>(T obj)
        {
            // TODO: remove
            return Serializer.SizeOf(obj);
        }

        protected static int SizeOf<T>(T obj, SaveDataFormat fmt)
        {
            return Serializer.SizeOf(obj, fmt);
        }

        protected NotSupportedException SizeNotDefined(SaveDataFormat fmt)
        {
            return new NotSupportedException(string.Format(Strings.Error_SizeNotDefined, fmt.FormatName));
        }
    }
}
