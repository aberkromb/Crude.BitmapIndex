using System;
using BitmapTests.Helpers;
using Crude.BitmapIndex;
using FluentAssertions;
using Xunit;

namespace BitmapTests.BitmapTests
{
    public class BitmapTests
    {
        [Theory]
        [MemberData(nameof(BitmapsProvider.TestBitMaps), MemberType = typeof(BitmapsProvider))]
        public void AndShouldReturnTrueIfBitsSame(Func<int, IBitmap> bitMapCtor)
        {
            // arrange
            var first = bitMapCtor(5);
            var second = bitMapCtor(5);
            first.Set(0, true);
            first.Set(4, true);
            second.Set(0, true);
            second.Set(4, true);

            // act
            var result = first.And(second);

            // assert
            result[0].Should().Be(true);
            result[4].Should().Be(true);
        }


        [Theory]
        [MemberData(nameof(BitmapsProvider.TestBitMaps), MemberType = typeof(BitmapsProvider))]
        public void AndNotShouldReturnFalseIfBitsSame(Func<int, IBitmap> bitMapCtor)
        {
            // arrange
            var first = bitMapCtor(100);
            var second = bitMapCtor(100);
            first.Set(0, true);
            first.Set(50, true);
            second.Set(0, true);
            second.Set(50, true);

            // act
            var result = first.AndNot(second);

            // assert
            result[0].Should().Be(false);
            result[50].Should().Be(false);
        }


        [Theory]
        [MemberData(nameof(BitmapsProvider.TestBitMaps), MemberType = typeof(BitmapsProvider))]
        public void OrShouldReturnTrueForBitsInOtherPosition(Func<int, IBitmap> bitMapCtor)
        {
            // arrange
            var first = bitMapCtor(5);
            var second = bitMapCtor(5);
            first.Set(0, true);
            first.Set(5, true);
            second.Set(1, true);
            second.Set(2, true);

            // act
            var result = first.Or(second);

            // assert
            result[0].Should().Be(true);
            result[5].Should().Be(true);
            result[1].Should().Be(true);
            result[2].Should().Be(true);
        }
        
        [Theory]
        [MemberData(nameof(BitmapsProvider.TestBitMaps), MemberType = typeof(BitmapsProvider))]
        public void NotShouldReturnCorrectResults(Func<int, IBitmap> bitMapCtor)
        {
            // arrange
            var bitMap = bitMapCtor(5);
            bitMap.Set(0, true);
            bitMap.Set(4, true);

            // act
            var result = bitMap.Not();

            // assert
            result[0].Should().Be(false);
            result[4].Should().Be(false);
            result[1].Should().Be(true);
            result[2].Should().Be(true);
        }
        
        [Theory]
        [MemberData(nameof(BitmapsProvider.TestBitMaps), MemberType = typeof(BitmapsProvider))]
        public void AllShouldReturnCorrectResults(Func<int, IBitmap> bitMapCtor)
        {
            // arrange
            var bitMapFirst = bitMapCtor(5);
            var bitMapSecond = bitMapCtor(5);
            bitMapFirst.Set(0, true);
            bitMapFirst.Set(4, true);
            bitMapSecond.Set(0, true);
            bitMapSecond.Set(4, true);

            // act
            var result = bitMapFirst.All(bitMapSecond);

            // assert
            result.Should().Be(true);
        }
        
        [Theory]
        [MemberData(nameof(BitmapsProvider.TestBitMaps), MemberType = typeof(BitmapsProvider))]
        public void AnyShouldReturnCorrectResults(Func<int, IBitmap> bitMapCtor)
        {
            // arrange
            var bitMapFirst = bitMapCtor(5);
            var bitMapSecond = bitMapCtor(5);
            bitMapFirst.Set(0, true);
            bitMapFirst.Set(4, true);
            bitMapSecond.Set(0, true);
            bitMapSecond.Set(4, true);

            // act
            var result = bitMapFirst.Any(bitMapSecond);

            // assert
            result.Should().Be(true);
        }
        
        [Theory]
        [MemberData(nameof(BitmapsProvider.TestBitMaps), MemberType = typeof(BitmapsProvider))]
        public void XorShouldReturnCorrectResults(Func<int, IBitmap> bitMapCtor)
        {
            // arrange
            var bitMapFirst = bitMapCtor(5);
            var bitMapSecond = bitMapCtor(5);
            bitMapFirst.Set(4, true);
            bitMapSecond.Set(3, true);

            // act
            var result = bitMapFirst.Xor(bitMapSecond);

            // assert
            result[0].Should().Be(false);
            result[3].Should().Be(true);
            result[4].Should().Be(true);
        }
        
        [Theory]
        [MemberData(nameof(BitmapsProvider.TestBitMaps), MemberType = typeof(BitmapsProvider))]
        public void ContainsShouldReturnCorrectResults(Func<int, IBitmap> bitMapCtor)
        {
            // arrange
            var bitMap = bitMapCtor(5);
            bitMap.Set(4, true);

            // act
            var result = bitMap.Contains(4);

            // assert
            result.Should().Be(true);
        }
    }
}