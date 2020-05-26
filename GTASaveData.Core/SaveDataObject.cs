using System;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// A GTA data structure that can be stored in a save file.
    /// </summary>
    public abstract class SaveDataObject : ObservableObject, ISerializable
    {
        int ISerializable.ReadData(StreamBuffer buf, FileFormat fmt)
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

        int ISerializable.WriteData(StreamBuffer buf, FileFormat fmt)
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

        int ISerializable.GetSize(FileFormat fmt)
        {
            return GetSize(fmt);
        }

        protected abstract void ReadData(StreamBuffer buf, FileFormat fmt);

        protected abstract void WriteData(StreamBuffer buf, FileFormat fmt);

        protected abstract int GetSize(FileFormat fmt);

        protected static int SizeOf<T>() where T : new()
        {
            // TODO: remove this so it's required to check against fmt
            return Serializer.SizeOf<T>();
        }

        protected static int SizeOf<T>(FileFormat fmt) where T : new()
        {
            return Serializer.SizeOf<T>(fmt);
        }

        protected static int SizeOf<T>(T obj)
        {
            // TODO: remove
            return Serializer.SizeOf(obj);
        }

        protected static int SizeOf<T>(T obj, FileFormat fmt)
        {
            return Serializer.SizeOf(obj, fmt);
        }

        protected NotSupportedException SizeNotDefined(FileFormat fmt)
        {
            return new NotSupportedException(string.Format(Strings.Error_SizeNotDefined, fmt.Name));
        }
    }
}
