using GTASaveData.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// A container for arbitraty data.
    /// </summary>
    public class Block : SerializableObject,
        IEquatable<Block>
    {
        private Array<byte> m_data;

        [JsonConverter(typeof(BinaryConverter))]
        public Array<byte> Data
        {
            get { return m_data; }
            set { m_data = value; OnPropertyChanged(); }
        }

        public int Length
        {
            get { return m_data.Count; }
        }

        public Block()
            : this(new byte[0])
        { }

        public Block(byte[] data)
        {
            m_data = data;
        }

        protected override void ReadObjectData(Serializer r, FileFormat fmt)
        {
            // nop
            Debug.WriteLine("Block#ReadObjectData(): useless call");
        }

        protected override void WriteObjectData(Serializer w, FileFormat fmt)
        {
            w.Write(m_data.ToArray());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Block);
        }

        public bool Equals(Block other)
        {
            if (other == null)
            {
                return false;
            }

            return m_data.SequenceEqual(other.m_data);
        }

        public static implicit operator byte[](Block b)
        {
            return b.m_data;
        }

        public static implicit operator Block(byte[] data)
        {
            return new Block(data);
        }
    }
}
