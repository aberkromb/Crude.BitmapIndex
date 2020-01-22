using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BitmapTests.Helpers;
using Crude.BitmapIndex.Implementations;
using Crude.BitmapIndex.Implementations.Bitmap;
using Crude.BitmapIndex.Implementations.Builder;

namespace Crude.BitmapBenchmarks
{
    [MemoryDiagnoser]
    public class BitmapIndex
    {
        private readonly int _count = 100_000;
        private List<MessageData> _data;
        private BitmapIndex<MessageData> _defaultBitMapIndex;
        private BitmapIndex<MessageData> _avx2BitMapIndex;

        [GlobalSetup]
        public void Setup()
        {
            _data = Generator.CreateRandomData(_count);

            _defaultBitMapIndex = new BitmapIndexBuilder<MessageData>()
                .IndexFor("IsPersistent", msg => msg.Persistent)
                .IndexFor("ServerIsFrederik", msg => msg.Server.Equals("Frederik", StringComparison.Ordinal))
                .IndexFor("ServerIsVickie", msg => msg.Server.Equals("Vickie", StringComparison.Ordinal))
                .IndexFor("ApplicationIsTrantow", msg => msg.Application.Equals("Trantow", StringComparison.Ordinal))
                .WithBitMap(i => new BitmapDefault(i))
                .ForData(_data)
                .Build();

            _avx2BitMapIndex = new BitmapIndexBuilder<MessageData>()
                .IndexFor("IsPersistent", msg => msg.Persistent)
                .IndexFor("ServerIsFrederik", msg => msg.Server.Equals("Frederik", StringComparison.Ordinal))
                .IndexFor("ServerIsVickie", msg => msg.Server.Equals("Vickie", StringComparison.Ordinal))
                .IndexFor("ApplicationIsTrantow", msg => msg.Application.Equals("Trantow", StringComparison.Ordinal))
                .WithBitMap(i => new BitmapAvx2(i))
                .ForData(_data)
                .Build();
        }

        [Benchmark]
        public void Iterate()
        {
            var result = new List<MessageData>();

            foreach (var t in _data)
                if (t.Persistent
                    && t.Server.Equals("Vickie", StringComparison.Ordinal)
                    && t.Application.Equals("Trantow", StringComparison.Ordinal))
                {
                    result.Add(t);
                }
        }

        [Benchmark]
        public void DefaultBitMap()
        {
            var query = _defaultBitMapIndex.NewQuery
                .Where("IsPersistent")
                .And("ServerIsVickie")
                .And("ApplicationIsTrantow");

            var result = query.Execute();
        }


        [Benchmark]
        public void Avx2BitMap()
        {
            var query = _avx2BitMapIndex.NewQuery
                .Where("IsPersistent")
                .And("ServerIsVickie")
                .And("ApplicationIsTrantow");

            var result = query.Execute();
        }
    }
}