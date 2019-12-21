using GTASaveData;
using System.IO;

namespace Tests.Helpers
{
    public static class SerializationHelper
    {
        //public static byte[] GetBytes<T>(T x, SystemType sysType = SystemType.Unspecified)
        //{
        //    return Serializer.Serialize<T>(x, sysType);
        //}

        public static byte[] GetBytes(string x, int? length = null, bool unicode = false, bool zeroTerminate = true)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (SaveDataSerializer s = SaveDataSerializer.CreateNew(m))
                {
                    s.Write(x, length, unicode, zeroTerminate);
                }

                return m.ToArray();
            }
        }

        //public static T FromBytes<T>(byte[] data, SystemType sysType = SystemType.Unspecified)
        //{
        //    return Serializer.Deserialize<T>(data, sysType);
        //}

        public static string FromBytes(byte[] data, bool unicode = false)
        {
            using (SaveDataSerializer s = SaveDataSerializer.CreateNew(new MemoryStream(data)))
            {
                return s.ReadString(unicode: unicode);
            }
        }

        public static string FromBytes(byte[] data, int length, bool unicode = false)
        {
            using (SaveDataSerializer s = SaveDataSerializer.CreateNew(new MemoryStream(data)))
            {
                return s.ReadString(length, unicode);
            }
        }
    }
}
