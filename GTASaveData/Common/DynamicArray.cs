using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace GTASaveData.Common
{
    public class DynamicArray<T> : ArrayBase<T>
        where T : new()
    {
        public override bool IsFixedSize => false;

        public DynamicArray()
            : base(0)
        { }

        public DynamicArray(int initialCount)
            : base(initialCount)
        { }
    }
}
