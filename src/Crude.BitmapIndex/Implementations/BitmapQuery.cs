using System;
using Crude.BitmapIndex.Helpers;

namespace Crude.BitmapIndex.Implementations
{
    public struct NonInitializedBitmapQuery<T>
    {
        private readonly IBitmapIndex<T> _bitMapIndex;

        internal NonInitializedBitmapQuery(IBitmapIndex<T> index) => _bitMapIndex = index;

        public BitmapQuery<T> Where(string key) => 
            new BitmapQuery<T>(_bitMapIndex, key);
    }

    public struct BitmapQuery<T>
    {
        private readonly IBitmapIndex<T> _bitMapIndex;

        private readonly IBitmap _queryResultBitMap;

        public BitmapQuery(IBitmapIndex<T> bitMapIndex, string key)
        {
            _bitMapIndex = bitMapIndex;
            _queryResultBitMap =  (IBitmap) _bitMapIndex[key].Clone();
        }

        public BitmapQuery<T> And(string key)
        {
            _queryResultBitMap.And(_bitMapIndex[key]);
            return this;
        }


        public BitmapQuery<T> AndNot(string key)
        {
            _queryResultBitMap.AndNot(_bitMapIndex[key]);
            return this;
        }


        public BitmapQuery<T> Or(string key)
        {
            _queryResultBitMap.Or(_bitMapIndex[key]);
            return this;
        }


        public BitmapQuery<T> Xor(string key)
        {
            _queryResultBitMap.Xor(_bitMapIndex[key]);
            return this;
        }


        public BitmapQuery<T> Not()
        {
            _queryResultBitMap.Not();
            return this;
        }

        public bool Any(string key) => _queryResultBitMap.Any(_bitMapIndex[key]);

        public bool All(string key) => _queryResultBitMap.All(_bitMapIndex[key]);

        public bool Contains(int i) => _queryResultBitMap.Contains(i);

        public T[] Execute()
        {
            var bitsCount = 0;

            for (var i = 0; i < _queryResultBitMap.GetArray.Length; i++)
                bitsCount += MathHelper.BitsCount((ulong) _queryResultBitMap.GetArray[i]);

            if (bitsCount <= 0)
                return Array.Empty<T>();

            var result = new T[bitsCount];
            var resultIndex = 0;

            for (var i = 0; i < _bitMapIndex.Count; i++)
            {
                if (_queryResultBitMap.Get(i))
                {
                    result[resultIndex] = _bitMapIndex[i];
                    resultIndex++;
                }
            }

            return result;
        }
    }
}