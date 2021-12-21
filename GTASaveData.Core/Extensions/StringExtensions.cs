using System.Text;

namespace GTASaveData.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the ASCII encoding of this string.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>The string as byte array representing ASCII characters.</returns>
        public static byte[] GetAsciiBytes(this string s)
        {
            return Encoding.ASCII.GetBytes(s);
        }

        /// <summary>
        /// Gets the Unicode (UTF-16LE) encoding of this string.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>The string as byte array representing UTF-16 characters.</returns>
        public static byte[] GetUnicodeBytes(this string s)
        {
            return Encoding.Unicode.GetBytes(s);
        }

        /// <summary>
        /// Trims null terminators and any garbage that appears after.
        /// </summary>
        /// <param name="s">The string to trim.</param>
        /// <returns>The trimmed string.</returns>
        public static string TrimFromZero(this string s)
        {
            int index = s.IndexOf('\0');
            if (index < 0)
            {
                return s;
            }

            return s.Substring(0, index);
        }
    }
}
