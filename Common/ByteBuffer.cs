namespace GTASaveData
{
    /// <summary>
    /// A byte array wrapper, made purely so byte[] can be manipulated as an object.
    /// </summary>
    public sealed class ByteBuffer
    {
        private readonly byte[] m_data;

        public ByteBuffer(int count)
        {
            m_data = new byte[count];
        }

        public ByteBuffer(byte[] data)
        {
            m_data = data;
        }

        public byte this[int i]
        {
            get { return m_data[i]; }
            set { m_data[i] = value; }
        }

        public int Length
        {
            get { return m_data.Length; }
        }

        public byte[] ToArray()
        {
            return m_data;
        }

        public static implicit operator byte[](ByteBuffer c)
        {
            return c.m_data;
        }

        public static implicit operator ByteBuffer(byte[] data)
        {
            return new ByteBuffer(data);
        }
    }
}
