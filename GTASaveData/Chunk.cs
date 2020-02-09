using GTASaveData.Serialization;
using Newtonsoft.Json;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// Represents an arbitrary data structure stored in a Grand Theft Auto save data file.
    /// </summary>
    public abstract class Chunk : ObservableObject, IChunk
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

        void IChunk.ReadObjectData(Serializer r, FileFormat fmt)
        {
            ReadObjectData(r, fmt);
        }

        void IChunk.WriteObjectData(Serializer w, FileFormat fmt)
        {
            WriteObjectData(w, fmt);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
