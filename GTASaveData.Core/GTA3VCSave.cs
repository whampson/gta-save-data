using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData
{
    public abstract class GTA3VCSave : GTASaveFile
    {
        public const int SaveHeaderSize = 8;

        private bool m_blockSizeChecks;
        
        [JsonIgnore]
        public bool BlockSizeChecks
        {
            get { return m_blockSizeChecks; }
            set { m_blockSizeChecks = value; OnPropertyChanged(); }
        }

        protected StreamBuffer WorkBuff { get; }
        protected int CheckSum { get; set; }
        protected abstract int BufferSize { get; }

        protected GTA3VCSave(int maxBufferSize)
        {
            WorkBuff = new StreamBuffer(new byte[maxBufferSize]);
        }

        public static int ReadSaveHeader(StreamBuffer buf, string tag)
        {
            string readTag = buf.ReadString(4);
            int size = buf.ReadInt32();

            Debug.Assert(readTag == tag, "Invalid block tag (expected: {0}, actual: {1})", tag, readTag);
            return size;
        }

        public static void WriteSaveHeader(StreamBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4, zeroTerminate: true);
            buf.Write(size);
        }

        protected abstract void LoadSimpleVars();
        protected abstract void SaveSimpleVars();

        protected T Load<T>() where T : SaveDataObject, new()
        {
            T obj = new T();
            Load(obj);

            return obj;
        }
        protected int Load<T>(T obj) where T : SaveDataObject
        {
            int size = WorkBuff.ReadInt32();
            int bytesRead = Serializer.Read(obj, WorkBuff, FileFormat);

            Debug.WriteLine("{0}: {1} bytes read", typeof(T).Name, bytesRead);
            Debug.Assert(bytesRead <= StreamBuffer.Align4Bytes(size));

            return bytesRead;
        }

        protected T LoadPreAlloc<T>() where T : SaveDataObject
        {
            int size = WorkBuff.ReadInt32();
            if (!(Activator.CreateInstance(typeof(T), size) is T obj))
            {
                throw new SerializationException("Object cannot be pre-allocated.");
            }
            Debug.WriteLine("{0}: {1} bytes pre-allocated", typeof(T).Name, size);

            int bytesRead = Serializer.Read(obj, WorkBuff, FileFormat);
            Debug.WriteLine("{0}: {1} bytes read", typeof(T).Name, bytesRead);
            Debug.Assert(bytesRead <= StreamBuffer.Align4Bytes(size));

            return obj;
        }

        protected void Save(SaveDataObject o)
        {
            int size, preSize, postData;

            preSize = WorkBuff.Cursor;
            WorkBuff.Skip(4);

            size = Serializer.Write(WorkBuff, o, FileFormat);
            postData = WorkBuff.Cursor;

            WorkBuff.Seek(preSize);
            WorkBuff.Write(size);
            WorkBuff.Seek(postData);
            WorkBuff.Align4Bytes();

            Debug.WriteLine("{0}: {1} bytes written", o.GetType().Name, size);
        }

        protected int ReadBlock(StreamBuffer file)
        {
            file.MarkCurrentPosition();
            WorkBuff.Reset();

            int size = file.ReadInt32();
            if ((uint) size > BufferSize)
            {
                Debug.WriteLine("Maximum block size exceeded! (value = {0}, max = {1})", (uint) size, BufferSize);
                if (BlockSizeChecks)
                {
                    throw BlockSizeExceeded((uint) size, BufferSize);
                }
            }

            WorkBuff.Write(file.ReadBytes(size));
            Debug.Assert(file.Offset == size + 4);

            WorkBuff.Reset();
            return size;
        }

        protected int WriteBlock(StreamBuffer file)
        {
            file.MarkCurrentPosition();

            byte[] data = WorkBuff.GetBytes();
            int size = data.Length;
            if ((uint) size > BufferSize)
            {
                Debug.WriteLine("Maximum block size exceeded! (value = {0}, max = {1})", (uint) size, BufferSize);
                if (BlockSizeChecks)
                {
                    throw BlockSizeExceeded((uint) size, BufferSize);
                }
            }

            file.Write(size);
            file.Write(data);
            file.Align4Bytes();

            Debug.Assert(file.Offset == size + 4);

            CheckSum += BitConverter.GetBytes(size).Sum(x => x);
            CheckSum += data.Sum(x => x);

            WorkBuff.Reset();
            return size;
        }

        protected SerializationException BlockSizeExceeded(uint value, int max)
        {
            return new SerializationException(Strings.Error_BlockSizeExceeded, value, max);
        }
    }
}