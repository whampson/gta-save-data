using GTASaveData.Types.Interfaces;

namespace GTASaveData
{
    public static class Serializer
    {
        /// <summary>
        /// Reads a value from a byte array.
        /// </summary>
        public static T Read<T>(byte[] buf)
        {
            Read(buf, SaveFileFormat.Default, out T obj);
            return obj;
        }

        /// <summary>
        /// Reads a value from a byte array using the specified format.
        /// </summary>
        public static T Read<T>(byte[] buf, SaveFileFormat fmt)
        {
            Read(buf, fmt, out T obj);
            return obj;
        }

        /// <summary>
        /// Reads a value from a byte array using the specified format and returns the number of bytes read.
        /// </summary>
        public static int Read<T>(byte[] buf, SaveFileFormat fmt, out T obj)
        {
            using (DataBuffer workBuf = new DataBuffer(buf))
            {
                return Read(workBuf, fmt, out obj);
            }
        }

        /// <summary>
        /// Reads a value from the current position in a buffer using the specified format and returns the number of bytes read.
        /// </summary>
        public static int Read<T>(DataBuffer buf, SaveFileFormat fmt, out T obj)
        {
            return buf.GenericRead(fmt, out obj);
        }

        /// <summary>
        /// Reads the object's data from a byte array using the specified format and returns the number of bytes read.
        /// </summary>
        public static int Read<T>(T obj, byte[] buf, SaveFileFormat fmt) where T : ISaveDataObject
        {
            using (DataBuffer workBuf = new DataBuffer(buf))
            {
                return Read(obj, workBuf, fmt);
            }
        }

        /// <summary>
        /// Reads the object's data from the current position in a buffer using the specified format and returns the number of bytes read.
        /// </summary>
        public static int Read<T>(T obj, DataBuffer buf, SaveFileFormat fmt) where T : ISaveDataObject
        {
            return obj.ReadObjectData(buf, fmt);
        }

        /// <summary>
        /// Converts a value into a byte array.
        /// </summary>
        public static byte[] Write<T>(T obj)
        {
            Write(obj, SaveFileFormat.Default, out byte[] data);
            return data;
        }

        /// <summary>
        /// Converts a value into a byte arrya using the specified format.
        /// </summary>
        public static byte[] Write<T>(T obj, SaveFileFormat fmt)
        {
            Write(obj, fmt, out byte[] data);
            return data;
        }

        /// <summary>
        /// Converts a value into a byte arrya using the specified format and returns the number of bytes written.
        /// </summary>
        public static int Write<T>(T obj, SaveFileFormat fmt, out byte[] data)
        {
            using (DataBuffer workBuf = new DataBuffer())
            {
                int bytesWritten = workBuf.GenericWrite(obj, fmt);
                data = workBuf.GetBytes();
                return bytesWritten;
            }
        }

        /// <summary>
        /// Writes an object to the current postion in the buffer using the specified format and returns the number of bytes written.
        /// </summary>
        public static int Write<T>(DataBuffer buf, T obj, SaveFileFormat fmt) where T : ISaveDataObject
        {
            return obj.WriteObjectData(buf, fmt);
        }
    }
}
