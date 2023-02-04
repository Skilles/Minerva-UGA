using Minerva.Features.Athena.Enums;

namespace Minerva.Features.Athena.Extensions;

public static class FlagEnumExtensions
{
    public static T ToEnum<T>(this int value) where T : Enum => (T)Enum.ToObject(typeof(T), value);
    
    public static T ToEnum<T>(this IEnumerable<int> values) where T : Enum
    {
        var result = values.Aggregate(0, (current, value) => current | value);
        return (T)Enum.ToObject(typeof(T), result);
    }
    
    public static T ToEnum<T>(this bool[] flags) where T : Enum
    {
        var result = 0;
        for (int i = 0; i < flags.Length; i++)
        {
            if (flags[i])
            {
                result |= 1 << i;
            }
        }
        return (T)Enum.ToObject(typeof(T), result);
    }
    
    // Converts a string value to an enum for example "W" converts to Wednesday and "Th" converts to Thursday
    public static CourseDateFlags ToCourseDate(this string value) =>
        value switch {
            "M" => CourseDateFlags.Monday,
            "T" => CourseDateFlags.Tuesday,
            "W" => CourseDateFlags.Wednesday,
            "R" => CourseDateFlags.Thursday,
            "F" => CourseDateFlags.Friday,
            "S" => CourseDateFlags.Saturday,
            _   => CourseDateFlags.None
        };
}