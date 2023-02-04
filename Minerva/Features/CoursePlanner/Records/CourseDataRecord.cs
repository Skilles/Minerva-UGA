namespace Minerva.Features.CoursePlanner.Records;

public record CourseDataRecord
(
    string FullName,
    List<CourseSectionDataRecord> Sections
);