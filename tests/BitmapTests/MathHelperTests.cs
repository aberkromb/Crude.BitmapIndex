using System;
using System.Linq;
using Crude.BitmapIndex.Helpers;
using FluentAssertions;
using Xunit;

namespace BitmapTests
{
    public class MathHelperTests
    {
        [Fact]
        public void Log2ShouldReturnSameValueMathLog2()
        {
            // arrange
            const int count = 1000;
            var data = Enumerable.Range(1, count).Select(i => new Random().Next(0, int.MaxValue)).ToList();

            // act
            var log2Native = data.Select(x => (int) Math.Log(x, 2)).ToList();
            var log2Custom = data.Select(x => MathHelper.Log2((ulong)x)).ToList();

            // assert
            log2Native.Should().BeEquivalentTo(log2Custom);
        }
        
        [Fact]
        public void Log2SpanShouldReturnSameValueMathLog2()
        {
            // arrange
            const int count = 1000;
            var data = Enumerable.Range(1, count).Select(i => new Random().Next(0, int.MaxValue)).ToList();

            // act
            var log2Native = data.Select(x => (int) Math.Log(x, 2)).ToList();
            var log2Span = data.Select(x => MathHelper.Log2Span((ulong)x)).ToList();

            // assert
            log2Native.Should().BeEquivalentTo(log2Span);
        }
    }
}