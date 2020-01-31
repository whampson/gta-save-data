﻿using System.Collections.Generic;

namespace GTASaveData.Common
{
    /// <summary>
    /// A variable-length array.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    public class DynamicArray<T> : Array<T>
        where T : new()
    {
        public override bool IsFixedSize => false;

        public DynamicArray()
            : base(0)
        { }

        public DynamicArray(int initialCount)
            : base(initialCount)
        { }

        public DynamicArray(IEnumerable<T> items)
            : base(items)
        { }

        public DynamicArray(List<T> items)
            : base(items)
        { }
    }
}