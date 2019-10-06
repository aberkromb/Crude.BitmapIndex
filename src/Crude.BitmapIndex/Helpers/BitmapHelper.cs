using System.Runtime.CompilerServices;

namespace Crude.BitmapIndex.Helpers
{
    public static class BitmapHelper
    {
        public const int BitsPerLong = 64;
        public const int BitShiftPerInt64 = 6;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetArrayLength(int n) =>
            (int) ((uint) (n - 1 + (1L << BitShiftPerInt64)) >> BitShiftPerInt64);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int quotient, int remainder) Div64Remainder(int number)
        {
            var quotient = (uint) number / 64;
            var remainder = number & (64 - 1);
            return ((int) quotient, remainder);
        }
    }
}