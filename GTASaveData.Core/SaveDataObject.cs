using System;
using WpfEssentials;

namespace GTASaveData
{
    public abstract class SaveDataObject : ObservableObject, ISaveDataObject
    {
        protected virtual void OnReading() { }
        protected virtual void OnRead() { }
        protected virtual void OnWriting() { }
        protected virtual void OnWrite() { }
        protected abstract void ReadData(StreamBuffer buf, FileFormat fmt);
        protected abstract void WriteData(StreamBuffer buf, FileFormat fmt);
        protected abstract int GetSize(FileFormat fmt);

        int ISaveDataObject.ReadData(StreamBuffer buf, FileFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.MarkedPosition;
            buf.Mark();
            start = buf.Position;

            OnReading();
            ReadData(buf, fmt);
            OnRead();

            len = buf.Position - start;
            buf.MarkedPosition = oldMark;

            return len;
        }

        int ISaveDataObject.WriteData(StreamBuffer buf, FileFormat fmt)
        {
            int oldMark, start, len;

            oldMark = buf.MarkedPosition;
            buf.Mark();
            start = buf.Position;

            OnWriting();
            WriteData(buf, fmt);
            OnWrite();

            len = buf.Position - start;
            buf.MarkedPosition = oldMark;

            return len;
        }

        int ISaveDataObject.GetSize(FileFormat fmt)
        {
            return GetSize(fmt);
        }

        public static int SizeOfType<T>() where T : new()
        {
            return Serializer.SizeOfType<T>();
        }

        public static int SizeOfType<T>(FileFormat fmt) where T : new()
        {
            return Serializer.SizeOfType<T>(fmt);
        }

        public static int SizeOfObject<T>(T obj)
        {
            return Serializer.SizeOfObject(obj);
        }

        public static int SizeOfObject<T>(T obj, FileFormat fmt)
        {
            return Serializer.SizeOfObject(obj, fmt);
        }

        protected InvalidOperationException SizeNotDefined(FileFormat fmt)
        {
            return new InvalidOperationException(string.Format(Strings.Error_InvalidOperation_SizeNotDefined, fmt.Name));
        }
    }
}
