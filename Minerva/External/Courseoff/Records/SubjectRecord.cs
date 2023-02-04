namespace Minerva.External.Courseoff.Records;

public record SubjectRecord
(
    string Ident,
    string Name
) : ICourseoffResponse;