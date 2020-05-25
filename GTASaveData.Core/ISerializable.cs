namespace GTASaveData
{
    /// <summary>
    /// Allows an object to control its own serialization and deserialization.
    /// </summary>
    /// <remarks>
    /// Not to be confused with <see cref="System.Runtime.Serialization.ISerializable"/>.
    /// </remarks>
    public interface ISerializable
    {
        /// <summary>
        /// Deserializes the object by readong its data from the specified <see cref="StreamBuffer"/>
        /// using the specified <see cref="SaveDataFormat"/> to control how data is read.
        /// </summary>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="fmt">The data format.</param>
        /// <returns>The number of bytes read.</returns>
        int ReadData(StreamBuffer buf, SaveDataFormat fmt);

        /// <summary>
        /// Serializes the object by writing its data to the specified <see cref="StreamBuffer"/>
        /// using the specified <see cref="SaveDataFormat"/> to control how data is written.
        /// </summary>
        /// <param name="buf">The buffer to write to.</param>
        /// <param name="fmt">The data format.</param>
        /// <returns>The number of bytes written.</returns>
        int WriteData(StreamBuffer buf, SaveDataFormat fmt);

        /// <summary>
        /// Gets the size in bytes of the serialized data.
        /// </summary>
        /// <param name="fmt">The data format.</param>
        /// <returns>The size of the serialized object in bytes.</returns>
        int GetSize(SaveDataFormat fmt);
    }
}
