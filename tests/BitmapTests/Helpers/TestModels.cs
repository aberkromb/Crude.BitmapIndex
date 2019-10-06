namespace BitmapTests.Helpers
{
    public class TestModels
    {
        public static SimpleDto GetSimpleDto() => new SimpleDto(123, "string", true, 1.5F);

        public static DtoWithNestedDto GetDtoWithNestedDto() =>
            new DtoWithNestedDto(123, -2.5D, new SimpleDto(123, "str", false, 3.4567f));
    }

    public class DtoWithNestedDto
    {
        public DtoWithNestedDto(int integer, double d, SimpleDto nestedDto)
        {
            Integer = integer;
            Double = d;
            NestedDto = nestedDto;
        }

        public int Integer { get; }
        public double Double { get; }
        public SimpleDto NestedDto { get; }
    }

    public class SimpleDto
    {
        public SimpleDto(int integer, string str, bool boolean, float f)
        {
            Integer = integer;
            Str = str;
            Boolean = boolean;
            Float = f;
        }

        public int Integer { get; }
        public string Str { get; }
        public bool Boolean { get; }
        public float Float { get; }
    }
}