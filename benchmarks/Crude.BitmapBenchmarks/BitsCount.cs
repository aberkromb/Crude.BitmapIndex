using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace Crude.BitmapBenchmarks
{
    public class BitsCount
    {
        private List<int> _longs;

        [GlobalSetup]
        public void Setup()
        {
            const int count = 1000;
            _longs = Enumerable.Range(1, count).ToList();
        }

        [Benchmark]
        public void MathHelper()
        {
            foreach (var l in _longs) Crude.BitmapIndex.Helpers.MathHelper.BitsCount((ulong) l);
        }

        [Benchmark]
        public void NewImpl()
        {
            foreach (var l in _longs) CountBits64(l);
        }

        [Benchmark]
        public void PopCountSIMD()
        {
            foreach (var l in _longs) Popcnt.X64.PopCount((ulong) l);
        }

        const ulong MaskMult = 0x0101010101010101;
        const ulong Mask1H = (~0UL / 3) << 1;
        const ulong Mask2L = ~0UL / 5;
        const ulong Mask4L = ~0UL / 17;

        private static int CountBits64(long c)
        {
            var v = (ulong) c;
            v -= (Mask1H & v) >> 1;
            v = (v & Mask2L) + ((v >> 2) & Mask2L);
            v += v >> 4;
            v &= Mask4L;
            return (int) ((v * MaskMult) >> 56);
        }
    }
}