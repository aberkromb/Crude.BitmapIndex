using System;
using Crude.BitmapIndex.Implementations;

namespace Crude.BitmapIndex
{
    public interface IBitmapIndex<T>
    {
        IBitmap this[string key] { get; }
        T this[int i] { get; }
        int DataCount { get; }
        UnitializedBitmapQuery<T> NewQuery { get; }
        void AddData(T item);
        void DeleteData(T item);
    }
}