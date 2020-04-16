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
            return t.Implements(typeof(INotifyPropertyChanged));
        }

        /// <summary>
        /// Gets a value indicating whether this type implements an interface.
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <param name="iface">the interface type to check for implementation</param>
        /// <returns>True if the type implements the interface, false otherwise.</returns>
        public static bool Implements(this Type t, Type iface)
        {
            return t.GetInterface(iface.Name) != null;
        }
    }
}
