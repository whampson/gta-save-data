using System;

namespace GTASaveData
{
    public class SerializationParams
    {
        public FileType FileType { get; set; }
        public bool BigEndian { get; set; }
        public PaddingScheme PaddingType { get; set; }
        public byte[] PaddingBytes { get; set; }

        public SerializationParams()
        {
            PaddingBytes = new byte[0];
        }

        public SerializationParams(FileType t) : this()
        {
            FileType = t;
        }

        public SerializationParams(SerializationParams other)
        {
            FileType = other.FileType;
            BigEndian = other.BigEndian;
            PaddingType = other.PaddingType;
            PaddingBytes = new byte[other.PaddingBytes.Length];
            Array.Copy(other.PaddingBytes, PaddingBytes, PaddingBytes.Length);
        }

        public static T GetDefaults<T>(FileType t)
            where T : SerializationParams, new()
        {
            return new T().GetDefaultsInternal<T>(t);
        }

        protected virtual T GetDefaultsInternal<T>(FileType t)
            where T : SerializationParams, new()
        {
            string msg = $"{nameof(GetDefaultsInternal)}() not implemented for type '{typeof(T).Name}'; please override.";
            throw new NotImplementedException(msg);
        }
    }
}
