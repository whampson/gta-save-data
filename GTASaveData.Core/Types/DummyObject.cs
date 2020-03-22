using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        [JsonConverter(typeof(BinaryConverter))]
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
            : this(new byte[0])
        { }

        public DummyObject(byte[] data)
        {
            m_data = data;
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            // nop
            Debug.WriteLine("Warning: Useless call to DummyObject#ReadObjectData()");
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
