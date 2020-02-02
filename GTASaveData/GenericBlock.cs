using GTASaveData.Serialization;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace GTASaveData
{
    public class GenericBlock : Chunk, IEquatable<GenericBlock>
    {
        private DynamicArray<byte> m_data;

        [JsonIgnore]
        public DynamicArray<byte> Data
        {
            get { return m_data; }
            set { m_data = value; OnPropertyChanged(); }
        }

        public byte[] Bytes
        {
            get { return m_data.ToArray(); }
        }

        public GenericBlock()
            : this(new byte[0])
        { }

        public GenericBlock(byte[] data)
        {
            Data = data;
        }

        private GenericBlock(SaveDataSerializer serializer, FileFormat format)
        {
            // nop
        }

        protected override void WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            serializer.Write(m_data.ToArray());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GenericBlock);
        }

        public bool Equals(GenericBlock other)
        {
            if (other == null)
            {
                return false;
            }

            return m_data.SequenceEqual(other.m_data);
        }
    }
}
