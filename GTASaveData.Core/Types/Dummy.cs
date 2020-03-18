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
    public class Dummy : GTAObject,
        IEquatable<Dummy>
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

        public Dummy()
            : this(new byte[0])
        { }

        public Dummy(byte[] data)
        {
            m_data = data;
        }

        protected override void ReadObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            // nop
            Debug.WriteLine("Useless call to Dummy#ReadObjectData()");
        }

        protected override void WriteObjectData(WorkBuffer buf, SaveFileFormat fmt)
        {
            buf.Write(m_data.ToArray());
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

            return m_data.SequenceEqual(other.m_data);
        }

        public static implicit operator byte[](Dummy b)
        {
            return b.m_data;
        }

        public static implicit operator Dummy(byte[] data)
        {
            return new Dummy(data);
        }
    }
}
