using System;

namespace Crude.BitmapIndex.Models
{
    internal readonly struct BitmapAndSelector<T>
    {
        public BitmapAndSelector(IBitmap bitMap, Func<T, bool> selector)
        {
            Bitmap = bitMap;
            Selector = selector;
        }

        public IBitmap Bitmap { get; }
        public Func<T, bool> Selector { get; }
    }
}