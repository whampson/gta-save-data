using Newtonsoft.Json;
using System.Linq;
using WpfEssentials;

namespace GTASaveData.Types
{
    public abstract class GTAObject : ObservableObject
    {
        #region Helper Functions
        public static Array<T> CreateArray<T>(int count) where T : new()
        {
            return Enumerable.Repeat(new T(), count).ToArray();
        }
        #endregion

        #region Stringification Functions
        public string ToJsonString()
        {
            return ToJsonString(Formatting.Indented);
        }

        public string ToJsonString(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }
        #endregion
    }
}
