using Newtonsoft.Json;

namespace GTASaveData
{
    /// <summary>
    /// Represents an arbitrary data structure stored in a GTA savedata file.
    /// </summary>
    public abstract class SaveDataObject : ObservableObject, ISaveDataSerializable
    {
        void ISaveDataSerializable.WriteObjectData(SaveDataSerializer serializer, SystemType system)
        {
            WriteObjectData(serializer, system);
        }

        protected abstract void WriteObjectData(SaveDataSerializer serializer, SystemType system);

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
