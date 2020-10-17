namespace GTASaveData.Types.Interfaces
{
    /// <summary>
    /// Allows an object to control its own binary serialization.
    /// </summary>
    public interface ISaveDataObject
    {
        /// <summary>
        /// Deserializes the object by reading its data from a <see cref="DataBuffer"/>
        /// using the specified <see cref="FileFormat"/> to control how data is read.
        /// </summary>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="fmt">The data format.</param>
        /// <returns>The number of bytes read.</returns>
        int ReadData(DataBuffer buf, FileFormat fmt);

        /// <summary>
        /// Serializes the object by writing its data to a <see cref="DataBuffer"/>
        /// using the specified <see cref="FileFormat"/> to control how data is written.
        /// </summary>
        /// <param name="buf">The buffer to write to.</param>
        /// <param name="fmt">The data format.</param>
        /// <returns>The number of bytes written.</returns>
        int WriteData(DataBuffer buf, FileFormat fmt);

        /// <summary>
        /// Gets the size in bytes of the serialized data.
        /// </summary>
        /// <param name="fmt">The data format.</param>
        int GetSize(FileFormat fmt);
    }
}
