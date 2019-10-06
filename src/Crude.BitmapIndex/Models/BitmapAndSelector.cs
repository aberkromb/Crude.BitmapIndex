using System;

namespace Crude.BitmapIndex.Models
{
    internal readonly struct BitmapAndSelector<T>
    {
        public BitmapAndSelector(IBitmap bitMap, Predicate<T> selector)
        {
            Bitmap = bitMap;
            Selector = selector;
        }

        public IBitmap Bitmap { get; }
        public Predicate<T> Selector { get; }
    }
}