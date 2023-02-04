using System.Text.Json.Serialization;

namespace Minerva.External.Courseoff.Records;

public record TimeslotRecord
(
    string Location, 
    [property: JsonPropertyName("start_time")]
    int StartTime, 
    [property: JsonPropertyName("end_time")]
    int EndTime,
    string Day
);