using System;
using System.Collections.Generic;
using System.Linq;

namespace Crude.BitmapIndex.Implementations.BitmapIndexes
{
    public class DefaultBitmapIndex<T> : IBitmapIndex<T>
    {
        private readonly Dictionary<string, IBitmap> _bitMaps;
        private readonly List<T> _data;
        private readonly Func<int, IBitmap> _bitMapFactory;

        public DefaultBitmapIndex(
            Dictionary<string, Predicate<T>> keys,
            IEnumerable<T> data,
            Func<int, IBitmap> bitMapFactory)
        {
            _bitMaps = new Dictionary<string, IBitmap>(keys.Count, StringComparer.Ordinal);
            _data = data.ToList();
            _bitMapFactory = bitMapFactory;

            foreach (var (key, predicate) in keys)
                _bitMaps[key] = FillBitmap(_data, predicate);
        }


        public IBitmap this[string key] => _bitMaps[key];

        public T this[int i] => _data[i];

        public int DataCount => _data.Count;

        public UnitializedBitmapQuery<T> NewQuery => new UnitializedBitmapQuery<T>(this);

        public void AddData(T item)
        {
            throw new NotImplementedException();
        }

        public void DeleteData(T item)
        {
            throw new NotImplementedException();
        }

        private IBitmap FillBitmap(IReadOnlyList<T> data, Predicate<T> predicate)
        {
            var bitmap = _bitMapFactory(_data.Count);
            
            for (var i = 0; i < data.Count; i++)
            {
                var d = data[i];
                bitmap.Set(i, predicate(d));
            }

            return bitmap;
        }
    }
}