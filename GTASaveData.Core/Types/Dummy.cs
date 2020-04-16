using GTASaveData.Converters;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace GTASaveData.Types
{
    /// <summary>
    /// A container for arbitraty data.
    /// </summary>
    public class Dummy : PreAllocatedSaveDataObject, IEquatable<Dummy>
    {
        private Array<byte> m_data;

        [JsonConverter(typeof(ByteArrayConverter))]
        public Array<byte> Data
        {
            get { return m_data; }
            set { m_data = value; OnPropertyChanged(); }
        }

        public Dummy()
            : base(0)
        { }

        public Dummy(int size)
            : base(size)
        { }

        protected override void PreAllocate(int size)
        {
            Data = new byte[size];
        }

        public static Dummy Load(byte[] data)
        {
            Dummy o = new Dummy(data.Length);
            Serializer.Read(o, data, DataFormat.Default);

            return o;
        }

        protected override void ReadObjectData(StreamBuffer buf, DataFormat fmt)
        {
            int count = Data.Count;
            Data = buf.ReadBytes(count);
        }

        protected override void WriteObjectData(StreamBuffer buf, DataFormat fmt)
        {
            buf.Write(Data.ToArray());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Dummy);
        }

        public bool Equals(Dummy other)
        {
            if (other == null)
            {
                return false;
            }

            return Data.SequenceEqual(other.Data);
        }
    }
}
