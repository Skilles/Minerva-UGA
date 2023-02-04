namespace Minerva.External.Courseoff.Records;

public record CourseRecord
(
    string Ident,
    string Name
) : ICourseoffResponse;