using System.Text;

namespace GTASaveData.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetAsciiBytes(this string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }
    }
}
