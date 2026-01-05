using System;

namespace GTASaveData.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Queries whether this type is derived from a specific interface type.
        /// </summary>
        /// <param name="t">The type to check.</param>
        /// <param name="iface">the interface type to check for implementation</param>
        /// <returns>True if this type implements the interface, false otherwise.</returns>
        public static bool Implements(this Type t, Type iface)
        {
            return t.GetInterface(iface.Name) != null;
        }
    }
}
