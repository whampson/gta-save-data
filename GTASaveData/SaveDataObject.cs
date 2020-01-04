using GTASaveData.Serialization;
using Newtonsoft.Json;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// Represents an arbitrary data structure stored in a Grand Theft Auto
    /// save data file.
    /// </summary>
    public abstract class SaveDataObject : ObservableObject,
        IGTASerializable
    {
        void IGTASerializable.WriteObjectData(SaveDataSerializer serializer, FileFormat format)
        {
            WriteObjectData(serializer, format);
        }

        /// <summary>
        /// Writes this object's data to the specified <see cref="SaveDataSerializer"/>
        /// using the specified <see cref="FileFormat"/>.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="format"></param>
        protected abstract void WriteObjectData(SaveDataSerializer serializer, FileFormat format);

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
