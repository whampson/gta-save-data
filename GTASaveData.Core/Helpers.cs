using GTASaveData.Types;
using System.Linq;

namespace GTASaveData
{
    public static class Helpers
    {
        public static Array<T> CreateArray<T>(int count) where T : new()
        {
            return Enumerable.Range(0, count).Select(x => new T()).ToArray();
        }
    }
}
