using System;

namespace GTASaveData.Types
{
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
