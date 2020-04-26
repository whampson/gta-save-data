using System;

namespace GTASaveData
{
    // TODO: consider removing

    /// <summary>
    /// Specifies the size of an <see cref="ISerializable"/> type in bytes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class SizeAttribute : Attribute
    {
        public int Size { get; }

        public SizeAttribute(int size)
        {
            Size = size;
        }
    }
}
