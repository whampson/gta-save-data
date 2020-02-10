using System.Collections.Generic;

namespace GTASaveData
{
    /// <summary>
    /// A fixed-length collection of objects with sequential and contiguous storage.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <remarks>
    /// This collection is WPF-ready. Changes to items in the collection and to the collection itself
    /// are observable by WPF UIs via the CollectionChanged and ItemStateChanged events.
    /// </remarks>
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
