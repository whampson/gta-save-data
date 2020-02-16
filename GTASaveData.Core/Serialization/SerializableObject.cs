using Newtonsoft.Json;
using WpfEssentials;

namespace GTASaveData.Serialization
{
    /// <summary>
    /// Represents an arbitrary data structure stored in a <i>Grand Theft Auto</i> save file.
    /// </summary>
    public abstract class SerializableObject : ObservableObject, ISerializable
    {
        /// <summary>
        /// Popualates this object's fields by reading data from the specified <see cref="Serializer"/>.
        /// </summary>
        /// <param name="r">The data input stream.</param>
        /// <param name="fmt">The data format for reading platform-specific data.</param>
        protected abstract void ReadObjectData(Serializer r, FileFormat fmt);

        /// <summary>
        /// Writes this object's data to the specified <see cref="Serializer"/>.
        /// </summary>
        /// <param name="w">The data output stream.</param>
        /// <param name="fmt">The data format for writing platform-specific data.</param>
        protected abstract void WriteObjectData(Serializer w, FileFormat fmt);

        void ISerializable.ReadObjectData(Serializer r, FileFormat fmt)
        {
            ReadObjectData(r, fmt);
        }

        void ISerializable.WriteObjectData(Serializer w, FileFormat fmt)
        {
            WriteObjectData(w, fmt);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        // TODO: optimize calculation with [SerializedSize()] on fixed-size types
        public static int SizeOf<T>() where T : SerializableObject, new()
        {
            return Serializer.Serialize(new T()).Length;
        }

        public static int SizeOf<T>(FileFormat fmt) where T : SerializableObject, new()
        {
            return Serializer.Serialize(new T(), format: fmt).Length;
        }
    }
}
