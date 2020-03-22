using Newtonsoft.Json;
using System.Linq;
using WpfEssentials;

namespace GTASaveData.Types
{
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

        public static Array<T> CreateArray<T>(int count)
            where T : new()
        {
            return Enumerable.Repeat(new T(), count).ToArray();
        }
    }
}
