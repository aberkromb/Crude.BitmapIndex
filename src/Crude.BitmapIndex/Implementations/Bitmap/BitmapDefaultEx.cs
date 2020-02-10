using System;
using System.Collections.Generic;
using Crude.BitmapIndex.Helpers;
using static Crude.BitmapIndex.Helpers.BitmapHelper;

namespace Crude.BitmapIndex.Implementations.Bitmap
{
    /// <summary>
    ///     experimental implementation
    /// </summary>
    internal sealed class BitmapDefaultEx : IBitmap
    {
        private readonly long[] _mapArray;
        private readonly int _mapLength;
        private int _bitsCount;

        public BitmapDefaultEx(int length)
        {
            _mapArray = new long[GetArrayLength(length)];
            _mapLength = length;
        }

        public BitmapDefaultEx(long[] values)
        {
            _mapArray = new long[values.Length];
            Array.Copy(values, 0, _mapArray, 0, values.Length);
            _mapLength = values.Length * BitsPerLong;
        }

        public int BitsCount
        {
            get
            {
                if (_bitsCount < 0)
                    UpdateCount();
                return _bitsCount;
            }
        }

        private int UpdateCount()
        {
            var bitsCount = 0;

            for (var i = 0; i < _mapArray.Length; i++)
                if (_mapArray[i] != 0)
                    bitsCount += MathHelper.BitsCount((ulong) _mapArray[i]);

            _bitsCount = bitsCount;

            return bitsCount;
        }


        public int Length => _mapLength;

        public bool this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public bool Get(int index)
        {
            var bitmapIndex = index / BitsPerLong;
            var bit = index % BitsPerLong;
            return (_mapArray[bitmapIndex] & (1L << bit)) != 0;
        }

        public void Set(int index, bool value)
        {
            var bitmapIndex = index / BitsPerLong;
            var bit = index % BitsPerLong;

            var newValue = _mapArray[bitmapIndex];

            if (value)
                newValue |= 1L << bit;
            else
                newValue &= ~(1L << bit);

            _mapArray[bitmapIndex] = newValue;

            _bitsCount = -1;
        }

        public bool Any(IBitmap other)
        {
            for (var i = 0; i < _mapArray.Length; i++)
                if ((_mapArray[i] & other.GetContainer(i)) != 0)
                    return true;
            return false;
        }

        public bool All(IBitmap other)
        {
            for (var i = 0; i < _mapArray.Length; i++)
                if ((_mapArray[i] & other.GetContainer(i)) != other.GetContainer(i))
                    return false;
            return true;
        }

        public IBitmap AndNot(IBitmap other)
        {
            var count = _mapArray.Length;

            var i = 0;
            for (; i < count; i++)
                _mapArray[i] = _mapArray[i] & ~ other.GetContainer(i);
            _bitsCount = -1;
            return this;
        }

        public IBitmap And(IBitmap other)
        {
            var count = _mapArray.Length;

            var i = 0;
            for (; i < count; i++)
                _mapArray[i] &= other.GetContainer(i);
            _bitsCount = -1;
            return this;
        }

        public IBitmap Or(IBitmap other)
        {
            var count = _mapArray.Length;
            var i = 0;
            for (; i < count; i++)
                _mapArray[i] |= other.GetContainer(i);
            _bitsCount = -1;
            return this;
        }

        public IBitmap Xor(IBitmap other)
        {
            for (var i = 0; i < _mapArray.Length; i++)
                _mapArray[i] ^= other.GetContainer(i);
            _bitsCount = -1;
            return this;
        }

        public IBitmap Not()
        {
            for (var i = 0; i < _mapArray.Length; i++)
                _mapArray[i] = ~_mapArray[i];
            _bitsCount = -1;
            return this;
        }

        public IBitmap From(IBitmap other)
        {
            for (var i = 0; i < _mapArray.Length; i++)
                _mapArray[i] = other.GetContainer(i);
            _bitsCount = -1;
            return this;
        }

        public IBitmap NotFrom(IBitmap other)
        {
            var count = _mapArray.Length;
            var i = 0;
            for (; i < count; i++)
                _mapArray[i] = ~other.GetContainer(i);
            _bitsCount = -1;
            return this;
        }


        public object Clone() => new BitmapDefaultEx(_mapArray);


        public bool Contains(int index) => this[index];


        public IEnumerable<int> Enumerate()
        {
            throw new NotImplementedException();
        }


        public long GetContainer(int index) =>
            _mapArray[index];

        public void SetAll(bool value)
        {
            throw new NotImplementedException();
        }


        public ReadOnlySpan<long> AsSpan()
        {
            throw new NotImplementedException();
        }
    }
}