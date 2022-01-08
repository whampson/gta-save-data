namespace GTASaveData
{
    public abstract class SerialiationParamsGTA3VC : SerializationParams
    {
        /// <summary>
        /// The size of the buffer used to load and store data blocks.
        /// Blocks cannot exceed the work buffer size.
        /// </summary>
        public int WorkBufferSize { get; set; }

        public SerialiationParamsGTA3VC() : base()
        { }

        public SerialiationParamsGTA3VC(SerialiationParamsGTA3VC other) : base(other)
        {
            WorkBufferSize = other.WorkBufferSize;
        }
    }
}
