using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// Commonalities between GTA3 and VC saves.
    /// </summary>
    public abstract class SaveFileGTA3VC : SaveFile, IDisposable
    {
        public const int BlockHeaderSize = 8;

        /// <summary>
        /// A fixed-size buffer used to read and build GTA3/VC saves.
        /// </summary>
        /// <remarks>
        /// Buffer size varies by file format.
        /// </remarks>
        protected DataBuffer WorkBuff { get; private set; }

        /// <summary>
        /// The data checksum.
        /// </summary>
        /// <remarks>
        /// Checksum is simply the sum of all preceding bytes.
        /// </remarks>
        protected int CheckSum { get; set; }

        private bool m_disposed;

        /// <summary>
        /// Creates a new <see cref="SaveFileGTA3VC"/> instance.
        /// </summary>
        protected SaveFileGTA3VC()
        { }

        /// <summary>
        /// Reads a block header and gets the block size.
        /// </summary>
        /// <param name="buf">The buffer to read.</param>
        /// <param name="tag">The block tag.</param>
        /// <returns>The size of the block data in bytes.</returns>
        public static int ReadBlockHeader(DataBuffer buf, string tag)
        {
            string readTag = buf.ReadString(4);
            int size = buf.ReadInt32();

            Debug.Assert(readTag == tag, $"Invalid block tag (expected: {tag}, actual: {readTag})");
            return size;
        }

        /// <summary>
        /// Writes a block header.
        /// </summary>
        /// <param name="buf">The buffer to write.</param>
        /// <param name="tag">The block tag.</param>
        /// <param name="size">The block data size in bytes.</param>
        public static void WriteBlockHeader(DataBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4, zeroTerminate: true);
            buf.Write(size);
        }

        /// <summary>
        /// Loads an object from the buffer.
        /// </summary>
        protected int LoadObject<T>(T obj) where T : SaveDataObject
        {
            int size = WorkBuff.ReadInt32();
            int bytesRead = Serializer.Read(obj, WorkBuff, FileFormat);
            WorkBuff.Align4();

            Debug.WriteLine($"{typeof(T).Name}: {bytesRead} bytes read.");
            Debug.Assert(bytesRead <= DataBuffer.Align4(size));

            return bytesRead;
        }

        /// <summary>
        /// Loads an instance of the specified type from the buffer.
        /// </summary>
        protected T LoadType<T>() where T : SaveDataObject, new()
        {
            T obj = new T();
            LoadObject(obj);

            return obj;
        }

        /// <summary>
        /// Allocates the required space for the specified type,
        /// then loads in an instance of that type.
        /// </summary>
        protected T LoadTypePreAlloc<T>() where T : SaveDataObject
        {
            int size = WorkBuff.ReadInt32();
            if (!(Activator.CreateInstance(typeof(T), size) is T obj))
            {
                throw new SerializationException(Strings.Error_Serialization_NoPreAlloc, typeof(T));
            }
            Debug.WriteLine($"{typeof(T).Name}: {size} bytes pre-allocated.");

            int bytesRead = Serializer.Read(obj, WorkBuff, FileFormat);
            WorkBuff.Align4();

            Debug.WriteLine($"{typeof(T).Name}: {bytesRead} bytes read.");
            Debug.Assert(bytesRead <= DataBuffer.Align4(size));

            return obj;
        }

        /// <summary>
        /// Writes an object to the buffer.
        /// </summary>
        /// <param name="obj"></param>
        protected void SaveObject(SaveDataObject obj)
        {
            int size, preSize, postData;

            preSize = WorkBuff.Position;
            WorkBuff.Skip(4);

            size = Serializer.Write(WorkBuff, obj, FileFormat);
            postData = WorkBuff.Position;

            WorkBuff.Seek(preSize);
            WorkBuff.Write(size);
            WorkBuff.Seek(postData);
            WorkBuff.Align4();

            Debug.WriteLine($"{obj.GetType().Name}: {size} bytes written.");
        }

        /// <summary>
        /// Reads a block of data from the file into the work buffer.
        /// </summary>
        protected int ReadBlock(DataBuffer file)
        {
            file.Mark();
            WorkBuff.Reset();

            int size = file.ReadInt32();
            if ((uint) size > WorkBuff.Length) throw new SerializationException(Strings.Error_Serialization_BadBlockSize, (uint) size);

            WorkBuff.Write(file.ReadBytes(size));
            file.Align4();

            Debug.Assert(file.Offset == size + 4);

            WorkBuff.Reset();
            return size;
        }

        /// <summary>
        /// Writes a block of data from the work buffer into the file.
        /// </summary>
        protected int WriteBlock(DataBuffer file)
        {
            file.Mark();

            byte[] data = WorkBuff.GetBytes();
            int size = data.Length;

            file.Write(size);
            file.Write(data);
            file.Align4();

            Debug.Assert(file.Offset == size + 4);
            CheckSum += file.GetBytesFromMark().Sum(x => x);

            WorkBuff.Reset();

            // game code has a bug where the size of the 'size' itself is not
            // factored in to the total file size, so savefiles are always
            // 4 * numBlocks bytes larger than told by SizeOfGameInBytes
            return size;
        }

        protected override void OnReading(FileFormat fmt)
        {
            base.OnReading(fmt);
            FileFormat = fmt;
            ReInitWorkBuff();
        }

        protected override void OnWriting(FileFormat fmt)
        {
            base.OnWriting(fmt);
            FileFormat = fmt;
            ReInitWorkBuff();
        }

        private void ReInitWorkBuff()
        {
            if (WorkBuff != null)
            {
                WorkBuff.Dispose();
            }

            WorkBuff = new DataBuffer(GetBufferSize())
            {
                BigEndian = false,
                PaddingType = PaddingType,
                PaddingBytes = PaddingBytes
            };
        }

        /// <summary>
        /// Gets the work buffer size.
        /// </summary>
        /// <returns>The size of the work buffer in bytes.</returns>
        protected abstract int GetBufferSize();

        public void Dispose()
        {
            if (!m_disposed)
            {
                if (WorkBuff != null)
                {
                    WorkBuff.Dispose();
                    WorkBuff = null;
                }
                m_disposed = true;
            }
        }
    }
}