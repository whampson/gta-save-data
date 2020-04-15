using GTASaveData.Converters;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace GTASaveData.Types
{
    /// <summary>
    /// A container for arbitraty data.
    /// </summary>
    public class DummyObject : PreAllocatedSaveDataObject, IEquatable<DummyObject>
    {
        private Array<byte> m_data;

        [JsonConverter(typeof(ByteArrayConverter))]
        public Array<byte> Data
        {
            get { return m_data; }
            set { m_data = value; OnPropertyChanged(); }
        }

        public DummyObject()
            : base(0)
        { }

        public DummyObject(int size)
            : base(size)
        { }

        public static DummyObject Load(byte[] data)
        {
            DummyObject o = new DummyObject(data.Length);
            Serializer.Read(o, data, SaveFileFormat.Default);

            return o;
        }

        protected override void PreAllocate(int size)
        {
            Data = new byte[size];
        }

        protected override void ReadObjectData(DataBuffer buf, SaveFileFormat fmt)
        {
            int count = Data.Count;
            Data = buf.ReadBytes(count);
        }

        protected override void WriteObjectData(DataBuffer buf, SaveFileFormat fmt)
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
