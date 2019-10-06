using System;
using System.Runtime.CompilerServices;

namespace Crude.BitmapIndex.Helpers
{
    public static class MathHelper
    {
        private static int[] Tab64 =
        {
            0, 58, 1, 59, 47, 53, 2, 60, 39, 48, 27, 54, 33, 42, 3, 61,
            51, 37, 40, 49, 18, 28, 20, 55, 30, 34, 11, 43, 14, 22, 4, 62,
            57, 46, 52, 38, 26, 32, 41, 50, 36, 17, 19, 29, 10, 13, 21, 56,
            45, 25, 31, 35, 16, 9, 12, 44, 24, 15, 8, 23, 7, 6, 5, 63
        };


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2(ulong value)
        {
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            value |= value >> 32;
            return Tab64[(value * 0x03f6eaf2cd271461) >> 58];
        }


        private static ReadOnlySpan<int> Tab64AsSpan => new ReadOnlySpan<int>(Tab64);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2Span(ulong value)
        {
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            value |= value >> 32;
            var i = (value * 0x03f6eaf2cd271461) >> 58;
            return Unsafe.Add(ref Unsafe.AsRef(Tab64AsSpan.GetPinnableReference()), (int) i);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BitsCount(ulong i)
        {
            i = i - ((i >> 1) & 0x5555555555555555UL);
            i = (i & 0x3333333333333333UL) + ((i >> 2) & 0x3333333333333333UL);
            return (int) (unchecked(((i + (i >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
        }
    }
}