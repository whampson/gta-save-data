using System;
using System.ComponentModel;

namespace GTASaveData.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsObservable(this Type t)
        {
            if (t == null)
            {
                return false;
            }

            return t.GetInterface(nameof(INotifyPropertyChanged)) != null;
        }
    }
}
