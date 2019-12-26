namespace GTASaveData
{
    /// <summary>
    /// Allows an object to control its own serialization for storage in a GTA savedata file.
    /// </summary>
    public interface ISaveDataSerializable
    {
        // === NOTE TO IMPLEMENTERS ===
        // You must create a deserialization constructor to populate your
        // object's fields when deserialization occurs. This constructor
        // should be protected or private and must have the following
        // signature:
        //     Ctor(SaveDataSerializer, SystemType)

        /// <summary>
        /// Writes this object's data to a <see cref="SaveDataSerializer"/>
        /// so it can be stored in a GTA savedata file.
        /// </summary>
        /// <param name="serializer">
        /// The <see cref="SaveDataSerializer"/> to write this object's data to.
        /// </param>
        /// <param name="system">
        /// The <see cref="SystemType"/> that data is being serialized for.
        /// </param>
        void WriteObjectData(SaveDataSerializer serializer, SystemType system);
    }
}
