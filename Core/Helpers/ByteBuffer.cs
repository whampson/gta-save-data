namespace GTASaveData.Helpers
{
    /// <summary>
    /// A byte array wrapper, made purely so byte[] can be manipulated as an object.
    /// </summary>
    public sealed class ByteBuffer
    {
        private readonly byte[] m_data;

        /// <summary>
        /// Creates a new <see cref="ByteBuffer"/> instance with the specified size.
        /// </summary>
        /// <param name="count">The number of bytes in the buffer.</param>
        public ByteBuffer(int count)
        {
            m_data = new byte[count];
        }

        /// <summary>
        /// Creates a <see cref="ByteBuffer"/> instance from the specified byte array.
        /// </summary>
        /// <param name="data">A byte array containing bytes to be copied into the buffer.</param>
        public ByteBuffer(byte[] data)
        {
            m_data = data;
        }

        /// <summary>
        /// Gets or sets the i'th byte of the buffer.
        /// </summary>
        /// <param name="i">The index of the byte to access.</param>
        /// <returns>The byte at the i'th position in the buffer.</returns>
        public byte this[int i]
        {
            get { return m_data[i]; }
            set { m_data[i] = value; }
        }

        /// <summary>
        /// Gets the total number of bytes in the buffer.
        /// </summary>
        public int Length
        {
            get { return m_data.Length; }
        }

        /// <summary>
        /// Converts this <see cref="ByteBuffer"/> into a byte array.
        /// </summary>
        /// <returns>The data in the <see cref="ByteBuffer"/> as a byte array.</returns>
        public byte[] ToArray()
        {
            return m_data;
        }

        /// <summary>
        /// Converts a <see cref="ByteBuffer"/> instance into a byte array.
        /// </summary>
        /// <param name="buf">The <see cref="ByteBuffer"/> to convert.</param>
        public static implicit operator byte[](ByteBuffer buf)
        {
            return buf.m_data;
        }

        /// <summary>
        /// Converts a byte array into a <see cref="ByteBuffer"/> instance.
        /// </summary>
        /// <param name="data">The byte array to convert.</param>
        public static implicit operator ByteBuffer(byte[] data)
        {
            return new ByteBuffer(data);
        }
    }
}
