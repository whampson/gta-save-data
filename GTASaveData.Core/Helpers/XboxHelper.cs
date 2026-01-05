using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace GTASaveData.Helpers
{
    public static class XboxHelper
    {
        public const int SignatureLength = 20;

        /// <summary>
        /// Generates a signature for a set of title data.
        /// </summary>
        /// <param name="titleKey">The 16-byte title key.</param>
        /// <param name="data">The data to sign.</param>
        /// <param name="offset">The starting offset in the data buffer.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The data signature.</returns>
        public static byte[] GenerateSignature(byte[] titleKey, byte[] data, int offset = 0, int length = -1)
        {
            if (length == -1)
            {
                length = data.Length;
            }

            using (HMACSHA1 hmac = new HMACSHA1(GenerateSignatureKey(titleKey)))
            {
                byte[] sig = hmac.ComputeHash(data, offset, length);

                Debug.Assert(sig.Length == SignatureLength);
                return sig;
            }
        }

        /// <summary>
        /// Generates a signing key.
        /// </summary>
        /// <param name="titleKey">The 16-byte title key.</param>
        /// <returns>A signing key.</returns>
        public static byte[] GenerateSignatureKey(byte[] titleKey)
        {
            const int KeyLength = 16;

            byte[] sigKey = new byte[KeyLength];
            using (HMACSHA1 hmac = new HMACSHA1(XboxKey))
            {
                byte[] result = hmac.ComputeHash(titleKey);
                Array.Copy(result, sigKey, KeyLength);
            }

            Debug.Assert(sigKey.Length == KeyLength);
            return sigKey;
        }

        /// <summary>
        /// Generates a 12-char hash of a string.
        /// </summary>
        /// <remarks>
        /// Used for save file names.
        /// </remarks>
        /// <param name="name">The string to hash.</param>
        /// <param name="length">The number of characters to include in the hashing process.</param>
        /// <returns></returns>
        public static string HashString(string name, int length = -1)
        {
            // I can't remember where I found this...

            const int EncodedNameLength = 12;

            if (length == -1)
            {
                length = name.Length;
            }

            ulong hash = 0;
            for (int i = 0; i < length; i++)
            {
                hash = (name[i] + (hash << 16)) % 0xFFFFFFFFFFC5ul;
            }

            char[] result = new char[EncodedNameLength];
            int index = result.Length - 1;
            int shift = 0;

            do
            {
                char value = (char) ((hash >> shift) & 0xF);
                value = (value > 9)
                    ? (char) (value + 55)
                    : (char) (value + 48);
                result[index] = value;
                shift += 4;
                index--;
            } while (shift <= 44);

            return new string(result);
        }

        public static readonly byte[] XboxKey =
{
            0x5C, 0x07, 0x33, 0xAE, 0x04, 0x01, 0xF7, 0xE8,
            0xBA, 0x79, 0x93, 0xFD, 0xCD, 0x2F, 0x1F, 0xE0
        };
    }
}
