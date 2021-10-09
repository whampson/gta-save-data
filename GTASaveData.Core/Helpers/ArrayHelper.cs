using System;
using System.Linq;
using GTASaveData.Interfaces;

namespace GTASaveData
{
    public static class ArrayHelper
    {
        /// <summary>
        /// Creates a new <see cref="ObservableArray{T}"/> with the specified number of elements.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="count">The initial number of elements.</param>
        /// <returns>The new <see cref="ObservableArray{T}"/> with <paramref name="count"/> elements.</returns>
        public static ObservableArray<T> CreateArray<T>(int count) where T : new()
        {
            return Enumerable.Range(0, count).Select(x => new T()).ToArray();
        }

        /// <summary>
        /// Creates a deep clone of an array.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="array">The array to clone.</param>
        /// <returns>A deep clone of the array.</returns>
        public static ObservableArray<T> DeepClone<T>(ObservableArray<T> array)
        {
            Type t = typeof(T);
            ObservableArray<T> newArray = new ObservableArray<T>();
            foreach (T item in array)
            {
                if (item is IDeepClonable<T>)
                {
                    newArray.Add((item as IDeepClonable<T>).DeepClone());
                }
                else if (t.IsValueType)
                {
                    newArray.Add(item);
                }
                else
                {
                    string msg = string.Format(Strings.Error_InvalidOperation_NotDeepClonable, t.Name);
                    throw new InvalidOperationException(msg);
                }
            }

            return newArray;
        }
    }
}
