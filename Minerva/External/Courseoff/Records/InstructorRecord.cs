using System.Text.Json.Serialization;

namespace Minerva.External.Courseoff.Records;

public record InstructorRecord
(
    [property: JsonPropertyName("fname")]
    string FirstName,
    [property: JsonPropertyName("lname")]
    string LastName
);