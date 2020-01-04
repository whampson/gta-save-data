namespace GTASaveData.Serialization
{
    /// <summary>
    /// Allows an object to control its own serialization for storage in a
    /// Grand Theft Auto save data file.
    /// </summary>
    /// <remarks>
    /// Note to inheritors: You must create a deserialization constructor
    /// to populate your object's fields when deserialization occurs. This
    /// constructor should be protected or private and must have the following
    /// signature:
    /// <code>Ctor(SaveDataSerializer, FileFormat)</code>
    /// </remarks>
    public interface IGTASerializable
    {
        /// <summary>
        /// Writes this object's data to the specified <see cref="SaveDataSerializer"/>
        /// using the specified <see cref="FileFormat"/>.
        /// </summary>
        /// <param name="serializer">
        /// The <see cref="SaveDataSerializer"/> to write this object's data to.
        /// </param>
        /// <param name="format">
        /// The <see cref="FileFormat"/> that data is being serialized for.
        /// </param>
        void WriteObjectData(SaveDataSerializer serializer, FileFormat format);
    }
}
