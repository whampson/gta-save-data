using System.Collections.Generic;

namespace GTASaveData
{
    /// <summary>
    /// A variable-length array.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    public class DynamicArray<T> : Array<T>
        where T : new()
    {
        public override bool IsFixedSize => false;

        public DynamicArray()
            : base(0)
        { }

        public DynamicArray(int initialCount)
            : base(initialCount)
        { }

        public DynamicArray(IEnumerable<T> items)
            : base(items)
        { }

        public DynamicArray(List<T> items)
            : base(items)
        { }

        public static implicit operator DynamicArray<T>(T[] array)
        {
            return new DynamicArray<T>(array);
        }

        public static implicit operator DynamicArray<T>(List<T> array)
        {
            return new DynamicArray<T>(array);
        }
    }
}
