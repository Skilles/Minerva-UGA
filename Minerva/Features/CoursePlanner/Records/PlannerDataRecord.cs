namespace Minerva.Features.CoursePlanner.Records;

public record PlannerDataRecord
(
    string PlannerName,
    IEnumerable<CourseDataRecord> Courses
);