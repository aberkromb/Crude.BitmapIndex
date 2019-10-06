using System;
using System.Collections.Generic;
using Crude.BitmapIndex.Implementations.Bitmap;

namespace BitmapTests
{
    public class BitmapsProvider
    {
        public static IEnumerable<object[]> TestBitMaps
        {
            get
            {
                yield return new object [] { (Func<int, BitmapDefault>)((count) => new BitmapDefault(count)) };
                yield return new object [] { (Func<int, BitmapAvx2>)((count) => new BitmapAvx2(count)) };
            }
        }
    }
}