namespace Minerva.Features.Athena.Enums;

[Flags]
public enum CourseDateFlags
{
    None = 0,
    Saturday = 1,
    Monday = 2,
    Tuesday = 4,
    Wednesday = 8,
    Thursday = 16,
    Friday = 32
}