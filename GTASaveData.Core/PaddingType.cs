namespace GTASaveData
{
    /// <summary>
    /// Defines data structure padding schemes.
    /// </summary>
    public enum PaddingType
    {
        /// <summary>
        /// The existing data in the buffer will be used as padding.
        /// This is how the game does padding.
        /// </summary>
        Default,

        /// <summary>
        /// Zeros will be used as padding.
        /// </summary>
        Zero,

        /// <summary>
        /// A specific pattern will be used as padding.
        /// </summary>
        Pattern,

        /// <summary>
        /// Random bytes will be used as padding.
        /// </summary>
        Random,
    };
}
