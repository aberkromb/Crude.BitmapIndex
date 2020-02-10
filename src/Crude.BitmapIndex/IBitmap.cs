using System;
using System.Collections.Generic;

namespace Crude.BitmapIndex
{
    /// <summary>
    ///     Define generalized bitmap methods
    /// </summary>
    public interface IBitmap : ICloneable
    {
        int BitsCount { get; }
        int Length { get; }
        bool Contains(int index);
        IEnumerable<int> Enumerate();
        bool this[int index] { get; set; }
        bool Get(int index);
        void Set(int index, bool value);
        long GetContainer(int index);
        void SetAll(bool value);
        bool Any(IBitmap other);
        bool All(IBitmap other);
        IBitmap AndNot(IBitmap other);
        IBitmap And(IBitmap other);
        IBitmap Or(IBitmap other);
        IBitmap Xor(IBitmap other);
        IBitmap Not();
        IBitmap From(IBitmap other);
        IBitmap NotFrom(IBitmap other);

        ReadOnlySpan<long> AsSpan();
    }
}