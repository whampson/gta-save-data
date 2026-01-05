namespace GTASaveData
{
    /// <summary>
    /// A serializable object containing an internal serialization buffer.
    /// </summary>
    public abstract class BufferedObject : SaveDataObject
    {
        private ObservableArray<byte> m_workBuffer;

        public BufferedObject() : this(0) { }
        public BufferedObject(int size) { m_workBuffer = new byte[size]; }
        public BufferedObject(BufferedObject other) { m_workBuffer = new ObservableArray<byte>(other.m_workBuffer); }

        /// <summary>
        /// The internal buffer containing object data.
        /// </summary>
        protected ObservableArray<byte> WorkBuffer
        {
            get { return m_workBuffer; }
            set { m_workBuffer = value; OnPropertyChanged(); }
        }

        protected override int GetSize(SerializationParams p)
        {
            return WorkBuffer.Count;
        }
    }
}
