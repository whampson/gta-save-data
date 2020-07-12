using System;
using System.Security.Cryptography;

namespace GTASaveData.Helpers
{
    public static class XboxHelper
    {
        public const int SignatureLength = 20;
        public const int KeyLength = 16;
        public const int SaveNameEncodingLength = 12;

        private static readonly byte[] CertificateKey =
        {
            0x5C, 0x07, 0x33, 0xAE, 0x04, 0x01, 0xF7, 0xE8,
            0xBA, 0x79, 0x93, 0xFD, 0xCD, 0x2F, 0x1F, 0xE0
        };

        public static byte[] CalculateGameSaveSignature(byte[] titleSigKey, byte[] data, int offset, int length)
        {
            byte[] authKey = new byte[KeyLength];
            using (HMACSHA1 hmac = new HMACSHA1(CertificateKey))
            {
                byte[] result = hmac.ComputeHash(titleSigKey);
                Array.Copy(result, authKey, KeyLength);
            }

            byte[] saveSig;
            using (HMACSHA1 hmac = new HMACSHA1(authKey))
            {
                saveSig = hmac.ComputeHash(data, offset, length);
            }

            return saveSig;
        }

        public static string EncodeGameSaveName(string name, int length)
        {
            ulong hash = 0;
            for (int i = 0; i < length; i++)
            {
                hash = (name[i] + (hash << 16)) % 0xFFFFFFFFFFC5ul;
            }

            char[] result = new char[SaveNameEncodingLength];
            int index = SaveNameEncodingLength - 1;
            int shift = 0;
            char value;
            do
            {
                value = (char) ((hash >> shift) & 0xF);
                value = (value > 9)
                    ? (char) (value + 55)
                    : (char) (value + 48);
                result[index] = value;
                shift += 4;
                index--;
            } while (shift <= 44);

            return new string(result);
        }
    }
}
