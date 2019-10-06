using System;
using System.Collections.Generic;

namespace Crude.BitmapIndex
{
    /// <summary>
    ///     Define generalized bitmap methods
    /// </summary>
    public interface IBitmap : IComparable<IBitmap>, ICloneable
    {
        long[] GetArray { get; }
        int Count { get; }
        int Length { get; }
        bool this[int index] { get; set; }
        bool Contains(int index);
        IEnumerable<int> Enumerate();
        bool Get(int index);
        void Set(int index, bool value);
        void SetAll(bool value);
        bool Any(IBitmap value);
        bool All(IBitmap value);
        IBitmap AndNot(IBitmap value);
        IBitmap And(IBitmap value);
        IBitmap Or(IBitmap value);
        IBitmap Xor(IBitmap value);
        IBitmap Not();
        IBitmap From(IBitmap value);
        IBitmap NotFrom(IBitmap value);
        void CopyTo(Array array, int index);
    }
}