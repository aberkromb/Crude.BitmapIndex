using System.Runtime.CompilerServices;

namespace Crude.BitmapIndex.Helpers
{
    internal static class BitmapHelper
    {
        internal const int BitsPerLong = 64;
        private const int BitShiftPerLong = 6;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetArrayLength(int n) =>
            (int) ((uint) (n - 1 + (1L << BitShiftPerLong)) >> BitShiftPerLong);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static (int quotient, int remainder) Div64Remainder(int number)
        {
            var quotient = (uint) number / 64;
            var remainder = number & (64 - 1);
            return ((int) quotient, remainder);
        }
    }
}