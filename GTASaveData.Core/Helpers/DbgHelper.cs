using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GTASaveData.Helpers
{
    public static class DbgHelper
    {
        public static void Print(string format, [CallerMemberName] string caller = "", params object[] args)
        {
            Debug.WriteLine($"{caller}(): {string.Format(format, args)}");
        }
    }
}
