using System;

namespace GTASaveData.Helpers
{
    public static class AddressHelper
    {
        /// <summary>
        /// Rounds an address up to the next multiple of 4.
        /// </summary>
        /// <param name="addr">The address to align.</param>
        /// <returns>The aligned address.</returns>
        public static int Align4(int addr)
        {
            if (addr < 0) throw new ArgumentOutOfRangeException(nameof(addr));

            const int WordSize = 4;
            return (addr + WordSize - 1) & ~(WordSize - 1);
        }
    }
}
