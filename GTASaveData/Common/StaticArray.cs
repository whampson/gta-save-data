using System.Collections.Specialized;

namespace GTASaveData.Common
{
    public class StaticArray<T> : ArrayBase<T>
        where T : new()
    {
        public override bool IsFixedSize => true;
        public StaticArray(int count)
            : base(count)
        { }
    }
}
