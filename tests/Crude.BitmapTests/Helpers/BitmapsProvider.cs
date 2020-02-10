using System;
using System.Collections.Generic;
using Crude.BitmapIndex.Implementations.Bitmap;

namespace BitmapTests.Helpers
{
    public class BitmapsProvider
    {
        public static IEnumerable<object[]> TestBitMaps
        {
            get
            {
                yield return new object [] { (Func<int, BitmapDefault>)((count) => new BitmapDefault(count)) };
                yield return new object [] { (Func<int, BitmapAvx2>)((count) => new BitmapAvx2(count)) };
                yield return new object [] { (Func<int, BitmapDefaultEx>)((count) => new BitmapDefaultEx(count)) };
            }
        }
    }
}