using System.Collections.Generic;
using BitmapTests.Helpers;
using Crude.BitmapIndex.Implementations.Builder;
using FluentAssertions;
using Xunit;

namespace BitmapTests.BuilderTests
{
    public class BuilderTests
    {
        [Fact]
        public void ForPrimitiveDtoAlwaysReturnCorrectKeys()
        {
            // arrange
            var dto = TestModels.GetSimpleDto();
            var builder = new Builder<SimpleDto>();

            // act
            builder.IndexFor(dto, simpleDto => simpleDto.Integer);
            builder.IndexFor(dto, simpleDto => simpleDto.Str);
            builder.IndexFor(dto, simpleDto => simpleDto.Boolean);
            builder.IndexFor(dto, simpleDto => simpleDto.Float);

            var builtKeys = builder.Keys();

            // assert
            builtKeys.Should().BeEquivalentTo(new List<string>
                { "Integer.123", "Str.string", "Boolean.True", "Float.1,5" });
        }


        [Fact]
        public void ForDtoWithNestedDtoAlwaysReturnCorrectKeys()
        {
            // arrange
            var dto = TestModels.GetDtoWithNestedDto();
            var builder = new Builder<DtoWithNestedDto>();

            // act
            builder.IndexFor(dto, simpleDto => simpleDto.Integer);
            builder.IndexFor(dto, simpleDto => simpleDto.Double);
            builder.IndexForClass(dto, simpleDto => simpleDto.NestedDto, nestedDto => nestedDto.Boolean);

            var builtKeys = builder.Keys();

            // assert
            builtKeys.Should().BeEquivalentTo(new List<string>
                { "Integer.123", "Double.-2,5", "NestedDto.Boolean.False" });
        }


        [Fact]
        public void IndexForWithCustomKeyReturnCorrectKeys()
        {
            // arrange
            var dto = TestModels.GetSimpleDto();
            var builder = new Builder<DtoWithNestedDto>();

            // act
            builder.IndexFor("testKey", simpleDto => simpleDto.NestedDto.Boolean);

            var builtKeys = builder.Keys();

            // assert
            builtKeys.Should().BeEquivalentTo("testKey");
        }
    }
}