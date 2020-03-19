using GTASaveData.Types.Interfaces;
using Newtonsoft.Json;
using System;
using WpfEssentials;

namespace GTASaveData.Types
{
    public abstract class GTAObject : ObservableObject, IGTAObject
    {
        int IGTAObject.ReadObjectData(WorkBuffer buf)
        {
            buf.ResetMark();
            ReadObjectData(buf, SaveFileFormat.Default);

            return buf.Offset;
        }

        int IGTAObject.ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.ResetMark();
            ReadObjectData(buf, fmt);

            return buf.Offset;
        }

        protected abstract void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt);

        int IGTAObject.WriteObjectData(WorkBuffer buf)
        {
            buf.ResetMark();
            WriteObjectData(buf, SaveFileFormat.Default);

            return buf.Offset;
        }

        int IGTAObject.WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.ResetMark();
            WriteObjectData(buf, fmt);

            return buf.Offset;
        }

        protected abstract void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt);

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static int SizeOf<T>() where T : GTAObject, new()
        {
            SizeAttribute sizeAttr = (SizeAttribute) Attribute.GetCustomAttribute(typeof(T), typeof(SizeAttribute));
            if (sizeAttr == null)
            {
                return SizeOf(new T());
            }

            return sizeAttr.Size;
        }

        // TODO: test this with descendent types
        public static int SizeOf<T>(T obj) where T : GTAObject
        {
            return Serializer.Write(obj).Length;
        }
    }
}
