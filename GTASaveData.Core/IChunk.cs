using GTASaveData.Serialization;

namespace GTASaveData
{
    /// <summary>
    /// Exposes the methods necessary for object serialization.
    /// The serialized blob is referred to as a Chunk.
    /// </summary>
    public interface IChunk
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
