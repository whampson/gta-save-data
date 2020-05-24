using Newtonsoft.Json;
using WpfEssentials;

namespace GTASaveData.Extensions
{
    public static class ObservableObjectExtensions
    {
        public static string ToJsonString(this ObservableObject o)
        {
            return ToJsonString(o, Formatting.Indented);
        }

        public static string ToJsonString(this ObservableObject o, Formatting formatting)
        {
            return JsonConvert.SerializeObject(o, formatting);
        }
    }
}
