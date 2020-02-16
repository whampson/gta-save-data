using GTASaveData.Serialization;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// A container for arbitraty data. Useful for storing unknown block data.
    /// </summary>
    public class Block : SerializableObject,
        IEquatable<Block>
    {
        // TODO: custom JSON serializer so Data can be a base64 string?

        private Array<byte> m_data;

        [JsonIgnore]
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
            Debug.WriteLine("WARN: Calling useless method: Block#ReadObjectData()");
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
