using System;

namespace GTASaveData
{
    /// <summary>
    /// Specifies the size of a <see cref="SaveDataObject"/> in bytes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SizeAttribute : Attribute
    {
        public int Size { get; }

        public SizeAttribute(int size)
        {
            Size = size;
        }
    }
}
