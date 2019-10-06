using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Crude.BitmapIndex.Helpers;

namespace Crude.BitmapBenchmarks
{
    [InProcess]
    public class Log2Benchmark
    {
        private const int Count = 1000;
        private readonly List<ulong> _data = new List<ulong>(Count);


        [GlobalSetup]
        public void Setup()
        {
            for (var i = 0; i < Count; i++)
                _data.Add((ulong) new Random().Next(0, int.MaxValue));
        }


        [Benchmark]
        public List<int> Log2Custom() => _data.Select(x => MathHelper.Log2(x)).ToList();

        [Benchmark]
        public List<int> Log2Native() => _data.Select(x => (int) Math.Log(x, 2)).ToList();

        [Benchmark]
        public List<int> Log2Span() => _data.Select(x => MathHelper.Log2Span(x)).ToList();
    }
}