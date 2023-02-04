using Minerva.External.Courseoff.Records;
using Minerva.Features.Athena.Enums;
using Minerva.Features.Athena.Records;

namespace Minerva.Features.Athena.Extensions;

public static class CourseoffExtensions
{
    public static IEnumerable<MeetingRecord> ToMeetings(this IEnumerable<TimeslotRecord> timeslots)
    {
        var meetings = timeslots
            .Select(t =>
            {
                var (location, room) = SplitRoomAndLocation(t.Location);
                return new MeetingRecord
                (
                    BuildingId: 0,
                    BuildingName: location,
                    Room: room,
                    StartTime: t.StartTime,
                    EndTime: t.EndTime,
                    Days: t.Day.ToCourseDate()
                );
            });
        
        // Group meetings by room and time. For each group, create a new meeting with the days combined
        return meetings
               .GroupBy(m => new { m.Room, m.StartTime })
               .Select(g => new MeetingRecord
                       (
                           BuildingId: 0, // TODO update this lazily with a lookup
                           BuildingName: g.First().BuildingName,
                           Room: g.Key.Room,
                           StartTime: g.Key.StartTime,
                           EndTime: g.First().EndTime, 
                           Days: g.Aggregate(CourseDateFlags.None, (acc, m) => acc | m.Days)
                       ));
    }

    private static (string, string) SplitRoomAndLocation(string location)
    {
        var span = location.AsSpan();
        var lastSpace = span.LastIndexOf(' ');
        if (lastSpace == -1)
        {
            return (location, location);
        }
        // The room is the last word in the location string
        var room = span[(lastSpace + 1)..].ToString();
        var locationSpan = span[..lastSpace];
        return (locationSpan.ToString(), room);
    }
}