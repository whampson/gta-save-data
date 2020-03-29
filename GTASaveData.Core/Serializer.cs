using GTASaveData.Types;
using GTASaveData.Types.Interfaces;

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
            using (DataBuffer wb = new DataBuffer(buf))
            {
                return Read(wb, fmt, out obj);
            }
        }

        public static T Read<T>(DataBuffer buf)
        {
            return Read<T>(buf, SaveFileFormat.Default);
        }

        public static T Read<T>(DataBuffer buf, SaveFileFormat fmt)
        {
            T obj = buf.GenericRead<T>(fmt);
            if (obj == null)
            {
                throw SerializationNotSupported();
            }

            return obj;
        }

        public static int Read<T>(DataBuffer buf, SaveFileFormat fmt, out T obj)
        {
            int oldMark, length;

            oldMark = buf.Mark;
            buf.MarkPosition();
            obj = buf.GenericRead<T>(fmt);
            length = buf.Offset;
            buf.Mark = oldMark;

            if (obj == null)
            {
                throw SerializationNotSupported();
            }

            return length;
        }

        public static int Read<T>(T obj, byte[] buf, SaveFileFormat fmt)
             where T : ISaveDataObject
        {
            using (DataBuffer wb = new DataBuffer(buf))
            {
                return obj.ReadObjectData(wb, fmt);
            }
        }

        public static byte[] Write<T>(T obj)
        {
            return Write(obj, SaveFileFormat.Default);
        }

        public static byte[] Write<T>(T obj, SaveFileFormat fmt)
        {
            using (DataBuffer wb = new DataBuffer())
            {
                Write(wb, obj, fmt);
                return wb.GetBytes();
            }
        }

        public static int Write<T>(T obj, SaveFileFormat fmt, out byte[] data)
        {
            using (DataBuffer wb = new DataBuffer())
            {
                int count = Write(wb, obj, fmt);
                data = wb.GetBytes();

                return count;
            }
        }

        public static int Write<T>(DataBuffer buf, T obj)
        {
            return Write(buf, obj, SaveFileFormat.Default);
        }

        public static int Write<T>(DataBuffer buf, T obj, SaveFileFormat fmt)
        {
            bool success;
            int oldMark, length;

            oldMark = buf.Mark;
            buf.MarkPosition();
            success = buf.GenericWrite(obj, fmt);
            length = buf.Offset;
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
