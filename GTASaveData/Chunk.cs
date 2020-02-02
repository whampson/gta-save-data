using GTASaveData.Serialization;
using Newtonsoft.Json;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// Represents an arbitrary data structure stored in a Grand Theft Auto
    /// save data file.
    /// </summary>
    public abstract class Chunk : ObservableObject, IGTASerializable
    {
        void IGTASerializable.WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            WriteObjectData(serializer, format);
        }

        /// <summary>
        /// Writes this object's data to the specified <see cref="SaveDataSerializer"/>
        /// using the specified <see cref="FileFormat"/>.
        /// </summary>
        /// <param name="w">Output stream.</param>
        /// <param name="fmt">File format.</param>
        protected abstract void WriteObjectData(SaveDataSerializer w, FileFormat fmt);

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
