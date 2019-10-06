using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Crude.BitmapIndex.Helpers;
using static Crude.BitmapIndex.Helpers.BitmapHelper;

[assembly: InternalsVisibleTo("BitmapTests")]
[assembly: InternalsVisibleTo("Crude.BitmapIndexBuilder")]

namespace Crude.BitmapIndex.Implementations.Bitmap
{
    /// <summary>
    ///     Default bitmap implementation
    /// </summary>
    internal sealed class BitmapDefault : IBitmap
    {
        private int _bitsCount = -1;

        private readonly long[] _mapArray;
        private readonly int _mapLength;

        public BitmapDefault(int length) : this(length, false) => _bitsCount = 0;

        public BitmapDefault(int length, bool defaultValue)
        {
            _mapArray = new long[GetArrayLength(length)];
            _mapLength = length;
            if (defaultValue)
                for (var i = 0; i < _mapArray.Length; i++)
                    _mapArray[i] = -1L;
            _bitsCount = defaultValue ? length : 0;
        }

        public BitmapDefault(long[] values)
        {
            _mapArray = new long[values.Length];
            Array.Copy(values, 0, _mapArray, 0, values.Length);
            _mapLength = values.Length * BitsPerLong;
        }

        public BitmapDefault(BitmapDefault bits)
        {
            var arrayLength = GetArrayLength(bits._mapLength);
            _mapArray = new long[arrayLength];
            Array.Copy(bits._mapArray, 0, _mapArray, 0, arrayLength);
            _mapLength = bits._mapLength;
        }


        public long[] GetArray => _mapArray;

        public int Count
        {
            get
            {
                if (_bitsCount < 0)
                    UpdateCountCache();
                return _bitsCount;
            }
        }

        public bool this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public int Length => _mapLength;

        public bool Contains(int index) => this[index];

        // TODO need fix
        public IEnumerable<int> Enumerate()
        {
            for (var word = 0; word < _mapArray.Length; word++)
            {
                var val = _mapArray[word];
                if (val != 0)
                    for (var bit = 0; bit < 64; bit++)
                        if ((val & (1 << bit)) != 0)
                            yield return _mapLength - ((word << 6) | bit);
            }
        }

        public bool Get(int index)
        {
            index = _mapLength - index;
            var (arrIndex, bit) = Div64Remainder(index);
            return (_mapArray[arrIndex] & (1L << bit)) != 0;
        }

        public void Set(int index, bool value)
        {
            index = _mapLength - index;
            var (arrIndex, bit) = Div64Remainder(index);
            var newValue = _mapArray[arrIndex];
            if (value)
                newValue |= 1L << bit;
            else
                newValue &= ~(1L << bit);
            _mapArray[arrIndex] = newValue;
            _bitsCount = -1;
        }

        public void SetAll(bool value)
        {
            var fillValue = value ? -1L : 0L;
            for (var i = 0; i < _mapArray.Length; i++)
                _mapArray[i] = fillValue;
            _bitsCount = value ? _mapLength : 0;
        }

        public bool Any(IBitmap value)
        {
            for (var i = 0; i < _mapArray.Length; i++)
                if ((_mapArray[i] & value.GetArray[i]) != 0)
                    return true;
            return false;
        }

        public bool All(IBitmap value)
        {
            for (var i = 0; i < _mapArray.Length; i++)
                if ((_mapArray[i] & value.GetArray[i]) != value.GetArray[i])
                    return false;
            return true;
        }

        public IBitmap AndNot(IBitmap value)
        {
            var count = _mapArray.Length;

            var i = 0;
            for (; i < count; i++)
                _mapArray[i] = _mapArray[i] & ~ value.GetArray[i];
            _bitsCount = -1;
            return this;
        }

        public IBitmap And(IBitmap value)
        {
            var count = _mapArray.Length;

            var i = 0;
            for (; i < count; i++)
                _mapArray[i] &= value.GetArray[i];
            _bitsCount = -1;
            return this;
        }

        public IBitmap Or(IBitmap value)
        {
            var count = _mapArray.Length;
            var i = 0;
            for (; i < count; i++)
                _mapArray[i] |= value.GetArray[i];
            _bitsCount = -1;
            return this;
        }

        public IBitmap Xor(IBitmap value)
        {
            for (var i = 0; i < _mapArray.Length; i++)
                _mapArray[i] ^= value.GetArray[i];
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

        public IBitmap From(IBitmap value)
        {
            for (var i = 0; i < _mapArray.Length; i++)
                _mapArray[i] = value.GetArray[i];
            _bitsCount = -1;
            return this;
        }

        public IBitmap NotFrom(IBitmap value)
        {
            var count = _mapArray.Length;
            var i = 0;
            for (; i < count; i++)
                _mapArray[i] = ~value.GetArray[i];
            _bitsCount = -1;
            return this;
        }

        public void CopyTo(Array array, int index)
        {
            if (array is long[] intArray)
                Array.Copy(_mapArray, 0, intArray, index, _mapArray.Length);
        }

        private int UpdateCountCache()
        {
            _bitsCount = 0;
            for (var i = 0; i < _mapArray.Length; i++)
                if (_mapArray[i] != 0)
                    _bitsCount += MathHelper.BitsCount((ulong) _mapArray[i]);

            return _bitsCount;
        }

        public object Clone() => new BitmapDefault(this);

        public int CompareTo(IBitmap other) => Count.CompareTo(other.Count);
    }
}