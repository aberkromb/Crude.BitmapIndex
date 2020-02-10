using System;
using BenchmarkDotNet.Running;

namespace Crude.BitmapBenchmarks
{
    static class Program
    {
        static void Main(string[] args) =>
            BenchmarkRunner.Run<IndexBuild>();
//            BenchmarkRunner.Run<BitmapIndex>();
//            BenchmarkRunner.Run<Log2Benchmark>();
//            BenchmarkRunner.Run<BitsCount>();
    }
}