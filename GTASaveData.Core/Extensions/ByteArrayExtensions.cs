using System;

namespace GTASaveData.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="byte"/> arrays.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Gets the index in the array of the first occurrence of a sequence.
        /// </summary>
        /// <param name="b">The array to search.</param>
        /// <param name="seq">The sequence to search for.</param>
        /// <param name="start">The starting offset in the array.</param>
        /// <returns>The index of the first occurrence of the sequence, or -1 if not found.</returns>
        public static int FindFirst(this byte[] b, byte[] seq, int start = 0)
        {
            if (b.Length < seq.Length)
            {
                throw new ArgumentException("The sequence length must be less than or equal to the length of the array.", nameof(seq));
            }

            if (seq == null)
            {
                throw new ArgumentNullException(nameof(seq));
            }

            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "The value must be a non-negative integer.");
            }

            int searchLen = b.Length - seq.Length + 1;
            if (start >= searchLen)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "The starting index must be smaller than the length of the sequence being searched for.");
            }

            if (b.Length == 0 || seq.Length == 0)
            {
                return -1;
            }

            int index = -1;
            for (int i = start; i < searchLen; i++)
            {
                if (b[i] == seq[0])
                {
                    // First byte match, lookahead for the rest
                    bool match = true;
                    for (int k = 1; k < b.Length - 1 && k < seq.Length; k++)
                    {
                        if (b[i + k] != seq[k])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        index = i;
                        break;
                    }
                }
            }

            return index;
        }
    }
}
