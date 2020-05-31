using Newtonsoft.Json;
using WpfEssentials;

namespace GTASaveData.Extensions
{
    public static class ObservableObjectExtensions
    {
        /// <summary>
        /// Gets the JSON representation of this object.
        /// </summary>
        /// <param name="o">The object to encode.</param>
        /// <returns>A JSON string representing the object.</returns>
        public static string ToJsonString(this ObservableObject o)
        {
            return ToJsonString(o, Formatting.Indented);
        }

        /// <summary>
        /// Gets the JSON representation of this object.
        /// </summary>
        /// <param name="o">The object to encode.</param>
        /// <param name="formatting">JSON formatting parameters.</param>
        /// <returns>A JSON string representing the object.</returns>
        public static string ToJsonString(this ObservableObject o, Formatting formatting)
        {
            return JsonConvert.SerializeObject(o, formatting);
        }
    }
}
