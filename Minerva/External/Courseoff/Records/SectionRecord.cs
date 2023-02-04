namespace Minerva.External.Courseoff.Records;

public record SectionRecord
(
    string Ident,
    float Credits, 
    TimeslotRecord[]? Timeslots,
    InstructorRecord? Instructor
) : ICourseoffResponse;