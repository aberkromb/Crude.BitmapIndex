using System;
using System.Collections.Generic;
using System.Linq;

namespace Crude.BitmapIndex.Implementations
{
    public class BitmapIndex<T>
    {
        private readonly Dictionary<string, IBitmap> _bitMaps;
        private readonly List<T> _data;
        private readonly Func<int, IBitmap> _bitMapFactory;

        public BitmapIndex(Dictionary<string, Predicate<T>> keys,
            IEnumerable<T> data,
            Func<int, IBitmap> bitMapFactory)
        {
            _bitMaps = new Dictionary<string, IBitmap>(keys.Count);
            _data = data.ToList();
            _bitMapFactory = bitMapFactory;

            foreach (var (key, predicate) in keys)
                _bitMaps[key] = FillBitmap(_data, _bitMapFactory(_data.Count), predicate);
        }


        public IBitmap this[string key] => _bitMaps[key];

        public T this[int i] => _data[i];

        public int Count => _data.Count;

        public BitmapQuery<T> NewQuery => new BitmapQuery<T>(this);

        public void AddKey(string keyName, Predicate<T> bitmapPredicate)
        {
            FillBitmap(_data, _bitMapFactory(_data.Count), bitmapPredicate);
        }

        private static IBitmap FillBitmap(List<T> data, IBitmap bitmap, Predicate<T> predicate)
        {
            for (var i = 0; i < data.Count; i++)
            {
                var d = data[i];
                bitmap.Set(i, predicate(d));
            }

            return bitmap;
        }
    }
}