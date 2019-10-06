using System;
using System.Collections.Generic;
using System.Linq;
using Crude.BitmapIndex.Models;

namespace Crude.BitmapIndex
{
    public class BitmapIndex<T>
    {
        private readonly Dictionary<string, BitmapAndSelector<T>> _bitMaps;
        private readonly List<T> _data;


        public BitmapIndex(Dictionary<string, Func<T, bool>> keys,
            IEnumerable<T> data,
            Func<int, IBitmap> bitMapFactory)
        {
            _bitMaps = new Dictionary<string, BitmapAndSelector<T>>(keys.Count);
            _data = data.ToList();

            foreach (var (key, selector) in keys)
                _bitMaps[key] = new BitmapAndSelector<T>(bitMapFactory(_data.Count), selector);

            FillIndex(_data);
        }


        public IBitmap this[string key] => _bitMaps[key].Bitmap;

        public T this[int i] => _data[i];

        public int Count => _data.Count;


        private void FillIndex(List<T> data)
        {
            foreach (var (_, bitMapAndSelector) in _bitMaps)
                for (var i = 0; i < data.Count; i++)
                {
                    var d = data[i];
                    bitMapAndSelector.Bitmap.Set(i, bitMapAndSelector.Selector(d));
                }
        }
        
        public BitmapQuery<T> NewQuery => new BitmapQuery<T>(this);
    }
}