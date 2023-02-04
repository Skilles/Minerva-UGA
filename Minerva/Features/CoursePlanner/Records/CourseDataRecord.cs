using Minerva.Features.Athena.Documents;

namespace Minerva.Features.CoursePlanner.Records;

public record CourseDataRecord
(
    string FullName,
    IEnumerable<SectionDocument> Sections
);