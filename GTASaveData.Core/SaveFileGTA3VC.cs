using GTASaveData.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// A <see cref="SaveFile"/> generalizing the structure shared by
    /// <i>Grand Theft Auto III</i> and <i>Grand Theft Auto: Vice City</i>.
    /// </summary>
    /// <remarks>
    /// The Load/Save functionality mimics that of the games themselves for the best data accuracy.
    /// </remarks>
    public abstract class SaveFileGTA3VC : SaveFile, IDisposable
    {
        private PaddingScheme m_paddingType;
        private byte[] m_paddingBytes;
        private bool m_disposed;

        /// <summary>
        /// A fixed-size buffer used while parsing and building GTA3/VC saves.
        /// </summary>
        /// <remarks>
        /// The buffer size may vary by the file type.
        /// </remarks>
        protected DataBuffer WorkBuff { get; private set; }

        /// <summary>
        /// A data integrity checksum stored at the end of the file.
        /// </summary>
        /// <remarks>
        /// The checksum is calculated by summing all preceding bytes in the file.</remarks>
        protected int CheckSum { get; set; }

        /// <summary>
        /// The data padding scheme, which controls the bytes written for data structure alignment and padding.
        /// </summary>
        public PaddingScheme PaddingType
        {
            get { return m_paddingType; }
            set { m_paddingType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The bytes to use for padding when the <see cref="PaddingType"/> is set to
        /// <see cref="PaddingScheme.Pattern"/>.
        /// </summary>
        public byte[] PaddingBytes
        {
            get { return m_paddingBytes; }
            set { m_paddingBytes = value; OnPropertyChanged(); }
        }

        protected SaveFileGTA3VC(Game game) : base(game) { }

        /// <summary>
        /// The size in bytes of a data block header.
        /// </summary>
        public const int BlockHeaderSize = 8;

        /// <summary>
        /// Reads a block header from the buffer.
        /// </summary>
        /// <param name="buf">The buffer to read.</param>
        /// <param name="tag">The block tag extracted from the header.</param>
        /// <returns>The size in bytes of the block excluding the header.</returns>
        public static int ReadBlockHeader(DataBuffer buf, out string tag)
        {
            tag = buf.ReadString(4);
            int size = buf.ReadInt32();

            return size;
        }

        /// <summary>
        /// Writes a block header to the buffer.
        /// </summary>
        /// <param name="buf">The buffer to write.</param>
        /// <param name="tag">The block tag.</param>
        /// <param name="size">The size in bytes of the block excluding the header.</param>
        public static void WriteBlockHeader(DataBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4, zeroTerminate: true);
            buf.Write(size);
        }

        /// <summary>
        /// Reads a chunk of data from the source buffer into the work buffer.
        /// </summary>
        /// <remarks>
        /// The size of the chunk is affixed to the prologue of the data being read
        /// as a 4-byte integer. The file pointer is aligned to the nearest 4-byte
        /// boundary after reading.
        /// </remarks>
        protected int FillWorkBuffer(DataBuffer src)
        {
            src.Mark();
            WorkBuff.Reset();

            int size = src.ReadInt32();
            if ((uint) size > WorkBuff.Length) throw new SerializationException(Strings.Error_Serialization_BadBlockSize, (uint) size);

            WorkBuff.Write(src.ReadBytes(size));
            src.Align4();

            Debug.Assert(src.Offset == size + 4);
            Debug.WriteLine($"{size} bytes read into work buffer");

            WorkBuff.Reset();
            return size;
        }

        /// <summary>
        /// Writes the current work buffer to the destination buffer,
        /// updates the <see cref="CheckSum"/>, then resets the work buffer.
        /// </summary>
        /// <remarks>
        /// The buffer size is affixed to the prologue of the stored data as a 4-byte integer. The
        /// file pointer is aligned to the nearest 4-byte boundary after writing.
        /// </remarks>
        protected int FlushWorkBuffer(DataBuffer dest)
        {
            dest.Mark();

            byte[] data = WorkBuff.GetBytes();
            int size = data.Length;

            dest.Write(size);
            dest.Write(data);
            dest.Align4();

            // The game code has a bug where the size of the 'size' DWORD itself
            // is not factored into the total file size. As a result, all save files
            // are 4 * numBlocks bytes larger than told by the 'SizeOfGameInBytes' constant
            // found in the file.
            Debug.Assert(dest.Offset - 4 == size);
            Debug.WriteLine($"{size} bytes written out of work buffer");

            CheckSum += dest.GetBytesFromMark().Sum(x => x);
            WorkBuff.Reset();

            return size;
        }

        /// <summary>
        /// Reads an object from the work buffer.
        /// </summary>
        /// <remarks>
        /// The object is preceded by a 4-byte value containing the object size.
        /// As a result, the actual number of bytes read is 4 larger than the object size.
        /// </remarks>
        protected T Get<T>() where T : SaveDataObject, new()
        {
            T obj = new T();
            _ = Get(ref obj);

            return obj;
        }

        /// <summary>
        /// Reads an object from the work buffer.
        /// </summary>
        /// <remarks>
        /// The object is preceded by a 4-byte value containing the object size.
        /// As a result, the actual number of bytes read is 4 larger than the object size.
        /// </remarks>
        /// <param name="obj">The object to read.</param>
        /// <returns>The number of bytes read.</returns>
        protected int Get<T>(ref T obj) where T : SaveDataObject
        {
            int size = WorkBuff.ReadInt32();
            if (obj is BufferedObject)
            {
                if (!(Activator.CreateInstance(typeof(T), size) is T o))
                {
                    throw new SerializationException(Strings.Error_Serialization_NoPreAlloc, typeof(T));
                }
                obj = o;
            }

            int bytesRead = WorkBuff.ReadObject(obj, FileType);
            WorkBuff.Align4();

            Debug.WriteLine($"{typeof(T).Name}: {bytesRead} bytes read");
            Debug.Assert(bytesRead <= DataBuffer.Align4(size));

            return bytesRead;
        }

        /// <summary>
        /// Writes an object to the work buffer.
        /// </summary>
        /// <remarks>
        /// The object is preceded by a 4-byte value containing the object size.
        /// As a result, the actual number of bytes written is 4 larger than the object size.
        /// </remarks>
        /// <param name="obj">The object to write.</param>
        protected void Put<T>(T obj) where T : SaveDataObject
        {
            int size, preSize, postData;

            preSize = WorkBuff.Position;
            WorkBuff.Skip(4);

            size = WorkBuff.Write(obj, FileType);
            postData = WorkBuff.Position;

            WorkBuff.Seek(preSize);
            WorkBuff.Write(size);
            WorkBuff.Seek(postData);
            WorkBuff.Align4();

            Debug.WriteLine($"{typeof(T).Name}: {size} bytes written");
        }

        protected override void OnReading(FileFormat fmt)
        {
            base.OnReading(fmt);
            FileType = fmt;
            InitWorkBuffer();
        }

        protected override void OnWriting(FileFormat fmt)
        {
            base.OnWriting(fmt);
            FileType = fmt;
            InitWorkBuffer();
        }

        protected override void OnFileLoad(string path)
        {
            base.OnFileLoad(path);

            if (FileType.IsPS2 || FileType.IsMobile)
            {
                TimeStamp = File.GetLastWriteTime(path);
            }
        }

        private void InitWorkBuffer()
        {
            if (WorkBuff != null)
            {
                WorkBuff.Dispose();
            }

            WorkBuff = new DataBuffer(GetWorkBufferSize())
            {
                BigEndian = false,
                PaddingType = PaddingType,
                PaddingBytes = PaddingBytes
            };
        }

        /// <summary>
        /// Returns the work buffer size.
        /// </summary>
        /// <remarks>
        /// The work buffer size may be dependent on the file type.</remarks>
        protected abstract int GetWorkBufferSize();

        /// <summary>
        /// Dispose of this object.
        /// </summary>
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