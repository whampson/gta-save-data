using System;

namespace GTASaveData
{
    public class SerializationParams
    {
        public bool BigEndian { get; set; }
        public PaddingScheme PaddingType { get; set; }
        public byte[] PaddingBytes { get; set; }

        public SerializationParams()
        {
            BigEndian = false;
            PaddingType = PaddingScheme.Skip;
            PaddingBytes = new byte[0];
        }

        public SerializationParams(SerializationParams other)
        {
            BigEndian = other.BigEndian;
            PaddingType = other.PaddingType;
            PaddingBytes = new byte[other.PaddingBytes.Length];
            Array.Copy(other.PaddingBytes, PaddingBytes, PaddingBytes.Length);
        }
    }
}
