using Minerva.Features.Athena.Documents;

namespace Minerva.Features.CoursePlanner.Records;

public record PlannerDataRecord
(
    string PlannerName,
    IDictionary<string, CourseDataRecord> Courses,
    IDictionary<int, SectionDocument> Sections
);