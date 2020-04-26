using System;

namespace GTASaveData.Extensions
{
    public static class TypeExtensions
    {
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
