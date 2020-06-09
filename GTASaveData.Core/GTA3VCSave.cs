using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// Commonalities between GTA3 and VC saves.
    /// </summary>
    public abstract class GTA3VCSave : SaveData, IDisposable
    {
        public const int BlockHeaderSize = 8;

        private bool m_disposed;

        protected StreamBuffer WorkBuff { get; private set; }
        protected int CheckSum { get; set; }

        protected GTA3VCSave()
        {
            WorkBuff = new StreamBuffer();
        }

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

        public static int ReadSaveHeader(StreamBuffer buf, string tag)
        {
            string readTag = buf.ReadString(4);
            int size = buf.ReadInt32();

            Debug.Assert(readTag == tag, $"Invalid block tag (expected: {tag}, actual: {readTag})");
            return size;
        }

        public static void WriteSaveHeader(StreamBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4, zeroTerminate: true);
            buf.Write(size);
        }

        protected override void OnReading()
        {
            base.OnReading();
            ReInitWorkBuff();
        }

        protected override void OnWriting()
        {
            base.OnWriting();
            ReInitWorkBuff();
        }

        private void ReInitWorkBuff()
        {
            if (WorkBuff != null)
            {
                WorkBuff.Dispose();
                WorkBuff = new StreamBuffer(GetBufferSize());
            }
        }

        protected abstract int GetBufferSize();

        protected T LoadType<T>() where T : SaveDataObject, new()
        {
            T obj = new T();
            LoadObject(obj);

            return obj;
        }

        protected T LoadTypePreAlloc<T>() where T : SaveDataObject
        {
            int size = WorkBuff.ReadInt32();
            if (!(Activator.CreateInstance(typeof(T), size) is T obj))
            {
                throw new SerializationException(Strings.Error_Serialization_NoPreAlloc, typeof(T));
            }
            Debug.WriteLine($"{typeof(T).Name}: {size} bytes pre-allocated");

            int bytesRead = Serializer.Read(obj, WorkBuff, FileFormat);
            WorkBuff.Align4();

            Debug.WriteLine($"{typeof(T).Name}: {bytesRead} bytes read");
            Debug.Assert(bytesRead <= StreamBuffer.Align4(size));

            return obj;
        }

        protected int LoadObject<T>(T obj) where T : SaveDataObject
        {
            int size = WorkBuff.ReadInt32();
            int bytesRead = Serializer.Read(obj, WorkBuff, FileFormat);
            WorkBuff.Align4();

            Debug.WriteLine($"{typeof(T).Name}: {bytesRead} bytes read");
            Debug.Assert(bytesRead <= StreamBuffer.Align4(size));

            return bytesRead;
        }

        protected void SaveObject(SaveDataObject o)
        {
            int size, preSize, postData;

            preSize = WorkBuff.Position;
            WorkBuff.Skip(4);

            size = Serializer.Write(WorkBuff, o, FileFormat);
            postData = WorkBuff.Position;

            WorkBuff.Seek(preSize);
            WorkBuff.Write(size);
            WorkBuff.Seek(postData);
            WorkBuff.Align4();

            Debug.WriteLine($"{o.GetType().Name}: {size} bytes written");
        }

        protected int ReadBlock(StreamBuffer file)
        {
            file.Mark();
            WorkBuff.Reset();

            int size = file.ReadInt32();
            if (size < 0)
            {
                throw new SerializationException(Strings.Error_Serialization_BadBlockSize, size);
            }
            WorkBuff.Write(file.ReadBytes(size));
            Debug.Assert(file.Offset == size + 4);
            file.Align4();

            WorkBuff.Reset();
            return size;
        }

        protected int WriteBlock(StreamBuffer file)
        {
            file.Mark();

            byte[] data = WorkBuff.GetBytes();
            int size = data.Length;

            file.Write(size);
            file.Write(data);
            file.Align4();

            Debug.Assert(file.Offset == size + 4);

            CheckSum += BitConverter.GetBytes(size).Sum(x => x);
            CheckSum += data.Sum(x => x);

            WorkBuff.Reset();
            return size;
        }
    }
}