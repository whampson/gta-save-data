using GTASaveData.Converters;
using Newtonsoft.Json;
using System;
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
            Data = new byte[count];
        }

        public DummyObject(byte[] data)
        {
            Data = data;
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            int count = Data.Count;
            Data = buf.ReadBytes(count);
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(Data.ToArray());
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

            return Data.SequenceEqual(other.Data);
        }
    }
}
