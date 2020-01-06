using System.Text;

namespace GTASaveData.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetAsciiBytes(this string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        public static byte[] GetUnicodeBytes(this string s)
        {
            return Encoding.Unicode.GetBytes(s);
        }
    }
}
