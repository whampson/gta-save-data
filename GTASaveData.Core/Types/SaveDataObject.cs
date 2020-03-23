﻿using GTASaveData.Types.Interfaces;
using System;

namespace GTASaveData.Types
{
    public abstract class SaveDataObject : GTAObject, ISaveDataObject
    {
        int ISaveDataObject.ReadObjectData(WorkBuffer buf)
        {
#if DEBUG
            int oldMark = buf.Mark;
            buf.MarkPosition();
            ReadObjectData(buf, SaveFileFormat.Default);
            buf.Mark = oldMark;
#else
            ReadObjectData(buf, SaveFileFormat.Default);
#endif

            return buf.Offset;
        }

        int ISaveDataObject.ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
#if DEBUG
            int oldMark = buf.Mark;
            buf.MarkPosition();
            ReadObjectData(buf, fmt);
            buf.Mark = oldMark;
#else
            ReadObjectData(buf, fmt);
#endif

            return buf.Offset;
        }

        protected abstract void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt);

        int ISaveDataObject.WriteObjectData(WorkBuffer buf)
        {
#if DEBUG
            int oldMark = buf.Mark;
            buf.MarkPosition();
            WriteObjectData(buf, SaveFileFormat.Default);
            buf.Mark = oldMark;
#else
            WriteObjectData(buf, SaveFileFormat.Default);
#endif

            return buf.Offset;
        }

        int ISaveDataObject.WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
#if DEBUG
            int oldMark = buf.Mark;
            buf.MarkPosition();
            WriteObjectData(buf, fmt);
            buf.Mark = oldMark;
#else
            WriteObjectData(buf, fmt);
#endif

            return buf.Offset;
        }

        protected abstract void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt);

        protected virtual int GetSize(SaveFileFormat fmt)
        {
            return Serializer.Write(this, fmt, out byte[] _);
        }

        public static int SizeOf<T>()
            where T : SaveDataObject, new()
        {
            SizeAttribute sizeAttr = (SizeAttribute) Attribute.GetCustomAttribute(typeof(T), typeof(SizeAttribute));
            if (sizeAttr == null)
            {
                return SizeOf(new T());
            }

            return sizeAttr.Size;
        }

        public static int SizeOf<T>(T obj)
            where T : SaveDataObject, new()
        {
            return SizeOf(obj, SaveFileFormat.Default);
        }

        public static int SizeOf<T>(SaveFileFormat fmt)
            where T : SaveDataObject, new()
        {
            return SizeOf(new T(), fmt);
        }

        public static int SizeOf<T>(T obj, SaveFileFormat fmt)
            where T : SaveDataObject, new()
        {
            return obj.GetSize(fmt);
        }
    }
}