using System;

namespace GTASaveData.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Gets the index in the array of the first occurrence of a sequence.
        /// </summary>
        /// <param name="arr">The array to search.</param>
        /// <param name="seq">The sequence to search for.</param>
        /// <param name="start">The starting offset in the array.</param>
        /// <returns>The index of the first occurrence of the sequence, or -1 if not found.</returns>
        public static int FindFirst(this byte[] arr, byte[] seq, int start = 0)
        {
            if (arr.Length < seq.Length)
            {
                throw new ArgumentException(Strings.Error_Argument_SequenceTooBig, nameof(seq));
            }

            if (seq == null)
            {
                throw new ArgumentNullException(nameof(seq));
            }

            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start), Strings.Error_Argument_NoNegative);
            }

            int searchLen = arr.Length - seq.Length + 1;
            if (start >= searchLen)
            {
                throw new ArgumentOutOfRangeException(nameof(start), Strings.Error_Argument_SequenceIndexOutOfRange);
            }

            if (arr.Length == 0 || seq.Length == 0)
            {
                return -1;
            }

            int index = -1;
            for (int i = start; i < searchLen; i++)
            {
                if (arr[i] == seq[0])
                {
                    // First byte match, lookahead for the rest
                    bool match = true;
                    for (int k = 1; k < arr.Length - 1 && k < seq.Length; k++)
                    {
                        if (arr[i + k] != seq[k])
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
