using System.Text.Json.Serialization;

namespace Minerva.External.Courseoff.Records;

public record TermRecord
(
    int Ident,
    string Semester,
    [property: JsonPropertyName("start_date")]
    long StartDate,
    [property: JsonPropertyName("end_date")]
    long EndDate,
    [property: JsonPropertyName("scraped_at")]
    DateTime ScrapedAt
) : ICourseoffResponse;