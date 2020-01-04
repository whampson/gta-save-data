namespace GTASaveData.Serialization
{
    /// <summary>
    /// Rules for data structure padding.
    /// </summary>
    public enum PaddingMode
    {
        /// <summary>
        /// Zero-value bytes will be used for padding.
        /// </summary>
        Zeros,

        /// <summary>
        /// Random bytes will be used for padding.
        /// </summary>
        Random,

        /// <summary>
        /// A pre-defined sequence of bytes will be used for padding.
        /// </summary>
        Sequence
    };
}
