using GTASaveData.Serialization;

namespace GTASaveData.Serialization
{
    /// <summary>
    /// Exposes the methods necessary for object serialization.
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// Reads this object's data from the specified <see cref="Serializer"/>.
        /// </summary>
        /// <param name="r">The data input stream.</param>
        /// <param name="fmt">The data format for reading platform-specific data.</param>
        void ReadObjectData(Serializer r, FileFormat fmt);

        /// <summary>
        /// Writes this object's data to the specified <see cref="Serializer"/>.
        /// </summary>
        /// <param name="w">The data output stream.</param>
        /// <param name="fmt">The data format for writing platform-specific data.</param>
        void WriteObjectData(Serializer w, FileFormat fmt);
    }
}
