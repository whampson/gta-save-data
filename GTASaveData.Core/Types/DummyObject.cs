using GTASaveData.Converters;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

namespace GTASaveData.Types
{
    /// <summary>
    /// A container for arbitraty data.
    /// </summary>
    public class DummyObject : SaveDataObject,
        IEquatable<DummyObject>
    {
        private Array<byte> m_data;

        [JsonConverter(typeof(ByteArrayConverter))]
        public Array<byte> Data
        {
            get { return m_data; }
            set { m_data = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public int Length
        {
            get { return m_data.Count; }
        }

        public DummyObject()
            : this(0)
        { }

        public DummyObject(int count)
        {
            m_data = new byte[count];
        }

        public DummyObject(byte[] data)
        {
            m_data = data;
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            byte[] data = buf.ReadBytes(m_data.Count);
            m_data = data;
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(m_data.ToArray());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DummyObject);
        }

        public bool Equals(DummyObject other)
        {
            if (other == null)
            {
                return false;
            }

            return m_data.SequenceEqual(other.m_data);
        }
    }
}
