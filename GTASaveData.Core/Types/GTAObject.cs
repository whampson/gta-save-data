using Newtonsoft.Json;
using System;
using System.Linq;
using WpfEssentials;

namespace GTASaveData.Types
{
    public abstract class GTAObject : ObservableObject
    {
        public string ToJsonString()
        {
            return ToJsonString(Formatting.Indented);
        }

        public string ToJsonString(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public static Array<T> CreateArray<T>(int count)
            where T : new()
        {
            return Enumerable.Repeat(new T(), count).ToArray();
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
            where T : SaveDataObject
        {
            return SizeOf(obj, SaveFileFormat.Default);
        }

        public static int SizeOf<T>(T obj, SaveFileFormat fmt)
            where T : SaveDataObject
        {
            return Serializer.Write(obj, fmt, out byte[] _);
        }
    }
}
