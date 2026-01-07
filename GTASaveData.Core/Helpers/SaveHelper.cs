namespace GTASaveData.Helpers
{
    public static class SaveHelper
    {
        /// <summary>
        /// The size in bytes of a data block header.
        /// </summary>
        public const int BlockHeaderSize = 8;

        /// <summary>
        /// Reads a block header from the buffer.
        /// </summary>
        /// <param name="buf">The buffer to read.</param>
        /// <param name="tag">The block tag extracted from the header.</param>
        /// <returns>The size in bytes of the block data to follow, as written in the header.</returns>
        public static int ReadBlockHeader(DataBuffer buf, out string tag)
        {
            tag = buf.ReadString(4);
            return buf.ReadInt32();
        }

        /// <summary>
        /// Writes a block header to the buffer.
        /// </summary>
        /// <param name="buf">The buffer to write.</param>
        /// <param name="tag">The 4-character block tag.</param>
        /// <param name="size">The size in bytes of the block data to follow.</param>
        public static void WriteBlockHeader(DataBuffer buf, string tag, int size)
        {
            buf.Write(tag, 4, zeroTerminate: true);
            buf.Write(size);
        }
    }
}
