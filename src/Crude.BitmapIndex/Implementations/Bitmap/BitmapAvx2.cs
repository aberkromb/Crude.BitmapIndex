using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static Crude.BitmapIndex.Helpers.BitmapHelper;

namespace Crude.BitmapIndex.Implementations.Bitmap
{
    /// <summary>
    ///     Avx 2 bitmap implementation
    /// </summary>
    internal sealed class BitmapAvx2 : IBitmap
    {
        public long[] GetArray => _mapArray;
        public int Count { get; }
        public int Length => _mapLength;

        private readonly long[] _mapArray;
        private readonly int _mapLength;

        private const int VecSize = 4;

        private int _bitsCount = -1;

        public BitmapAvx2(int length) : this(length, false) => _bitsCount = 0;

        public BitmapAvx2(int length, bool defaultValue)
        {
            var alignedLength = GetArrayLength(length) + CalculatePadding(length);

            _mapArray = new long[alignedLength];
            _mapLength = length;
            if (defaultValue)
                for (var i = 0; i < _mapArray.Length; i++)
                    _mapArray[i] = -1L;
            _bitsCount = defaultValue ? length : 0;
        }

        public BitmapAvx2(long[] values)
        {
            _mapArray = new long[values.Length + CalculatePadding(values.Length)];
            Array.Copy(values, 0, _mapArray, 0, values.Length);
            _mapLength = values.Length * BitsPerLong;
        }

        public BitmapAvx2(IBitmap bits)
        {
            var alignedLength = GetArrayLength(bits.Length) + CalculatePadding(bits.Length);
            _mapArray = new long[alignedLength];
            Array.Copy(bits.GetArray, 0, _mapArray, 0, alignedLength);
            _mapLength = bits.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CalculatePadding(int initialLen) => 4 - initialLen & (4 - 1);

        public bool this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public bool Contains(int index) => Get(index);

        public IEnumerable<int> Enumerate()
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Get(int index)
        {
            index = _mapLength - index;
            var (arrIndex, bit) = Div64Remainder(index);
            return (_mapArray[arrIndex] & (1L << bit)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        private const int VectorsEqualsMask = -1;

        public unsafe bool Any(IBitmap value)
        {
            var valueArray = value.GetArray;

            fixed (long* v = &valueArray[0])
            fixed (long* l = &_mapArray[0])
            {
                for (var i = 0; i <= _mapArray.Length; i += VecSize)
                {
                    var left = Avx.LoadVector256(l + i);
                    var right = Avx.LoadVector256(v + i);
                    var equalVector = Avx2.CompareEqual(left, right);
                    if (Avx2.MoveMask(equalVector.AsByte()) != VectorsEqualsMask)
                        return true;
                }
            }

            return false;
        }

        public unsafe bool All(IBitmap value)
        {
            var valueArray = value.GetArray;

            fixed (long* v = &valueArray[0])
            fixed (long* l = &_mapArray[0])
            {
                for (var i = 0; i < _mapArray.Length; i += VecSize)
                {
                    var left = Avx.LoadVector256(l + i);
                    var right = Avx.LoadVector256(v + i);
                    var equalVector = Avx2.CompareEqual(left, right);

                    if (Avx2.MoveMask(equalVector.AsByte()) != VectorsEqualsMask)
                        return false;
                }
            }

            return true;
        }

        public unsafe IBitmap AndNot(IBitmap value)
        {
            var valueArray = value.GetArray;

            fixed (long* v = &valueArray[0])
            fixed (long* l = &_mapArray[0])
            {
                for (var i = 0; i <= _mapArray.Length; i += VecSize)
                {
                    var left = Avx.LoadVector256(l + i);
                    var right = Avx.LoadVector256(v + i);
                    var resultVector = Avx2.AndNot(left, right);
                    Avx.Store(l + i, resultVector);
                }
            }

            return this;
        }

        public unsafe IBitmap And(IBitmap value)
        {
            var valueArray = value.GetArray;

            fixed (long* v = &valueArray[0])
            fixed (long* l = &_mapArray[0])
            {
                for (var i = 0; i <= _mapArray.Length; i += VecSize)
                {
                    var left = Avx.LoadVector256(l + i);
                    var right = Avx.LoadVector256(v + i);
                    var resultVector = Avx2.And(left, right);

                    Avx.Store(l + i, resultVector);
                }
            }

            return this;
        }

        public unsafe IBitmap Or(IBitmap value)
        {
            var valueArray = value.GetArray;

            fixed (long* v = &valueArray[0])
            fixed (long* l = &_mapArray[0])
            {
                for (var i = 0; i <= _mapArray.Length; i += VecSize)
                {
                    var left = Avx.LoadVector256(l + i);
                    var right = Avx.LoadVector256(v + i);
                    var resultVector = Avx2.Or(left, right);

                    Avx.Store(l + i, resultVector);
                }
            }

            return this;
        }

        public unsafe IBitmap Xor(IBitmap value)
        {
            var valueArray = value.GetArray;

            fixed (long* v = &valueArray[0])
            fixed (long* l = &_mapArray[0])
            {
                for (var i = 0; i <= _mapArray.Length; i += VecSize)
                {
                    var left = Avx.LoadVector256(l + i);
                    var right = Avx.LoadVector256(v + i);
                    var resultVector = Avx2.Xor(left, right);

                    Avx.Store(l + i, resultVector);
                }
            }

            return this;
        }

        //TODO benchmark it to default
        public unsafe IBitmap Not()
        {
            fixed (long* l = &_mapArray[0])
            {
                for (var i = 0; i <= _mapArray.Length; i += VecSize)
                {
                    var vector = Avx.LoadVector256(l + i);
                    var resultVector = Avx2.Xor(vector, Avx2.CompareEqual(vector, vector));

                    Avx.Store(l + i, resultVector);
                }
            }

            return this;
        }

        public IBitmap From(IBitmap value)
        {
            Array.Copy(value.GetArray, _mapArray, 0);
            return this;
        }

        public IBitmap NotFrom(IBitmap value)
        {
            for (var i = 0; i < _mapArray.Length; i++)
                _mapArray[i] = ~value.GetArray[i];
            _bitsCount = -1;
            return this;
        }

        public void CopyTo(Array array, int index) => Array.Copy(_mapArray, array, index);

        public object Clone() => new BitmapAvx2(this);

        public int CompareTo(IBitmap other) => Count.CompareTo(other.Count);
    }
}