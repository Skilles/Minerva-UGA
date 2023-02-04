namespace Minerva.Features.CoursePlanner.Records;

public record PlannerDataRecord
(
    string PlannerName,
    List<CourseDataRecord> Courses
);