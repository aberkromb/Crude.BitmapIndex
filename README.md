# Crude.BitmapIndex
.NET bitmap index in memory implementation

# Example
```cs
_bitmapIndex = new BitmapBuilder<Data>()
                // keys and predicates for bitmap creation  
                .IndexFor("IsPersistent", data => data.Persistent)
                .IndexFor("ServerIsFrederik", data => data.Server.Equals("Frederik", StringComparison.Ordinal))
                .IndexFor("ServerIsVickie", data => data.Server.Equals("Vickie", StringComparison.Ordinal))
                .IndexFor("ApplicationIsTrantow", data => data.Application.Equals("Trantow", StringComparison.Ordinal))
                
                // constructor func for custom IBitmap implementations, default is BitmapDefault 
                .WithBitMap(count => new BitmapDefault(count))
                
                // your IEnumerable<T> 
                .ForData(_data) 
                .Build();
```

# Benchmarks
```
BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-7500U CPU 2.70GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=3.0.100-preview9-014004
  [Host]     : .NET Core 3.0.0-preview9-19423-09 (CoreCLR 4.700.19.42102, CoreFX 4.700.19.42104), 64bit RyuJIT
  DefaultJob : .NET Core 3.0.0-preview9-19423-09 (CoreCLR 4.700.19.42102, CoreFX 4.700.19.42104), 64bit RyuJIT


|        Method |        Mean |      Error |     StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |------------:|-----------:|-----------:|-------:|------:|------:|----------:|
|       Iterate | 5,891.07 us | 72.3925 us | 67.7160 us |      - |     - |     - |      32 B |
| DefaultBitMap |    18.09 us |  0.1953 us |  0.1631 us | 5.9814 |     - |     - |   12560 B |
|    Avx2BitMap |    11.82 us |  0.0801 us |  0.0710 us | 5.9814 |     - |     - |   12560 B |

```

# TODO
- [ ] Add an item and update index
- [ ] Delete an item and update index
