namespace GTASaveData
{
    public abstract class SerialiationParamsGTA3VC : SerializationParams
    {
        public int WorkBufferSize { get; set; }

        public SerialiationParamsGTA3VC() : base()
        { }

        public SerialiationParamsGTA3VC(SerialiationParamsGTA3VC other) : base(other)
        {
            WorkBufferSize = other.WorkBufferSize;
        }
    }
}
