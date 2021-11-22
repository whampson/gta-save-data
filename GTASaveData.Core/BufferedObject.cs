namespace GTASaveData
{
    /// <summary>
    /// A serializable object containing an internal serialization buffer
    /// that's allocated during object construction.
    /// </summary>
    /// <remarks>
    /// Extend this class if the type you're working with has a variable
    /// size that cannot be determined implicitly from the type's own data
    /// (i.e. the size must be known before reading and cannot be determined while reading).
    /// </remarks>
    public abstract class BufferedObject : SaveDataObject
    {
        private ObservableArray<byte> m_workBuffer;

        public BufferedObject() : this(0) { }
        public BufferedObject(int size) { m_workBuffer = new byte[size]; }
        public BufferedObject(BufferedObject other) { m_workBuffer = new ObservableArray<byte>(other.m_workBuffer); }

        /// <summary>
        /// The internal work buffer allocated at object creation.
        /// </summary>
        protected ObservableArray<byte> WorkBuffer
        {
            get { return m_workBuffer; }
            set { m_workBuffer = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Fills the work buffer with data from the source buffer.
        /// </summary>
        protected virtual void FillWorkBuffer(DataBuffer src)
        {
            int count = SizeOf(this);
            WorkBuffer = src.ReadBytes(count);
        }

        /// <summary>
        /// Writes data in the work buffer to the specified destination buffer.
        /// </summary>
        protected virtual void WriteWorkBuffer(DataBuffer dest)
        {
            _ = dest.Write(WorkBuffer.ToArray());
        }

        protected override int GetSize(FileFormat fmt)
        {
            return WorkBuffer.Count;
        }
    }
}
