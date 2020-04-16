using Newtonsoft.Json;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// Represents a data structure from a <i>Grand Theft Auto</i> game.
    /// </summary>
    public abstract class GTAObject : ObservableObject
    {
        public string ToJsonString()
        {
            return ToJsonString(Formatting.Indented);
        }

        public string ToJsonString(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }
    }
}
