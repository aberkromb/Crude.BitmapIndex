using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BitmapTests.Helpers;
using Crude.BitmapIndex.Implementations.Bitmap;
using Crude.BitmapIndex.Implementations.BitmapIndexes;
using Crude.BitmapIndex.Implementations.Builder;

namespace Crude.BitmapBenchmarks
{
    [MemoryDiagnoser]
    public class IndexBuild
    {
        private List<MessageData> _data;

        [GlobalSetup]
        public void Setup()
        {
            _data = Generator.CreateRandomData(100_000);
        }

        [Benchmark]
        public DefaultBitmapIndex<MessageData> BuildIndex()
        {
            var _defaultBitMapIndex = new BitmapIndexBuilder<MessageData>()
                .IndexFor("IsPersistent", msg => msg.Persistent)
                .IndexFor("ServerIsFrederik", msg => msg.Server.Equals("Frederik", StringComparison.Ordinal))
                .IndexFor("ServerIsVickie", msg => msg.Server.Equals("Vickie", StringComparison.Ordinal))
                .IndexFor("ApplicationIsTrantow", msg => msg.Application.Equals("Trantow", StringComparison.Ordinal))
                .WithBitMap(i => new BitmapDefault(i))
                .ForData(_data)
                .Build();
            
            return _defaultBitMapIndex;
        }
    }
}