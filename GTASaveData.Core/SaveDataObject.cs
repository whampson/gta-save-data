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

        int ISerializable.GetSize(DataFormat fmt)
        {
            return GetSize(fmt);
        }

        protected abstract void ReadObjectData(StreamBuffer buf, DataFormat fmt);

        protected abstract void WriteObjectData(StreamBuffer buf, DataFormat fmt);

        protected abstract int GetSize(DataFormat fmt);

        // TODO: remove this, force implementation on children >:)
        //protected virtual int GetSize(DataFormat fmt)
        //{
        //    Debug.WriteLine("Warning: {0}#GetSize() has not been overridden! Calling {0}#WriteObjectData() to compute size.", (object) GetType().Name);
        //    using (StreamBuffer buf = new StreamBuffer())
        //    {
        //        return ((ISerializable) this).WriteObjectData(buf, fmt);
        //    }
        //}

        // Wrappers for convenience
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
    }
}
