using System;
using Crude.BitmapIndex.Implementations;

namespace Crude.BitmapIndex
{
    public interface IBitmapIndex<T>
    {
        IBitmap this[string key] { get; }
        T this[int i] { get; }
        int Count { get; }
        NonInitializedBitmapQuery<T> NewQuery { get; }
        void AddKey(string keyName, Predicate<T> bitmapPredicate);
    }
}