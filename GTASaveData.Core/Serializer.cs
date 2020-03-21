namespace GTASaveData
{
    public static class Serializer
    {
        public static T Read<T>(byte[] buf)
        {
            return Read<T>(buf, SaveFileFormat.Default);
        }

        public static T Read<T>(byte[] buf, SaveFileFormat fmt)
        {
            Read(buf, fmt, out T obj);
            return obj;
        }

        public static int Read<T>(byte[] buf, SaveFileFormat fmt, out T obj)
        {
            using (WorkBuffer wb = new WorkBuffer(buf))
            {
                return Read(wb, fmt, out obj);
            }
        }

        public static T Read<T>(WorkBuffer buf)
        {
            return Read<T>(buf, SaveFileFormat.Default);
        }

        public static T Read<T>(WorkBuffer buf, SaveFileFormat fmt)
        {
            T obj = buf.GenericRead<T>(fmt);
            if (obj == null)
            {
                throw SerializationNotSupported();
            }

            return obj;
        }

        public static int Read<T>(WorkBuffer buf, SaveFileFormat fmt, out T obj)
        {
            int oldMark = buf.Mark;
            buf.ResetMark();

            obj = buf.GenericRead<T>(fmt);
            int length = buf.Offset;

            buf.Mark = oldMark;

            if (obj == null)
            {
                throw SerializationNotSupported();
            }

            return length;
        }

        public static byte[] Write<T>(T obj)
        {
            return Write(obj, SaveFileFormat.Default);
        }

        public static byte[] Write<T>(T obj, SaveFileFormat fmt)
        {
            using (WorkBuffer wb = new WorkBuffer())
            {
                Write(wb, obj, fmt);
                return wb.ToArray();
            }
        }

        public static int Write<T>(byte[] buf, T obj)
        {
            return Write(buf, obj, SaveFileFormat.Default);
        }

        public static int Write<T>(byte[] buf, T obj, SaveFileFormat fmt)
        {
            return Write(buf, obj, fmt, out byte[] _);
        }

        public static int Write<T>(byte[] buf, T obj, SaveFileFormat fmt, out byte[] data)
        {
            using (WorkBuffer wb = new WorkBuffer(buf))
            {
                int count = Write(wb, obj, fmt);
                data = wb.ToArray();

                return count;
            }
        }

        public static int Write<T>(WorkBuffer buf, T obj)
        {
            return Write(buf, obj, SaveFileFormat.Default);
        }

        public static int Write<T>(WorkBuffer buf, T obj, SaveFileFormat fmt)
        {
            int oldMark = buf.Mark;
            buf.ResetMark();

            bool success = buf.GenericWrite(obj, fmt);
            int length = buf.Offset;

            buf.Mark = oldMark;

            if (!success)
            {
                throw SerializationNotSupported();
            }

            return length;
        }

        private static SerializationException SerializationNotSupported()
        {
            return new SerializationException(Strings.Error_InvalidOperation_NoSerialization);
        }
    }
}
