using System.Collections.Generic;

namespace GTASaveData.Common
{
    /// <summary>
    /// A fixed-length array.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    public class StaticArray<T> : Array<T>
        where T : new()
    {
        public override bool IsFixedSize => true;

        public StaticArray(int count)
            : base(count)
        { }

        public StaticArray(IEnumerable<T> items)
            : base(items)
        { }

        public StaticArray(List<T> items)
            : base(items)
        { }

        public static implicit operator StaticArray<T>(T[] array)
        {
            return new StaticArray<T>(array);
        }
    }
}
