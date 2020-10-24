using GTASaveData.Interfaces;
using GTASaveData.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace GTASaveData
{
    /// <summary>
    /// A container for arbitraty data.
    /// </summary>
    public class Dummy : SaveDataObject,
        IEquatable<Dummy>, IDeepClonable<Dummy>
    {
        private Array<byte> m_data;

        [JsonConverter(typeof(ByteArrayConverter))]
        public Array<byte> Data
        {
            get { return m_data; }
            set { m_data = value; OnPropertyChanged(); }
        }

        public Dummy()
            : this(0)
        { }

        public Dummy(int size)
        {
            Data = new byte[size];
        }

        public Dummy(Dummy other)
        {
            Data = other.Data;
        }

        protected override void ReadData(DataBuffer buf, FileFormat fmt)
        {
            int count = Data.Count;
            Data = buf.ReadBytes(count);
        }

        protected override void WriteData(DataBuffer buf, FileFormat fmt)
        {
            buf.Write(Data.ToArray());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return Data.Count;
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

        public Dummy DeepClone()
        {
            return new Dummy(this);
        }
    }
}
