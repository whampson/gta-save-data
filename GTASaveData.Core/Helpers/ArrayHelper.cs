using System.Linq;

namespace GTASaveData
{
    public static class ArrayHelper
    {
        /// <summary>
        /// Creates a new <see cref="Array{T}"/> with the specified number of elements.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="count">The initial number of elements.</param>
        /// <returns>The new <see cref="Array{T}"/> with <paramref name="count"/> elements.</returns>
        public static Array<T> CreateArray<T>(int count) where T : new()
        {
            return Enumerable.Range(0, count).Select(x => new T()).ToArray();
        }

        public static Array<T> DeepClone<T>(Array<T> array)
        {
            Array<T> newArray = new Array<T>();
            foreach (T item in array)
            {
                if (item is IDeepClonable<T>)
                {
                    newArray.Add((item as IDeepClonable<T>).DeepClone());
                }
                else
                {
                    newArray.Add(item);
                }
            }

            return newArray;
        }
    }
}
