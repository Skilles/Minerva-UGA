using Minerva.Features.Athena.Enums;

namespace Minerva.Features.Athena.Records;

public record MeetingRecord
(
    int BuildingId, 
    string BuildingName,
    string Room,
    int StartTime,
    int EndTime,
    CourseDateFlags Days
);