namespace GTASaveData.Interfaces
{
    /// <summary>
    /// A serialization interface for objects written to <i>Grand Theft Auto</i> save files.
    /// </summary>
    public interface ISaveDataObject
    {
        /// <summary>
        /// Populates this object's fields by deserializing its data from a 
        /// <see cref="DataBuffer"/> using the specified <see cref="SerializationParams"/>
        /// to control how data is read.
        /// </summary>
        /// <param name="buf">The buffer to read from.</param>
        /// <param name="p">The <see cref="SerializationParams"/> controlling how data is read.</param>
        /// <returns>The number of bytes read.</returns>
        int ReadData(DataBuffer buf, SerializationParams p);

        /// <summary>
        /// Serializes the object by writing its fields to a <see cref="DataBuffer"/>
        /// using the specified <see cref="SerializationParams"/> to control how data is written.
        /// </summary>
        /// <param name="buf">The buffer to write to.</param>
        /// <param name="p">The <see cref="SerializationParams"/> controlling how data is written.</param>
        /// <returns>The number of bytes written.</returns>
        int WriteData(DataBuffer buf, SerializationParams p);

        /// <summary>
        /// Gets the size in bytes of this object.
        /// </summary>
        /// <param name="p">The <see cref="SerializationParams"/> controlling how data is written,
        /// which may affect the size.</param>
        /// <returns>The size in bytes of the serialized object.</returns>
        int GetSize(SerializationParams p);
    }
}
