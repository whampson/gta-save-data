namespace GTASaveData.Extensions
{
    public static class ByteArrayExtensions
    {
        public static int FindFirst(this byte[] b, byte[] seq, int start = 0)
        {
            int searchLen = b.Length - seq.Length + 1;

            if (b == null || seq == null ||
                b.Length == 0 || seq.Length == 0 ||
                b.Length < seq.Length || start < 0 ||
                start >= searchLen)
            {
                return -1;
            }

            int index = -1;
            for (int i = start; i < searchLen; i++)
            {
                if (b[i] == seq[0])
                {
                    // First byte match, kick off lookahead routine
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
