using System;
using System.ComponentModel;

namespace GTASaveData.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets a value indicating whether this type implements <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <returns>True if the type is observable, false otherwise.</returns>
        public static bool IsObservable(this Type t)
        {
            return t.GetInterface(nameof(INotifyPropertyChanged)) != null;
        }
    }
}
