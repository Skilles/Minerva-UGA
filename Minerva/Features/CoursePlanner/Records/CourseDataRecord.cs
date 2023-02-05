namespace Minerva.Features.CoursePlanner.Records;

public record CourseDataRecord
(
    string FullName,
    IEnumerable<int> Sections
);