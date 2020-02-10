using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Crude.BitmapIndex.Implementations.BitmapIndexes
{
    public class ConcurrentBitmapIndex<T> : IBitmapIndex<T>
    {
        private readonly Dictionary<string, Predicate<T>> _predicates;
        private readonly ConcurrentDictionary<string, IBitmap> _bitMaps;
        private readonly List<T> _data;
        private readonly Func<int, IBitmap> _bitMapFactory;

        public ConcurrentBitmapIndex(
            Dictionary<string, Predicate<T>> predicates,
            IEnumerable<T> data,
            Func<int, IBitmap> bitMapFactory)
        {
            _bitMaps = new ConcurrentDictionary<string, IBitmap>(
                Environment.ProcessorCount,
                predicates.Count,StringComparer.Ordinal);
            
            _data = data.ToList();
            _bitMapFactory = bitMapFactory;
            _predicates = predicates;

            FillBitmaps(predicates);
        }


        public IBitmap this[string key] => _bitMaps[key];

        public T this[int i] => _data[i];

        public int DataCount => _data.Count;

        public UnitializedBitmapQuery<T> NewQuery => new UnitializedBitmapQuery<T>(this);

        public void AddData(T item)
        {
            lock (_data)
            {
                _data.Add(item);
                FillBitmaps(_predicates);
            }
        }

        public void DeleteData(T item)
        {
            throw new NotImplementedException();
        }

        private void FillBitmaps(Dictionary<string, Predicate<T>> keys)
        {
            foreach (var (key, predicate) in keys)
                _bitMaps[key] = FillBitmap(predicate);
        }

        private IBitmap FillBitmap(Predicate<T> predicate)
        {
            var bitmap = _bitMapFactory(_data.Count);

            for (var i = 0; i < _data.Count; i++)
            {
                var d = _data[i];
                bitmap.Set(i, predicate(d));
            }

            return bitmap;
        }
    }
}