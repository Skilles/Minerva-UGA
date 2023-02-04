using Minerva.Features.Athena.Documents;

namespace Minerva.Features.CoursePlanner.Records;

public record CourseSectionDataRecord
(
    SectionDocument Section,
    SectionCapacityRecord Capacity
);