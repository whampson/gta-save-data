using System;
using System.Collections.Generic;
using System.Linq;

namespace GTASaveData.Extensions
{
    public static class ByteArrayExtensions
    {
        static readonly int[] Empty = new int[0];

        public static int[] FindAll(this byte[] src, byte[] seq)
        {
            if (IsFindEmpty(src, seq))
                return Empty;

            var list = new List<int>();
            for (int i = 0; i < src.Length; i++)
                if (IsFindMatch(src, i, seq))
                    list.Add(i);

            return list.Count == 0 ? Empty : list.ToArray();
        }

        public static int FindFirst(this byte[] src, byte[] seq)
        {
            if (IsFindEmpty(src, seq))
                return -1;

            for (int i = 0; i < src.Length; i++)
                if (IsFindMatch(src, i, seq))
                    return i;

            return -1;
        }

        private static bool IsFindEmpty(byte[] src, byte[] seq)
        {
            return src == null || seq == null ||
                src.Length == 0 || seq.Length == 0 ||
                src.Length < seq.Length;
        }

        private static bool IsFindMatch(byte[] src, int pos, byte[] seq)
        {
            if ((src.Length - pos) < seq.Length)
                return false;

            for (int i = 0; i < seq.Length; i++)
                if (src[pos + i] != seq[i])
                    return false;

            return true;
        }

        // look ma, no braces!
    }
}
